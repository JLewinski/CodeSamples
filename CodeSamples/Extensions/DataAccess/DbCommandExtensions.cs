using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSamples.Extensions.DataAccess
{
    public static class DbCommandExtensions
    {
        public static void AddParameter(this SqlCommand command, string name, object value, Type valueType)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = name;

            if (valueType == typeof(DataTable))
            {
                parameter.Value = value;
                parameter.SqlDbType = SqlDbType.Structured;
            }
            else if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = value;
            }

            command.Parameters.Add(parameter);
        }

        public static void AddParameters(this SqlCommand command, object parameters)
        {
            foreach (var property in parameters.GetType().GetProperties())
            {
                command.AddParameter(property.Name, property.GetValue(parameters), property.PropertyType);
            }
        }
    }
}
