using BusinessLayer.InrefacesRepository;
using DataLayer.Entities;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.ImplementationsRepository
{
    public class Repository<TElement> : IRepository<TElement> where TElement : class, IEntity<int>
    {
        private readonly DbContext db;
        private readonly DbSet<TElement> _dbSet;

        public Repository(DbContext db)
        {
            this.db = db;
            _dbSet = db.Set<TElement>();
        }

        public async Task CreateAsync(TElement entity)
        {
            await _dbSet.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async void Delete(int? id)
        {
            _dbSet.Remove(await GetByIdAsync(id));
            await db.SaveChangesAsync();
        }

        public async void Delete(TElement entity)
        {
            _dbSet.Remove(entity);
            await db.SaveChangesAsync();
        }

        public async void DeleteRange(IEnumerable<TElement> entities)
        {
            _dbSet.RemoveRange(entities);
            await db.SaveChangesAsync();
        }

        public IQueryable<TElement> GetAll()
        {
            return _dbSet.AsQueryable();
        }
        
        public async Task<TElement> GetByIdAsync(int? id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(TElement entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IQueryable<TElement> Include(params Expression<Func<TElement, object>>[] includeProperties)
        {
            IQueryable<TElement> query = _dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        
        public async Task SaveChanges()
        {
           await db.SaveChangesAsync();
        }
    }
}
