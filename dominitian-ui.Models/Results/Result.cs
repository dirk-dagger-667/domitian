using dominitian.Models.Extensions;

namespace dominitian_ui.Models.Results
{
    public class Result
    {
        protected Result(bool isSuccess, ResultTypes resultType, Error? error = null)
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
            : this(false, ResultTypes.BadRequest, Error.Exception)
        {
            if (ex is null)
            {
                throw new ArgumentException("Invalid error", nameof(ex));
            }

            Exception = ex;
            InnerException = ex.GetInnermostException();
        }

        public Exception? Exception { get; protected set; }

        public string? InnerException { get; protected set; }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public ResultTypes Type { get; protected set; }

        public Error? Error { get; }

        public static Result Success() => new(true, ResultTypes.Ok, Error.None);

        public static Result Created() => new (true, ResultTypes.CreatedAt, Error.CreatedEntity);

        public static Result Failure(ResultTypes type, Error? error = null) => new(false, type, error);

        public static Result Failure(Exception ex) => new(ex);
    }

    public class Result<T> : Result
    {
        private Result(T? data, bool isSuccess, ResultTypes type, Error? error = null)
            : base(isSuccess, type, error)
            => Data = data;

        private Result(Exception ex)
            : this(default, false, ResultTypes.BadRequest, Error.Exception)
        {
            Exception = ex;
            InnerException = ex.GetInnermostException();
        }

        public T? Data { get; set; }

        public static Result<T> Success(T data) => new(data, true, ResultTypes.Ok, Error.None);

        public static Result<T> Created(T data) => new(data, true, ResultTypes.CreatedAt, Error.CreatedEntity);

        public static new Result<T> Failure(ResultTypes type, Error? error = null) => new(default, false, type, error);

        public static new Result<T> Failure(Exception ex) => new(ex);
    }
}
