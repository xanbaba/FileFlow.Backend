using FileFlow.Application.Database;
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
    }
}