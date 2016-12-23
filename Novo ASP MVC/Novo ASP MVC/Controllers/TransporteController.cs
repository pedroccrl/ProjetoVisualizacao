using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Novo_ASP_MVC.Controllers.Transporte
{
    public class TransporteController : Controller
    {
        // GET: Transporte
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Brt()
        {
            return View();
        }

        public ActionResult LinhaOnibus()
        {
            return View();
        }
    }
}