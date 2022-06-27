using System.Text.Json;

namespace eStoreClient.Utilities
{
    public static class SerializerOptions
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public static JsonSerializerOptions CaseInsensitive = jsonSerializerOptions;
    }
}
