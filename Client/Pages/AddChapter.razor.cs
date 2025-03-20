using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ThePenfolio.Client.Shared.model;
using ThePenfolio.Shared.DTOs;
using ThePenfolio.Shared.libraries;

namespace ThePenfolio.Client.Pages
{
    public partial class AddChapter : ComponentBase
    {
        [Parameter] public Guid BookId { get; set; }
        [Inject] private ILocalStorageService LocalStorage { get; set; }
        [Inject] private HttpClient Http { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private Guid? userId;

        private bool isLoading = true;
        private bool isSubmitting = false;

        private BookDTO book;
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

            book = await Http.GetFromJsonAsync<BookDTO>(ApiRoutes.Book.GET_ById(BookId));

            if (book.AuthorId != userId)
            {
                NavigationManager.NavigateTo("");
            }
            
            chapter = new ChapterDTO
            {
                BookId = BookId
            };
            
            isLoading = false;
        }

        private async Task OnSubmit(ChapterDTO chapter)
        {
            isSubmitting = true;

            var result = await Http.PostAsJsonAsync(ApiRoutes.Chapter.POST(), chapter);

            if (result.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("dashboard");
            }
        }
    }
}

