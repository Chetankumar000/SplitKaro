using System.ComponentModel.DataAnnotations;

namespace SplitKaro.Dtos.Group
{
    public class CreateGroupDto
    {
        [Required]
        public string GroupName { get; set; } = string.Empty;
    }
}
