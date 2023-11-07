using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMaintainBackStage.Service
{
    public class BookService : IBookService
    {
        private BookMaintainBackStage.Dao.IBookDao bookDao { get; set; }
        public void EditBook(Model.BookEditArg arg)
        {           
            bookDao.EditBook(arg);
        }
        public List<Model.BookEditArg> GetBooksByCondition(Model.BookSearchArg arg)
        {          
            return bookDao.GetBooksByCondition(arg);
        }
        public List<Model.BookEditArg> GetBorrowedRecordByBookID(string bookID)
        {            
            return bookDao.GetBorrowedRecordByBookID(bookID);
        }
        public void InsertBook(Model.BookCreateArg bookInfo)
        {           
            bookDao.InsertBook(bookInfo);
        }
        public string DeleteBookByBookID(string bookID, string bookStatusCode)
        {            
            return bookDao.DeleteBookByBookID(bookID, bookStatusCode);
        }
    }
}