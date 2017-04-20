using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace Novo_ASP_MVC.Models
{
    public class Veiculo
    {
        public string Codigo { get; set; }
        public string Linha { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float Latitude { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float Longitude { get; set; }
        public string DataHora { get; set; }
        public string Velocidade { get; set; }
    }
}