using FileFlow.Application.Database;
using FileFlow.Application.MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileFlow.Application;

public static class HostApplicationBuilderExtensions
{
    public static void AddApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        });

        builder.Services.AddScoped<IEventBus, EventBus>();
        builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<AssemblyMarker>());
    }

    public static void UseApplication(this IHost app)
    {
        using var serviceScope = app.Services.CreateScope();

        var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        appDbContext.Database.EnsureCreated();
    }
}