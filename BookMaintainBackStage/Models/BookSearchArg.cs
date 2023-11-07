using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace BookMaintainBackStage.Models
{
    public class BookSearchArg
    {
        /// <summary>
        /// 類別代號
        /// </summary>
        [DisplayName("圖書類別")]
        public string BookClassID { get; set; }

        /// <summary>
        /// 書籍名稱
        /// </summary>
        [DisplayName("書名")]
        public string BookName { get; set; }

        /// <summary>
        /// 借書狀態 (可以借出 以借出 不可借出)
        /// </summary>
        [DisplayName("借閱狀態代號")]
        public string BookStatusName { get; set; }
        /// <summary>
        /// 借書者
        /// </summary>
        [DisplayName("借閱人")]
        public string KeeperID { get; set; }

        /// 借書狀態ID A,B,U (A可以借出 B以借出 U不可借出)
        /// </summary>
        [DisplayName("借閱狀態")]
        public string BookStatusCode { get; set; }

        /// <summary>
        /// 書籍購書日期
        /// </summary>
        public string BookBoughtDate { get; set; }

        /// <summary>
        /// 英文名稱
        /// </summary>
        public string UserEname { get; set; }

        /// <summary>
        /// 借書者
        /// </summary>
        public string BookID { get; set; }
    }
}