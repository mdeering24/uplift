using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uplift.DataAccess.Data.Repository;
using Uplift.Models;
using Uplift.Models.ViewModels;

namespace Uplift.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private HomeVM _homeVM;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _homeVM = new HomeVM
            {
                CategoryList = _unitOfWork.Category.GetAll(),
                ServiceList = _unitOfWork.Service.GetAll(includedProperties: "Frequency")
            };
        }

        public IActionResult Index()
        {
            return View(_homeVM);
        }

        public IActionResult Details(int id)
        {
            Service service = _unitOfWork.Service.GetFirstOrDefault(includedProperties: "Category,Frequency", filter: c => c.Id == id);
            return View(service);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
