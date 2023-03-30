using LocalEdit.FlowTypes;

namespace LocalEdit.CountDownTypes
{
    public class CountDownConfiguration
    {
        public TimeSpan? StartTime { get; set; } = LocalEdit.Shared.DateTimeExtension.ToRealLocalTime(DateTime.Now).TimeOfDay;// DateTime.Now.TimeOfDay;
        public TimeSpan? EndTime { get; set; } = LocalEdit.Shared.DateTimeExtension.ToRealLocalTime(DateTime.Now).TimeOfDay.Add(new TimeSpan(1, 0, 0)); //DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0));
        public int WarningMinutes { get; set; } = 5;
    }
}
