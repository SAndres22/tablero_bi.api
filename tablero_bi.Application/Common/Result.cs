namespace tablero_bi.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public int code { get; private set; }
        public List<string> message { get; private set; } = new List<string>();
        public T data { get; private set; } = default(T);

        public Result<T> Success(T data, List<string> message = null, bool success = true, int code = 200)
        {

            return new Result<T>()
            {
                message = message,
                data = data,
                code = code,
                IsSuccess = success
            };
        }

        public Result<T> Failed(List<string> message, T data = default(T), bool success = false, int code = 400)
        {
            return new Result<T>()
            {
                message = message,
                data = data,
                code = code,
                IsSuccess = success
            };
        }
    }
}
