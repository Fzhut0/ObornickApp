using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace API.Helpers
{
    public partial class MessageConversationReader
    {
        [JsonProperty("data")]
        public MessageConversationReaderData[] Data { get; set; }
    }

    public partial class Participants
    {
        [JsonProperty("data")]
        public ParticipantsData[] Data { get; set; }
    }

    public partial class MessageConversationReaderData
    {
       [JsonProperty("participants")]
        public Participants Participants { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; } 
    }

    public partial class ParticipantsData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class MessageConversationReader
    {
        public static MessageConversationReader FromJson(string json) => JsonConvert.DeserializeObject<MessageConversationReader>(json, Converter.Settings);
    }

    
    public static class Serialize
    {
        public static string ToJson(this MessageConversationReader self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
    
}