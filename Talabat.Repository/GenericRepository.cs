using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        } //ask CLR to create object from dbContext implicitly

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {

            return await _dbContext.Set<T>().ToListAsync();

        }



        public async Task<T> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _dbContext.Products
                        .Where(p => p.Id == id)
                        .Include(P => P.ProductBrand)
                        .Include(P => P.ProductType)
                        .FirstOrDefaultAsync() as T;

            }
            // _dbContext.Products .Include(p => p.Type) .Include(p => p.Brand).ToListAsync();
            return await _dbContext.Set<T>().FindAsync(id);
        }


        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }


        public async Task<T> GetByIdWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }


        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }



        public async Task AddAsync(T entity)
            => await _dbContext.Set<T>().AddAsync(entity);


        public void Update(T entity)
            => _dbContext.Set<T>().Update(entity);


        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

    }
}
