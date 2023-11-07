using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookMaintainBackStage.Service
{
    
    public class CodeService : ICodeService
    {
        /// <summary>
        /// 取得圖書類別 (BOOK_CLASS) sql語法
        /// </summary>
        /// <returns></returns>
        private BookMaintainBackStage.Dao.ICodeDao codeDao { get; set; }

        public List<SelectListItem> GetBookCategoryCodeTable()
        {           
            return codeDao.GetBookCategoryCodeTable();
        }

        /// <summary>
        /// 取得圖書類別 (BOOK_CLASS) sql語法
        /// </summary>
        /// <returns></returns>

        public List<SelectListItem> GetBookNameCodeTable()
        {            
            return codeDao.GetBookNameCodeTable();
        }

        /// <summary>
        /// 取得書籍代碼檔 (BOOK_CODE) sql語法
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookStatusCodeTable()
        {           
            return codeDao.GetBookStatusCodeTable();
        }

        /// <summary>
        /// 取得人員檔 (MEMBER_M) sql語法
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetUserCodeTable()
        {          
            return codeDao.GetUserCodeTable();
        }

        /// <summary>
        /// 取得人員檔 (MEMBER_M) sql語法
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetFullNameCodeTable()
        {        
            return codeDao.GetFullNameCodeTable();
        }

        /// <summary>
        /// 取得codeTable的部分資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCodeTable(string sql, string textCol, string valueCol)
        {            
            return codeDao.GetCodeTable(sql, textCol, valueCol);
        }
    }
}
