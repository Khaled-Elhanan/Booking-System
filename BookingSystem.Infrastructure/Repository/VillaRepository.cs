using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookingSystem.Infrastructure.Repository
{
    public class VillaRepository : Repository<Villa> ,  IVillaRepository
    {
        private readonly ApplicationDbContext _context;
        public VillaRepository(ApplicationDbContext context):base(context)
        {
            _context = context; 
        }


        public void Update(Villa entity)
        {
            _context.Villas.Update(entity);
        }
    }
}
