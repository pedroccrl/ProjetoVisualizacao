using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Novo_ASP_MVC.Models.GeoJson
{
    public class GeoJsonFeatureCollection<T>
    {
        [JsonProperty("type")]
        public string Type { get { return "FeatureCollection"; } }

        [JsonProperty("features")]
        public List<GeoJsonFeature<T>> Features { get; set; }

        public void Add(T obj, Coordenada coord)
        {
            if (Features == null) Features = new List<GeoJsonFeature<T>>();
            
            var geoJsonGeom = new GeoJsonGeometry(coord);
            var geoJsonFeat = new GeoJsonFeature<T>(obj, geoJsonGeom);

            Features.Add(geoJsonFeat);
        }
    }

    public class GeoJsonFeature <T>
    {
        [JsonProperty("type")]
        public string Type { get; private set; } = "Feature";

        [JsonProperty("properties")]
        public T Properties { get; set; }

        [JsonProperty("geometry")]
        public GeoJsonGeometry Geometry { get; set; }

        public GeoJsonFeature(T objeto)
        {
            Properties = objeto;
        }

        public GeoJsonFeature(T objeto, GeoJsonGeometry geoJsonGeom)
        {
            Properties = objeto;
            Geometry = geoJsonGeom;
        }
    }

    /// <summary>
    /// RFC: https://tools.ietf.org/html/rfc7946
    /// </summary>
    public class GeoJsonGeometry
    {
        /// <summary>
        /// GeoJSON supports the following geometry types: Point, LineString, Polygon, MultiPoint, and MultiLineString. 
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("coordinates")]
        public float[] Coordinates { get; set; }


        public GeoJsonGeometry(Coordenada coord, GeometryTypes geoType = GeometryTypes.Point)
        {
            if (geoType != GeometryTypes.Point) throw new Exception("Apenas se pode criar um ponto com uma coordenada.");

            Type = "Point";
            Coordinates = new float[] { coord.Latitude, coord.Longitude };
        }

        public GeoJsonGeometry(Coordenada[] coords, GeometryTypes geoType)
        {

        }
    }

    public enum GeometryTypes
    {
        Point,
        LineString,
        Polygon,
        MultiPoint,
        MultiLineString,
    }
}