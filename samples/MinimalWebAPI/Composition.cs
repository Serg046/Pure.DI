// ReSharper disable UnusedMember.Local

namespace MinimalWebAPI;

using Pure.DI;
using Pure.DI.MS;
using WeatherForecast;
using static Pure.DI.Lifetime;

internal partial class Composition: ServiceProviderFactory<Composition>
{
    private static void Setup() =>
        DI.Setup(nameof(Composition))
            .DependsOn(Base)
            // Specifies not to attempt to resolve types whose fully qualified name
            // begins with Microsoft.Extensions., Microsoft.AspNetCore.
            // since ServiceProvider will be used to retrieve them.
            .Hint(
                Hint.OnCannotResolveContractTypeNameRegularExpression,
                @"^Microsoft\.(Extensions|AspNetCore)\..+$")
            
            .Bind().As(Singleton).To<WeatherForecastService>()
                .Root<IWeatherForecastService>()
            
            // Application composition root
            .Root<Program>("Root");
}