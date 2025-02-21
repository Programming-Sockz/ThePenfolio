using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ThePenfolio.Shared.DTOs;
using System.Net.Http.Json;
using ThePenfolio.Client.Shared.model;
using ThePenfolio.Shared.libraries;

namespace ThePenfolio.Client.Pages
{
	public partial class Register
	{
		[Inject] public HttpClient Http { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public ILocalStorageService LocalStorageService { get; set; }

		private RegisterDTO register = new();
		private LoginResponseDTO? responseDTO;
		private bool isSubmitting;

		private async Task ValidSubmit()
		{
			if(!isSubmitting)
			{
				isSubmitting = true;
				var response = await Http.PostAsJsonAsync(ApiRoutes.User.POST_Register(), register);

				if(response.IsSuccessStatusCode)
				{
					responseDTO = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

					if (responseDTO.Success)
					{
						await LocalStorageService.SetItemAsync(LoginStamp.LoginStampStorageKey, new LoginStamp(responseDTO));
						NavigationManager.NavigateTo("");
					}
				}

				isSubmitting=false;
			}
		}
	}
}
