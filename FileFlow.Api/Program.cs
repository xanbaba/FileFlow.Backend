using System.Security.Claims;
using FileFlow.Api;
using FileFlow.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using AssemblyMarker = FileFlow.Api.AssemblyMarker;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register endpoints in DI
builder.Services.AddEndpoints();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(x =>
    x.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder
            .WithOrigins(
                "http://localhost:5173",
                "https://file-flow-frontend-xanbabas-projects.vercel.app",
                "https://file-flow-frontend-git-master-xanbabas-projects.vercel.app",
                "https://file-flow-frontend-git-dev-xanbabas-projects.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    })
);

builder.AddApplication(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
    b => b.MigrationsAssembly(typeof(AssemblyMarker).Assembly))
);

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Build the application.
var app = builder.Build();

// Enable Swagger and Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseApplication();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();