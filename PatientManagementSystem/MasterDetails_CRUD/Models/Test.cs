using System.ComponentModel.DataAnnotations;

namespace MasterDetails_CRUD.Models
{
    public class Test
    {
        public int TestId { get; set; }
        [Required, Display(Name = "Test Name")]
        public string TestName { get; set; } = default!;
        public virtual ICollection<PatientTestInfo> PatientTestInfos { get; set; } = new List<PatientTestInfo>();
    }
}
