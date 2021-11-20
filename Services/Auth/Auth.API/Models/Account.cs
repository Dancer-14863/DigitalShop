using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.API.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required] 
        [StringLength(50)] 
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(254)] // according to RFC 5321
        public string? Email { get; set; }

        [Required] 
        [DataType(DataType.Password)]
        [StringLength(200)] 
        public string? Password { get; set; } 

        [ForeignKey("Role")] 
        public int RoleId { get; set; }

        [NotMapped] 
        public Role? Role { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? UpdatedAt { get; set; } = null;
        
    }
}