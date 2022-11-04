namespace LocalEdit.FlowTypes
{
    public class FlowDocument
    {
        List<FlowItem> Items { get; set; }= new List<FlowItem>();
        List<FlowRelationship> Relationships { get; set; }= new List<FlowRelationship>();
    }
}
