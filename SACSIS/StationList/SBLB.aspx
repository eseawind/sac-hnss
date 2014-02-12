<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SBLB.aspx.cs" Inherits="SACSIS.StationList.SBLB" %>

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
            height: 100%;
            overflow: auto;
            margin: 0px;
            padding: 0px;
            background-color: #ffffff;
        }
        .div_1
        {
            background-image: url(../img/SBLB.jpg);
            height: 100px;
            width: 190px;
            position: relative;
            display: block;
            background-repeat: no-repeat;
            background-position: left;
        }
        .div_top_1
        {
            color: #d6000c;
            font-size: 16px;
            font: "宋体";
            font-weight: bold;
            position: absolute;
            top: 10px;
            left: 110px;
            width: 20px;
            height: 17px;
        }
        .div_top_2
        {
            color: #000000;
            font-size: 16px;
            font: "宋体";
            position: absolute;
            top: 35px;
            left: 90px;
            width: 140px;
            height: 17px;
        }
        .div_top_3
        {
            color: #000000;
            font-size: 16px;
            font: "宋体";
            position: absolute;
            top: 60px;
            left: 90px;
            width: 140px;
            height: 17px;
        }
    </style>

    <script type="text/javascript">
    	<!--
        var num = 0;
        $(function () {
            /*初始化数据*/
            Init();
        });


        /*初始化页面数据   开始*/
        function Init() {
            var vars = new Array(), hash, Groupzl_id;

            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            $.post("SBLB.aspx", { param: vars["id"] }, function (data) {
                $("#dv_show").show();
                $("#dv_data").html(data.tb);
                $("#dv_show").hide();
            }, 'json');
        }
        /*初始化页面数据   结束*/


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
 <div id="dv_show" style="text-align: center; margin-top: 200px;">
                <img src="../img/loading.gif" />
            </div>
    <div id="dv_data" style="text-align: center; float: inherit;">
    </div>
    
</body>
</html>
