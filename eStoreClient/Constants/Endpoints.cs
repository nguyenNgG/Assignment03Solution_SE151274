namespace eStoreClient.Constants
{
    public static class Endpoints
    {
        private const string BaseUri = "http://localhost:5000/api";
        private const string BaseOdataUri = "http://localhost:5000/odata";

        public static string Register = $"{BaseUri}/Members/register";
        public static string Login = $"{BaseUri}/Members/login";
        public static string Logout = $"{BaseUri}/Members/logout";
        public static string Current = $"{BaseUri}/Members/current";
        public static string Authorize = $"{BaseUri}/Members/authorize";
        public static string Cart = $"{BaseUri}/Members/cart";
        public static string Email = $"{BaseUri}/Members/email";
        public static string Password = $"{BaseUri}/Members/password";
        public static string Name = $"{BaseUri}/Members/name";

        public static string Members = $"{BaseOdataUri}/Members";
        public static string Products = $"{BaseOdataUri}/Products";
        public static string Orders = $"{BaseOdataUri}/Orders";
        public static string OrderDetails = $"{BaseOdataUri}/OrderDetails";
        public static string Categories = $"{BaseOdataUri}/Categories";
    }
}
