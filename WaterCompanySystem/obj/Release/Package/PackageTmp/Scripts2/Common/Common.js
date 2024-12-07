///////here you can find common function
///////wirte by sharif moawia


//Show Feedback Massage Call from Layout Screen
function ShowFeedback(message, type, messageStyle) {
     var massage = "<div class='alert alert-" + messageStyle.toString().toLowerCase() + "'> <a href='#' class='close' data-dismiss='alert'>&times;</a> <strong>" + type + "! </strong> " + message + ". </div>";
     $('#FeedbackMassage').html(massage);

     notifyAlert(message, messageStyle, type);
}

//Hide Feedback
function HideFeedback() {
    $('#FeedbackMassage').html("");
}

function ApplylinksPermissions() {
    // ApplylinksPermissions();
    

    ////////////////////////////////////

    console.log("from common.js : Applying Link Permissions");
    $.ajax({

        url: '/Home/GetPermitedControllersForCurrentUser',
        type: 'GET',
        //data: JSON.stringify(input),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            //  console.log("ajax success links permisoins");
            console.log("permisions are available in: \n" + data);

            var shown = 0;
            var hidden = 0;


            // or starts with fa fa-fw  ,i[class*='fa fa-fw']

            $(".sub li a").each(function (outerIndex, currentIElement) {

                // top li element (module, e.g. setting)
                // var  currentModule = $(currentIElement).parent('li').parent('li');



                // <a> anchor element
                var currentLink = $(currentIElement);//.parent();

                var lnkHref = currentLink.attr("href");

                var fIndexOfSlash = lnkHref.indexOf("/");

                var lastIndexOfSlash = lnkHref.lastIndexOf("/");

                var h = "";

                if ((fIndexOfSlash == -1) & (lastIndexOfSlash == -1)) {
                    h = lnkHref;
                }

                else if ((fIndexOfSlash == lastIndexOfSlash)) {

                    h = lnkHref.substring(0, lastIndexOfSlash);

                    if (h == "") {

                        h = lnkHref.substring(fIndexOfSlash + 1, lnkHref.length);
                    }
                }


                else if ((lastIndexOfSlash != 0)) {

                    h = lnkHref.substring(fIndexOfSlash + 1, lastIndexOfSlash);
                }

                else {

                    h = lnkHref.substring(fIndexOfSlash + 1, lnkHref.length);
                }


                h = h.toLowerCase();

                var cIndex = data.indexOf(',' + h + ',');
                //var cIndex = 0;
                //var tempCIndex = data.indexOf(h+',');

                if (cIndex < 0) {
                    currentLink.parent().hide();
                    //console.log(h + " is not allowed" + " (" + cIndex + ") >> hidden");
                    hidden++;
                }
                else {
                    var fi = cIndex;
                    var li = cIndex + h.length + 1;
                    var c1 = data.charAt(fi);
                    var c2 = data.charAt(li);
                    if (c1 == ',' || c2 == ',') {

                        // currentLink.parent() = inner <li> element
                        //currentLink.parent().show();
                        //currentLink.parent().removeAttr("hidden");
                        currentLink.parent().css("display", "block");

                    }
                    else {
                        // currentLink.parent().hide();
                        //currentLink.parent().attr("hidden", "true");
                        currentLink.parent().css("display", "none");
                    }

                    //console.log(h + " is  allowed" + " (" + cIndex + ") >> shown");
                    shown++;
                }


                var modules = jQuery("#mega-menu-4 > li");


                $(modules).each(function (i, module) {

                    // var visibleLinksCount = $(this).find("ul > li").length;
                    var Links = $(module).find("ul > li");
                    var LinksCount = $(module).find("ul > li").length;
                    var visibleLinksCount = 0;

                    for (var n = 0; n < Links.length; n++) {
                        if ($(Links[n]).css('display') != 'none') {
                            visibleLinksCount++;
                        }
                    }

                    if (visibleLinksCount == 0) {
                        $(this).hide();
                    }

                });

            });


            // var total = shown + hidden;
            //console.log("shown=" + shown + " -- hidden=" + hidden + " | total=" + total);
        },

        error: function (err) {
            //console.log("ajax failure (links permisoins): " + err.val());
        }
    });

     ///////////////////////////////////
}



//**ajax call for fill dropdownlist 
//filterInput Parameter
//Url Post url
//targetId Target element Id

