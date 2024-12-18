using System.Text;
using API.Application.Extensions;
using API.Application.Middleware;
using API.Core.Configuration;
using API.Core.Domain.Mapping;
using API.Infrastructure;
using API.Infrastructure.Initializers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MfaSettings>(builder.Configuration.GetSection("Mfa"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Add authentication
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    var jwtConfig = builder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("Key")
                                     ?? throw new NullReferenceException("JWT key cannot be null"));
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.GetValue<string>("Issuer"),
        ValidAudience = jwtConfig.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.BuildIdentityUser();

// Cors
builder.Services.AddCors(options => {
    var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl")
                      ?? throw new NullReferenceException("Frontend URL not found");

    options.AddPolicy(name: "Production",
        policy => {
            policy.WithOrigins(frontendUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });

    options.AddPolicy(name: "Development",
        policy => {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true);
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Add fluent validation
builder.Services.AddFluentValidationAutoValidation();

// Add automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services and repositories to ApplicationBuilder
builder.Services.AddServicesAndRepositories();

// DbContext
builder.Services.AddDbContext<AppDbContext>(db => {
    db.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    db.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseMiddleware<CurrentContextMiddleware>();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || args.Contains("swagger") || args.Contains("--swagger")) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment() || args.Contains("db-init") || args.Contains("--db-init")) {
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await dbInitializer.Init();
}

//app.UseHttpsRedirection();

app.Run();
