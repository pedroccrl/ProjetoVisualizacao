using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Novo_ASP_MVC.Models.Helper;
using System.Web.Http;

namespace Novo_ASP_MVC.Controllers.Api
{
    public class ViewsController : ApiController
    {
        public async Task<string> Get(string id)
        {
            id = id.Replace("@", ".").Replace("$", "/");
            var http = new HttpClient();
            var json = await http.GetStringAsync(new Uri("http://" + id));

            var jsonCol = new JsonColumns(json);

            var str = jsonCol.ToJsonString;
            return str;
        }
    }
}
