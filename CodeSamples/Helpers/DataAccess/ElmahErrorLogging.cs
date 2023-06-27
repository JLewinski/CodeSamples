using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeSamples.Helpers.DataAccess
{
    public static class ElmahErrorLogging
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

        public static void LogNotFound(string storedProcedureName, object parameters)
        {
            //Log(new Exceptions.NotFoundDbException(storedProcedureName, parameters, new StackTrace()));
        }

        public static void LogParameters(Exception ex, object parameters)
        {
            Log(ex, $"parameters: {parameters.SimpleToJson()}");
        }

        public static void Log(Exception ex)
        {
            Log(ex, null);
        }

        public static void Log(Exception ex, string errorMessage = null)
        {
            Log(ex, errorMessage, null, null);
        }
        public static void Log(Exception ex, string errorMessage = null, string extraStacktrace = null)
        {
            Log(ex, errorMessage, extraStacktrace, null);
        }

        public static void Log(Exception ex, string errorMessage = null, string extraStacktrace = null, string hostName = null)
        {
            if (ex == null)
            {
                ex = new Exception(errorMessage);
            }

            string errorMessageSecondary = "";
            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = ex.Message;
            }
            else
            {
                errorMessageSecondary = ex.Message;
            }

            var error = new Elmah.Error(ex)
            {
                ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                Message = errorMessage,
                Detail = $"{errorMessageSecondary} \n{extraStacktrace}",
                Source = AppDomain.CurrentDomain.FriendlyName,
                HostName = hostName ?? (HttpContext.Current != null ? "Website" : AppDomain.CurrentDomain.FriendlyName)
            };

            error.Detail += $"\nStack Trace:\n{ex.StackTrace ?? new StackTrace().ToString()}";

            var innerException = ex.InnerException;
            while (innerException != null)
            {
                error.Detail += $"\n\nInner Exception: {innerException.Message}";
                if (string.IsNullOrEmpty(innerException.StackTrace))
                {
                    error.Detail += $"\nInner Stack Trace:\n{innerException.StackTrace ?? new StackTrace().ToString()}";
                }
                innerException = innerException.InnerException;
            }

            try
            {
                Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(error);
                Console.WriteLine($"~~ELMAH: {errorMessage} / {errorMessageSecondary}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
