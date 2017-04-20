using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcharCentroRegiao
{
    class Program
    {
        static void Main(string[] args)
        {
            string kml = File.ReadAllText("Municipios_MG.kml");
            kml = kml.Replace("<coordinates>", "@");
            kml = kml.Replace("</coordinates>", "@");

            var coordenadasStr = kml.Split('@');

            List<Municipio> municipios = new List<Municipio>();

            // kml: longitude, latitude

            for (int i = 1; i < coordenadasStr.Length; i++)
            {
                if (i % 2 == 0) continue;

                var municipio = new Municipio();

                var coords = coordenadasStr[i].Split(' ');
                foreach (var latLonStr in coords)
                {
                    var latLon = latLonStr.Split(',');
                    municipio.Coordenadas.Add(new Coordenada(latLon[0], latLon[1]));
                }

                municipio.EncontraCentro();
                municipios.Add(municipio);
            }

            var geojson = new MunicipiosToGeoJson(municipios).Convert();

            File.WriteAllText("Municipios_MG.json", JsonConvert.SerializeObject(municipios));
            File.WriteAllText("Municipios_MG.geojson", JsonConvert.SerializeObject(geojson));
        }
        
    }

    public class Coordenada
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public Coordenada(string lng, string lat)
        {
            Longitude = lng;
            Latitude = lat;
        }
    }

    public class Municipio
    {
        public List<Coordenada> Coordenadas { get; set; }
        public Coordenada Centro { get; set; }
        public string Nome { get; set; }

        public Municipio()
        {
            Coordenadas = new List<Coordenada>();
        }

        public void EncontraCentro()
        {
            if (Coordenadas.Count == 0) throw new Exception("Impossível encontrar centro sem coordenadas.");
            var minLat = AchaMenorLatitude();
            var maxLat = AchaMaiorLatitude();
            var minLng = AchaMenorLongitude();
            var maxLng = AchaMaiorLongitude();
            
        }

        Coordenada AchaMenorLongitude()
        {
            float menor = 999999;
            Coordenada menorCoord = null;
            foreach (var item in Coordenadas)
            {
                float lng = float.Parse(item.Longitude, CultureInfo.InvariantCulture);
                if (lng < menor)
                {
                    menor = lng;
                    menorCoord = item;
                }
            }
            return menorCoord;
        }

        Coordenada AchaMaiorLongitude()
        {
            float maior = -999999;
            Coordenada maiorCoord = null;
            foreach (var item in Coordenadas)
            {
                float lng = float.Parse(item.Longitude, CultureInfo.InvariantCulture);
                if (lng > maior)
                {
                    maior = lng;
                    maiorCoord = item;
                }
            }
            return maiorCoord;
        }

        Coordenada AchaMenorLatitude()
        {
            float menor = 999999;
            Coordenada menorCoord = null;
            foreach (var item in Coordenadas)
            {
                float lng = float.Parse(item.Latitude, CultureInfo.InvariantCulture);
                if (lng < menor)
                {
                    menor = lng;
                    menorCoord = item;
                }
            }
            return menorCoord;
        }

        Coordenada AchaMaiorLatitude()
        {
            float maior = -999999;
            Coordenada maiorCoord = null;
            foreach (var item in Coordenadas)
            {
                float lng = float.Parse(item.Latitude, CultureInfo.InvariantCulture);
                if (lng > maior)
                {
                    maior = lng;
                    maiorCoord = item;
                }
            }
            return maiorCoord;
        }
    }
}
