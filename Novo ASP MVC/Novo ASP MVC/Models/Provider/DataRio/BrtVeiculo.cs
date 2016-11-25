using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Novo_ASP_MVC.Models.Provider.DataRio
{
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