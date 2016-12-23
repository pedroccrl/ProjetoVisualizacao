using Novo_ASP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Novo_ASP_MVC.Controllers.Ferramenta
{
    public class FerramentaController : Controller
    {
        // GET: Ferramenta
/*        public ActionResult Home()
        {
            ViewBag.dominio = "get";
            return View();
        }
        */
        public ActionResult Home(Dataset d)
        {
            if(d != null)
            {
                ViewBag.dominio = d.Dominio;
                ViewBag.id = d.ID;
            }
            return View();
        }
        

    }
}