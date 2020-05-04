using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return Json(new { data = _unitOfWork.ApplicationUsers.GetAll(u=> u.Id != claims.Value) });
        }

        [HttpGet]
        public IActionResult Lock(string id)
        {
            if(id != null)
            {
                _unitOfWork.ApplicationUsers.LockUser(id);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult UnLock(string id)
        {
            if (id != null)
            {
                _unitOfWork.ApplicationUsers.UnLockUser(id);
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}