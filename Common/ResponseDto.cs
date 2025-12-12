namespace Common
{
    public class ResponseDto<T>
    {
        public ResultDto Result { get; set; }

        public ErrorDto Error { get; set; }

        public bool IsSuccess { get; set; }

        private ResponseDto(bool status, ResultDto result, ErrorDto error)
        {
            IsSuccess = status;
            Result = result;
            Error = error;
        }
        public static ResponseDto<T> Success(ResultDto result)
        {
            return new ResponseDto<T>(true, result, null);

        }
        public static ResponseDto<T> Failure(ErrorDto error)
        {
            return new ResponseDto<T>(false, null, error);

        }


    }
}
