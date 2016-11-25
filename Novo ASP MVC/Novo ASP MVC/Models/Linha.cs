using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Novo_ASP_MVC.Models
{
    public class Linha
    {
        /// <summary>
        /// Código da linha
        /// </summary>
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Agencia { get; set; }
        //public int Sequencia { get; set; }
        /// <summary>
        /// Id do shape (na verdade, não sei pra que serve, mas...)
        /// </summary>
        public int ShapeId { get; set; }
        /// <summary>
        /// Pontos dos trajetos
        /// </summary>
        public List<Coordenada> Coordenadas { get; set; }
    }
}