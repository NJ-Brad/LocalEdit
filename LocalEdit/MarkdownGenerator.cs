namespace LocalEdit
{
    public class MarkdownGenerator
    {
        public static string WrapMermaid(string mermaidString)
        {
            return $@"```mermaid
{mermaidString}
```";
        }
    }
}
