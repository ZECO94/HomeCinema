using HomeCinema.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public ICategoryRepository CategoryRepository { get; private set; }
        public IMovieRepository MovieRepository { get; private set; }
        public IActorRepository ActorRepository { get; private set; }
        public ICartRepository CartRepository { get; private set; }
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }
        public IOrderRepository OrderRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
            MovieRepository = new MovieRepository(_context);
            ActorRepository = new ActorRepository(_context);
            CartRepository = new CartRepository(_context);
            ApplicationUserRepository = new ApplicationUserRepository(_context);
            OrderDetailsRepository = new OdrerDetailsRepository(_context);
            OrderRepository = new OrderRepository(_context);
            CompanyRepository = new CompanyRepository(_context);
        }
    }
}
