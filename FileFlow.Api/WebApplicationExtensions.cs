using FileFlow.Api.Endpoints;

namespace FileFlow.Api;

public static class WebApplicationExtensions
{
    public static void AddEndpoints(this IServiceCollection collection)
    {
        collection.Scan(scan =>
            scan.FromAssemblyOf<ProjectReference>()
                .AddClasses(classes =>
                    classes.AssignableTo<IEndpoint>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
        );
    }

    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        foreach (var endpoint in endpoints)
        {
            endpoint.Map(app);
        }
    }
}