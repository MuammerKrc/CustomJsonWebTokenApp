using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Dtos
{
    public class Response<TModel> where TModel :class
    {
        public bool IsSuccess { get; private set; }
        public TModel Model { get; private set; }
        public  int StatusCode { get; private set; } = 200;
        public  ErrorDto Error { get; private set; }

        public static Response<TModel> SuccessResponse(TModel model, int statusCode=200)
        {
            return new Response<TModel>() { IsSuccess = true, Model = model, StatusCode = statusCode };
        }
        public static Response<TModel> SuccessResponse( int statusCode = 200)
        {
            return new Response<TModel>() { IsSuccess = true, StatusCode = statusCode ,};
        }
        public static Response<TModel> ErrorResponse(ErrorDto errors,int statusCode=500)
        {
            return new Response<TModel>(){Error = errors,StatusCode = statusCode,IsSuccess = false};
        }

        public static Response<TModel> ErrorResponse(string error,bool isShow=true, int statusCode = 500)
        {
            return new Response<TModel>() {Error = new ErrorDto(isShow:false,error:error),StatusCode = statusCode,IsSuccess = false};
        }
    }
}
