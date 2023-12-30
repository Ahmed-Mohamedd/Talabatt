using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Repositories;
using Talabat.Core.specifications;
using Talabat.DAL.Entities;
using Talabat.Repository.Context;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context=context;
        }
        #region without specifications
        public async Task<IEnumerable<T>> GetAll()
            => await _context.Set<T>().ToListAsync();

        public async Task<T> GetById(int id)
            => await _context.Set<T>().FindAsync(id); 
        #endregion

        public async Task<IEnumerable<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
            => await ApplySpecifications(spec).CountAsync();

        public IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return  SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }


        public async Task Add(T entity)
        => await _context.Set<T>().AddAsync(entity);

        public void Delete(T entity)
            => _context.Set<T>().Remove(entity);

        public void Update(T entity)
            => _context.Set<T>().Update(entity);
    }
}
