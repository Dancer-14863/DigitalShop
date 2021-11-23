using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.API.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required] 
        [DataType(DataType.Text)]
        [StringLength(50)] 
        public string? ProductName { get; set; }

        [Required] 
        [DataType(DataType.Text)]
        [StringLength(250)] 
        public string? ProductDescription { get; set; } 

        [Required] 
        // taken from https://stackoverflow.com/a/19814988
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        [DataType(DataType.Currency)]
        public decimal ProductPrice { get; set; }
        
        [Required]
        [Range(0, 9999)]
        public int ProductQuantity { get; set; }
        
        [Required]
        [ForeignKey("Account")]
        public int OwnerId { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; } = null;
    }
}