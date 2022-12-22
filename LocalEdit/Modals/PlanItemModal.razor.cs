using Blazorise;
using LocalEdit.PlanTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class PlanItemModal : LE_ModalBase
    {
        private string DecodePlanItemId(string id)
        {
            string rtnVal = id;

            foreach (PlanItem fi in Items)
            {
                if (fi.ID == id)
                {
                    rtnVal = fi.Label == null ? "" : fi.Label;
                    break;
                }
            }

            return rtnVal;
        }


        PlanDependencyModal? planDependencyModalRef = null;
        bool adding = false;

        PlanItemDependency? SelectedDependencyRow { get; set; } = new();

        private Task EditDependency()
        {
            if (SelectedDependencyRow == null)
            {
                return Task.CompletedTask;
            }

            if (planDependencyModalRef != null)
            {
                //planDependencyModalRef.Item = selectedDependencyRow;

                planDependencyModalRef?.ShowModal();
            }
            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewDependency()
        {
            PlanItemDependency newItem = new PlanItemDependency();

            SelectedDependencyRow = newItem;
            Item.Dependencies.Add(newItem);
            adding = true;

            return EditDependency();
        }

        private Task OnDependencyModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (planDependencyModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedDependencyRow != null)
                    {
                        Item.Dependencies.Remove(SelectedDependencyRow);
                        SelectedDependencyRow = null;
                    }
                }
            }
            adding = false;

            return Task.CompletedTask;
        }

        private Task DeleteDependency()
        {
            if (SelectedDependencyRow != null)
            {
                Item.Dependencies.Remove(SelectedDependencyRow);
                SelectedDependencyRow = null;
            }

            return Task.CompletedTask;
        }



        [Parameter]
        public PlanItem Item { get; set; } = new();

        [Parameter]
        public List<PlanItem> Items { get; set; } = new();
    }
}
