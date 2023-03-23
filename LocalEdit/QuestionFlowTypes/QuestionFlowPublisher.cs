using LocalEdit.FlowTypes;
using LocalEdit.Shared;
using System.Text;

namespace LocalEdit.QuestionFlowTypes
{
    public class QuestionFlowPublisher
    {
        public static string Publish(QuestionFlowDocument QuestionFlow)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(MermaidHeader(QuestionFlow));

            // go through and get all of the questions created
            foreach (var item in QuestionFlow.items)
            {
                //item = workspace.items[itmNum];
                sb.Append(MermaidItem(item));
            }

            // NOTE: Will need to make sure default flows are created.  See FlowViz for details

            // go through again and add all of the connections
            foreach (var item in QuestionFlow.items)
            {
                if (item.linkLogic != null)
                {
                    foreach (LinkLogic linkLogic in item.linkLogic)
                    {
                        QuestionFlowRelationship rel = new();
                        rel.From = Utils.VOD(item.id);
                        rel.To = linkLogic.jumpToItemId;
                        rel.Label = linkLogic.asString;
                        sb.Append(MermaidConnection(rel));

                        //rtnVal.Relationships.Add(new FlowRelationship { From = Utils.VOD(itmFlow.id), To = Utils.VOD(linkLogic.jumpToItemId), Label = linkLogic.ToString().Trim().Replace("\r\n", "<br/>") });
                    }
                }
                //if (item.NextQuestions != null)
                //{
                //    foreach (var rel in item.NextQuestions)
                //    {
                //        rel.From = Utils.VOD(item.id);
                //        sb.Append(MermaidConnection(rel));
                //    }
                //}
            }

            return sb.ToString();
        }

        //private static FlowDocument ToFlowDocument(QuestionFlowDocument flow)
        //{
        //    FlowDocument rtnVal = new FlowDocument();

        //    QuestionFlowItem? previousItem = null;
        //    QuestionFlowItem? previousUnconditional = null;

        //    if (flow == null)
        //        return rtnVal;

        //    if (flow.items != null)
        //    {
        //        foreach (QuestionFlowItem itmFlow in flow.items)
        //        {
        //            rtnVal.Items.Add(new FlowItem { ID = itmFlow.id, Description = "", Label = Utils.VOD(itmFlow.title) });

        //            if (previousItem != null)
        //            {
        //                if ((itmFlow.flowEntryLogic == null) || (itmFlow.flowEntryLogic.Count == 0))
        //                {
        //                    rtnVal.Relationships.Add(new FlowRelationship { From = Utils.VOD(previousItem.id), To = Utils.VOD(itmFlow.id), Label = " " });
        //                }
        //                else
        //                {
        //                    foreach (object obj in itmFlow.flowEntryLogic)
        //                    {
        //                        rtnVal.Relationships.Add(new FlowRelationship { From = Utils.VOD(previousItem.id), To = Utils.VOD(itmFlow.id), Label = obj.ToString().Trim().Replace("\r\n", "<br/>") });
        //                    }
        //                }
        //            }

        //            if ((previousUnconditional != null) && (previousUnconditional != previousItem))
        //            {
        //                rtnVal.Relationships.Add(new FlowRelationship { From = Utils.VOD(previousUnconditional.id), To = Utils.VOD(itmFlow.id), Label = "Otherwise" });
        //            }

        //            if (itmFlow.flowEntryLogic == null)
        //            {
        //                previousUnconditional = itmFlow;
        //            }

        //            previousItem = itmFlow;

        //            if (itmFlow.linkLogic != null)
        //            {
        //                foreach (LinkLogic linkLogic in itmFlow.linkLogic)
        //                {
        //                    rtnVal.Relationships.Add(new FlowRelationship { From = Utils.VOD(itmFlow.id), To = Utils.VOD(linkLogic.jumpToItemId), Label = linkLogic.ToString().Trim().Replace("\r\n", "<br/>") });
        //                }

        //                previousItem = null;
        //            }

        //        }
        //    }
        //    return rtnVal;
        //}

        private static string MermaidHeader(QuestionFlowDocument flow)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("graph TD");
            // classDef borderless stroke-width:0px
            // classDef darkBlue fill:#00008B, color:#fff
            // classDef brightBlue fill:#6082B6, color:#fff
            // classDef gray fill:#62524F, color:#fff
            // classDef gray2 fill:#4F625B, color:#fff

            // ");

            return sb.ToString();
        }

        private static string BuildIndentation(int level)
        {
            string rtnVal = "";

            for (var i = 0; i < (4 * level); i++)
            {
                rtnVal = rtnVal + " ";
            }
            return rtnVal;
        }

        private static string MermaidItem(QuestionFlowItem item, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

            // https://bobbyhadz.com/blog/javascript-typeerror-replaceall-is-not-a-function
            string brokenLabel = String.Join("<br/>", item.title.Split("`"));

            brokenLabel = $"\"{brokenLabel}\"";

            sb.AppendLine(@$"{indentation}{item.id}[{brokenLabel}]");

            return sb.ToString();
        }

        private static string MermaidConnection(QuestionFlowRelationship rel, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

            string from = rel.From;
            string to = rel.To;

            sb.AppendLine($"{indentation}{from}--\"{rel.Label}\"-->{to}");

            return sb.ToString();
        }


    }
}
