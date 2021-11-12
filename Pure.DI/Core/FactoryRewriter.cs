﻿// ReSharper disable IdentifierTypo
// ReSharper disable InvertIf
// ReSharper disable MergeIntoPattern
// ReSharper disable ClassNeverInstantiated.Global
namespace Pure.DI.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal class FactoryRewriter: CSharpSyntaxRewriter
    {
        private readonly IBuildContext _buildContext;
        private readonly ICannotResolveExceptionFactory _cannotResolveExceptionFactory;
        private readonly ICache<FactoryKey, SyntaxNode?> _nodeCache;
        private readonly ICache<SyntaxNode, ITypeSymbol?> _typeCache;
        private Dependency _dependency;
        private IBuildStrategy? _buildStrategy;
        private SyntaxToken _contextIdentifier;
        private ExpressionSyntax? _defaultTag;

        public FactoryRewriter(
            IBuildContext buildContext,
            ICannotResolveExceptionFactory cannotResolveExceptionFactory,
            [Tag(Tags.ContainerScope)] ICache<FactoryKey, SyntaxNode?> nodeCache,
            [Tag(Tags.ContainerScope)] ICache<SyntaxNode, ITypeSymbol?> typeCache)
            : base(true)
        {
            _buildContext = buildContext;
            _cannotResolveExceptionFactory = cannotResolveExceptionFactory;
            _nodeCache = nodeCache;
            _typeCache = typeCache;
        }

        public FactoryRewriter Initialize(
            Dependency dependency,
            IBuildStrategy buildStrategy,
            SyntaxToken contextIdentifier
        )
        {
            _dependency = dependency;
            _buildStrategy = buildStrategy;
            _contextIdentifier = contextIdentifier;
            if (dependency.Implementation.Type is INamedTypeSymbol namedTypeSymbol)
            {
                if(namedTypeSymbol.IsGenericType && namedTypeSymbol.ConstructUnboundGenericType().ToString() == "System.Func<>")
                {
                    _defaultTag = dependency.Tag;
                }
            }
            return this;
        }

        public override SyntaxNode VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            var args = node.Arguments.ToArray();
            ReplaceTypes(args);
            return SyntaxFactory.TypeArgumentList().AddArguments(args);
        }

        public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (
                node.Kind() == SyntaxKind.SimpleMemberAccessExpression
                && node.Expression is IdentifierNameSyntax identifierName
                && identifierName.ToString() == _contextIdentifier.Text)
            {
                var method = node.ChildNodes().OfType<GenericNameSyntax>().FirstOrDefault();
                if (method != null)
                {
                    var args = method.TypeArgumentList.Arguments.ToArray();
                    ReplaceTypes(args);
                    if (_dependency.Binding.AnyTag && _dependency.Tag != null)
                    {
                        var expression = VisitGenericName(method);
                        if (expression is GenericNameSyntax genericName)
                        {
                            return SyntaxFactory.ParenthesizedLambdaExpression(
                                SyntaxFactory.InvocationExpression(genericName)
                                    .AddArgumentListArguments(SyntaxFactory.Argument(_dependency.Tag)));
                        }

                        return expression;
                    }
                }

                return Visit(method);
            }

            return base.VisitMemberAccessExpression(node);
        }

        public override SyntaxNode VisitTypeOfExpression(TypeOfExpressionSyntax node) => 
            SyntaxFactory.TypeOfExpression(ReplaceType(node.Type));

        public override SyntaxNode? VisitVariableDeclaration(VariableDeclarationSyntax node) =>
            base.VisitVariableDeclaration(node.WithType(ReplaceType(node.Type)));

        public override SyntaxNode VisitGenericName(GenericNameSyntax node)
        {
            if (node.IsUnboundGenericName)
            {
                return node;
            }

            if (node.Identifier.Text == nameof(IContext.Resolve) && node.Parent?.Parent is InvocationExpressionSyntax invocation && IsResolveMethod(invocation))
            {
                var semanticModel = node.GetSemanticModel(_dependency.Implementation);
                var argType = node.TypeArgumentList.Arguments[0];
                var type = _typeCache.GetOrAdd(argType, i => semanticModel.GetTypeInfo(i).Type); 
                if (type != default)
                {
                    var tag = invocation.ArgumentList.Arguments.Count == 1 ? invocation.ArgumentList.Arguments[0].Expression : _defaultTag;
                    var dependencyType = _dependency.TypesMap.ConstructType(new SemanticType(type, semanticModel));
                    var dependency = _buildContext.TypeResolver.Resolve(dependencyType, tag);
                    try
                    {
                        return ReplaceLambdaByCreateExpression(dependency, dependencyType);
                    }
                    catch (BuildException ex)
                    {
                        if (ex.Id == Diagnostics.Error.CircularDependency)
                        {
                            return ReplaceLambdaByResolveCall(dependencyType, tag);
                        }

                        throw;
                    }
                }
            }
            
            var args = node.TypeArgumentList.Arguments.ToArray();
            ReplaceTypes(args);
            return node.WithTypeArgumentList(SyntaxFactory.TypeArgumentList().AddArguments(args.ToArray()));
        }

        public override SyntaxNode? VisitInvocationExpression(InvocationExpressionSyntax node) => 
            IsResolveMethod(node) ? 
                _nodeCache.GetOrAdd(new FactoryKey(_dependency, node), _ => VisitInvocationExpressionInternal(node))
                : base.VisitInvocationExpression(node);

        private SyntaxNode? VisitInvocationExpressionInternal(InvocationExpressionSyntax node)
        {
            if (IsResolveMethod(node))
            {
                var expression = base.VisitInvocationExpression(node);
                if (expression is InvocationExpressionSyntax invocation)
                {
                    return invocation.Expression;
                }
            }

            return base.VisitInvocationExpression(node);
        }

        private static bool IsResolveMethod(InvocationExpressionSyntax invocation) =>
            invocation.ArgumentList.Arguments.Count <= 1
            && invocation.Expression is MemberAccessExpressionSyntax memberAccess
            && memberAccess.Kind() == SyntaxKind.SimpleMemberAccessExpression
            && memberAccess.Name is GenericNameSyntax genericName
            && genericName.Identifier.Text == nameof(IContext.Resolve)
            && genericName.TypeArgumentList.Arguments.Count == 1;

        private static SyntaxNode ReplaceLambdaByResolveCall(SemanticType dependencyType, ExpressionSyntax? tag)
        {
            var result = SyntaxFactory.InvocationExpression(
                SyntaxFactory.GenericName(nameof(IContext.Resolve))
                    .AddTypeArgumentListArguments(dependencyType));

            if (tag != default)
            {
                result = result.AddArgumentListArguments(SyntaxFactory.Argument(tag));
            }

            return result;
        }

        private SyntaxNode ReplaceLambdaByCreateExpression(Dependency dependency, SemanticType dependencyType)
        {
            var expression = _buildStrategy!.TryBuild(dependency, dependencyType);
            if (expression == null)
            {
                throw _cannotResolveExceptionFactory.Create(dependency.Binding, dependency.Tag, "a factory");
            }

            return expression;
        }

        private void ReplaceTypes(IList<TypeSyntax> args)
        {
            for (var i = 0; i < args.Count; i++)
            {
                args[i] = ReplaceType(args[i]);
            }
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private TypeSyntax ReplaceType(TypeSyntax typeSyntax) => 
            (TypeSyntax)_nodeCache.GetOrAdd(new FactoryKey(_dependency, typeSyntax), _ => ReplaceTypeInternal(typeSyntax))!;

        private TypeSyntax ReplaceTypeInternal(TypeSyntax typeSyntax)
        {
            var semanticModel = typeSyntax.SyntaxTree.GetRoot().GetSemanticModel(_dependency.Implementation);
            var typeSymbol = _typeCache.GetOrAdd(typeSyntax, _ => semanticModel.GetTypeInfo(typeSyntax).Type);
            SemanticType? sematicType;
            switch (typeSymbol)
            {
                case INamedTypeSymbol namedTypeSymbol:
                {
                    var curType = new SemanticType(namedTypeSymbol, _dependency.Implementation);
                    sematicType= _dependency.TypesMap.ConstructType(curType);
                    return sematicType.TypeSyntax;
                }
                
                case IArrayTypeSymbol arrayTypeSymbol:
                {
                    var curType = new SemanticType(arrayTypeSymbol.ElementType, _dependency.Implementation);
                    sematicType = _dependency.TypesMap.ConstructType(curType);
                    return SyntaxFactory.ArrayType(sematicType).AddRankSpecifiers(SyntaxFactory.ArrayRankSpecifier());
                }
                
                default:
                    return typeSyntax;
            }
        }

        internal class FactoryKey
        {
            private readonly Dependency _dependency;
            private readonly SyntaxNode _node;

            public FactoryKey(Dependency dependency, SyntaxNode node)
            {
                _dependency = dependency;
                _node = node;
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                FactoryKey other = (FactoryKey)obj;
                return _dependency.Equals(other._dependency) && _node.Equals(other._node);
            }
            
            public override int GetHashCode()
            {
                unchecked
                {
                    return (_dependency.GetHashCode() * 397) ^ _node.GetHashCode();
                }
            }
        }
    }
}