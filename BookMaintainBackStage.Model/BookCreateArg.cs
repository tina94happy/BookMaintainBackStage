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
    /// 書籍新增參數
    /// </summary>
    public class BookCreateArg
    {
        /// <summary>
        /// 書籍名稱
        /// </summary>
        [DisplayName("*書名")]
        [Required(ErrorMessage = "此欄位必填")]
        [StringLength(200, ErrorMessage = "此欄位僅接受200個字")]
        public string BookName { get; set; }

        /// <summary>
        /// BOOK_AUTHOR
        /// </summary>
        [DisplayName("*作者")]
        [Required(ErrorMessage = "此欄位必填")]
        [StringLength(30, ErrorMessage = "此欄位僅接受30個字")]
        public string BookAuthor { get; set; }

        /// <summary>
        /// 出版商
        /// </summary>
        [DisplayName("*出版商")]
        [Required(ErrorMessage = "此欄位必填")]
        [StringLength(20, ErrorMessage = "此欄位僅接受20個字")]
        public string BookPublisher { get; set; }

        /// <summary>
        /// 內容簡介
        /// </summary>
        [DisplayName("*內容簡介")]
        [Required(ErrorMessage = "此欄位必填")]
        [StringLength(1200, ErrorMessage = "此欄位僅接受1200個字")]
        public string BookNote { get; set; }

        /// <summary>
        /// 書籍購書日期
        /// </summary>
        [DisplayName("*購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookBoughtDate { get; set; }

        /// <summary>
        /// 類別名稱
        /// </summary>
        /// 
        [DisplayName("*圖書類別")]
        public string BookClassName { get; set; }

        /// <summary>
        /// 類別代號
        /// </summary>
        [DisplayName("類別代號")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookClassID { get; set; }

        /// <summary>
        /// 書籍ID
        /// </summary>
        public string BookID { get; set; }

        /// 借書狀態ID (A可以借出 B以借出 U不可借出)
        /// </summary>
        [DisplayName("*借閱狀態")]
        public string BookStatusCode { get; set; }
    }
}
