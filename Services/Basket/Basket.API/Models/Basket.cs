using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basket.API.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        
        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; } = null;
    }
}