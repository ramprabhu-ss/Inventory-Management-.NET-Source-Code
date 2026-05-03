using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Configuration;

namespace InventoryManagement.IL
{
    public class ClsUtility
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionName"].ToString();
        public MySqlConnection sqlConnection;

        public void OpenConnection()
        {
            try
            {
                sqlConnection = new MySqlConnection(connectionString);

                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }

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
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
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
                OpenConnection();
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlQuery, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                CloseConnection();
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
                OpenConnection();
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlQuery, sqlConnection);
                sqlDataAdapter.Fill(dataSet);
                CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }

            return dataSet;
        }

        public void ExecuteNonQuery(MySqlCommand sqlCommand)
        {
            try
            {
                OpenConnection();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ExecuteNonQueryTransaction(MySqlCommand sqlCommand)
        {
            int TransactionStatus;

            try
            {
                OpenConnection();
                sqlCommand.Connection = sqlConnection;
                TransactionStatus = sqlCommand.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }

            return TransactionStatus;
        }
    }
}