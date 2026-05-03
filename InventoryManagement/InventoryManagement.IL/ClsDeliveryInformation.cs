using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsDeliveryInformation
    {
        readonly ClsUtility objUtilitiy = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand sqlCommand;

        private string ProductId;
        private string Quantity;
        private string Price;
        private string TotalAmount;
        private string Remarks;
        private string created_at;
        private string updated_at;
        private string CreatedBy;
        private string UpdatedBy;
        private string TransactionType;

        public string Param_ProductId
        {
            get { return ProductId; }
            set { ProductId = value; }
        }

        public string Param_Quantity
        {
            get { return Quantity; }
            set { Quantity = value; }
        }

        public string Param_Price
        {
            get { return Price; }
            set { Price = value; }
        }
        public string Param_TotalAmount
        {
            get { return TotalAmount; }
            set { TotalAmount = value; }
        }
        public string Param_Remarks
        {
            get { return Remarks; }
            set { Remarks = value; }
        }
        public string Param_created_at
        {
            get { return created_at; }
            set { created_at = value; }
        }

        public string Param_updated_at
        {
            get { return updated_at; }
            set { updated_at = value; }
        }

        public string Param_CreatedBy
        {
            get { return CreatedBy; }
            set { CreatedBy = value; }
        }

        public string Param_UpdatedBy
        {
            get { return UpdatedBy; }
            set { UpdatedBy = value; }
        }

        public string Param_TransactionType
        {
            get { return TransactionType; }
            set { TransactionType = value; }
        }

        public DataTable GetProducts()
        {
            DataTable dt;
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("CALL SP_GET_PRODUCTS;");
                dt = objUtilitiy.GetDataTable(sqlQueryBuilder.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public DataTable GetDeliveryDetails(string entryDate)
        {
            DataTable dt;
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("CALL SP_GET_DELIVERY_DETAILS ('@DeliveryDate');");
                sqlQueryBuilder.Replace("@DeliveryDate", entryDate);
                dt = objUtilitiy.GetDataTable(sqlQueryBuilder.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public int InitiateTransaction(ArrayList arrListDeliveryInfo, ArrayList arrListDeliveryDetail)
        {
            int TransactionStatus = 0;

            try
            {
                if (arrListDeliveryInfo.Count > 0 && arrListDeliveryDetail.Count > 0)
                {
                    sqlQueryBuilder = new StringBuilder();

                    /*for (int i = 0; i < arrListDeliveryInfo.Count; i++)
                    {
                        sqlQueryBuilder.Append("INSERT INTO delivery_inf (Delivery_ID, DeliveryDate, EmployeeID, Total_Amount, ");
                        sqlQueryBuilder.Append("Total_Quantity, CreatedBy, created_at) VALUES (");
                        sqlQueryBuilder.Append(arrListDeliveryInfo[i].ToString());
                        sqlQueryBuilder.Append(");\n");
                    }*/

                    for (int i = 0; i < arrListDeliveryDetail.Count; i++)
                    {
                        sqlQueryBuilder.Append("INSERT INTO delivery_item_details (Detail_ID, Delivery_ID, DeliveryDate, EmployeeID, ");
                        sqlQueryBuilder.Append("ProductID, created_at) VALUES (");
                        sqlQueryBuilder.Append(arrListDeliveryDetail[i].ToString());
                        sqlQueryBuilder.Append(")\n;");
                    }

                    sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                    TransactionStatus = objUtilitiy.ExecuteNonQueryTransaction(sqlCommand);
                }

                /*if (objDeliveryInformation.Param_TransactionType == "INSERT")
                {
                    sqlQueryBuilder = new StringBuilder();
                    sqlQueryBuilder.Append("INSERT INTO delivery_inf (area_id, area_name, created_at, ZIPCODE) VALUES ");
                    sqlQueryBuilder.Append("(@area_id, @area_name, @created_at, @ZIPCODE);");

                    sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                    sqlCommand.Parameters.AddWithValue("@area_id", objDeliveryInformation.Param_ProductId);
                    sqlCommand.Parameters.AddWithValue("@area_name", objDeliveryInformation.Param_Quantity);
                    sqlCommand.Parameters.AddWithValue("@created_at", objDeliveryInformation.Param_Price);
                    sqlCommand.Parameters.AddWithValue("@ZIPCODE", objDeliveryInformation.Param_TotalAmount);
                    sqlCommand.Parameters.AddWithValue("@ZIPCODE", objDeliveryInformation.Param_Remarks);
                    sqlCommand.Parameters.AddWithValue("@ZIPCODE", objDeliveryInformation.Param_CreatedBy);
                    sqlCommand.Parameters.AddWithValue("@ZIPCODE", objDeliveryInformation.Param_created_at);
                }
                else if (objDeliveryInformation.Param_TransactionType == "UPDATE")
                {
                }*/
            }
            catch (Exception)
            {
                throw;
            }

            return TransactionStatus;
        }
    }
}