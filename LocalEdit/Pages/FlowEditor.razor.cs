using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;
using LocalEdit.FlowTypes;
using Blazorise.Components;
using System.Text.Json;
using LocalEdit.LpeTypes;
//using StardustDL.RazorComponents.Markdown;
using LocalEdit.Shared;
using LocalEdit.PlanTypes;

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

        //void Edit()
        //{

        //}

            Mermaid?  MermaidOne { get; set; }

        protected override Task OnInitializedAsync()
        {
            Document = new FlowDocument
            {
                Items = new List<FlowItem>(new[]
            {
//            C4TestData.InternalPerson,
            new FlowItem{ID = "Q1", ItemType=FlowItemType.Question, Label="Question One"},
            new FlowItem{ID = "Q2", ItemType=FlowItemType.Question, Label="Question Two"},
            new FlowItem{ID = "Q3", ItemType=FlowItemType.Question, Label="Question Three"},
            new FlowItem{ID = "Q4", ItemType=FlowItemType.Question, Label="Question Four"}
        }),

                Relationships = new List<FlowRelationship>(new[]
            {
            new FlowRelationship{ From="Q1", To ="Q2", Label= "Step One"},
            new FlowRelationship{ From="Q1", To ="Q3", Label="Alt Flow"},
            new FlowRelationship{ From="Q3", To ="Q4", Label="Step One"},
            new FlowRelationship{ From="Q2", To ="Q4", Label="Weird Flow"},
            new FlowRelationship{ From="Q4", To ="Q1", Label="Vicious Cycle"}
        })
            };

            return base.OnInitializedAsync();
        }

        FlowItem? SelectedItemRow { get; set; } = new();
        FlowRelationship? SelectedRelationshipRow { get; set; } = new();

        FlowDocument? Document { get; set; } = new FlowDocument();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;

        string selectedTab = "general";

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                //GenerateMarkdown();

                if(Document != null)
                    MermaidOne?.DisplayDiagram(FlowPublisher.Publish(Document));

            }

            return Task.CompletedTask;
        }


        private Task NewFlow()
        {
            if (FileManagementModalRef != null)
            {
                FileManagementModalRef.Name = "New_Flow.json";
            }

            Document = new()
            {
                Items = new List<FlowItem>(new[]
            {
    //            C4TestData.InternalPerson,
                new FlowItem{ID = "Q1", ItemType=FlowItemType.Question, Label="Question One"},
                new FlowItem{ID = "Q2", ItemType=FlowItemType.Question, Label="Question Two"},
                new FlowItem{ID = "Q3", ItemType=FlowItemType.Question, Label="Question Three"},
                new FlowItem{ID = "Q4", ItemType=FlowItemType.Question, Label="Question Four"}
            }),

                Relationships = new List<FlowRelationship>(new[]
            {
                new FlowRelationship{ From="Q1", To ="Q2", Label= "Step One"},
                new FlowRelationship{ From="Q1", To ="Q3", Label="Alt Flow"},
                new FlowRelationship{ From="Q3", To ="Q4", Label="Step One"},
                new FlowRelationship{ From="Q2", To ="Q4", Label="Weird Flow"},
                new FlowRelationship{ From="Q4", To ="Q1", Label="Vicious Cycle"}
            })
            };

            //ResetValidation();

            return Task.CompletedTask;
        }

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
            if (SelectedItemRow == null)
            {
                return Task.CompletedTask;
            }

            //flowItemModalRef.Item = SelectedItemRow;

            flowItemModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewItem()
        {
            FlowItem newItem = new()
            {
                ItemType = FlowItemType.Question,
                ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper(),
                Label = "New Question"
            };

            SelectedItemRow = newItem;
            Document?.Items.Add(newItem);
            adding = true;

            return ShowItemModal();
        }

        private Task OnFlowItemModalClosed()
        {
            if(adding)
            {
                // remove the new item, if add was cancelled
                if (flowItemModalRef?.Result == ModalResult.Cancel)
                {
                    if(SelectedItemRow != null)
                        Document?.Items.Remove(SelectedItemRow);
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
                Document?.Items.Remove(SelectedItemRow);
                SelectedItemRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ShowRelationshipModal()
        {
            if (SelectedRelationshipRow == null)
            {
                return Task.CompletedTask;
            }

            //flowRelationshipModalRef.Item = selectedRelationshipRow;

            flowRelationshipModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewRelationship()
        {
            FlowRelationship newRelationship = new()
            {
                Label = "New Relationship"
            };

            SelectedRelationshipRow = newRelationship;
            Document?.Relationships.Add(newRelationship);
            adding = true;

            return ShowRelationshipModal();
        }

        private string DecodeFlowId(string id)
        {
            string rtnVal = id;

            if((Document != null) && (Document.Items != null))
            {
                foreach (FlowItem fi in Document.Items)
                {
                    if (fi.ID == id)
                    {
                        rtnVal = fi.Label;
                        break;
                    }
                }
            }
            return rtnVal;
        }

        private Task DeleteRelationship()
        {
            if (SelectedRelationshipRow != null)
            {
                Document?.Relationships.Remove(SelectedRelationshipRow);
                SelectedRelationshipRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnFlowRelationshipModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (flowRelationshipModalRef?.Result == ModalResult.Cancel)
                {
                    if(SelectedRelationshipRow != null)
                        Document?.Relationships.Remove(SelectedRelationshipRow);
                    SelectedRelationshipRow = null;
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        FileManagementModal? FileManagementModalRef { get; set; } = new();

        private Task LoadFile()
        {
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
            //flowItemModalRef.item = selectedItemRow;

            if (FileManagementModalRef != null)
            {
                FileManagementModalRef.SaveFile(fileText);
            }
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
                    FileManagementModalRef.Name = "Flow.md";
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
                    FileManagementModalRef.Name = "flow.html";
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
                if ((FileManagementModalRef != null) && (!string.IsNullOrEmpty(FileManagementModalRef.FileText)))
                {
                    Document = JsonSerializer.Deserialize(FileManagementModalRef.FileText, typeof(FlowDocument)) as FlowDocument;
                    InvokeAsync(() => StateHasChanged());
                }
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
            //mermaidText = FlowPublisher.Publish(Document);
            if(Document != null)
                MarkdownText = MarkdownGenerator.WrapMermaid(FlowPublisher.Publish(Document));

//            markdownRef.Value = MarkdownText;
            return Task.CompletedTask;
        }

        private Task<string> GenerateHtml()
        {
            string rtnVal = string.Empty;
            if (Document != null)
                rtnVal = HtmlGenerator.WrapMermaid(FlowPublisher.Publish(Document));
            return Task.FromResult(rtnVal);
        }

    }
}
