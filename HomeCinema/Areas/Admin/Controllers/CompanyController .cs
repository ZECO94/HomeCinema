using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HomeCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.CompanyRepository.Get(null);
            return View(categories);
        }
        public IActionResult Create()
        {
            var company = new Company();
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.CompanyRepository.Add(company);
                return RedirectToAction("Index", "Company");
            }
            return View(company);
        }
        public ActionResult Edit(int id) 
        {
            var company = _unitOfWork.CompanyRepository.GetOne(x => x.Id == id);
            return company != null ? View(company) : NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company) 
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.CompanyRepository.Update(company);
                return RedirectToAction("Index", "Company");
            }
            return View(company);
        }
        public IActionResult Delete(int id)
        {
            var company = _unitOfWork.CompanyRepository.GetOne(x => x.Id == id);
            if(company != null)
            {
                _unitOfWork.CompanyRepository.Remove(company);
                return RedirectToAction("Index", "Company");  
            }
            return RedirectToAction("NotFound", "Home");
        }
    }
}
