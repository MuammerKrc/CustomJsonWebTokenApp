using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Models;

namespace CoreLayer.IRepositories
{
    public interface IBaseRepositories<TModel,TKey>where TModel:BaseModel<TKey>
    {
        Task<TModel> GetByIdAsync(TKey id);
        Task<IEnumerable<TModel>> GetAllAsync();
        IQueryable<TModel> Where(Expression<Func<TModel,bool>> predicate);
        void Add(TModel model);
        void Update(TModel model);
        Task RemoveAsync(TKey id);

    }
}
