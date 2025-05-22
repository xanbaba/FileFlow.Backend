using System.Security.Claims;
using FileFlow.Api;
using FileFlow.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AssemblyMarker = FileFlow.Api.AssemblyMarker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

builder.AddApplication(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(AssemblyMarker).Assembly))
);

// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseApplication();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();