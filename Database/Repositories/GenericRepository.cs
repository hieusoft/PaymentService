using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentService.Models.Entities;

namespace PaymentService.Database.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbContext context;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T?> GetOneAsync(ulong id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
        }
        
        public async Task DeleteAsync(ulong id)
        {

            var entity = await dbSet.FindAsync(id);
            if (entity != null) dbSet.Remove(entity);
        }
        
        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}