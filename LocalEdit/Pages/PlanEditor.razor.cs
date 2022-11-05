using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;
using LocalEdit.PlanTypes;
using Blazorise.Components;
using System.Text.Json;

namespace LocalEdit.Pages
{
    public partial class PlanEditor : ComponentBase
    {
        // Add
        // Click Add
        // Create new record
        // set modalObject to new record
        // Open editor
        // When editor closes
        // If cancelled - exit
        // else add record to tree and model

        // Update
        // click edit
        // create new record
        // copy selected record to new record
        // set modalObject to new record
        // open editor
        // When editor closes
        // If cancelled - exit
        // else update model and tree

        void Edit()
        {

        }

        protected override Task OnInitializedAsync()
        {
            Document.BaseUrl = "https://gimme";
            Document.Items = new List<PlanItem>(new[]
            {
                new PlanItem{ID = "Q1", Label="Question One", StoryId="1"},
                new PlanItem{ID = "Q2", Label="Question Two", StoryId="2", Dependencies = new List<PlanItemDependency>(new[]
                {
                    new PlanItemDependency{ ID = "Q1", DependencyType="ITEM"}
                }) },
                new PlanItem{ID = "Q3", Label="Question Three", StoryId="3"},
                new PlanItem{ID = "Q4", Label="Question Four", StoryId="4"}
            });


            return base.OnInitializedAsync();
        }

        PlanItem selectedItemRow { get; set; }

        PlanDocument Document { get; set; } = new PlanDocument();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;

        private PlanItemModal? planItemModalRef;

        private Task ShowItemModal()
        {
            if (selectedItemRow == null)
            {
                return Task.CompletedTask;
            }
            planItemModalRef.Item = selectedItemRow;

            planItemModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewItem()
        {
            PlanItem newItem = new PlanItem();
            newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
            newItem.Label = "New Plan Item";

            selectedItemRow = newItem;
            Document.Items.Add(newItem);
            adding = true;

            return ShowItemModal();
        }

        private Task OnPlanItemModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (planItemModalRef.Result == ModalResult.Cancel)
                {
                    selectedItemRow = null;
                    Document.Items.Remove(selectedItemRow);
                }
            }
            adding = false;

            return Task.CompletedTask;
        }

        public static string CalculateLink(string baseUrl, string storyId)
        {
            return baseUrl + "/" + storyId;
        }

        private string DecodePlanId(string id)
        {
            string rtnVal = id;

            foreach (PlanItem fi in Document.Items)
            {
                if (fi.ID == id)
                {
                    rtnVal = fi.Label;
                    break;
                }
            }

            return rtnVal;
        }

        FileManagementModal fileManagementModalRef;

        private Task LoadFile()
        {
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //flowItemModalRef.item = selectedItemRow;

            fileManagementModalRef?.LoadFile();
            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task SaveFile()
        {
            string fileText = JsonSerializer.Serialize(Document);
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //flowItemModalRef.item = selectedItemRow;

            fileManagementModalRef.SaveFile(fileText);
            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnFileManagementModalClosed()
        {
            if (fileManagementModalRef.Result == ModalResult.OK)
            {
                //MarkdownText = fileManagementModalRef.FileText;
                Document = (PlanDocument)JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(PlanDocument));
                InvokeAsync(() => StateHasChanged());
            }
            //if (adding)
            //{
            //    // remove the new item, if add was cancelled
            //    if (flowRelationshipModalRef.Result == ModalResult.Cancel)
            //    {
            //        FlowRelationships.Remove(selectedRelationshipRow);
            //    }
            //}
            //adding = false;

            return Task.CompletedTask;
        }

        //        private string GenerateMermaidText(FlowDocument document)
        private Task GenerateMarkdown()
        {
            MarkdownText = MarkdownGenerator.WrapMermaid(PlanPublisher.Publish(Document));
            return Task.CompletedTask;
        }
    }
}
