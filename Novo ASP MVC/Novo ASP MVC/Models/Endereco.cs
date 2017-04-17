using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Novo_ASP_MVC.Models
{
    public class Endereco
    {
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Pais { get; set; }
    }
}