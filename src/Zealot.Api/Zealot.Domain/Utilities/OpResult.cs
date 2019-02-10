namespace Zealot.Domain.Utilities
{
    public class OpResult
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public static OpResult Ok
        {
            get
            {
                return new OpResult { Success = true };
            }
        }
        public static OpResult Bad(string errorCode, string errorMessage)
        {
            return new OpResult
            {
                Success = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }
    }
}
