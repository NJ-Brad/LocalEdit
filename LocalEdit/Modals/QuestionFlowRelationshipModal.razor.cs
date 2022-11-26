using Blazorise;
using LocalEdit.QuestionFlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class QuestionFlowRelationshipModal : LE_ModalBase
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
        public QuestionFlowRelationship item { get; set; } = new();

        [Parameter]
        public List<QuestionFlowItem> Items { get; set; } = new();

    }
}
