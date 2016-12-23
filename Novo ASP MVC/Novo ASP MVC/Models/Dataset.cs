using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Novo_ASP_MVC.Models
{
    public class Dataset
    {  
        [Required(ErrorMessage = "Dominio é necéssario")]
        public string Dominio { get; set; }

        [Required(ErrorMessage = "ID é necessário")]
        [StringLength(9,ErrorMessage = "Tamanho de ID incorreto")]
        public string ID { get; set; }

        public string Nome { get; set; }

        public Dataset()
        {

        }

    }
}