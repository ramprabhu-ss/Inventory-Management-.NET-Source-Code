using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsProductCategoryMaster
    {
        readonly ClsUtility objUtility = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public int CATEGORY_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }

        public DataTable GetProductCategory()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT category_id, category_name FROM product_category ORDER BY category_name");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public int CreateCategory(ClsProductCategoryMaster objCategoryMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO product_category (category_name, created_by, created_at) ");
                sqlQueryBuilder.Append("VALUES (@category_name, @created_by, @created_at)");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@category_name", objCategoryMaster.CATEGORY_NAME ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@created_by", string.IsNullOrWhiteSpace(objCategoryMaster.CREATED_BY) ? DBNull.Value : (object)objCategoryMaster.CREATED_BY);
                objUtility.sqlCommand.Parameters.AddWithValue("@created_at", objCategoryMaster.CREATED_AT.HasValue ? (object)objCategoryMaster.CREATED_AT : DBNull.Value);

                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (Exception)
            {
                objUtility.RollbackTransaction();
                throw;
            }
            return rowsAffected;
        }

        public int UpdateCategory(ClsProductCategoryMaster objCategoryMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE product_category SET category_name = @category_name, ");
                sqlQueryBuilder.Append("updated_by = @updated_by, updated_at = @updated_at WHERE category_id = @category_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@category_id", objCategoryMaster.CATEGORY_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@category_name", objCategoryMaster.CATEGORY_NAME ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@updated_by", string.IsNullOrWhiteSpace(objCategoryMaster.UPDATED_BY) ? DBNull.Value : (object)objCategoryMaster.UPDATED_BY);
                objUtility.sqlCommand.Parameters.AddWithValue("@updated_at", objCategoryMaster.UPDATED_AT.HasValue ? (object)objCategoryMaster.UPDATED_AT : DBNull.Value);

                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (Exception)
            {
                objUtility.RollbackTransaction();
                throw;
            }
            return rowsAffected;
        }

        public int DeleteCategory(ClsProductCategoryMaster objCategoryMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM product_category WHERE category_id = @category_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@category_id", objCategoryMaster.CATEGORY_ID);
                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (Exception)
            {
                objUtility.RollbackTransaction();
                throw;
            }
            return rowsAffected;
        }
    }
}
