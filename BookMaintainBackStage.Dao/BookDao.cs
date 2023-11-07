using BookMaintainBackStage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BookMaintainBackStage.Dao
{
    public class BookDao : IBookDao
    {
        //TODO:可以只用Model嗎?可是都用using了
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>

        //未來可更動變數
        readonly string createUser = "loginUser"; /*未來登入此後台的操作人員*/
        readonly string bookStatus = BookStatus.AVAILABLE; /*可以借出*/

        private string GetDBConnectionString()
        {
            return BookMaintainBackStage.Common.ConfigTool.GetDBConnectionString("DBConn");
        }

        public void EditBook(Model.BookEditArg arg)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    // 調用兩個public void
                    this.EditBookByBookID(arg);
                    if (arg.BookStatusCode == BookStatus.IS_BORROWED)
                    {
                        this.CreateBorrowedRecordByBookID(arg);
                    }
                    // 如果操作成功，則提交交易
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    BookMaintainBackStage.Common.Logger.Write(BookMaintainBackStage.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                    // 如果有異常發生，交易自動回滾
                    Console.WriteLine(ex.Message);
                }
            }
        }

        ///查詢，與GetBooksByCondition合併
        public List<Model.BookEditArg> GetBooksByCondition(Model.BookSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT
	                        bc.BOOK_CLASS_NAME, bd.BOOK_NAME
                           ,CONVERT(VARCHAR, bd.BOOK_BOUGHT_DATE, 23) AS BOOK_BOUGHT_DATE
                           ,bc1.CODE_NAME, bd.BOOK_STATUS
                           ,mm.USER_ENAME, bc.BOOK_CLASS_ID
                           ,bd.BOOK_ID, mm.[USER_ID]
                           ,bd.BOOK_AUTHOR, bd.BOOK_PUBLISHER, bd.BOOK_NOTE
                           ,CONCAT(USER_ENAME,'-',USER_CNAME) AS FULL_NAME
                        FROM BOOK_DATA bd
                        INNER JOIN BOOK_CLASS bc
	                        ON bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID
                        LEFT JOIN MEMBER_M mm
	                        ON bd.BOOK_KEEPER = mm.[user_id]
                        INNER JOIN BOOK_CODE bc1
	                        ON bd.BOOK_STATUS = bc1.CODE_ID

                        WHERE (bd.BOOK_ID = @BOOK_ID
                        OR @BOOK_ID = '')
                        AND (bc.BOOK_CLASS_ID = @BOOK_CLASS_ID
                        OR @BOOK_CLASS_ID = '')
                        AND (bd.BOOK_STATUS = @BOOK_STATUS
                        OR @BOOK_STATUS = '')
                        AND (mm.[USER_ID] = @USER_ID
                        OR @USER_ID = '')
                        AND (bc1.CODE_TYPE = 'BOOK_STATUS')
                        AND (bd.BOOK_NAME LIKE '%' + @BOOK_NAME + '%')
                        ORDER BY BOOK_BOUGHT_DATE";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", arg.BookClassID == null ? string.Empty : arg.BookClassID));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", arg.BookStatusCode == null ? string.Empty : arg.BookStatusCode));
                cmd.Parameters.Add(new SqlParameter("@USER_ID", arg.KeeperID == null ? string.Empty : arg.KeeperID));
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", arg.BookName == null ? string.Empty : arg.BookName));
                cmd.Parameters.Add(new SqlParameter("@BOOK_ID", arg.BookID == null ? string.Empty : arg.BookID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }


        ///根據bookID查詢借閱紀錄
        public List<Model.BookEditArg> GetBorrowedRecordByBookID(string bookID)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT
	                        CONVERT(VARCHAR, blr.LEND_DATE, 111) AS LEND_DATE
                           ,blr.KEEPER_ID, mm.USER_ENAME, mm.USER_CNAME
                            FROM BOOK_LEND_RECORD blr
                            INNER JOIN MEMBER_M mm
	                            ON mm.[USER_ID] = KEEPER_ID
                            WHERE blr.BOOK_ID = @BOOK_ID
                            ORDER BY LEND_DATE DESC";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_ID", bookID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBorrowedRecordToList(dt);
        }

        /// <summary>
        /// 新增書籍
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns>書籍編號</returns>
        public void InsertBook(Model.BookCreateArg bookInfo)
        {
            try
            {   //動一個table可能不包transaction(depend on team) 可使用ado .net transaction
                // TODO: ado .net transaction
                string sql = @" BEGIN TRY -- + transaction
	                            BEGIN TRANSACTION
		                            INSERT INTO BOOK_DATA (BOOK_NAME, BOOK_AUTHOR , BOOK_PUBLISHER, BOOK_NOTE,
                                                BOOK_BOUGHT_DATE, BOOK_CLASS_ID, BOOK_STATUS,CREATE_DATE,CREATE_USER)
		                            VALUES (@BOOK_NAME, @BOOK_AUTHOR, @BOOK_PUBLISHER,@BOOK_NOTE,
                                            @BOOK_BOUGHT_DATE,@BOOK_CLASS_ID, @BOOK_STATUS, GETDATE(),@CREATE_USER);
	                            COMMIT TRANSACTION
                                END TRY
                                BEGIN CATCH
	                                SELECT ERROR_NUMBER() AS ErrNum, ERROR_MESSAGE() AS ErrMsg;
	                                ROLLBACK TRANSACTION;
                                END CATCH;
						    SELECT SCOPE_IDENTITY()";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", bookInfo.BookName));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", bookInfo.BookAuthor));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", bookInfo.BookPublisher));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", bookInfo.BookNote));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", bookInfo.BookBoughtDate));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", bookInfo.BookClassID));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", bookStatus));   /*可根據需求做更動*/
                    cmd.Parameters.Add(new SqlParameter("@CREATE_USER", createUser));    /*可根據需求做更動*/
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 刪除書籍，並刪除相關借閱紀錄
        /// </summary>
        public string DeleteBookByBookID(string bookID, string bookStatusCode)
        {
            if (bookStatusCode != BookStatus.IS_BORROWED) //function放service or controller
            {
                try
                {
                    string sql = @"BEGIN TRY -- + transaction
                                BEGIN TRANSACTION
                                    DELETE FROM dbo.BOOK_DATA WHERE BOOK_ID = @BOOK_ID
                                    DELETE FROM dbo.BOOK_LEND_RECORD WHERE BOOK_ID = @BOOK_ID
                                COMMIT TRANSACTION
                               END TRY
                               BEGIN CATCH
                                SELECT ERROR_NUMBER() AS ErrNum, ERROR_MESSAGE() AS ErrMsg;
                                ROLLBACK TRANSACTION;
                               END CATCH; ";
                    using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.Add(new SqlParameter("@BOOK_ID", bookID));
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        return Message.DELETE_SUCCESS;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else { return Message.DELETE_ERROR_MESSAGE; }
        } 

        /// <summary>
        /// 新增借閱紀錄
        /// </summary>
        public void CreateBorrowedRecordByBookID(Model.BookEditArg arg)   //改成bookID
        {
            try
            {
                string sql = @"INSERT INTO BOOK_LEND_RECORD (BOOK_ID, KEEPER_ID, LEND_DATE, CRE_DATE, CRE_USR) VALUES
                              (@BOOK_ID, @USER_ID, GETDATE(), GETDATE(), @CRE_USR)";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@BOOK_ID", arg.BookID));
                    cmd.Parameters.Add(new SqlParameter("@USER_ID", arg.KeeperID));
                    cmd.Parameters.Add(new SqlParameter("@CRE_USR", createUser));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改書籍
        /// </summary>
        public void EditBookByBookID(Model.BookEditArg arg)
        {
            try
            {
                string sql = @"UPDATE dbo.BOOK_DATA
	                            SET BOOK_NAME = @BOOK_NAME, BOOK_AUTHOR = @BOOK_AUTHOR
	                               ,BOOK_PUBLISHER = @BOOK_PUBLISHER	                               
	                               ,BOOK_BOUGHT_DATE = @BOOK_BOUGHT_DATE
	                               ,BOOK_CLASS_ID = @BOOK_CLASS_ID
	                               ,BOOK_STATUS = @BOOK_STATUS
	                               ,BOOK_KEEPER = @USER_ID, BOOK_NOTE = @BOOK_NOTE
	                            WHERE BOOK_ID = @BOOK_ID;";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@BOOK_NAME", arg.BookName));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", arg.BookAuthor));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", arg.BookPublisher));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_NOTE", arg.BookNote));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", arg.BookBoughtDate));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", arg.BookClassID));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", arg.BookStatusCode));
                    cmd.Parameters.Add(new SqlParameter("@USER_ID", arg.KeeperID == null ? string.Empty : arg.KeeperID));
                    cmd.Parameters.Add(new SqlParameter("@BOOK_ID", arg.BookID));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Model.BookEditArg> MapBookDataToList(DataTable dt)
        {
            List<Model.BookEditArg> result = new List<BookEditArg>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new BookEditArg()
                {
                    BookClassName = row["BOOK_CLASS_NAME"].ToString(),
                    BookName = row["BOOK_NAME"].ToString(),
                    BookBoughtDate = row["BOOK_BOUGHT_DATE"].ToString(),
                    BookStatusName = row["CODE_NAME"].ToString(),
                    BookStatusCode = row["BOOK_STATUS"].ToString(),
                    UserEname = row["USER_ENAME"].ToString(),
                    BookID = row["BOOK_ID"].ToString(),
                    BookClassID = row["BOOK_CLASS_ID"].ToString(),
                    BookAuthor = row["BOOK_AUTHOR"].ToString(),
                    BookPublisher = row["BOOK_PUBLISHER"].ToString(),
                    BookNote = row["BOOK_NOTE"].ToString(),
                    UserFullName = row["FULL_NAME"].ToString(),
                    KeeperID = row["USER_ID"].ToString(),
                });
            }
            return result;
        }

        private List<Model.BookEditArg> MapBorrowedRecordToList(DataTable dt)
        {
            List<Model.BookEditArg> result = new List<BookEditArg>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new BookEditArg()
                {
                    LendDate = row["LEND_DATE"].ToString(),
                    KeeperID = row["KEEPER_ID"].ToString(),
                    UserEname = row["USER_ENAME"].ToString(),
                    UserCname = row["USER_CNAME"].ToString()
                });
            }
            return result;
        }
    }
}
