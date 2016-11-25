using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Novo_ASP_MVC.Models.Provider.DataRio
{
    public static class LinhaOnibus
    {
        /// <summary>
        /// Retorna todas as linhas
        /// </summary>
        public static IEnumerable<Linha> GetAll()
        {
            List<Linha> linhas = new List<Linha>();
            foreach (var nome in Directory.GetFiles(HttpContext.Current.Server.MapPath("~/App_Data/data.rio/onibus/")))
            {
                linhas.Add(ParseCsvLinha(nome));
            }
            return linhas;
        }

        /// <summary>
        /// Retorna uma linha pelo nome
        /// </summary>
        /// <param name="nome"></param>
        public static void GetLinhaByName(string nome)
        {

        }

        static Linha ParseCsvLinha(string fileName)
        {
            var linhaOnibus = new Linha { Coordenadas = new List<Coordenada>() };
            var linhasArquivo = File.ReadAllLines(fileName);
            for (int i = 1; i < linhasArquivo.Length; i++)
            {
                var campos = linhasArquivo[i].Split(',');
                var form = campos[5].Replace("\"", "");
                linhaOnibus.Coordenadas.Add(new Coordenada { Latitude = float.Parse(campos[5].Replace("\"",""), System.Globalization.CultureInfo.InvariantCulture), Longitude = float.Parse(campos[6].Replace("\"", ""), System.Globalization.CultureInfo.InvariantCulture) });
                if (i > 1) continue;
                linhaOnibus.Nome = campos[0];
                linhaOnibus.Descricao = campos[1];
                linhaOnibus.Agencia = campos[2];
                linhaOnibus.ShapeId = int.Parse(campos[4]);
            }
            return linhaOnibus;
        }
    }
}