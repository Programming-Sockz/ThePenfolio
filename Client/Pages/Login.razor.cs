using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ThePenfolio.Shared.DTOs;
using System.Net.Http.Json;
using ThePenfolio.Shared.libraries;

namespace ThePenfolio.Client.Pages
{
    public partial class Login
    {
        [Inject] public HttpClient Http { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ILocalStorageService LocalStorageService { get; set; }

        private string loginStorageKey = "loginStamp";
        private LoginRequestDTO login = new();
        private LoginResponseDTO? responseDTO;
        private bool isSubmitting = false;

        private async Task ValidSubmit()
        {
    
        }
    }
}
