namespace Pure.DI.Core;

internal class DependencyGraphBuilder : IDependencyGraphBuilder
{
    private readonly IBuilder<MdSetup, IEnumerable<DependencyNode>>[] _dependencyNodeBuilders;
    private readonly IBuilder<MdBinding, ISet<Injection>> _injectionsBuilder;
    private readonly IMarker _marker;
    private readonly IUnboundTypeConstructor _unboundTypeConstructor;
    private readonly Func<ITypeConstructor> _typeConstructorFactory;
    private readonly Func<IBuilder<RewriterContext<MdFactory>, MdFactory>> _factoryRewriterFactory;

    public DependencyGraphBuilder(
        IBuilder<MdSetup, IEnumerable<DependencyNode>>[] dependencyNodeBuilders,
        IBuilder<MdBinding, ISet<Injection>> injectionsBuilder,
        IMarker marker,
        IUnboundTypeConstructor unboundTypeConstructor,
        Func<ITypeConstructor> typeConstructorFactory,
        Func<IBuilder<RewriterContext<MdFactory>, MdFactory>> factoryRewriterFactory)
    {
        _dependencyNodeBuilders = dependencyNodeBuilders;
        _injectionsBuilder = injectionsBuilder;
        _marker = marker;
        _unboundTypeConstructor = unboundTypeConstructor;
        _typeConstructorFactory = typeConstructorFactory;
        _factoryRewriterFactory = factoryRewriterFactory;
    }

    public IEnumerable<DependencyNode> TryBuild(
        MdSetup setup,
        IReadOnlyCollection<ProcessingNode> nodes,
        out DependencyGraph? dependencyGraph,
        CancellationToken cancellationToken)
    {
        dependencyGraph = default;
        var maxId = 0;
        var mapBuilder = ImmutableDictionary.CreateBuilder<Injection, DependencyNode>();
        var queue = new Queue<ProcessingNode>();
        foreach (var processingNode in nodes)
        {
            var node = processingNode.Node;
            if (node.Binding.Id > maxId)
            {
                maxId = node.Binding.Id;
            }

            if (node.Root is not { })
            {
                foreach (var exposedInjection in processingNode.ExposedInjections)
                {
                    mapBuilder[exposedInjection] = node;
                }
            }

            if (!processingNode.IsMarkerBased)
            {
                queue.Enqueue(processingNode);
            }
        }

        var map = mapBuilder.ToImmutableDictionary();
        var isValid = true;
        var processed = new List<ProcessingNode>();
        var notProcessed = new List<ProcessingNode>();
        while (queue.TryDequeue(out var node))
        {
            var targetNode = node.Node;
            foreach (var injection in node.Injections)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (map.TryGetValue(injection, out var sourceNode))
                {
                    continue;
                }

                switch (injection.Type)
                {
                    case INamedTypeSymbol { IsGenericType: true } geneticType:
                    {
                        // Generic
                        var unboundType = _unboundTypeConstructor.Construct(targetNode.Binding.SemanticModel.Compilation, injection.Type);
                        var unboundInjection = injection with { Type = unboundType };
                        if (map.TryGetValue(unboundInjection, out sourceNode))
                        {
                            var newBinding = CreateGenericBinding(targetNode, injection, sourceNode, ++maxId, cancellationToken);
                            var newNode = CreateNodes(setup, newBinding, cancellationToken)
                                .Single(i => i.Variation == sourceNode.Variation);
                            map = map.Add(injection, newNode);
                            queue.Enqueue(CreateNewProcessingNode(newNode));
                            continue;
                        }

                        // Construct
                        if (geneticType.TypeArguments is [{ } enumerableType])
                        {
                            var constructKind = geneticType.ConstructUnboundGenericType().ToString() switch
                            {
                                "System.Span<>" => MdConstructKind.Span,
                                "System.ReadOnlySpan<>" => MdConstructKind.Span,
                                "System.Collections.Generic.IEnumerable<>" => MdConstructKind.Enumerable,
                                _ => default(MdConstructKind?)
                            };

                            if (constructKind.HasValue)
                            {
                                var enumerableBinding = CreateConstructBinding(setup, targetNode, injection, enumerableType, ++maxId, constructKind.Value);
                                return CreateNodes(setup, enumerableBinding, cancellationToken);
                            }
                        }

                        break;
                    }

                    // Array construct
                    case IArrayTypeSymbol arrayType:
                    {
                        var arrayBinding = CreateConstructBinding(setup, targetNode, injection, arrayType.ElementType, ++maxId, MdConstructKind.Array);
                        return CreateNodes(setup, arrayBinding, cancellationToken);
                    }
                }

                // Auto-binding
                if (injection.Type is { IsAbstract: false, SpecialType: SpecialType.None })
                {
                    var autoBinding = CreateAutoBinding(setup, targetNode, injection, ++maxId);
                    return CreateNodes(setup, autoBinding, cancellationToken);
                }

                // Not processed
                notProcessed.Add(node);
                isValid = false;
                break;
            }

            if (!isValid)
            {
                break;
            }
            
            processed.Add(node);
        }

