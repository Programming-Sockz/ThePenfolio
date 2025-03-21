using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using ThePenfolio.Shared.DTOs;
using ThePenfolio.Shared.libraries;

namespace ThePenfolio.Client.Pages
{
    public partial class BookInformation
    {
        [Parameter] public Guid BookId { get; set; }
        [Inject] private HttpClient Http { get; set; }
        [Inject] private ILocalStorageService LocalStorage { get; set; }

        private Guid userId;
        private BookDTO book = new();

        protected override async Task OnParametersSetAsync()
        {
            book = await Http.GetFromJsonAsync<BookDTO>(ApiRoutes.Book.GET_ById(BookId));
        }
    }
}
