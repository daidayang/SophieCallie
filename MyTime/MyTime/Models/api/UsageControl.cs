namespace MyTime.Models.api
{
    [Serializable]

    public class UsageControl
    {
        public List<DateRange> DateRanges { get; set; }
        public List<ControlItem> ControlItems { get; set; }
    }
}
