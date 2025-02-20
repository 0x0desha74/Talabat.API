using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly StoreContext _dbContext;
        private Hashtable? _repositories;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories is null)
                _repositories = new Hashtable();
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, repository);
            }
            
            return _repositories[type] as GenericRepository<TEntity>;
        }



        public async Task<int> Complete()
            => await _dbContext.SaveChangesAsync();


        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
    }
}
