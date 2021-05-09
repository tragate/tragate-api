namespace Tragate.Common.Result
{
    public class OkResult : BaseResult
    {
        public object Data { get; set; }
        public string Message { get; set; }
    }
}