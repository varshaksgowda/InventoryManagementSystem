﻿namespace IMSWebApi.DTO
{
    public class OrderDTO
    {
        
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