        var entriesBuilder = ImmutableArray.CreateBuilder<GraphEntry<DependencyNode, Dependency>>();
        var unresolvedSource = new DependencyNode(0);
        foreach (var node in processed)
        {
            var edges = ImmutableArray.CreateBuilder<Dependency>(node.Injections.Length);
            foreach (var injection in node.Injections)
            {
                var dependency = map.TryGetValue(injection, out var sourceNode)
                    ? new Dependency(true, sourceNode, injection, node.Node)
                    : new Dependency(false, unresolvedSource, injection, node.Node);

                edges.Add(dependency);
            }

            entriesBuilder.Add(new GraphEntry<DependencyNode, Dependency>(node.Node, edges.MoveToImmutable()));
        }

        if (notProcessed.Any())
        {
            foreach (var node in notProcessed)
            {
                var edges = ImmutableArray.CreateBuilder<Dependency>(node.Injections.Length);
                foreach (var injection in node.Injections)
                {
                    edges.Add(new Dependency(false, unresolvedSource, injection, node.Node));
                }

                entriesBuilder.Add(new GraphEntry<DependencyNode, Dependency>(node.Node, edges.MoveToImmutable()));
            }
        }

        dependencyGraph = new DependencyGraph(
            isValid,
            setup,
            new Graph<DependencyNode, Dependency>(entriesBuilder.ToImmutable()),
            map,
            ImmutableDictionary<Injection, Root>.Empty);
        
