function initDropDownList(selector, actionName, onChange) {
    $(selector).kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    url: "/Book/" + actionName,
                    dataType: "json",
                    type: "post"
                }
            }
        },
        change: function (e) {
            e.preventDefault();
            if (onChange != undefined) {
                onChange();
            }
            
        },
        optionLabel: "請選擇"
    });
};

function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
    return false;
};