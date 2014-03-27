<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetSisReportStyle.aspx.cs" Inherits="SACSIS.SisReport.SetSisReportStyle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报表样式设置</title>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <script src="../Js/json.js" type="text/javascript"></script>
    <script src="../Js/jQueryEasyUI/jquery-1.6.2.js" type="text/javascript"></script>
    <script src="../Js/PageLibrary.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            height: 100%;         
            margin: 0px;
            padding: 0px;
        }
    </style>
    <script type="text/javascript">
        var Request = new Object();
        Request = GetRequest();

        var orgId =   Request['orgID']; 
        var treeId =Request['TREEID'];

        var globalKeyId = "";      //keyid关键字
        var globalReportName = ""; //报表描述
        var globalReportID = "";   //PageParameterGet("id");
        var globalSelObj;          //select的json对象数据

        $(document).ready(function () {
            $("#org").css("display", ""); //隐藏Treeid 
            $("#txtOrgId").val(orgId);
            $("#txtTreeId").val(treeId);
            GetRptIdList(treeId);
        });

        //获取样式List
        function GetRptIdList(treeId) {
            $.post("SetSisReportStyle.aspx", { param: 'GetIdList', treeID: treeId }, function (data) {
                if (Number(data.count) == 1) {
                    var list = data.list;
                    globalSelObj = data.list;
                    $("#selIdList").empty();
                    if (list != null) {
                        for (var i = 0; i < list.length; i++) {
                            $("#selIdList").append("<option value='" + list[i].REPORTID + "'>" + list[i].REPORTNAME + "</option>");
                        }
                        if ($("#selIdList")[0].length > 0) {
                            globalReportName = globalSelObj[0].REPORTNAME;
                            globalReportID = globalSelObj[0].REPORTID;
                            globalKeyId = globalSelObj[0].ID_KEY;
                            $("#txtRptId").val(globalReportID);
                            $("#thsel").css("display", "");
                            $("#thtxt").css("display", "none");
                            $("#txtRptName").val(globalReportName);
                            GetData();
                        }

                    } else { $("#selIdList").append("<option value='0'>无数据</option>"); }
                } else {
                    var timestamp = new Date().getTime();
                    $("#txtRptId").val(timestamp);
                    $("#thsel").css("display", "none");
                    $("#thtxt").css("display", "");
                }
            }, 'json');
        }
        //获取报表样式
        function GetData() {
            var p = { RptID: globalReportID };
            var ps = { act: "GetStyle", param: JSON.stringify(p) };
            $.ajax({
                url: "SisReportProxy.aspx", //ReportProxy.aspx
                type: 'POST',
                async: true,
                data: ps,
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                error: function (textStatus) {
                    alert(textStatus.responseText);
                },
                success: function (data, textStatus) {
                    var objResult = JSON.parse(data);
                    if (objResult.ErrMessage != null && objResult.ErrMessage != "") {
                        alert(objResult.ErrMessage);
                    } else {
                        var rpt = JSON.parse(objResult.ResultData);
                        if (rpt.RptStyle != null && rpt.RptStyle != "") {
                            $("#sp1")[0].XMLData = ReplaceStrAnd(ReplaceStrCompare(rpt.RptStyle));
                            globalAction = "view";
                        } else {
                            $("#sp1")[0].HTMLData = "<Data />";
                        }
                    }
                }
            });
        }
        //保存报表样式
        function SaveStyle() {
            var d = ReplaceRef($("#sp1")[0].XMLData);
            var rptId = $("#txtRptId").val(); // globalReportID; //txtRptId
            var rptName = $("#txtRptName").val();
            var tId = $("#txtTreeId").val();
            var orgList = $("#txtOrgId").val();
            var styleType = $("#selRptType").val();

            var ps = { RptID: rptId, RptName: rptName, RptStyle: d, styleType: styleType };
            var p = { act: "SaveStyle", param: JSON.stringify(ps), doact: 'shan' }; //globalAction

            $.ajax({
                url: "SisReportProxy.aspx",
                type: 'POST',
                async: true,
                data: p,
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                error: function (textStatus) {
                    alert(textStatus.responseText);
                },
                success: function (data, textStatus) {
                    var objResult = JSON.parse(data);
                    if (objResult.Status) {
                        globalAction = "view";

                        alert("保存成功");
                    } else {
                        alert(objResult.ErrMessage);
                    }
                }
            });
        }
        //描述Change 
        function SetDesc() {
            globalReportID = $("#selIdList").val();
            globalReportName = document.getElementById("selIdList").options[document.getElementById("selIdList").selectedIndex].text;

            $("#txtRptId").val(globalReportID);
            $("#txtRptName").val(globalReportName);

            GetData();
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

        //新建方法
        function Add() {
            var timestamp = new Date().getTime();
            $("#txtRptId").val(timestamp);
            //$("#sp1")[0].HTMLData = "<Data />";

            $("#thsel").css("display", "none");
            $("#thtxt").css("display", "");

            $("#txtRptName").val('');
        }
        function Resize() { }
    </script>
</head>
<body  style="margin: 0px;font-size: 12px;" onresize="Resize()">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="scriptMng" runat="server">
    </asp:ScriptManager>
    </form>
    <table cellpadding="0" cellspacing="0" width="100%" border="0" style="table-layout: fixed; height: 100%;">
        <tr style="height: 30px">
            <td>
                <table width="100%" style="table-layout: fixed">
                    <tr>
                        <td style="width: 120px; " >
                            报表类型：
                        </td>
                        <td width="150px">
                            <select id="selRptType">
                                <option value="D">日报</option>
                                <option value="M">月报</option>
                                <option value="Y">年报</option>
                                <option value="DD">日明细报表(24小时)</option>
                                <option value="MM">月明细报表(31天)</option>
                                <option value="YY">年明细报表(12月)</option>
                                <option value="O">其它</option>
                            </select>
                        </td>
                        <td style="width: 100px;">
                            模板ID：
                        </td>
                        <td style="width: 120px;">
                            <input type="text" disabled="disabled" id="txtRptId" style="width: 100px" />
                        </td>
                        <td style="width: 100px;">
                            模板描述：
                        </td>
                        <td id="thtxt">
                            <input type="text" id="txtRptName" style="width: 200px" />
                        </td>
                        <td id="thsel">
                            <select id="selIdList" style="width: 100px;" onchange="SetDesc()">
                            </select>
                        </td>
                         <td >
                            <a href="#" onclick="Add()">【新建】</a> <a href="#" onclick="SaveStyle()">【保存】</a>
                        </td>
                    </tr>
                    
                    <tr id="org">
                        <td>
                            组织机构树ID：
                        </td>
                        <td>
                            <input type="text" id="txtTreeId" style="width: 140px" />
                        </td>
                        <td>
                            组织机构ID：
                        </td>
                        <td>
                            <input type="text" id="txtOrgId" style="width: 100px" />
                        </td>
                       <%-- <td colspan="2">
                            <a href="#" onclick="Add()">【新建】</a> <a href="#" onclick="SaveStyle()">【保存】</a>
                        </td>--%>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
          <tr>
            <td id="tdSP" style="border-right: #808080 1px solid; border-top: #808080 1px solid;
                border-left: #808080 1px solid; border-bottom: #808080 1px solid">
                <object classid="clsid:0002E559-0000-0000-C000-000000000046" codebase="../ActiveX/owc11.exe"
                    id="sp1" style="width: 100%; height:460px;">
                    
                    <param name="DataType" value="XMLData" />
                    <param name="DataType" value="XMLData" />
                    <param name="AllowPropertyToolbox" value="True" />
                    <param name="AutoFit" value="0" />
                    <param name="Calculation" value="xlCalculationAutomatic" />
                    <param name="DisplayColumnHeadings" value="True" />
                    <param name="DisplayGridlines" value="True" />
                    <param name="DisplayHorizontalScrollBar" value="True" />
                    <param name="DisplayOfficeLogo" value="True" />
                    <param name="DisplayPropertyToolbox" value="False" />
                    <param name="DisplayRowHeadings" value="True" />
                    <param name="DisplayTitleBar" value="False" />
                    <param name="DisplayToolbar" value="True" />
                    <param name="DisplayVerticalScrollBar" value="False" />
                    <param name="DisplayWorkbookTabs" value="True" />
                    <param name="EnableEvents" value="True" />
                    <param name="MaxHeight" value="100%" />
                    <param name="MaxWidth" value="100%" />
                    <param name="MoveAfterReturn" value="-1" />
                    <param name="MoveAfterReturnDirection" value="-4121" />
                    <param name="RightToLeft" value="0" />
                    <param name="ScreenUpdating" value="True" />
                    <param name="EnableUndo" value="True" />
                    <param name="XMLData" value="&lt;?xml version=&quot;1.0&quot;?&gt;
&lt;ss:Workbook xmlns:x=&quot;urn:schemas-microsoft-com:office:excel&quot;
 xmlns:ss=&quot;urn:schemas-microsoft-com:office:spreadsheet&quot;
 xmlns:c=&quot;urn:schemas-microsoft-com:office:component:spreadsheet&quot;&gt;
 &lt;x:ExcelWorkbook&gt;
  &lt;x:ProtectStructure&gt;False&lt;/x:ProtectStructure&gt;
  &lt;x:ActiveSheet&gt;0&lt;/x:ActiveSheet&gt;
  &lt;x:HideVerticalScrollBar/&gt;
 &lt;/x:ExcelWorkbook&gt;
 &lt;ss:Styles&gt;
  &lt;ss:Style ss:ID=&quot;Default&quot;&gt;
   &lt;ss:Alignment ss:Horizontal=&quot;Automatic&quot; ss:Rotate=&quot;0.0&quot; ss:Vertical=&quot;Bottom&quot;
    ss:ReadingOrder=&quot;Context&quot;/&gt;
   &lt;ss:Borders&gt;
   &lt;/ss:Borders&gt;
   &lt;ss:Font ss:FontName=&quot;宋体&quot; ss:Size=&quot;11&quot; ss:Color=&quot;Automatic&quot; ss:Bold=&quot;0&quot;
    ss:Italic=&quot;0&quot; ss:Underline=&quot;None&quot;/&gt;
   &lt;ss:Interior ss:Color=&quot;Automatic&quot; ss:Pattern=&quot;None&quot;/&gt;
   &lt;ss:NumberFormat ss:Format=&quot;General&quot;/&gt;
   &lt;ss:Protection ss:Protected=&quot;1&quot;/&gt;
  &lt;/ss:Style&gt;
 &lt;/ss:Styles&gt;
 &lt;c:ComponentOptions&gt;
  &lt;c:Label&gt;
   &lt;c:Caption&gt;Microsoft Office 电子表格&lt;/c:Caption&gt;
  &lt;/c:Label&gt;
  &lt;c:MaxHeight&gt;100%&lt;/c:MaxHeight&gt;
  &lt;c:MaxWidth&gt;100%&lt;/c:MaxWidth&gt;
  &lt;c:NextSheetNumber&gt;4&lt;/c:NextSheetNumber&gt;
 &lt;/c:ComponentOptions&gt;
 &lt;x:WorkbookOptions&gt;
  &lt;c:OWCVersion&gt;12.0.0.4518         &lt;/c:OWCVersion&gt;
  &lt;x:Height&gt;12171&lt;/x:Height&gt;
  &lt;x:Width&gt;24580&lt;/x:Width&gt;
 &lt;/x:WorkbookOptions&gt;
 &lt;ss:Worksheet ss:Name=&quot;Sheet1&quot;&gt;
  &lt;x:WorksheetOptions&gt;
   &lt;x:Selected/&gt;
   &lt;x:ViewableRange&gt;R1:R262144&lt;/x:ViewableRange&gt;
   &lt;x:Selection&gt;R3C7&lt;/x:Selection&gt;
   &lt;x:TopRowVisible&gt;0&lt;/x:TopRowVisible&gt;
   &lt;x:LeftColumnVisible&gt;0&lt;/x:LeftColumnVisible&gt;
   &lt;x:ProtectContents&gt;False&lt;/x:ProtectContents&gt;
  &lt;/x:WorksheetOptions&gt;
  &lt;c:WorksheetOptions&gt;
  &lt;/c:WorksheetOptions&gt;
  &lt;ss:Table ss:DefaultColumnWidth=&quot;54.0&quot; ss:DefaultRowHeight=&quot;12.75&quot;&gt;
  &lt;/ss:Table&gt;
 &lt;/ss:Worksheet&gt;
 &lt;ss:Worksheet ss:Name=&quot;Sheet2&quot;&gt;
  &lt;x:WorksheetOptions&gt;
   &lt;x:ViewableRange&gt;R1:R262144&lt;/x:ViewableRange&gt;
   &lt;x:Selection&gt;R1C1&lt;/x:Selection&gt;
   &lt;x:TopRowVisible&gt;0&lt;/x:TopRowVisible&gt;
   &lt;x:LeftColumnVisible&gt;0&lt;/x:LeftColumnVisible&gt;
   &lt;x:ProtectContents&gt;False&lt;/x:ProtectContents&gt;
  &lt;/x:WorksheetOptions&gt;
  &lt;c:WorksheetOptions&gt;
  &lt;/c:WorksheetOptions&gt;
  &lt;ss:Table ss:DefaultColumnWidth=&quot;54.0&quot; ss:DefaultRowHeight=&quot;12.75&quot;&gt;
  &lt;/ss:Table&gt;
 &lt;/ss:Worksheet&gt;
 &lt;ss:Worksheet ss:Name=&quot;Sheet3&quot;&gt;
  &lt;x:WorksheetOptions&gt;
   &lt;x:ViewableRange&gt;R1:R262144&lt;/x:ViewableRange&gt;
   &lt;x:Selection&gt;R1C1&lt;/x:Selection&gt;
   &lt;x:TopRowVisible&gt;0&lt;/x:TopRowVisible&gt;
   &lt;x:LeftColumnVisible&gt;0&lt;/x:LeftColumnVisible&gt;
   &lt;x:ProtectContents&gt;False&lt;/x:ProtectContents&gt;
  &lt;/x:WorksheetOptions&gt;
  &lt;c:WorksheetOptions&gt;
  &lt;/c:WorksheetOptions&gt;
  &lt;ss:Table ss:DefaultColumnWidth=&quot;54.0&quot; ss:DefaultRowHeight=&quot;12.75&quot;&gt;
  &lt;/ss:Table&gt;
 &lt;/ss:Worksheet&gt;
&lt;/ss:Workbook&gt;
" />
                </object>              
            </td>
        </tr>
        <tr style="height: 0px; display: none">
            <td>
            </td>
        </tr>
    </table>
</body>
</html>
