

$(document).ready(function () {
    //url 參數取得
    const BookID = getUrlParameter("BookID");
    const Edit = getUrlParameter("Edit");
    //驗證規則設定
    validator = $("form").kendoValidator({
        rules: {
            // 特定id需參照dateValid進行驗證
            dateValid: function (input) {
                if (input.is("[id='BookBoughtDate']") && input.val() != "") {
                    return $('#BookBoughtDate').data('kendoDatePicker').value() ? true : false
                }
                return true;
            },
            maxlength: function (input) {
                if (input.is("[maxlength]")) {
                    var maxLength = parseInt(input.attr("maxlength"));
                    if (input.val().length > maxLength) {
                        return false;
                    }
                }
                return true;
            }
        },
        messages: { //custom rules messages
            dateValid: "日期格式錯誤",
            required: "此為必填欄位",
            maxlength: "此欄位最多只能輸入 {0} 個字"  //TODO: 這樣需要改name=20 很怪
        }
    }).data("kendoValidator");
    //借閱狀態 
    //TODO 放funciton
    //借閱狀態
    var onBookStatusEChange = function () {

        if ($("#bookStatus_E").data("kendoDropDownList").value() == bookStatus.IS_BORROWED ||
            $("#bookStatus_E").data("kendoDropDownList").value() == bookStatus.IS_BORROWED_2) {
            $("#keeperSelector").data("kendoDropDownList").enable(true);
        }
        else {
            $("#keeperSelector").data("kendoDropDownList").value("");
            $("#keeperSelector").data("kendoDropDownList").enable(false);
        }

    }

    //kendoDatePicker初始化
    function kendoDatePickerInit() {
        //kendo日期格式
        $("#BookBoughtDate").kendoDatePicker({
            format: "yyyy/MM/dd"
        });
    };
    kendoDatePickerInit();

    //取得下拉式選單
    function getDropdownList() {
        //圖書類別
        initDropDownList("#bookClassSelector", "GetBookCategoryData");
        //借閱人
        initDropDownList("#keeperSelector", "GetKeeperData");
        //借閱狀態
        initDropDownList("#bookStatus_E", "GetBookStatusData", onBookStatusEChange);
    };   
    getDropdownList();

    

    //書本預設資料取得
    $.ajax({
        type: "POST",
        url: "/Book/GetBookEditInfo",
        data: {
            BookID: BookID
        },
        dataType: "json",
        success: function (res) {
            if (res) {
                $('#BookName').val(res.BookName);
                $("#BookAuthor").val(res.BookAuthor);
                $("#BookPublisher").val(res.BookPublisher);
                $("#BookNote").val(res.BookNote);
                $("#BookBoughtDate").data("kendoDatePicker").value(res.BookBoughtDate);
                $("#bookClassSelector").data("kendoDropDownList").value(res.BookClassID);
                $("#bookStatus_E").data("kendoDropDownList").value(res.BookStatusCode);
                $("#keeperSelector").data("kendoDropDownList").value(res.KeeperID);
                if (res.BookStatusCode == bookStatus.AVAILABLE || res.BookStatusCode == bookStatus.NOT_AVAILABLE) {
                    $("#keeperSelector").data("kendoDropDownList").enable(false);
                }
            }
        }, error: function (error) {
            alert("系統發生錯誤");
        }
    });
         
    //若是明細頁則將編輯頁所有input disabled
    if (Edit != "true") {
        $("#submitBtn").css("display", "none");
        $("#btnDelete").css("display", "none");
        $("#keeperSelector").data("kendoDropDownList").enable(false);
        $("#bookStatus_E").data("kendoDropDownList").enable(false);
        $("#bookClassSelector").data("kendoDropDownList").enable(false);
        $("title").text("Detail");
        $('input').attr('disabled', true);
        $('textarea').attr('disabled', true);
        $('select').attr('disabled', true);
    }
    //[存檔]btn
    $("#submitBtn").click(function (e) {
        e.preventDefault();
        //儲存前先檢查表單內容
        if (validator.validate()) {
            $.ajax({
                type: "POST",
                url: "/Book/Edit",
                data: {
                    BookID: BookID,
                    BookName: $('#BookName').val(),
                    BookAuthor: $("#BookAuthor").val(),
                    BookPublisher: $("#BookPublisher").val(),
                    BookNote: $("#BookNote").val(),
                    BookBoughtDate: kendo.toString($("#BookBoughtDate").data("kendoDatePicker").value(), "yyyy-MM-dd"), //TODO:
                    BookClassID: $("#bookClassSelector").data("kendoDropDownList").value(),
                    BookStatusCode: $("#bookStatus_E").data("kendoDropDownList").value(),
                    KeeperID: $("#keeperSelector").data("kendoDropDownList").value()
                },
                dataType: "json",
                success: function (res) {
                    alert(res.message);
                }, error: function (error) {
                    alert(message.SYSTEM_ERROR);
                }
            });
        }
    });

    //[刪除]btn
    $("#btnDelete").click(function (e) {
        e.preventDefault();
        if (confirm("是否刪除書籍?")) {
            $.ajax({
                type: "POST",
                url: "/Book/Delete",
                data: {
                    bookID: BookID
                },
                dataType: "json",
                success: function (result) {
                    if (result.status) {
                        alert(result.message);
                        window.location.href = '/Book/Index';
                    } else {
                        alert(result.message);
                    }
                }, error: function (error) {
                    alert(error.responseText);
                }
            });
        }
    });


});

