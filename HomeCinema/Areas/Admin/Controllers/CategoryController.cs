using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.CategoryRepository.Get(null);
            return View(categories);
        }
        public IActionResult Create()
        {
            var category = new Category();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(category);
                return RedirectToAction("Index","Category");
            }
            return View(category);
        }
        public ActionResult Edit(int id) 
        {
            var category = _unitOfWork.CategoryRepository.GetOne(x => x.Id == id);
            return category!=null ? View(category) : NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category) 
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Update(category);
                return RedirectToAction("Index","Category");
            }
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetOne(x => x.Id == id);
            if(category!=null)
            {
                _unitOfWork.CategoryRepository.Remove(category);
                return RedirectToAction("Index", "Category");  
            }
            return RedirectToAction("NotFound", "Home");
        }
    }
}
