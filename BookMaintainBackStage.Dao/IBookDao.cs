using BookMaintainBackStage.Model;
using System.Collections.Generic;

namespace BookMaintainBackStage.Dao
{
    public interface IBookDao
    {
        void CreateBorrowedRecordByBookID(BookEditArg arg);
        string DeleteBookByBookID(string bookID, string bookStatusCode);
        void EditBook(BookEditArg arg);
        void EditBookByBookID(BookEditArg arg);
        List<BookEditArg> GetBooksByCondition(BookSearchArg arg);
        List<BookEditArg> GetBorrowedRecordByBookID(string bookID);
        void InsertBook(BookCreateArg bookInfo);
    }
}