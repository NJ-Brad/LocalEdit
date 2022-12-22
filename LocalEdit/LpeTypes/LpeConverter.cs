using LocalEdit.FlowTypes;
using LocalEdit.SequenceTypes;
using LocalEdit.Shared;

namespace LocalEdit.LpeTypes
{
    public class LpeConverter
    {
        public static SequenceDocument ToSequenceDocument(Root flow)
        {
            SequenceDocument rtnVal = new SequenceDocument();

            ItemSequence? previousItem = null;
            ItemSequence? previousUnconditional = null;

            if (flow == null)
                return rtnVal;

            if (flow.ItemFlow != null)
            {
                foreach (ItemSequence itmFlow in flow.ItemFlow)
                {
                    rtnVal.Items.Add(new SequenceItem { /*ID = itmFlow.itemName, */Description = "", Label = Utils.VOD(itmFlow.Title) });

                    if (previousItem != null)
                    {
                        if (itmFlow.EntryLogic == null)
                        {
                            rtnVal.Relationships.Add(new SequenceRelationship { From = Utils.VOD(previousItem.Title), To = Utils.VOD(itmFlow.Title), Label = " " });
                        }
                        else
                        {
                            rtnVal.Relationships.Add(new SequenceRelationship { From = Utils.VOD(previousItem.Title), To = Utils.VOD(itmFlow.Title), Label = itmFlow.EntryLogic.ToString().Trim().Replace("\r\n", "<br/>") });
                        }
                    }

                    if ((previousUnconditional != null) && (previousUnconditional != previousItem))
                    {
                        rtnVal.Relationships.Add(new SequenceRelationship { From = Utils.VOD(previousUnconditional.Title), To = Utils.VOD(itmFlow.Title), Label = "Otherwise" });
                    }

                    if (itmFlow.EntryLogic == null)
                    {
                        previousUnconditional = itmFlow;
                    }

                    previousItem = itmFlow;
                }
            }
            return rtnVal;
        }
    }
}
