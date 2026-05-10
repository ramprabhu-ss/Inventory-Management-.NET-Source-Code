using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace InventoryManagement.IL
{
    public class ClsUtility
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionName"].ToString();
        static MySqlConnection sqlConnection;
        static MySqlCommand sqlCommand = new MySqlCommand();
        static MySqlTransaction sqlTransaction;

        public void OpenConnection()
        {
            try
            {
                sqlConnection = new MySqlConnection(connectionString);

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

        public int ExecuteNonQueryTransaction(string sqlQuery)
        {
            int TransactionStatus;

            try
            {
                OpenConnection();
                sqlCommand.CommandText = sqlQuery;
                TransactionStatus = sqlCommand.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }

            return TransactionStatus;
        }

        public int ExecuteNonQueryTransaction(MySqlCommand sqlCommandNew)
        {
            int TransactionStatus = 0;

            try
            {
                sqlCommand.CommandText = sqlCommandNew.CommandText;
                sqlCommand.Parameters.Clear();
                foreach (MySqlParameter param in sqlCommandNew.Parameters)
                {
                    // The ICloneable interface provides a Clone() method
                    sqlCommand.Parameters.Add(((ICloneable)param).Clone());
                }
                TransactionStatus = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                //throw;
            }
            return TransactionStatus;
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
    }
}