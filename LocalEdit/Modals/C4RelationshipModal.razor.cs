using Blazorise;
using LocalEdit.C4Types;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
    {
        public partial class C4RelationshipModal : LE_ModalBase
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
            public C4Relationship item { get; set; } = new();

            [Parameter]
            public List<C4Item> Items { get; set; } = new();

        }
    }
