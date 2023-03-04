namespace Pure.DI.Core;

internal class CodeGraphWalker<TContext>
{
    private readonly IVarIdGenerator _idGenerator;

    public CodeGraphWalker(IVarIdGenerator idGenerator) => _idGenerator = idGenerator;

    public virtual void VisitGraph(
        TContext context,
        DependencyGraph dependencyGraph, 
        IDictionary<MdBinding, Variable> variables,
        CancellationToken cancellationToken)
    {
        foreach (var root in dependencyGraph.Roots.Values)
        {
            cancellationToken.ThrowIfCancellationRequested();
            VisitRoot(context, dependencyGraph, variables, root, cancellationToken);
            var keysToRemove = variables
                .Where(i => i.Value.Node.Lifetime != Lifetime.Singleton && i.Value.Node.Arg is not {})
                .Select(i => i.Key)
                .ToImmutableArray();

            foreach (var binding in keysToRemove)
            {
                variables.Remove(binding);
            }
        }
    }

    public virtual void VisitRoot(
        TContext context,
        DependencyGraph dependencyGraph,
        IDictionary<MdBinding, Variable> variables,
        Root root,
        CancellationToken cancellationToken)
    {
        foreach (var variable in variables.Values)
        {
            variable.IsCreated = false;
        }

        var rootVariable = CreateVariable(variables, root.Node);
        VisitRootVariable(context, dependencyGraph, variables, rootVariable, cancellationToken);
    }
    
    public virtual void VisitRootVariable(
        TContext context,
        DependencyGraph dependencyGraph,
        IDictionary<MdBinding, Variable> variables,
        Variable rootVariable,
        CancellationToken cancellationToken)
    {
        var targets = new Stack<Variable>();
        var blocks = new Stack<Block>();
        var blockRootVariables = new Stack<Variable>();
        blockRootVariables.Push(rootVariable);
        while (blockRootVariables.TryPop(out var target))
        {
            cancellationToken.ThrowIfCancellationRequested();
            targets.Push(target);
            var resolves = new Stack<Instantiation>();
            while (targets.TryPop(out var targetVariable))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var arguments = new List<Variable>();
                if (dependencyGraph.Graph.TryGetEdges(targetVariable.Node, out var dependencies))
                {
                    foreach (var dependency in dependencies)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var var = CreateVariable(variables, dependency.Source);
                        if (var.IsBlockRoot)
                        {
                            blockRootVariables.Push(var);
                        }
                        else
                        {
                            if (dependency.Injection.Resolver is not { } || var.Node.Lifetime == Lifetime.PerResolve)
                            {
                                targets.Push(var);
                            }
                        }
                        
                        arguments.Add(var);
                    }
                }

                resolves.Push(new Instantiation(targetVariable, arguments.ToImmutableArray()));
            }

            blocks.Push(new Block(target, resolves.ToImmutableArray()));
        }

        while (blocks.TryPop(out var block))
        {
            VisitBlock(context, dependencyGraph, rootVariable, block, cancellationToken);
        }
    }

    public virtual void VisitBlock(TContext context,
        DependencyGraph dependencyGraph,
        Variable root,
        Block block,
        CancellationToken cancellationToken)
    {
        foreach (var resolve in block.Resolves)
        {
            cancellationToken.ThrowIfCancellationRequested();
            VisitInstantiation(context, dependencyGraph, root, block, resolve, cancellationToken);
        }
    }

    public virtual void VisitInstantiation(
        TContext context,
        DependencyGraph dependencyGraph,
        Variable rootVariable,
        Block block,
        Instantiation instantiation,
        CancellationToken cancellationToken)
    {
        if (instantiation.Target.IsCreated)
        {
            return;
        }

        if (instantiation.Target.Node.Implementation is { } implementation)
        {
            VisitImplementation(context, dependencyGraph, rootVariable, block, instantiation, implementation, cancellationToken);
        }
        else
        {
            if (instantiation.Target.Node.Factory is { } factory)
            {
                VisitFactory(context, dependencyGraph, rootVariable, block, instantiation, factory, cancellationToken);
            }
            else
            {
                if (instantiation.Target.Node.Arg is { } arg)
                {
                    VisitArg(context, dependencyGraph, rootVariable, block, instantiation, arg, cancellationToken);
                }   
            }
        }
    }

    public virtual void VisitImplementation(
        TContext context,
        DependencyGraph dependencyGraph,
        Variable root,
        Block block,
        Instantiation instantiation,
        DpImplementation implementation,
        CancellationToken cancellationToken)
    {
    }

    public virtual void VisitFactory(
        TContext context,
        DependencyGraph dependencyGraph,
        Variable root,
        Block block,
        Instantiation instantiation,
        DpFactory factory,
        CancellationToken cancellationToken)
    {
    }

    public virtual void VisitArg(
        TContext context,
        DependencyGraph dependencyGraph,
        Variable rootVariable,
        Block block,
        Instantiation instantiation,
        DpArg dpArg,
        CancellationToken cancellationToken)
    {
    }
    
    protected Variable CreateVariable(IDictionary<MdBinding, Variable> variables, DependencyNode node)
    {
        switch (node)
        {
            case { Arg: {} }:
                if (variables.TryGetValue(node.Binding, out var argVar))
                {
                    return argVar;
                }

                argVar = new Variable(0, node)
                {
                    IsDeclared = true,
                    IsCreated = true
                };
                
                variables.Add(node.Binding, argVar);
                return argVar;
            
            case { Lifetime: Lifetime.Singleton }:
                if (variables.TryGetValue(node.Binding, out var singletonVar))
                {
                    return singletonVar;
                }

                singletonVar = new Variable(0, node)
                {
                    IsDeclared = true,
                    IsBlockRoot = true
                };

                variables.Add(node.Binding, singletonVar);
                return singletonVar;

            case { Lifetime: Lifetime.PerResolve }:
                if (variables.TryGetValue(node.Binding, out var perResolveVar))
                {
                    return perResolveVar;
                }

                perResolveVar = new Variable(_idGenerator.NextId, node);
                variables.Add(node.Binding, perResolveVar);
                return perResolveVar;

            default:
                var transientVar = new Variable(_idGenerator.NextId, node);
                return transientVar;
        }
    }
}