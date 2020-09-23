using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataLayer.Entities;
using Library.Models;

namespace BusinessLayer.InrefacesRepository
{
    public interface IRepository<TElement> where TElement : IEntity<int>
    {
        IQueryable<TElement> GetAll();
        Task<TElement> GetByIdAsync(int? id);
        Task CreateAsync(TElement entity);
        Task UpdateAsync(TElement entity);
        void Delete(int? id);
        void Delete(TElement entity);
        void DeleteRange(IEnumerable<TElement> entities);
        IQueryable<TElement> Include(params Expression<Func<TElement, object>>[] includeProperties);
        Task SaveChanges();
    }
}
