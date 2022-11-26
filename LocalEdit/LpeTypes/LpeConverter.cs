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
            ItemSequence previousUnconditional = null;

            foreach (ItemSequence itmFlow in flow.itemFlow)
            {
                rtnVal.Items.Add(new SequenceItem { /*ID = itmFlow.itemName, */Description = "", Label = itmFlow.title });

                if (previousItem != null)
                {
                    if (itmFlow.entryLogic == null)
                    {
                        rtnVal.Relationships.Add(new SequenceRelationship { From = previousItem.title, To = itmFlow.title, Label = " " });
                    }
                    else
                    {
                        rtnVal.Relationships.Add(new SequenceRelationship { From = previousItem.title, To = itmFlow.title, Label = itmFlow.entryLogic.ToString().Trim().Replace("\r\n", "<br/>") });
                    }
                }

                if ((previousUnconditional != null) && (previousUnconditional != previousItem))
                {
                    rtnVal.Relationships.Add(new SequenceRelationship { From = previousUnconditional.title, To = itmFlow.title, Label = "Otherwise" });
                }

                if (itmFlow.entryLogic == null)
                {
                    previousUnconditional = itmFlow;
                }

                previousItem = itmFlow;
            }

            return rtnVal;
        }
    }
}
