using domitian.Models.Extensions;

namespace domitian.Models.Results
{
    public class Result
    {
        protected Result(bool isSuccess, ResultType resultType, Error? error = null)
        {
            if (error is not null 
                && (isSuccess && (error != Error.None && error != Error.CreatedEntity) 
                    || !isSuccess && (error == Error.None && error == Error.CreatedEntity)))
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            Type = resultType;
        }

        protected Result(Exception ex)
            : this(false, ResultType.BadRequest, Error.Exception)
        {
            if (ex is null)
            {
                throw new ArgumentException("Invalid error", nameof(ex));
            }

            Exception = ex;
            InnerException = ex.GetInnermostExceptionMessage();
        }

        public string Title { get; set; }

        public Exception? Exception { get; protected set; }

        public string? InnerException { get; protected set; }
      
        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public ResultType Type { get; protected set; }

        public Error? Error { get; }

        public static Result Success() => new(true, ResultType.Ok, Error.None);

        public static Result Created() => new (true, ResultType.CreatedAt, Error.CreatedEntity);

        public static Result Failure( string title, ResultType type, Error? error = null) => new(false, type, error);

        public static Result Failure(Exception ex) => new(ex);
    }

    public class Result<T> : Result
    {
        private Result(T? data, bool isSuccess, ResultType type, Error? error = null)
            : base(isSuccess, type, error)
            => Data = data;

        private Result(Exception ex)
            : this(default, false, ResultType.BadRequest, Error.Exception)
        {
            Exception = ex;
            InnerException = ex.GetInnermostExceptionMessage();
        }

        public T? Data { get; set; }

        public static Result<T> Success(T data) => new(data, true, ResultType.Ok, Error.None);

        public static Result<T> Created(T data) => new(data, true, ResultType.CreatedAt, Error.CreatedEntity);

        public static new Result<T> Failure(string title, ResultType type, Error? error = null) => new(default, false, type, error);

        public static new Result<T> Failure(Exception ex) => new(ex);
    }
}
