using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Infrastructure.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber> , IVillaNumberRepository
    {
        private readonly ApplicationDbContext _context;
        public VillaNumberRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
     
        public void Update(VillaNumber entity)
        {
            _context.VillaNumbers.Update(entity);
        }
    }
}
