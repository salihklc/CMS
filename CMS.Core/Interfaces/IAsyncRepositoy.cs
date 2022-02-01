
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Common.Interfaces;
using CMS.Core.Entities;

namespace CMS.Core.Interfaces{

    public interface IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<DataTableResponse<T>> ListAsync(IDataTableRequest spec);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}