function GetDropDownData(input, url, targetId, defaultValue, withdefaultValue) {

    var target = $('#' + targetId);

    $.ajax({

          url: url,
          type: "POST",
          traditional: true,
          data: JSON.stringify(input),
          dataType: "json",
          contentType: "application/json; charset=utf-8",

          success: function (data) {

              target.empty();

              if (withdefaultValue)

                   target.append("<option value='" + 0 + "'>" + defaultValue + " </option>");

               $(data).each(function () {
                   $(document.createElement('option'))

		    .attr('value', this.Value)
		    .text(this.Text)

		    .appendTo(target);

               });
               return;
          },
          error: function () {
               return;
          }

    });

}


function OpenWindowWithPost(link, params) {
    //written by Ahmed Murtada
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", link);
    form.setAttribute("target", "_blank");
    for (var i in params) {
        if (params.hasOwnProperty(i)) {
            var input = document.createElement('input');
            input.type = 'hidden';
            input.name = i;
            input.value = params[i];
            form.appendChild(input);
        }
    }
    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
}


//Get data for Chosen dropdownlist
function GetChosenDropDownData(input, url, targetId, defaultValue, withdefaultValue) {
     var target = $('#' + targetId);
     $.ajax({
          url: url,
          type: "POST",
          data: JSON.stringify(input),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               target.empty();
               if (withdefaultValue)
                    target.append("<option value='" + 0 + "'>" + defaultValue + " </option>");
               target.trigger("liszt:updated");
               $(data).each(function () {
                    target.append("<option value='" + this.Value + "'>" + this.Text + "</option>");
                    target.trigger("liszt:updated");
               });
               return;
          },
          error: function () {
               return;
          }
     });
}

//Get checkbox data by ajax
function GetCheckBoxData(input, url, targetId) {
     var target = $('#' + targetId);
     $.ajax({
          url: url,
          type: "POST",
          data: JSON.stringify(input),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               target.prop('checked', data);
               return;
          },
          error: function () {
               return;
          }
     });
}



//Get partial view and show in target by targetId
function GetPartialView(input, url, targetId) {
     var target = $('#' + targetId);
     $.ajax({
          url: url,
          type: "POST",
          data: JSON.stringify(input),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               target.html(data);
               $("[Chosen = 'Chosen']").chosen();
               $(".alert alert-info").alert();
               return;
          },
          error: function () {
               return;
          }
     });
}



//Get partial view add show as dialog
function GetPartialViewDialog(input, url, targetId, dialogId) {

    var target = $('#' + targetId);

     $.ajax({
          url: url,
          type: "POST",
          data: JSON.stringify(input),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               target.html(data);
               $('#' + dialogId).modal('show');
               return;
          },
          error: function () {
               return;
          }
     });
}



//Ajax Call and return to callbackfunction
function AjaxCall(input, url, callbackfunction, extraPrem) {
    
     $.ajax({
          url: url,
          type: "POST",
          traditional: true,
          data: JSON.stringify(input),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               window.myFunction = window[callbackfunction](data, extraPrem);
               return;
          },
          error: function () {
               return;
          }
     });
}


//Get Data For Input by ajax
function GetDataForInput(input, url, targetId) {
     var target = $('#' + targetId);
     $.ajax({
          url: url,
          type: "POST",
          data: JSON.stringify(input),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               target.val(data);
               return data;
          },
          error: function () {
               return;
          }
     });
}


///get Datetime by ajax
function GetDateForInput(input, url, targetId) {
     var target = $('#' + targetId);
     $.ajax({
          url: url,
          type: "POST",
          data: JSON.stringify(input),
          dataType: "json",
          contentType: "application/json; charset=utf-8",
          success: function (data) {
               var value = new Date(parseInt(data.replace("/Date(", "").replace(")/", ""), 10));
               target.val($.datepicker.formatDate('dd/mm/yy', new Date(value)));
               return;
          },
          error: function () {
               return;
          }
     });
}


///Renew Record
function ReNewRecord(postbackurl,input, url, lan, dialogId) {
   
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(input),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
           if (data) {
               $('#' + dialogId).modal('hide');
               window.location.reload(postbackurl);
           }
        },
        error: function () {
            return;
        }
    });
}

//Ban Record
function BanRecord(postbackurl, input, url, lan, dialogId) {

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(input),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data) {
                $('#' + dialogId).modal('hide');
                window.location.reload(postbackurl);
            }
        },
        error: function () {
            return;
        }
    });
}

