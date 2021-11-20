using System.ComponentModel.DataAnnotations;

namespace Auth.API.Dto
{
    public class RegisterAccountDto
    {
        [Required] 
        [StringLength(50)] 
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(254)] // according to RFC 5321
        public string? Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; } 
        
        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; } 
    }
}