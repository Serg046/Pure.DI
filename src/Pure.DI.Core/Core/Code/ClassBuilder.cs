// ReSharper disable ClassNeverInstantiated.Global
namespace Pure.DI.Core.Code;

internal sealed class ClassBuilder : IBuilder<CompositionCode, CompositionCode>
{
    
    private readonly IBuilder<CompositionCode, CompositionCode> _usingDeclarationsBuilder;
    private readonly IInformation _information;
    private readonly CancellationToken _cancellationToken;
    private readonly ImmutableArray<IBuilder<CompositionCode, CompositionCode>> _codeBuilders;
    
    public ClassBuilder(
        [Tag(WellknownTag.UsingDeclarationsBuilder)] IBuilder<CompositionCode, CompositionCode> usingDeclarationsBuilder,
        [Tag(WellknownTag.SingletonFieldsBuilder)] IBuilder<CompositionCode, CompositionCode> singletonFieldsBuilder,
        [Tag(WellknownTag.ArgFieldsBuilder)] IBuilder<CompositionCode, CompositionCode> argFieldsBuilder,
        [Tag(WellknownTag.PrimaryConstructorBuilder)] IBuilder<CompositionCode, CompositionCode> primaryConstructorBuilder,
        [Tag(WellknownTag.DefaultConstructorBuilder)] IBuilder<CompositionCode, CompositionCode> defaultConstructorBuilder,
        [Tag(WellknownTag.ChildConstructorBuilder)] IBuilder<CompositionCode, CompositionCode> childConstructorBuilder,
        [Tag(WellknownTag.RootPropertiesBuilder)] IBuilder<CompositionCode, CompositionCode> rootPropertiesBuilder,
        [Tag(WellknownTag.ApiMembersBuilder)] IBuilder<CompositionCode, CompositionCode> apiMembersBuilder,
        [Tag(WellknownTag.DisposeMethodBuilder)] IBuilder<CompositionCode, CompositionCode> disposeMethodBuilder,
        [Tag(WellknownTag.ToStringMethodBuilder)] IBuilder<CompositionCode, CompositionCode> toStringBuilder,
        [Tag(WellknownTag.ResolversFieldsBuilder)] IBuilder<CompositionCode, CompositionCode> resolversFieldsBuilder,
        [Tag(WellknownTag.StaticConstructorBuilder)] IBuilder<CompositionCode, CompositionCode> staticConstructorBuilder,
        [Tag(WellknownTag.ResolverClassesBuilder)] IBuilder<CompositionCode, CompositionCode> resolversClassesBuilder,
        IInformation information,
        CancellationToken cancellationToken)
    {
        _usingDeclarationsBuilder = usingDeclarationsBuilder;
        _information = information;
        _cancellationToken = cancellationToken;
        _codeBuilders = ImmutableArray.Create(
            singletonFieldsBuilder,
            argFieldsBuilder,
            primaryConstructorBuilder,
            defaultConstructorBuilder,
            childConstructorBuilder,
            rootPropertiesBuilder,
            apiMembersBuilder,
            disposeMethodBuilder,
            toStringBuilder,
            resolversFieldsBuilder,
            staticConstructorBuilder,
            resolversClassesBuilder);
    }

    public CompositionCode Build(CompositionCode composition)
    {
        var code = composition.Code;
        code.AppendLine("// <auto-generated/>");
        code.AppendLine($"// by {_information.Description}");
        code.AppendLine("#nullable enable");
        code.AppendLine("#pragma warning disable");
        code.AppendLine();

        composition = _usingDeclarationsBuilder.Build(composition);
        
        var nsIndent = Disposables.Empty;
        if (!string.IsNullOrWhiteSpace(composition.Source.Source.Name.Namespace))
        {
            code.AppendLine($"namespace {composition.Source.Source.Name.Namespace}");
            code.AppendLine("{");
            nsIndent = code.Indent();
        }

        code.AppendLine($"[{Names.SystemNamespace}Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]");
        var implementingInterfaces = composition.DisposableSingletonsCount > 0 ? $": {Names.IDisposableInterfaceName}" : "";
        code.AppendLine($"partial class {composition.Source.Source.Name.ClassName}{implementingInterfaces}");
        code.AppendLine("{");

        using (code.Indent())
        {
            // Generate class members
            foreach (var builder in _codeBuilders)
            {
                _cancellationToken.ThrowIfCancellationRequested();
                composition = builder.Build(composition);
            }
        }

        code.AppendLine("}");

        // ReSharper disable once InvertIf
        if (!string.IsNullOrWhiteSpace(composition.Source.Source.Name.Namespace))
        {
            // ReSharper disable once RedundantAssignment
            nsIndent.Dispose();
            code.AppendLine("}");
        }
        
        code.AppendLine("#pragma warning restore");
        return composition;
    }
}