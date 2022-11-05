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

        PlanItemDependency selectedDependencyRow = null;

        [Parameter]
        public PlanItem Item { get; set; } = new();

        [Parameter]
        public List<PlanItem> Items { get; set; } = new();
    }
}
