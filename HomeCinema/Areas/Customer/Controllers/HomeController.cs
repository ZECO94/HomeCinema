using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace HomeCinema.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;




        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var movies = _unitOfWork.MovieRepository.Get(null, x => x.Category);
            return View(movies);
        }
        public IActionResult Details(int movieId)
        {
            Cart newCart = new()
            {
                Movie = _unitOfWork.MovieRepository.GetOne(x => x.Id == movieId, x => x.Actors, x => x.Category),
                MovieId = movieId
            };
            return View(newCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(Cart cart)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            cart.ApplicationUserId = userId;
            cart.Count = 1;
            _unitOfWork.CartRepository.Add(cart);
            return RedirectToAction("Index", "Home");
        }
            
            //var result = _unitOfWork.CartRepository.Get(x => x.ApplicationUserId == userId, x => x.Movie);
            //TempData["result"] = JsonConvert.SerializeObject(result);
            //ViewBag.Total = result.Sum(x => x.Count * x.Movie.Price);
            //return View(result);
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
