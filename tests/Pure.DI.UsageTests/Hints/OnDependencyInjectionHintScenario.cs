﻿/*
$v=true
$p=2
$d=OnDependencyInjection Hint
$h=The _OnDependencyInjection_ hint determines whether to generate partial _OnDependencyInjection_ method to control of dependency injection.
*/

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable CheckNamespace
// ReSharper disable UnusedParameterInPartialMethod
// ReSharper disable UnusedVariable
namespace Pure.DI.UsageTests.Hints.OnDependencyInjectionHintScenario;

using System.Collections.Immutable;
using Shouldly;
using Xunit;

// {
public interface IDependency
{
}

public class Dependency : IDependency
{
}

public interface IService
{
    IDependency Dependency { get; }
}

public class Service : IService
{
    public Service(IDependency dependency)
    {
        Dependency = dependency;
    }

    public IDependency Dependency { get; }
}

internal partial class Composition
{
    private readonly List<string> _log;

    public Composition(List<string> log)
        : this()
    {
        _log = log;
    }

    private partial T OnDependencyInjection<T>(in T value, object? tag, Lifetime lifetime)
    {
        _log.Add($"{value?.GetType().Name} injected");
        return value;
    }
}
// }

public class Scenario
{
    [Fact]
    public void Run()
    {
        // ToString = On
// {            
        // OnDependencyInjection = On
        // OnDependencyInjectionContractTypeNameRegularExpression = IDependency
        DI.Setup("Composition")
            .Bind<IDependency>().To<Dependency>()
            .Bind<IService>().Tags().To<Service>()
            .Root<IService>("Root");

        var log = new List<string>();
        var composition = new Composition(log);
        var service = composition.Root;
        
        log.ShouldBe(ImmutableArray.Create("Dependency injected"));
// }
        TestTools.SaveClassDiagram(composition, nameof(OnDependencyInjectionHintScenario));
    }
}