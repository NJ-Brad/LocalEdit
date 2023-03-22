namespace FlowViz.FlowTypes
{
    public class FlowDocument
    {
        public List<FlowItem> Items { get; set; }= new List<FlowItem>();
        public List<FlowRelationship> Relationships { get; set; }= new List<FlowRelationship>();
    }
}
