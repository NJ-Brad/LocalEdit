using Blazorise;
using LocalEdit.QuestionFlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class QuestionFlowItemModal : LE_ModalBase
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

        public override async Task ResetValidation()
        {
            await validations.ClearAll();
        }

        [Parameter]
        public QuestionFlowItem item { get; set; } = new();

        public QuestionFlowRelationship? selectedRelationshipRow { get; set; }

    }
}
