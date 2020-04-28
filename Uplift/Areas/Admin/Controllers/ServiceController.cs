using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Uplift.DataAccess.Data.Repository;
using Uplift.Models;
using Uplift.Models.ViewModels;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public ServiceVM serviceVM { get; set; }

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = webHost;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            serviceVM = new ServiceVM 
            {
                Service = new Service(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyListDropDown(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropDown()
            };

            if (id == null)
            {
                return View(serviceVM);
            }

            serviceVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            if (serviceVM.Service == null)
            {
                return NotFound();
            }
            
            return View(serviceVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert() //argument is from bindproperty above
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extension = Path.GetExtension(files[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    serviceVM.Service.ImageUrl = @"\images\services\" + fileName + extension;
                }
                if(serviceVM.Service.Id == 0)
                {
                    _unitOfWork.Service.Add(serviceVM.Service);
                }
                else
                {
                    _unitOfWork.Service.Update(serviceVM.Service);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includedProperties:"Category,Frequency") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Service.Get(id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Service.Remove(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleting Successful" });
        }

        #endregion
    }
}