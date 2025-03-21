using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ThePenfolio.Client.Shared.model;
using ThePenfolio.Shared.DTOs;
using ThePenfolio.Shared.libraries;
using static ThePenfolio.Shared.libraries.ApiRoutes;

namespace ThePenfolio.Client.Pages
{
    public partial class ReadChapter : ComponentBase
    {
        [Parameter] public Guid ChapterId { get; set; }
        [Inject] public HttpClient Http { get; set; }
        [Inject] public ILocalStorageService LocalStorage { get; set; }

        private ChapterDTO chapter = new();
        private Guid previousChapterId = Guid.Empty;
        private Guid nextChapterId = Guid.Empty;
        private Guid? userId;
        
        protected override async Task OnParametersSetAsync()
        {
            var loginStamp = await LocalStorage.GetItemAsync<LoginStamp>(LoginStamp.LoginStampStorageKey);

            if (loginStamp != null)
            {
                userId = loginStamp.UserId;
            }

            chapter = await Http.GetFromJsonAsync<ChapterDTO>(ApiRoutes.Chapter.GET_ById(ChapterId));

            var chapters = chapter.Book.Chapters.ToList();

            int currentIndex = chapters.IndexOf(chapters.First(x=>x.Id == chapter.Id));

            if (currentIndex != -1)
            {
                // Check if the current chapter is not the last one
                if (currentIndex < chapters.Count - 1)
                {
                    var nextChapter = chapters[currentIndex + 1];
                    nextChapterId = nextChapter.Id;
                }
                else
                {
                    nextChapterId = Guid.Empty;
                }

                // Check if the current chapter is not the first one
                if (currentIndex > 0)
                {
                    var previousChapter = chapters[currentIndex - 1];
                    previousChapterId = previousChapter.Id;
                }
                else
                {
                    previousChapterId = Guid.Empty;
                }
            }

            StateHasChanged();
        }

        private async Task ToggleLike()
        {
            if (userId.HasValue)
            {
                ChapterUserLikesDTO chapterUserLikesDTO = new ChapterUserLikesDTO();
                
                if (chapter.ChapterUserLikes == null || chapter.ChapterUserLikes.All(x => x.UserId != userId))
                {
                    chapterUserLikesDTO.UserId = userId.Value;
                    chapterUserLikesDTO.ChapterId = chapter.Id;

                    if(chapter.ChapterUserLikes == null)
                    {
                        chapter.ChapterUserLikes = [chapterUserLikesDTO];
                    }
                    else
                    {
                        chapter.ChapterUserLikes.Add(chapterUserLikesDTO);
                    }
                }
                else
                {
                    var like = chapter.ChapterUserLikes.First(x => x.UserId == userId);
                    chapterUserLikesDTO = like;
                    chapter.ChapterUserLikes.Remove(like);
                }

                await Http.PostAsJsonAsync(ApiRoutes.ChapterUserLikes.POST_ToggleLike(), chapterUserLikesDTO);
                
                StateHasChanged();
            }
        }
    }
}

