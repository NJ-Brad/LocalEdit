namespace LocalEdit.QuestionFlowTypes
{
    public class QuestionFlowDocument
    {
        public bool hasFooter { get; set; }
        public bool hasHeader { get; set; }
        public QuestionFlowHeaderConfig? headerConfig { get; set; }
        public List<QuestionFlowItem> Items { get; set; }= new List<QuestionFlowItem>();
    }
}
