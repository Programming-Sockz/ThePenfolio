using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Blazored.Modal;
using Blazored.Modal.Services;
using ThePenfolio.Client.Shared.Modals;
using ThePenfolio.Client.Shared.model;
using ThePenfolio.Shared.DTOs;
using ThePenfolio.Shared.libraries;

namespace ThePenfolio.Client.Pages
{
    public partial class Dashboard
    {
        [Inject] private ILocalStorageService LocalStorage { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private HttpClient Http { get; set; }
        [Inject] private IModalService ModalService { get; set; }

        private bool isLoading = true;
        private Guid? userId;
        private List<BookDTO> books = new();

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

            await LoadBooks();

            isLoading = false;
        }

        private async Task LoadBooks()
        {
            books = await Http.GetFromJsonAsync<List<BookDTO>>(ApiRoutes.Book.GET_ByAuthorId(userId.Value, true));
        }

        private void NavigateToEditBook(Guid? id)
        {
            if (id == null)
            {
                NavigationManager.NavigateTo("/editbook");
            }
            else
            {
                NavigationManager.NavigateTo($"/editbook/{id}");
            }
        }

        private void CreateChapter(Guid id)
        {
            NavigationManager.NavigateTo($"/addchapter/{id}");
        }
        
        private async Task OpenChapterModal(BookDTO book)
        {
            var options = new ModalOptions()
            {
                HideCloseButton = false,
                HideHeader = false,
                DisableBackgroundCancel = false,
                Position = ModalPosition.Middle,
                Size = ModalSize.Large
            };
            
            var parameters = new ModalParameters()
                .Add(nameof(ChapterListModal.Chapters), book.Chapters?.ToList() ?? new List<ChapterDTO>())
                .Add(nameof(ChapterListModal.BookId), book.Id)
                .Add(nameof(ChapterListModal.IsAuthorView), true);
            
            var result = await ModalService.Show<ChapterListModal>("Chapters", parameters, options).Result;
        }
    }
}
