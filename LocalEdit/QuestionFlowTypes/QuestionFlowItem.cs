using LocalEdit.SequenceTypes;
using System.Text.Json.Serialization;

namespace LocalEdit.QuestionFlowTypes
{
    public class QuestionFlowItem
    {
        public string? id { get; set; } = "";
        [JsonPropertyName("type")]
        public string? itemType { get; set; }
        public string? title { get; set; } = "";
        public List<object> queryLogic { get; set; } = new();
        public List<object> flowEntryLogic { get; set; } = new();
        public List<QuestionFlowRelationship>? NextQuestions { get; set; } = new List<QuestionFlowRelationship>();
        public List<LinkLogic> linkLogic { get; set; } = new List<LinkLogic>();

        //public string id { get; set; }
        //[JsonPropertyName("type")]
        //public string itemType { get; set; }
        //public string title { get; set; }
        //public List<object> queryLogic { get; set; }
        //public List<object> flowEntryLogic { get; set; }
        //public List<LinkLogic> linkLogic { get; set; }
    }
}
