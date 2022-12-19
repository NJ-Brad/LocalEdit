using Blazorise;
using LocalEdit.FlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class FlowRelationshipModal : LE_ModalBase
    {
        Validations? validations;

        public override async Task<bool> Validate()
        {
            bool rtnVal = false;
            if (validations != null)
            {
                if (await validations.ValidateAll())
                {
                    rtnVal = true;
                }
            }
            else
                rtnVal = true;

            return rtnVal;
        }

        public override async Task ResetValidation()
        {
            await validations?.ClearAll();
        }

        [Parameter]
        public FlowRelationship item { get; set; } = new();

        [Parameter]
        public List<FlowItem> Items { get; set; } = new();

    }
}
