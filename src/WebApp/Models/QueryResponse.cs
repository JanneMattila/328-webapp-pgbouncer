using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class QueryResponse
    {
        [JsonPropertyName("durations")]
        public List<double> Durations { get; set; }

        public QueryResponse()
        {
            Durations = new List<double>();
        }
    }
}
