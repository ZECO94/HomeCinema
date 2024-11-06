﻿using HomeCinema.DataAccess.Repository.IRepository;
using HomeCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.DataAccess.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext _context) : base(_context)
        {

        }
    }
}
