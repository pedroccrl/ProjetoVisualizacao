using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcharCentroRegiao
{
    public class Geometry
    {
        public string type { get; set; }
        public List<List<List<double[]>>> coordinates { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
        public int Id { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class GeoJson
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }
}
