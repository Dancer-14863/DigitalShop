using System;
using System.Collections.Generic;

namespace Basket.API.Dto
{
    public class GetBasketDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}