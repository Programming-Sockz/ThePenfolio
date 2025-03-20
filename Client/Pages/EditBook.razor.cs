using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ThePenfolio.Client.Shared.model;
using ThePenfolio.Shared.DTOs;
using ThePenfolio.Shared.libraries;

namespace ThePenfolio.Client.Pages
{
    public partial class EditBook
    {
        [Parameter] public Guid? Id { get; set; }
        [Inject] private HttpClient Http { get; set; }
        [Inject] private ILocalStorageService LocalStorage { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        
        private Guid? userId;
        private BookDTO book = new();
        private bool isLoading = true;
        private bool isSubmitting = false;
        
        private List<GenreDTO> genres = new();
        private List<Guid> selectedGenres = new();
        private List<TagDTO> tags = new();
        private List<TagDTO> selectedTags = new();
        private List<TagDTO> tagSearchResult = new();
        
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

            genres = await Http.GetFromJsonAsync<List<GenreDTO>>(ApiRoutes.Genres.GET_All());
            tags = await Http.GetFromJsonAsync<List<TagDTO>>(ApiRoutes.Tags.GET_All());
            
            if (Id.HasValue)
            {
                book = await Http.GetFromJsonAsync<BookDTO>(ApiRoutes.Book.GET_ById(Id.Value));
                
                if (book.AuthorId != userId)
                {
                    NavigationManager.NavigateTo("");
                }
                
                selectedGenres = book.Genres.Select(x => x.Id).ToList();
                selectedTags = book.Tags.ToList();
            }
            
            isLoading = false;
            StateHasChanged();
        }
        
        private async Task OnValidSubmit()
        {
            isSubmitting = true;
            book.Genres = genres.Where(x => selectedGenres.Contains(x.Id)).ToList();
            book.Tags = selectedTags;
            book.AuthorId = userId.Value;

            if (Id.HasValue)
            {
                _ = await Http.PutAsJsonAsync(ApiRoutes.Book.PUT(Id.Value), book);
            }
            else
            {
                book.ReleaseDate = DateTime.Now;
                _ = await Http.PostAsJsonAsync(ApiRoutes.Book.POST(), book);
            }
            
            NavigationManager.NavigateTo("dashboard");
        }

        private void ToggleGenre(Guid genre)
        {
            if (selectedGenres.Any(x => x == genre))
            {
                selectedGenres.Remove(genre);
            }
            else
            {
                selectedGenres.Add(genre);
            }

            StateHasChanged();
        }

        private bool IsGenreSelcted(Guid genre)
        {
            return selectedGenres.Any(x => x == genre);
        }
    
        private void OnTagSelected(TagDTO match)
        {
            if (!selectedTags.Any(x => x.Id == match.Id))
            {
                selectedTags.Add(match);
            }
            StateHasChanged();
        }

        private void RemoveTag(Guid tagId)
        {
            selectedTags.Remove(selectedTags.First(x => x.Id == tagId));
            StateHasChanged();
        }

        private void OnImageChange(string base64Image)
        {
            book.Image = base64Image;
            StateHasChanged();
        }
    }
}

