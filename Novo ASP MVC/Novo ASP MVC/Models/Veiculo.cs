using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Novo_ASP_MVC.Models
{
    public class Veiculo
    {
        public string Codigo { get; set; }
        public string Linha { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string DataHora { get; set; }
        public string Velocidade { get; set; }
    }
}