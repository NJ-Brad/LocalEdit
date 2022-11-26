using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;
using LocalEdit.QuestionFlowTypes;
using Blazorise.Components;
using System.Text.Json;
using LocalEdit.LpeTypes;
//using StardustDL.RazorComponents.Markdown;
using LocalEdit.Shared;
using LocalEdit.PlanTypes;

namespace LocalEdit.Pages
{
    public partial class QuestionFlowEditor : ComponentBase
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

            Mermaid?  MermaidOne { get; set; }

        protected override Task OnInitializedAsync()
        {
            this.Document.Items = new List<QuestionFlowItem>(new[]
            {
//            C4TestData.InternalPerson,
            new QuestionFlowItem{/*ID = "Q1", */Label="Question One"},
            new QuestionFlowItem{/*ID = "Q2", */Label="Question Two"},
            new QuestionFlowItem{/*ID = "Q3", */Label="Question Three"},
            new QuestionFlowItem{/*ID = "Q4", */Label="Question Four"}
        });

        //    this.Document.Relationships = new List<QuestionFlowRelationship>(new[]
        //    {
        //    new QuestionFlowRelationship{ From="Question One", To ="Question Two", Label= "Step One"},
        //    new QuestionFlowRelationship{ From="Question One", To ="Question Three", Label="Alt QuestionFlow"},
        //    new QuestionFlowRelationship{ From="Question Three", To ="Question Four", Label="Step One"},
        //    new QuestionFlowRelationship{ From="Question Two", To ="Question Four", Label="Weird QuestionFlow"},
        //    new QuestionFlowRelationship{ From="Question Four", To ="Question One", Label="Vicious Cycle"}
        //});

            return base.OnInitializedAsync();
        }

        QuestionFlowItem selectedItemRow { get; set; }
        QuestionFlowRelationship selectedRelationshipRow { get; set; }

        QuestionFlowDocument Document { get; set; } = new QuestionFlowDocument();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;

        string selectedTab = "general";

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                //GenerateMarkdown();

