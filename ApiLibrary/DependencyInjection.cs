﻿using ApiLibrary.Authentication;
using ApiLibrary.Common;
using ApiLibrary.Persistence;
using ApiLibrary.Repositories.LastConnectionRepository;
using ApiLibrary.Repositories.LoginFailureRepository;
using ApiLibrary.Repositories.PasswordResetRepository;
using ApiLibrary.Repositories.SecretRepository;
using ApiLibrary.Repositories.UserRepository;
using ApiLibrary.UserPasswordReset;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApiLibrary
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiLibrary(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(ConnectionStringBuilder.Create(),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 4,
                            maxRetryDelay: TimeSpan.FromSeconds(1),
                            errorNumbersToAdd: new int[] { });
                    }
                ));

            services.AddDataProtection();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILoginFailureRepository, LoginFailureRepository>();
            services.AddTransient<ISecretRepository, SecretRepository>();
            services.AddTransient<IPasswordResetRepository, PasswordResetRepository>();
            services.AddTransient<ILastConnectionRepository, LastConnectionRepository>();

            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ISecretPasswordHasher, SecretPasswordHasher>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddSingleton<ITokenProvider, TokenProvider>();

            return services;
        }
    }
}