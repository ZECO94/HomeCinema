using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.DataAccess.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext _context) : base(_context)
        {

        }
    }
}
