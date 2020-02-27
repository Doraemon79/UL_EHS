namespace UlApi.Models
{
    public class Output
    {
        public string Result { get; }
        public string ErrorMessage { get; }
        public int ErrorCode { get; }

        public Output(string result)
        {
            Result = result;
        }

        public Output(string errorMessage, int errorCode)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }
    }
}
