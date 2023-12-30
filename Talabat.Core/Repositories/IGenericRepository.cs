using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.specifications;
using Talabat.DAL.Entities;

namespace Talabat.Core.Repositories
{
    public  interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Specifications
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        #endregion

        Task<IEnumerable<T>> GetAllWithSpec(ISpecification<T> spec);
        Task<T> GetByIdWithSpec(ISpecification<T> spec);

        Task<int> GetCountAsync(ISpecification<T> spec);

        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
