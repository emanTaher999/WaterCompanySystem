//Written By a.talha
//Validation && Ajax Functions
//date 24/03/2019


//validate year
function yearValidation(year, ev) {

    var text = /^[0-9]+$/;
    if (ev.type == "blur" || year.length == 4 && ev.keyCode != 8 && ev.keyCode != 46) {
        if (year != 0) {
            if ((year != "") && (!text.test(year))) {

                alert("Please Enter Numeric Values Only");
                return false;
            }

            if (year.length != 4) {
                alert("Year is not proper. Please check");
                return false;
            }
            var current_year = new Date().getFullYear();
            if ((year < 1920) || (year > current_year)) {
                alert("Year should be in range 1920 to current year");
                return false;
            }
            return true;
        }
    }
}
//Check Mobile No is Valid
function CheckMobileNo(mobile, errorMsg, errorTitle) {
    var num = mobile.val();
    if (!$.isNumeric(num) || num.length !== 10) {


        toastr.error(errorMsg, errorTitle);

        mobile.val('');
        return;
    }
    var first = num.startsWith("01");
    var second = num.startsWith("09");

    if (first === false && second === false) {
        toastr.error(errorMsg,
            errorTitle);
        mobile.val('');
    }


}
//get File Extension
function getExtension(filename) {
    var parts = filename.split('.');
    return parts[parts.length - 1];
}
//Check File is Image
function isImage(filename) {
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'jpg':
            return true;
    }
    return false;
}
//Check File is PDF
function isPdf(filename) {
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'pdf':
            return true;
    }
    return false;
}
//Validate Email
function IsEmail(email) {
    var regex =
        /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
    return regex.test(email);
}
//Check Email is valid
function CheckEmail(emailObj, errorMsg, errorTitle) {
    var email = emailObj.val();
    if (!IsEmail(email)) {
        emailObj.val('');
        toastr.error(errorMsg, errorTitle);
    }
}

//Check input is Alphapet
//call on key press
function CheckAlphapet(e, errorMsg, errorTitle) {
    var regex = new RegExp("^[0-9]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(str)) {
        return true;
    } else {
        e.preventDefault();
        toastr.error(errorMsg, errorTitle);
        return false;
    }
}
//check input is number
//call on key press
function CheckisNumber(e, errorMsg, errorTitle) {
    var regex = new RegExp("^[0-9]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    } else {
        e.preventDefault();
        toastr.error(errorMsg, errorTitle);
        return false;
    }
}

//Check Date is valid
function CheckDate(dateObj, errorMsg, errorTitle) {
    var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    var Val_date = dateObj.val();
    if (Val_date.match(dateformat)) {
        var seperator1 = Val_date.split('/');
        var seperator2 = Val_date.split('-');

        if (seperator1.length > 1) {
            var splitdate = Val_date.split('/');
        } else if (seperator2.length > 1) {
            var splitdate = Val_date.split('-');
        }
        var dd = parseInt(splitdate[0]);
        var mm = parseInt(splitdate[1]);
        var yy = parseInt(splitdate[2]);
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (mm == 1 || mm > 2) {
            if (dd > ListofDays[mm - 1]) {
                toastr.error(errorMsg, errorTitle);
                dateObj.val('');
                return false;
            }
        }
        if (mm == 2) {
            var lyear = false;
            if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                lyear = true;
            }
            if ((lyear == false) && (dd >= 29)) {

                toastr.error(errorMsg, errorTitle);
                dateObj.val('');
                return false;
            }
            if ((lyear == true) && (dd > 29)) {
                toastr.error(errorMsg, errorTitle);
                dateObj.val('');
                return false;
            }
        }
    } else {
        toastr.error(errorMsg, errorTitle);
        dateObj.val('');
        return false;
    }
}

//Check is AplhaNumeric 
function isAlphaNumeric(str) {
    var code, i, len;

    for (i = 0, len = str.length; i < len; i++) {
        code = str.charCodeAt(i);
        if (!(code > 47 && code < 58) && // numeric (0-9)
            !(code > 64 && code < 91) && // upper alpha (A-Z)
            !(code > 96 && code < 123)) { // lower alpha (a-z)
            return false;
        }
    }
    return true;
};

//parse returned date from json to view
function parseJsonDate(jsonDate) {
    if (jsonDate !== null) {
       // var offset = new Date().getTimezoneOffset() * 60000;
        var parts = /\/Date\((-?\d+)([+-]\d{2})?(\d{2})?.*/.exec(jsonDate);
        if (parts[2] == undefined) parts[2] = 0;
        if (parts[3] == undefined) parts[3] = 0;
        //   d = new Date(+parts[1] + offset + parts[2] * 3600000 + parts[3] * 60000);
           d = new Date(+parts[1] + parts[2] * 3600000 + parts[3] * 60000);

        date = d.getDate() ;
        date = date < 10 ? "0" + date : date;
        mon = d.getMonth() + 1;
        mon = mon < 10 ? "0" + mon : mon;
        year = d.getFullYear();
        return (date + "/" + mon + "/" + year);
    } else {
        return ("");
    }
};

//Ajax Post
function AjaxPost(input, url) {
    var result = "";
    $.ajax({
        url: url,
        type: "POST",
        traditional: true,
        async: false,
        data: JSON.stringify(input),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            result = data;
        },
        error: function () {
           
        }
    });
    return result;
}
//Ajax Async Post
function AjaxAsyncPost(input, url) {
    var result = "";
    $.ajax({
        url: url,
        async: true,
        type: "POST",
        traditional: true,
        data: JSON.stringify(input),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            result = data;
        },
        error: function () {
           
        }
    });
    return result;
}

//Ajax Get
function AjaxGet(url) {
    var result = "";
    $.ajax({
        url: url,
        type: "GET",
        async:false,
        traditional: true,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            result = data;
        },
        error: function () {
           
        }
    });
    return result;
}
//Ajax Async Post
function AjaxAsyncGet(url) {
    var result = "";
    $.ajax({
        url: url,
        async: true,
        type: "GET",
        traditional: true,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            result = data;
        },
        error: function () {
           
        }
    });
    return result;
}
function handleResponse(data) {
    return data;
}

function FillChosen(chosenddl, defaulttext,data) {
    chosenddl.empty();

    chosenddl.append("<option value=''>" + defaulttext + " </option>");
    RefreshChosen(chosenddl);
    $(data).each(function () {
        chosenddl.append("<option value='" + this.Value + "'>" + this.Text + "</option>");
        RefreshChosen(chosenddl);
    });
}

function FillDropDown(ddl, defaulttext, data) {
    ddl.empty();
   
    ddl.append("<option value=''>" + defaulttext + " </option>");
    $(data).each(function () {
        $(document.createElement('option'))
            .attr('value', this.Value)
            .text(this.Text)
            .appendTo(ddl);
    });
}

function RefreshChosen(chosenddl) {
    chosenddl.trigger('chosen:updated');
}
//Show Toaster Message
function ShowMessage(message, title) {
    toastr.error(message, title);
}

