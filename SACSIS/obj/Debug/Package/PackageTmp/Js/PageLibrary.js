/// <reference name="MicrosoftAjax.js"/>


function PageOutPut(outString) {
    /// <summary>
    /// 页面输出转换
    /// </summary>	
    /// <param name="outString">输出字符</param>
    var strText, strTemp;
    strText = /&/g;
    strTemp = outString.replace(strText, "&#38;");
    strText = />/g;
    strTemp = strTemp.replace(strText, "&#62;");
    strText = /</g;
    strTemp = strTemp.replace(strText, "&#60;");
    return strTemp;
}

function PageParameterGet(name, urlSearch) {
    /// <summary>读取页面url参数</summary>
    /// <param name="name">参数名</param>
    /// <param name="urlSearch">可以指定分析符合url.search部分字符串格式的参数列表，如果空缺则分析window.location.search</param>
    /// <returns>参数值</returns>
    var search;
    if (urlSearch != null) {
        search = urlSearch;
    } else {
        search = window.location.search;
    }
    if (search.substr(0, 1) == "?") {
        search = search.substr(1);
    }
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = search.match(reg);
    if (r != null)
        return unescape(r[2]);
    else
        return null;
}

function GetElementsByAttribute(tagName, tagAttribute, attributeValue) {
    /// <summary>
    /// 根据属性值获取指定类型的html节点对象
    /// </summary>	
    /// <param name="tagName">节点标签名</param>
    /// <param name="tagAttribute">要检查的属性名</param>
    /// <param name="attributeValue">要检查的属性值，如果为空，则只检查属性存在</param>
    var arrObj = new Array();
    var objs = document.getElementsByTagName(tagName);
    for (var i = 0; i < objs.length; i++) {
        if (objs[i][tagAttribute] != null) {
            if (attributeValue != null) {
                if (objs[i][tagAttribute] == attributeValue) {
                    arrObj.push(objs[i]);
                }
            }
            else {
                arrObj.push(objs[i]);
            }
        }
    }
    return arrObj;
}

function PageCheckBoxSelectAll(source, targetName) {
    /// <summary>根据点击checkbox状态设置页面指定name的checkbox的选择状态</summary>
    /// <param name="source">点击的checkbox的ID或对象</param>
    /// <param name="targetname">需要设置的checkboax name</param>
    /// <returns></returns>
    var sourceObj;

    if (typeof (source) == "object")
        sourceObj = source;
    else
        sourceObj = $get(source);

    var boolChecked = sourceObj.checked;
    var chkobj = document.getElementsByName(targetName);
    for (var i = 0; i < chkobj.length; i++) {
        chkobj[i].checked = boolChecked;
    }
}
function PageCheckBoxValuesGet(name) {
    /// <summary>获取选中的checkbox value</summary>
    /// <param name="name">checkboax name</param>
    /// <returns>value 序列，","分隔</returns>
    var strSelectedValue = "";
    var chkobj = document.getElementsByName(name);
    for (var i = 0; i < chkobj.length; i++) {
        if (chkobj[i].checked) {
            strSelectedValue += chkobj[i].value + ",";
        }
    }
    if (strSelectedValue.length > 0)
        strSelectedValue = strSelectedValue.substring(0, strSelectedValue.length - 1);
    return strSelectedValue;
}
function PageRadioSelectedValue(name) {
    /// <summary>获取选中的radio value</summary>
    /// <param name="name">radio name</param>
    /// <returns>radio value</returns>
    var strSelectedValue = "";
    var chkobj = document.getElementsByName(name);
    for (var i = 0; i < chkobj.length; i++) {
        if (chkobj[i].checked) {
            strSelectedValue = chkobj[i].value;
            break;
        }
    }
    return strSelectedValue;
}

