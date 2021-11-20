using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Auth.API.Dto
{
    public class SignInDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [DataMember]
        public string Email { get; set; }
        
        [Required]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}