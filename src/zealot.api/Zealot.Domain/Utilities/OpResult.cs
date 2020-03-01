using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Zealot.Domain.Utilities
{
    public class OpResult
    {
        public bool Success { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public static OpResult Ok
        {
            get
            {
                return new OpResult { Success = true };
            }
        }
        public static OpResult Bad(ErrorCode errorCode, string errorMessage)
        {
            return new OpResult
            {
                Success = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }
    }

    public class OpResult<T> : OpResult
        where T : class
    {
        public T Object { get; set; }

        public OpResult()
        {
        }
        public OpResult(bool success, T @object)
        {
            Success = success;
            Object = @object;
        }
        public new static OpResult<T> Bad(ErrorCode errorCode, string errorMessage)
        {
            return new OpResult<T>
            {
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }
    }
}
