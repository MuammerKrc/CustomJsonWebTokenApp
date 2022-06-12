using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.IRepositories;
using CoreLayer.IServices;
using CoreLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SharedLayer.Dtos;

namespace DataLayer.Repositories
{
    public class BaseRepository<TModel,TKey>: IBaseRepositories<TModel,TKey> where TModel:BaseModel<TKey>
    {
        private readonly DbContext _context;
        private readonly DbSet<TModel> _dbset;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbset = _context.Set<TModel>();
        }

        public async  Task<TModel> GetByIdAsync(TKey id)
        {
            var result = await _dbset.FirstOrDefaultAsync(i => i.Id.Equals(id));
            return result;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return  await _dbset.ToListAsync();
        }

        public IQueryable<TModel> Where(Expression<Func<TModel, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }

        public  void Add(TModel model)
        {
            _context.Entry<TModel>(model).State = EntityState.Added;
        }

        public void Update(TModel model)
        {
            _context.Entry<TModel>(model).State = EntityState.Modified;
        }

        public async Task RemoveAsync(TKey id)
        {
            var result=await _dbset.FirstOrDefaultAsync(i => i.Id.Equals(id));
            result.IsActive = false;
            Update(result);
        }
    }
}
