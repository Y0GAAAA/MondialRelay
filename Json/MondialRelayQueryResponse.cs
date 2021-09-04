using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MondialRelay.Json
{

    public class MondialRelayQueryResponse
    {

        public bool Success { get; set; }
        public string Message { get; set; }

        [JsonIgnore]
        public object[] ModelState { get; set; }
        [JsonIgnore]
        public string Action { get; set; }
        [JsonIgnore]
        public bool Data { get; set; }

        public static MondialRelayQueryResponse FromJsonString(string json)
        {
            return JsonSerializer.Deserialize<MondialRelayQueryResponse>(json);
        }

    }

}
