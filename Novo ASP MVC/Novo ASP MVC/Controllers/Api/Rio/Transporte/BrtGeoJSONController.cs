using System.Net.Http;
using System.Web;
using System.Web.Http;

using Novo_ASP_MVC.Models.GeoJson;
using Novo_ASP_MVC.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Novo_ASP_MVC.Controllers.Api.Rio
{
    /// <summary>
    /// Conjuntos de dados relacionados ao serviço de tempo real que informa as posições dos veículos pertencentes ao sistema BRT, incluindo linhas alimentadoras. 
    /// <para>http://data.rio/dataset/brt-gps</para>
    /// </summary>
    [Route("api/rio/transporte/brt")]
    public class BrtGeoJSONController : ApiController
    {
        [HttpGet]
        public async Task<GeoJsonFeatureCollection<Veiculo>> Get()
        {
            string[] linhas = new string[] { };
            var gc = new GeoJsonFeatureCollection<Veiculo>();
            var ip = HttpContext.Current.Request.UserHostAddress;
            HttpClient client = new HttpClient();
            var r = await client.GetAsync("http://webapibrt.rio.rj.gov.br/api/v1/brt");
            var json = await r.Content.ReadAsStringAsync();
            var vs = JsonConvert.DeserializeObject<Models.Provider.DataRio.BrtVeiculos>(json);

            foreach (var item in vs.veiculos)
            {
                var veic = new Veiculo
                {
                    Codigo = item.codigo,
                    Linha = item.linha.ToString(),
                    DataHora = item.datahora.ToString(),
                    Velocidade = item.velocidade.ToString()
                };
                var coord = new Coordenada { Latitude = item.latitude, Longitude = item.longitude };

                gc.Add(veic, coord);
            }
            return gc;
        }

        

    }
}
