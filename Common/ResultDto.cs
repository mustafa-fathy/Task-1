namespace Common
{
    public class ResultDto
    {
        public object Result { get; set; }
        public string Message { get; set; }
    }
    public class ResultDtoThirdParty<T>
    {
        public T Result { get; set; }

    }
}
