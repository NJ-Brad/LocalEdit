using System.Text;

// https://json2csharp.com/
namespace LocalEdit.LpeTypes
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class EntryLogic
    {
        public List<ValueCondition> valueConditions { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (ValueCondition condition in valueConditions)
            {
                if (sb.Length > 0)
                {
                    sb.Append("AND ");
                }
                sb.AppendLine($"{condition.itemName} = {condition.itemValue}");
            }

            return sb.ToString();
        }
    }

    public class HeaderConfig
    {
        public string phoneNumber { get; set; }
        public string logo { get; set; }
    }

    public class ItemFlow
    {
        public string title { get; set; }
        public string itemName { get; set; }
        public string itemType { get; set; }
        public EntryLogic entryLogic { get; set; }
    }

    public class Root
    {
        public bool hasFooter { get; set; }
        public bool hasHeader { get; set; }
        public HeaderConfig headerConfig { get; set; }
        public List<ItemFlow> itemFlow { get; set; }
    }

    public class ValueCondition
    {
        public string itemName { get; set; }
        public string itemValue { get; set; }
        public string answerType { get; set; }
    }

}
