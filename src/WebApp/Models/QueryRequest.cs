using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class QueryRequest
    {
        [JsonPropertyName("connectionString")]
        public string ConnectionString { get; set; }

        [JsonPropertyName("Query")]
        public string Query { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
