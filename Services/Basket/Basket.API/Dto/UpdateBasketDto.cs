using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Dto
{
    public class UpdateBasketDto
    {
        [Required]
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}