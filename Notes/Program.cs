using Blazored.LocalStorage;
using MudBlazor.Services;
using Notes.Components;
using Notes.Hubs;

namespace Notes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents().AddCircuitOptions(o =>
                {
                    if (builder.Environment.IsDevelopment()) //only add details when debugging
                    {
                        o.DetailedErrors = true;
                    }
                });

            builder.Services.AddMudServices();
            builder.Services.AddBlazoredLocalStorage();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

            app.MapHub<NotesHub>("/noteshub");

            app.Run();
        }
    }
}
