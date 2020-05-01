using System;
using System.Collections.Generic;
using System.Text;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IOrderDetailsRespository : IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);
    }
}
