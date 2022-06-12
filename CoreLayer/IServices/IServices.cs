using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedLayer.Dtos;

namespace CoreLayer.IServices
{
    public interface IServices<TModel,TDto,TKey> where TModel:BaseModel<TKey> where TDto:class
    {
        Task<Response<TDto>> GetByIdAsync(TKey id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TModel, bool>> predicate);
        Task<Response<TDto>> AddAsync(TModel model);
        Task<Response<TDto>> UpdateAsync(TModel model);
        Response<NoResponse> Remove(TKey id);
    }
}
