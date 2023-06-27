using CodeSamples.Extensions;
using System.Data.Common;
using System.Data.SqlClient;

namespace CodeSamples.Exceptions
{
    public class SampleDbException : DbException
    {
        public SampleDbException(SqlException e, object parameters) : base($"parameters: {parameters.SimpleToJson()}", e)
        {

        }

        public SampleDbException(SqlException e, string storedProcedureName, object parameters) : base($"{storedProcedureName} has error with parameters: {parameters.SimpleToJson()}", e)
        {

        }
    }
}
