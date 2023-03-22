namespace FlowViz.FlowTypes
{
    public class FlowItem
    {
        public FlowItemType ItemType { get; set; } = FlowItemType.Question;
        public string Label { get; set; } = "";
        public string Description { get; set; } = "";
        public string ID { get; set; } = "";

        //// This is only used for Boundary type items
        //public List<FlowItem> Children { get; set; }= new List<FlowItem>();
    }
}
