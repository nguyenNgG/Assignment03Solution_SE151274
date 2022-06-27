namespace eStoreClient.Constants
{
    public static class Endpoints
    {
        private static string BaseUri = "http://localhost:5000/api";
        private static string BaseOdataUri = "http://localhost:5000/odata";

        public static string Register = $"{BaseUri}/Members/register";
        public static string Login = $"{BaseUri}/Members/login";
        public static string Authenticate = $"{BaseUri}/Members/authenticate";
        public static string Authorize = $"{BaseUri}/Members/authorize";
        public static string Current = $"{BaseUri}/Members/current";
        public static string Cart = $"{BaseUri}/Members/cart";

        public static string Members = $"{BaseOdataUri}/Members";

    }
}
