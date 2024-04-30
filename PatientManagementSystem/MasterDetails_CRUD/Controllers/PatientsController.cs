using MasterDetails_CRUD.Models;
using MasterDetails_CRUD.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MasterDetails_CRUD.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PatientDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public PatientsController(PatientDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index(string userText, string sortOrder, int page)
        {
            IQueryable<Patient> patient = _context.Patients.Include(x => x.PatientTestInfos).ThenInclude(x => x.Test);
            ViewBag.search = userText;
            ViewBag.sort = string.IsNullOrEmpty(sortOrder) ? "desc_name" : "";
            if (!string.IsNullOrEmpty(userText))
            {
                userText = userText.ToLower();
                patient = patient.Where(x => x.PatientName.ToLower().Contains(userText));
            }

            switch (sortOrder)
            {
                case "desc_name":
                    patient = patient.OrderByDescending(e => e.PatientName);
                    break;
                default:
                    patient = patient.OrderBy(e => e.PatientName);
                    break;
            }
            if (page <= 0) page = 1;
            int pageSize = 2;
            ViewBag.pSize = pageSize;
            return View(await PaginatedList<Patient>.CreateAsync(patient, page, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult AddNewTest(int id)
        {
            ViewBag.test = new SelectList(_context.Tests, "TestId", "TestName", id.ToString() ?? "");
            return PartialView("_addNewTest");
        }
        [HttpPost]
        public IActionResult Create(PatientVM vm, int[] testId)
        {
            if (ModelState.IsValid)
            {
                Patient patient = new Patient
                {
                    PatientName = vm.PatientName,
                    DateOfBirth = vm.DateOfBirth,
                    Phone = vm.Phone,
                    Admission = vm.Admission
                };
                var file = vm.ImageFile;
                string webroot = _environment.WebRootPath;
                string folder = "Images";
                string imgFile = Path.GetFileName(file.FileName);
                string fileTosave = Path.Combine(webroot, folder, imgFile);
                if (file != null)
                {
                    using (var stream = new FileStream(fileTosave, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        patient.Image = "/" + folder + "/" + imgFile;
                    }
                }
                foreach (var test in testId)
                {
                    PatientTestInfo info = new PatientTestInfo
                    {
                        Patient = patient,
                        PatientId = patient.PatientId,
                        TestId = test
                    };
                    _context.PatientTestInfos.Add(info);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            var patient = _context.Patients.FirstOrDefault(x => x.PatientId == id);

            PatientVM vm = new PatientVM
            {
                PatientId = patient.PatientId,
                PatientName = patient.PatientName,
                DateOfBirth = patient.DateOfBirth,
                Phone = patient.Phone,
                Admission = patient.Admission,
                Image = patient.Image

            };
            var existTest = _context.PatientTestInfos.Where(x => x.PatientId == id).ToList();
            foreach (var item in existTest)
            {
                vm.TestList.Add(item.TestId);
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PatientVM vm, int[] testId)
        {
            if(ModelState.IsValid)
            {
                Patient patient = new Patient
                {
                    PatientId=vm.PatientId,
                    PatientName = vm.PatientName,
                    DateOfBirth = vm.DateOfBirth,
                    Phone = vm.Phone,
                    Admission = vm.Admission,
                    Image=vm.Image
                    
                };
                var file = vm.ImageFile;
                if (file != null)
                {
                    string webroot = _environment.WebRootPath;
                    string folder = "Images";
                    string imgFile = Path.GetFileName(file.FileName);
                    string fileTosave = Path.Combine(webroot, folder, imgFile);
                    if (file != null)
                    {
                        using (var stream = new FileStream(fileTosave, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            patient.Image = "/" + folder + "/" + imgFile;
                        }
                    }
                }
                else
                {
                    patient.Image = vm.Image;
                }
                var existTest = _context.PatientTestInfos.Where(x => x.PatientId == patient.PatientId).ToList();
                foreach (var item in existTest)
                {
                    _context.PatientTestInfos.Remove(item);
                }
                foreach (var test in testId)
                {
                    PatientTestInfo info = new PatientTestInfo
                    {
                        PatientId = patient.PatientId,
                        TestId = test
                    };
                    _context.PatientTestInfos.Add(info);
                }
                _context.Update(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                var deleteTest = _context.PatientTestInfos.Where(x => x.TestId == id).ToList();
                _context.PatientTestInfos.RemoveRange(deleteTest);
                _context.Patients.Remove(patient);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
