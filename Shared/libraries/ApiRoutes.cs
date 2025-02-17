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
    }
}
