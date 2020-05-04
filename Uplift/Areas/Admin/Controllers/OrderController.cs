using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            if(status != null)
            {
                return Json(new { data = _unitOfWork.OrderHeaders.GetAll(o => o.Status == status) });
            }
            return Json(new { data = _unitOfWork.OrderHeaders.GetAll() });
        }
        #endregion
    }
}