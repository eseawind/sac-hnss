<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Czxxylb.aspx.cs" Inherits="SACSIS.StationList.Czxxylb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>场站信息一览</title>
    <link href="../Js/jQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../css/zTreeStyle/djxt.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jQueryEasyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            load();
            //            setInterval(function () {
            //                load();
            //            }, 1000 * 3);
        });
        var vars = new Array(), hash;
        function load() {
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            //vars["company_id"] 
            $.post("Czxxylb.aspx", { param: vars["company_id"] }, function (data) {
                $("#dv_show").show();
                var list = data.list;
                $("#dv_fd").html(list);
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
<body style="font-size: 12px;">
    <div id="dv_body" class="easyui-tabs" data-options="tools:'#tab-tools'">
        <div title="光伏" data-options="tools:'#p-tools'" style="padding: 5px;">
           <div id="dv_show" style="text-align: center; margin-top: 200px;">
                <img src="../img/loading.gif" />
            </div>
            <div id="dv_fd">
            
            </div>
        </div>
    </div>
</body>
</html>
