using LocalEdit.FlowTypes;
using LocalEdit.SequenceTypes;

namespace LocalEdit.LpeTypes
{
    public class LpeConverter
    {
        public static SequenceDocument ToSequenceDocument(Root flow)
        {
            SequenceDocument rtnVal = new SequenceDocument();

            ItemSequence previousItem = null;

            foreach (ItemSequence itmFlow in flow.itemSequence)
            {
                rtnVal.Items.Add(new SequenceItem { /*ID = itmFlow.itemName, */Description = "", ItemType = SequenceItemType.Question, Label = itmFlow.title });

                if (previousItem != null)
                {
                    if (itmFlow.entryLogic == null)
                    {
                        rtnVal.Relationships.Add(new SequenceRelationship { From = previousItem.itemName, To = itmFlow.itemName, Label = " " });
                    }
                    else
                    {
                        rtnVal.Relationships.Add(new SequenceRelationship { From = previousItem.itemName, To = itmFlow.itemName, Label = itmFlow.entryLogic.ToString().Trim().Replace("\r\n", "<br/>") });
                    }
                }
                previousItem = itmFlow;
            }

            return rtnVal;
        }
    }
}
