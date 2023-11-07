using BookMaintainBackStage.Model;//using BMB.Service
using BookMaintainBackStage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookMaintainBackStage.Controllers
{
    public class BookController : Controller
    {
        /// <summary>
        /// 在這個 Controller 類別中，包含了以下幾個 Action 方法：
        //Index：用來查詢顯示所有書籍的列表。
        //Create：用來新增書籍的頁面。
        //Delete：用來刪除書籍的頁面。
        //Edit：用來編輯書籍的頁面。
        //Detail:用來顯示書籍明細的頁面。
        //BorrowedRecord：用來查看指定書籍的借閱紀錄。
        /// </summary>

        public ICodeService codeService { get; set; }
        public IBookService bookService { get; set; }

        [HttpGet()]
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 取得書籍類別資料
        /// </summary>
        [HttpPost]
        public JsonResult GetBookCategoryData()
        {
            var result = this.codeService.GetBookCategoryCodeTable();
            return Json(result);
        }


        /// <summary>
        /// 取得借閱人資料
        /// </summary>
        [HttpPost]
        public JsonResult GetKeeperData()
        {
            var result = this.codeService.GetUserCodeTable();
            return Json(result);
        }


        /// <summary>
        /// 取得書籍狀態資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBookStatusData()
        {//TODO??
            var result = this.codeService.GetBookStatusCodeTable();
            return Json(result);
        }

        /// <summary>
        /// 取得書籍名稱
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBookNameData()
        {
            var result = this.codeService.GetBookNameCodeTable();
            return Json(result);
        }

        /// <summary>
        /// 查詢書籍
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchBook(Model.BookSearchArg arg)
        {
            var result = this.bookService.GetBooksByCondition(arg);
            return Json(result);
        }

        /// <summary>
        /// 新增書籍頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 新增書籍
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult Create(Model.BookCreateArg arg)
        {
            try
            {
                // 格式驗證
                if (ModelState.IsValid)
                {
                    //新增書籍
                    bookService.InsertBook(arg);
                    return Json(new { success = true});
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刪除書籍
        /// </summary>
        [HttpPost()]
        public JsonResult Delete(Model.BookSearchArg arg)
        {
            var bookData = bookService.GetBooksByCondition(arg)[0];
            try
            {
                string message = bookService.DeleteBookByBookID(arg.BookID, bookData.BookStatusCode);
                if (message == Message.DELETE_SUCCESS)
                {
                    return Json(new { status = true, message = message });
                }
                else
                {
                    return Json(new { status = false, message = message });
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 編輯頁面
        /// </summary>
        [HttpGet()]
        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 取得編輯頁面預設資料
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetBookEditInfo(Model.BookSearchArg arg)
        {
            try
            {
                if (arg.BookID != null)
                {
                    TempData["BookID"] = arg.BookID;
                    var bookData = bookService.GetBooksByCondition(arg)[0];
                    return Json(bookData);
                }
                throw new Exception("缺少BookID");
            }
            catch (Exception ex)
            {
                BookMaintainBackStage.Common.Logger.Write(BookMaintainBackStage.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 修改書籍資訊
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult Edit(Model.BookEditArg arg)
        {
            //TODO:尚未寫若已借出則不可借閱欄位為空
            if ((arg.BookStatusCode==BookStatus.IS_BORROWED || arg.BookStatusCode == BookStatus.IS_BORROWED_2)  && arg.KeeperID == null)
            {
                return Json(new { status = false, message = "若為已借出，則借閱人不可為空" });
            }
            try
            {
                if (ModelState.IsValid)
                {
                    bookService.EditBook(arg);
                    return Json(new { status = true, message = Message.SAVE_SUCCESS });
                }
                return Json(new { status = false, message = Message.SAVE_FAILED });
            }
            catch (Exception ex)
            {
                BookMaintainBackStage.Common.Logger.Write(BookMaintainBackStage.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 書籍明細畫面顯示
        /// </summary>
        [HttpGet()]
        public ActionResult Detail()
        {
            return View("Edit");
        }


        /// <summary>
        /// 借閱紀錄畫面
        /// </summary>
        [HttpGet()]
        public ActionResult BorrowedRecord()
        {
            return View();
        }


        /// <summary>
        /// 取借閱紀錄
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult getBorrowedRecord(string BookID)
        {
            var BorrowedRecordResult = bookService.GetBorrowedRecordByBookID(BookID);
            return Json(BorrowedRecordResult);
        }

    }
}
//TODO: 注意foreach