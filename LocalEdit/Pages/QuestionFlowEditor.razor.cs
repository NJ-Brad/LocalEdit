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
using LocalEdit.FlowTypes;

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

        //void Edit()
        //{

        //}

            Mermaid?  MermaidOne { get; set; }

        protected override Task OnInitializedAsync()
        {
            Document = new()
            {
                items = new List<QuestionFlowItem>(new[]
            {
//            C4TestData.InternalPerson,
            new QuestionFlowItem{id = "Q1", title="Question One"},
            new QuestionFlowItem{id = "Q2", title="Question Two"},
            new QuestionFlowItem{id = "Q3", title="Question Three"},
            new QuestionFlowItem{id = "Q4", title="Question Four"}
        })
            };

            //    this.Document.Relationships = new List<QuestionFlowRelationship>(new[]
            //    {
            //    new QuestionFlowRelationship{ From="Question One", To ="Question Two", title= "Step One"},
            //    new QuestionFlowRelationship{ From="Question One", To ="Question Three", title="Alt QuestionFlow"},
            //    new QuestionFlowRelationship{ From="Question Three", To ="Question Four", title="Step One"},
            //    new QuestionFlowRelationship{ From="Question Two", To ="Question Four", title="Weird QuestionFlow"},
            //    new QuestionFlowRelationship{ From="Question Four", To ="Question One", title="Vicious Cycle"}
            //});

            return base.OnInitializedAsync();
        }

        QuestionFlowItem? SelectedItemRow { get; set; } = new();

        //QuestionFlowRelationship? SelectedRelationshipRow { get; set; }

        QuestionFlowDocument? Document { get; set; } = new QuestionFlowDocument();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;

        string selectedTab = "general";

        private async Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                //GenerateMarkdown();

                if ((Document != null) && (MermaidOne != null))
                {
                    // convert to "normal" flow
                    FlowDocument fd = LocalEdit.QuestionFlowTypes.LpeConverter.ToFlowDocument(Document);

                    await MermaidOne.DisplayDiagram(QuestionFlowPublisher.Publish(Document));
                }
            }

            //return Task.CompletedTask;
        }


        private Task NewQuestionFlow()
        {
            if(FileManagementModalRef != null)
                FileManagementModalRef.Name = "New_QuestionFlow.json";

            Document = new()
            {
                items = new List<QuestionFlowItem>(new[]
            {
    //            C4TestData.InternalPerson,
            new QuestionFlowItem{id = "Q1", title="Question One"},
            new QuestionFlowItem{id = "Q2", title="Question Two"},
            new QuestionFlowItem{id = "Q3", title="Question Three"},
            new QuestionFlowItem{id = "Q4", title="Question Four"}
        })
            };

            //this.Document.Relationships = new List<QuestionFlowRelationship>(new[]
            //{
            //new QuestionFlowRelationship{ From="Question One", To ="Question Two", title= "Step One"},
            //new QuestionFlowRelationship{ From="Question One", To ="Question Three", title="Alt QuestionFlow"},
            //new QuestionFlowRelationship{ From="Question Three", To ="Question Four", title="Step One"},
            //new QuestionFlowRelationship{ From="Question Two", To ="Question Four", title="Weird QuestionFlow"},
            //new QuestionFlowRelationship{ From="Question Four", To ="Question One", title="Vicious Cycle"}
            //});

            //ResetValidation();

            return Task.CompletedTask;
        }

        //        List<QuestionFlowItem> QuestionFlowItems = new List<QuestionFlowItem>(new[]
        //        {
        ////            C4TestData.InternalPerson,
        //            new QuestionFlowItem{ID = "Q1", ItemType=QuestionFlowItemType.Question, title="Question One"},
        //            new QuestionFlowItem{ID = "Q2", ItemType=QuestionFlowItemType.Question, title="Question Two"},
        //            new QuestionFlowItem{ID = "Q3", ItemType=QuestionFlowItemType.Question, title="Question Three"},
        //            new QuestionFlowItem{ID = "Q4", ItemType=QuestionFlowItemType.Question, title="Question Four"}
        //        });

        //        List<QuestionFlowRelationship> QuestionFlowRelationships = new List<QuestionFlowRelationship>(new[]
        //        {
        //            new QuestionFlowRelationship{ From="Q1", To ="Q2", title= "Step One"},
        //            new QuestionFlowRelationship{ From="Q1", To ="Q3", title="Alt QuestionFlow"},
        //            new QuestionFlowRelationship{ From="Q3", To ="Q4", title="Step One"},
        //            new QuestionFlowRelationship{ From="Q2", To ="Q4", title="Weird QuestionFlow"},
        //            new QuestionFlowRelationship{ From="Q4", To ="Q1", title="Vicious Cycle"}
        //        });

        private QuestionFlowItemModal? QuestionFlowItemModalRef;
        //private QuestionFlowRelationshipModal? QuestionFlowRelationshipModalRef;

        private Task ShowItemModal()
        {
            if (SelectedItemRow == null)
            {
                return Task.CompletedTask;
            }
            //QuestionFlowItemModalRef.Item = selectedItemRow;

            QuestionFlowItemModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewItem()
        {
            QuestionFlowItem newItem = new()
            {
                //newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
                title = "New Question"
            };

            SelectedItemRow = newItem;
            Document?.items.Add(newItem);
            adding = true;

            return ShowItemModal();
        }

        private Task OnQuestionFlowItemModalClosed()
        {
            if(adding)
            {
                // remove the new item, if add was cancelled
                if (QuestionFlowItemModalRef?.Result == ModalResult.Cancel)
                {
                    if(SelectedItemRow != null)
                        Document?.items.Remove(SelectedItemRow);
                    SelectedItemRow = null;
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task DeleteItem()
        {
            if (SelectedItemRow != null)
            {
                Document?.items.Remove(SelectedItemRow);
                SelectedItemRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        //private Task ShowRelationshipModal()
        //{
        //    //if (selectedRelationshipRow == null)
        //    //{
        //    //    return Task.CompletedTask;
        //    //}
        //    //QuestionFlowRelationshipModalRef.item = selectedRelationshipRow;

        //    //QuestionFlowRelationshipModalRef?.ShowModal();

        //    ////InvokeAsync(() => StateHasChanged());

        //    return Task.CompletedTask;
        //}

        //private Task AddNewRelationship()
        //{
        //    //QuestionFlowRelationship newRelationship = new QuestionFlowRelationship();
        //    //newRelationship.title = "New Relationship";

        //    //selectedRelationshipRow = newRelationship;
        //    //Document.Relationships.Add(newRelationship);
        //    //adding = true;

        //    return ShowRelationshipModal();
        //}

        //private string DecodeQuestionFlowId(string id)
        //{
        //    string rtnVal = id;

        //    foreach (QuestionFlowItem fi in Document.Items)
        //    {
        //        if(fi.ID == id)
        //        {
        //            rtnVal = fi.title;
        //            break;
        //        }
        //    }

        //    return rtnVal;
        //}

        //private Task DeleteRelationship()
        //{
        //    //if (selectedRelationshipRow != null)
        //    //{
        //    //    Document.Relationships.Remove(selectedRelationshipRow);
        //    //    selectedRelationshipRow = null;
        //    //}

        //    //InvokeAsync(() => StateHasChanged());

        //    return Task.CompletedTask;
        //}

        //private Task OnQuestionFlowRelationshipModalClosed()
        //{
        //    //if (adding)
        //    //{
        //    //    // remove the new item, if add was cancelled
        //    //    if (QuestionFlowRelationshipModalRef.Result == ModalResult.Cancel)
        //    //    {
        //    //        Document.Relationships.Remove(selectedRelationshipRow);
        //    //        selectedRelationshipRow = null;
        //    //    }
        //    //}
        //    //adding = false;

        //    //InvokeAsync(() => StateHasChanged());

        //    return Task.CompletedTask;
        //}

        FileManagementModal? FileManagementModalRef { get; set; }

        //bool isLpeFile = false;

        private Task LoadFile()
        {
            //isLpeFile = false;

            FileManagementModalRef?.LoadFile();

            return Task.CompletedTask;
        }

        private Task LoadLpeFile()
        {
            //isLpeFile = true;

            FileManagementModalRef?.LoadFile();

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

            FileManagementModalRef?.SaveFile(fileText);
            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ExportFile()
        {
  //          if (Validate().Result)
            {
                GenerateMarkdown();

                if (FileManagementModalRef != null)
                {
                    FileManagementModalRef.Name = "QuestionFlow.md";
                    FileManagementModalRef.SaveFile(MarkdownText);
                }
            }

            return Task.CompletedTask;
        }

        private Task ExportHtml()
        {
//            if (Validate().Result)
            {
                string htmlText = GenerateHtml().Result;

                if (FileManagementModalRef != null)
                {
                    FileManagementModalRef.Name = "QuestionFlow.html";
                    FileManagementModalRef.SaveFile(htmlText);
                }
            }
            return Task.CompletedTask;
        }


//        MarkdownRenderer markdownRef = null;

        //void OnClickNode(string nodeId)
        //{
        //    // TODO: do something with nodeId
        //}

        private Task OnFileManagementModalClosed()
        {
            if (FileManagementModalRef?.Result == ModalResult.OK)
            {
                if (string.IsNullOrEmpty(FileManagementModalRef.FileText))
                    Document = null;
                else
                    Document = JsonSerializer.Deserialize(FileManagementModalRef.FileText, typeof(QuestionFlowDocument)) as QuestionFlowDocument;
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
            if (Document != null)
                MarkdownText = MarkdownGenerator.WrapMermaid(QuestionFlowPublisher.Publish(Document));

            //            markdownRef.Value = MarkdownText;
            return Task.CompletedTask;
        }

        private Task<string> GenerateHtml()
        {
            string htmlText = string.Empty;
            if (Document != null)
                htmlText = HtmlGenerator.WrapMermaid(QuestionFlowPublisher.Publish(Document));
            return Task.FromResult(htmlText);
        }

    }
}
