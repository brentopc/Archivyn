using Archivyn.Components;
using Archivyn.Data;
using Microsoft.EntityFrameworkCore;

namespace Archivyn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddDbContext<ArchivynDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Archivyn")));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                    .GetRequiredService<ArchivynDbContext>();

                //db.Database.EnsureCreated();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
