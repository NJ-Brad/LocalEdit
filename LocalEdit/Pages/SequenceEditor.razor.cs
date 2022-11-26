using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;
using LocalEdit.SequenceTypes;
using Blazorise.Components;
using System.Text.Json;
using LocalEdit.LpeTypes;
//using StardustDL.RazorComponents.Markdown;
using LocalEdit.Shared;
using LocalEdit.PlanTypes;

namespace LocalEdit.Pages
{
    public partial class SequenceEditor : ComponentBase
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

            Mermaid  MermaidOne { get; set; }

        protected override Task OnInitializedAsync()
        {
            this.Document.Items = new List<SequenceItem>(new[]
            {
//            C4TestData.InternalPerson,
            new SequenceItem{/*ID = "Q1", */Label="Question One"},
            new SequenceItem{/*ID = "Q2", */Label="Question Two"},
            new SequenceItem{/*ID = "Q3", */Label="Question Three"},
            new SequenceItem{/*ID = "Q4", */Label="Question Four"}
        });

            this.Document.Relationships = new List<SequenceRelationship>(new[]
            {
            new SequenceRelationship{ From="Question One", To ="Question Two", Label= "Step One"},
            new SequenceRelationship{ From="Question One", To ="Question Three", Label="Alt Sequence"},
            new SequenceRelationship{ From="Question Three", To ="Question Four", Label="Step One"},
            new SequenceRelationship{ From="Question Two", To ="Question Four", Label="Weird Sequence"},
            new SequenceRelationship{ From="Question Four", To ="Question One", Label="Vicious Cycle"}
        });

            return base.OnInitializedAsync();
        }

        SequenceItem selectedItemRow { get; set; }
        SequenceRelationship selectedRelationshipRow { get; set; }

        SequenceDocument Document { get; set; } = new SequenceDocument();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;

        string selectedTab = "general";

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                //GenerateMarkdown();

