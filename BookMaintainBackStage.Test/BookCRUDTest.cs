using BookMaintainBackStage.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookMaintainBackStage.Dao;
using System.Linq;

namespace BookMaintainBackStage.Test
{
    /// <summary>
    /// BookCRUDTest 的摘要說明
    /// </summary>
    [TestClass]
    public class BookCRUDTest
    {
        public string TestBookID;
        public string TestBookStatusCode;
        public BookDao bookDao = new BookDao();

        #region 其他測試屬性
        //
        // 您可以使用下列其他屬性撰寫測試: 
        //
        // 執行該類別中第一項測試前，使用 ClassInitialize 執行程式碼
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在類別中的所有測試執行後，使用 ClassCleanup 執行程式碼
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在執行每一項測試之前，先使用 TestInitialize 執行程式碼 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在執行每一項測試之後，使用 TestCleanup 執行程式碼
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        /// <summary>
        /// 查看是否有查詢到書籍，正向負向都要測
        /// </summary>
        [TestMethod]
        public void TestBookSearch()
        {            
            BookSearchArg arg = new BookSearchArg();
            var bookdata = bookDao.GetBooksByCondition(arg);
            Assert.IsTrue(bookdata.Any());
        }

        [TestInitialize]
        public void TestOnBorrowInit()
        {
            BookCreateArg fakeBookData = new BookCreateArg();
            
            //建立假資料
            fakeBookData.BookStatusCode = BookStatus.IS_BORROWED;
            fakeBookData.BookName = "[勿動]tina測試資料!!";
            fakeBookData.BookAuthor = "[勿動]tina測試資料!!";
            fakeBookData.BookPublisher = "[勿動]tina測試資料!!";
            fakeBookData.BookNote = "[勿動]tina測試資料!!";
            fakeBookData.BookBoughtDate = "2023-03-09";
            fakeBookData.BookClassID = "DB";
            bookDao.InsertBook(fakeBookData);

            //從DB取此假資料
            BookSearchArg arg = new BookSearchArg();
            var bookData = bookDao.GetBooksByCondition(arg)[0];
            TestBookID = bookData.BookID;
            TestBookStatusCode = bookData.BookStatusCode;
        }

        /// <summary>
        /// 書本借閱狀態與可否刪除的關係，若已借出則無法刪除
        /// </summary>
        [TestMethod]
        public void TestDeleteOnBorrow()
        {
            //取得刪除此書返回的message     
            string message = bookDao.DeleteBookByBookID(TestBookID, TestBookStatusCode);
           
            Assert.AreEqual(message, Message.DELETE_ERROR_MESSAGE);
        }

        [TestCleanup]
        public void Clean()
        {
            TestBookID = "";
            TestBookStatusCode = "";
        }
    }
}
