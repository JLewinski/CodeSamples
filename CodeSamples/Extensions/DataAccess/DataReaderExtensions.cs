using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace CodeSamples.Extensions.DataAccess
{
    public static class DataReaderExtensions
    {
        /// <summary>
        /// This calls the <see cref="GetValue{T}(IDataReader, string)"/> in a try-catch. This
        /// should be used if it is not certain that a column with <paramref name="columnName"/>
        /// exists in the row currently being read. However, it can be used in other cases
        /// </summary>
        /// <typeparam name="T">Type of the value being read</typeparam>
        /// <param name="columnName">Name of column</param>
        /// <param name="value">Value read</param>
        /// <returns></returns>
        public static bool TryGetValue<T>(this SqlDataReader reader, string columnName, out T value)
        {
            try
            {
                value = reader.GetValue<T>(columnName);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        ///<summary>
        ///    Gets the value of type <typeparamref name="T"/> from the specified column <paramref name="columnName"/>.
        ///</summary>
        ///<exception cref="InvalidCastException">
        ///    If the value in the database is null but the type is not nullable
        ///</exception>
        ///<exception cref="IndexOutOfRangeException">
        ///    A column with <paramref name="columnName"/> does not exist in the results.
        ///</exception>
        public static T GetValue<T>(this SqlDataReader dataReader, string columnName)
        {
            object value = dataReader[columnName];
            return ReadValue<T>(value, columnName);
        }

        public static T ReadValue<T>(object value, string columnName = null)
        {
            var genType = typeof(T);

            if (value == DBNull.Value)
            {
                if (Nullable.GetUnderlyingType(genType) == null && genType.IsValueType)
                {
                    if (columnName != null)
                    {
                        throw new InvalidCastException($"Field: {columnName} could not be cast to a type of {genType.Name}");
                    }
                    else
                    {
                        throw new InvalidCastException($"Scalar could not be cast to a type of {genType.Name}");
                    }
                }
                return default;
            }

            //If it is a Nullable ValueType then get the actual value type instead of Nullable ('int' instead of 'int?')
            genType = Nullable.GetUnderlyingType(genType) ?? genType;

            if (genType == typeof(bool))
            {
                value = Convert.ToBoolean(value);
            }
            else if (genType == typeof(double))
            {
                value = Convert.ToDouble(value);
            }
            else if (genType == typeof(decimal))
            {
                value = Convert.ToDecimal(value);
            }
            else if (genType.IsEnum)
            {
                //I have no idea why this works here and not in the last if-else, but it does...
                return (T)((dynamic)Convert.ToInt32(value));
            }
            else if (genType == typeof(DateTime))
            {
                value = Convert.ToDateTime(value);
            }
            else if (genType == typeof(Guid))
            {
                return (T)value;
            }
            else if (genType.BaseType == typeof(ValueType))
            {
                //This will cover all other number types
                return (T)(dynamic)Convert.ToInt32(value);
            }

            return (T)value;
        }

        public static Task<T> GetValueAsync<T>(this SqlDataReader reader, string columnName)
        {
            return Task.Run(() => reader.GetValue<T>(columnName));
        }

        public static bool HasColumn(this IDataRecord dataRecord, string columnName)
        {
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                if (dataRecord.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        ///<summary>
        ///    Reads a list of objects from the dataReader. This function assumes you have already
        ///    checked that the reader has rows and is on the first row.
        ///</summary>
        public static List<T> ReadList<T>(this SqlDataReader dataReader, Func<SqlDataReader, T> readFunction)
        {
            var list = new List<T>();
            do
            {
                list.Add(readFunction(dataReader));
            } while (dataReader.Read());
            return list;
        }

        ///<summary>
        ///    Reads a list of objects from the dataReader. This function assumes you have already
        ///    checked that the reader has rows and is on the first row.
        ///</summary>
        public static async Task<List<T>> ReadListAsync<T>(this SqlDataReader dataReader, Func<SqlDataReader, T> readFunctionAsync)
        {
            var list = new List<T>();
            do
            {
                list.Add(readFunctionAsync(dataReader));
            } while (await dataReader.ReadAsync());
            return list;
        }

        ///<summary>
        ///    Reads a list of objects from the dataReader. This function assumes you have already
        ///    checked that the reader has rows and is on the first row.
        ///</summary>
        public static async Task<List<T>> ReadList<T>(this SqlDataReader dataReader, Func<SqlDataReader, Task<T>> readFunction)
        {
            var list = new List<T>();
            do
            {
                list.Add(await readFunction(dataReader));
            } while (dataReader.Read());
            return list;
        }

        ///<summary>
        ///    Reads a dictionary of objects from the dataReader. This function assumes you have already
        ///    checked that the reader has rows and is on the first row.
        ///</summary>
        public static Dictionary<T, T2> ReadDictionary<T, T2>(this SqlDataReader dataReader, Func<SqlDataReader, (T, T2)> readFunction)
        {
            var dictionary = new Dictionary<T, T2>();
            do
            {
                var (key, value) = readFunction(dataReader);
                dictionary.Add(key, value);
            } while (dataReader.Read());
            return dictionary;
        }
    }
}
