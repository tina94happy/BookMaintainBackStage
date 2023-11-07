using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace BookMaintainBackStage.Models
{
    /// <summary>
    /// 書籍編輯參數
    /// </summary>
    public class BookEditArg : BookCreateArg
    {
        /// 借書狀態ID (A可以借出 B以借出 U不可借出)
        /// </summary>
        [DisplayName("*借閱狀態")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookStatusCode { get; set; }

        /// 借書狀態  (可以借出 以借出 不可借出)
        /// </summary>
        public string BookStatusName { get; set; }

        /// <summary>
        /// 借書者ID
        /// </summary>
        public string KeeperID { get; set; }

        /// <summary>
        /// 借書者全名 = UserEname-UserCname
        /// </summary>
        [DisplayName("借閱人")]
        public string UserFullName { get; set; } //是不是取為KeeperFullName比較好

        /// <summary>
        /// 英文名稱
        /// </summary>
        public string UserEname { get; set; } //是不是取為KeeperEName比較好

        /// <summary>
        /// 中文名稱
        /// </summary>
        public string UserCname { get; set; } //是不是取為KeeperCName比較好

        /// <summary>
        /// 借書時間
        /// </summary>
        public string LendDate { get; set; }
    }
}