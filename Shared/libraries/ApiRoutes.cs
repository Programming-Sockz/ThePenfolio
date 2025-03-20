using ThePenfolio.Shared.Enums;
namespace ThePenfolio.Shared.libraries
{
    public static class ApiRoutes
    {
        public static class Test
        {
            private const string _base = "api/test";
            
            public static string Get() 
            => $"{_base}";
        }
        
        public static class Book
        {
            private const string _base = "api/books";
            
            public static string GET() 
            => $"{_base}";
            
            public static string POST() 
            => $"{_base}";
            
            public static string GET_ByBookTitle(string title) 
            => $"{_base}/{title}";
            
            public static string GET_ById(Guid id) 
                => $"{_base}/id/{id}";

            public static string GET_ReducedById(Guid id) 
                => $"{_base}/reduced/{id}";

            public static string GET_ByAuthorId(Guid id, bool isAuthorView = false) 
                => $"{_base}/author/{id}?isAuthorview={isAuthorView}";
            
            public static string PUT(Guid id)
                => $"{_base}/{id}";

            public static string POST_Search()
                => $"{_base}/search";
        }

        public static class BookList
        {
            private const string _base = "api/booklist";

            public static string POST_AddBookToList()
                => $"{_base}/addBook";
            
            public static string POST_RemoveBookToList()
                => $"{_base}/removeBook";
            
            public static string GET_BooksInList(Guid id, ListTypes listTypes)
            => $"_base/{id}/{(int)listTypes}";
        }

        public static class User
        {
            const string _base = "api/User";

            public static string POST_Register()
                => $"{_base}/register";
            public static string POST_Login()
                => $"{_base}/login";
        }
        
        public static class Tags
        {
            const string _base = "/api/Tags";
            
            public static string POST()
                => $"{_base}";
            
            public static string GET_All()
                => $"{_base}";
            
            public static string POST_UpdateTags()
                => $"{_base}/updatetags";
            
            public static string DELETE_ById(Guid id)
                => $"{_base}/{id}";
            public static string GET_ById(Guid id)
                => $"{_base}/{id}";
            public static string GET_BooksById(Guid id)
                => $"{_base}/books/{id}";
            public static string GET_ByName(string name)
                => $"{_base}/name/{name}";
        }
        
        public static class Genres
        {
            const string _base = "/api/Genres";
            
            public static string POST()
                => $"{_base}";
            
            public static string GET_All()
                => $"{_base}";
            
            public static string POST_UpdateGenres()
                => $"{_base}/updategenres";
            
            public static string DELETE_ById(Guid id)
                => $"{_base}/{id}";
            public static string GET_ById(Guid id)
                => $"{_base}/{id}";
            public static string GET_BooksById(Guid id)
                => $"{_base}/books/{id}";
            public static string GET_ByName(string name)
                => $"{_base}/name/{name}";
        }

        public static class Chapter
        {
            const string _base = "/api/Chapter";
            
            public static string POST()
                => $"{_base}";
            public static string PUT()
                => $"{_base}";
            public static string DELETE(Guid id)
                => $"{_base}/{id}";
            public static string GET_ById(Guid id)
                => $"{_base}/{id}";
        }
    }
}
