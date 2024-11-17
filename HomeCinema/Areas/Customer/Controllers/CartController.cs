using HomeCinema.DataAccess.Repository;
using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using HomeCinema.Models.ViewModels;
using HomeCinema.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.Checkout;
using Stripe.Climate;

namespace HomeCinema.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize] 
    public class CartController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public CartVM cartVM { get; set; }
        private readonly IUnitOfWork _unitOfWork;

        public CartController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var result = _unitOfWork.CartRepository.Get(x => x.ApplicationUserId == userId, x => x.Movie);
            TempData["result"] = JsonConvert.SerializeObject(result);
            ViewBag.Total = result.Sum(x => x.Count * x.Movie.Price);
            return View(result);
        }
        public IActionResult Summary()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            cartVM = new()
            {
                carts = _unitOfWork.CartRepository.Get(c => c.ApplicationUserId == userId, x => x.Movie),
                Order = new()
            };
            cartVM.Order.ApplicationUser = _unitOfWork.ApplicationUserRepository.GetOne(x => x.Id == userId);
            cartVM.Order.Name = cartVM.Order.ApplicationUser.Name;
            cartVM.Order.Address = cartVM.Order.ApplicationUser.Address;
            cartVM.Order.State = cartVM.Order.ApplicationUser.State;
            cartVM.Order.City = cartVM.Order.ApplicationUser.City;
            cartVM.Order.PostalCode = cartVM.Order.ApplicationUser.PostalCode;
            cartVM.Order.PhoneNumber = cartVM.Order.ApplicationUser.PhoneNumber;
            var result = _unitOfWork.CartRepository.Get(x => x.ApplicationUserId == userId, x => x.Movie);
			ViewBag.Total = result.Sum(x => x.Count * x.Movie.Price);
			return View(cartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Summary(CartVM cart)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (cart.Order == null)
            {
                cart.Order = new();
            }

            // Get user details
            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.GetOne(x => x.Id == userId);

            // Populate order fields
            cart.Order.OrderDate = DateTime.Now;
            cart.Order.ApplicationUserId = userId;
            cart.Order.Name = applicationUser.Name;
            cart.Order.Address = applicationUser.Address;
            cart.Order.State = applicationUser.State;
            cart.Order.City = applicationUser.City;
            cart.Order.PostalCode = applicationUser.PostalCode;
            cart.Order.PhoneNumber = applicationUser.PhoneNumber;
            var result = _unitOfWork.CartRepository.Get(x => x.ApplicationUserId == userId, x => x.Movie);
            cart.Order.OrderTotal = (double)result.Sum(x => x.Count * x.Movie.Price);
             

            // Set order status based on user's company ID
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                cart.Order.OrderStatus = StaticDetails.StatusPending;
                cart.Order.PaymentStatus = StaticDetails.StatusPending;
            }
            else
            {
                cart.Order.OrderStatus = StaticDetails.PaymentStatusDelayed;
                cart.Order.PaymentStatus = StaticDetails.PaymentStatusApproved;
            }

            // Add order to database
            _unitOfWork.OrderRepository.Add(cart.Order);

            // Add order details
            foreach (var item in _unitOfWork.CartRepository.Get(c => c.ApplicationUserId == userId, x => x.Movie))
            {
                OrderDetails details = new()
                {
                    MovieId = item.MovieId,
                    OrderId = cart.Order.Id,
                    Price = (double)item.Movie.Price,
                    Qty = item.Count
                };
                _unitOfWork.OrderDetailsRepository.Add(details);
            }

            

            return RedirectToAction(nameof(Confirm), new { orderId = cart.Order.Id });
        }

        public IActionResult Confirm(int orderId)
        {
            return View(orderId);
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