function PageSelectItemExists(selID, keyValue) {
    /// <summary>判断下拉框是否存在指定值</summary>	
    /// <param name="selID">下拉框ID</param>
    /// <param name="keyValue">option值</param>
    /// <returns>bool</returns>  
    var objSel = $get(selID);
    var isExisted = false;
    for (var i = 0; i < objSel.options.length; i++) {
        if (objSel.options[i].value == keyValue) {
            isExisted = true;
            break;
        }
    }
    return isExisted;
}
function PageSelectOptionsRemoveAll(selID) {
    /// <summary>移除所有下拉选项</summary>	
    /// <param name="selID">下拉框ID</param>
    /// <returns></returns> 
    var objSel = $get(selID);
    for (var j = objSel.length - 1; j >= 0; j--) {
        nIndex = j;
        objSel.remove(nIndex);
    }
}
function PageSelectOptionsRemoveSelected(selID) {
    /// <summary>移除选中的下拉选项</summary>	
    /// <param name="selID">下拉框ID</param>
    /// <returns></returns> 
    var objOption, nIndex;
    var objSel = $get(selID);
    if (objSel.length == 0) {
        //alert("请选择要删除的项目！");
    }
    else {
        if (objSel.selectedIndex == -1) {
            //alert("请选择要删除的项目！");
        }
        else {
            for (var j = objSel.length - 1; j >= 0; j--) {
                if (objSel.options(j).selected) {
                    nIndex = j;
                    objSel.remove(nIndex);
                }
            }
        }
    }
}

function PageProgressShow() {
    /// <summary>显示进度条</summary>
    /// <returns></returns>
    if (document.getElementById("divKYMask") == null) {
        var strHTML = '<div id="divKYMask" style="position:absolute; left:0; top:0;width:100%;height:100%;z-index:100000">';
        strHTML += '<iframe id="fraKYMask" style="position:absolute;filter:alpha(opacity=40);z-index:0;" src="about:blank"  width="100%" height="100%" frameborder="no"></iframe>';
        strHTML += '<div style="width:214px;border:2px dotted green;padding-top:3px;height:40px;left:50%; top:50%; margin-left:-107px; margin-top:-20px;padding-left:8px;position:absolute;middle;z-index:1">';
        //strHTML += '<img src="/images/loading.gif" />';
        strHTML += '<span class="progressimg"></span>';
        strHTML += '<span style="font-size:14px;position:relative;top:-10px"> 数据处理中，请稍候......</span></div>';
        strHTML += '<div id style="width:100%;height:100%;position:absolute;z-index:1"></div>';
        strHTML += '</div>';
        document.body.insertAdjacentHTML("afterBegin", strHTML);

        document.getElementById("fraKYMask").contentWindow.document.bgColor = "#f9f9f9";
    }
    else {
        document.getElementById("divKYMask").style.display = "";
    }
    //document.getElementById("divKYMask").style.cursor = "wait";

    try {
        document.getElementById("divKYMask").focus();
    }
    catch (e) { }
}
function PageProgressHide() {
    /// <summary>隐藏进度条</summary>
    /// <returns></returns>
    if (document.getElementById("divKYMask")) {
        document.getElementById("divKYMask").style.display = "none";
    }
}

