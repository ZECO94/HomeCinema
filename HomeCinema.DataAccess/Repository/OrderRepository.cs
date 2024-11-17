using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using HomeCinema.Utility;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.DataAccess.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
		private ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext _context) : base(_context)
        {
			_context = _context;
        }

		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
			var orderDb = _context.Orders.FirstOrDefault(x => x.Id == id);
			if(orderDb != null) 
			{
				orderDb.OrderStatus = orderStatus;
				if (!string.IsNullOrEmpty(paymentStatus))
				{
					orderDb.PaymentStatus = paymentStatus;
				}
			}
		}

		public void UpdateStripePayment(int id, string seesionId, string paymentInrenId)
		{
			var orderDb = _context.Orders.FirstOrDefault(x => x.Id == id);
			if(!string.IsNullOrEmpty(seesionId))
			{
				orderDb.SessionId = seesionId;
			}
			if(!string.IsNullOrEmpty(paymentInrenId))
			{
				orderDb.PaymentIntentId = paymentInrenId;
				orderDb.OrderDate = DateTime.Now;
			}


		}
	}
}
