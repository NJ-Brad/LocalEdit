﻿using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;
using LocalEdit.PlanTypes;
using Blazorise.Components;
using System.Text.Json;
//using StardustDL.RazorComponents.Markdown;
using LocalEdit.Shared;
using Blazorise.DataGrid;
using System.Threading.Channels;
using System.Xml.Linq;

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
        Mermaid? MermaidTwo { get; set; }
        SvgDisplay? SvgDisplayOne { get; set; }

        private WIPManager? wIPManagerRef;

        private string lastSavedDocumentText = "";

        bool useBuiltInEditor = false;
        private DataGridEditMode sprintEditMode = DataGridEditMode.Inline;

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
                //new PlanItem{ID = "Q2", Label="Question Two", StoryId="2", Duration="2", Dependencies = new List<PlanItemDependency>(new[]
                //{
                //    new PlanItemDependency{ ID = "Q1", DependencyType="ITEM"}
                //}) },
                new PlanItem{ID = "Q2", Label="Question Two", StoryId="2", Duration="2"},
                new PlanItem{ID = "Q3", Label="Question Three", StoryId="3", Duration="3"},
                new PlanItem{ID = "Q4", Label="Question Four", StoryId="4", Duration="4"}
            }),
                Sprints = new List<Sprint>(new[]
                {
                    new Sprint{Label = "Sprint 1", StartDate=new DateOnly(2022, 11,  04), EndDate=new DateOnly(2022, 11,  5)},
                    new Sprint{Label = "Sprint 2", StartDate=new DateOnly(2022, 11,  06), EndDate=new DateOnly(2022, 11,  7)},
                    new Sprint{Label = "Sprint 3", StartDate=new DateOnly(2022, 11,  8), EndDate=new DateOnly(2022, 11,  8)},
                    new Sprint{Label = "Sprint 4", StartDate=new DateOnly(2022, 11,  9), EndDate=new DateOnly(2022, 11,  10)}
                })
            };

            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (await wIPManagerRef.DataExists())
                {
                    //messageBoxRef.ShowModal();
                    // last button will be "primary"
                    // messageBoxRef.ShowModal("MYMESSAGE", "Parameter Title", "Parameter Question Text", new List<string>(){ "Cancel", "Get Rekt", "OK" });
                    messageBoxRef.ShowModal("RESUME_WIP", "Resume", "Would you like to pick up where you left off?<br/>Select Yes, if you have not saved your work.", new List<string>() { "No", "Yes" });
                }
                else
                {
                    wIPManagerRef.Start();
                }
            }
            base.OnAfterRender(firstRender);
        }

        //PlanItem? selectedItemRow = new();
        PlanItem? selectedItemRow = null;
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

        Sprint? selectedSprintRow = new();
        Sprint? SelectedSprintRow
        {
            get => selectedSprintRow;
            set
            {
                selectedSprintRow = value;

                sprintUpAllowed = false;
                sprintDownAllowed = false;


                if (selectedSprintRow != null)
                {
                    int? numItems = Document?.Sprints?.Count;

                    if (numItems > 1)
                    {
                        int rowPosition = GetSprintPosition(selectedSprintRow.ID);
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

        private int GetSprintPosition(string itemId)
        {
            int rtnVal = -1;    // not found

            for (int pos = 0; pos < Document?.Sprints?.Count; pos++)
            {
                if (Document?.Sprints[pos].ID == itemId)
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

        private Task SprintUp()
        {
            if (SelectedSprintRow != null)
            {
                int position = GetSprintPosition(SelectedSprintRow.ID);

                if (position != -1)
                {
                    Document?.Sprints.Remove(SelectedSprintRow);
                    Document?.Sprints.Insert(position - 1, SelectedSprintRow);
                    // enable buttons appropriately
                    SelectedSprintRow = SelectedSprintRow;
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

        private Task SprintDown()
        {
            if (SelectedSprintRow != null)
            {
                int position = GetSprintPosition(SelectedSprintRow.ID);

                if (position != -1)
                {
                    Document?.Sprints.Remove(SelectedSprintRow);
                    Document?.Sprints.Insert(position + 1, SelectedSprintRow);
                    // enable buttons appropriately
                    SelectedSprintRow = SelectedSprintRow;
                }
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }


        bool upAllowed { get; set; } = false;
        bool downAllowed { get; set; } = false;
        bool sprintUpAllowed { get; set; } = false;
        bool sprintDownAllowed { get; set; } = false;


        PlanDocument Document { get; set; } = new();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;
        bool addingSprint = false;

        private PlanItemModal? planItemModalRef;
        private SprintModal? sprintModalRef;
        private PlanExportModal? planExportModalRef;

        string selectedTab = "general";

        //        int tabSwitchesToIgnore = 0;
        MarkupString timelineSvg { get; set; }

        private async Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                if ((MermaidOne != null) && (Document != null))
                {
                    await MermaidOne.DisplayDiagram(PlanPublisher.Publish(Document));
                }
            }

            if (selectedTab == "preview2")
            {
                if ((MermaidTwo != null) && (Document != null))
                {
                    // this wonky work around takes care of an issue where it takes THREE TRIES to get the diagram to update
                    int tries = 0;
                    bool worked = false;

                    while (!worked && (tries < 4))
                    {
                        try
                        {
                            //string input = TimelinePublisher.Publish(Document);
                            //await MermaidTwo.DisplayDiagram(input);

                            await MermaidTwo.DisplayDiagram(TimelinePublisher.Publish(Document));

                            ////SvgDisplayOne.ChildContent = new MarkupString(MermaidTwo.SvgText);
                            ////timelineSvg = AddContent(MermaidTwo.SvgText);
                            //timelineSvg = new MarkupString(MermaidTwo.SvgText);

                            //timelineSvg = new MarkupString(MermaidTwo.GenerateSvg(TimelinePublisher.Publish(Document)));
                            //timelineSvg = new MarkupString(MermaidTwo.SvgText);
                            worked = true;
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine(ex.ToString());
                            tries++;
                        }
                        // <circle cx="50" cy="50" r="40" stroke="green" stroke-width="4" fill="yellow" />
                        // https://stackoverflow.com/questions/60785749/using-svgs-in-blazor-page#60787289
                    }
                }
            }

            //return Task.CompletedTask;
        }

        private RenderFragment AddContent(string textContent) => builder =>
        {
            builder.AddContent(1, textContent);
        };


        private async Task RefreshTimeline()
        {
            if ((MermaidTwo != null) && (Document != null))
            {
                string input = TimelinePublisher.Publish(Document);
                MermaidTwo.DisplayDiagram(input);
            }
        }

        private Task ShowItemModal()
        {
            if (SelectedItemRow == null)
            {
                return Task.CompletedTask;
            }

            planItemModalRef?.ShowModal();

            return Task.CompletedTask;
        }

        private Task ShowSprintModal()
        {
            if (SelectedSprintRow == null)
            {
                return Task.CompletedTask;
            }

            sprintModalRef?.ShowModal();

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
        private Task AddNewSprint()
        {
            Sprint newItem = new()
            {
                ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper(),
                Label = "New Sprint",
                StartDate = DateOnly.FromDateTime( DateTime.Now.Date),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7).Date)
            };

            SelectedSprintRow = newItem;
            Document?.Sprints?.Add(newItem);
            addingSprint = true;

            return ShowSprintModal();
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
                    //SelectedItemRow = null;
                    SelectedItemRow = new();
                }
            }
            adding = false;

            return Task.CompletedTask;
        }

        private Task OnSprintModalClosed()
        {
            if (addingSprint)
            {
                // remove the new item, if add was cancelled
                if (sprintModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedSprintRow != null)
                        Document?.Sprints?.Remove(SelectedSprintRow);
                    SelectedSprintRow = null;
                }
            }
            addingSprint = false;

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

        private Task DeleteSprint()
        {
            // remove the new item, if add was cancelled
            if (SelectedSprintRow != null)
            {
                Document?.Sprints?.Remove(SelectedSprintRow);
                SelectedSprintRow = null;
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
        MessageBox? messageBoxRef;

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


        private async Task NewPlan()
        {
            if ((await IsDocumentDirty()) && (await wIPManagerRef.DataExists()))
                {
                    //messageBoxRef.ShowModal();
                    // last button will be "primary"
                    // messageBoxRef.ShowModal("MYMESSAGE", "Parameter Title", "Parameter Question Text", new List<string>(){ "Cancel", "Get Rekt", "OK" });
                    messageBoxRef.ShowModal("NEW_PLAN", "New Plan", "Would you like to save your current work before creating a new plan.", new List<string>() { "No", "Yes" });
                }
            else
            {
                ImplementNewPlan();
            }

            //if (fileManagementModalRef != null)
            //    fileManagementModalRef.Name = "New_Plan.json";

            //Document = new()
            //{
            //    Title = "Hello Brad",
            //    StartDate = ToIsoString(DateTime.Today),
            //    BaseUrl = "https://gimme",
            //    Items = new List<PlanItem>()
            //};

            //_ = ResetValidation();

            //            return Task.CompletedTask;
        }

        private async Task Export()
        {
            planExportModalRef.ShowModal();
        }

        private async Task LoadFile()
        {
            if ((await IsDocumentDirty()) && (await wIPManagerRef.DataExists()))
            {
                //messageBoxRef.ShowModal();
                // last button will be "primary"
                // messageBoxRef.ShowModal("MYMESSAGE", "Parameter Title", "Parameter Question Text", new List<string>(){ "Cancel", "Get Rekt", "OK" });
                messageBoxRef.ShowModal("LOAD_PLAN", "Load Plan", "Would you like to save your current work before loading a different plan.", new List<string>() { "No", "Yes" });
            }
            else
            { 
                ImplementLoadPlan();
            }
            //fileManagementModalRef?.LoadFile();

//            return Task.CompletedTask;
        }

        private async Task<bool>IsDocumentDirty()
        {
            string fileText = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;

            return (lastSavedDocumentText != fileText);
        }

        private Task StartWIP()
        {
            wIPManagerRef.Start();
            return Task.CompletedTask;
        }

        private Task LoadWIP()
        {
            wIPManagerRef.Load();
            // we don't know if it has been saved
            lastSavedDocumentText = "";
            return Task.CompletedTask;
        }

        private Task SaveWIP()
        {
            string fileText = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;
            wIPManagerRef.Data = fileText;
            //            wIPManagerRef.Load();
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
            lastSavedDocumentText = fileText;

            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ExportGanttMarkdown()
        {
            if (Validate().Result)
            {
                GenerateGanntMarkdown();

                if (fileManagementModalRef != null)
                {
                    fileManagementModalRef.Name = "plan.md";
                    fileManagementModalRef.SaveFile(MarkdownText);
                }
            }

            return Task.CompletedTask;
        }

        private Task ExportGanttHtml()
        {
            if (Validate().Result)
            {
                string htmlText = GenerateGanttHtml().Result;

                if (fileManagementModalRef != null)
                {
                    fileManagementModalRef.Name = "plan.html";
                    fileManagementModalRef.SaveFile(htmlText);
                }
            }
            return Task.CompletedTask;
        }

        private Task ExportTimelineMarkdown()
        {
            if (Validate().Result)
            {
                GenerateTimelineMarkdown();

                if (fileManagementModalRef != null)
                {
                    fileManagementModalRef.Name = "plan.md";
                    fileManagementModalRef.SaveFile(MarkdownText);
                }
            }

            return Task.CompletedTask;
        }

        private Task ExportTimelineHtml()
        {
            if (Validate().Result)
            {
                string htmlText = GenerateTimelineHtml().Result;

                if (fileManagementModalRef != null)
                {
                    fileManagementModalRef.Name = "plan.html";
                    fileManagementModalRef.SaveFile(htmlText);
                }
            }
            return Task.CompletedTask;
        }
        private Task OnWipDataRequired()
        {
            //if (fileManagementModalRef?.Result == ModalResult.OK)
            //{
            //    //MarkdownText = fileManagementModalRef.FileText;
            //    if (fileManagementModalRef.FileText != null)
            //    {
            //        Document = (JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(PlanDocument)) as PlanDocument) ?? new PlanDocument();
            //    }
            //    InvokeAsync(() => StateHasChanged());
            //}

            wIPManagerRef.Data = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;

            return Task.CompletedTask;
        }


        private Task OnWipDataReady()
        {
            //if (fileManagementModalRef?.Result == ModalResult.OK)
            //{
            //    //MarkdownText = fileManagementModalRef.FileText;
            //    if (fileManagementModalRef.FileText != null)
            //    {
            //        Document = (JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(PlanDocument)) as PlanDocument) ?? new PlanDocument();
            //    }
            //    InvokeAsync(() => StateHasChanged());
            //}

            Document = (JsonSerializer.Deserialize(wIPManagerRef.Data, typeof(PlanDocument)) as PlanDocument) ?? new PlanDocument();
            InvokeAsync(() => StateHasChanged());

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

        private Task OnPlanExportClosed()
        {
            switch(planExportModalRef.DiagramType)
            {
                case "gantt":
                    switch (planExportModalRef.OutputFormat)
                    {
                        case "html":
                            ExportGanttHtml();
                            break;
                        case "markdown":
                            ExportGanttMarkdown();
                            break;
                    }
                    break;
                case "timeline":
                    switch (planExportModalRef.OutputFormat)
                    {
                        case "html":
                            ExportTimelineHtml();
                            break;
                        case "markdown":
                            ExportTimelineMarkdown();
                            break;
                    }
                    break;
            }
            //planExportModalRef.OutputFormat

            return Task.CompletedTask;

    //< Button Size = "Size.Small" Color = "Color.Primary" Clicked = "@ExportGanttMarkdown" >
    //    < Icon Name = "IconName.ArrowRight" />
    //    Gantt(Markdown)
    //</ Button >
    //< Button Size = "Size.Small" Color = "Color.Primary" Clicked = "@ExportTimelineMarkdown" >
    //    < Icon Name = "IconName.ArrowRight" />
    //    Timeline(Markdown)
    //</ Button >
    //< Button Size = "Size.Small" Color = "Color.Primary" Clicked = "@ExportGanttHtml" >
    //    < Icon Name = "IconName.ArrowRight" />
    //    Gantt(HTML)
    //</ Button >
    //< Button Size = "Size.Small" Color = "Color.Primary" Clicked = "@ExportTimelineHtml" >
    //    < Icon Name = "IconName.ArrowRight" />
    //    Timeline(HTML)
    //</ Button >

        }

        private Task OnMessageBoxClosed()
        {
            switch(messageBoxRef?.MessageBoxID)
            {
                case "RESUME_WIP":
                    switch (messageBoxRef?.Result)
                    {
                        case ModalResult.ButtonTwo:
                            wIPManagerRef.Load();
                            //lastSavedDocumentText = wIPManagerRef.Data;
                            lastSavedDocumentText = "";
                            break;
                    }
                    wIPManagerRef.Start();
                    break;
                case "LOAD_PLAN":
                    switch (messageBoxRef?.Result)
                    {
                        case ModalResult.ButtonOne:
                            ImplementLoadPlan();
                            break;
                        case ModalResult.ButtonTwo:
                            wIPManagerRef.Load();
                            //lastSavedDocumentText = wIPManagerRef.Data;
                            lastSavedDocumentText = "";
                            break;
                    }
                    break;
                case "NEW_PLAN":
                    switch (messageBoxRef?.Result)
                    {
                        case ModalResult.ButtonOne:
                            ImplementNewPlan();
                            break;
                        case ModalResult.ButtonTwo:
                            wIPManagerRef.Load();
                            //lastSavedDocumentText = wIPManagerRef.Data;
                            lastSavedDocumentText = "";
                            break;
                    }
                    break;

        }
            //// "RESUME_WIP"
            //switch (messageBoxRef?.Result)
            //{
            //    case ModalResult.ButtonOne:
            //        break;
            //    case ModalResult.ButtonTwo:
            //        break;
            //    case ModalResult.ButtonThree:
            //        break;
            //    case ModalResult.ButtonFour:
            //        break;
            //    case ModalResult.ButtonFive:
            //        break;
            //    default:
            //        break;
            //}


            if (messageBoxRef?.Result == ModalResult.OK)
            {
                ////MarkdownText = fileManagementModalRef.FileText;
                //if (fileManagementModalRef.FileText != null)
                //{
                //    Document = (JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(PlanDocument)) as PlanDocument) ?? new PlanDocument();
                //}
                //InvokeAsync(() => StateHasChanged());
            }

            return Task.CompletedTask;
        }

        private void ImplementLoadPlan()
        {
            fileManagementModalRef?.LoadFile();
            lastSavedDocumentText = wIPManagerRef.Data;
        }

        void ImplementNewPlan()
        {
            if (fileManagementModalRef != null)
                fileManagementModalRef.Name = "New_Plan.json";

            Document = new()
            {
                Title = "Hello Brad",
                StartDate = ToIsoString(DateTime.Today),
                BaseUrl = "https://gimme",
                Items = new List<PlanItem>()
            };

            lastSavedDocumentText = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;
            _ = ResetValidation();
        }

        //        private string GenerateMermaidText(FlowDocument document)
        private Task GenerateGanntMarkdown()
        {
            //mermaidOne.DisplayDiagram(PlanPublisher.Publish(Document));
            //mermaidText = PlanPublisher.Publish(Document);
            if (Document != null)
                MarkdownText = MarkdownGenerator.WrapMermaid(PlanPublisher.Publish(Document));
//            markdownRef.Value = MarkdownText;
            return Task.CompletedTask;
        }
        private Task<string> GenerateGanttHtml()
        {
            string rtnVal = string.Empty;
            if (Document != null)
                rtnVal = HtmlGenerator.WrapMermaid(PlanPublisher.Publish(Document));
            return Task.FromResult(rtnVal);
        }

        private Task GenerateTimelineMarkdown()
        {
            //mermaidOne.DisplayDiagram(PlanPublisher.Publish(Document));
            //mermaidText = PlanPublisher.Publish(Document);
            if (Document != null)
                MarkdownText = MarkdownGenerator.WrapMermaid(TimelinePublisher.Publish(Document));
            //            markdownRef.Value = MarkdownText;
            return Task.CompletedTask;
        }
        private Task<string> GenerateTimelineHtml()
        {
            string rtnVal = string.Empty;
            if (Document != null)
                rtnVal = HtmlGenerator.WrapMermaid(TimelinePublisher.Publish(Document));
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
