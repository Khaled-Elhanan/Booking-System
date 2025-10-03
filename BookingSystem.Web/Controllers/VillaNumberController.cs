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
        public IActionResult Edit(int villaNumberId)
        {
            
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _context.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber=_context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
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
                _context.VillaNumbers.Update(vm.VillaNumber);
                _context.SaveChanges();
                TempData["success"] = "The villa Number has been updated successfuly";
                return RedirectToAction("Index");
            }
         
            vm.VillaList = _context.Villas.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(vm);

        }


        public IActionResult Delete(int villaNumberId)
        {
            var obj = _context.VillaNumbers
                .Include(v => v.Villa)
                .FirstOrDefault(x => x.Villa_Number == villaNumberId);
            return View(obj);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(VillaNumber vm)
        {
            VillaNumber? objFromDb = _context.VillaNumbers
                .FirstOrDefault(u => u.Villa_Number == vm.Villa_Number);
            if(objFromDb is not null)
            {
                _context.VillaNumbers.Remove(objFromDb);
                _context.SaveChanges();
                TempData["success"] = "The villa has been deleted successfuly";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa number could not be deleted ";
            return View();
        }


    }
}
