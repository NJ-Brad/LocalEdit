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

        void Edit()
        {

        }

//        MarkdownRenderer markdownRef = null;
        //Mermaid mermaidOne;

        void OnClickNode(string nodeId)
        {
            // TODO: do something with nodeId
        }

    Mermaid MermaidOne { get; set; }


        protected override Task OnInitializedAsync()
        {
            Document.Title = "Hello Brad";
            Document.StartDate = "2022-11-06";
            Document.BaseUrl = "https://gimme";
            Document.Items = new List<PlanItem>(new[]
            {
                new PlanItem{ID = "Q1", Label="Question One", StoryId="1", Duration="1"},
                new PlanItem{ID = "Q2", Label="Question Two", StoryId="2", Duration="2", Dependencies = new List<PlanItemDependency>(new[]
                {
                    new PlanItemDependency{ ID = "Q1", DependencyType="ITEM"}
                }) },
                new PlanItem{ID = "Q3", Label="Question Three", StoryId="3", Duration="3"},
                new PlanItem{ID = "Q4", Label="Question Four", StoryId="4", Duration="4"}
            });


            return base.OnInitializedAsync();
        }

        PlanItem selectedItemRow { get; set; }

        PlanDocument Document { get; set; } = new PlanDocument();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;

        private PlanItemModal? planItemModalRef;

        string selectedTab = "general";

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                //GenerateMarkdown();
                MermaidOne.DisplayDiagram(PlanPublisher.Publish(Document));
            }

            return Task.CompletedTask;
        }

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
                    Document.Items.Remove(selectedItemRow);
                    selectedItemRow = null;
                }
            }
            adding = false;

            return Task.CompletedTask;
        }

        private Task DeleteItem()
        {
            // remove the new item, if add was cancelled
            if (selectedItemRow != null)
            {
                Document.Items.Remove(selectedItemRow);
                selectedItemRow = null;
            }

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

        Validations? validations;

        public async Task<bool> Validate()
        {
            bool rtnVal = false;
            if (await validations.ValidateAll())
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public async Task ResetValidation()
        {
            await validations.ClearAll();
        }


        private Task NewPlan()
        {
            fileManagementModalRef.Name = "New_Plan.json";

            Document.Title = "Hello Brad";
            Document.StartDate = ToIsoString(DateTime.Today);
            Document.BaseUrl = "https://gimme";
            Document.Items = new List<PlanItem>();

            ResetValidation();

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

            fileManagementModalRef.SaveFile(fileText);

            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ExportFile()
        {
            if (Validate().Result)
            {
                GenerateMarkdown();

                fileManagementModalRef.Name = "plan.md";
                fileManagementModalRef.SaveFile(MarkdownText);
            }

            return Task.CompletedTask;
        }

        private Task ExportHtml()
        {
            if (Validate().Result)
            {
                string htmlText = GenerateHtml().Result;

                fileManagementModalRef.Name = "plan.html";
                fileManagementModalRef.SaveFile(htmlText);
            }
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

            return Task.CompletedTask;
        }

        //        private string GenerateMermaidText(FlowDocument document)
        private Task GenerateMarkdown()
        {
            //mermaidOne.DisplayDiagram(PlanPublisher.Publish(Document));
            //mermaidText = PlanPublisher.Publish(Document);
            MarkdownText = MarkdownGenerator.WrapMermaid(PlanPublisher.Publish(Document));
//            markdownRef.Value = MarkdownText;
            return Task.CompletedTask;
        }
        private Task<string> GenerateHtml()
        {
            string htmlText = HtmlGenerator.WrapMermaid(PlanPublisher.Publish(Document));
            return Task.FromResult(htmlText);
        }

        public string ToIsoString(DateTime date)
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
