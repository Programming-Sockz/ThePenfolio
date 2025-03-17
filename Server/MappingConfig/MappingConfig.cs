using Mapster;
using ThePenfolio.Server.Models;
using ThePenfolio.Shared.DTOs;
using Tag=AntDesign.Tag;
namespace ThePenfolio.Server.MappingConfig
{
    public class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Book, BookDTO>.NewConfig()
                .Map(dest => dest.Genres, src => src.BookGenres != null && src.BookGenres.Any() ? src.BookGenres.Select(bg => bg.Genre) : null)
                .Map(dest => dest.Tags, src => src.BookTags != null && src.BookTags.Any() ? src.BookTags.Select(bg => bg.Tag) : null)
                .Map(dest => dest.Author, src => src.Author != null ? src.Author.Adapt<UserDTO>() : null)
                .Map(dest => dest.Chapters, src => src.Chapters != null && src.Chapters.Any() ? src.Chapters.Select(c => c.Adapt<ChapterDTO>()) : null)
                .Map(dest => dest.UserBookReviews, src => src.UserBookReviews != null && src.UserBookReviews.Any() ? src.UserBookReviews.Select(bg => bg.Adapt<UserBookReviewsDTO>()) : null);

            TypeAdapterConfig<Genre, GenreDTO>.NewConfig();
            TypeAdapterConfig<Tag, TagDTO>.NewConfig();
            TypeAdapterConfig<User, UserDTO>.NewConfig();
            TypeAdapterConfig<Chapter, ChapterDTO>.NewConfig();
            TypeAdapterConfig<UserBookReviews, UserBookReviewsDTO>.NewConfig();
        }
    }
}
