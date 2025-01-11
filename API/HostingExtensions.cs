using Microsoft.OpenApi.Models;
using Infrastructure;
using Application;

namespace API;

internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Gryphon",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Url = new Uri("https://github.com/Mondelezz"),
                    Email = @"pankov.egor26032005@yandex.ru",
                    Name = "Mondelezz"
                },
            });

            string[] xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.AllDirectories);

            foreach (string xmlFile in xmlFiles)
            {
                o.IncludeXmlComments(xmlFile, true);
            };
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

        return app;
    }
}
