using MySql.Data.MySqlClient;
using Novo_ASP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Novo_ASP_MVC.Views.Perfil
{
    public class PerfilController : Controller
    {
        // GET: Perfil
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["info"];
            List<Dataset> Lista = new List<Dataset>();
            //Se usuário não estiver logado
            if (cookie == null)
                return Redirect("/Login/Entrar");
            //Senão, acessa o BD
            MySqlConnection conn = new MySqlConnection("server=localhost;database=progweb;uid=root;pwd=");
            try
            {
                conn.Open();
                var comando = conn.CreateCommand();
                string select = "(SELECT id FROM usuarios WHERE usuarios.usuario = @usuario)";
                string query = "SELECT nome, dominio, id_dataset FROM datasets WHERE " + select + " = id_usuario";
                comando.CommandText = query;
                comando.Parameters.AddWithValue("@usuario", cookie["user"]);

                var reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Dataset d = new Dataset
                        {
                            Dominio = reader.GetString("dominio"),
                            ID = reader.GetString("id_dataset")
                        };

                    if (!reader.IsDBNull(0))
                        d.Nome = reader.GetString("nome");
                    
                    Lista.Add(d);
                }
            }
            finally
            {
                conn.Close();
            }
            return View(Lista);
        }
        //[HttpPost]
        public ActionResult Deletar(Dataset d)
        {
            HttpCookie cookie = Request.Cookies["info"];
            MySqlConnection conn = new MySqlConnection("server=localhost;database=progweb;uid=root;pwd=");
            try
            {
                conn.Open();
                var comando = conn.CreateCommand();
                string GetUser = "(SELECT id FROM usuarios WHERE usuario = @usuario)";
                comando.CommandText = "DELETE FROM datasets WHERE "+ GetUser +" = id_usuario AND dominio = @dominio AND id_dataset = @id";
                comando.Parameters.AddWithValue("@dominio", d.Dominio);
                comando.Parameters.AddWithValue("@id", d.ID);
                comando.Parameters.AddWithValue("@usuario", cookie["user"]);
                var reader = comando.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Adicionar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Adicionar(Dataset d)
        {
            if (ModelState.IsValid)
            {
                HttpCookie cookie = Request.Cookies["info"];
                List<Dataset> Lista = new List<Dataset>();
                //Se usuário não estiver logado
                if (cookie == null)
                    return Redirect("/Login/Entrar");
                //Se estiver
                MySqlConnection conn = new MySqlConnection("server=localhost;database=progweb;uid=root;pwd=");
                try
                {
                    conn.Open();
                    var comando = conn.CreateCommand();
                    comando.CommandText = "INSERT INTO datasets(nome, dominio, id_dataset, id_usuario) VALUE (@nome,@dominio,@id, (SELECT id FROM usuarios WHERE usuario = @usuario));";
                    comando.Parameters.AddWithValue("@nome", d.Nome);
                    comando.Parameters.AddWithValue("@dominio", d.Dominio);
                    comando.Parameters.AddWithValue("@id", d.ID);
                    comando.Parameters.AddWithValue("@usuario", cookie["user"]);

                    var reader = comando.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult ChamaFerramenta(Dataset d)
        {
            
            return RedirectToAction("Home","Ferramenta",d);
        }

    }
}