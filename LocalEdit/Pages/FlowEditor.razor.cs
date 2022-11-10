using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;
using LocalEdit.FlowTypes;
using Blazorise.Components;
using System.Text.Json;
using LocalEdit.LpeTypes;

namespace LocalEdit.Pages
{
    public partial class FlowEditor : ComponentBase
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
            this.Document.Items = new List<FlowItem>(new[]
            {
//            C4TestData.InternalPerson,
            new FlowItem{ID = "Q1", ItemType=FlowItemType.Question, Label="Question One"},
            new FlowItem{ID = "Q2", ItemType=FlowItemType.Question, Label="Question Two"},
            new FlowItem{ID = "Q3", ItemType=FlowItemType.Question, Label="Question Three"},
            new FlowItem{ID = "Q4", ItemType=FlowItemType.Question, Label="Question Four"}
        });

            this.Document.Relationships = new List<FlowRelationship>(new[]
            {
            new FlowRelationship{ From="Q1", To ="Q2", Label= "Step One"},
            new FlowRelationship{ From="Q1", To ="Q3", Label="Alt Flow"},
            new FlowRelationship{ From="Q3", To ="Q4", Label="Step One"},
            new FlowRelationship{ From="Q2", To ="Q4", Label="Weird Flow"},
            new FlowRelationship{ From="Q4", To ="Q1", Label="Vicious Cycle"}
        });

            return base.OnInitializedAsync();
        }

        FlowItem selectedItemRow { get; set; }
        FlowRelationship selectedRelationshipRow { get; set; }

        FlowDocument Document { get; set; } = new FlowDocument();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;

//        List<FlowItem> FlowItems = new List<FlowItem>(new[]
//        {
////            C4TestData.InternalPerson,
//            new FlowItem{ID = "Q1", ItemType=FlowItemType.Question, Label="Question One"},
//            new FlowItem{ID = "Q2", ItemType=FlowItemType.Question, Label="Question Two"},
//            new FlowItem{ID = "Q3", ItemType=FlowItemType.Question, Label="Question Three"},
//            new FlowItem{ID = "Q4", ItemType=FlowItemType.Question, Label="Question Four"}
//        });

//        List<FlowRelationship> FlowRelationships = new List<FlowRelationship>(new[]
//        {
//            new FlowRelationship{ From="Q1", To ="Q2", Label= "Step One"},
//            new FlowRelationship{ From="Q1", To ="Q3", Label="Alt Flow"},
//            new FlowRelationship{ From="Q3", To ="Q4", Label="Step One"},
//            new FlowRelationship{ From="Q2", To ="Q4", Label="Weird Flow"},
//            new FlowRelationship{ From="Q4", To ="Q1", Label="Vicious Cycle"}
//        });

        private FlowItemModal? flowItemModalRef;
        private FlowRelationshipModal? flowRelationshipModalRef;

        private Task ShowItemModal()
        {
            if (selectedItemRow == null)
            {
                return Task.CompletedTask;
            }
            flowItemModalRef.item = selectedItemRow;

            flowItemModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewItem()
        {
            FlowItem newItem = new FlowItem();
            newItem.ItemType = FlowItemType.Question;
            newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
            newItem.Label = "New Question";

            selectedItemRow = newItem;
            Document.Items.Add(newItem);
            adding = true;

            return ShowItemModal();
        }

        private Task OnFlowItemModalClosed()
        {
            if(adding)
            {
                // remove the new item, if add was cancelled
                if (flowItemModalRef.Result == ModalResult.Cancel)
                {
                    Document.Items.Remove(selectedItemRow);
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task DeleteItem()
        {
            if (selectedItemRow != null)
            {
                Document.Items.Remove(selectedItemRow);
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ShowRelationshipModal()
        {
            if (selectedRelationshipRow == null)
            {
                return Task.CompletedTask;
            }
            flowRelationshipModalRef.item = selectedRelationshipRow;

            flowRelationshipModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewRelationship()
        {
            FlowRelationship newRelationship = new FlowRelationship();
            newRelationship.Label = "New Relationship";

            selectedRelationshipRow = newRelationship;
            Document.Relationships.Add(newRelationship);
            adding = true;

            return ShowRelationshipModal();
        }

        private string DecodeFlowId(string id)
        {
            string rtnVal = id;

            foreach (FlowItem fi in Document.Items)
            {
                if(fi.ID == id)
                {
                    rtnVal = fi.Label;
                    break;
                }
            }

            return rtnVal;
        }

        private Task DeleteRelationship()
        {
            if (selectedRelationshipRow != null)
            {
                Document.Relationships.Remove(selectedRelationshipRow);
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnFlowRelationshipModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (flowRelationshipModalRef.Result == ModalResult.Cancel)
                {
                    Document.Relationships.Remove(selectedRelationshipRow);
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        FileManagementModal fileManagementModalRef;

        bool isLpeFile = false;

        private Task LoadFile()
        {
            isLpeFile = false;

            fileManagementModalRef?.LoadFile();

            return Task.CompletedTask;
        }

        private Task LoadLpeFile()
        {
            isLpeFile = true;

            fileManagementModalRef?.LoadFile();

            return Task.CompletedTask;
        }


        private Task SaveFile()
        {
            string fileText = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;
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
                if (isLpeFile)
                {
                    string test = fileManagementModalRef.FileText;
                    Document = LpeConverter.ToFlowDocument((Root)JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(Root)));
                }
                else
                {
                    Document = (FlowDocument)JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(FlowDocument));
                }
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
            MarkdownText = MarkdownGenerator.WrapMermaid(FlowPublisher.Publish(Document));
            return Task.CompletedTask;
        }
    }
}
