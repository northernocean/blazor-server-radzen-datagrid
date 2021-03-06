﻿window.baselib = {
    alert: function (msg) {
        alert(msg);
    },
    selectedIndex: function (id, index) {
        document.getElementById(id).selectedIndex = index;
    },
    hideModal: function (selector) {
        var x = $(selector).modal('hide');
    },
    showModal: function (selector) {
        var x = $(selector).modal('show');
    },
    scrollTextArea: function () {
        const textarea = document.getElementById("mock-console");
        console.log(textarea);
        textarea.scrollTop = textarea.scrollHeight;
    },
    setScrollTextArea: function () {
        setTimeout(baselib.scrollTextArea, 20);
    }
}


window.scrapeTable = function (selector) {
    var arrData = [];
    $("#ships-grid tbody tr").each(function () {
        var currentRow = $(this);

        var col1_value = currentRow.find("td:eq(2) span.ui-cell-data").text();
        var col2_value = currentRow.find("td:eq(3)").text();
        var col3_value = currentRow.find("td:eq(4)").text();
        try {
            var obj = {};
            obj.col1 = col1_value;
            obj.col2 = col2_value.substring(4);
            obj.col3 = col3_value.substring(8);
            arrData.push(obj);
        }
        catch { }
    });
    console.log(arrData);
}
