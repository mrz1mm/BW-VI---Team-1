using BW_VI___Team_1.Interfaces;
using BW_VI___Team_1.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BW_VI___Team_1.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthSvc _authSvc;

        public AuthController(IAuthSvc authSvc)
        {
            _authSvc = authSvc;
        }

        // VIEWS
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }


        // METODI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username, Password, Role")] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _authSvc.Register(model);
            if (!success)
            {
                TempData["error"] = "Username già esistente";
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username, Password")] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Errore nei dati inseriti";
                return View();
            }

            var success = await _authSvc.Login(model);
            if (!success)
            {
                TempData["error"] = "Account non esistente";
                return View();
            }

            TempData["Success"] = "Login effettuato con successo";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _authSvc.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
