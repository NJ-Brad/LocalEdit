using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;

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

        void SelChanged(C4Item item)
        {
            this.selectedNode = item;
            parentNode = FindParent(this.selectedNode, C4Items);
        }

        private C4Item FindParent(C4Item NodeInQuestion, IEnumerable<C4Item> collection)
        {
            C4Item parentNode = null;

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

        IEnumerable<C4Item> C4Items = new[]
        {
            new C4Item{ItemType=C4TypeEnum.Person, Text="Customer", Description="A customer of the bank, with personal bank accounts", IsExternal=true},
            new C4Item{ItemType=C4TypeEnum.EnterpriseBoundary, Text="Internet Banking", 
                Children=new[]{
                    new C4Item{ItemType=C4TypeEnum.Container, Text ="Web Application", Technology="Java, Spring MVC", Description="Delivers the static content and the Internet banking SPA" }
                }
            }
        };

        //        Person(customer, Customer, "A customer of the bank, with personal bank accounts")
        //System_Boundary(c1, "Internet Banking")
        //        {
        //            Container(web_app, "Web Application", "Java, Spring MVC", "Delivers the static content and the Internet banking SPA")
        //Container(spa, "Single-Page App", "JavaScript, Angular", "Provides all the Internet banking functionality to cutomers via their web browser")
        //Container(mobile_app, "Mobile App", "C#, Xamarin", "Provides a limited subset of the Internet banking functionality to customers via their mobile device")
        //ContainerDb(database, "Database", "SQL Database", "Stores user registraion information, hased auth credentials, access logs, etc.")
        //Container(backend_api, "API Application", "Java, Docker Container", "Provides Internet banking functionality via API")
        //}
        //        System_Ext(email_system, "E-Mail System", "The internal Microsoft Exchange system", "envelope")
        //System_Ext(banking_system, "Mainframe Banking System", "Stores all of the core banking information about customers, accounts, transactions, etc.")


        C4Item selectedNode = new ();
        C4Item? parentNode = null;
        C4Item? potentialParentNode = null;
        C4Item? newItem = null;

        private Modal? modalRef;
        private Modal? newItemModalRef;

        private TestModal? testModalRef;


        private bool cancelClose;

        private bool modalVisible;
        private bool newItemModalVisible;

        private bool cancelled = false;

        PropertyEditor? propEditor;

        private Task ShowModal()
        {
            modalVisible = true;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        //        private Task ShowNewItemModal()
        private Task ShowNewItemModal(C4Item parentNode)
        {
            potentialParentNode = parentNode;
            
            newItemModalVisible = true;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task HideModal()
        {
            modalVisible = false;

            return Task.CompletedTask;
        }

        private Task CloseModal()
        {
            // possibly add a chack for changed and prompt to lose changes

            cancelClose = false;
            cancelled = true;

            return modalRef.Hide();
        }

        C4TypeEnum? newItemType;

        private Task CloseNewItemModal()
        {
            newItemType = null;

            return newItemModalRef.Close(CloseReason.EscapeClosing);
        }

        private Task CreateItem(C4TypeEnum itemType)
        {
            newItemType = itemType;

            newItem = new C4Item() { ItemType = itemType };
//            selectedNode = newItem;

            return newItemModalRef.Close(CloseReason.EscapeClosing);
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

        private async Task TryCloseModal()
        {
            // add a check for validity

            cancelClose = true;

            if (await propEditor.IsValid())
            {
                cancelClose = false;
            }

            await modalRef.Hide();
        }

        private Task OnModelOpened()
        {
            // reset, for the next attempt to close
            cancelClose = false;
//            propEditor.ResetValidation();
            cancelled = false;

            return Task.CompletedTask;
        }

        private Task OnNewItemModalOpened()
        {
            //// reset, for the next attempt to close
            //cancelClose = false;
            ////            propEditor.ResetValidation();
            //cancelled = false;

            return Task.CompletedTask;
        }

        private Task OnModalClosing(ModalClosingEventArgs e)
        {
            // just set Cancel to prevent modal from closing

            if (e.CloseReason == CloseReason.EscapeClosing)
            {
                CloseModal();
            }

            if (cancelClose || e.CloseReason != CloseReason.UserClosing)
            {
                e.Cancel = true;
            }

            return Task.CompletedTask;
        }

        private Task OnNewItemModalClosed()
        {
            if(newItemType != null)
            {
                ShowModal();
            }
            return Task.CompletedTask;
        }


        private Task ShowTestModal()
        {
            return testModalRef?.ShowModal();
        }

        private Task OnTestModalClosed()
        {
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

    }
}
