using MySql.Data.MySqlClient;
using Novo_ASP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Novo_ASP_MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Entrar()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Sair()
        {
            ViewBag.User = Request.Cookies["info"]["user"];
            return View();
        }
        [HttpGet]
        public ActionResult Logoff()
        {
            if (Request.Cookies["info"] != null)
            {
                Response.Cookies["info"].Expires = DateTime.Now.AddDays(-1);
            }
           return Redirect("/Login/Entrar");   
        }

        [HttpPost]
        public ActionResult Entrar(Usuario u)
        {
            ModelState.Remove("Email");
            if (ModelState.IsValid)
            {
                Debug.WriteLine("teste");
                //Verifica usuário e senha.
                MySqlConnection conn = new MySqlConnection("server=localhost;database=progweb;uid=root;pwd=");
                try
                {
                    conn.Open();
                    var comando = conn.CreateCommand();
                    //comando.CommandText = "SELECT executa_login('" + u.User+ "','" + u.Senha + "')";
                    comando.CommandText = "SELECT executa_login(@usuario,@senha)";
                    comando.Parameters.AddWithValue("@usuario", u.User);
                    comando.Parameters.AddWithValue("@senha", u.Senha);
                    var reader = comando.ExecuteReader();
                    reader.Read();
                    var resultado = reader.GetBoolean(0);
                    //Se estiver correto 
                    if(resultado)
                    {
                        //Cria o cookie
                        HttpCookie c = Response.Cookies.Get("info");
                        if (c == null)
                        {
                            //Se o cookie não existe, instancia ele e adiciona a lista de cookies
                            c = new HttpCookie("info");
                            Response.Cookies.Add(c);
                        }
                        c.Values["logado"] = "sim"; //é necessário?
                        c.Values["user"] = u.User;
                        c.Values["ultimaVisita"] = DateTime.Now.ToString();
                        c.Expires = DateTime.Now.AddDays(3);
                        Response.Cookies.Set(c);    //Altera o cookie
                    }
                    else
                    {
                        ModelState.AddModelError("senha", "Senha incorreta");
                        conn.Close();
                        return View();
                    }
                }
                finally
                {
                    conn.Close();
                }
                return Redirect("/Ferramenta/Home");


            }
            else
                //Senão, exibe a pagina dnv.
                return View();
        }
        [HttpPost]
        public ActionResult Cadastrar(Usuario u)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection conn = new MySqlConnection("server=localhost;database=progweb;uid=root;pwd=");
                try
                {
                    conn.Open();
                    var comando = conn.CreateCommand();
                    comando.CommandText = "INSERT INTO usuarios(email,usuario,senha) VALUE (@email, @usuario, @senha);";
                    comando.Parameters.AddWithValue("@email", u.Email);
                    comando.Parameters.AddWithValue("@usuario", u.User);
                    comando.Parameters.AddWithValue("@senha", u.Senha);
                    comando.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
                //Cadastra o usuário no BD
                return Redirect("/Ferramenta/Home");
            }
            else
            {
                //Volta para a mesma página e exibe as mensagens de erro.
                return View();
            }
        }

    }
}