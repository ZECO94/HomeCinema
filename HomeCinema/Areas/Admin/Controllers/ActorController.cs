using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ActorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var actors = _unitOfWork.ActorRepository.Get(null);
            return View(actors);
        }
        public IActionResult Create()
        {
            var actor = new Actor();
            return View(actor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Actor actor)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ActorRepository.Add(actor);
                return RedirectToAction("Index", "Actor");
            }
            return View(actor);
        }
        public IActionResult Edit(int id)
        {
            var actor = _unitOfWork.ActorRepository.GetOne(x => x.Id == id);
            return actor != null ? View(actor) : RedirectToAction("NotFound", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Actor actor)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ActorRepository.Update(actor);
                return RedirectToAction("Index", "Actor");
            }
            return View(actor);
        }
        public IActionResult Delete(int id)
        {
            var actor = _unitOfWork.ActorRepository.GetOne(x => x.Id == id);
            if (actor != null)
            {
                _unitOfWork.ActorRepository.Remove(actor);
                return RedirectToAction("Index", "Actor");
            }
            return RedirectToAction("NotFound", "Home");
        }
    }
}
