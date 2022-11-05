namespace LocalEdit.PlanTypes
{
    public class PlanDocument
    {
        public string Title { get; set; } = "";
        public string StartDate { get; set; } = "";
        public string BaseUrl { get; set; } = "https://";
        public List<PlanItem> Items { get; set; } = new List<PlanItem>();
    }
}
