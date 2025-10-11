using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BookingSystem.Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IUnitOfWork   _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork=unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var vaills = _unitOfWork.Villa.GetAll();
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
            if(ModelState.IsValid)
            {
                if(obj.Image !=null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath=Path.Combine(_webHostEnvironment.WebRootPath,@"images\Villa");

                    if(!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    if(!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(imagePath,fileName),FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\Villa\" + fileName;

                }
                 _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been created successfuly";
                return RedirectToAction("Index");
            }
            return View();
        }

        
        public IActionResult Edit(int id )
        {
            Villa? obj = _unitOfWork.Villa.Get(x => x.Id == id);
            if(obj==null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Villa obj  )
        {
            if(ModelState.IsValid)
            {
                if(obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Villa");

                    if(!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    if(!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\Villa\" + fileName;
                }

                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been updated successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.Villa.Get(x => x.Id == id);
            return View(obj);

        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var obj = _unitOfWork.Villa.Get(x => x.Id == id);
            if(obj == null)
            {
                return RedirectToAction("Index");
            }

            if(!string.IsNullOrEmpty(obj.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                if(System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _unitOfWork.Villa.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "The villa has been deleted successfuly";
            return RedirectToAction("Index");
        }
      

    }
}
