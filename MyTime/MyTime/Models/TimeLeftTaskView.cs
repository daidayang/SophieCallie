namespace MyTime.Models
{
    public class TimeLeftTaskView
    {
        public List<TimeLeft> Tasks { get; set; }
        public string SelectedTaskId { get; set; }
        public TimeLeft SelectedTask => Tasks?.FirstOrDefault(t => t.Name == SelectedTaskId);
        public string ButtonText => SelectedTask?.State == true ? "Stop" : "Start";
    }
}
