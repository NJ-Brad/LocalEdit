using Blazorise;
using LocalEdit.QuestionFlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class QuestionFlowItemModal : LE_ModalBase
    {
        [Parameter]
        public QuestionFlowItem Item { get; set; } = new();

        [Parameter]
        public List<QuestionFlowItem> Items { get; set; } = new();

        public QuestionFlowRelationship? SelectedRelationshipRow { get; set; } = new();
        private QuestionFlowRelationshipModal? QuestionFlowRelationshipModalRef;
        bool adding = false;

        private Task ShowRelationshipModal()
        {
            if (SelectedRelationshipRow == null)
            {
                return Task.CompletedTask;
            }
            if (QuestionFlowRelationshipModalRef != null)
            { 
                //QuestionFlowRelationshipModalRef.Item = SelectedRelationshipRow;

                QuestionFlowRelationshipModalRef?.ShowModal();
            }
            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewRelationship()
        {
            QuestionFlowRelationship newRelationship = new()
            {
                Label = "New Relationship"
            };

            SelectedRelationshipRow = newRelationship;
            Item?.NextQuestions?.Add(newRelationship);
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
            if (SelectedRelationshipRow != null)
            {
                Item?.NextQuestions?.Remove(SelectedRelationshipRow);
                SelectedRelationshipRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnQuestionFlowRelationshipModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (QuestionFlowRelationshipModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedRelationshipRow != null)
                    {
                        Item?.NextQuestions?.Remove(SelectedRelationshipRow);
                        SelectedRelationshipRow = null;
                    }
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

    }
}
