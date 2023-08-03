﻿using System.CommandLine;
using Build;using HostApi;
using NuGet.Versioning;
using Pure.DI;

const string defaultVersion = "2.0.*";

NuGetVersion? versionOverride = default;
if (NuGetVersion.TryParse(Property.Get("version", ""), out var versionOverrideValue))
{
    versionOverride = versionOverrideValue;
}

Directory.SetCurrentDirectory(Tools.GetSolutionDirectory());
var settings = new Settings(
    Environment.GetEnvironmentVariable("TEAMCITY_VERSION") is not null,
    "Release",
    VersionRange.Parse(defaultVersion),
    versionOverride,
    Property.Get("NuGetKey", string.Empty),
    new CodeAnalysis(new Version(4, 3, 1)));

new DotNetBuildServerShutdown().Run();

var composition = new Composition(settings);
return await composition.Root.RunAsync();

internal partial class Program
{
    private readonly RootCommand _rootCommand;

    public Program(
        [Tag("readme")] ITarget<int> readme,
        [Tag("pack")] ITarget<string> pack,
        [Tag("benchmarks")] ITarget<int> benchmarks,
        [Tag("deploy")] ITarget<int> deploy,
        [Tag("template")] ITarget<string> template)
    {
        var readmeCommand = new Command("readme", "Generates README.MD");
        readmeCommand.SetHandler(readme.RunAsync);
        readmeCommand.AddAlias("r");

        var packCommand = new Command("pack", "Creates NuGet packages");
        packCommand.SetHandler(pack.RunAsync);
        packCommand.AddAlias("p");

        var benchmarksCommand = new Command("benchmarks", "Runs benchmarks");
        benchmarksCommand.SetHandler(benchmarks.RunAsync);
        benchmarksCommand.AddAlias("b");

        var deployCommand = new Command("deploy", "Push NuGet packages");
        deployCommand.SetHandler(deploy.RunAsync);
        deployCommand.AddAlias("d");

        var deployTemplateCommand = new Command("template", "Push NuGet packages");
        deployTemplateCommand.SetHandler(template.RunAsync);
        deployTemplateCommand.AddAlias("t");

        _rootCommand = new RootCommand
        {
            readmeCommand,
            packCommand,
            benchmarksCommand,
            deployCommand,
            deployTemplateCommand
        };
    }

    private Task<int> RunAsync() => _rootCommand.InvokeAsync(Args.ToArray());
}
