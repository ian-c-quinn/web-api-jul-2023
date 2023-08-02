using Microsoft.EntityFrameworkCore;



namespace EmployeesHrApi.Data;



public class EmployeeDataContext : DbContext
{
    public EmployeeDataContext(DbContextOptions<EmployeeDataContext> options) : base(options)
    {

    }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<HiringRequests> HiringRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
             .Property(e => e.Email).HasMaxLength(200);

        modelBuilder.Entity<Employee>().Property(e => e.FirstName).HasMaxLength(200);
        modelBuilder.Entity<Employee>().Property(e => e.LastName).HasMaxLength(200);
        modelBuilder.Entity<Employee>().HasIndex(e => e.Department).IsClustered(false);
        modelBuilder.Entity<Employee>().Property(e => e.Salary).HasPrecision(16, 2);
        modelBuilder.Entity<HiringRequests>().Property(hr => hr.RequiredSalary).HasPrecision(16, 2);
    }

    public IQueryable<Employee> GetEmployeesByDepartment(string department)
    {
        if (department != "All")
        {
            return Employees.Where(e => e.Department == department);
        }
        else
        {
            return Employees;
        }
    }
}
