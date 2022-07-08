using BusinessObject;

namespace eStoreClient.Utilities
{
    public static class StringTrimmer
    {
        public static Product TrimProduct(Product product)
        {
            if (product != null)
            {
                product.ProductName = product.ProductName.Trim();
            }
            return product;
        }
    }
}
