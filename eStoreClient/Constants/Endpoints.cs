﻿namespace eStoreClient.Constants
{
    public static class Endpoints
    {
        private static string BaseUri = "http://localhost:5000/api";
        private static string BaseOdataUri = "http://localhost:5000/odata";

        public static string Register = $"{BaseUri}/Users/register";
        public static string Login = $"{BaseUri}/Users/login";
        public static string Authenticate = $"{BaseUri}/Users/authenticate";
        public static string Authorize = $"{BaseUri}/Users/authorize";
        public static string Current = $"{BaseUri}/Users/current";
        public static string Cart = $"{BaseUri}/Users/cart";

        public static string Members = $"{BaseOdataUri}/Members";

    }
}