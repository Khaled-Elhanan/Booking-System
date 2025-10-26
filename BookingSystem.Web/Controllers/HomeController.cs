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
                VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenities")  ,
                CheckInDate= DateOnly.FromDateTime(DateTime.Now),
                
                Nights= 1
            };
            
            return View(homeVM);
        }

       


        [HttpPost]
        public IActionResult GetVillasByDate(int nights , DateOnly checkInDate)
        {
            var villasList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenities");
            foreach (var villa in villasList)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }
            HomeVM homeVM = new()
            {
                CheckInDate = checkInDate,
                VillaList = villasList,
                
                Nights = nights

            };
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
