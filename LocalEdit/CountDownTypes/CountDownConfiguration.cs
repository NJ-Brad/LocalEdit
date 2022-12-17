using LocalEdit.FlowTypes;

namespace LocalEdit.CountDownTypes
{
    public class CountDownConfiguration
    {
        public TimeSpan? StartTime { get; set; } = DateTime.Now.TimeOfDay;
        public TimeSpan? EndTime { get; set; } = DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0));
        public int WarningMinutes { get; set; } = 5;
    }
}
