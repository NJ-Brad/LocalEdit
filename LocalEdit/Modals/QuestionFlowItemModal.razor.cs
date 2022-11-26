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

        [Parameter]
        public List<QuestionFlowItem> Items { get; set; } = new();

        public QuestionFlowRelationship? selectedRelationshipRow { get; set; }
        private QuestionFlowRelationshipModal? QuestionFlowRelationshipModalRef;
        bool adding = false;

        private Task ShowRelationshipModal()
        {
            if (selectedRelationshipRow == null)
            {
                return Task.CompletedTask;
            }
            QuestionFlowRelationshipModalRef.item = selectedRelationshipRow;

            QuestionFlowRelationshipModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewRelationship()
        {
            QuestionFlowRelationship newRelationship = new QuestionFlowRelationship();
            newRelationship.Label = "New Relationship";

            selectedRelationshipRow = newRelationship;
            item.NextQuestions.Add(newRelationship);
            adding = true;

            return ShowRelationshipModal();
        }

        //private string DecodeQuestionFlowId(string id)
        //{
        //    string rtnVal = id;

        //    foreach (QuestionFlowItem fi in Document.Items)
        //    {
        //        if(fi.ID == id)
        //        {
        //            rtnVal = fi.Label;
        //            break;
        //        }
        //    }

        //    return rtnVal;
        //}

        private Task DeleteRelationship()
        {
            if (selectedRelationshipRow != null)
            {
                item.NextQuestions.Remove(selectedRelationshipRow);
                selectedRelationshipRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnQuestionFlowRelationshipModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (QuestionFlowRelationshipModalRef.Result == ModalResult.Cancel)
                {
                    item.NextQuestions.Remove(selectedRelationshipRow);
                    selectedRelationshipRow = null;
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

    }
}
