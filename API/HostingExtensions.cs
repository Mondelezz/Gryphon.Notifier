using Microsoft.OpenApi.Models;
using Infrastructure;
using Application;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using API.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GoogleOptions = API.Options.GoogleOptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Domain.Models;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;

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

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                GoogleOptions googleOptions = builder.Configuration
                .GetSection(nameof(GoogleOptions))
                .Get<GoogleOptions>() ?? throw new ArgumentException(nameof(GoogleOptions));

                string clientId = googleOptions.ClientId ?? string.Empty;

                if (string.IsNullOrEmpty(clientId))
                {
                    throw new ArgumentNullException(clientId, "is null or empty");
                }

                string clientSecret = googleOptions.ClientSecret ?? string.Empty;

                if (string.IsNullOrEmpty(clientSecret))
                {
                    throw new ArgumentNullException(clientSecret, "is null or empty");
                }

                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.SaveTokens = true;
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                options.Scope.Add("offline_access"); // Для получения Refresh token
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 JwtOptions jwtOptions = builder.Configuration.GetSection(JwtOptions.JwtOptionsKey)
                         .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtOptions.Issuer,
                     ValidAudience = jwtOptions.Audience,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                 };

                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         context.Token = context.Request.Cookies["ACCESS_TOKEN"];
                         return Task.CompletedTask;
                     }
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

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        return app;
    }
}
