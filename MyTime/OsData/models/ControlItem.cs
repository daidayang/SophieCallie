using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace OsData.models
{
    public enum ControlItemType
    {
        WWW,
        APP
    }

    public class ControlItem
    {
        public string Name { get; set; }
        public string Identifier { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ControlItemType Type { get; set; }
    }
}
