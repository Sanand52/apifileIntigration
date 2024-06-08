using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;
using WebApplication1.Models.NewFolder;


namespace WebApplication1.DBconn
{
    public class Dbconn : DbContext
    {
        public Dbconn(DbContextOptions<Dbconn> options) : base(options) { }

        


        public DbSet<Employee> Employees { get; set; }

        public DbSet<ImageEmp> Images { get; set; }

        //create dbset of view model
        [NotMapped]
        public DbSet<EmployeeViewMdel> EmployeesViewMdels { get; set; }

        //create override for nokey error in view model.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeViewMdel>().HasNoKey();
        }
    }
}
