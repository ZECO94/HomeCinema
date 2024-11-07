using HomeCinema.DataAccess.Repository;
using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.Checkout;

namespace HomeCinema.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (id != 0)
            {
                Cart newCart = new()
                {
                    MovieId = id,
                    Count = 1,
                    ApplicationUserId = userId
                };
                _unitOfWork.CartRepository.Add(newCart);
                return RedirectToAction("Index", "Home");
            }
            var result = _unitOfWork.CartRepository.Get(x => x.ApplicationUserId == userId, x => x.Movie);
            TempData["result"] = JsonConvert.SerializeObject(result);
            ViewBag.Total = result.Sum(x => x.Count * x.Movie.Price);
            return View(result);
        }
        public IActionResult Summary() 
        {
            return View();
        }
        public IActionResult Increment(int id)
        {
            var cartItem = _unitOfWork.CartRepository.GetOne(c => c.Id == id);
            if (cartItem != null)
            {
                cartItem.Count++;
                _unitOfWork.CartRepository.Update(cartItem);
            }
            return RedirectToAction("Index","Cart");
        }
        public IActionResult Decrement(int id)
        {
            var cartItem = _unitOfWork.CartRepository.GetOne(c => c.Id == id);
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    _unitOfWork.CartRepository.Update(cartItem);
                }
                else
                {
                    _unitOfWork.CartRepository.Remove(cartItem);
                }
            }
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Remove(int id)
        {
            var cartItem = _unitOfWork.CartRepository.GetOne(c => c.Id == id);
            if (cartItem != null)
            {
                _unitOfWork.CartRepository.Remove(cartItem);
            }
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Payment()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/checkout/cancel",
            };

            string cart = (string)TempData["result"];
            var items = JsonConvert.DeserializeObject<IEnumerable<Cart>>(cart);

            foreach (var model in items)
            {
                var result = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "EGP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = model.Movie.Name,
                            Description = model.Movie.Description,
                        },
                        UnitAmount = (long)(model.Movie.Price * 100),
                    },
                    Quantity = model.Count,
                };
                options.LineItems.Add(result);
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}

