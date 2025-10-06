using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;
using BookingSystem.Infrastructure.Migrations;
using BookingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace BookingSystem.Web.Controllers
{
    
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
          
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var vaills = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(vaills);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumber = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(villaNumber);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VillaNumberVM vm)
        {
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(x => x.Villa_Number == vm.VillaNumber.Villa_Number);

            if(ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.VillaNumber.Add(vm.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa Number has been created successfuly";
                return RedirectToAction("Index");
            }
            if(roomNumberExists)
            {
                TempData["error"] = "The villa Number already exists";
            }
            vm.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(vm);
           
            
        }
        public IActionResult Edit(int villaNumberId)
        {
            
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber= _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberId)
            };

            if (villaNumberVM == null) {
                return RedirectToAction("Error","Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VillaNumberVM vm)
        {
         

            if (ModelState.IsValid )
            {
                _unitOfWork.VillaNumber.Update(vm.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa Number has been updated successfuly";
                return RedirectToAction("Index");
            }
         
            vm.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(vm);

        }


        public IActionResult Delete(int villaNumberId)
        {
            var villaNumber = _unitOfWork.VillaNumber.
                Get(x => x.Villa_Number == villaNumberId, includeProperties: "Villa");
            if ( villaNumber==null)
            {
                return  RedirectToAction("Error", "Home");  
            }
            return View(villaNumber);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(VillaNumber vm)
        {
            VillaNumber? objFromDb = _unitOfWork.VillaNumber
                .Get(u => u.Villa_Number == vm.Villa_Number);
            if(objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfuly";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa number could not be deleted ";
            return View();
        }


    }
}
