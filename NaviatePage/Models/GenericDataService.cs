using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaviatePage.Models
{
    public class GenericDataService<T> : IDataService<T> where T : class
    {
        private readonly QuanLyKhoContextFactory _contextFactory;

        public GenericDataService(QuanLyKhoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> entityEntry = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
                return entityEntry.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var entity = await context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<T> Get(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().ToListAsync();
            }
        }

        public async Task<T> Update(int id, T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var existingEntity = await context.Set<T>().FindAsync(id);
                if (existingEntity != null)
                {
                    context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
                    return existingEntity;
                }
                return null;
            }
        }
    }
}