using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
        {
            var query = inputQuery; //query = _dbContext.Products()


            //Where()
            if (spec.Criteria is not null) //P => P.Id == id
                query = query.Where(spec.Criteria); //query = _dbContext.Products.Where(P => P.Id == id)


            //OrderBy()
            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);


            //OrderByDescending()
            if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);


            //Skip(), Take()
            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip);
                query = query.Take(spec.Take);
            }

            //Include().Include()
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            //query = _dbContext.Products.Where(P => P.Id == id).OrderBy(P =>P.Name).Include(P => P.Brand).Include(P => P.Type);
            
            
            return query;
        }
    }
}
   