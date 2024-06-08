using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "name is required")]
        public string Name { get; set; }

        [Required]
        [Range (18,150, ErrorMessage = "age is between 18-150")]
        public int Age { get; set; }

    }
}
