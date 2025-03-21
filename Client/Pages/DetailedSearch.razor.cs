using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using ThePenfolio.Shared.DTOs;
using ThePenfolio.Shared.libraries;

namespace ThePenfolio.Client.Pages
{
    public partial class DetailedSearch
    {
        [Inject] private HttpClient Http { get; set; }
        private BookSearchDTO bookSearch = new();

        private List<BookDTO> books = new List<BookDTO>();

        private List<TagDTO> tags = new();
        private List<GenreDTO> genres = new();

        private List<TagDTO> selectedIncludeTags = new();
        private List<TagDTO> selectedExcludeTags = new();

        protected override async Task OnInitializedAsync()
        {
            tags = await Http.GetFromJsonAsync<List<TagDTO>>(ApiRoutes.Tags.GET_All());
            genres = await Http.GetFromJsonAsync<List<GenreDTO>>(ApiRoutes.Genres.GET_All());
        }

        private async Task Search()
        {
            bookSearch.IncludeTags = selectedIncludeTags.Select(x => x.Id).ToList();
            bookSearch.ExcludeTags = selectedExcludeTags.Select(x => x.Id).ToList();

            var result = await Http.PostAsJsonAsync(ApiRoutes.Book.POST_Search(), bookSearch);
            if(result.IsSuccessStatusCode)
            {
                books = await result.Content.ReadFromJsonAsync<List<BookDTO>>();
            }
        }

        private async Task OnIncludeTagSelected(TagDTO tag)
        {
            if (!selectedIncludeTags.Contains(tag))
            {
                selectedIncludeTags.Add(tag);
            }
        }

        private async Task RemoveIncludeTag(Guid tagId)
        {
            selectedIncludeTags.RemoveAll(x => x.Id == tagId);
        }

        private async Task OnExcludeTagSelected(TagDTO tag)
        {
            if (!selectedExcludeTags.Contains(tag))
            {
                selectedExcludeTags.Add(tag);
            }
        }

        private async Task RemoveExcludeTag(Guid tagId)
        {
            selectedExcludeTags.RemoveAll(x => x.Id == tagId);
        }

        private async Task ToggleIncludeGenre(Guid genreId)
        {
            if(IsIncludeGenreSelcted(genreId))
            {
                bookSearch.IncludeGenres.Remove(genreId);
            }
            else
            {
                bookSearch.IncludeGenres.Add(genreId);
            }
        }
        private async Task ToggleExcludeGenre(Guid genreId)
        {
            if (IsExcludeGenreSelcted(genreId))
            {
                bookSearch.ExcludeGenres.Remove(genreId);
            }
            else
            {
                bookSearch.ExcludeGenres.Add(genreId);
            }
        }

        private bool IsIncludeGenreSelcted(Guid genreId)
        {
            return bookSearch.IncludeGenres.Contains(genreId);
        }

        private bool IsExcludeGenreSelcted(Guid genreId)
        {
            return bookSearch.ExcludeGenres.Contains(genreId);
        }
    }
}
