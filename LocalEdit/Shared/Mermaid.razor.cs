using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LocalEdit.Shared
{
    public partial class Mermaid : ComponentBase
    {
        // https://stackoverflow.com/questions/58346600/why-do-blazor-components-and-elements-not-have-id-attributes
        [Parameter] public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public async Task TriggerClick()
        {
            var input2 = "graph LR \n" +
                "A[Brad] --- B[Load Balancer] \n" +
                "B-->C[Server01] \n" +
                "B-->D(Server02) \n";

            //            await JSRuntime.InvokeVoidAsync("renderSampleMermaidDiagram");
            await JSRuntime.InvokeVoidAsync("renderMermaidDiagram", this.Id, input2);
            //await JSRuntime.InvokeVoidAsync("renderMermaidDiagram2", "output");
        }

        public async Task DisplayDiagram(string input)
        {
            //var input2 = "graph LR \n" +
            //    "A[Brad] --- B[Load Balancer] \n" +
            //    "B-->C[Server01] \n" +
            //    "B-->D(Server02) \n";

            var input2 = "C4Context\n Person_Ext(CUSTOMER, \"Customer\", \"A customer of the bank, with personal bank accounts\")\n";


//            await JSRuntime.InvokeVoidAsync("MermaidInitialize");
                                    await JSRuntime.InvokeVoidAsync("renderMermaidDiagram", this.Id, input);
//                        await JSRuntime.InvokeVoidAsync("renderMermaidDiagram", this.Id, input2);

            //var input2 = "graph LR \n" +
            //    "A[Client] --- B[Load Balancer] \n" +
            //    "B-->C[Server01] \n" +
            //    "B-->D(Server02) \n";


            //JSRuntime.InvokeVoidAsync("renderMermaid", this.Id, input2);

            //ChildContent = $"<h1>{input}</h1>";

            //ChildContent = AddContent($"<h1>{input}</h1>");
            //ChildContent = AddContent2(input);

            InvokeAsync(() => StateHasChanged());

        }
        private RenderFragment AddContent(string textContent) => builder =>
        {
            builder.AddContent(1, textContent);
        };

        private RenderFragment AddContent2(string textContent) => builder =>
        {
            builder.OpenElement(0, "h1");
            builder.AddContent(1, textContent);
            builder.CloseElement();
        };

        //protected override void OnInitialized()
        //{
        //    childContent = AddContent();
        //}
    }
}
