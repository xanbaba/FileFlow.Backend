using FileFlow.Application.Database;
using FileFlow.Application.MessageBus;
using FileFlow.Application.Options;
using FileFlow.Application.Services;
using FileFlow.Application.Services.Abstractions;
using FileFlow.Application.Utilities.Auth0Utility;
using FileFlow.Application.Utilities.EmailUtility;
using FileFlow.Application.Utilities.FileStorageUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileFlow.Application;

public static class HostApplicationBuilderExtensions
{
    public static void AddApplication(this IHostApplicationBuilder builder,
        Action<DbContextOptionsBuilder> dbContextOptionsAction)
    {
        builder.Services.AddDbContext<AppDbContext>(dbContextOptionsAction);
        
        builder.Services.Configure<Auth0Options>(
            builder.Configuration.GetSection("Auth0"));
        
        builder.Services.Configure<EmailSettings>(
            builder.Configuration.GetSection("EmailSettings"));
        
        builder.Services.AddHttpClient("Auth0Client", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Auth0:Domain"]!);
        });

        builder.Services.AddScoped<IEventBus, EventBus>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IFolderService, FolderService>();
        builder.Services.AddScoped<IItemService, ItemService>();
        builder.Services.AddScoped<IFileStorage, FileStorage>();
        builder.Services.AddScoped<ISupportEmail, SupportEmail>();
        builder.Services.AddSingleton<IAuth0AccessTokenProvider, Auth0AccessTokenProvider>();
        builder.Services.AddScoped<IUserUtility, UserUtility>();
        builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<AssemblyMarker>());
    }

    public static void UseApplication(this IHost app)
    {
        using var serviceScope = app.Services.CreateScope();

        var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        appDbContext.Database.Migrate();
    }
}