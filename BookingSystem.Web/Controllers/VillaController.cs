using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BookingSystem.Web.Controllers
{
    
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepo;
        public VillaController(IVillaRepository villaRepo)
        {
            _villaRepo = villaRepo;
        }
        public IActionResult Index()
        {
            var vaills = _villaRepo.GetAll();
            return View(vaills);
        }

        public IActionResult Create()
        {

          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Villa obj)
        {
            _villaRepo.Add(obj);
            _villaRepo.Save();
            TempData["success"] = "The villa has been created successfuly";
            return RedirectToAction("Index");
        }

        
        public IActionResult Edit(int id )
        {
            Villa? obj = _villaRepo.Get(x => x.Id == id);
            if(obj==null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Villa obj  )
        {
            _villaRepo.Update(obj);
            _villaRepo.Save();
            TempData["success"] = "The villa has been updated successfuly";
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var obj = _villaRepo.Get(x => x.Id == id);
            return View(obj);

        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var obj = _villaRepo.Get(x => x.Id == id);
            _villaRepo.Remove(obj);
            _villaRepo.Save();
            TempData["success"] = "The villa has been deleted successfuly";
            return RedirectToAction("Index");
        }
      

    }
}
