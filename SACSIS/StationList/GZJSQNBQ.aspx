<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GZJSQNBQ.aspx.cs" Inherits="SACSIS.StationList.GZJSQNBQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Js/jQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../css/update8.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../Js/jQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            font-family: 宋体, Helvetica, sans-serif;
            font-size: 12px;
            background: #dfe8f6;
        }
        #menu
        {
            border: 1px solid #2a88bb;
        }
        .button
        {
            width: 56px; /*图片宽带*/
            background: url(../img/button.jpg) no-repeat left top; /*图片路径*/
            border: none; /*去掉边框*/
            height: 24px; /*图片高度*/
            color: White;
            vertical-align: middle;
            text-align: center;
        }
        .style5
        {
            font-size: 12px;
            color: Black;
        }
        .style6
        {
            font-size: 13px;
            color: #0a4869;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript" language="javascript">
        $(function () {
            /*初始化数据*/
            Init();
        });
        function Init() {
            var vars = new Array(), hash, Groupzl_id;

            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            $.post("GZJSQNBQ.aspx", { param: vars["id"] }, function (data) {
                $("#dv_show").show();
                $("#dv_data").html(data.tb);
                $("#dv_show").hide();
            }, 'json');
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
    <div id="menu">
        <div id="dv_show" style="text-align: center; margin-top: 200px;">
            <img src="../img/loading.gif" />
        </div>
        <div id="dv_data" style="text-align: center; float: inherit;">
        </div>
    </div>
    </form>
</body>
</html>
