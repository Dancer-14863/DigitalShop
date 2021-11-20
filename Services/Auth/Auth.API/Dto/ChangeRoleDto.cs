using System.ComponentModel.DataAnnotations;

namespace Auth.API.Dto
{
    public class ChangeRoleDto
    {
        [Required]
        public string RoleName { get; set; }
    }
}