using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.NewFolder
{
    //view model for get data view.
    [NotMapped]
    public class EmployeeViewMdel
    {
        
        public int? Id { get; set; } 
        public string Name { get; set; }
        public int? Age { get; set; }
        public int? IMAGEId { get; set; }
        public string? Imagepath { get; set; }
        

    }

}
