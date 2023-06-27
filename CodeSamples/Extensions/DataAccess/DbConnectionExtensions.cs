using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSamples.Extensions.DataAccess
{
    public static class DbConnectionExtensions
    {
        public static SqlCommand CreateStoredProcedure(this SqlConnection connection, string storedProcedureName, object parameters = null)
        {
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedureName;

            if (parameters != null)
            {
                command.AddParameters(parameters);
            }

            return command;
        }
    }
}
