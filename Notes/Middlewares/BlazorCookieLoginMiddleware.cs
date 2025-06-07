using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using Notes.Hubs;
using Notes.Models;
using Notes.Services;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Notes.Middlewares
{
    public class LoginInfo
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }

    public class BlazorCookieLoginMiddleware
    {
        public static IDictionary<Guid, LoginInfo> Logins { get; private set; } = new ConcurrentDictionary<Guid, LoginInfo>();
        public static IDictionary<string, (string Event, DateTime Time, Severity Severity)> PendingNotifications { get; private set; } = new ConcurrentDictionary<string, (string Event, DateTime Time, Severity Severity)>();

        private readonly RequestDelegate _next;
        private readonly IHubContext<NotesHub> _hubContext;

        public BlazorCookieLoginMiddleware(RequestDelegate next, IHubContext<NotesHub> hubContext)
        {
            _next = next;
            _hubContext = hubContext;
        }

        public async Task Invoke(HttpContext context, SignInManager<User> signInMgr)
        {
            if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
            {
                var key = Guid.Parse(context.Request.Query["key"]);
                var info = Logins[key];

                var result = await signInMgr.PasswordSignInAsync(info.Username, info.Password, false, lockoutOnFailure: true);
                info.Password = null;
                if (result.Succeeded)
                {
                    Logins.Remove(key);
                    PendingNotifications[info.Username] = ("You logged in successfully!", DateTime.Now, Severity.Success);
                    context.Response.Redirect("/");
                    return;
                }
                else if (result.RequiresTwoFactor)
                {
                    //TODO: redirect to 2FA razor component
                    //context.Response.Redirect("/loginwith2fa/" + key);
                    return;
                }
                else
                {
                    PendingNotifications[info.Username] = ("Something went wrong during authentication, please try again later!", DateTime.Now, Severity.Error);
                    context.Response.Redirect("/");
                    return;
                }
            }

            if (context.Request.Path == "/logout" && context.Request.Query.ContainsKey("key"))
            {
                var key = Guid.Parse(context.Request.Query["key"]);
                var info = Logins[key];

                await signInMgr.SignOutAsync();

                PendingNotifications[info.Username] = ("You logged out successfully!", DateTime.Now, Severity.Success);
                context.Response.Redirect("/");
                return;
            }

            await _next.Invoke(context);

        }
    }

    public static class BlazorCookieLoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseBlazorCookieLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BlazorCookieLoginMiddleware>();
        }
    }
}
