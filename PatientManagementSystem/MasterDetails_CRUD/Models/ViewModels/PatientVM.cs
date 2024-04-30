using System.ComponentModel.DataAnnotations;

namespace MasterDetails_CRUD.Models.ViewModels
{
    public class PatientVM
    {
        public PatientVM()
        {
            this.TestList = new List<int>();
        }
        public int PatientId { get; set; }
        [Required, Display(Name = "Patient Name")]
        public string PatientName { get; set; } = default!;
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = default!;
        public string? Image { get; set; } = default!;
        public IFormFile? ImageFile { get; set; }
        public bool Admission { get; set; }
        public List<int> TestList { get; set; }
    }
}
