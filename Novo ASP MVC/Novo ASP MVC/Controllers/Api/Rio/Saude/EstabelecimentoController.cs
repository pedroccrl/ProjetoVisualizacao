using Newtonsoft.Json;
using Novo_ASP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Novo_ASP_MVC.Controllers.Api.Rio.Saude
{
    [Route("api/rio/saude/estabelecimentos")]
    public class EstabelecimentoController : ApiController
    {
        public async Task<string> Get()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            var req = await http.GetAsync("http://dadosabertos.rio.rj.gov.br/apiSaude/apresentacao/rest/index.cfm/estabelecimentos");
            var json = await req.Content.ReadAsStringAsync();
            var csv = JsonConvert.DeserializeObject<CsvJson>(json);

            foreach (var item in csv.DATA)
            {

            }

            return null;
        }
    }
}