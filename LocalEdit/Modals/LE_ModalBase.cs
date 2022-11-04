using Blazorise;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public class LE_ModalBase : ComponentBase
    {
        protected Modal? modalRef;
        private bool cancelClose;
        protected bool modalVisible;
        //private bool cancelled = false;
        //private bool validationRequired = false;

        public ModalResult Result { get; private set; }


        [Parameter] public EventCallback Closed { get; set; }

        public Task ShowModal()
        {
            modalVisible = true;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        protected Task OnModalClosing(ModalClosingEventArgs e)
        {
            // just set Cancel to prevent modal from closing

            if (e.CloseReason == CloseReason.EscapeClosing)
            {
                Result = ModalResult.Cancel;

                CloseModal();
            }

            //            if (cancelClose || e.CloseReason != CloseReason.UserClosing)
            if (cancelClose)
            {
                e.Cancel = true;
            }

            // reset - This covers the case where a user clicks Save (and gets an error), then clicks escape
            //validationRequired = false;
            cancelClose = false;

            return Task.CompletedTask;
        }

        protected Task CloseModal()
        {
            // possibly add a chack for changed and prompt to lose changes

            cancelClose = false;
            //cancelled = true;
            Result = ModalResult.Cancel;

            modalRef.Hide();

            return Closed.InvokeAsync();
        }

        protected async Task OnModalOpened()
        {
            // reset, for the next attempt to close
            cancelClose = false;
            //cancelled = false;
            await ResetValidation();
        }

        public virtual async Task<bool> Validate()
        {
            return true;
        }

        public virtual async Task ResetValidation()
        {
//            return Task.CompletedTask;
        }

        protected async Task TryCloseModal()
        {
            // add a check for validity
            //validationRequired = true;

            Result = ModalResult.OK;
            cancelClose = true;

            //            if (await propEditor.IsValid())
            if (await Validate())
            {
                cancelClose = false;
            }

            modalRef.Hide();

            Closed.InvokeAsync();
        }
    }
}
