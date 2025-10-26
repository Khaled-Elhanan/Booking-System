using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Application.Common.Utility;
using BookingSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingSystem.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        public IActionResult FinalizeBooking(int villaId, DateOnly checkInDate , int nights )
        {

            var clamisIdentity = (ClaimsIdentity)User.Identity;
            var userId = clamisIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = _unitOfWork.User.Get(x => x.Id ==userId);

            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.Villa.Get(v => v.Id == villaId, includeProperties:"VillaAmenities"),
                // Booking date: when the booking was made (current date/time)
                BookingDate = DateTime.Now,
                // Check-in date: the date the guest will arrive
                CheckInDate = checkInDate.ToDateTime(TimeOnly.MinValue),
                Nights = nights,
                // Check-out date: the date after spending all nights (checkInDate + nights)
                // Example: Check-in Jan 1st, 3 nights = Check-out Jan 4th
                CheckOutDate = checkInDate.AddDays(nights).ToDateTime(TimeOnly.MinValue),
                UserId=userId ,
                Phone=user.PhoneNumber,
                Email=user.Email,
                Name=user.Name

                
            };
            booking.TotalCost = booking.Villa.Price * nights;
            return View(booking);
        }
        [Authorize]
        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var villa = _unitOfWork.Villa.Get(x => x.Id == booking.VillaId);
            booking.TotalCost = villa.Price * booking.Nights;
            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;
            booking.UserId = userId;

            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
            return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
        }
        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            return View(bookingId);
        }
    }
   
}
