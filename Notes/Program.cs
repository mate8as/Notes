using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Notes.Components;
using Notes.Hubs;
using Notes.Middlewares;
using Notes.Models;
using Notes.StateContainers;
using System;

namespace Notes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddCircuitOptions(o =>
                {
                    if (builder.Environment.IsDevelopment()) //only add details when debugging
                    {
                        o.DetailedErrors = true;
                    }
                });

            var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();
            var connectionString = configuration.GetConnectionString("NotesContext");

            builder.Services.AddDbContext<NotesContext>(options =>
                    options.UseMySql(
                       connectionString,
                        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"),
                        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
                    )
            );

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<NotesContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Notes.Auth";
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/access-denied";
                options.SlidingExpiration = true;

                // Critical settings for modern browsers:
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                // Essential for Blazor Server:
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false; // For GDPR compliance, set as needed
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });


            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddMudServices();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddControllers();

            builder.Services.AddScoped<DarkModeSwitchStateContainer>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCookiePolicy();   // Cookie policy before Authentication

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<BlazorCookieLoginMiddleware>();
            app.UseAntiforgery();

            // Map endpoints last
            app.MapControllers();
            app.MapStaticAssets();
            app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
            app.MapHub<NotesHub>("/noteshub");

            app.Run();
        }
    }
}
