using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using HomeCinema.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovieController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var movies = _unitOfWork.MovieRepository.Get(null,x=>x.Category);
            return View(movies);
        }
        public IActionResult Create()
        {
            var movie = new Movie()
            {
                Code = Guid.NewGuid().ToString()
            };
            var categories = _unitOfWork.CategoryRepository.Get(null);
            ViewBag.Categories = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            ViewBag.Status = Enum.GetValues(typeof(MovieStatus)).
            Cast<MovieStatus>().Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            });
            return View(movie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.MovieRepository.Add(movie);
                return RedirectToAction("Index", "Movie");
            }
            return View(movie);
        }
        public IActionResult Edit(int id)
        {
            var categories = _unitOfWork.CategoryRepository.Get(null);
            ViewBag.Categories = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            ViewBag.Status = Enum.GetValues(typeof(MovieStatus)).
            Cast<MovieStatus>().Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            });
            var movie = _unitOfWork.MovieRepository.GetOne(x => x.Id == id);
            return movie!=null ? View(movie) : RedirectToAction("NotFound","Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie movie)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.MovieRepository.Update(movie);
                return RedirectToAction("Index", "Movie");
            }
            return View(movie);
        }
        public IActionResult Delete(int id)
        {
            var movie = _unitOfWork.MovieRepository.GetOne(x => x.Id == id);
            if (movie != null)
            {
                _unitOfWork.MovieRepository.Remove(movie);
                return RedirectToAction("Index", "Movie");
            }
            return RedirectToAction("NotFound","Home");
        }
    }
}
