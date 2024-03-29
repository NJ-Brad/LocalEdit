﻿using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit.C4Types;
using LocalEdit.Modals;
using LocalEdit.PlanTypes;
using Blazorise.Components;
using System.Text.Json;

namespace LocalEdit.Pages
{
    public partial class MarkdownEditor : ComponentBase
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

        string markdownValue = "# EasyMDE \n Go ahead, play around with the editor! Be sure to check out **bold**, *italic*, [links](https://google.com) and all the other features. You can type the Markdown syntax, use the toolbar, or use shortcuts like `ctrl-b` or `cmd-b`.";

        string? markdownHtml { get; set; }

        protected override void OnInitialized()
        {
            markdownHtml = Markdig.Markdown.ToHtml(markdownValue ?? string.Empty);

            base.OnInitialized();
        }

        Task OnMarkdownValueChanged(string value)
        {
            markdownValue = value;

            markdownHtml = Markdig.Markdown.ToHtml(markdownValue ?? string.Empty);

            return Task.CompletedTask;
        }

        FileManagementModal? FileManagementModalRef { get; set; }

        private Task NewMdDocument()
        {
            if(FileManagementModalRef != null)
                FileManagementModalRef.Name = "New_Document.md";

            markdownValue = "";

            return Task.CompletedTask;
        }

        private Task LoadFile()
        {
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //flowItemModalRef.item = selectedItemRow;

            FileManagementModalRef?.LoadFile();
            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task SaveFile()
        {
            string fileText = markdownValue;
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //flowItemModalRef.item = selectedItemRow;

            if (FileManagementModalRef != null)
                FileManagementModalRef.SaveFile(fileText);

            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnFileManagementModalClosed()
        {
            if (FileManagementModalRef?.Result == ModalResult.OK)
            {
                if ((FileManagementModalRef != null) && (FileManagementModalRef.FileText != null))
                    markdownValue = FileManagementModalRef.FileText;
                InvokeAsync(() => StateHasChanged());
            }

            return Task.CompletedTask;
        }
    }
}
