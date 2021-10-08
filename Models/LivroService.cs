using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Biblioteca.Models {
    public class LivroService {
        public void Inserir (Livro l) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                bc.Livros.Add (l);
                bc.SaveChanges ();
            }
        }

        public void Atualizar (Livro l) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                Livro livro = bc.Livros.Find (l.Id);
                livro.Autor = l.Autor;
                livro.Titulo = l.Titulo;
                livro.Ano = l.Ano;

                bc.SaveChanges ();
            }
        }

        public ICollection<Livro> ListarTodos (int pagina = 1, int tamanho = 10, Filtragem filtro = null) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                IQueryable<Livro> query;
                int pular = (pagina - 1) * tamanho;

                if (filtro != null) {
                    switch (filtro.TipoFiltro) {
                        case "Autor":
                            query = bc.Livros.Where (l => l.Autor.Contains (filtro.Filtro, StringComparison.CurrentCultureIgnoreCase));
                            break;

                        case "Titulo":
                            query = bc.Livros.Where (l => l.Titulo.Contains (filtro.Filtro, StringComparison.CurrentCultureIgnoreCase));
                            break;

                        default:
                            query = bc.Livros;
                            break;
                    }
                } else {
                    query = bc.Livros;
                }
                return query.OrderBy (l => l.Titulo).Skip (pular).Take (tamanho).ToList ();
            }
        }

        public ICollection<Livro> ListarDisponiveis () {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                return
                bc.Livros
                    .Where (l => !(bc.Emprestimos.Where (e => e.Devolvido == false).Select (e => e.LivroId).Contains (l.Id)))
                    .ToList ();
            }
        }

        public Livro ObterPorId (int id) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                return bc.Livros.Find (id);
            }
        }

        public int NumeroDeLivros () {
            using (var context = new BibliotecaContext ()) {
                return context.Livros.Count ();
            }
        }
    }
}