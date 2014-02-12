<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Web.Main" %>

<html>
<head id="Head1" runat="server">
    <title>汉能控股集团总部光伏电站数据监测平台</title>
    <link href="../Js/jQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../css/update8.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jQueryEasyUI/jquery-1.6.2.js" type="text/javascript"></script>
    <style type="text/css">
        html
        {
            overflow: hidden;
        }
        body
        {
            height: 100%;
            overflow: hidden;
            margin: 0px;
            padding: 0px;
        }
        .box
        {
            height: 100%;
            background: #ff0000;
            position: absolute;
            width: 100%;
        }
        .Text1
        {
            font-size: 13px;
            font-weight: normal;
            color: #555656;
        }
        .Text2
        {
            font-size: 13px;
            font-weight: normal;
            color: #FFFFFF;
        }
        .Text3
        {
            font-size: 13px;
            font-weight: normal;
            color: Yellow;
        }
        A:link
        {
            text-decoration: none;
        }
        A:visited
        {
            text-decoration: none;
        }
        a
        {
            float: left;
            display: block;
            width: 100px;
            height: 34px;
            line-height: 34px;
            overflow: hidden;
            text-align: center;
            font-size: 13px;
            color: #555656;
            background: url("../img/MENU_1.jpg") no-repeat;
            margin-top: 0px;
        }
        .menu01
        {
            color: #ffffff;
            background: url("../img/MENU_1_checked.jpg") no-repeat;
        }
        .menu02
        {
            color: #555656;
            background: url("../img/MENU_1.jpg") no-repeat;
        }
    </style>
    <script type="text/javascript">
		<!--

        $(document).ready(function () {
            $('#mu').hide();
            $('#FItem').hide();
            $("#menu1").attr("class", "menu01");

            $("#dv_connect").css("height", pageHeight() - 74);
            $("#dv_connect").css("width", pageWidth());

            //$("#dv_Menu_left").css("width", pageWidth() - 250);

            $("#frm").css("height", pageHeight() - 84);
            Menu();
        });

        /*Cookie 操作*/
        function setCookie(name, value, time) {
            var strsec = getsec(time);
            var exp = new Date();
            exp.setTime(exp.getTime() + strsec * 1);
            document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
        }

        function getsec(str) {
            var str1 = str.substring(1, str.length) * 1;
            var str2 = str.substring(0, 1);
            if (str2 == "s") {
                return str1 * 1000;
            }
            else if (str2 == "h") {
                return str1 * 60 * 60 * 1000;
            }
            else if (str2 == "d") {
                return str1 * 24 * 60 * 60 * 1000;
            }
        }
        //删除cookies
        function delCookie(name) {
            var exp = new Date();
            exp.setTime(exp.getTime() - 1);
            var cval = getCookie(name);
            if (cval != null)
                document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
        }
        /*Cookie 操作*/

        function SetUrl(judge, id, url) {
            if (judge == 1) {
                if (url != '' && url != null && url != undefined) {
                    setCookie('url', url, 's60');
                }
                window.frm.location.href = "Connect.aspx?id=" + id;
            } else {
                document.getElementById("frm").src = url;
            }
        }
        function Menu() {
            window.frm.location.href = "default.aspx";
        }

        function changeMenu(t) {
            var s = $('#mu').attr("Value");
            //alert(s);
            t = String(t);
            //alert($("#menu"+t).css("class"));
            for (i = 0; i <= s; i++) {
                if (i == t) {
                    $("#menu" + t).attr("class", "menu01");
                } else {
                    $("#menu" + i).attr("class", "menu02");
                }
            }

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
		//-->
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%">
        <tr>
            <td colspan="2" style="height: 50px; background-image: url(../img/banner_02.jpg);
                width: 100%">
                <div id="dv_head" style="height: 50px; margin-bottom: 0px; margin-left: 0px; margin-right: 0px;
                    margin-top: 0px;">
                    <img src="../img/banner.jpg" height="50" alt="" />
                </div>
            </td>
        </tr>
        <tr style="background-image: url(../img/MENU_2_bg.gif); height: 34px;">
            <td>
                <div id="dv_Menu_left" runat="server">
                </div>
            </td>
            <td>
                <input id="mu" type="text" runat="server" size="1" /><input id="FItem" type="text"
                    runat="server" size="1" />
            </td>
            <!--<td align="right" valign="middle">
            <a class="Text3">欢迎您</a>&nbsp;&nbsp;
            <asp:Label ID="lblUserWelcome" runat="server" CssClass="Text1"></asp:Label>&nbsp;
            <asp:LinkButton ID="linkBtnLogout" runat="server" OnClick="linkBtnLogout_Click" BorderStyle="None" CssClass="Text1">注销</asp:LinkButton>&nbsp;&nbsp;
        </td>-->
        </tr>
        <tr>
            <td colspan="2">
                <div id="dv_connect" style="background-color: Lime;">
                    <iframe width="100%" src="" id="frm" frameborder="0" name="frm" scrolling="yes"></iframe>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
