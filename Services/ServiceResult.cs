using System.Net;

namespace ExamApp.Services
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public List<string>? ErrorMessage { get; set; }
        public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0;
        public bool IsFail => !IsSuccess;
        public HttpStatusCode Status { get; set; }

        public static ServiceResult<T> Success(T data, HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ServiceResult<T>()
            {
                Data = data,
                Status = status
            };
        }

        public static ServiceResult<T> Fail(List<string> errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T>()
            {
                ErrorMessage = errorMessage,
                Status = status
            };
        }

        public static ServiceResult<T> Fail(string errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T>()
            {
                ErrorMessage = new List<string> { errorMessage },
                Status = status
            };
        }
    }


    //Geriye datayı döndürmeyen işlemler için kullanılacak olan sınıf(örneğin update, delete)
    public class ServiceResult
    {
        public List<string>? ErrorMessage { get; set; }
        public bool IsSuccess => ErrorMessage == null || ErrorMessage.Count == 0;
        public bool IsFail => !IsSuccess;
        public HttpStatusCode Status { get; set; }

        public static ServiceResult Success(HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ServiceResult()
            {
                Status = status
            };
        }

        public static ServiceResult Fail(List<string> errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult()
            {
                ErrorMessage = errorMessage,
                Status = status
            };
        }

        public static ServiceResult Fail(string errorMessage, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult()
            {
                ErrorMessage = new List<string> { errorMessage },
                Status = status
            };
        }
    }
}