function PageCookieGet(name) {
    /// <summary>读取COOKIE值</summary>
    /// <param name="name">COOKIE名称</param>
    /// <returns>读取COOKIE值</returns>
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function PageCookieCreate(name, value, days) {
    /// <summary>创建COOKIE</summary>
    /// <param name="name">COOKIE名称</param>
    /// <param name="value">值</param>
    /// <param name="days">保存天数</param>
    /// <returns></returns>
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}

function PageModalDialogShow(url) {
    /// <summary>弹开模式窗体</summary>
    /// <param name="url">路径</param>
    /// <returns></returns>   
    return window.showModalDialog(url, window, "scroll:0;AddressBar:0;resizable:0;help:no;status:1;");
}
function PageModelessDialogShow(url) {
    /// <summary>弹开非模式窗体</summary>
    /// <param name="url">路径</param>
    /// <returns></returns>
    return window.showModelessDialog(url, window, "scroll:0;AddressBar:0;resizable:0;help:no;status:1;");
}
function PageDialogSet(dialogWidth, dialogHeight) {
    /// <summary>设置对话框高宽</summary>
    /// <param name="dialogWidth">宽度(整型)</param>
    /// <param name="dialogHeight">高度(整型)</param>
    /// <returns></returns>
    
    //var iTop = (window.screen.availHeight - 30 - dialogHeight) / 2;       //获得窗口的垂直位置;
    //var iLeft = (window.screen.availWidth - 10 - dialogWidth) / 2;           //获得窗口的水平位置;
    
    var userAgent = window.clientInformation.userAgent;
    var version = 7;
    if ((i = userAgent.indexOf("MSIE")) >= 0) {
        version = parseFloat(userAgent.substr(i + 4));
    }
    if (version >= 7) {
        window.dialogHeight = dialogHeight + "px";
        window.dialogWidth = dialogWidth + "px";
    }
    else {//IE6及以下
        window.dialogHeight = parseInt(dialogHeight, 10) + 49 + "px";
        window.dialogWidth = parseInt(dialogWidth, 10) + 6 + "px";
    }
//    alert(iTop + "," + iLeft);
//    window.offsetLeft = iLeft;
//    window.offsetTop = iTop;
}

function DateAdd(dateValue, interval, value) {
    ///<summary>日期加减</summary> 
    ///<param name="dateValue">需要加减的日期值</param> 
    ///<param name="interval">需要加减的部分y-year m-month d-day mi-minute s-second ms-milli</param> 
    ///<param name="value">值</param> 

    var d = new Date(dateValue);
    if (!interval || value === 0) return d;
    switch (interval.toLowerCase()) {
        case "ms":
            d.setMilliseconds(d.getMilliseconds() + value);
            break;
        case "s":
            d.setSeconds(d.getSeconds() + value);
            break;
        case "mi":
            d.setMinutes(d.getMinutes() + value);
            break;
        case "h":
            d.setHours(d.getHours() + value);
            break;
        case "d":
            d.setDate(d.getDate() + value);
            break;
        case "m":
            var day = d.getDate();

            /*
            当日期大于28号以后需要计算月底的对齐问题
            所以一律计算到目标月度下一个月的1号
            然后再setDate(0)计算月底的日期
            */
            if (day > 28) {
                var p = new Date(d);
                p.setDate(1);
                p.setMonth(d.getMonth() + value + 1);
                p.setDate(0);
                day = Math.min(day, p.getDate());
            }
            d.setDate(day);
            d.setMonth(d.getMonth() + value);
            break;
        case "y":
            d.setFullYear(d.getFullYear() + value);
            break;
    }
    return d;
}

function ReplaceRef(strValue) {
    var strText = /"/g;
    var str="ss:Table ss:id=\"CX\"";//ss: Table

    //strValue = strValue.replace("ss:Table", str);
    return strValue.replace(strText, "&quot;");
}

function ReplaceChr(strValue) {
    /// <summary>替换字符中的"<",">","&",在构建XML时用到 </summary>
    /// <param name="strString">字符串</param>
    /// <returns>字符串</returns>
    var strText;
    var strTemp;
    strText = /&/g;
    strTemp = strValue.replace(strText, "&amp;");
    strText = /</g;
    strTemp = strTemp.replace(strText, "&lt;");
    strText = />/g;
    strTemp = strTemp.replace(strText, "&gt;");
    return strTemp.replace(strText, "&gt;");
    //strText=/'/g;
    //return strTemp.replace(strText,"&apos;");			
}

function ReplaceStrCompare(strValue) {
    /// <summary>替换字符中的"&lt;","&gt;"为"<",">",在构建HTML时用到 </summary>
    /// <param name="strString">字符串</param>
    /// <returns>字符串</returns>
    var strText;
    var strTemp;
    strText = /&lt;/g;
    strTemp = strValue.replace(strText, "<");
    strText = /&gt;/g;
    return strTemp.replace(strText, ">");
}

function ReplaceStrAnd(strValue) {
    /// <summary>替换字符中的"&amp;"为"","&",替换"&quot;'为“"” </summary>
    /// <param name="strString">字符串</param>
    /// <returns>字符串</returns>
    var strText;
    strText = /&amp;/g;
    var strTemp = strValue.replace(strText, "&");

    strText = /&quot/g;
    strTemp = strTemp.replace(strText, "\"");
    return strTemp;
}

function GetFormatDate(dt,format) {
    /// <summary>设置当前格式化的日期</summary>
    /// <param name="dt">日期</param>
    /// <param name="format">日期格式 year-yyyy month-yyyy-mm day-yyyy-mm-dd</param>
    /// <returns>格式化之后的日期</returns>
    //var dt = new Date();
    var year = dt.getFullYear();
    var month = dt.getMonth();
    var actMonth = parseInt(month + 1);
    if (actMonth < 10) {
        actMonth = "0" + actMonth;
    }
    var day = dt.getDate();
    if (day < 10) {
        day = "0" + day;
    }

    if (format == "year") {
        return year;
    }
    else if (format == "month") {
        return year + "-" + actMonth;
    }
    else if (format == "day") {
        return year + "-" + actMonth + "-" + day;
    }
} 