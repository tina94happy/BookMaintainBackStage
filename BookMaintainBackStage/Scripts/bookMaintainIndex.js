$(document).ready(function () {
    //TODO:把js拉出去
    eventBind();
    initKendoComponent();

    //相關按鈕
    function eventBind() {
        //[查詢]btn
        $("#bookSearchBtn").click(function (e) {
            e.preventDefault();
            $("#grid").data("kendoGrid").dataSource.read();
        });
        //[清除]btn (搜尋列)
        $("#bookSearchResetBtn").click(function (e) {
            e.preventDefault();
            //TODO: 是否有更好的做法
            //TODO: .data('kendoDropDownList')改成function
            $("#BookNameSearch").data('kendoAutoComplete').value("");
            $("#bookClassSelector").data('kendoDropDownList').value("");
            $("#keeperSelector").data('kendoDropDownList').value("");
            $("#bookStatusSelector").data('kendoDropDownList').value("");
            //按下清除的同時grid會取全部data
            $("#grid").data("kendoGrid").dataSource.read();
        });
        
    };
    
    function initKendoComponent() {
        //書名 kendoAutoComplete
        $("#BookNameSearch").kendoAutoComplete({
            placeholder: "請輸入書名",
            dataTextField: "Text",
            filter: "contains",
            dataSource: {
                transport: {
                    read: {
                        url: "/Book/GetBookNameData",
                        dataType: "json",
                        type: "post"
                    }
                }
            }
        });
        
        //圖書類別
        initDropDownList("#bookClassSelector", "GetBookCategoryData");
        //借閱人
        initDropDownList("#keeperSelector", "GetKeeperData");
        //書籍借閱狀態
        initDropDownList("#bookStatusSelector", "GetBookStatusData")
    };

    //[刪除]btn
    function deleteBook(e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var data = this.dataItem(tr);
        if (confirm("是否刪除書籍?")) {
            $.ajax({
                type: "POST",
                url: "/Book/Delete",
                data: {
                    bookID: data['BookID']
                },
                dataType: "json",
                success: function (result) {
                    if (result.status) {
                        $(tr).remove();
                        alert(result.message);
                    } else {
                        alert(result.message);
                    }
                }, error: function (error) {
                    alert(error.responseText);
                }
            });
            //deleteBook(data.BookID); //TODO:忘了當初要幹麻
        }
    };
    //[編輯]btn
    function modifyBook(e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var data = this.dataItem(tr);
        window.location.href = "/Book/Edit?BookID=" + data['BookID'] + '&Edit=true';
    };
    //[借閱紀錄]btn
    function getBookRecord(e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var data = this.dataItem(tr);
        window.location.href = "/Book/BorrowedRecord?BookID=" + data['BookID'];
    };

    //kendo grid configuration
    $("#grid").kendoGrid({
        dataSource: {
            transport: {
                read: {
                    url: "/Book/SearchBook",
                    type: "post",
                    dataType: "json",
                    data: function () {
                        return {
                            //查詢過濾條件
                            //名稱對應到MODEL的屬性
                            BookClassID: $("#bookClassSelector").data("kendoDropDownList").value(),
                            KeeperID: $("#keeperSelector").data("kendoDropDownList").value(),
                            BookStatusCode: $("#bookStatusSelector").data("kendoDropDownList").value(),
                            BookName: $("#BookNameSearch").data("kendoAutoComplete").value()
                        }
                    }
                }
            },
            pageSize: 20
        },
        height: 550,
        scrollable: true,
        sortable: true,
        filterable: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [
            { field: "BookClassName", title: "圖書類別", width: "15%" },
            {
                field: "BookName", title: "書名", width: "38%",
                template: '<a href="\\#" class ="bookDetailUrl fake-link" ><span class="fake-link">#= BookName#</span></a>'
            },
            { field: "BookBoughtDate", title: "購書日期", width: "auto" },
            { field: "BookStatusName", title: "借閱狀態", width: "auto" },
            { field: "UserFullName", title: "借閱人", width: "auto" },
            { hidden: true, field: "BookID" },
            {
                command: [
                    { text: "借閱紀錄", click: getBookRecord },
                    { text: "編輯", click: modifyBook },
                    { text: "刪除", click: deleteBook }
                ], width: "19%"
            }
        ],
        editable: {
            mode: "inline", // mode can be incell/inline/popup with Q1 '12 Beta Release of Kendo UI
            confirmation: false // the confirmation message for destroy command
        },
        //TODO: 可刪editable: "inline" 這個設定將啟用行內編輯模式，使用戶可以直接在 Grid 中編輯和更新行。
    });

    //點擊超連結前往書籍明細
    $("#grid").on("click", ".bookDetailUrl", function (e) {
        // 在這裡寫下要執行的程式碼
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        // 取得該資料列的 dataItem
        var data = $("#grid").data("kendoGrid").dataItem(tr);
     
        window.location.href = "/Book/Detail?BookID=" + data['BookID'];
    });




});

