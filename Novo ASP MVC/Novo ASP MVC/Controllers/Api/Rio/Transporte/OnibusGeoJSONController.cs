using Novo_ASP_MVC.Models;
using Novo_ASP_MVC.Models.GeoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Novo_ASP_MVC.Controllers.Api.Rio
{
    [Route("api/rio/onibus")]
    public class OnibusGeoJSONController : ApiController
    {
        public async Task<GeoJsonFeatureCollection<Veiculo>> Get()
        {
            return null;
        }
    }
}