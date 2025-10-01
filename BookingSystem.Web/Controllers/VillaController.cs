using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BookingSystem.Web.Controllers
{
    
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var vaills = _context.Villas.ToList();
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
            _context.Villas.Add(obj);
            _context.SaveChanges();
            TempData["success"] = "The villa has been created successfuly";
            return RedirectToAction("Index");
        }

        
        public IActionResult Edit(int id )
        {
            Villa? obj = _context.Villas.FirstOrDefault(x => x.Id == id);
            if(obj==null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Villa obj  )
        {
             _context.Villas.Update(obj);
             _context.SaveChanges();
            TempData["success"] = "The villa has been updated successfuly";
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var obj = _context.Villas.FirstOrDefault(x => x.Id == id);
            return View(obj);

        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var obj = _context.Villas.FirstOrDefault(x => x.Id == id);
            _context.Villas.Remove(obj);
            _context.SaveChanges();
            TempData["success"] = "The villa has been deleted successfuly";
            return RedirectToAction("Index");
        }
      

    }
}
