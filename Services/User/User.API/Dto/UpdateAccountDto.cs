using System.ComponentModel.DataAnnotations;

namespace User.API.Dto
{
    public class UpdateAccountDto
    {
        [Required] 
        [StringLength(50)] 
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(254)] // according to RFC 5321
        public string? Email { get; set; }
    }
}