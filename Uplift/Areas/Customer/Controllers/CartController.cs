using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Uplift.DataAccess.Data.Repository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        [BindProperty]
        public CartVM cartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            cartVM = new CartVM
            {
                OrderHeader = new Models.OrderHeader(),
                ServiceList = new List<Service>()
            };
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach(int serviceId in sessionList)
                {
                    cartVM.ServiceList.Add(
                        _unitOfWork.Service.GetFirstOrDefault(
                            u => u.Id == serviceId,
                            includedProperties: "Frequency,Category")
                        );
                }
            }
            return View(cartVM);
        }

        public IActionResult Remove(int id)
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                sessionList.Remove(id);
                HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Summary()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    cartVM.ServiceList.Add(
                        _unitOfWork.Service.GetFirstOrDefault(
                            u => u.Id == serviceId,
                            includedProperties: "Frequency,Category")
                        );
                }
            }
            return View(cartVM);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                cartVM.ServiceList = new List<Service>();
                foreach (int serviceId in sessionList)
                {
                    cartVM.ServiceList.Add(
                        _unitOfWork.Service.GetFirstOrDefault(
                            u => u.Id == serviceId,
                            includedProperties: "Frequency,Category")
                        );
                }
            }
            if (!ModelState.IsValid)
            {
                return View(cartVM);
            }
            cartVM.OrderHeader.OrderDate = DateTime.Now;
            cartVM.OrderHeader.Status = SD.OrderStatusSubmitted;
            cartVM.OrderHeader.ServiceCount = cartVM.ServiceList.Count;
            _unitOfWork.OrderHeaders.Add(cartVM.OrderHeader);
            _unitOfWork.Save();

            foreach(var item in cartVM.ServiceList)
            {
                OrderDetails orderDetails = new OrderDetails
                {
                    ServiceId = item.Id,
                    OrderHeaderId = cartVM.OrderHeader.Id,
                    ServiceName = item.Name,
                    Price = item.Price
                };
                _unitOfWork.OrderDetails.Add(orderDetails);
            }
            _unitOfWork.Save();
            HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
            return RedirectToAction(nameof(OrderConfirmation), new { id = cartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            cartVM.OrderHeader = _unitOfWork.OrderHeaders.Get(id);
            var orderDetails = _unitOfWork.OrderDetails.GetAll(filter: o => o.OrderHeaderId == id, includedProperties: "Service,Frequency");
            foreach(var item in orderDetails)
            {
                cartVM.ServiceList.Add(item.Service);
            }
            return View(cartVM);
        }
    }
}