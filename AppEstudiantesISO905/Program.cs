using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Application.Services;
using AppEstudiantesISO905.Domain.Contracts;
using AppEstudiantesISO905.Repository.JwtToken;
using AppEstudiantesISO905.Repository.UsuarioRepository;
using AppEstudiantesISO905.Repository.WebAppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AppEstudiantesISO905.Repository.EstudianteRepository;
using AppEstudiantesISO905.Repository.MateriaRepository;
using AppEstudiantesISO905.Repository.CalificacionesRepository;

namespace AppEstudiantesISO905
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<EstudiantesIso905Context>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
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
                        // Tomar token desde la cookie
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
                options.LoginPath = "/Auth/Login"; // Aquí defines la ruta de login
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
