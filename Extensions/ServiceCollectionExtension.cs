﻿ using BlogApi.Context;
using BlogApi.Managers;
using BlogApi.Options;
using BlogApi.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Extensions;

public static class ServiceCollectionExtension
{
    private static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(JwtOptions));
        services.Configure<JwtOptions>(section);
        
        JwtOptions jwtOptions = section.Get<JwtOptions>()!;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var signingKey = System.Text.Encoding.UTF32.GetBytes(jwtOptions.SigningKey);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtOptions.ValidIssuer,
                    ValidAudience = jwtOptions.ValidAudience,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwt(configuration);
        services.AddScoped<JwtTokenManager>();
        services.AddScoped<UserManager>();
        services.AddScoped<BlogManager>();
        services.AddScoped<PostManager>();
        services.AddHttpContextAccessor();
        services.AddScoped<UserProvider>();
        services.AddSwaggerGenWithToken();
        
    }




    public static void MigrateBlogDb(this WebApplication app)
    {
        var identityDb = app.Services.GetService<AppDbContext>();
        if (identityDb != null)
            identityDb.Database.Migrate();
    }
}