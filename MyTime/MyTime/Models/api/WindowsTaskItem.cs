namespace MyTime.Models.api
{
    [Serializable]

    public class WindowsTaskItem
    {
        public string TaskName { get; set; }
        public string ExePath { get; set; }
        public int PID { get; set; }
        public string Status { get; set; }
    }
}
