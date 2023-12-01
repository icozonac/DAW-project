using System.ComponentModel.DataAnnotations;

namespace TW.Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }



    }
}
