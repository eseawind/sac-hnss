<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpData.aspx.cs" Inherits="SACSIS.Data.UpData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据填报</title>
    <link href="../Js/jQueryEasyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../Js/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
    <link href="../css/themes/default/default.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../Js/jQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Js/kindeditor.js" type="text/javascript"></script>
    <script src="../Js/lang/zh_CN.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            height: 100%;
            margin: 10px;
            padding: 0px;
        }
        #menu
        {
            border: 1px solid #2a88bb;
        }
        .ipt
        {
            width: 80px;
            border: solid 1px black;
        }
        .ipt_txt
        {
            width: 330px;
            border: solid 1px black;
        }
        .ipt_zb
        {
            border: solid 1px black;
        }
        .tdh
        {
            width: 90px;
        }
    </style>
    <script type="text/javascript">
    	<!--
        var ids;
        var connect_0;
        var connect_1;
        var connect_2;
        var connect_3;
        var connect_4;
        var connect_5;
        var connect_6;
        var connect_7;
        var connect_8;

        $(function () {
            load();
            $("#btnUp").click(function () {
                var keyValue = '';
                var val = ids.split('~');
                if (val.length > 1) {
                    var key = val[0].split('*');
                    for (var i = 0; i < key.length; i++) {
                        keyValue += key[i] + '~' + jQuery('#' + key[i]).val() + '`';
                    }
                    keyValue = keyValue.substring(0, keyValue.length - 1);

                    key = val[1].split('~');
                    if (key.length == 1) {
                        keyValue += key[0] + '~' + connect_0.html() + '`';
                    } else if (key.length == 2) {
                        keyValue += key[0] + '~' + connect_0.html() + '`';
                        keyValue += key[1] + '~' + connect_1.html() + '`';
                    } else if (key.length == 3) {
                        keyValue += key[0] + '~' + connect_0.html() + '`';
                        keyValue += key[1] + '~' + connect_1.html() + '`';
                        keyValue += key[2] + '~' + connect_2.html() + '`';
                    } else if (key.length == 4) {
                        keyValue += key[0] + '~' + connect_0.html() + '`';
                        keyValue += key[1] + '~' + connect_1.html() + '`';
                        keyValue += key[2] + '~' + connect_2.html() + '`';
                        keyValue += key[3] + '~' + connect_3.html() + '`';
                    } else if (key.length == 5) {
                        keyValue += key[0] + '~' + connect_0.html() + '`';
                        keyValue += key[1] + '~' + connect_1.html() + '`';
                        keyValue += key[2] + '~' + connect_2.html() + '`';
                        keyValue += key[3] + '~' + connect_3.html() + '`';
                        keyValue += key[4] + '~' + connect_4.html() + '`';
                    } else if (key.length == 6) {
                        keyValue += key[0] + '~' + connect_0.html() + '`';
                        keyValue += key[1] + '~' + connect_1.html() + '`';
                        keyValue += key[2] + '~' + connect_2.html() + '`';
                        keyValue += key[3] + '~' + connect_3.html() + '`';
                        keyValue += key[4] + '~' + connect_4.html() + '`';
                        keyValue += key[5] + '~' + connect_5.html() + '`';
                    } else if (key.length == 7) {
                        keyValue += key[0] + '~' + connect_0.html() + '`';
                        keyValue += key[1] + '~' + connect_1.html() + '`';
                        keyValue += key[2] + '~' + connect_2.html() + '`';
                        keyValue += key[3] + '~' + connect_3.html() + '`';
                        keyValue += key[4] + '~' + connect_4.html() + '`';
                        keyValue += key[5] + '~' + connect_5.html() + '`';
                        keyValue += key[6] + '~' + connect_6.html() + '`';
                    } else if (key.length == 8) {
                        keyValue += key[0] + '~' + connect_0.html() + '`';
                        keyValue += key[1] + '~' + connect_1.html() + '`';
                        keyValue += key[2] + '~' + connect_2.html() + '`';
                        keyValue += key[3] + '~' + connect_3.html() + '`';
                        keyValue += key[4] + '~' + connect_4.html() + '`';
                        keyValue += key[5] + '~' + connect_5.html() + '`';
                        keyValue += key[6] + '~' + connect_6.html() + '`';
                        keyValue += key[7] + '~' + connect_7.html() + '`';
                    } else if (key.length == 9) {
                        keyValue += key[0] + '~' + connect_0.html() + '`';
                        keyValue += key[1] + '~' + connect_1.html() + '`';
                        keyValue += key[2] + '~' + connect_2.html() + '`';
                        keyValue += key[3] + '~' + connect_3.html() + '`';
                        keyValue += key[4] + '~' + connect_4.html() + '`';
                        keyValue += key[5] + '~' + connect_5.html() + '`';
                        keyValue += key[6] + '~' + connect_6.html() + '`';
                        keyValue += key[7] + '~' + connect_7.html() + '`';
                        keyValue += key[8] + '~' + connect_8.html() + '`';

                    }
                } else {
                    var key = ids.split('*');
                    for (var i = 0; i < key.length; i++) {
                        keyValue += key[i] + '~' + jQuery('#' + key[i]).val() + '`';
                    }
                    keyValue = keyValue.substring(0, keyValue.length - 1);
                }
                $.messager.confirm('数据填报', '请确认是否要保存数据!', function (ok) {
                    if (ok) {
                        $.post("UpData.aspx", { param: 'query', dTime: $("#txtDay").val(), value: escape(keyValue), mTime: $("#txtMonth").val(), yTime: $("#txtYear").val() }, function (data) {
                            $.messager.alert('数据填报', data.info, 'info');
                        }, 'json');
                    } else {
                        $.messager.alert('数据填报', '数据保存已经取消!', 'info');
                    }
                });
            });

            //公式计算
            $("#btnReckon").click(function () {
                var keyValue = '';
                var key = ids.split('*');
                for (var i = 0; i < key.length; i++) {
                    keyValue += key[i] + '~' + jQuery('#' + key[i]).val() + '`';
                }
                keyValue = keyValue.substring(0, keyValue.length - 1);
                $.post("UpData.aspx", { param: 'reckon', value: escape(keyValue) }, function (data) {
                    var val = data.key;
                    var key = val.split(';');
                    for (var i = 0; i < key.length; i++) {
                        var value = key[i].toString().split('*');
                        $("#" + value[0]).val(value[1]);
                    }
                }, 'json');
            });
        });

        var num;
        var kineNum = 0;
        function load() {
            $.post("UpData.aspx", { param: 'Init', dTime: $("#txtDay").val(), mTime: $("#txtMonth").val(), yTime: $("#txtYear").val() }, function (data) {
                var tb = data.table;
                $("#dv_body").html(tb);
                ids = data.key;
                kineNum = Number(data.kind_count);
                if (Number(data.kind_count) > 0) {
                    connect_0 = KindEditor.create('textarea[name="content_0"]');
                    connect_1 = KindEditor.create('textarea[name="content_1"]');
                    connect_2 = KindEditor.create('textarea[name="content_2"]');
                    connect_3 = KindEditor.create('textarea[name="content_3"]');
                    connect_4 = KindEditor.create('textarea[name="content_4"]');
                    connect_5 = KindEditor.create('textarea[name="content_5"]');
                    connect_6 = KindEditor.create('textarea[name="content_6"]');
                    connect_7 = KindEditor.create('textarea[name="content_7"]');
                    connect_8 = KindEditor.create('textarea[name="content_8"]');
                }

            }, 'json');
        }
        function SetTime() {
            var myDate = new Date();
            var date;
            date = myDate.getFullYear() + '-' + Number(myDate.getMonth() + 1) + '-' + Number(myDate.getDate());
            $("#txtDay").val(date);
            $("#txtMonth").val(myDate.getFullYear() + '-' + Number(myDate.getMonth() + 1));
            $("#txtYear").val(myDate.getFullYear());
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
<body style="font-size: 12px;">
    <table>
        <tr>
            <td>
                <div id="dv_day" style="margin-top: 5px; height: 30px;" runat="server">
                    &nbsp; &nbsp 开始时间: &nbsp; &nbsp;
                    <input type="text" id="txtDay" style="text-align: center;" runat="server" readonly="readonly"
                        onfocus="WdatePicker({skin:'whyGreen',dateFmt:'yyyy-MM-dd'})" class="Wdate" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <div id="dv_month" style="margin-top: 5px; height: 30px;" runat="server">
                    &nbsp; &nbsp 开始时间: &nbsp; &nbsp;
                    <input type="text" id="txtMonth" style="text-align: center;" runat="server" readonly="readonly"
                        onfocus="WdatePicker({skin:'whyGreen',dateFmt:'yyyy-MM'})" class="Wdate" />
                </div>
                <div id="dv_year" style="margin-top: 5px; height: 30px;" runat="server">
                    &nbsp; &nbsp 开始时间: &nbsp; &nbsp;
                    <input type="text" id="txtYear" style="text-align: center;" runat="server" readonly="readonly"
                        onfocus="WdatePicker({skin:'whyGreen',dateFmt:'yyyy'})" class="Wdate" />
                </div>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a id="btnReckon" href="#"
                    class="easyui-linkbutton" data-options="iconCls:'icon-ok'">计&nbsp;&nbsp;算</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <a id="btnUp" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-ok'">保&nbsp;&nbsp;存</a>
            </td>
        </tr>
    </table>
    <div id="dv_body">
    </div>
</body>
</html>
