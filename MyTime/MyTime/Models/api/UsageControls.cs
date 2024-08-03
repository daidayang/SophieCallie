namespace MyTime.Models.api
{
    [Serializable]
    public class UsageControls
    {
        public List<UsageControl> Controls { get; set; }
        public List<MyTimeTaskItem> Tasks { get; set; }
    }
}
