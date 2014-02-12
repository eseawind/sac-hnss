<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SACSIS.Defalut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            height: 100%;
            overflow: auto;
            margin: 0px;
            padding: 0px;
            background-color: #ffffff;
            text-align: center;
        }
        .div_bg
        {
            background-color: #ffffff;
            width: 1220px;
            height: 768px;
            border: 0px red solid;
            overflow: auto;
            text-align: center;
        }
        .div_1
        {
            background-image: url(../img/default_bg_3-1.jpg);
            height: 116px;
            width: 400px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
            text-align: center;
            line-height: 116px;
            float: left;
        }
        .div_2
        {
            background-image: url(../img/default_bg_3-2.jpg);
            height: 116px;
            width: 400px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
            text-align: center;
            line-height: 116px;
            float: left;
        }
        .div_3
        {
            background-image: url(../img/default_bg_3-3.jpg);
            height: 116px;
            width: 400px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
            text-align: center;
            line-height: 116px;
            float: left;
        }
        .div_map
        {
            margin-top:116px;
            background-color: #c8c8c8;
            height: 552px;
            width: 1218px;
            position: relative;
            display: block;
            background-image: url(../img/default_bg_6.jpg);
        }
        
        .div_top_1
        {
            color: #000000;
            font-size: 14px;
            font: "宋体";
            width: 140px;
            height: 17px;
        }
    </style>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <script src="../Js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var a = '<%=User_id%>';
            if (a == "admin") {
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
            var a = '<%=User_id%>';
            if (a == "admin") {
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
//            $.post(
//        "../datafile/Get_Groupzl_Data.aspx",
//        {
//            Reaload_id: "1=1", num: 1
//        },
//    function (data) {
//        $("#lbl_Reaload").html(data);
//    },
//    "html");
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
    <form id="form1" runat="server" style="background-color: ffffff;">
    <div class="div_bg">
        <div class="div_1">
            <asp:Label ID="lbl_Capacity" runat="server" Text="0"></asp:Label>&nbsp;MW</div>
        <div class="div_2">
            <asp:Label ID="lbl_Reaload" runat="server" Text="0"></asp:Label>&nbsp;kW</div>
        <div class="div_3">
            <asp:Label ID="lbl_Power" runat="server" Text="0"></asp:Label>&nbsp;kWh</div>
        <div class="div_map">
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
            <div id="gz" style="position: absolute; top: 115px; left: 830px; cursor: pointer;"
                title="广州">
                <img alt="" src="../img/default_gf.png" />
            </div>
            <div id="els" style="position: absolute; top: 70px; left: 850px;" title="俄罗斯乌苏里斯克">
                <img alt="" src="../img/default_gf.png" />
            </div>
            <div id="mfr" style="position: absolute; top: 122px; left: 241px;" title="莫菲尔电站二期">
                <img alt="" src="../img/default_gf.png" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
