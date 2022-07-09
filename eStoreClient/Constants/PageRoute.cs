namespace eStoreClient.Constants
{
    public static class PageRoute
    {
        // Home
        public const string Home = "/Index";

        // Members
        public const string Members = "/Members/Index";
        public const string Login = "/Members/Login";
        public const string Logout = "/Members/Logout";
        public const string Register = "/Members/Register";
        public const string Profile = "/Members/Details";

        // Order
        public const string Orders = "/Orders/Index";
        public const string OrderPrepare = "/Orders/Prepare";

        // Reports
        public const string Report = "/Reports/Report";

        // Products
        public const string Products = "/Products/Index";

        // Cart
        public const string Cart = "/Carts/Details";
        public const string CartCreate = "/Carts/Create";
        public const string CartEdit = "/Carts/Edit";
        public const string CartDelete = "/Carts/Delete";

    }
}
