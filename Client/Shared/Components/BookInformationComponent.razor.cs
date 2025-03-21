using Microsoft.AspNetCore.Components;
using ThePenfolio.Shared.DTOs;

namespace ThePenfolio.Client.Shared.Components
{
    public partial class BookInformationComponent
    {
        [Parameter] public BookDTO Book { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }
}
