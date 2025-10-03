using BookingSystem.Application.Common.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookingSystem.Infrastructure.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _context;
        public VillaRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }


        public void Add(Villa entity)
        {
            _context.Add(entity);
        }

        public Villa Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
        {
            IQueryable<Villa> query = _context.Set<Villa>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Villa> query = _context.Set<Villa>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if(! string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Remove(Villa entity)
        {
            _context.Remove(entity); 
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Villa entity)
        {
            _context.Villas.Update(entity);
        }
    }
}
