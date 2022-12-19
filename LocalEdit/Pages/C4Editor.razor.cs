using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;
using System.Text.Json;
//using StardustDL.RazorComponents.Markdown;
using LocalEdit.Shared;

namespace LocalEdit.Pages
{
    public partial class C4Editor : ComponentBase
    {
        // need a state diagram for add / edit

        //** Edit in place **//
        // Add
        // Click Add
        // Create new record
        // Open editor
        // When editor closes
        // If cancelled - exit
        // else add record to tree and model

        // Update
        // click edit
        // open editor
        // When editor closes
        // If cancelled - revert changes from model back to tree - Model = undo for any edits that were made
        // else update model to match tree

        //** Edit copy **//
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

        //void SelChanged(C4Item item)
        //{
        //    this.SelectedNode = item;
        //    parentNode = FindParent(this.SelectedNode, Document.Model);
        //}

        C4Workspace? Document { get; set; } = new C4Workspace();
//        MarkdownRenderer markdownRef;
        C4ItemEditModal? c4ItemModalRef = null;

        private Task ShowItemModal()
        {
            if (SelectedNode == null)
            {
                return Task.CompletedTask;
            }

            if (c4ItemModalRef != null)
            {
                c4ItemModalRef.ParentType = parentNode == null ? C4TypeEnum.Unknown : parentNode.ItemType;
                c4ItemModalRef.SelectedNode = SelectedNode;

                c4ItemModalRef?.ShowModal();
            }

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        bool adding;

        // Is this a case where I need to wait to create the new item?
        // For consistency, i should create the item, then remove it on close, if I hit cancel
        // I could create the item, once I know the type...  Bleep.  I know that I am creating an item.
        // I just dow't know the node type
        // If the user cancels on the node type selection, call the cancel method
        // This will allow the main editor to only need to call the single modal

        private Task AddNewItem()
        {
            C4Item newItem = new C4Item();
            //newItem.ItemType = FlowItemType.Question;
            //newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
            //newItem.Label = "New Question";

            Document.Model.Add(newItem);
            SelectedNode = newItem;
            adding = true;

            InvokeAsync(() => StateHasChanged());

            return ShowItemModal();
        }

        private Task AddNewChildItem()
        {
            C4Item newItem = new C4Item();
            //newItem.ItemType = FlowItemType.Question;
            //newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
            //newItem.Label = "New Question";

            parentNode = SelectedNode;

            SelectedNode.Children.Add(newItem);
            SelectedNode = newItem;
            adding = true;

            InvokeAsync(() => StateHasChanged());

            return ShowItemModal();
        }

        private C4Item FindParent(C4Item NodeInQuestion, IEnumerable<C4Item> collection)
        {
            C4Item? parentNode = null;

            foreach (C4Item potential in collection)
            {
                // if it doesn't have childrent, it cannot be a parent
                if(potential.Children.Count() > 0)
                {
                    foreach (C4Item child in potential.Children)
                    {
                        if (child.Alias == NodeInQuestion.Alias)
                        {
                            parentNode = potential;
                        }
                        else
                        {
                            parentNode = FindParent(NodeInQuestion, child.Children);
                        }

                        if (parentNode != null)
                            break;
                    }
                }
                if (parentNode != null)
                    break;
            }

            return parentNode;
        }

        IEnumerable<C4Item> C4Items1 = new[]
        {
            C4TestData.InternalPerson,
            C4TestData.ExternalPerson,
            C4TestData.Boundary,
            C4TestData.SystemBoundary,
            C4TestData.EnterpriseBoundary,
            C4TestData.ContainerBoundary,
            C4TestData.Component,
            C4TestData.Database,
            C4TestData.Container,
            C4TestData.Node,
            C4TestData.InternalSystem,
            C4TestData.ExternalSystem,
            C4TestData.InternalDatabaseSystem,
            C4TestData.ExternalDatabaseSystem
        };


        //    IEnumerable<Item> Items = new[]
        //       {
        //    new Item { Text = "Item 1" },
        //    new Item {
        //        Text = "Item 2",
        //        Children = new []
        //{
        //            new Item { Text = "Item 2.1" },
        //            new Item { Text = "Item 2.2", Children = new []
        //    {
        //                new Item { Text = "Item 2.2.1" },
        //                new Item { Text = "Item 2.2.2" },
        //                new Item { Text = "Item 2.2.3" },
        //                new Item { Text = "Item 2.2.4" }
        //            }
        //        },
        //        new Item { Text = "Item 2.3" },
        //        new Item { Text = "Item 2.4" }
        //        }
        //    },
        //    new Item { Text = "Item 3" },
        //};

        private Task NewC4Document()
        {
            Document.Model = new List<C4Item>(new[]
{
            new C4Item{ItemType=C4TypeEnum.Person, Text="Customer", Description="A customer of the bank, with personal bank accounts", IsExternal=true},
            new C4Item{ItemType=C4TypeEnum.EnterpriseBoundary, Text="Internet Banking",
                Children=new List<C4Item>( new[]{
                    new C4Item{ItemType=C4TypeEnum.Container, Text ="Web Application", Technology="Java, Spring MVC", Description="Delivers the static content and the Internet banking SPA" }
                })
            }
        });
            return Task.CompletedTask;
        }

        private Task LoadFile()
        {
            fileManagementModalRef?.LoadFile();

            return Task.CompletedTask;
        }

        FileManagementModal fileManagementModalRef;

        private Task OnFileManagementModalClosed()
        {
            if (fileManagementModalRef.Result == ModalResult.OK)
            {
                Document = (C4Workspace)JsonSerializer.Deserialize(fileManagementModalRef.FileText, typeof(C4Workspace));
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

        C4Item? selectedNode = null;
        C4Item? parentNode = null;
        C4Item? potentialParentNode = null;
        C4Item? newItem = null;

        //private Modal? modalRef;
        private Modal? NewItemModalRef;
        //private Modal? C4ItemModalRef;


        //private bool cancelClose;

        //private bool modalVisible;
        //private bool newItemModalVisible;

        //private bool cancelled = false;

        private Task ShowModal()
        {
//            modalVisible = true;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        //        private Task ShowNewItemModal()
        private Task ShowNewItemModal(C4Item parentNode)
        {
            potentialParentNode = parentNode;
            
            //newItemModalVisible = true;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        //private Task HideModal()
        //{
        //    modalVisible = false;

        //    return Task.CompletedTask;
        //}

        //private Task CloseModal()
        //{
        //    // possibly add a check for changed and prompt to lose changes

        //    cancelClose = false;
        //    cancelled = true;

        //    return modalRef.Hide();
        //}

        C4TypeEnum? newItemType;

        private Task CloseNewItemModal()
        {
            newItemType = null;

            return NewItemModalRef.Close(CloseReason.EscapeClosing);
        }

        private Task CreateItem(C4TypeEnum itemType)
        {
            newItemType = itemType;

            newItem = new C4Item() { ItemType = itemType };
//            selectedNode = newItem;

            return NewItemModalRef.Close(CloseReason.EscapeClosing);
        }

        private bool ShouldShow(C4TypeEnum itemType, C4Item? parentNode)
        {
            bool rtnVal = false;

            if (parentNode == null)
            {
                switch (itemType)
                {
                    case C4TypeEnum.Person:
                    case C4TypeEnum.System:
                    case C4TypeEnum.EnterpriseBoundary:
                        rtnVal = true;
                        break;
                }
            }
            else
            {
                switch (parentNode.ItemType)
                {
                    case C4TypeEnum.Person:
                        // nothing is allowed
                        break;
                    case C4TypeEnum.System:
                        switch (itemType)
                        {
                            case C4TypeEnum.Container:
                            case C4TypeEnum.Database:
                                rtnVal = true;
                                break;
                        }
                        break;
                    case C4TypeEnum.EnterpriseBoundary:
                        switch (itemType)
                        {
                            case C4TypeEnum.Person:
                            case C4TypeEnum.System:
                                rtnVal = true;
                                break;
                        }
                        break;
                    case C4TypeEnum.Container:
                        switch (itemType)
                        {
                            case C4TypeEnum.Component:
                                rtnVal = true;
                                break;
                        }
                        break;

                }
            }

            return rtnVal;

        }

        //private async Task TryCloseModal()
        //{
        //    // add a check for validity

        //    cancelClose = true;

        //    if (await propEditor.IsValid())
        //    {
        //        cancelClose = false;
        //    }

        //    await modalRef.Hide();
        //}

//        private Task OnModelOpened()
//        {
//            // reset, for the next attempt to close
//            cancelClose = false;
////            propEditor.ResetValidation();
//            cancelled = false;

//            return Task.CompletedTask;
//        }

        private Task OnNewItemModalOpened()
        {
            //// reset, for the next attempt to close
            //cancelClose = false;
            ////            propEditor.ResetValidation();
            //cancelled = false;

            return Task.CompletedTask;
        }

        string MarkdownText { get; set; } = string.Empty;

        Mermaid mermaidOne;
        Mermaid mermaidTwo;
        Mermaid mermaidThree;

        string diagramOneDefinition = "One";
        string diagramTwoDefinition = "Two";
        string diagramThreeDefinition = "Three";

        void OnClickNode(string nodeId)
        {
            // TODO: do something with nodeId
        }

        public C4Item SelectedNode { get => selectedNode; 
            set
            {
                selectedNode = value; 
                parentNode = FindParent(value, Document.Model);
            }
        }

        //private Task OnModalClosing(ModalClosingEventArgs e)
        //{
        //    // just set Cancel to prevent modal from closing

        //    if (e.CloseReason == CloseReason.EscapeClosing)
        //    {
        //        CloseModal();
        //    }

        //    if (cancelClose || e.CloseReason != CloseReason.UserClosing)
        //    {
        //        e.Cancel = true;
        //    }

        //    return Task.CompletedTask;
        //}

        private Task OnNewItemModalClosed()
        {
            if(newItemType != null)
            {
                ShowModal();
            }
            return Task.CompletedTask;
        }


        
            private Task OnC4ItemModalClosed()
        { 
            if(adding)
            {
                // remove the new item, if add was cancelled
                if (c4ItemModalRef.Result == ModalResult.Cancel)
                {
                    Document.Model.Remove(SelectedNode);
                    SelectedNode = null;
                }
}
adding = false;

InvokeAsync(() => StateHasChanged());

return Task.CompletedTask;
}

        C4Relationship selectedRelationshipRow { get; set; }

        // this will need to go deeper, to find child items
        private string DecodeFlowId(string id)
        {
            string rtnVal = id;

            //foreach (C4Item fi in Document.Model)
            foreach (C4Item fi in c4RelationshipModalRef.AllItems)
            {
                if (fi.Alias == id)
                {
                    rtnVal = fi.Text;
                    break;
                }
            }

            return rtnVal;
        }

        private Task AddNewRelationship()
        {
            C4Relationship newRelationship = new C4Relationship();
            newRelationship.Text = "New Relationship";

            selectedRelationshipRow = newRelationship;
            Document.Relationships.Add(newRelationship);
            adding = true;

            return ShowRelationshipModal();
        }

        C4RelationshipModal c4RelationshipModalRef = null;

        private Task ShowRelationshipModal()
        {
            if (selectedRelationshipRow == null)
            {
                return Task.CompletedTask;
            }
            c4RelationshipModalRef.Item = selectedRelationshipRow;

            c4RelationshipModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnC4RelationshipModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (c4RelationshipModalRef.Result == ModalResult.Cancel)
                {
                    Document.Relationships.Remove(selectedRelationshipRow);
                    selectedRelationshipRow = null;
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }


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


        private Task OnNewItemModalClosing(ModalClosingEventArgs e)
        {
            // just set Cancel to prevent modal from closing

            //if (e.CloseReason == CloseReason.EscapeClosing)
            //{
            //    CloseModal();
            //}

            //if (cancelClose || e.CloseReason != CloseReason.UserClosing)
            //{
            //    e.Cancel = true;
            //}

            return Task.CompletedTask;
        }

        private Task DeleteItem()
        {
            if (SelectedNode != null)
            {
                if (parentNode != null)
                {
                    parentNode.Children.Remove(SelectedNode);
                }
                else
                {
                    Document.Model.Remove(SelectedNode);
                }
                SelectedNode = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        protected string testVal { get; set; }
        public Mermaid? MermaidOne { get => mermaidOne; set => mermaidOne = value; }
        public Mermaid? MermaidTwo { get => mermaidTwo; set => mermaidTwo = value; }
        public Mermaid? MermaidThree { get => mermaidThree; set => mermaidThree = value; }

        private async Task GenerateMarkdown()
        {
            //MarkdownText = MarkdownGenerator.WrapMermaid(C4Publisher.Publish(Document));

            //MarkdownText = MarkdownGenerator.WrapMermaid(C4Publisher.Publish(Document, "Context"));
            //testVal = MarkdownText;

            //MarkdownText = MarkdownGenerator.WrapMermaid("Context Diagram", C4PublisherLegacy.Publish(Document, "Context"),
            //    "Container Diagram", C4PublisherLegacy.Publish(Document, "Container"),
            //    "Component Diagram", C4PublisherLegacy.Publish(Document, "Component"));

            MarkdownText = MarkdownGenerator.WrapMermaid("Context Diagram", C4Publisher.Publish(Document, "Context"),
                "Container Diagram", C4Publisher.Publish(Document, "Container"),
                "Component Diagram", C4Publisher.Publish(Document, "Component"));

            //await mermaidOne.DisplayDiagram(C4Publisher.Publish(Document, "Context"));
            //await mermaidTwo.DisplayDiagram(C4Publisher.Publish(Document, "Container"));
            //await mermaidThree.DisplayDiagram(C4Publisher.Publish(Document, "Component"));

            //await mermaidOne.DisplayDiagram(C4PublisherLegacy.Publish(Document, "Context"));
            //await mermaidTwo.DisplayDiagram(C4PublisherLegacy.Publish(Document, "Container"));
            //await mermaidThree.DisplayDiagram(C4PublisherLegacy.Publish(Document, "Component"));

            //diagramOneDefinition = C4PublisherLegacy.Publish(Document, "Context");
            //diagramTwoDefinition = C4PublisherLegacy.Publish(Document, "Container");
            //diagramThreeDefinition = C4PublisherLegacy.Publish(Document, "Component");
            //testVal = diagramOneDefinition;

            //markdownRef.Value = MarkdownText;

            //            markdownRef.Value = @"# Preview not available:  
            //## The version of Mermaid used by this control is out of date";
            //return Task.CompletedTask;
        }

        private Task<string> GenerateHtml()
        {
            //            string htmlText = HtmlGenerator.WrapMermaid(C4Publisher.Publish(Document));

            string htmlText = HtmlGenerator.WrapMermaid("Context Diagram", C4Publisher.Publish(Document, "Context"),
                "Container Diagram", C4Publisher.Publish(Document, "Container"),
                "Component Diagram", C4Publisher.Publish(Document, "Component"));

            return Task.FromResult(htmlText);
        }

        private Task ExportFile()
        {
            //          if (Validate().Result)
            {
                GenerateMarkdown();

                fileManagementModalRef.Name = "Flow.md";
                fileManagementModalRef.SaveFile(MarkdownText);
            }

            return Task.CompletedTask;
        }

        private Task ExportHtml()
        {
            //            if (Validate().Result)
            {
                string htmlText = GenerateHtml().Result;

                fileManagementModalRef.Name = "flow.html";
                fileManagementModalRef.SaveFile(htmlText);
            }
            return Task.CompletedTask;
        }

        string selectedTab = "general";

        private async Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                GenerateMarkdown();
                InvokeAsync(() => StateHasChanged());

                await MermaidOne.DisplayDiagram(C4Publisher.Publish(Document, "Context"));
                await MermaidTwo.DisplayDiagram(C4Publisher.Publish(Document, "Container"));
                await MermaidThree.DisplayDiagram(C4Publisher.Publish(Document, "Component"));

            }

            return;
        }

    }
}
