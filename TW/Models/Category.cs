using System.ComponentModel.DataAnnotations;

namespace TW.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [Range(minimum:1,maximum: int.MaxValue, ErrorMessage = "Display Order for category must be greater than 0")]
        [Display(Name="Display Order")]
        public int DisplayOrder { get; set; }


    }
}
