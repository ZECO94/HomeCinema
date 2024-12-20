﻿using HomeCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCinema.DataAccess.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
        void UpdateStripePayment(int id, string seesionId, string paymentInrenId);

    }
}
