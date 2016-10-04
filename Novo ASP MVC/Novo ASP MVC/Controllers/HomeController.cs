using Novo_ASP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Novo_ASP_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Map()
        {
            ViewBag.Teste = "Carregado.";
            ViewBag.Title = "Olaaa";
            return View();
        }

        public JsonResult GetAllItems()
        {
            var ps = new List<Produto>();
            ps.Add(new Produto { Nome = "lalala" });
            return Json(ps);
        }
    }
}