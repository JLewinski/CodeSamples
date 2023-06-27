using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSamples.Extensions.DataAccess;
using CodeSamples.Exceptions;

namespace CodeSamples.Helpers.DataAccess
{
    public static class StoredProcedureHelper
    {
        const int DefaultTimeout = 30;

        //----------------------EXECUTE----------------------
        public static int ExecuteStoredProcedure(string storedProcedureName, object parameters = null, int timeout = DefaultTimeout)
        {
            int rowsModified = 0;
            try
            {
                using (var conn = new SqlConnection(Global.ConnectionString))
                using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
                {
                    command.CommandTimeout = timeout;
                    conn.Open();
                    rowsModified = command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                throw new SampleDbException(e, storedProcedureName, parameters);
            }
            return rowsModified;
        }

        public static int ExecuteStoredProcedure(string storedProcedureName, object parameters, SqlConnection conn, int timeout = DefaultTimeout)
        {
            int rowsModified = 0;
            try
            {
                using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
                {
                    command.CommandTimeout = timeout;
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    rowsModified = command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                throw new SampleDbException(e, storedProcedureName, parameters);
            }
            return rowsModified;
        }

        public static Task ExecuteStoredProcedureAsync(string storedProcedureName, object parameters = null, SqlConnection conn = null)
        {
            return Task.Run(() =>
            {
                ExecuteStoredProcedure(storedProcedureName, parameters, conn);
            });
        }
        //----------------------GET LIST----------------------
        public static List<T> GetList<T>(string storedProcedureName, Func<SqlDataReader, T> readFunction, SqlConnection conn) => GetList(storedProcedureName, null, readFunction, DefaultTimeout, conn);

        /// <inheritdoc cref="GetList{T}(string, object, Func{IDataReader, T}, int)"/>
        public static List<T> GetList<T>(string storedProcedureName, Func<SqlDataReader, T> readFunction) => GetList(storedProcedureName, null, readFunction, DefaultTimeout);

        /// <inheritdoc cref="GetList{T}(string, object, Func{IDataReader, T}, int)"/>
        public static List<T> GetList<T>(string storedProcedureName, Func<SqlDataReader, T> readFunction, int timeout) => GetList(storedProcedureName, null, readFunction, timeout);

        /// <inheritdoc cref="GetList{T}(string, object, Func{IDataReader, T}, int)"/>
        public static List<T> GetList<T>(string storedProcedureName, object parameters, Func<SqlDataReader, T> readFunction) => GetList(storedProcedureName, parameters, readFunction, DefaultTimeout);

        private static List<T> GetList<T>(string storedProcedureName, object parameters, Func<SqlDataReader, T> readFunction, int timeout, SqlConnection conn)
        {
            var list = new List<T>();
            using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
            {
                command.CommandTimeout = timeout;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        return dataReader.ReadList(readFunction);
                    }
                    else
                    {
                        ElmahErrorLogging.LogNotFound(storedProcedureName, parameters);
                    }
                }
            }
            return list;
        }

        /// <summary>
        ///     Executes the stored procedure <paramref name="storedProcedureName"/> with <paramref name="parameters"/>
        ///     and reads it with <paramref name="readFunction"/>. The <paramref name="timeout"/> is read after the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="readFunction">Function to read each item in the list</param>
        public static List<T> GetList<T>(string storedProcedureName, object parameters, Func<SqlDataReader, T> readFunction, int timeout)
        {
            var list = new List<T>();
            try
            {
                using (var conn = new SqlConnection(Global.ConnectionString))
                {
                    list = GetList(storedProcedureName, parameters, readFunction, timeout, conn);
                }
            }
            catch (SqlException e)
            {
                throw new SampleDbException(e, storedProcedureName, parameters);
            }
            return list;
        }

        //----------------------GET LIST WITH COUNT----------------------
        /// <inheritdoc cref="GetList{T}(string, object, Func{IDataReader, T}, int, out int)"/>
        public static List<T> GetList<T>(string storedProcedureName, Func<SqlDataReader, T> readFunction, out int itemCount) => GetList(storedProcedureName, readFunction, DefaultTimeout, out itemCount);

