namespace LocalEdit.SequenceTypes
{
    public class SequenceItem
    {
        public SequenceItemType ItemType { get; set; } = SequenceItemType.Question;
        public string Label { get; set; } = "";
        public string Description { get; set; } = "";
        //public string ID { get; set; } = "";

        //// This is only used for Boundary type items
        //public List<SequenceItem> Children { get; set; }= new List<SequenceItem>();
    }
}
