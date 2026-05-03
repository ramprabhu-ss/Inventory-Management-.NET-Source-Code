using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsDeliveryInformation
    {
        readonly ClsUtility objUtilitiy = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand sqlCommand;
        int TransactionStatus;

        private string area_id;
        private string area_name;
        private string created_at;
        private string updated_at;
        private string CreatedBy;
        private string UpdatedBy;
        //private string ZIPCODE;

        public string AREA_ID
        {
            get { return area_id; }
            set { area_id = value; }
        }

        public string AREA_NAME
        {
            get { return area_name; }
            set { area_name = value; }
        }

        public string CREATED_AT
        {
            get { return created_at; }
            set { created_at = value; }
        }

        public string UPDATED_AT
        {
            get { return updated_at; }
            set { updated_at = value; }
        }

        public string CREATED_BY
        {
            get { return CreatedBy; }
            set { CreatedBy = value; }
        }

        public string UPDATED_BY
        {
            get { return UpdatedBy; }
            set { UpdatedBy = value; }
        }

        /*public string ZIP_CODE
        {
            get { return ZIPCODE; }
            set { ZIPCODE = value; }
        }*/

        public DataTable GetProducts()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT ProductID, ProductName FROM product ORDER BY ProductName");
                dt = objUtilitiy.GetDataTable(sqlQueryBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable GetDeliveryInformation(string entryDate)
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT IFNULL(DTL.ProductID,'') AS ProductId, IFNULL(INF.Total_Quantity,'') AS Quantity, '' AS Price, ");
                sqlQueryBuilder.Append("IFNULL(INF.Total_Amount,'') AS TotalAmount, '' AS Remarks ");
                sqlQueryBuilder.Append("FROM delivery_inf AS INF LEFT OUTER JOIN delivery_item_details AS DTL ON INF.Delivery_ID = DTL.Delivery_ID ");
                sqlQueryBuilder.Append("WHERE INF.DeliveryDate = '@DeliveryDate';");
                sqlQueryBuilder.Replace("@DeliveryDate", entryDate);
                dt = objUtilitiy.GetDataTable(sqlQueryBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public int CreateArea(ClsAreaMaster objAreaMaster)
        {
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO area_master (area_id, area_name, created_at, ZIPCODE) VALUES ");
                sqlQueryBuilder.Append("(@area_id, @area_name, @created_at, @ZIPCODE);");

                sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                sqlCommand.Parameters.AddWithValue("@area_id", objAreaMaster.AREA_ID);
                sqlCommand.Parameters.AddWithValue("@area_name", objAreaMaster.AREA_NAME);
                sqlCommand.Parameters.AddWithValue("@created_at", objAreaMaster.CREATED_AT);
                //sqlCommand.Parameters.AddWithValue("@ZIPCODE", objAreaMaster.ZIPCODE);

                TransactionStatus = objUtilitiy.ExecuteNonQueryTransaction(sqlCommand);
                return TransactionStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateArea(ClsAreaMaster objAreaMaster)
        {
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE area_master SET area_id = @area_id, area_name = @area_name, updated_at = @updated_at, ");
                sqlQueryBuilder.Append("ZIPCODE = @ZIPCODE WHERE area_id = @area_id");

                sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                sqlCommand.Parameters.AddWithValue("@area_id", objAreaMaster.AREA_ID);
                sqlCommand.Parameters.AddWithValue("@area_name", objAreaMaster.AREA_NAME);
                sqlCommand.Parameters.AddWithValue("@updated_at", objAreaMaster.UPDATED_AT);
                //sqlCommand.Parameters.AddWithValue("@ZIPCODE", objAreaMaster.ZIPCODE);

                TransactionStatus = objUtilitiy.ExecuteNonQueryTransaction(sqlCommand);
                return TransactionStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteArea(ClsAreaMaster objAreaMaster)
        {
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM area_master WHERE area_id = @area_id");

                sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                sqlCommand.Parameters.AddWithValue("@area_id", objAreaMaster.AREA_ID);

                TransactionStatus = objUtilitiy.ExecuteNonQueryTransaction(sqlCommand);
                return TransactionStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}