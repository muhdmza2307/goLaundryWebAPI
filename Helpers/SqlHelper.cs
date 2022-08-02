using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace goLaundryWebAPI.Helpers
{
    public class SqlHelper
    {
        private string ConnectionString = string.Empty;

        public SqlHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public int SqlExecuteNonQuery(string queryString, SqlParameter[] parameters)
        {
            int affectedRows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = queryString;

                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                command.Parameters.Add(param);
                            }
                        }
                        affectedRows = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SqlExecuteNonQuery Error: " + ex.Message);
            }
            return affectedRows;
        }

        public string SqlExecuteScalar(string queryString, SqlParameter[] parameters, int OutputParamID = 0)
        {
            string result = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = queryString;
                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                command.Parameters.Add(param);
                            }
                        }
                        object strObject = command.ExecuteScalar();
                        if (strObject != null)
                        {
                            result = strObject.ToString();
                        }
                        else if (OutputParamID != 0)
                        {
                            result = command.Parameters[OutputParamID].SqlValue.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SqlExecuteScalar Error: " + ex.Message);
            }
            return result;
        }

        public DataTableReader SqlExecute_DataTableReader(string queryString, SqlParameter[] parameters)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = queryString;

                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                command.Parameters.Add(param);
                            }
                        }
                        SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                        dt.Load(reader);
                        reader.Close();
                        reader.Dispose();
                    }
                    connection.Close();
                }
                DataTableReader dtExecReader = dt.CreateDataReader();
                return dtExecReader;
            }
            catch (Exception ex)
            {
                throw new Exception("SqlExecute_DataTableReader Error: " + ex.Message);
            }
        }

        public DataSet SqlExecute_DataSet(string queryString, SqlParameter[] parameters)
        {
            DataSet ds = null;
            SqlDataAdapter objSqlDataAdapter = null;
            try
            {
                objSqlDataAdapter = new SqlDataAdapter();
                ds = new DataSet();
                using (SqlConnection Connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand Command = Connection.CreateCommand())
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandText = queryString;
                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                Command.Parameters.Add(param);
                            }
                        }
                        objSqlDataAdapter.SelectCommand = Command;
                        objSqlDataAdapter.Fill(ds);
                        Connection.Close();
                        Connection.Dispose();
                        Command.Dispose();
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("SqlExecute_DataSet Error: " + ex.Message);
            }
            finally
            {
                ds = null;
                objSqlDataAdapter.Dispose();
            }
        }

        public DataTable SqlExecute_DataTable(string queryString, SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = queryString;

                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                command.Parameters.Add(param);
                            }
                        }

                        SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                        command.Dispose();
                        dt.Load(reader);

                        reader.Close();
                        reader.Dispose();
                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SqlExecute_DataTable Error: " + ex.Message);
            }
            return dt;
        }
    }
}
