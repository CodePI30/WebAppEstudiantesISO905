using AppEstudiantesISO905.Application.Contracts;
using AppEstudiantesISO905.Application.Services;
using AppEstudiantesISO905.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppEstudiantesISO905.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILoginUsuarioHandler _loginHandler;

        public AuthController(ILoginUsuarioHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var token = await _loginHandler.Handle(request.Email, request.Password);
                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(2)
                });

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("", "Usuario o contraseña inválidos");
                return View(request);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login", "Auth");
        }
    }
}
