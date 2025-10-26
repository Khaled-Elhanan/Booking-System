using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;

namespace BookingSystem.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(Booking entity)
        {
            _context.Bookings.Update(entity);
        }
    }
}

