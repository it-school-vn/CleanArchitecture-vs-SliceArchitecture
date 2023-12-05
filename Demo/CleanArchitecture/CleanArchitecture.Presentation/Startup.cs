using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using CleanArchitecture.Infrastructure.Auth.Github;
using CleanArchitecture.Infrastructure.Auth.Google;
using CleanArchitecture.Infrastructure.Auth.Microsoft;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Presentation.Api
{
    public static class Startup
    {
        public class EnumSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema model, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    model.Enum.Clear();
                    foreach (string enumName in Enum.GetNames(context.Type))
                    {
                        var memberInfo = context.Type.GetMember(enumName).FirstOrDefault(m => m.DeclaringType == context.Type);
                        EnumMemberAttribute? enumMemberAttribute = memberInfo?.GetCustomAttributes(typeof(EnumMemberAttribute), false).OfType<EnumMemberAttribute>().FirstOrDefault();
                        string label = enumMemberAttribute == null || string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
                         ? enumName
                         : enumMemberAttribute.Value;
                        model.Enum.Add(new OpenApiString(label));
                    }
                }
            }
        }
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
             {
                 c.SchemaFilter<EnumSchemaFilter>();
                 c.MapType<DateOnly>(() => new OpenApiSchema
                 {
                     Type = "string",
                     Format = "date",
                     Example = new OpenApiString("2023-01-01")
                 });
                 c.MapType<Ulid>(() => new OpenApiSchema
                 {
                     Type = "string",
                     Format = "string",
                     Example = new OpenApiString("01HEXY08C8AADWQA6ZCE3YT3ZW")
                 });
                 c.MapType<TimeOnly>(() => new OpenApiSchema
                 {
                     Type = "string",
                     Format = "TimeOnlyFormat",
                     Example = new OpenApiString("09:00:00")
                 });
                 c.CustomSchemaIds(x =>
                  {
                      var modelName = x.GetCustomAttributes<DisplayNameAttribute>()
                      .SingleOrDefault()?
                      .DisplayName;

                      return String.IsNullOrEmpty(modelName) ? x.Name : modelName;
                  });

                 c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                 {
                     Type = SecuritySchemeType.Http,
                     Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                     Description = "Enter JWT Bearer token **_only_**",
                     BearerFormat = "JWT",
                     In = ParameterLocation.Header,
                     Name = JwtBearerDefaults.AuthenticationScheme,
                     Reference = new OpenApiReference
                     {
                         Id = JwtBearerDefaults.AuthenticationScheme,
                         Type = ReferenceType.SecurityScheme
                     }

                 });



                 c.AddSecurityDefinition(GoogleJwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                 {
                     Type = SecuritySchemeType.Http,
                     Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                     Description = "Enter Google JWT Bearer token **_only_**",
                     // OpenIdConnectUrl = new Uri("https://accounts.google.com/.well-known/openid-configuration"),
                     Reference = new OpenApiReference
                     {
                         Id = GoogleJwtBearerDefaults.AuthenticationScheme,
                         Type = ReferenceType.SecurityScheme
                     },
                     Name = GoogleJwtBearerDefaults.AuthenticationScheme,
                     BearerFormat = "JWT",
                     In = ParameterLocation.Header,
                     /* Extensions = new Dictionary<string, IOpenApiExtension>
                       {
                           { "x-tokenName", new OpenApiString("token id_token") }
                       },*/


                 });

                 c.AddSecurityDefinition(MicrosoftJwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                 {
                     Type = SecuritySchemeType.Http,
                     Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                     Description = "Enter Microsoft JWT Bearer token **_only_**",
                     Reference = new OpenApiReference
                     {
                         Id = MicrosoftJwtBearerDefaults.AuthenticationScheme,
                         Type = ReferenceType.SecurityScheme
                     },
                     Name = MicrosoftJwtBearerDefaults.AuthenticationScheme,
                     BearerFormat = "JWT",
                     In = ParameterLocation.Header,

                 });

                 c.AddSecurityDefinition(GithubTokenDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                 {
                     Type = SecuritySchemeType.ApiKey,
                     Scheme = GithubTokenDefaults.ApiKey,
                     Description = "Enter Github access token **_only_**",
                     Reference = new OpenApiReference
                     {
                         Id = GithubTokenDefaults.AuthenticationScheme,
                         Type = ReferenceType.SecurityScheme
                     },
                     Name = GithubTokenDefaults.ApiKey,
                     In = ParameterLocation.Header,

                 });

                 c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme }
                            },new[] { JwtBearerDefaults.AuthenticationScheme }
                        },
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = GoogleJwtBearerDefaults.AuthenticationScheme }
                            },new[] { GoogleJwtBearerDefaults.AuthenticationScheme }
                        },
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = MicrosoftJwtBearerDefaults.AuthenticationScheme }
                            },new[] { MicrosoftJwtBearerDefaults.AuthenticationScheme }
                        },
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = GithubTokenDefaults.AuthenticationScheme }
                            },new[] { GithubTokenDefaults.AuthenticationScheme }
                        }

                 });


             }

             );
        }
    }
}