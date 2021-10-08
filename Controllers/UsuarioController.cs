using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers {
    public class UsuarioController : Controller {

        public IActionResult Cadastro () {
            Autenticacao.CheckLogin (this);
            return View ();
        }

        [HttpPost]
        public IActionResult Cadastro (Usuario novoUsuario) {
            UsuarioService usuarioService = new UsuarioService ();

            if (novoUsuario.Id == 0) {
                if (usuarioService.Inserir (novoUsuario)) {
                    return RedirectToAction ("Listagem");
                } else {
                    ViewData["Mensagem"] = "Login já existe! Por favor escolha outro.";
                    return View (novoUsuario);
                }
            } else {
                if (usuarioService.Atualizar (novoUsuario)) {
                    return RedirectToAction ("Listagem");
                } else {
                    ViewData["Mensagem"] = "Login já existe! Por favor escolha outro.";
                    return View ("Edicao", novoUsuario);
                }
            }

        }

        public IActionResult Listagem (string tipoFiltro, string filtro) {
            Autenticacao.CheckLogin (this);
            Filtragem objFiltro = null;
            if (!string.IsNullOrEmpty (filtro)) {
                objFiltro = new Filtragem ();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            UsuarioService usuarioService = new UsuarioService ();
            return View (usuarioService.ListarTodos (objFiltro));
        }

        public IActionResult Edicao (int id) {
            Autenticacao.CheckLogin (this);
            UsuarioService usuarioService = new UsuarioService ();
            Usuario usuario = usuarioService.ObterPorId (id);
            return View (usuario);
        }

        public IActionResult Exclui (int id) {
            Autenticacao.CheckLogin (this);
            UsuarioService usuarioService = new UsuarioService ();
            usuarioService.Excluir (id);
            return RedirectToAction ("Listagem");
        }
    }
}