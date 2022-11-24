using Blazorise;
using System.Text;

namespace LocalEdit.SequenceTypes
{
    public class SequencePublisher
    {
        public static string Publish(SequenceDocument Sequence)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(MermaidHeader(Sequence));

            foreach (var item in Sequence.Items)
            {
                //item = workspace.items[itmNum];
                sb.Append(MermaidItem(item));
            }

            foreach (var rel in Sequence.Relationships)
            {
                sb.Append(MermaidConnection(rel));
            }

            return sb.ToString();
        }

        private static string MermaidHeader(SequenceDocument Sequence)
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("graph TD");

            sb.AppendLine("%% Created by LocalEdit");
            sb.AppendLine("sequenceDiagram");

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

        private static string MermaidItem(SequenceItem item, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

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
                case SequenceItemType.Question:
                    //case "ACTION":
                    //sb.AppendLine(@$"{ indentation}{ item.ID}[{ brokenLabel}]");

                    //sb.AppendLine(@$"{indentation}{item.ID}[{brokenLabel}]");
                    sb.AppendLine(@$"{indentation}participant {item.Label}");
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

        private static string MermaidConnection(SequenceRelationship rel, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

            string from = rel.From;
            string to = rel.To;

            sb.AppendLine($"{indentation}{from}->>{to}: {rel.Label}");

            // solid
            // Alice->> John: Hello John, how are you?

            // dotted
            //John-- >> Alice: Great!

            return sb.ToString();
        }


    }
}