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
            if (!isSubmitting)
            {
                isSubmitting = true;
                var response = await Http.PostAsJsonAsync(ApiRoutes.User.POST_Login(), login);
                if (response.IsSuccessStatusCode)
                {
                    responseDTO = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                    if (responseDTO.Success)
                    {
                        await LocalStorageService.SetItemAsync(loginStorageKey, responseDTO);
                        NavigationManager.NavigateTo("");
                    }
                    else
                    {
                        isSubmitting = false;
                    }
                }
            }
        }
    }
}
