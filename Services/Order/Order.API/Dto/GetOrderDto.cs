using System;
using System.Collections.Generic;

namespace Order.API.Dto
{
    public class GetOrderDto
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int OwnerId { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
    }
}