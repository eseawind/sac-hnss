<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Fb-zl.aspx.cs" Inherits="SACSIS.ProductOverview.fb_zl" %>

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
            background-color: #ffffff;
        }
        .div_1
        {
            background-image: url(../img/group-zl-1.jpg);
            height: 107px;
            width: 189px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
        }
        .div_2
        {
            background-image: url(../img/group-zl-2.jpg);
            height: 107px;
            width: 189px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
        }
        .div_3
        {
            background-image: url(../img/group-zl-3.jpg);
            height: 107px;
            width: 189px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
        }
        .div_4
        {
            background-image: url(../img/default_bg_1.jpg);
            height: 107px;
            width: 189px;
            position: relative;
            display: block;
        }
        .div_5
        {
            background-image: url(../img/group-zl-5.jpg);
            height: 107px;
            width: 189px;
            position: relative;
            display: block;
        }
        .div_6
        {
            background-image: url(../img/group-zl-6.jpg);
            height: 107px;
            width: 189px;
            position: relative;
            display: block;
        }
        .div_10
        {
            background-image: url(../img/group-zl-10.jpg);
            height: 256px;
            width: 414px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
        }
        .div_11
        {
            background-image: url(../img/group-zl-11.jpg);
            height: 256px;
            width: 414px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
        }
        .div_12
        {
            background-image: url(../img/group-zl-12.jpg);
            height: 421px;
            width: 556px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
        }
        .div_13
        {
            background-image: url(../img/group-zl-13.jpg);
            height: 421px;
            width: 556px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
        }
        .div_11
        {
            background-image: url(../img/group-zl-11.jpg);
            height: 256px;
            width: 414px;
            display: block;
            background-repeat: no-repeat;
            background-position: center;
        }
        .div_top_1
        {
            color: #000000;
            font-size: 14px;
            font: "宋体";
            position: absolute;
            top: 50px;
            left: 20px;
            width: 140px;
            height: 17px;
        }
        .text_1
        {
            color: #000000;
            font-size: 14px;
            font: "宋体";
            width: 140px;
            height: 17px;
            text-align: center;
        }
    </style>
    <script src="../Js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../Js/highcharts.js" type="text/javascript"></script>
    <script src="../Js/exporting.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var vars = new Array(), hash;
        $(document).ready(function () {


            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            var fbzl_id = "";
            if (vars["id"] != null) {
                fbzl_id += vars["id"] + ",";
            }
            else {
                fbzl_id += 1 + ",";
            }
            if (vars["company_id"] != null) {
                fbzl_id += vars["company_id"] + ",";
            }
            else {
                fbzl_id += ",";
            }
            if (vars["dep_id"] != null) {
                fbzl_id += vars["dep_id"];
            }
            $.post(
             "../datafile/Get_Groupzl_Data.aspx",
        {
            fb_id: fbzl_id
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
            $.post("Fb-zl.aspx", { Groupzl_id: "发电比重", id: "container1", Range: vars["id"] }, function (data) {

                DRAWPIE(data);
            }, 'json');
            $.post("Fb-zl.aspx", { Line_id: "实时负荷", id: "container2", Range: vars["id"] }, function (data) {

                DRAWLINE(data);
            }, 'json');
        });
        function Reaload() {
            $.post(
        "../datafile/Get_Groupzl_Data.aspx",
        {
            Reaload_id: "t_orgid='" + vars["company_id"] + "'", num: 1
        },
    function (data) {
        $("#lbl_Reaload").html(data);
    },
    "html");
        }


        var chart1;
        function DRAWPIE(data) {
            chart1 = new Highcharts.Chart({
                chart: {
                    type: 'pie',
                    renderTo: data.name,
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                title: {
                    text: data.title
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                exporting: {
                    enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            color: '#000000',
                            connectorColor: '#000000',
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                },
                series: data.list
            });
        }
        function DRAWLINE(data) {
            chart1 = new Highcharts.Chart({
                chart: {
                    type: 'column',
                    renderTo: 'container2'
                },
                title: {
                    text: '分公司实时负荷'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: ['实时负荷']
                },
                exporting: {
                    enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '负荷(kW)'
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.y:.1f}kW</b>'
                    //            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    //            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    //                    '<td style="padding:0"><b>{point.y:.1f} KW</b></td></tr>',
                    //            footerFormat: '</table>',
                    //            shared: true,
                    //            useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: data.list
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="background-image: url(../img/group-zl-0.jpg); background-repeat: repeat-x;
                    height: 8px;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div>
                        <table height="100%" width="100%" border="1" cellpadding="0" cellspacing="0" bordercolor="#FFFFFF">
                            <tr>
                                <td>
                                </td>
                                <td class="div_1" align="center" valign="middle">
                                    <div class="text_1">
                                        <asp:Label ID="lbl_Capacity" runat="server" Text="0"></asp:Label>MW
                                    </div>
                                </td>
                                <td>
                                </td>
                                <td class="div_2" align="center" valign="middle">
                                    <div class="text_1">
                                        <asp:Label ID="lbl_Reaload" runat="server" Text="0"></asp:Label>kW
                                    </div>
                                </td>
                                <td>
                                </td>
                                <td class="div_3" align="center" valign="middle">
                                    <div class="text_1">
                                        <asp:Label ID="lbl_Power" runat="server" Text="0"></asp:Label>kWh</div>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="background-image: url(../img/group-zl-9.jpg); height: 31px">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div>
                        <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
                            <%--                            <tr>
                                <td colspan="5" style="background-color: #FFFFFF; height: 50px;">
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="background-color: #ffffff; height: 256px;">
                                </td>
                                <td class="div_12">
                                     <div id="container1" style="width: 400px; height: 300px; margin: 0px auto auto 30px;">
                                    </div>
                                </td>
                                <td style="background-color: #ffffff; height: 256px; width: 90px">
                                </td>
                                <td class="div_13">
                                    <div id="container2" style="width: 400px; height: 300px; margin: 0px auto auto 30px;">
                                    </div>
                                </td>
                                <td style="background-color: #ffffff; height: 256px;">
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
