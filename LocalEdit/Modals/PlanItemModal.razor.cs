using Blazorise;
using LocalEdit.PlanTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class PlanItemModal : LE_ModalBase
    {
        Validations? validations;

        public override async Task<bool> Validate()
        {
            bool rtnVal = false;
            if (await validations.ValidateAll())
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        private string DecodePlanItemId(string id)
        {
            string rtnVal = id;

            foreach (PlanItem fi in Items)
            {
                if (fi.ID == id)
                {
                    rtnVal = fi.Label;
                    break;
                }
            }

            return rtnVal;
        }


        public override async Task ResetValidation()
        {
            await validations.ClearAll();
        }

        PlanDependencyModal planDependencyModalRef = null;
        bool adding = false;

        PlanItemDependency selectedDependencyRow = null;

        private Task EditDependency()
        {
            if (selectedDependencyRow == null)
            {
                return Task.CompletedTask;
            }
            planDependencyModalRef.Item = selectedDependencyRow;

            planDependencyModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewDependency()
        {
            PlanItemDependency newItem = new PlanItemDependency();

            selectedDependencyRow = newItem;
            Item.Dependencies.Add(newItem);
            adding = true;

            return EditDependency();
        }

        private Task OnDependencyModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (planDependencyModalRef.Result == ModalResult.Cancel)
                {
                    Item.Dependencies.Remove(selectedDependencyRow);
                    selectedDependencyRow = null;
                }
            }
            adding = false;

            return Task.CompletedTask;
        }




        [Parameter]
        public PlanItem Item { get; set; } = new();

        [Parameter]
        public List<PlanItem> Items { get; set; } = new();
    }
}
