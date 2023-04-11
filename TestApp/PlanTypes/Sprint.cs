namespace LocalEdit.PlanTypes
{
    public class Sprint
    {
        public string? ID { get; set; }
        public string Label { get; set; } = "";
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
