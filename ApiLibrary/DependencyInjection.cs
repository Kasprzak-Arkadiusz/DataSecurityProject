﻿using System;
using ApiLibrary.Authentication;
using ApiLibrary.Persistence;
using ApiLibrary.Repositories.LoginFailureRepository;
using ApiLibrary.Repositories.SecretRepository;
using ApiLibrary.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiLibrary
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiLibrary(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 4,
                            maxRetryDelay: TimeSpan.FromSeconds(1),
                            errorNumbersToAdd: new int[] { });
                    }
                ));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILoginFailureRepository, LoginFailureRepository>();
            services.AddTransient<ISecretRepository, SecretRepository>();

            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ISecretPasswordHasher, SecretPasswordHasher>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}