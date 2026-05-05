using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.ModelLayer.AppSettings;

namespace WorkVista.API.ExtensionClasses
{
    public static class ServiceExtension
    {
        public static void AddInterfaceClasses(this IServiceCollection services)
        {
            string filePathToIoC = AppDomain.CurrentDomain.BaseDirectory;
            List<Dictionary<string, string>> configDis = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(File.ReadAllText(filePathToIoC + "\\IOC.json"));
            foreach (Dictionary<string, string> entry in configDis)
            {
                Type interfaceType = Type.GetType(entry[CommonConstants.INTERFACE]);
                Type implementationType = Type.GetType(entry[CommonConstants.IMPLEMENTATION]);

                services.AddScoped(interfaceType, implementationType);
            }
        }

        public static IServiceCollection ConfigureOpenAPI(this IServiceCollection services)
        {
            // Configure Open API (Swagger)
            // More Info: https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("swagger", new OpenApiInfo
                {
                    Title = "WorkVista API",
                    Version = ""
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
          }
        },
        new List<string>()
      }
        });
            });
        }

        public static AuthenticationBuilder ConfigureJwtAuthentication(this IServiceCollection services, AppSettingsModel settings)
        {
            // Add Authentication to Services
            return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters =
              new TokenValidationParameters
              {
                  ValidIssuer = settings.JWTSettings.Issuer,
                  ValidAudience = settings.JWTSettings.Audience,
                  IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(settings.JWTSettings.Key)),
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ClockSkew = TimeSpan.FromMinutes(settings.JWTSettings.MinutesToExpiration)
              };
            });
        }

        public static IServiceCollection ConfigureJwtAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorization(options =>
            {
                options.AddPolicy("GetSwitchRole", policy => policy.RequireClaim("GetSwitchRole"));
                options.AddPolicy("GetDashboard", policy => policy.RequireClaim("GetDashboard"));
            });
        }


        public static IServiceCollection ConfigureCors(this IServiceCollection services, AppSettingsModel settings)
        {
            // Add CORS
            return services.AddCors(options =>
            {
                options.AddPolicy(CommonConstants.CORS_POLICY,
              builder =>
              {
                  //builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                  //builder.WithOrigins("http://localhost:5173", "http://localhost:5173")
                  builder.WithOrigins(settings.EnableCorsOriginsAllowed.Split(";"))
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowCredentials();
                  // .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
                  //.SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
              });
            });
        }
    }
}