        /// <inheritdoc cref="GetList{T}(string, object, Func{IDataReader, T}, int, out int)"/>
        public static List<T> GetList<T>(string storedProcedureName, Func<SqlDataReader, T> readFunction, int timeout, out int itemCount) => GetList(storedProcedureName, null, readFunction, timeout, out itemCount);

        /// <inheritdoc cref="GetList{T}(string, object, Func{IDataReader, T}, int, out int)"/>
		public static List<T> GetList<T>(string storedProcedureName, object parameters, Func<SqlDataReader, T> readFunction, out int itemCount) => GetList(storedProcedureName, parameters, readFunction, DefaultTimeout, out itemCount);

        /// <summary>
        ///     Executes the stored procedure <paramref name="storedProcedureName"/> with <paramref name="parameters"/>
        ///     and reads it with <paramref name="readFunction"/>. The <paramref name="itemCount"/> is read before the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="readFunction">Function to read each item in the list</param>
        /// <param name="itemCount">Read after the list</param>
        public static List<T> GetList<T>(string storedProcedureName, object parameters, Func<SqlDataReader, T> readFunction, int timeout, out int itemCount)
        {
            var list = new List<T>();
            try
            {
                using (var conn = new SqlConnection(Global.ConnectionString))
                using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
                {
                    conn.Open();
                    command.CommandTimeout = timeout;
                    using (var dataReader = command.ExecuteReader())
                    {
                        dataReader.Read();
                        itemCount = Convert.ToInt32(dataReader[0]);
                        if (dataReader.NextResult() && dataReader.Read())
                        {
                            list = dataReader.ReadList(readFunction);
                        }
                        else
                        {
                            ElmahErrorLogging.LogNotFound(storedProcedureName, parameters);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new SampleDbException(e, storedProcedureName, parameters);
            }
            return list;
        }

        public static async Task<List<T>> GetListAsync<T>(string storedProcedureName, object parameters, Func<SqlDataReader, T> readFunction)
        {
            var list = new List<T>();
            try
            {
                using (var conn = new SqlConnection(Global.ConnectionString))
                using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
                {
                    conn.Open();
                    using (var dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            list = await dataReader.ReadListAsync(readFunction);
                        }
                        else
                        {
                            //TODO: I think it may be better to throw an exception here, but that would break a lot of code
                            ElmahErrorLogging.LogNotFound(storedProcedureName, parameters);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new SampleDbException(e, storedProcedureName, parameters);
            }
            return list;
        }

        public static Task<(List<T> list, int count)> GetListFromStoredProcedureWithCountAsync<T>(string storedProcedureName, object parameters, Func<SqlDataReader, Task<T>> readFunction)
        {
            return Task.Run(async () =>
            {
                var list = new List<T>();
                int itemCount;
                try
                {
                    using (var conn = new SqlConnection(Global.ConnectionString))
                    using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
                    {
                        conn.Open();
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                list.Add(await readFunction(dataReader));
                            }
                            itemCount = dataReader.NextResult() && dataReader.Read() ? (int)dataReader[0] : list.Count;
                        }
                    }
                }
                catch (SqlException e)
                {
                    throw new SampleDbException(e, storedProcedureName, parameters);
                }
                return (list, itemCount);
            });
        }

        //----------------------GET DICTIONARY----------------------
        public static Dictionary<T, T2> GetDictionary<T, T2>(string storedProcedureName, Func<SqlDataReader, (T, T2)> readFunction) => GetDictionary(storedProcedureName, null, readFunction);
        public static Dictionary<T, T2> GetDictionary<T, T2>(string storedProcedureName, object parameters, Func<SqlDataReader, (T, T2)> readFunction)
        {
            Dictionary<T, T2> dictionary;
            try
            {
                using (var conn = new SqlConnection(Global.ConnectionString))
                using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
                {
                    conn.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            dictionary = dataReader.ReadDictionary(readFunction);
                        }
                        else
                        {
                            ElmahErrorLogging.LogNotFound(storedProcedureName, parameters);
                            dictionary = new Dictionary<T, T2>();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new SampleDbException(e, storedProcedureName, parameters);
            }

            return dictionary;
        }

        //----------------------GET ITEM----------------------
        public static T GetItem<T>(string storedProcedureName, Func<SqlDataReader, T> readFunction, int timeout = DefaultTimeout) => GetItem(storedProcedureName, null, readFunction, timeout);
        public static T GetItem<T>(string storedProcedureName, object parameters, Func<SqlDataReader, T> readFunction, int timeout = DefaultTimeout) => GetItem(storedProcedureName, parameters, readFunction, null, timeout);
        public static T GetItem<T>(string storedProcedureName, object parameters, Func<SqlDataReader, T> readFunction, SqlConnection conn, int timeout = DefaultTimeout)
        {
            T returnValue = default(T);
            if (conn == null)
            {
                T obj;
                using (conn = new SqlConnection(Global.ConnectionString))
                {
                    obj = GetItem(storedProcedureName, parameters, readFunction, conn);
                }
                return obj;
            }
            try
            {
                using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
                {
                    command.CommandTimeout = timeout;
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            returnValue = readFunction(dataReader);
                        }
                        else if (default(T) == null)
                        {
                            ElmahErrorLogging.LogNotFound(storedProcedureName, parameters);
                        }
                        else
                        {
                            throw new NoNullAllowedException($"The '{storedProcedureName}' stored procedure returned 0 rows");
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new SampleDbException(e, storedProcedureName, parameters);
            }
            return returnValue;
        }

        public static Task<T> GetItemAsync<T>(string storedProcedureName, Func<SqlDataReader, T> readFunction, SqlConnection conn = null) => GetItemAsync(storedProcedureName, null, readFunction, conn);
        public static Task<T> GetItemAsync<T>(string storedProcedureName, object parameters, Func<SqlDataReader, T> readFunction, SqlConnection conn = null)
        {
            return Task.Run(() =>
            {
                return GetItem(storedProcedureName, parameters, readFunction, conn);
            });
        }

        public static Task<T> GetItemAsync<T>(string storedProcedureName, object parameters, Func<SqlDataReader, Task<T>> readFunction)
        {
            return Task.Run(async () =>
            {
                try
                {
                    using (var conn = new SqlConnection(Global.ConnectionString))
                    using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
                    {
                        conn.Open();
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                return await readFunction(dataReader);
                            }
                            else
                            {
                                throw new NoNullAllowedException($"The '{storedProcedureName}' stored procedure returned 0 rows");
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    throw new SampleDbException(e, storedProcedureName, parameters);
                }
            });
        }

        //----------------------GET SCALAR----------------------

        public static T GetScalar<T>(string storedProcedureName, int timeout = DefaultTimeout) => GetScalar<T>(storedProcedureName, null, timeout);
        public static T GetScalar<T>(string storedProcedureName, object parameters, int timeout = DefaultTimeout) => GetScalar<T>(storedProcedureName, parameters, null, timeout);
        public static T GetScalar<T>(string storedProcedureName, object parameters, SqlConnection conn, int timeout = DefaultTimeout)
        {
            T obj;
            try
            {
                if (conn == null)
                {
                    using (conn = new SqlConnection(Global.ConnectionString))
                    {
                        obj = GetScalar<T>(storedProcedureName, parameters, conn);
                    }
                }
                else
                {
                    using (var command = conn.CreateStoredProcedure(storedProcedureName, parameters))
                    {
                        command.CommandTimeout = timeout;
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        obj = DataReaderExtensions.ReadValue<T>(command.ExecuteScalar());
                    }
                }
            }
            catch (SqlException e)
            {
                throw new SampleDbException(e, storedProcedureName, parameters);
            }
            return obj;
        }

        public static Task<T> GetScalarAsync<T>(string storedProcedureName) => GetScalarAsync<T>(storedProcedureName, null);
        public static Task<T> GetScalarAsync<T>(string storedProcedureName, object parameters)
        {
            return Task.Run(() =>
            {
                return GetScalar<T>(storedProcedureName, parameters);
            });
        }
    }
}