        return ImmutableArray<DependencyNode>.Empty;
    }

    private MdBinding CreateGenericBinding(
        DependencyNode targetNode,
        Injection injection,
        DependencyNode sourceNode,
        int newId,
        CancellationToken cancellationToken)
    {
        var semanticModel = targetNode.Binding.SemanticModel;
        var compilation = semanticModel.Compilation;
        var typeConstructor = _typeConstructorFactory();
        typeConstructor.Bind(sourceNode.Type, injection.Type);
        var newContracts = sourceNode.Binding.Contracts.Select(contract => contract with { ContractType = typeConstructor.Construct(semanticModel.Compilation, contract.ContractType) }).ToImmutableArray();
        var newBinding = sourceNode.Binding with
        {
            Id = newId,
            Contracts = newContracts,
            Implementation = sourceNode.Binding.Implementation.HasValue
                ? sourceNode.Binding.Implementation.Value with
                {
                    Type = typeConstructor.Construct(compilation, sourceNode.Binding.Implementation.Value.Type)
                }
                : default(MdImplementation?),
            Factory = sourceNode.Binding.Factory.HasValue
                ? _factoryRewriterFactory().Build(
                    new RewriterContext<MdFactory>(typeConstructor, sourceNode.Binding.Factory.Value),
                    cancellationToken)
                : default(MdFactory?),
            Arg = sourceNode.Binding.Arg.HasValue
                ? sourceNode.Binding.Arg.Value with
                {
                    Type = typeConstructor.Construct(compilation, sourceNode.Binding.Arg.Value.Type)
                }
                : default(MdArg?)
        };
        return newBinding;
    }

    private MdBinding CreateAutoBinding(
        MdSetup setup,
        DependencyNode targetNode,
        Injection injection,
        int newId)
    {
        var semanticModel = targetNode.Binding.SemanticModel;
        var compilation = semanticModel.Compilation;
        var sourceType = injection.Type;
        var typeConstructor = _typeConstructorFactory();
        if (_marker.IsMarkerBased(injection.Type))
        {
            typeConstructor.Bind(injection.Type, injection.Type);
            sourceType = typeConstructor.Construct(compilation, injection.Type);
        }

        var newContracts = ImmutableArray.Create(new MdContract(semanticModel, setup.Source, sourceType, ImmutableArray<MdTag>.Empty));
        var newBinding = new MdBinding(
            newId,
            targetNode.Binding.Source,
            semanticModel,
            newContracts,
            ImmutableArray<MdTag>.Empty,
            new MdLifetime(semanticModel, setup.Source, Lifetime.Transient),
            new MdImplementation(semanticModel, setup.Source, sourceType));
        return newBinding;
    }

    private static MdBinding CreateConstructBinding(
        MdSetup setup,
        DependencyNode targetNode,
        Injection injection,
        ITypeSymbol elementType,
        int newId,
        MdConstructKind constructKind)
    {
        var dependencyContractsBuilder = ImmutableArray.CreateBuilder<MdContract>();
        foreach (var nestedBinding in setup.Bindings.Where(i => i != targetNode.Binding))
        {
            var matchedContracts = nestedBinding.Contracts.Where(i => SymbolEqualityComparer.Default.Equals(i.ContractType, elementType)).ToImmutableArray();
            if (!matchedContracts.Any())
            {
                continue;
            }

            var tag = matchedContracts.First().Tags.Concat(nestedBinding.Tags).Select(i => i.Value).FirstOrDefault();
            var tags = tag is { }
                ? ImmutableArray.Create(new MdTag(targetNode.Binding.SemanticModel, targetNode.Binding.Source, 0, tag))
                : ImmutableArray<MdTag>.Empty;
            dependencyContractsBuilder.Add(new MdContract(targetNode.Binding.SemanticModel, targetNode.Binding.Source, elementType, tags));
        }

        var newContracts = ImmutableArray.Create(new MdContract(targetNode.Binding.SemanticModel, targetNode.Binding.Source, injection.Type, ImmutableArray<MdTag>.Empty));
        return new MdBinding(newId, targetNode.Binding.Source, targetNode.Binding.SemanticModel, newContracts, ImmutableArray<MdTag>.Empty)
        {
            Id = newId,
            SemanticModel = targetNode.Binding.SemanticModel,
            Source = targetNode.Binding.Source,
            Construct = new MdConstruct(
                targetNode.Binding.SemanticModel,
                targetNode.Binding.Source,
                injection.Type,
                elementType,
                constructKind,
                dependencyContractsBuilder.ToImmutable())
        };
    }
    
    private ProcessingNode CreateNewProcessingNode(DependencyNode dependencyNode)
    {
        var exposedInjections = _injectionsBuilder.Build(dependencyNode.Binding, CancellationToken.None);
        return new ProcessingNode(dependencyNode, exposedInjections, _marker);
    }

    private IEnumerable<DependencyNode> CreateNodes(
        MdSetup setup,
        MdBinding binding,
        CancellationToken cancellationToken)
    {
        var newSetup = setup with { Roots = ImmutableArray<MdRoot>.Empty, Bindings = ImmutableArray.Create(binding) };
        return _dependencyNodeBuilders.SelectMany(builder => builder.Build(newSetup, cancellationToken));
    }
}