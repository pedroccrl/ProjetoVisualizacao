using Newtonsoft.Json;
using Novo_ASP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Novo_ASP_MVC.Controllers.Transporte
{
    public class BrtController : ApiController
    {
        // GET: api/Brt
        public async Task<IEnumerable<Veiculo>> Get()
        {
            var veiculos = new List<Veiculo>();
            HttpClient client = new HttpClient();
            var r = await client.GetAsync("http://webapibrt.rio.rj.gov.br/api/v1/brt");
            var json = await r.Content.ReadAsStringAsync();
            var vs = JsonConvert.DeserializeObject<BrtVeiculos>(json);
            foreach (var item in vs.veiculos)
            {
                veiculos.Add(new Veiculo
                {
                    Codigo = item.codigo,
                    Linha = item.linha.ToString(),
                    Latitude = item.latitude,
                    Longitude = item.longitude,
                    DataHora = item.datahora.ToString(),
                    Velocidade = item.velocidade.ToString()
                });
            }
            return veiculos;
        }

        // GET: api/Brt/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Brt
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Brt/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Brt/5
        public void Delete(int id)
        {
        }
    }


    public class BrtVeiculos
    {
        public BrtVeiculo[] veiculos { get; set; }
    }

    public class BrtVeiculo
    {
        public string codigo { get; set; }
        public object linha { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public long datahora { get; set; }
        public float velocidade { get; set; }
    }

}
