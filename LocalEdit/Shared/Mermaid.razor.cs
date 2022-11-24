using BlazorPanzoom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

        string SvgText { get; set; } = "";

        public async Task DisplayDiagram(string input)
        {
            //SvgText = await JSRuntime.InvokeAsync<string>("generateMermaidSvg", input);
            // https://stackoverflow.com/questions/60785749/using-svgs-in-blazor-page

             await JSRuntime.InvokeVoidAsync("renderMermaidDiagram", this.Id, input);

            InvokeAsync(() => StateHasChanged());

        }
        //private RenderFragment AddContent(string textContent) => builder =>
        //{
        //    builder.AddContent(1, textContent);
        //};

        //private RenderFragment AddContent2(string textContent) => builder =>
        //{
        //    builder.OpenElement(0, "h1");
        //    builder.AddContent(1, textContent);
        //    builder.CloseElement();
        //};

        private double _rangeValue = 1.0;

        private double RangeValue
        {
            get => _rangeValue;
            set
            {
                _rangeValue = value;
                _panzoom.ZoomAsync(value);
            }
        }

        private bool _panEnabled = true;

        //private bool PanEnabled
        //{
        //    get => _panEnabled;
        //    set
        //    {
        //        _panEnabled = value;
        //        _panzoom.SetOptionsAsync(new PanzoomOptions { DisablePan = !_panEnabled });
        //    }
        //}

        private Panzoom? _panzoom { get; set; }

        //private async Task OnZoomInClick(MouseEventArgs args)
        //{
        //    await _panzoom.ZoomInAsync();
        //    await UpdateSlider();
        //}

        //private async Task OnZoomOutClick(MouseEventArgs args)
        //{
        //    await _panzoom.ZoomOutAsync();
        //    await UpdateSlider();
        //}

        //private async Task OnResetClick(MouseEventArgs args)
        //{
        //    await _panzoom.ResetAsync();
        //    await UpdateSlider();
        //}

        //private async Task UpdateSlider()
        //{
        //    var scale = await _panzoom.GetScaleAsync();
        //    _rangeValue = scale;
        //}

    }
}
