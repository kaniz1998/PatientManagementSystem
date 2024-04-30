using Microsoft.EntityFrameworkCore;

namespace MasterDetails_CRUD.Models
{
    public class PatientDbContext:DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
        {
            
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<PatientTestInfo> PatientTestInfos { get; set; }
    }
}
