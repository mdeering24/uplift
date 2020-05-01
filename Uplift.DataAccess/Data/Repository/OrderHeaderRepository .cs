using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRespository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
        public void Update(OrderHeader orderHeader)
        {
            var objFromDb = _db.OrderHeaders.FirstOrDefault(s => s.Id == orderHeader.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = orderHeader.Name;
                objFromDb.PhoneNumber = orderHeader.PhoneNumber;
                objFromDb.Email = orderHeader.Email;
                objFromDb.StreetAddress = orderHeader.StreetAddress;
                objFromDb.City = orderHeader.City;
                objFromDb.State = orderHeader.State;
                objFromDb.PostalCode = orderHeader.PostalCode;
                objFromDb.Status = orderHeader.Status;
                objFromDb.Comments = orderHeader.Comments;
                objFromDb.ServiceCount = orderHeader.ServiceCount;
            }
        }
    }
}
