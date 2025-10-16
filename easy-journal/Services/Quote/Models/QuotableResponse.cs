using System.Text.Json.Serialization;

namespace easy_journal.Servicess.Quote.Models
{
    public class QuotableResponse
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }
    }
}
