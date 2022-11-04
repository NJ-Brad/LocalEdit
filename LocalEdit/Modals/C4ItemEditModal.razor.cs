using Blazorise;
using LocalEdit.C4Types;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class C4ItemEditModal : ComponentBase
    {
        private Modal? modalRef;
        C4Item selectedNode = new();
        private bool cancelClose;
        private bool modalVisible;
        private bool cancelled = false;
        PropertyEditor? propEditor;

        private Task OnModalClosing(ModalClosingEventArgs e)
        {
            // just set Cancel to prevent modal from closing

            if (e.CloseReason == CloseReason.EscapeClosing)
            {
                CloseModal();
            }

            if (cancelClose || e.CloseReason != CloseReason.UserClosing)
            {
                e.Cancel = true;
            }

            return Task.CompletedTask;
        }

        private Task CloseModal()
        {
            // possibly add a chack for changed and prompt to lose changes

            cancelClose = false;
            //cancelled = true;

            return modalRef.Hide();
        }

        private Task OnModalOpened()
        {
            // reset, for the next attempt to close
            cancelClose = false;
            //            propEditor.ResetValidation();
            cancelled = false;

            return Task.CompletedTask;
        }
        private async Task TryCloseModal()
        {
            // add a check for validity

            cancelClose = true;

            if (await propEditor.IsValid())
            {
                cancelClose = false;
            }

            await modalRef.Hide();
        }
    }
}
