using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Basket")]
        public int BasketId { get; set; }
        
        [Required]
        [ForeignKey("Account")]
        public int OwnerId { get; set; }
        
        [Required] 
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [Required] 
        public bool IsPaid { get; set; } = false;
        
        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; } = null;
    }
}