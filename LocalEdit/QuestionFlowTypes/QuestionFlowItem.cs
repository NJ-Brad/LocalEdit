using LocalEdit.SequenceTypes;

namespace LocalEdit.QuestionFlowTypes
{
    public class QuestionFlowItem
    {
        public string? ID { get; set; } = "";
        public string? Label { get; set; } = "";
        public string? Description { get; set; } = "";
        public List<QuestionFlowRelationship>? NextQuestions { get; set; } = new List<QuestionFlowRelationship>();

    }
}
