using Microsoft.AspNetCore.Components;
using ThePenfolio.Shared.DTOs;
using ThePenfolio.Shared.Enums;

namespace ThePenfolio.Client.Shared.Components
{
    public partial class ChapterForm : ComponentBase
    {
        [Parameter] public ChapterDTO Chapter { get; set; }
        [Parameter] public EventCallback<ChapterDTO> OnSubmit { get; set; }
        [Parameter] public bool IsSubmitting { get; set; }
        
        private bool showReleaseDatePicker = false;

        protected override void OnInitialized()
        {
            if (Chapter.Id != Guid.Empty && Chapter.ReleasedOn != null && Chapter.ReleasedOn > DateTime.Now)
            {
                showReleaseDatePicker = true;
            }
        }
        
        private async Task OnValidSubmit()
        {
            if(Chapter.Id == Guid.Empty)
            {
                Chapter.CreatedOn = DateTime.Now;
            }
            Chapter.LastEditedOn = DateTime.Now;
            await OnSubmit.InvokeAsync(Chapter);
        }

        private bool IsReleaseTypeSelected(ReleaseType type)
        {
            switch (type)
            {
                case ReleaseType.Draft:
                    return Chapter.ReleasedOn == null;
                case ReleaseType.Published:
                    return Chapter.ReleasedOn <= DateTime.Now;
                case ReleaseType.FutureRelease:
                    return Chapter.ReleasedOn > DateTime.Now;
                default:
                    return false;
            }
        }

        private void OnReleaseTypeSelected(ChangeEventArgs args)
        {
            if (Enum.TryParse(args.Value?.ToString(), out ReleaseType type))
            {
                switch (type)
                {
                    case ReleaseType.Draft:
                        showReleaseDatePicker = false;
                        Chapter.ReleasedOn = null;
                        return;
                    case ReleaseType.Published:
                        showReleaseDatePicker = false;
                        Chapter.ReleasedOn = DateTime.Now;
                        return;
                    case ReleaseType.FutureRelease:
                        showReleaseDatePicker = true;
                        Chapter.ReleasedOn = DateTime.Now;
                        return;
                }
            }
        }
    }
}

