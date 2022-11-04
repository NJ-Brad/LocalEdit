using Blazorise;
using LocalEdit.Modals;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace LocalEdit.Pages
{
    public partial class FileTest : ComponentBase
    {
        string fileText = "";

        protected override async Task OnInitializedAsync()
        {
//            await localStorage.SetItemAsync("name", "John Smith");
            //var name = await localStorage.GetItemAsync<string>("name");
            //var upl = await localStorage.GetItemAsync<string>("uploaded");

            //var exists = await localStorage.GetItemAsync<string>("i18nextLng");

            //var keys = await localStorage.KeysAsync();
        }


        async Task OnFileUpload(FileUploadEventArgs e)
        {
            try
            {
                using (MemoryStream result = new MemoryStream())
                {
                    await e.File.OpenReadStream(long.MaxValue).CopyToAsync(result);
                    result.Seek(0, SeekOrigin.Begin);
                    fileText = await new StreamReader(result).ReadToEndAsync();
                    //fileText = await new StreamReader(e.File.OpenReadStream()).ReadToEndAsync();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                this.StateHasChanged();
            }
        }

        //private async void LoadFiles(InputFileChangeEventArgs e)
        //{
        //    // https://learn.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-6.0&pivots=webassembly

        //    var reader = await new StreamReader(e.File.OpenReadStream()).ReadToEndAsync();
        //    //await localStorage.SetItemAsync("uploaded", reader);
        //    fileText = reader;

        //    // Note that the following line is necessary because otherwise
        //    // Blazor would not recognize the state change and refresh the UI
        //    //InvokeAsync(() =>
        //    //{
        //    StateHasChanged();
        //    //});
        //}

        private async void IncrementCount()
        {
            // https://stackoverflow.com/questions/16072709/converting-string-to-byte-array-in-c-sharp
            byte[] bytes = Encoding.ASCII.GetBytes(fileText);
            //            await BlazorDownloadFileService.DownloadFileAsync("example.txt", bytes);

            //        currentCount++;
        }

        FileManagementModal fileManagementModalRef;

        private Task ShowFileModal()
        {
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //flowItemModalRef.item = selectedItemRow;

            fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnFileManagementModalClosed()
        {
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


    }
}
