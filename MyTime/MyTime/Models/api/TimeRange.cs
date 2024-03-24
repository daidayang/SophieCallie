namespace MyTime.Models.api
{
    [Serializable]

    public class TimeRange
    {
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
    }
}
