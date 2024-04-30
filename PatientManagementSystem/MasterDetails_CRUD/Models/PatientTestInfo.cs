using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDetails_CRUD.Models
{
    public class PatientTestInfo
    {
        public int PatientTestInfoId { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        [ForeignKey("Test")]
        public int TestId { get; set; }
        public virtual Patient Patient { get; set; } = default!;
        public virtual Test Test { get; set; } = default!;
    }
}
