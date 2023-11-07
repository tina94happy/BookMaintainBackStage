using System.Collections.Generic;
using System.Web.Mvc;

namespace BookMaintainBackStage.Service
{
    public interface ICodeService
    {
        List<SelectListItem> GetBookCategoryCodeTable();
        List<SelectListItem> GetBookNameCodeTable();
        List<SelectListItem> GetBookStatusCodeTable();
        List<SelectListItem> GetCodeTable(string sql, string textCol, string valueCol);
        List<SelectListItem> GetFullNameCodeTable();
        List<SelectListItem> GetUserCodeTable();
    }
}