using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace eStoreClient.Models
{
    public class ODataModels<T>
    {
        [JsonPropertyName("value")]
        public List<T> List { get; set; }
    }
}
