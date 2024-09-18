using System.ComponentModel.DataAnnotations;

namespace SplitKaro.Dtos.Group
{
    public class UpdateGroupDto
    {
        [Required]

        public string GroupName { get; set; } = string.Empty;
    }
}
