using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingSystem.Web.Controllers
{

    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var vaills = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(vaills);
        }

        public IActionResult Create()
        {
            AmenityVM amenitie = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(amenitie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AmenityVM vm)
        {
            bool roomNumberExists = _unitOfWork.Amenity.Any(x => x.Id == vm.Amenity.Id);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.Amenity.Add(vm.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been created successfuly";
                return RedirectToAction("Index");
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The amenity already exists";
            }
            vm.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(vm);


        }
        public IActionResult Edit(int amenitieId)
        {

            AmenityVM amenitieVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(x => x.Id == amenitieId)
            };

            if (amenitieVM == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenitieVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AmenityVM vm)
        {


            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(vm.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been updated successfuly";
                return RedirectToAction("Index");
            }

            vm.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(vm);

        }


        public IActionResult Delete(int amenitieId)
        {
            var amenityEntity = _unitOfWork.Amenity.
                Get(x => x.Id == amenitieId, includeProperties: "Villa");
            if (amenityEntity == null)
            {
                return RedirectToAction("Error", "Home");
            }

            AmenityVM vm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                Amenity = amenityEntity
            };

            return View(vm);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            Amenity? objFromDb = _unitOfWork.Amenity
                .Get(u => u.Id == id);
            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been deleted successfuly";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The amenity could not be deleted ";
            return View();
        }


    }
}
