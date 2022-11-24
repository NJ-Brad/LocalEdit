using Blazorise;
using LocalEdit.SequenceTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class SequenceItemModal : LE_ModalBase
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
        public SequenceItem item { get; set; } = new();

    }
}
