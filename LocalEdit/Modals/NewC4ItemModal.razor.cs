using Blazorise;
using LocalEdit.C4Types;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class NewC4ItemModal : ComponentBase
    {
        C4Item? newItem = null;

        private Modal? newItemModalRef;

        //private bool cancelClose;

        C4Item? potentialParentNode = null;
        private bool newItemModalVisible;
        C4TypeEnum? newItemType;

        private Task CreateItem(C4TypeEnum itemType)
        {
            newItemType = itemType;

            newItem = new C4Item() { ItemType = itemType };
            //            selectedNode = newItem;

            return newItemModalRef.Close(CloseReason.EscapeClosing);
        }

        private bool ShouldShow(C4TypeEnum itemType, C4Item? parentNode)
        {
            bool rtnVal = false;

            if (parentNode == null)
            {
                switch (itemType)
                {
                    case C4TypeEnum.Person:
                    case C4TypeEnum.System:
                    case C4TypeEnum.EnterpriseBoundary:
                        rtnVal = true;
                        break;
                }
            }
            else
            {
                switch (parentNode.ItemType)
                {
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
            }

            return rtnVal;

        }

        private Task ShowNewItemModal(C4Item parentNode)
        {
//            potentialParentNode = parentNode;

            newItemModalVisible = true;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task CloseNewItemModal()
        {
            newItemType = null;

            return newItemModalRef.Close(CloseReason.EscapeClosing);
        }

        private Task OnNewItemModalClosed()
        {
            if (newItemType != null)
            {
//                ShowModal();
            }
            return Task.CompletedTask;
        }

        private Task OnNewItemModalOpened()
        {
            //// reset, for the next attempt to close
            //cancelClose = false;
            ////            propEditor.ResetValidation();
            //cancelled = false;

            return Task.CompletedTask;
        }


        private Task OnNewItemModalClosing(ModalClosingEventArgs e)
        {
            // just set Cancel to prevent modal from closing

            //if (e.CloseReason == CloseReason.EscapeClosing)
            //{
            //    CloseModal();
            //}

            //if (cancelClose || e.CloseReason != CloseReason.UserClosing)
            //{
            //    e.Cancel = true;
            //}

            return Task.CompletedTask;
        }

    }
}
