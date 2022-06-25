using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.IRepositories;
using CoreLayer.IServices;
using CoreLayer.IUnitOfWorks;
using CoreLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using ServiceLayer.MapperSettings;
using SharedLayer.Dtos;

namespace ServiceLayer.Services
{
    public class BaseService<TModel, TDto, TKey> : IBaseService<TModel, TDto, TKey> where TModel : BaseModel<TKey> where TDto:class
    {
        private ILogger<BaseService<TModel, TDto, TKey>> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepositories<TModel,TKey> _repositories;

        public BaseService(IUnitOfWork unitOfWork, IBaseRepositories<TModel, TKey> repositories)
        {
            _unitOfWork = unitOfWork;
            _repositories = repositories;
        }

        protected Response<TDto> GetClientError()
        {
            return Response<TDto>.ErrorResponse("Client tarafında bir hata meydana geldi");
        }
        protected Response<IEnumerable<TDto>> GetClientErrors()
        {
            return Response<IEnumerable<TDto>>.ErrorResponse("Client tarafında bir hata meydana geldi");
        }
        protected Response<List<TDto>> GetClientErrorsList()
        {
            return Response<List<TDto>>.ErrorResponse("Client tarafında bir hata meydana geldi");
        }
        public async Task<Response<TDto>> GetByIdAsync(TKey id)
        {
            try
            {
                var result= await _repositories.GetByIdAsync(id);
                if (result != null)
                {
                    return Response<TDto>.SuccessResponse(ObjectMapper.Mapper.Map<TModel,TDto>(result),200);
                }
                else
                {
                    return Response<TDto>.ErrorResponse("Bu id sahip bir ürün bulunmadı",statusCode:400);
                }

            }
            catch (Exception e)
            {
               return GetClientError();
            }
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repositories.GetAllAsync();
                return Response<IEnumerable<TDto>>.SuccessResponse(ObjectMapper.Mapper.Map<IEnumerable<TModel>, IEnumerable<TDto>>(result), 200);
                
            }
            catch (Exception e)
            {
                return GetClientErrors();
            }
        }

        public async Task<Response<List<TDto>>> Where(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                var result = await _repositories.Where(predicate).ToListAsync();
                return Response<List<TDto>>.SuccessResponse(ObjectMapper.Mapper.Map<List<TModel>,List<TDto>>(result));

            }
            catch (Exception e)
            {
                return GetClientErrorsList();
            }
        }

        public async Task<Response<TDto>> AddAsync(TModel model)
        {
            try
            {
                 _repositories.Add(model);
                await _unitOfWork.SaveChangeAsync();
                return Response<TDto>.SuccessResponse(ObjectMapper.Mapper.Map<TModel, TDto>(model));
            }
            catch (Exception e)
            {
                return GetClientError();
            }
        }

        public async Task<Response<TDto>> UpdateAsync(TModel model)
        {
            try
            {
                _repositories.Update(model);
                await _unitOfWork.SaveChangeAsync();
                return Response<TDto>.SuccessResponse();
            }
            catch (Exception e)
            {
                return GetClientError();
            }
        }

        public async Task<Response<TDto>> RemoveAsync(TKey id)
        {
            try
            {
                await _repositories.RemoveAsync(id);
                await _unitOfWork.SaveChangeAsync();
                return Response<TDto>.SuccessResponse();
            }
            catch (Exception e)
            { 
                return GetClientError();
            }
        }
    }
}
