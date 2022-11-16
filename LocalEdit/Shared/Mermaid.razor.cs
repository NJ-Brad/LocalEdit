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

        public async Task DisplayDiagram(string input)
        {
            //await JSRuntime.InvokeVoidAsync("renderMermaidDiagram", this.Id, input);

            //JSRuntime.InvokeVoidAsync("remderMermaid", this.Id, input);

            //ChildContent = $"<h1>{input}</h1>";

            ChildContent = AddContent($"<h1>{input}</h1>");
            ChildContent = AddContent2(input);

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
