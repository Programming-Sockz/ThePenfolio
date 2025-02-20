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
                => $"{_base}/{id}";

            public static string GET_ReducedById(Guid id) 
                => $"{_base}/reduced/{id}";

            public static string GET_ByAuthorId(Guid id) 
                => $"{_base}/author/{id}";
            
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
    }
}
