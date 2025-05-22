using FileFlow.Application.Database;
using FileFlow.Application.MessageBus;
using FileFlow.Application.Services;
using FileFlow.Application.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileFlow.Application;

public static class HostApplicationBuilderExtensions
{
    public static void AddApplication(this IHostApplicationBuilder builder, Action<DbContextOptionsBuilder> dbContextOptionsAction)
    {
        builder.Services.AddDbContext<AppDbContext>(dbContextOptionsAction);

        builder.Services.AddScoped<IEventBus, EventBus>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IFolderService, FolderService>();
        builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<AssemblyMarker>());
    }

    public static void UseApplication(this IHost app)
    {
        using var serviceScope = app.Services.CreateScope();

        var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        appDbContext.Database.EnsureCreated();
    }
}