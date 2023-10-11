using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UsersAPI.Models.DatabaseContext;

namespace UsersAPI.Models
{
    static public class Extensions
    {
        static public void AddUsersSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        static public void AddUsersAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthorizationOpt.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthorizationOpt.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthorizationOpt.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
        }

        static public void AddUsersDBContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UsersDataBaseContext>((options) => options.UseMySQL(connectionString));
        }
    }
}
