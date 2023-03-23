using LocalEdit.FlowTypes;
using LocalEdit.SequenceTypes;
using LocalEdit.Shared;

namespace LocalEdit.QuestionFlowTypes
{
    public class LpeConverter
    {
        public static SequenceDocument ToSequenceDocument(QuestionFlowDocument flow)
        {
            SequenceDocument rtnVal = new SequenceDocument();

            QuestionFlowItem? previousItem = null;
            QuestionFlowItem? previousUnconditional = null;

            if (flow == null)
                return rtnVal;

            if (flow.items != null)
            {
                foreach (QuestionFlowItem itmFlow in flow.items)
                {
                    rtnVal.Items.Add(new SequenceItem { /*ID = itmFlow.itemName, */Description = "", Label = Utils.VOD(itmFlow.title) });

                    if (previousItem != null)
                    {
                        if (itmFlow.flowEntryLogic == null)
                        {
                            rtnVal.Relationships.Add(new SequenceRelationship { From = Utils.VOD(previousItem.title), To = Utils.VOD(itmFlow.title), Label = " " });
                        }
                        else
                        {
                            rtnVal.Relationships.Add(new SequenceRelationship { From = Utils.VOD(previousItem.title), To = Utils.VOD(itmFlow.title), Label = itmFlow.flowEntryLogic.ToString().Trim().Replace("\r\n", "<br/>") });
                        }
                    }

                    if ((previousUnconditional != null) && (previousUnconditional != previousItem))
                    {
                        rtnVal.Relationships.Add(new SequenceRelationship { From = Utils.VOD(previousUnconditional.title), To = Utils.VOD(itmFlow.title), Label = "Otherwise" });
                    }

                    if (itmFlow.flowEntryLogic == null)
                    {
                        previousUnconditional = itmFlow;
                    }

                    previousItem = itmFlow;
                }
            }
            return rtnVal;
        }
        public static FlowDocument ToFlowDocument(QuestionFlowDocument flow)
        {
            FlowDocument rtnVal = new FlowDocument();

            QuestionFlowItem? previousItem = null;
            QuestionFlowItem? previousUnconditional = null;

            if (flow == null)
                return rtnVal;

            if (flow.items != null)
            {
                foreach (QuestionFlowItem itmFlow in flow.items)
                {
                    rtnVal.Items.Add(new FlowItem { ID = itmFlow.id, Description = "", Label = Utils.VOD(itmFlow.title) });

                    if (previousItem != null)
                    {
                        if ((itmFlow.flowEntryLogic == null) || (itmFlow.flowEntryLogic.Count == 0))
                        {
                            rtnVal.Relationships.Add(new FlowRelationship { From = Utils.VOD(previousItem.id), To = Utils.VOD(itmFlow.id), Label = " " });
                        }
                        else
                        {
                            foreach (object obj in itmFlow.flowEntryLogic)
                            { 
                                rtnVal.Relationships.Add(new FlowRelationship { From = Utils.VOD(previousItem.id), To = Utils.VOD(itmFlow.id), Label = obj.ToString().Trim().Replace("\r\n", "<br/>") });
                            }
                        }
                    }

                    if ((previousUnconditional != null) && (previousUnconditional != previousItem))
                    {
                        rtnVal.Relationships.Add(new FlowRelationship { From = Utils.VOD(previousUnconditional.id), To = Utils.VOD(itmFlow.id), Label = "Otherwise" });
                    }

                    if (itmFlow.flowEntryLogic == null)
                    {
                        previousUnconditional = itmFlow;
                    }

                    previousItem = itmFlow;

                    if (itmFlow.linkLogic != null)
                    {
                        foreach (LinkLogic linkLogic in itmFlow.linkLogic)
                        {
                            rtnVal.Relationships.Add(new FlowRelationship { From = Utils.VOD(itmFlow.id), To = Utils.VOD(linkLogic.jumpToItemId), Label = linkLogic.ToString().Trim().Replace("\r\n", "<br/>") });
                        }

                        previousItem = null;
                    }

                }
            }
            return rtnVal;
        }
    }
}
