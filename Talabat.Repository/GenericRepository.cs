using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        } //ask clr to create object from dbContext implicitly

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IEnumerable<T>) await _dbContext.Products
                        .Include(p => p.Type)
                        .Include(p => p.Brand)
                        .ToListAsync();
            }
            return await _dbContext.Set<T>().ToListAsync();

        }
        public async Task<T?> GetByIdAsync(int? id)
        {
            if(typeof(T) == typeof(Product))
            {
                return   await _dbContext.Products
                        .Where(p => p.Id == id)
                        .Include(P => P.Brand)
                        .Include(P => P.Type)
                        .FirstOrDefaultAsync() as T;

            }
            return await _dbContext.Set<T>().FindAsync(id);
        }
    }
}
