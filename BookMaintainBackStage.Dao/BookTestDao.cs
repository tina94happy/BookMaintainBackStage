using BookMaintainBackStage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMaintainBackStage.Dao
{
    public class BookTestDao :IBookDao
    {
        public void CreateBorrowedRecordByBookID(BookEditArg arg)
        {
            throw new NotImplementedException();
        }

        public void EditBook(BookEditArg arg)
        {
            throw new NotImplementedException();
        }

        public void EditBookByBookID(BookEditArg arg)
        {
            throw new NotImplementedException();
        }

        public List<BookEditArg> GetBooksByCondition(BookSearchArg arg)
        {
            var result = new List<BookEditArg>();

            result.Add(new BookEditArg
            {
                BookClassName= "Banking",
                BookName = "CORPORATE",
                BookBoughtDate = "2017/01/08",
                BookStatusName = "已借出",
                UserFullName = "Andy",
            });
            result.Add(new BookEditArg
            {
                BookClassName = "Banking",
                BookName = "Financial Services",
                BookBoughtDate = "2017/01/08",
                BookStatusName = "已借出",
                UserFullName = "Andy",
            });
            return result;
        }

        public void InsertBook(BookCreateArg bookInfo)
        {
            throw new NotImplementedException();
        }

        string IBookDao.DeleteBookByBookID(string bookID, string bookStatusCode)
        {
            throw new NotImplementedException();
        }

        List<BookEditArg> IBookDao.GetBorrowedRecordByBookID(string bookID)
        {
            throw new NotImplementedException();
        }



    }
}
