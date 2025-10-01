using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;
using BookingSystem.Infrastructure.Migrations;
using BookingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Web.Controllers
{
    
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _context;
        public VillaNumberController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var vaills = _context.VillaNumbers.Include(x=>x.Villa).ToList();
            return View(vaills);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumber = new()
            {
                VillaList = _context.Villas.ToList().Select(x => new SelectListItem
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
            bool roomNumberExists = _context.VillaNumbers.Any(x => x.Villa_Number == vm.VillaNumber.Villa_Number);

            if(ModelState.IsValid && !roomNumberExists)
            {
                _context.VillaNumbers.Add(vm.VillaNumber);
                _context.SaveChanges();
                TempData["success"] = "The villa Number has been created successfuly";
                return RedirectToAction("Index");
            }
            if(roomNumberExists)
            {
                TempData["error"] = "The villa Number already exists";
            }
            vm.VillaList = _context.Villas.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(vm);
           
            
        }


        //public IActionResult Edit(int id)
        //{
        //    Villa? obj = _context.VillaNumbers.FirstOrDefault(x => x.VillaId == id);
        //    if (obj == null)
        //    {
        //        return RedirectToAction("Error", "Home");
        //    }
        //    return View(obj);
        //}

        //[HttpPost]
        //public IActionResult Edit(villaNumber obj)
        //{
        //    _context.VillaNumbers.Update(obj);
        //    _context.SaveChanges();
        //    TempData["success"] = "The villa has been updated successfuly";
        //    return RedirectToAction("Index");
        //}


        public IActionResult Delete(int id)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _context.Villas.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            var obj = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == id);
            return View(obj);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var obj = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == id);
            _context.VillaNumbers.Remove(obj);
            _context.SaveChanges();
            TempData["success"] = "The villa has been deleted successfuly";
            return RedirectToAction("Index");
        }


    }
}
