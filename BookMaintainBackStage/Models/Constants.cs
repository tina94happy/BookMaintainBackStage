using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMaintainBackStage.Models
{
    public class Constants
    {
        public static class BookStatus
        {
            /// <summary>
            /// 狀態BOOK_CODE.CODE_ID 
            ///(A可以借出 B以借出 U不可借出)
            /// </summary>
            public const string AVAILABLE = "A";
            public const string IS_BORROWED = "B";
            public const string NOT_AVAILABLE = "U";
        }

        public static class Message
        {
            /// <summary>
            /// 狀態BOOK_CODE.CODE_ID 
            ///(A可以借出 B以借出 U不可借出)
            /// </summary>
            public const string INSERT_SUCCESS = "新增成功";
            public const string INSERT_FAILED = "新增失敗";
            public const string SAVE_SUCCESS = "存檔成功";
            public const string SAVE_FAILED = "存檔失敗";
            public const string DELETE_ERROR_MESSAGE = "書籍狀態為已借出，故不可刪除";
            public const string DELETE_SUCCESS = "刪除成功";
            public const string DELETE_FAILED = "刪除失敗";
        }
    }
}