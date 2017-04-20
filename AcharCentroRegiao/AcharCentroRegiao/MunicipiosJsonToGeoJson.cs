using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcharCentroRegiao
{
    public class MunicipiosToGeoJson
    {
        List<Municipio> Municipios;

        public MunicipiosToGeoJson(List<Municipio> municipios)
        {
            Municipios = municipios;
        }

        public GeoJson Convert()
        {
            var geojson = new GeoJson();
            geojson.type = "FeatureCollection";
            geojson.features = new List<Feature>();

            int id = 0;

            foreach (var item in Municipios)
            {
                
                var feature = new Feature
                {
                    type = "Feature",
                    properties = new Properties { Id = id++ },

                };
                feature.geometry = new Geometry { type = "Polygon" };
                feature.geometry.coordinates = new List<List<List<double[]>>>();

                var ccc = new List<List<List<double[]>>>();
                var cc = new List<List<double[]>>();
                var c = new List<double[]>();
                foreach (var coords in item.Coordenadas)
                {
                    var lng = double.Parse(coords.Longitude, CultureInfo.InvariantCulture);
                    var lat = double.Parse(coords.Latitude, CultureInfo.InvariantCulture);
                    var coord = new double[] { lng, lat };
                    

                    

                    c.Add(coord);
                    

                    

                }
                cc.Add(c);
                feature.geometry.coordinates.Add(cc);
                geojson.features.Add(feature);
            }

            return geojson;
        }
    }
}
