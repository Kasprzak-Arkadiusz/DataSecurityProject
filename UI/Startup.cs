using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using UI.Utils;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDetection();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index";
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
                    options.SlidingExpiration = false;
                    options.AccessDeniedPath = "/Home/Index";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                });

            services.AddHttpClient("api", c =>
                {
                    c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiUrl"));
                    c.DefaultRequestHeaders.Add(
                        HeaderNames.Accept, "*/*");
                    c.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                    c.DefaultRequestHeaders.Add("Keep-Alive", "3600");
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var certificate = new X509Certificate2(
                        Environment.GetEnvironmentVariable("CertificateFileLocation"),
                        Environment.GetEnvironmentVariable("CertificatePassword"));
                    var certificateValidator = new SingleCertificateValidator(certificate);

                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = certificateValidator.Validate
                    };
                });

            services.AddDataProtection();

            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiForgeryFieldName";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.Cookie.Name = "AntiForgeryCookie";
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseDetection();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy",
                    "default-src 'self';" +
                    " connect-src https://api/ https://localhost:5001/ 'self'");

                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}