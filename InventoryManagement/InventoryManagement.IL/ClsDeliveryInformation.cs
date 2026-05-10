using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsDeliveryInformation
    {
        public ClsUtility objUtilitiy = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand sqlCommand;

        public DataSet GetMasters()
        {
            DataSet ds;
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("CALL DEL_INF_GET_MASTERS;");
                ds = objUtilitiy.GetDataSet(sqlQueryBuilder.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        public DataSet GetDeliveryDetails(string entryDate)
        {
            DataSet ds;
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("CALL DEL_INF_GET_DELIVERY_INFORMATION ('@DeliveryDate');");
                sqlQueryBuilder.Replace("@DeliveryDate", entryDate);
                ds = objUtilitiy.GetDataSet(sqlQueryBuilder.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        public int InitiateTransaction(ArrayList arrListDeliveryInfo, ArrayList arrListDeliveryDetail, ArrayList arrListPaymentDetail)
        {
            int TransactionStatus = 0;

            try
            {
                if (arrListDeliveryInfo.Count > 0 && arrListDeliveryDetail.Count > 0 && arrListPaymentDetail.Count > 0)
                {
                    objUtilitiy.BeginTransaction();

                    for (int i = 0; i < arrListDeliveryInfo.Count; i++)
                    {
                        DeliveryMaster objDeliveryMaster = (DeliveryMaster)arrListDeliveryInfo[i];
                        
                        sqlQueryBuilder = new StringBuilder();
                        sqlQueryBuilder.Append("INSERT INTO delivery_inf (Delivery_ID, DeliveryDate, EmployeeID, Total_Amount, ");
                        sqlQueryBuilder.Append("Total_Quantity, CreatedBy, created_at) VALUES (@Delivery_ID, @DeliveryDate, ");
                        sqlQueryBuilder.Append("@EmployeeID, @Total_Amount, @Total_Quantity, @CreatedBy, @created_at);\n");

                        sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                        sqlCommand.Parameters.AddWithValue("@Delivery_ID", "");
                        sqlCommand.Parameters.AddWithValue("@DeliveryDate", objDeliveryMaster.Param_DeliveryDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeID", objDeliveryMaster.Param_EmployeeID);
                        sqlCommand.Parameters.AddWithValue("@Total_Amount", objDeliveryMaster.Param_TotalAmount);
                        sqlCommand.Parameters.AddWithValue("@Total_Quantity", objDeliveryMaster.Param_TotalQuantity);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", objDeliveryMaster.Param_CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@created_at", objDeliveryMaster.Param_created_at);

                        TransactionStatus = objUtilitiy.ExecuteNonQueryTransaction(sqlCommand);
                        sqlCommand.Parameters.Clear();
                        sqlQueryBuilder = new StringBuilder();
                    }

                    for (int i = 0; i < arrListDeliveryDetail.Count; i++)
                    {
                        DeliveryDetails objDeliveryDetails = (DeliveryDetails)arrListDeliveryDetail[i];

                        sqlQueryBuilder = new StringBuilder();
                        sqlQueryBuilder.Append("INSERT INTO delivery_item_details (Delivery_ID, DeliveryDate, EmployeeID, ");
                        sqlQueryBuilder.Append("ProductID, QuantityOutForDelivery, Price, created_at) VALUES (@Delivery_ID, ");
                        sqlQueryBuilder.Append("@DeliveryDate, @EmployeeID, @ProductID, @QuantityOutForDelivery, @Price, @created_at);\n");

                        sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                        sqlCommand.Parameters.AddWithValue("@Detail_ID", "396");
                        sqlCommand.Parameters.AddWithValue("@Delivery_ID", "");
                        sqlCommand.Parameters.AddWithValue("@DeliveryDate", objDeliveryDetails.Param_DeliveryDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeID", objDeliveryDetails.Param_EmployeeID);
                        sqlCommand.Parameters.AddWithValue("@ProductID", objDeliveryDetails.Param_ProductId);
                        sqlCommand.Parameters.AddWithValue("@QuantityOutForDelivery", objDeliveryDetails.Param_Quantity);
                        sqlCommand.Parameters.AddWithValue("@Price", objDeliveryDetails.Param_Price);
                        sqlCommand.Parameters.AddWithValue("@created_at", objDeliveryDetails.Param_created_at);

                        TransactionStatus = objUtilitiy.ExecuteNonQueryTransaction(sqlCommand);
                        sqlCommand.Parameters.Clear();
                    }

                    for (int i = 0; i < arrListPaymentDetail.Count; i++)
                    {
                        PaymentDetails objPaymentDetails = (PaymentDetails)arrListPaymentDetail[i];

                        sqlQueryBuilder = new StringBuilder();
                        sqlQueryBuilder.Append("INSERT INTO delivery_item_payment (Payment_ID, Delivery_ID, DeliveryDate, EmployeeID, ");
                        sqlQueryBuilder.Append("PaymentMode, Amount, created_at) VALUES (@Payment_ID, @Delivery_ID, ");
                        sqlQueryBuilder.Append("@DeliveryDate, @EmployeeID, @PaymentMode, @Amount, @created_at);\n");

                        sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                        sqlCommand.Parameters.AddWithValue("@Payment_ID", "");
                        sqlCommand.Parameters.AddWithValue("@Delivery_ID", "");
                        sqlCommand.Parameters.AddWithValue("@DeliveryDate", objPaymentDetails.Param_DeliveryDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeID", objPaymentDetails.Param_EmployeeID);
                        sqlCommand.Parameters.AddWithValue("@PaymentMode", objPaymentDetails.Param_PaymentMode);
                        sqlCommand.Parameters.AddWithValue("@Amount", objPaymentDetails.Param_Amount);
                        sqlCommand.Parameters.AddWithValue("@created_at", objPaymentDetails.Param_created_at);

                        TransactionStatus = objUtilitiy.ExecuteNonQueryTransaction(sqlCommand);
                        sqlCommand.Parameters.Clear();
                    }

                    objUtilitiy.CommitTransaction();
                    //sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                    //TransactionStatus = objUtilitiy.ExecuteNonQueryTransaction(sqlCommand.CommandText);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return TransactionStatus;
        }
    }

    public class DeliveryMaster
    {
        private string DeliveryId;
        private string DeliveryDate;
        private string EmployeeID;
        private string TotalQuantity;
        private string TotalAmount;
        private string created_at;
        private string updated_at;
        private string CreatedBy;
        private string UpdatedBy;
        private string TransactionType;

        public string Param_DeliveryId
        {
            get { return DeliveryId; }
            set { DeliveryId = value; }
        }

        public string Param_DeliveryDate
        {
            get { return DeliveryDate; }
            set { DeliveryDate = value; }
        }

        public string Param_EmployeeID
        {
            get { return EmployeeID; }
            set { EmployeeID = value; }
        }

        public string Param_TotalQuantity
        {
            get { return TotalQuantity; }
            set { TotalQuantity = value; }
        }

        public string Param_TotalAmount
        {
            get { return TotalAmount; }
            set { TotalAmount = value; }
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
    }

    public class DeliveryDetails
    {
        private string DeliveryId;
        private string DeliveryDate;
        private string EmployeeID;
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

        public string Param_DeliveryId
        {
            get { return DeliveryId; }
            set { DeliveryId = value; }
        }

        public string Param_DeliveryDate
        {
            get { return DeliveryDate; }
            set { DeliveryDate = value; }
        }

        public string Param_EmployeeID
        {
            get { return EmployeeID; }
            set { EmployeeID = value; }
        }

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
    }

    public class PaymentDetails
    {
        private string DeliveryId;
        private string DeliveryDate;
        private string EmployeeID;
        private string PaymentMode;
        private string Amount;
        private string created_at;
        private string updated_at;
        private string CreatedBy;
        private string UpdatedBy;
        private string TransactionType;

        public string Param_DeliveryId
        {
            get { return DeliveryId; }
            set { DeliveryId = value; }
        }

        public string Param_DeliveryDate
        {
            get { return DeliveryDate; }
            set { DeliveryDate = value; }
        }

        public string Param_EmployeeID
        {
            get { return EmployeeID; }
            set { EmployeeID = value; }
        }

        public string Param_PaymentMode
        {
            get { return PaymentMode; }
            set { PaymentMode = value; }
        }

        public string Param_Amount
        {
            get { return Amount; }
            set { Amount = value; }
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
    }
}