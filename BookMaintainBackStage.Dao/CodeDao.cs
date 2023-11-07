using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookMaintainBackStage.Dao
{
    public class CodeDao : ICodeDao
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return BookMaintainBackStage.Common.ConfigTool.GetDBConnectionString("DBConn");
        }

        /// <summary>
        /// 取得圖書類別 (BOOK_CLASS) sql語法
        /// </summary>
        /// <returns></returns>

        public List<SelectListItem> GetBookCategoryCodeTable()
        {
            string BookCategorySQL = @"SELECT BOOK_CLASS_NAME, BOOK_CLASS_ID FROM dbo.BOOK_CLASS";
            return this.GetCodeTable(BookCategorySQL, "BOOK_CLASS_NAME", "BOOK_CLASS_ID");  //還要看columnName
        }

        /// <summary>
        /// 取得圖書類別 (BOOK_CLASS) sql語法
        /// </summary>
        /// <returns></returns>

        public List<SelectListItem> GetBookNameCodeTable()
        {
            string BookCategorySQL = @"SELECT BOOK_NAME, BOOK_ID FROM dbo.BOOK_DATA";
            return this.GetCodeTable(BookCategorySQL, "BOOK_NAME", "BOOK_ID");  //還要看columnName
        }

        /// <summary>
        /// 取得書籍代碼檔 (BOOK_CODE) sql語法
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookStatusCodeTable()
        {
            string BookStatusSQL = @"SELECT CODE_NAME,CODE_ID FROM dbo.BOOK_CODE AS bc WHERE bc.CODE_TYPE='BOOK_STATUS'";
            return this.GetCodeTable(BookStatusSQL, "CODE_NAME", "CODE_ID");
        }

        /// <summary>
        /// 取得人員檔 (MEMBER_M) sql語法
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetUserCodeTable()
        {
            string UserCodeTableSQL = @"SELECT USER_ENAME, USER_ID FROM dbo.MEMBER_M";
            return this.GetCodeTable(UserCodeTableSQL, "USER_ENAME", "USER_ID");
        }

        /// <summary>
        /// 取得人員檔 (MEMBER_M) sql語法
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetFullNameCodeTable()
        {
            string UserCodeTableSQL = @"SELECT CONCAT(USER_ENAME,'-',USER_CNAME) AS FULL_NAME, USER_ID FROM dbo.MEMBER_M";
            return this.GetCodeTable(UserCodeTableSQL, "FULL_NAME", "USER_ID");
        }

        /// <summary>
        /// 取得codeTable的部分資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCodeTable(string sql, string textCol, string valueCol)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt, textCol, valueCol);

        }
        /// <summary>
        /// Maping 代碼資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapCodeData(DataTable dt, string textCol, string valueCol)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row[textCol].ToString(), //id name repeated
                    Value = row[valueCol].ToString()
                });
            }
            return result;
        }
    }
}
