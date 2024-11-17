using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IActorRepository ActorRepository { get; }
        IMovieRepository MovieRepository { get; }
        ICartRepository CartRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }
        ICompanyRepository CompanyRepository { get; }


    }
}
