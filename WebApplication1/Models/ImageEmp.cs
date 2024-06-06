using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class ImageEmp
    {
        [Key]
        public int Id { get; set; }

        public string? Imagepath { get; set; }

        public Employee Employee { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

    }
}
