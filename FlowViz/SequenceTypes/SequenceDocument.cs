namespace FlowViz.SequenceTypes
{
    public class SequenceDocument
    {
        public List<SequenceItem> Items { get; set; }= new List<SequenceItem>();
        public List<SequenceRelationship> Relationships { get; set; }= new List<SequenceRelationship>();
    }
}
