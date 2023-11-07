using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookMaintainBackStage.Dao
{
    public class MockCodeDao :ICodeDao
    {
        private string rootCodeDataFilePath = @"C:\Users\tina_wt_su\source\workshop_要上傳的資料\Course6_BackEnd\BookMaintainBackStage\BookMaintainBackStage\App_Data\MockData\";

        public List<SelectListItem> GetBookCategoryCodeTable()
        {
            string bookClassFilePath = rootCodeDataFilePath + "BOOK_CLASS.txt";

            var lines = File.ReadAllLines(bookClassFilePath);
            List<SelectListItem> result = new List<SelectListItem>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                result.Add(new SelectListItem()
                {
                    Text = item.Split(splitChar.ToCharArray())[1],
                    Value = item.Split(splitChar.ToCharArray())[0]
                });
            }
            return result;
        }

        public List<SelectListItem> GetBookNameCodeTable()
        {
            throw new NotImplementedException();
        }


        public List<SelectListItem> GetBookStatusCodeTable()
        {
            string bookCodeFilePath = rootCodeDataFilePath + "BOOK_CODE.txt";

            var lines = File.ReadAllLines(bookCodeFilePath);
            List<SelectListItem> result = new List<SelectListItem>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                //TODO:內有BOOK_STATUS與BLOOD_TYPE，目前只取BOOK_STATUS
                if (item.Split(splitChar.ToCharArray())[0] == "BOOK_STATUS")
                {
                    result.Add(new SelectListItem()
                    {
                        Text = item.Split(splitChar.ToCharArray())[3],
                        Value = item.Split(splitChar.ToCharArray())[1]
                    });
                }

            }
            return result;
        }

        public List<SelectListItem> GetCodeTable(string sql, string textCol, string valueCol)
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetFullNameCodeTable()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetUserCodeTable()
        {
            string memborFilePath = rootCodeDataFilePath + "MEMBER_M.txt";

            var lines = File.ReadAllLines(memborFilePath);
            List<SelectListItem> result = new List<SelectListItem>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                result.Add(new SelectListItem()
                {
                    Text = item.Split(splitChar.ToCharArray())[1],
                    Value = item.Split(splitChar.ToCharArray())[0]
                });
            }
            return result;
        }
    }
}
