using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRespository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
        public void Update(OrderDetails orderDetails)
        {
            var objFromDb = _db.OrderDetails.FirstOrDefault(s => s.Id == orderDetails.Id);
            if (objFromDb != null)
            {
                objFromDb.OrderHeaderId = orderDetails.OrderHeader.Id;
                objFromDb.ServiceId = orderDetails.Service.Id;
                objFromDb.ServiceName = orderDetails.Service.Name;
                objFromDb.Price = orderDetails.Service.Price;
            }
        }
    }
}
