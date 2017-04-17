using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Novo_ASP_MVC.Models.Helper
{
    public class JsonColumns
    {
        [JsonProperty("columns", Required = Required.Always)]
        public List<Column> Columns { get; set; }

        public JsonColumns(string json)
        {
            Columns = new List<Column>();
            var reader = new JsonTextReader(new StringReader(json));

            while (reader.Read())
            {
                if (reader.Value != null && reader.TokenType == JsonToken.PropertyName)
                {
                    var name = reader.Value as string;
                    if (!Columns.Exists(c => c.fieldName == name))
                    {
                        var col = new Column
                        {
                            name = name.ToUpper(),
                            fieldName = name,
                            dataTypeName = reader.ValueType.Name
                        };
                        Columns.Add(col);
                    }
                }
            }
        }

        [JsonIgnore]
        public string ToJsonString
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Column
        {
            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int id { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string name { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string dataTypeName { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string fieldName { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int position { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string renderTypeName { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int tableColumnId { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int width { get; set; }
        }
    }


}