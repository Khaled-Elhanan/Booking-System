using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork=unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmentiy")  ,
                CheckInDate= DateOnly.FromDateTime(DateTime.Now),
                
                Nights= 1
            };
            
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult Index(HomeVM homeVM)
        {
            homeVM.VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmentiy");  
            foreach(var villa in homeVM.VillaList)
            {
                if(villa.Id %2==0)
                {
                    villa.IsAvailable = false; 
                }
            }
            

            return View(homeVM);
        }


        [HttpPost]
        public IActionResult GetVillasByDate(HomeVM homeVM)
        {
            var villasList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmentiy");
            foreach (var villa in villasList)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }
            homeVM.VillaList = villasList;
            
            // Add success message for AJAX response
            TempData["success"] = $"Availability checked for {homeVM.CheckInDate:MMM dd, yyyy} - {homeVM.Nights} night(s)";
            
            return PartialView("_VillaList", homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
