using Blazorise;
using LocalEdit.C4Types;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class C4ItemEditModal : LE_ModalBase
    {
        //private Modal? modalRef;
        public C4TypeEnum ParentType { get; set; } = C4TypeEnum.Unknown;
        public C4Item SelectedNode { get; set; } = new ();
        //private bool cancelClose;
        //private bool modalVisible;
        //private bool cancelled = false;

        //private Task OnModalClosing(ModalClosingEventArgs e)
        //{
        //    // just set Cancel to prevent modal from closing

        //    if (e.CloseReason == CloseReason.EscapeClosing)
        //    {
        //        CloseModal();
        //    }

        //    if (cancelClose || e.CloseReason != CloseReason.UserClosing)
        //    {
        //        e.Cancel = true;
        //    }

        //    return Task.CompletedTask;
        //}

        //private Task CloseModal()
        //{
        //    // possibly add a chack for changed and prompt to lose changes

        //    cancelClose = false;
        //    //cancelled = true;

        //    return modalRef.Hide();
        //}

        Alert myAlert;

        private bool ShouldShow(C4TypeEnum itemType)
        {
            bool rtnVal = false;

            switch (ParentType)
            {
                case C4TypeEnum.Unknown:
                    switch (itemType)
                    {
                        case C4TypeEnum.Person:
                        case C4TypeEnum.System:
                        case C4TypeEnum.EnterpriseBoundary:
                            rtnVal = true;
                            break;
                    }
                    break;
                case C4TypeEnum.Person:
                    // nothing is allowed
                    break;
                case C4TypeEnum.System:
                    switch (itemType)
                    {
                        case C4TypeEnum.Container:
                        case C4TypeEnum.Database:
                            rtnVal = true;
                            break;
                    }
                    break;
                case C4TypeEnum.EnterpriseBoundary:
                    switch (itemType)
                    {
                        case C4TypeEnum.Person:
                        case C4TypeEnum.System:
                            rtnVal = true;
                            break;
                    }
                    break;
                case C4TypeEnum.Container:
                    switch (itemType)
                    {
                        case C4TypeEnum.Component:
                            rtnVal = true;
                            break;
                    }
                    break;
                }
            return rtnVal;
        }

        public override Task Opened()
        {
            if (ParentType == C4TypeEnum.Unknown)
            {
                myAlert.Show();
            }

            return Task.CompletedTask;
        }


        //private Task OnModalOpened()
        //{
        //    // reset, for the next attempt to close
        //    cancelClose = false;
        //    //            propEditor.ResetValidation();
        //    cancelled = false;

        //    return Task.CompletedTask;
        //}
        //private async Task TryCloseModal()
        //{
        //    // add a check for validity

        //    cancelClose = true;

        //    if (await propEditor.IsValid())
        //    {
        //        cancelClose = false;
        //    }

        //    await modalRef.Hide();
        //}

        bool showA = true;
        Validations? validations;
        //private bool isRegistrationSuccess = false;

        public async Task<bool> IsValid()
        {
            bool rtnVal = false;
            if (await validations.ValidateAll())
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public override async Task ResetValidation()
        {
            await validations.ClearAll();
        }
    }
}
