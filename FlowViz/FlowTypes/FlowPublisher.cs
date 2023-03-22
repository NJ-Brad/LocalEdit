using System.Text;

namespace FlowViz.FlowTypes
{
    public class FlowPublisher
    {
        public static string Publish(FlowDocument flow)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(MermaidHeader(flow));

            foreach (var item in flow.Items)
            {
                //item = workspace.items[itmNum];
                sb.Append(MermaidItem(item));
            }

            foreach (var rel in flow.Relationships)
            {
                sb.Append(MermaidConnection(rel));
            }

            return sb.ToString();
        }

        private static string MermaidHeader(FlowDocument flow)
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

        private static string MermaidItem(FlowItem item, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

            // https://bobbyhadz.com/blog/javascript-typeerror-replaceall-is-not-a-function
            string brokenLabel = String.Join("<br/>", item.Label.Split("`"));

            brokenLabel = $"\"{brokenLabel}\"";

            switch (item.ItemType)
            {
                //case "BOUNDARY":
                //    if (item.items.length === 0)
                //    {
                //        // a boundary with nothing in it, should not be displayed
                //        // sb.appendLine(`${indentation}${item.id}[${item.label}]`);
                //    }
                //    else
                //    {
                //        sb.appendLine(`${ indentation}
                //        subgraph ${ item.id}
                //        [${ brokenLabel}]`);
                //        indent++;

                //        for (var item2 of item.items)
                //        {
                //            sb.appendLine(this.mermaidItem(item2, indent).trimEnd());
                //        }
                //        sb.appendLine(`${ indentation}
                //        end`);
                //    }
                //    break;
                case FlowItemType.Question:
                    //case "ACTION":
                    sb.AppendLine(@$"{ indentation}{ item.ID}[{ brokenLabel}]");
                    break;
                    //        case "DECISION":
                    //    sb.appendLine(`${ indentation}${ item.id}
                    //    {${ brokenLabel} }`);
                    //    break;
                    //case "START":
                    //case "END":
                    //    sb.appendLine(`${ indentation}${ item.id} ([${ brokenLabel}])`);
                    //    break;
                    //case "SUB":
                    //    sb.appendLine(`${ indentation}${ item.id}
                    //    [[${ brokenLabel}]]`);
                    //    break;
            }

            return sb.ToString();
        }

        private static string MermaidConnection(FlowRelationship rel, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

            string from = rel.From;
            string to = rel.To;

            sb.AppendLine($"{ indentation}{ from}--\"{rel.Label}\"-->{to}");

            return sb.ToString();
        }


    }
}