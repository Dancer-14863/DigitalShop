using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Models
{
    public class Basket
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Account")]
        public int OwnerId { get; set; }
        
        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; } = null;
    }
}