<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowReport.aspx.cs" Inherits="SACSIS.SisReport.ShowReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>报表展示</title>
    <meta content="JavaScript" name="vs_defaultClientScript"/>   
    <style type="text/css">
        table
        {
            border-collapse: collapse;
            vert-align: bottom; /**/
        }
      
    </style>        
    <link href="../css/SIS.css" rel="stylesheet" type="text/css" />
    <link href="../css/zTreeStyle/djxt.css" rel="stylesheet" type="text/css" />
    <link href="../css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />        
    <link href="../Js/jQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
     <script src="../Js/Excel.js" type="text/javascript" language="javascript"  ></script>   
    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../Js/jQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Js/jQueryZtree/jquery.ztree.core-3.5.js" type="text/javascript"></script>
    <script src="../Js/Excel.js" type="text/javascript"></script>
    <script src="../Js/PageLibrary.js" type="text/javascript" ></script>

    <script type="text/javascript" language="javascript">
        var Request = new Object();
        Request = GetRequest();

        var date = "";  //时间变量
        var dType = ""; //日期类型
        //var rptId =  Request['id']; //'1375946526874'; //GetQueryString("id"); 
        //var rptName = Request['name']; //GetQueryString("name");
        //var globalOrgId = Request['orgID'];// '10034'; //  // GetQueryString("orgID");
        //var globalTreeId = Request['TREEID']; //  'XNY'; // GetQueryString("TREEID");
        var rptId = Request['id']; //GetQueryString("id"); 
        var rptName = Request['name']; //GetQueryString("name");
        var globalOrgId =""; //  // GetQueryString("orgID");
        var globalTreeId = ""; // GetQueryString("TREEID");
        $(document).ready(function () {

            //根据报表类型来动态切换日期框的类型
            var myDate = new Date();
            var date = DateAdd(myDate, "d", -1);
            var dt = date.getFullYear() + "-" + parseInt(date.getMonth() + 1) + "-" + parseInt(date.getDate());

            $("#dateD").css("display", "none");
            $("#dateM").css("display", "none");
            $("#dateY").css("display", "none");

            $.post("ReportTreeProxy.aspx", { param: 'isDate', rptid: rptId, name: rptName, orgid: globalOrgId, treeid: globalTreeId }, function (data) {
                dType = data.dateType;
                if (dType != "" && dType != null) {
                    if (dType == 'D') {
                        $("#dateD").css("display", "");
                        $("#dateD").val(date.getFullYear() + "-" + parseInt(date.getMonth() + 1) + "-" + parseInt(date.getDate()));
                        date = $("#dateD").val();
                    }
                    else if (dType == 'M' || dType == 'MM') {
                        $("#dateM").css("display", "");
                        $("#dateM").val(date.getFullYear() + "-" + parseInt(date.getMonth() + 1));
                        date = $("#dateM").val();
                    }
                    else if (dType == 'Y' || dType == 'YY') {
                        $("#dateY").css("display", "");
                        $("#dateY").val(date.getFullYear());
                        date = $("#dateY").val();
                    }
                    GetData(date);
                } else {
                    $("#thShow").css("display", "none");
                    $("#dvShow").html("此机构无此报表!");
                }
            }, 'json');

            $("#btnCX").click(function () {
                if (dType == 'D') {
                    date = $("#dateD").val();
                } else if (dType == 'M' || dType == 'MM') {
                    date = $("#dateM").val();
                } else if (dType == 'Y' || dType == 'YY') {
                    date = $("#dateY").val();
                }
                GetData(date);
            });

            $("#btnDC").click(function () {
                toExcel(CX);
            });
        });

        function GetData(date) {
            var p = { param: 'query', time: date, rptid: rptId, name: rptName, orgid: globalOrgId, treeid: globalTreeId };
            $("#divWait").show();
            $.ajax({
                url: "ReportTreeProxy.aspx",
                type: 'POST',
                async: true,
                data: p,
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                error: function (textStatus) {
                    alert(textStatus.responseText);
                },
                success: function (data, textStatus) {
                    $("#dvShow").html(data);
                },
                complete: function (response, textStatus) {
                    $("#divWait").hide();
                }
            });
        }

        function Print() {
            var aaa = document.all.dvShow.innerHTML; // document.all.dvShow.innerHTML;
            var ddd = document.body.innerHTML;
            document.body.innerHTML = aaa;
            window.print();
            document.body.innerHTML = ddd;
            return false;
        }

        function GetRequest() {
            var url = location.search; //获取url中"?"符以及其后的字串  
            var theRequest = new Object();
            if (url.indexOf("?") != -1)//url中存在问号，也就说有参数。  
            {
                var str = url.substr(1);
                strs = str.split("&");
                for (var i = 0; i < strs.length; i++) {
                    theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
                }
            }
            return theRequest;
        }       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    </form>
    <div id="dv_right">
        <table style="width: 100%;">
            <tr>
                <td id="thShow" align="left" valign="middle">
                    <label id="lalStime" runat="server" class="td_title">
                        查询日期:</label>
                    <input type="text" id="dateD" style="text-align: center;" runat="server" readonly="readonly"
                        onfocus="WdatePicker({skin:'whyGreen',dateFmt:'yyyy-MM-dd'})" class="Wdate" />
                    <input type="text" id="dateM" style="text-align: center;" runat="server" readonly="readonly"
                        onfocus="WdatePicker({skin:'whyGreen',dateFmt:'yyyy-MM'})" class="Wdate" />
                    <input type="text" id="dateY" style="text-align: center;" runat="server" readonly="readonly"
                        onfocus="WdatePicker({skin:'whyGreen',dateFmt:'yyyy'})" class="Wdate" />
                    <a id="btnCX" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-ok'">查询</a>
                    <a id="btnDC" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-ok'">导出</a>
                </td>
            </tr>
        </table>
        <div id="divWait" style="height: 30px; display: none">
            <img id="loadImg" src="../img/loading.gif" />
        </div>
        
        <table border="0" width="100%" id="CX" style="overflow-x: auto; overflow-y: auto">
            <tr style="width:100%;">
                <td >
                     <div id="dvShow" runat="server"></div>                   
                </td>
            </tr>
        </table>       
        <div id="showInfo" style="overflow-x: auto; overflow-y: auto">          
        </div>
    </div>
</body>
</html>
