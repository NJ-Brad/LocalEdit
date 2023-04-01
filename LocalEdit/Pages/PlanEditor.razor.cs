using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;
using LocalEdit.PlanTypes;
using Blazorise.Components;
using System.Text.Json;
//using StardustDL.RazorComponents.Markdown;
using LocalEdit.Shared;

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

        //void Edit()
        //{

        //}

//        MarkdownRenderer markdownRef = null;
        //Mermaid mermaidOne;

        //void OnClickNode(string nodeId)
        //{
        //    // TODO: do something with nodeId
        //}

    Mermaid? MermaidOne { get; set; }


        protected override Task OnInitializedAsync()
        {
            Document = new()
            {
                Title = "Hello Brad",
                StartDate = "2022-11-06",
                BaseUrl = "https://gimme",
                Items = new List<PlanItem>(new[]
            {
                new PlanItem{ID = "Q1", Label="Question One", StoryId="1", Duration="1"},
                new PlanItem{ID = "Q2", Label="Question Two", StoryId="2", Duration="2", Dependencies = new List<PlanItemDependency>(new[]
                {
                    new PlanItemDependency{ ID = "Q1", DependencyType="ITEM"}
                }) },
                new PlanItem{ID = "Q3", Label="Question Three", StoryId="3", Duration="3"},
                new PlanItem{ID = "Q4", Label="Question Four", StoryId="4", Duration="4"}
            })
            };


            return base.OnInitializedAsync();
        }

        PlanItem? selectedItemRow = new();
        PlanItem? SelectedItemRow
        {
            get => selectedItemRow;
            set
            {
                selectedItemRow = value;

                upAllowed = false;
                downAllowed = false;


                if (selectedItemRow != null)
                {
                    int? numItems = Document?.Items?.Count;

                    if (numItems > 1)
                    {
                        int rowPosition = GetPosition(selectedItemRow.ID);
                        if (rowPosition != -1)  // there is a selection
                        {
                            if (rowPosition > 0)
                            {
                                upAllowed = true;
                            }
                            if (rowPosition < numItems - 1)
                            {
                                downAllowed = true;
                            }
                        }
                    }
                }
            }
        }

        private int GetPosition(string itemId)
        {
            int rtnVal = -1;    // not found

            for (int pos = 0; pos < Document?.Items?.Count; pos++)
            {
                if (Document?.Items[pos].ID == itemId)
                {
                    rtnVal = pos;
                }
            }

            return rtnVal;
        }

        private Task ItemUp()
        {
            if (SelectedItemRow != null)
            {
                int position = GetPosition(SelectedItemRow.ID);

                if (position != -1)
                {
                    Document?.Items.Remove(SelectedItemRow);
                    Document?.Items.Insert(position - 1, SelectedItemRow);
                    // enable buttons appropriately
                    SelectedItemRow = SelectedItemRow;
                }
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ItemDown()
        {
            if (SelectedItemRow != null)
            {
                int position = GetPosition(SelectedItemRow.ID);

                if (position != -1)
                {
                    Document?.Items.Remove(SelectedItemRow);
                    Document?.Items.Insert(position + 1, SelectedItemRow);
                    // enable buttons appropriately
                    SelectedItemRow = SelectedItemRow;
                }
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }


        bool upAllowed { get; set; } = false;
        bool downAllowed { get; set; } = false;
        PlanDocument Document { get; set; } = new();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;

        private PlanItemModal? planItemModalRef;

        string selectedTab = "general";

        private async Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                //GenerateMarkdown();
                if((MermaidOne != null) &&(Document != null))
                    await MermaidOne.DisplayDiagram(PlanPublisher.Publish(Document));
            }

            //return Task.CompletedTask;
        }

        private Task ShowItemModal()
        {
            if (SelectedItemRow == null)
            {
                return Task.CompletedTask;
            }
            //planItemModalRef.Item = SelectedItemRow;

            planItemModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewItem()
        {
            PlanItem newItem = new()
            {
                ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper(),
                Label = "New Plan Item",
                Duration = "1"
            };

            SelectedItemRow = newItem;
            Document?.Items?.Add(newItem);
            adding = true;

            return ShowItemModal();
        }

        private Task OnPlanItemModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (planItemModalRef?.Result == ModalResult.Cancel)
                {
                    if(SelectedItemRow != null)
                        Document?.Items?.Remove(SelectedItemRow);
                    SelectedItemRow = null;
                }
            }
            adding = false;

            return Task.CompletedTask;
        }

        private Task DeleteItem()
        {
            // remove the new item, if add was cancelled
            if (SelectedItemRow != null)
            {
                Document?.Items?.Remove(SelectedItemRow);
                SelectedItemRow = null;
            }

            return Task.CompletedTask;
        }

        public static string CalculateLink(string? baseUrl, string storyId)
        {
            return baseUrl + "/" + storyId;
        }

        //private string DecodePlanId(string id)
        //{
        //    string rtnVal = id;

        //    if (Document != null)
        //    {
        //        foreach (PlanItem fi in Document.Items)
        //        {
        //            if (fi.ID == id)
        //            {
        //                rtnVal = (fi.Label == null) ? "" : fi.Label;
        //                break;
        //            }
        //        }
        //    }

        //    return rtnVal;
        //}

        FileManagementModal? fileManagementModalRef;

        Validations? validations;
        public async Task<bool> Validate()
        {
            bool rtnVal = false;
            if (validations != null)
            {
                if (await validations.ValidateAll())
                {
                    rtnVal = true;
                }
            }
            else
            {
                rtnVal = true;
            }
            return rtnVal;
        }

        public async Task ResetValidation()
        {
            if (validations != null)
            {
                await validations.ClearAll();
            }
        }


        private Task NewPlan()
        {
            if(fileManagementModalRef != null)
                fileManagementModalRef.Name = "New_Plan.json";

            Document = new()
            {
                Title = "Hello Brad",
                StartDate = ToIsoString(DateTime.Today),
                BaseUrl = "https://gimme",
                Items = new List<PlanItem>()
            };

            _ = ResetValidation();

            return Task.CompletedTask;
        }

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
            string fileText = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //flowItemModalRef.item = selectedItemRow;

            fileManagementModalRef?.SaveFile(fileText);

            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ExportFile()
        {
            if (Validate().Result)
            {
                GenerateMarkdown();

                if (fileManagementModalRef != null)
                {
                    fileManagementModalRef.Name = "plan.md";
                    fileManagementModalRef.SaveFile(MarkdownText);
                }
            }

            return Task.CompletedTask;
        }

        private Task ExportHtml()
        {
            if (Validate().Result)
            {
                string htmlText = GenerateHtml().Result;

                if (fileManagementModalRef != null)
                {
                    fileManagementModalRef.Name = "plan.html";
                    fileManagementModalRef.SaveFile(htmlText);
                }
            }
            return Task.CompletedTask;
        }


        private Task OnFileManagementModalClosed()
        {
            if (fileManagementModalRef?.Result == ModalResult.OK)
            {
                //MarkdownText = fileManagementModalRef.FileText;
                if (fileManagementModalRef.FileText != null)
                {
                    Document = (JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(PlanDocument)) as PlanDocument) ?? new PlanDocument();
                }
                InvokeAsync(() => StateHasChanged());
            }

            return Task.CompletedTask;
        }

        //        private string GenerateMermaidText(FlowDocument document)
        private Task GenerateMarkdown()
        {
            //mermaidOne.DisplayDiagram(PlanPublisher.Publish(Document));
            //mermaidText = PlanPublisher.Publish(Document);
            if (Document != null)
                MarkdownText = MarkdownGenerator.WrapMermaid(PlanPublisher.Publish(Document));
//            markdownRef.Value = MarkdownText;
            return Task.CompletedTask;
        }
        private Task<string> GenerateHtml()
        {
            string rtnVal = string.Empty;
            if (Document != null)
                rtnVal = HtmlGenerator.WrapMermaid(PlanPublisher.Publish(Document));
            return Task.FromResult(rtnVal);
        }

        public static string ToIsoString(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int dt = date.Day;

            string dtString = dt.ToString();
            string monthString = month.ToString();

            if (dt< 10)
            {
                dtString = '0' + dt.ToString();
            }
            if (month< 10)
            {
                monthString = '0' + month.ToString();
            }

            return (year.ToString() + '-' + monthString + '-' + dtString);
        }
    }
}
