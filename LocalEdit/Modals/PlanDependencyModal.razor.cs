using Blazorise;
using LocalEdit.PlanTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class PlanDependencyModal : LE_ModalBase
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

        DatePicker<DateTime?>? datePicker;

        DateTime? selectedDate;

        void OnDateChanged(DateTime? date)
        {
            selectedDate = date;
            if (date.HasValue)
            {
                Item.StartDate = date.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                Item.StartDate = string.Empty;
            }
        }

        PlanItemDependency item = new();

        [Parameter]
        public PlanItemDependency Item
        {
            get { return item; }
            set
            { 
                item = value;

                DateTime asDate = DateTime.Today;

                if(DateTime.TryParse(value.StartDate, out asDate))
                {
                }
                datePicker.Date = asDate;
            }
        }

        [Parameter]
        public List<PlanItem> Items { get; set; } = new();

    }
}
