namespace LocalEdit
{
    public class HtmlGenerator
    {
        public static string WrapMermaid(string mermaidString)
        {
            return $@"<!DOCTYPE html >
 <html lang = ""en"" >
    <head >
      <meta charset = ""utf-8"" />
     </head >
     <body >
       <pre class=""mermaid"">
{mermaidString}
"
+@"</pre>
    <script type = ""module"" >
      import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@9/dist/mermaid.esm.min.mjs';
            mermaid.initialize({startOnLoad: true });
    </script >
  </body >
</html >
";
        }


    }
}
