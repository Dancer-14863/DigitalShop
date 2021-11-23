using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Order.API.Dto
{
    public class CreateOrderDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int BasketId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int OwnerId { get; set; }

        [Required] 
        public bool IsPaid { get; set; } = false;
    }
}