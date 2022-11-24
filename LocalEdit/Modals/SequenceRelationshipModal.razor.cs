using Blazorise;
using LocalEdit.SequenceTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class SequenceRelationshipModal : LE_ModalBase
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
        public SequenceRelationship item { get; set; } = new();

        [Parameter]
        public List<SequenceItem> Items { get; set; } = new();

    }
}
