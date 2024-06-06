using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


namespace WebApplication1.DBconn
{
    public class Dbconn:DbContext
    {
        public Dbconn(DbContextOptions<Dbconn> options): base(options) { }


        public DbSet<Employee> Employees { get; set; }

        public DbSet<ImageEmp> Images { get; set; }
    }
}
