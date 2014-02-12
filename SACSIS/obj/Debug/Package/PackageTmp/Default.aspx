<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SACSIS.Defalut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            height: 100%;
            overflow: auto;
            margin: 0px;
            padding: 0px;
            background-color: #c8c8c8;
        }
        .div_1
        {
            background-image: url(../img/default_bg_3-1.jpg);
            position: relative;
            display: block;
        }
        .div_2
        {
            background-image: url(../img/default_bg_3-2.jpg);
            position: relative;
            display: block;
        }
        .div_3
        {
            background-image: url(../img/default_bg_3-3.jpg);
            position: relative;
            display: block;
        }
        .div_4
        {
            background-image: url(../img/default_bg_1.jpg);
            position: relative;
            display: block;
        }
        .div_5
        {
            background-image: url(../img/default_bg_1.jpg);
            position: relative;
            display: block;
        }
        .div_6
        {
            background-image: url(../img/default_bg_3-6.jpg);
            position: relative;
            display: block;
        }
        .div_top_1
        {
            color: #000000;
            font-size: 14px;
            font: "宋体";
            position: absolute;
            top: 65px;
            left: 40px;
            width: 140px;
            height: 17px;
        }
    </style>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <script src="../Js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            
        });
        $(document).ready(function () {
           var a = '<%=User_id%>';
            if(a =="admin")
            {
            $("#gz").click(function () {
                location.href = "../StationList/SY_CZ.aspx"; //location.href实现客户端页面的跳转 
            });
                
            }

            $.post(
        "../datafile/Get_Groupzl_Data.aspx",
        {
            id: "1=1"
        },
    function (data) {
        var array = new Array();
        array = data.split(',');
        $("#lbl_Capacity").html(array[0]);
        $("#lbl_Reaload").html(array[1]);
        $("#lbl_Power").html(array[2]);
    },
    "html");
            setInterval(Reaload, "30000");
        });
        function Reaload() {
            $.post(
        "../datafile/Get_Groupzl_Data.aspx",
        {
            Reaload_id: "1=1", num: 1
        },
    function (data) {
        $("#lbl_Reaload").html(data);
    },
    "html");
}
function pageHeight() {
    if ($.browser.msie) {
        return document.compatMode == "CSS1Compat" ? document.documentElement.clientHeight :
            document.body.clientHeight;
    } else {
        return self.innerHeight;
    }
};

function pageWidth() {
    if ($.browser.msie) {
        return document.compatMode == "CSS1Compat" ? document.documentElement.clientWidth :
            document.body.clientWidth;
    } else {
        return self.innerWidth;
    }
};   
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#c8c8c8">
         <tr>
                <td colspan="8" style="background-image: url(../img/default_bg_0.jpg); background-repeat: repeat-x;
                    height: 15px;">
                </td>
            </tr>
            <tr >
                <td style="background-image: url(../img/default_bg_1.jpg); height: 116px; width: 20%;">
                </td>
                <td class="div_1"  align="center" valign="middle">
                    <div class="div_top_1">
                        <asp:Label ID="lbl_Capacity" runat="server" Text="0"></asp:Label>&nbsp;MW
                    </div>
                </td>
                <td class="div_4">
                    <div class="div_top_1">
                    </div>
                </td>
                <td class="div_2" align="center" valign="middle">
                    <div class="div_top_1">
                        <asp:Label ID="lbl_Reaload" runat="server" Text="0"></asp:Label>&nbsp;kW</div>
                </td>
                <td class="div_5">
                    <div class="div_top_1">
                    </div>
                </td>
                <td class="div_3" align="center" valign="middle">
                    <div class="div_top_1">
                        <asp:Label ID="lbl_Power" runat="server" Text="0"></asp:Label>&nbsp;kWh</div>
                </td>
                <td style="background-image: url(../img/default_bg_1.jpg); height: 116px; width: 20%;">
                </td>
            </tr>
            <tr>
                <td style="background-image: url(../img/default_bg_2.jpg); height: 554px;">
                </td>
                <td colspan="6" style="background-image: url(../img/default_bg_2.jpg); height: 554px;
                    width: 1218px; position: relative; display: block;">
                    <div id="hr" style="position: absolute; top: 80px; left: 805px;" title="怀柔">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="wz" style="position: absolute; top: 80px; left: 780px" title="吴忠">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="qh" style="position: absolute; top: 90px; left: 755px" title="青海">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="xj-fh" style="position: absolute; top: 70px; left: 730px" title="新疆-福海">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="ydl-lkl" style="position: absolute; top: 75px; left: 510px" title="意大利-拉奎拉">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="xl" style="position: absolute; top: 83px; left: 535px" title="希腊">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="hk" style="position: absolute; top: 130px; left: 812px" title="海口">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="yc" style="position: absolute; top: 90px; left: 815px" title="禹城">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="gz" style="position: absolute; top: 115px; left: 830px; cursor: pointer;" title="广州">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="els" style="position: absolute; top: 70px; left: 850px;" title="俄罗斯乌苏里斯克">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <div id="mfr" style="position: absolute; top: 122px; left: 241px;" title="莫菲尔电站二期">
                        <img alt="" src="../img/default_gf.png" />
                    </div>
                    <img alt="" src="../img/default_bg_6.jpg" style="height: 552px; width: 1218px;" />
                </td>
                <td style="background-image: url(../img/default_bg_2.jpg); height: 554px">
                </td>
            </tr>
            <tr>
                <td style="background-color: #c8c8c8">
                </td>
                <td colspan="6" style="background-color: #c8c8c8">
                </td>
                <td style="background-color: #c8c8c8">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
