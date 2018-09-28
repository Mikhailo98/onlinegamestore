using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Filter
{
    public class CustomApiException : Exception
    {
        public readonly HttpStatusCode Code;
        public readonly Dictionary<string, IEnumerable<string>> Fields;

        public CustomApiException(HttpStatusCode statusCode) : base(statusCode.ToString()) => Code = statusCode;

        public CustomApiException(HttpStatusCode statusCode, string message) : base(message) => Code = statusCode;

        public CustomApiException(HttpStatusCode statusCode, string message, Dictionary<string, IEnumerable<string>> fields) : base(message)
        {
            Code = statusCode;
            Fields = fields;
        }
    }
}
