namespace LocalEdit.PlanTypes
{
    public class PlanItem
    {
        public string ID { get; set; }
        public string Label { get; set; }
        public string StoryId { get; set; }
        public string Duration { get; set; }
        public List<PlanItemDependency> Dependencies { get; set; } = new List<PlanItemDependency>();
    }
}