                MermaidOne.DisplayDiagram(QuestionFlowPublisher.Publish(Document));

            }

            return Task.CompletedTask;
        }


        private Task NewQuestionFlow()
        {
            fileManagementModalRef.Name = "New_QuestionFlow.json";

            this.Document.Items = new List<QuestionFlowItem>(new[]
            {
    //            C4TestData.InternalPerson,
            new QuestionFlowItem{/*ID = "Q1", */Label="Question One"},
            new QuestionFlowItem{/*ID = "Q2", */Label="Question Two"},
            new QuestionFlowItem{/*ID = "Q3", */Label="Question Three"},
            new QuestionFlowItem{/*ID = "Q4", */Label="Question Four"}
        });

            //this.Document.Relationships = new List<QuestionFlowRelationship>(new[]
            //{
            //new QuestionFlowRelationship{ From="Question One", To ="Question Two", Label= "Step One"},
            //new QuestionFlowRelationship{ From="Question One", To ="Question Three", Label="Alt QuestionFlow"},
            //new QuestionFlowRelationship{ From="Question Three", To ="Question Four", Label="Step One"},
            //new QuestionFlowRelationship{ From="Question Two", To ="Question Four", Label="Weird QuestionFlow"},
            //new QuestionFlowRelationship{ From="Question Four", To ="Question One", Label="Vicious Cycle"}
            //});

            //ResetValidation();

            return Task.CompletedTask;
        }

        //        List<QuestionFlowItem> QuestionFlowItems = new List<QuestionFlowItem>(new[]
        //        {
        ////            C4TestData.InternalPerson,
        //            new QuestionFlowItem{ID = "Q1", ItemType=QuestionFlowItemType.Question, Label="Question One"},
        //            new QuestionFlowItem{ID = "Q2", ItemType=QuestionFlowItemType.Question, Label="Question Two"},
        //            new QuestionFlowItem{ID = "Q3", ItemType=QuestionFlowItemType.Question, Label="Question Three"},
        //            new QuestionFlowItem{ID = "Q4", ItemType=QuestionFlowItemType.Question, Label="Question Four"}
        //        });

        //        List<QuestionFlowRelationship> QuestionFlowRelationships = new List<QuestionFlowRelationship>(new[]
        //        {
        //            new QuestionFlowRelationship{ From="Q1", To ="Q2", Label= "Step One"},
        //            new QuestionFlowRelationship{ From="Q1", To ="Q3", Label="Alt QuestionFlow"},
        //            new QuestionFlowRelationship{ From="Q3", To ="Q4", Label="Step One"},
        //            new QuestionFlowRelationship{ From="Q2", To ="Q4", Label="Weird QuestionFlow"},
        //            new QuestionFlowRelationship{ From="Q4", To ="Q1", Label="Vicious Cycle"}
        //        });

        private QuestionFlowItemModal? QuestionFlowItemModalRef;
        //private QuestionFlowRelationshipModal? QuestionFlowRelationshipModalRef;

        private Task ShowItemModal()
        {
            if (selectedItemRow == null)
            {
                return Task.CompletedTask;
            }
            QuestionFlowItemModalRef.item = selectedItemRow;

            QuestionFlowItemModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewItem()
        {
            QuestionFlowItem newItem = new QuestionFlowItem();
            //newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
            newItem.Label = "New Question";

            selectedItemRow = newItem;
            Document.Items.Add(newItem);
            adding = true;

            return ShowItemModal();
        }

        private Task OnQuestionFlowItemModalClosed()
        {
            if(adding)
            {
                // remove the new item, if add was cancelled
                if (QuestionFlowItemModalRef.Result == ModalResult.Cancel)
                {
                    Document.Items.Remove(selectedItemRow);
                    selectedItemRow = null;
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
                selectedItemRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ShowRelationshipModal()
        {
            //if (selectedRelationshipRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //QuestionFlowRelationshipModalRef.item = selectedRelationshipRow;

            //QuestionFlowRelationshipModalRef?.ShowModal();

            ////InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewRelationship()
        {
            //QuestionFlowRelationship newRelationship = new QuestionFlowRelationship();
            //newRelationship.Label = "New Relationship";

            //selectedRelationshipRow = newRelationship;
            //Document.Relationships.Add(newRelationship);
            //adding = true;

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
            //if (selectedRelationshipRow != null)
            //{
            //    Document.Relationships.Remove(selectedRelationshipRow);
            //    selectedRelationshipRow = null;
            //}

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnQuestionFlowRelationshipModalClosed()
        {
            //if (adding)
            //{
            //    // remove the new item, if add was cancelled
            //    if (QuestionFlowRelationshipModalRef.Result == ModalResult.Cancel)
            //    {
            //        Document.Relationships.Remove(selectedRelationshipRow);
            //        selectedRelationshipRow = null;
            //    }
            //}
            //adding = false;

            //InvokeAsync(() => StateHasChanged());

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
            //QuestionFlowItemModalRef.item = selectedItemRow;

            fileManagementModalRef.SaveFile(fileText);
            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ExportFile()
        {
  //          if (Validate().Result)
            {
                GenerateMarkdown();

                fileManagementModalRef.Name = "QuestionFlow.md";
                fileManagementModalRef.SaveFile(MarkdownText);
            }

            return Task.CompletedTask;
        }

        private Task ExportHtml()
        {
//            if (Validate().Result)
            {
                string htmlText = GenerateHtml().Result;

                fileManagementModalRef.Name = "QuestionFlow.html";
                fileManagementModalRef.SaveFile(htmlText);
            }
            return Task.CompletedTask;
        }


//        MarkdownRenderer markdownRef = null;

        void OnClickNode(string nodeId)
        {
            // TODO: do something with nodeId
        }

        private Task OnFileManagementModalClosed()
        {
            if (fileManagementModalRef.Result == ModalResult.OK)
            {
                Document = (QuestionFlowDocument)JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(QuestionFlowDocument));
                InvokeAsync(() => StateHasChanged());
            }
            //if (adding)
            //{
            //    // remove the new item, if add was cancelled
            //    if (QuestionFlowRelationshipModalRef.Result == ModalResult.Cancel)
            //    {
            //        QuestionFlowRelationships.Remove(selectedRelationshipRow);
            //    }
            //}
            //adding = false;

            return Task.CompletedTask;
        }

        //        private string GenerateMermaidText(QuestionFlowDocument document)
        private Task GenerateMarkdown()
        {
            //mermaidText = QuestionFlowPublisher.Publish(Document);
            MarkdownText = MarkdownGenerator.WrapMermaid(QuestionFlowPublisher.Publish(Document));

//            markdownRef.Value = MarkdownText;
            return Task.CompletedTask;
        }

        private Task<string> GenerateHtml()
        {
            string htmlText = HtmlGenerator.WrapMermaid(QuestionFlowPublisher.Publish(Document));
            return Task.FromResult(htmlText);
        }

    }
}
