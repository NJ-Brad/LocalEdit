using System.Text;
namespace LocalEdit.C4Types
{
    public class C4Publisher
    {
        public static string Publish(C4Workspace workspace)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append(MermaidHeader(workspace));

            //foreach (var item in workspace.Items)
            //{
            //    //item = workspace.items[itmNum];
            //    sb.Append(MermaidItem(item));
            //}

            //foreach (var rel in workspace.Relationships)
            //{
            //    sb.Append(MermaidConnection(rel));
            //}

            return sb.ToString();
        }
    }
}
