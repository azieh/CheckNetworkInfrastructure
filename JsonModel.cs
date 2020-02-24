using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ServiceChecker
{
    public partial class JsonModel
    {
        [JsonProperty("listenPorts")]
        public int[] ListenPorts { get; set; }

        [JsonProperty("pingList")]
        public Dictionary<string, int[]> PingList { get; set; }
    }

    public partial class JsonModel
    {
        public static JsonModel FromJson(string json) => JsonConvert.DeserializeObject<JsonModel>(json, ServiceChecker.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this JsonModel self) => JsonConvert.SerializeObject(self, ServiceChecker.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