                MermaidOne.DisplayDiagram(SequencePublisher.Publish(Document));

            }

            return Task.CompletedTask;
        }


        private Task NewSequence()
        {
            fileManagementModalRef.Name = "New_Sequence.json";

            this.Document.Items = new List<SequenceItem>(new[]
            {
    //            C4TestData.InternalPerson,
            new SequenceItem{/*ID = "Q1", */Label="Question One"},
            new SequenceItem{/*ID = "Q2", */Label="Question Two"},
            new SequenceItem{/*ID = "Q3", */Label="Question Three"},
            new SequenceItem{/*ID = "Q4", */Label="Question Four"}
        });

            this.Document.Relationships = new List<SequenceRelationship>(new[]
            {
            new SequenceRelationship{ From="Question One", To ="Question Two", Label= "Step One"},
            new SequenceRelationship{ From="Question One", To ="Question Three", Label="Alt Sequence"},
            new SequenceRelationship{ From="Question Three", To ="Question Four", Label="Step One"},
            new SequenceRelationship{ From="Question Two", To ="Question Four", Label="Weird Sequence"},
            new SequenceRelationship{ From="Question Four", To ="Question One", Label="Vicious Cycle"}
            });

            //ResetValidation();

            return Task.CompletedTask;
        }

        //        List<SequenceItem> SequenceItems = new List<SequenceItem>(new[]
        //        {
        ////            C4TestData.InternalPerson,
        //            new SequenceItem{ID = "Q1", ItemType=SequenceItemType.Question, Label="Question One"},
        //            new SequenceItem{ID = "Q2", ItemType=SequenceItemType.Question, Label="Question Two"},
        //            new SequenceItem{ID = "Q3", ItemType=SequenceItemType.Question, Label="Question Three"},
        //            new SequenceItem{ID = "Q4", ItemType=SequenceItemType.Question, Label="Question Four"}
        //        });

        //        List<SequenceRelationship> SequenceRelationships = new List<SequenceRelationship>(new[]
        //        {
        //            new SequenceRelationship{ From="Q1", To ="Q2", Label= "Step One"},
        //            new SequenceRelationship{ From="Q1", To ="Q3", Label="Alt Sequence"},
        //            new SequenceRelationship{ From="Q3", To ="Q4", Label="Step One"},
        //            new SequenceRelationship{ From="Q2", To ="Q4", Label="Weird Sequence"},
        //            new SequenceRelationship{ From="Q4", To ="Q1", Label="Vicious Cycle"}
        //        });

        private SequenceItemModal? SequenceItemModalRef;
        private SequenceRelationshipModal? SequenceRelationshipModalRef;

        private Task ShowItemModal()
        {
            if (selectedItemRow == null)
            {
                return Task.CompletedTask;
            }
            SequenceItemModalRef.item = selectedItemRow;

            SequenceItemModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewItem()
        {
            SequenceItem newItem = new SequenceItem();
            //newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
            newItem.Label = "New Question";

            selectedItemRow = newItem;
            Document.Items.Add(newItem);
            adding = true;

            return ShowItemModal();
        }

        private Task OnSequenceItemModalClosed()
        {
            if(adding)
            {
                // remove the new item, if add was cancelled
                if (SequenceItemModalRef.Result == ModalResult.Cancel)
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
            if (selectedRelationshipRow == null)
            {
                return Task.CompletedTask;
            }
            SequenceRelationshipModalRef.item = selectedRelationshipRow;

            SequenceRelationshipModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewRelationship()
        {
            SequenceRelationship newRelationship = new SequenceRelationship();
            newRelationship.Label = "New Relationship";

            selectedRelationshipRow = newRelationship;
            Document.Relationships.Add(newRelationship);
            adding = true;

            return ShowRelationshipModal();
        }

        //private string DecodeSequenceId(string id)
        //{
        //    string rtnVal = id;

        //    foreach (SequenceItem fi in Document.Items)
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
                Document.Relationships.Remove(selectedRelationshipRow);
                selectedRelationshipRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnSequenceRelationshipModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (SequenceRelationshipModalRef.Result == ModalResult.Cancel)
                {
                    Document.Relationships.Remove(selectedRelationshipRow);
                    selectedRelationshipRow = null;
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
            //SequenceItemModalRef.item = selectedItemRow;

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

                fileManagementModalRef.Name = "Sequence.md";
                fileManagementModalRef.SaveFile(MarkdownText);
            }

            return Task.CompletedTask;
        }

        private Task ExportHtml()
        {
//            if (Validate().Result)
            {
                string htmlText = GenerateHtml().Result;

                fileManagementModalRef.Name = "Sequence.html";
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
                if (isLpeFile)
                {
                    //string test = fileManagementModalRef.FileText;
                    //Root rt = (Root)JsonSerializer.Deserialize(test, typeof(Root));
                    //Document = LpeConverter.ToSequenceDocument(rt);
                    Document = LpeConverter.ToSequenceDocument((Root)JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(Root)));
                }
                else
                {
                    Document = (SequenceDocument)JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(SequenceDocument));
                }
                InvokeAsync(() => StateHasChanged());
            }
            //if (adding)
            //{
            //    // remove the new item, if add was cancelled
            //    if (SequenceRelationshipModalRef.Result == ModalResult.Cancel)
            //    {
            //        SequenceRelationships.Remove(selectedRelationshipRow);
            //    }
            //}
            //adding = false;

            return Task.CompletedTask;
        }

        //        private string GenerateMermaidText(SequenceDocument document)
        private Task GenerateMarkdown()
        {
            //mermaidText = SequencePublisher.Publish(Document);
            MarkdownText = MarkdownGenerator.WrapMermaid(SequencePublisher.Publish(Document));

//            markdownRef.Value = MarkdownText;
            return Task.CompletedTask;
        }

        private Task<string> GenerateHtml()
        {
            string htmlText = HtmlGenerator.WrapMermaid(SequencePublisher.Publish(Document));
            return Task.FromResult(htmlText);
        }

    }
}
