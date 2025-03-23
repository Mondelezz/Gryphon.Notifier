using Microsoft.OpenApi.Models;
using Infrastructure;
using Application;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Domain.Models;

namespace API;

internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy
                .WithOrigins("http://localhost:5173")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection("GoogleOptions"));

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); // Конвертирует enum в string

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
            }

            options.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));
        });

        GoogleOptions? googleOptions = builder.Configuration
            .GetSection(nameof(GoogleOptions))
            .Get<GoogleOptions>();

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddGoogle(options =>
            {
                string clientId = googleOptions?.ClientId ?? string.Empty;

                if (string.IsNullOrEmpty(clientId))
                {
                    throw new ArgumentNullException(clientId, "is null or empty");
                }

                string clientSecret = googleOptions?.ClientSecret ?? string.Empty;

                if (string.IsNullOrEmpty(clientSecret))
                {
                    throw new ArgumentNullException(clientSecret, "is null or empty");
                }

                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url"); // для получения изображения пользователя
            });

        builder.Services.AddIdentity<User, IdentityRole>()
            .Add;

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

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
