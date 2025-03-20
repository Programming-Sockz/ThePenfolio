using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ThePenfolio.Client.Shared.model;
using ThePenfolio.Shared.DTOs;
using ThePenfolio.Shared.libraries;

namespace ThePenfolio.Client.Pages
{
    public partial class EditChapter : ComponentBase
    {
        [Parameter] public Guid ChapterId { get; set; }
        [Inject] private ILocalStorageService LocalStorage { get; set; }
        [Inject] private HttpClient Http { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private Guid? userId;

        private bool isLoading = true;
        private bool isSubmitting = false;
        
        private ChapterDTO chapter;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            
            var loginStamp = await LocalStorage.GetItemAsync<LoginStamp>(LoginStamp.LoginStampStorageKey);

            if (loginStamp == null)
            {
                isLoading = false;
                return;
            }
            
            userId = loginStamp.UserId;

            chapter = await Http.GetFromJsonAsync<ChapterDTO>(ApiRoutes.Chapter.GET_ById(ChapterId));

            if (chapter.Book.AuthorId != userId)
            {
                NavigationManager.NavigateTo("");
            }

            chapter.Book = null;

            isLoading = false;
        }

        private async Task OnSubmit(ChapterDTO chapter)
        {
            isSubmitting = true;

            var result = await Http.PutAsJsonAsync(ApiRoutes.Chapter.PUT(), chapter);

            if(result.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("dashboard");
            }
        }
    }
}

