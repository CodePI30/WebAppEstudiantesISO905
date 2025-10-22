
using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Application.Services;
using AppEstudiantesISO905.Domain.Contracts;
using AppEstudiantesISO905.Repository.CalificacionesRepository;
using AppEstudiantesISO905.Repository.DependencyInjection;
using AppEstudiantesISO905.Repository.EstudianteRepository;
using AppEstudiantesISO905.Repository.JwtToken;
using AppEstudiantesISO905.Repository.MateriaRepository;
using AppEstudiantesISO905.Repository.UsuarioRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AppEstudiantesISO905.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "AppEstudiantesISO905 API", Version = "v1" });

                // Configuración para JWT
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Ingrese el token JWT. Ejemplo: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            //DbContext Configuration Injection
            builder.Services.AddRepository(builder.Configuration);

            //Repository
            builder.Services.AddScoped<IUsuarioData, UsuarioData>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IEstudianteData, EstudianteData>();
            builder.Services.AddScoped<IMateriaData, MateriaData>();
            builder.Services.AddScoped<ICalificacionData, CalificacionData>();

            //Application
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<ILoginUsuarioHandler, LoginUsuarioHandler>();
            builder.Services.AddScoped<IEstudianteService, EstudianteService>();
            builder.Services.AddScoped<IMateriaService, MateriaService>();
            builder.Services.AddScoped<ICalificacionService, CalificacionService>();

            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                        if (!string.IsNullOrEmpty(authHeader))
                        {
                            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = $"Bearer {authHeader.Trim()}".Substring(7);
                            }
                            else
                            {
                                context.Token = authHeader.Substring("Bearer ".Length).Trim();
                            }
                        }
                        else
                        {
                            var cookieToken = context.Request.Cookies["AuthToken"];
                            if (!string.IsNullOrEmpty(cookieToken))
                            {
                                context.Token = cookieToken;
                            }
                        }

                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });


            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers();

            app.Run();
        }
    }
}
