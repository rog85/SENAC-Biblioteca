using System;
using Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController (ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index () {
            Autenticacao.CheckLogin (this);
            return View ();
        }

        public IActionResult Login () {
            return View ();
        }

        [HttpPost]
        public IActionResult Login (string login, string senha) {
            UsuarioService usuarioService = new UsuarioService ();
            Usuario usuario = usuarioService.ValidarLogin (login, senha);

            if (usuario == null) {
                ViewData["Erro"] = "Dados inválidos";
                return View ();
            } else {
                HttpContext.Session.SetString ("Login", usuario.Login);
                return RedirectToAction ("Index");
            }
        }

        public IActionResult Logout () {
            HttpContext.Session.SetString ("Login", "");
            return RedirectToAction ("Index");
        }

        public IActionResult Privacy () {
            return View ();
        }
    }
}