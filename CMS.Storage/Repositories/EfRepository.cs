using Microsoft.EntityFrameworkCore;
using CMS.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Interfaces;
using System;
using CMS.Common.Interfaces;

namespace CMS.Storage.Repositories
{
    /// <summary>
    /// "There's some repetition here - couldn't we have some the sync methods call the async?"
    /// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly DbContext _dbContext;

        public EfRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(r => r.Idx == id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<DataTableResponse<T>> ListAsync(IDataTableRequest spec)
        {
            return AppyDataTable(spec);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                entity.InsertDate = DateTime.Now;
                _dbContext.Set<T>().Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                var local = _dbContext.Set<T>().Local.FirstOrDefault(entry => entry.Idx.Equals(entity.Idx));
                if (local != null)
                {

                    _dbContext.Entry(local).State = EntityState.Detached;
                }
                entity.UpdateDate = DateTime.Now;
                _dbContext.Entry(entity).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                var local = _dbContext.Set<T>().Local.FirstOrDefault(entry => entry.Idx.Equals(entity.Idx));
                if (local != null)
                {

                    _dbContext.Entry(local).State = EntityState.Detached;
                }
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }

        private DataTableResponse<T> AppyDataTable(IDataTableRequest spec)
        {
            return DataTableEvaluator<T>.GetQueryable(_dbContext.Set<T>().Where(d => d.Status == 0).AsQueryable(), spec);
        }
    }
}
