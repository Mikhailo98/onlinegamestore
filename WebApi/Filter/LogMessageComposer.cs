using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Filter
{
    public static class LogMessageComposer
    {
        public static string Compose(params KeyValuePair<string, object>[] values)
        {
            return JsonConvert.SerializeObject(values);
        }

        public static string Compose(params object[] values)
        {
            return JsonConvert.SerializeObject(values);
        }

        public static string Compose<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
