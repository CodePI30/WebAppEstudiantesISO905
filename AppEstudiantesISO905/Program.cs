using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Application.Services;
using AppEstudiantesISO905.Domain.Contracts;
using AppEstudiantesISO905.Repository.JwtToken;
using AppEstudiantesISO905.Repository.UsuarioRepository;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AppEstudiantesISO905.Repository.EstudianteRepository;
using AppEstudiantesISO905.Repository.MateriaRepository;
using AppEstudiantesISO905.Repository.CalificacionesRepository;
using AppEstudiantesISO905.Repository.DependencyInjection;

namespace AppEstudiantesISO905
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
                        // Token from cookies
                        var token = context.Request.Cookies["AuthToken"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
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
            })
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/Auth/Login";
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