function ShowLocalReport(reportName, data, param, check) {
    if (check) {
        window.open(
            "/ReportView/ViewReportLocal?strReportName=" + reportName + "&RD=" +
            JSON.stringify(data) + "&RP=" + JSON.stringify(param) + "&subName=" + null + "&subReportData=" + null
            , 'mywindow', 'fullscreen=yes, scrollbars=auto'
            );
    } else {
        window.open(reportName, 'mywindow', 'fullscreen=yes, scrollbars=auto');
    }
}


///Show Report in
function ShowReport(input, url, report, lan) {
     $.ajax({
          url: url,
          data: JSON.stringify(input),
          type: 'POST',
          contentType: 'application/json;',
          dataType: 'json',
          success: function (data) {
              if (data == true) {
                  window.open("/ReportView/ViewReport?strReportName=" + report, 'mywindow', 'fullscreen=yes, scrollbars=auto');
              } else {  
                  if (lan == "ar") {
                      ShowFeedback("لا توجد بيانات لعرضها", "خطأ", "Error");
                  }else {
                           ShowFeedback("No Data To show", "Error", "Error");
                  }
                  
               }
          }
     });
}



//show report by id 
function ShowReportByIdWithAction(input, url, report, id) {
     $.ajax({
          url: url,
          data: JSON.stringify(input),
          type: 'POST',
          contentType: 'application/json;',
          dataType: 'json',
          success: function (data) {

              if (data == true) {

                    window.open("/ReportView/ViewReport?strReportName=" + report + "&&id=" + id, 'mywindow', 'fullscreen=yes, scrollbars=auto');
               }
          }
     });
}


function ShowReportById(report, id) {
     window.open("/ReportView/ViewReport?strReportName=" + report + "&&id=" + id, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

//Loading bar in ajax call
//$(document).ready(function () {
//    $.ajaxSetup({
//        'beforeSend': function () {
//            $("#ProgressBar").progressbar({
//                value: false,
//            });
//        },
//        'complete': function () {
//            $("#ProgressBar").progressbar({
//                value: false,
//            });
//        }
//    });
//});

function commonTest() {

    console.log("calling function from common");
}


function CurrentDatePicker(_id,_date){
    ////  AMD.Murtada      ////
    var result = _date.split("/");
    var day = result[0] ;
    var month =result[1]-1 ;
    var year = result[2] ;
    $("#"+_id).datepicker("setDate", new Date(year, month, day ));
}

function digitGrouping(id) {

    ////  AMD.Murtada      ////
    /////////// Enchacing in it ... '//////////

    //digitGrouping => Get id and set output result in the Field

    var vlue = $('#' + id).val();

    var final = startNumberGrouping(vlue);
    
    $('#' + id).val(final);

}


function startNumberGrouping(vlue) {

    //digitGrouping => Get number and output result
    var final = "";
    var temp = "";
    var res = vlue.split(".");
    if (res.length === 1) {
        temp = removeCommas(vlue);
        final = groupingExecute(temp);
    }

    else {
        temp = removeCommas(res[0]);
        final = groupingExecute(temp);
        temp = removeCommas(res[1]);
        final = final + "." + groupingExecute(temp);
    }

    //auth = auth.replace(/,/g, "");

    return final;
}


function removeCommas(str) {

    ////  AMD.Murtada      ////
    //digitGrouping => addintional addon to remove Commas

    while (str.search(",") >= 0) {

        str = (str + "").replace(',', '');

    }

    return str;
}


function groupingExecute(vlue) {

    //digitGrouping => doing the main Operation
    var auth = vlue.toString("{0:##,##.##}");
    var offset = vlue.length % 3;
    if (offset == 0)
        auth = vlue.substring(0, offset) + vlue.substring(offset).replace(/([0-9]{3})(?=[0-9]+)/g, "$1,");
    else
        auth = vlue.substring(0, offset) + vlue.substring(offset).replace(/([0-9]{3})/g, ",$1");

    return auth;
}




function fillDropdown(_id, data, withdefaultValue, defaultValue) {
    ////  AMD.Murtada      ////
    /////////// Modifying in it ... '//////////

    var target = $("#" + _id);
        target.empty();
        if (withdefaultValue)

            target.append("<option value='" + 0 + "'>" + defaultValue + " </option>");

        $(data).each(function () {

            $(document.createElement('option'))

    .attr('value', this.Value)

    .text(this.Text)

    .appendTo(target);

        });

        return;
}