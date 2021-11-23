using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Dto
{
    public class CreateBasketDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int OwnerId { get; set; }
        [Required]
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}