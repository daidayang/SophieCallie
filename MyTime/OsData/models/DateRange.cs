using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace OsData.models
{
    [Serializable]
    [Flags]
    public enum MyDow
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64
    }

    [Serializable]
    public class DateRange
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MyDow DOW { get; set; }
        public List<TimeRange> TimeRanges { get; set; }
    }
}
