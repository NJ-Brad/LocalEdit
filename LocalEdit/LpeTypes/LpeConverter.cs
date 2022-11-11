using LocalEdit.FlowTypes;

namespace LocalEdit.LpeTypes
{
    public class LpeConverter
    {
        public static FlowDocument ToFlowDocument(Root flow)
        {
            FlowDocument rtnVal = new FlowDocument();

            ItemFlow previousItem = null;

            foreach (ItemFlow itmFlow in flow.itemFlow)
            {
                rtnVal.Items.Add(new FlowItem { ID = itmFlow.itemName, Description = "", ItemType = FlowItemType.Question, Label = itmFlow.title });

                if (previousItem != null)
                {
                    if (itmFlow.entryLogic == null)
                    {
                        rtnVal.Relationships.Add(new FlowRelationship { From = previousItem.itemName, To = itmFlow.itemName, Label = " " });
                    }
                    else
                    {
                        rtnVal.Relationships.Add(new FlowRelationship { From = previousItem.itemName, To = itmFlow.itemName, Label = itmFlow.entryLogic.ToString().Trim().Replace("\r\n", "<br/>") });
                    }
                }
                previousItem = itmFlow;
            }

            return rtnVal;
        }
    }
}
