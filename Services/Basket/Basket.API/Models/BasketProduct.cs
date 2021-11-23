using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basket.API.Models
{
    public class BasketProduct
    {
        public int Id { get; set; }
        [ForeignKey("Basket")] 
        public int BasketId { get; set; }
        [ForeignKey("Product")] 
        public int ProductId { get; set; }
        
        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; } = null;
    }
}