using System;
namespace Common.Core
{
    public class GenericResponse<T> where T : class, new()
    {
        public string MetaData { get; set; }
        public Exception Error { get; set; }
        public bool Success { get; set; }
        public T Response { get; set; }
    }
    public class StringResponse
    {
        public Exception Error { get; set; }
        public bool Success { get; set; }
        public string Response { get; set; }
    }
    public class BooleanResponse
    {
        public Exception Error { get; set; }
        public bool Success { get; set; }
    }
}
