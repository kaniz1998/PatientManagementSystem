using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MasterDetails_CRUD.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        [Required, Display(Name = "Patient Name")]
        public string PatientName { get; set; } = default!;
        [Required, Display(Name = "Date Of Birth"), Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = default!;
        public string Image { get; set; } = default!;
        public bool Admission { get; set; }
        public virtual ICollection<PatientTestInfo> PatientTestInfos { get; set; } = new List<PatientTestInfo>();
    }
}
