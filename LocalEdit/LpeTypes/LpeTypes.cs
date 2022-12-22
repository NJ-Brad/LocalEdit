using System.Text;
using System.Text.Json.Serialization;

// https://json2csharp.com/
namespace LocalEdit.LpeTypes
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class EntryLogic
    {
        public List<ValueCondition>? valueConditions { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (valueConditions != null)
            {
                foreach (ValueCondition condition in valueConditions)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("AND ");
                    }
                    sb.AppendLine($"{condition.ItemName} = {condition.ItemValue}");
                }
            }

            return sb.ToString();
        }
    }

    public class HeaderConfig
    {
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
        [JsonPropertyName("logo")]
        public string? Logo { get; set; }
    }

    public class ItemSequence
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("itemName")]
        public string? ItemName { get; set; }
        [JsonPropertyName("itemType")]
        public string? ItemType { get; set; }
        [JsonPropertyName("entryLogic")]
        public EntryLogic? EntryLogic { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("hasFooter")]
        public bool HasFooter { get; set; }
        [JsonPropertyName("hasHeader")]
        public bool HasHeader { get; set; }
        [JsonPropertyName("headerConfig")]
        public HeaderConfig? HeaderConfig { get; set; }
        [JsonPropertyName("itemFlow")]
        public List<ItemSequence>? ItemFlow { get; set; }
    }

    public class ValueCondition
    {
        [JsonPropertyName("itemName")]
        public string? ItemName { get; set; }
        [JsonPropertyName("itemValue")]
        public string? ItemValue { get; set; }
        [JsonPropertyName("answerType")]
        public string? AnswerType { get; set; }
    }

}
