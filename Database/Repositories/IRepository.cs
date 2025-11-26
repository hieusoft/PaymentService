using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentService.Models.Entities;

namespace PaymentService.Database.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetOneAsync(ulong id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(ulong Id);
        Task DeleteAsync(T entity);
        Task SaveAsync();
    }
}