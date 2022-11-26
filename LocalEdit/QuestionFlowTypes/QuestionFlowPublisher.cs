using Blazorise;
using System.Text;

namespace LocalEdit.QuestionFlowTypes
{
    public class QuestionFlowPublisher
    {
        public static string Publish(QuestionFlowDocument QuestionFlow)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(MermaidHeader(QuestionFlow));

            foreach (var item in QuestionFlow.Items)
            {
                //item = workspace.items[itmNum];
                sb.Append(MermaidItem(item));
            }

            //foreach (var rel in QuestionFlow.Relationships)
            //{
            //    sb.Append(MermaidConnection(rel));
            //}

            return sb.ToString();
        }

        private static string MermaidHeader(QuestionFlowDocument QuestionFlow)
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("graph TD");

            sb.AppendLine("%% Created by LocalEdit");
            sb.AppendLine("QuestionFlowDiagram");

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

            sb.AppendLine(@$"{indentation}participant {item.Label}");

            return sb.ToString();
        }

        private static string MermaidConnection(QuestionFlowRelationship rel, int indent = 1)
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