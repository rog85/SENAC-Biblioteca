using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Biblioteca.Models {
    public class UsuarioService {

        public bool Inserir (Usuario novoUsuario) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                if (bc.Usuarios.Any (u => u.Login.Equals (novoUsuario.Login))) {
                    return false;
                }
                string senha = novoUsuario.Senha;
                novoUsuario.Senha = CriaSenhaMD5 (senha);
                bc.Usuarios.Add (novoUsuario);
                bc.SaveChanges ();
            }
            return true;
        }

        public bool Atualizar (Usuario novoUsuario) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                Usuario usuario = bc.Usuarios.Find (novoUsuario.Id);

                if (bc.Usuarios.Any (u => u.Login.Equals (novoUsuario.Login) &&
                        !novoUsuario.Login.Equals (usuario.Login))) {
                    return false;
                }

                usuario.Nome = novoUsuario.Nome;
                usuario.Login = novoUsuario.Login;
                string novaSenhaHash = CriaSenhaMD5 (novoUsuario.Senha);
                if (!usuario.Senha.Equals (novaSenhaHash)) {
                    usuario.Senha = novaSenhaHash;
                }
                bc.SaveChanges ();
            }
            return true;
        }

        public void Excluir (int id) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                bc.Usuarios.Remove (ObterPorId (id));
                bc.SaveChanges ();
            }
        }

        public ICollection<Usuario> ListarTodos (int pagina = 1, int tamanho = 5, Filtragem filtro = null) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                IQueryable<Usuario> query;
                int pular = (pagina - 1) * tamanho;

                if (filtro != null) {
                    switch (filtro.TipoFiltro) {
                        case "Nome":
                            query = bc.Usuarios.Where (u => u.Nome.Contains (filtro.Filtro, StringComparison.CurrentCultureIgnoreCase));
                            break;

                        case "Login":
                            query = bc.Usuarios.Where (u => u.Login.Contains (filtro.Filtro, StringComparison.CurrentCultureIgnoreCase));
                            break;

                        default:
                            query = bc.Usuarios;
                            break;
                    }
                } else {
                    query = bc.Usuarios;
                }
                return query.Skip (pular).Take (tamanho).ToList ();
            }
        }

        public Usuario ObterPorId (int id) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {
                return bc.Usuarios.Find (id);
            }
        }

        public Usuario ValidarLogin (string login, string senha) {
            using (BibliotecaContext bc = new BibliotecaContext ()) {

                string hashMD5 = CriaSenhaMD5 (senha);
                Usuario encontrado = bc.Usuarios.FirstOrDefault (u => u.Login.Equals (login) && u.Senha.Equals (hashMD5));
                return encontrado;
            }
        }

        public string CriaSenhaMD5 (string senha) {
            string hashMD5 = null;

            if (!string.IsNullOrEmpty (senha))
                hashMD5 = BitConverter.ToString (MD5.Create ().ComputeHash (Encoding.ASCII.GetBytes (senha))).Replace ("-", "");

            return hashMD5;
        }

        public int NumeroDeUsuarios () {
            using (var context = new BibliotecaContext ()) {
                return context.Usuarios.Count ();
            }
        }
    }
}