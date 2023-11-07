using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMaintainBackStage.Model
{
    /// <summary>
    /// 書籍編輯參數
    /// </summary>
    public class BookEditArg : BookCreateArg
    {
        

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
