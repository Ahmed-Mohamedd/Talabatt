using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.specifications;
using Talabat.DAL.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery , ISpecification<T> spec)
        {

            var Query = InputQuery;

            if (spec.Criteria is not null)
                Query = Query.Where(spec.Criteria);

            if(spec.OrderBy is not null)
                Query = Query.OrderBy(spec.OrderBy);

            if (spec.OrderByDesc is not null)
                Query = Query.OrderByDescending(spec.OrderByDesc);
            
            if (spec.IsPaginationEnabled)
                Query=Query.Skip(spec.Skip).Take(spec.Take);

            Query = spec.Includes.Aggregate(Query, (CurrentQuery, include) => CurrentQuery.Include(include));

            return Query;
        }
    }
}
