﻿using BusinessObject;

namespace eStoreClient.Models
{
    public class OrderWithTotal : Order
    {
        public decimal TotalPrice { get; set; }
    }
}
