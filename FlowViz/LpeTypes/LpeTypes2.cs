using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// https://json2csharp.com/
namespace FlowViz.LpeTypes
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class HeaderConfig
    {
        public string phoneNumber { get; set; }
        public string logo { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        [JsonPropertyName("type")]
        public string itemType { get; set; }
        public string title { get; set; }
        public List<object> queryLogic { get; set; }
        public List<object> flowEntryLogic { get; set; }
        public List<LinkLogic> linkLogic { get; set; }
    }

    public class LinkLogic
    {
        public string jumpToItemId { get; set; }
        public string id { get; set; }
        public string value { get; set; }
        [JsonPropertyName("type")]
        public string linkType { get; set; }

        public override string ToString()
        {
            return $"{linkType} {value}";
        }
    }

    public class Root
    {
        public bool hasFooter { get; set; }
        public bool hasHeader { get; set; }
        public HeaderConfig headerConfig { get; set; }
        public List<Item> items { get; set; }
    }

}
