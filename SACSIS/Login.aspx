<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.Login" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>汉能控股集团光伏电站数据监测平台</title>
    <style type="text/css">
        .button
        {
            width: 60px; /*图片宽带*/
            background-image: url(../img/login_button.jpg);
            border: none; /*去掉边框*/
            height: 60px; /*图片高度*/
            color: White;
            vertical-align: middle;
            text-align: center;
        }
        .body
        {
            background-color: #014b92;
        }
    </style>
</head>
<body class="body" text="#000000" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
    <form id="form1" runat="server">
    <div>
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                </td>
            </tr>
            <tr align="center" valign="middle">
                <td align="center" valign="middle">
                    <div>
                        <table width="644" height="465" border="0" cellpadding="0" cellspacing="0" style="background-image: url(../img/loginIndex-fx.jpg);
                            background-repeat: no-repeat;">
                            <tr>
                                <td height="348" colspan="3" valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td width="300" height="78" valign="top">
                                </td>
                                <td width="303" align="center" valign="top">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td  height="39" align="right" valign="bottom" style="font-size: 9pt" >
                                                用户名：
                                            </td>
                                            <td align="left" valign="bottom"><asp:TextBox ID="UserName" runat="server" Width="120px" Height="22px" TabIndex="1"></asp:TextBox></td>
                                            <td width="84" rowspan="2" align="center" valign="middle">
                                                <asp:Button ID="Submit" OnClick="btnSure_Click" runat="server" CssClass="button">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="39" align="right" valign="middle" style="font-size: 9pt">
                                                密&nbsp;&nbsp;&nbsp;码：
                                            </td>
                                            <td align="left" valign="top"><asp:TextBox ID="PassWord" runat="server" Width="120px" TextMode="Password"
                                                    Height="22px" TabIndex="2"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" valign="top" align="center">
                                    <a href="ActiveX/owc11.exe">报表插件</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
