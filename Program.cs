using Archivyn.Components;
using Archivyn.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace Archivyn
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddDbContext<ArchivynDbContext>(options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    options.UseInMemoryDatabase("ArchivynLocal");
                }
                else
                {
                    var connectionString =
                        builder.Configuration.GetConnectionString("Archivyn")
                        ?? throw new InvalidOperationException(
                            "The Archivyn database connection string is missing.");

                    options.UseNpgsql(connectionString);
                }
            });

            builder.Services.AddMudServices();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                    .GetRequiredService<ArchivynDbContext>();

                if (app.Environment.IsDevelopment())
                {
                    db.Database.EnsureCreated();
                }
                else
                {
                    db.Database.Migrate();
                }

                await db.EnsureSystemKeywordsOnAllDocumentTypesAsync();
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
