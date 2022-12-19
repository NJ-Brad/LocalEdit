﻿using Blazorise;
using LocalEdit.C4Types;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class C4RelationshipModal : LE_ModalBase
    {
        Validations? validations;

        public override async Task<bool> Validate()
        {
            bool rtnVal = false;
            if (validations != null) 
                if (await validations.ValidateAll())
                {
                    rtnVal = true;
                }

            return rtnVal;
        }

        public override async Task ResetValidation()
        {
            if(validations != null)
                await validations.ClearAll();
        }

        public override async Task Opened()
        {
            await InvokeAsync(() => StateHasChanged());

            //            return Task.CompletedTask;
            return;
        }


        [Parameter]
        public C4Relationship Item { get; set; } = new();

        [Parameter]
        public List<C4Item> Items { get => items;
            set { 
                items = value;
                GetAllItems(items);
            } 
        }

        private List<C4Item> items = new ();

        public List<C4Item> AllItems { get; set; } = new();

        private void GetAllItems(List<C4Item> items)
        {
            AllItems.Clear();
            GetAllItems_(items);
        }

        private void GetAllItems_(List<C4Item> items)
        {
            foreach (C4Item item in items)
            {
                AllItems.Add(item);
                GetAllItems_(item.Children);
            }
        }

    }
}
