using Blazorise;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class TestModal : LE_ModalBase
    {
        private bool validChecked = false;

        public override async Task<bool> Validate()
        {
            return validChecked;
        }
    }
}
