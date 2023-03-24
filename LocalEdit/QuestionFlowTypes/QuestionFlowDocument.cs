using LocalEdit.LpeTypes;

namespace LocalEdit.QuestionFlowTypes
{
    public class QuestionFlowDocument
    {
        public QuestionFlowDocument()
        {
            headerConfig = new();
        }

        public bool hasFooter { get; set; }
        public bool hasHeader { get; set; }
        public QuestionFlowHeaderConfig? headerConfig { get; set; }
        public List<QuestionFlowItem> items { get; set; }= new List<QuestionFlowItem>();
    }
}
