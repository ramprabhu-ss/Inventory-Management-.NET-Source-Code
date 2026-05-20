using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace InventoryManagement.IL
{
    public class ClsUtility
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionName"].ToString();
        static MySqlConnection sqlConnection;
        public MySqlCommand sqlCommand = new MySqlCommand();
        static MySqlTransaction sqlTransaction;

        public void OpenConnection()
        {
            try
            {
                sqlConnection = new MySqlConnection(connectionString);
                sqlCommand.Connection = sqlConnection;

                if (sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();

                sqlConnection.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CloseConnection()
        {
            try
            {
                sqlConnection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetDataTable(string sqlQuery)
        {
            DataTable dataTable = new DataTable();

            try
            {
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlQuery, connectionString);
                sqlDataAdapter.Fill(dataTable);
            }
            catch (Exception)
            {
                throw;
            }

            return dataTable;
        }

        public DataSet GetDataSet(string sqlQuery)
        {
            DataSet dataSet = new DataSet();

            try
            {
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlQuery, connectionString);
                sqlDataAdapter.Fill(dataSet);
            }
            catch (Exception)
            {
                throw;
            }

            return dataSet;
        }

        public void ExecuteNonQuery(string sqlQuery)
        {
            try
            {
                OpenConnection();
                sqlCommand.CommandText = sqlQuery;
                sqlCommand.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ExecuteNonQueryTransaction()
        {
            int rowsAffected;

            try
            {
                rowsAffected = sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
            }
            catch (Exception)
            {
                throw;
            }

            return rowsAffected;
        }

        public int ExecuteNonQueryTransaction(string sqlQuery)
        {
            int rowsAffected;

            try
            {
                //OpenConnection();
                sqlCommand.CommandText = sqlQuery;
                rowsAffected = sqlCommand.ExecuteNonQuery();
                //CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }

            return rowsAffected;
        }

        public int ExecuteNonQueryTransaction(MySqlCommand sqlCommandNew)
        {
            int rowsAffected = 0;

            try
            {
                sqlCommand.Parameters.Clear();

                /*
                foreach (MySqlParameter param in sqlCommandNew.Parameters)
                {
                    // The ICloneable interface provides a Clone() method
                    sqlCommand.Parameters.Add(((ICloneable)param).Clone());
                }
                */

                // Clone and copy the parameters to the new command
                var clonedParameters = sqlCommandNew.Parameters.Cast<ICloneable>()
                    .Select(p => p.Clone() as MySqlParameter)
                    .Where(p => p != null)
                    .ToArray();

                sqlCommand.Parameters.AddRange(clonedParameters);
                sqlCommand.CommandText = sqlCommandNew.CommandText;
                rowsAffected = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            return rowsAffected;
        }

        public void BeginTransaction()
        {
            try
            {
                OpenConnection();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTransaction;
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
            }
        }

        public void CommitTransaction()
        {
            try
            {
                sqlTransaction.Commit();
                CloseConnection();
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
                CloseConnection();
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                sqlTransaction.Rollback();
                CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}