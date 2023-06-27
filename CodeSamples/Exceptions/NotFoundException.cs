using CodeSamples.Extensions;
using System;
using System.Text;

namespace CodeSamples.Exceptions
{
    public class NotFoundDbException : Exception
    {
        private readonly string details;

        public override string StackTrace => details + base.StackTrace;

        public NotFoundDbException(string storedProcedureName, object parameters, System.Diagnostics.StackTrace stackTrace = null) : base($"{storedProcedureName} could not find item")
        {
            var sb = new StringBuilder();
            sb.Append($"{storedProcedureName} could not find item with parameters:\n\n{parameters.SimpleToJson()}\n\n");

            if (stackTrace != null)
            {
                sb.Append(stackTrace.ToString());
                sb.Append("\n\n");
            }
            details = sb.ToString();
        }
    }
}
