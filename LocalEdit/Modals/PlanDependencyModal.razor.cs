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

        void ValidateId(ValidatorEventArgs e)
        {
            e.Status = ValidationStatus.Success;
            if (Item.DependencyType == "OTHER")
            {
                string value = Convert.ToString(e.Value);

                //e.Status = string.IsNullOrEmpty(startDate) ? ValidationStatus.None :
                //    email.Contains("@") ? ValidationStatus.Success : ValidationStatus.Error;
                e.Status = string.IsNullOrEmpty(value) ? ValidationStatus.Error : ValidationStatus.Success;
            }
            else
            {
                e.Status = ValidationStatus.None;
            }

        }

        void ValidateStartDate(ValidatorEventArgs e)
        {
            e.Status = ValidationStatus.Success;
            if (Item.DependencyType == "DATE")
            {
                DateTime? testVal = (e.Value as dynamic)[0] as DateTime?;

                //dynamic v2 = e.Value;
                //DateTime? v3 = v2[0] as DateTime?;

                if (testVal == DateTime.MinValue)
                {
                    e.Status = ValidationStatus.Error;
                }
            }
            else
            {
                e.Status = ValidationStatus.None;
            }

            //e.Status = string.IsNullOrEmpty(startDate) ? ValidationStatus.None :
            //    email.Contains("@") ? ValidationStatus.Success : ValidationStatus.Error;
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
