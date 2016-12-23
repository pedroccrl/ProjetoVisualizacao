using Novo_ASP_MVC.Models;
using Novo_ASP_MVC.Models.Provider.DataRio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Novo_ASP_MVC.Controllers.Transporte.Onibus
{
    public class LinhaOnibusController : ApiController
    {
        // GET: Linha
        public IEnumerable<Linha> Get()
        {
            return LinhaOnibus.GetAll();
        }
    }
}