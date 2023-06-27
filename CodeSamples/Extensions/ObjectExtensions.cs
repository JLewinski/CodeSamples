using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSamples.Extensions
{
    public static class ObjectExtensions
    {
        public static string SimpleToJson(this object parameters)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            foreach (var property in parameters.GetType().GetProperties())
            {
                sb.Append($"\t{property.Name}: {property.GetValue(parameters)?.ToString()}\n");
            }
            sb.Append("}\n\n");
            return sb.ToString();
        }
    }
}
