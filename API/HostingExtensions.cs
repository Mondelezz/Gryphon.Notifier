using Microsoft.OpenApi.Models;
using Infrastructure;
using Application;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace API;

internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Gryphon",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Url = new Uri("https://github.com/Mondelezz"),
                    Email = "pankov.egor26032005@yandex.ru",
                    Name = "Mondelezz"
                },
            });

            foreach (string xmlFile in Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.AllDirectories))
            {
                options.IncludeXmlComments(xmlFile, true);
            };

            options.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));
        });

        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(Path.Combine("appsettings.json"), optional: false, reloadOnChange: true);

        builder.Services.RegisterInfrastructureLayer(builder.Configuration, builder.Environment);
        builder.Services.RegisterApplicationLayer();

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        // Накатываем миграции TODO : не использовать в Production
        using (IServiceScope scope = app.Services.CreateScope())
        {
            MigrationDbContext db = scope.ServiceProvider.GetRequiredService<MigrationDbContext>();
            db.Database.Migrate();
        }

        return app;
    }
}
