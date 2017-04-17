using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Novo_ASP_MVC.Models
{
    public class CsvJson
    {
        public List<string> COLUMNS { get; set; }
        public List<List<string>> DATA { get; set; }
    }
}