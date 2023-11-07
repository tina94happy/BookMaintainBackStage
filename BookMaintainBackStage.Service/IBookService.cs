using BookMaintainBackStage.Model;
using System;
using System.Collections.Generic;

namespace BookMaintainBackStage.Service
{
    public interface IBookService
    {
        string DeleteBookByBookID(string bookID, string bookStatusCode);
        void EditBook(BookEditArg arg);
        List<BookEditArg> GetBooksByCondition(BookSearchArg arg);
        List<BookEditArg> GetBorrowedRecordByBookID(string bookID);
        void InsertBook(BookCreateArg bookInfo);
    }
}