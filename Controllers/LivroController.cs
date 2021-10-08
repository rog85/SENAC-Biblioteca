using System;
using System.Collections.Generic;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers {
    public class LivroController : Controller {

        public IActionResult Cadastro () {
            Autenticacao.CheckLogin (this);
            return View ();
        }

        [HttpPost]
        public IActionResult Cadastro (Livro l) {
            LivroService livroService = new LivroService ();

            if (l.Id == 0) {
                livroService.Inserir (l);
            } else {
                livroService.Atualizar (l);
            }

            return RedirectToAction ("Listagem");
        }

        public IActionResult Listagem (string tipoFiltro, string filtro, int p = 1) {
            Autenticacao.CheckLogin (this);
            Filtragem objFiltro = null;
            if (!string.IsNullOrEmpty (filtro)) {
                objFiltro = new Filtragem ();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            int quantidadePorPagina = 4;
            LivroService livroService = new LivroService ();
            int totalDeRegistros = livroService.NumeroDeLivros ();
            ICollection<Livro> lista = livroService.ListarTodos (p, quantidadePorPagina, objFiltro);
            ViewData["NroPaginas"] = (int) Math.Ceiling ((double) totalDeRegistros / quantidadePorPagina);
            return View (lista);
        }

        public IActionResult Edicao (int id) {
            Autenticacao.CheckLogin (this);
            LivroService ls = new LivroService ();
            Livro l = ls.ObterPorId (id);
            return View (l);
        }
    }
}