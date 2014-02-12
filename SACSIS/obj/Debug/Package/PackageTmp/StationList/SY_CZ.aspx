<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SY_CZ.aspx.cs" Inherits="SACSIS.StationList.SY_CZ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <script src="../Js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="../Js/jQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/css/djxt.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Js/highcharts.js" type="text/javascript"></script>
    <script src="../Js/exporting.js" type="text/javascript"></script>
    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            height: 100%;
            overflow: auto;
            margin: 0px;
            padding: 0px;
            background-color: #ffffff;
        }
        .div_title1
        {
            background-image: url(../img/SY_CZ_bg_3.jpg);
            height: 52px;
            width: 30px;
        }
        .div_title2
        {
            background-image: url(../img/SY_CZ_bg_3.jpg);
            height: 52px;
            width: 500px;
            color: #db010f;
            font-size: 12px;
            font-weight: bold;
            line-height: 52px;
        }
        .div_title3
        {
            background-image: url(../img/SY_CZ_bg_3.jpg);
            height: 52px;
            width: 30px;
        }
        .button
        {
            background-image: url(../img/SY_CZ_btn_1.jpg);
            height: 38px;
            width: 65px;
            text-align: center;
            line-height: 38px;
            color: #FFFFFF;
            font-size: 12px;
            border: none;
            cursor: pointer;
        }
        .div_table_1
        {
            background-color: #ffffff;
            height: 35px;
            line-height: 35px;
            text-align: center;
            font-size: 14px;
        }
        .div_table_2
        {
            background-color: #e8e8e8;
            height: 35px;
            line-height: 35px;
            text-align: center;
            font-size: 14px;
        }
        
        .div_13
        {
            background-image: url(../img/group-zl-13.jpg);
            height: 421px;
            width: 556px;
            position: relative;
            display: block;
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
    </style>
    <script language="javascript" type="text/javascript">
        var vars = new Array(), hash, Groupzl_id;
        $(function () {
            

            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            Reload();
            //setInterval(Reload, "30000");
        });
        function Reload() {

            if (vars["plantid"] == "T_QHHNZ") {
                $("#dianzhan_img").attr("src", "../img/QHHNZ_img_1.jpg");
            }
            else if (vars["plantid"] == "T_NXWZ") {
                $("#dianzhan_img").attr("src", "../img/NXWZ_img_1.jpg");
            }

            $.post("SY_CZ.aspx", { plant_id: vars["plantid"] }, function (data) {
                //                                fz  //辐照
                //                                sd  //湿度
                //                                wd  //温度
                //                                fs  //风速

                $("#rl").html(data.rl);
                //alert(data.fj_count);
                $("#nbq").html(data.fj_count);
                $("#div_quyu").html(data.tb);
                $("#fshe").html(data.fz);
                $("#wd").html(data.wd);
                $("#sd").html(data.sd);
                $("#fs").html(data.fs);
                $("#gl").html(data.gl);
                var glb = $.parseJSON(data.glb);
                // initGL($.parseJSON(data.glb), "div4");
                var day = $.parseJSON(data.day);
                var rz = $.parseJSON(data.rz);
                initGL(glb.gl, "div4");
                initGL(day.day, "div5");
                initGL(rz.rz, "div6");
                $("#dlD").html(data.d_dl);
                $("#dlM").html(data.y_dl);
                if (vars["plantid"] != "T_QHHNZ") {
                    initChart("aa");
                }
            }, 'json');
        }

       
        var chart1;
        var initGL = function (gl,div_id) {
            //var dataJson = $.parseJSON(gl);
            chart1 = new Highcharts.Chart({
                chart: {
                    renderTo: div_id,
                    type: 'bar',
                    backgroundColor: '#e7ebef',
                    plotBackgroundColor: '#f3f6f8',
                    spacingTop: 0,            //图表上方的空白(好用)
                    spacingRight: 5,
                    spacingBottom: 0,
                    spacingLeft: 5
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: ['']
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: ''
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                exporting: {
                    enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
                },
                series: [{
                    data: gl
                }]
            });
        }
        var initChart = function (tagPower) {
            var dataJson;
            var ss;
            var pName = tagPower;

            $.ajax({
                url: "../WebService/WebService.asmx/GetFHByTimeGF", //GetFHByTimeByFourMin /10000
                contentType: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: "{'pName':'T_GZJSQ'}", // 
                beforeSend: function () {
                },
                success: function (json) {
                    dataJson = $.parseJSON(json.d);
                    SetChartData();
                },
                error: function (x, e) {
                    alert(x.responseText);
                },
                complete: function () {
                }
            });
            var SetChartData = function () {
                var chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'div8',
                        type: 'spline',
                        backgroundColor: '#e7ebef',
                        plotBackgroundColor: '#f3f6f8'
                    },
                    credits: {
                        enabled: false
                    },
                    title: {
                        text: '',
                        style: {
                            fontFamily: '"微软雅黑"',
                            fontSize: '14pt'
                        }
                    },
                    subtitle: {
                        enable: false
                    },
                    xAxis: {
                        type: 'datetime',
                        dateTimeLabelFormats: {
                            day: '00:00'
                        },
                        maxZoom: 2 * 3600 * 1000,
                        showFirstLabel: true,
                        showLastLabel: true,
                        tickWidth: 0,
                        gridLineWidth: 1

                    },
                    yAxis: {
                        title: {
                            text: ''
                        },
                        labels: {
                            formatter: function () {
                                return this.value;
                            }
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        crosshairs: {
                            width: 1,
                            color: 'red'
                        },
                        shared: true,
                        xDateFormat: '<b>时间：%H:%M:%S</b>'
                    },
                    plotOptions: {
                        spline: {
                            lineWidth: 5,
                            marker: {
                                enabled: false,
                                radius: 4,
                                lineColor: '#666666',
                                lineWidth: 1
                            }
                        },
                        enabled: false
                    },
                    series: [{
                        name: '发电功率',
                        lineWidth: 2,
                        marker: {
                            symbol: 'circle'
                        },
                        color: 'green',
                        data: dataJson.fh,
                        pointStart: Date.UTC(2012, 3, 26),
                        pointInterval: 3600 * 1000 //3600 * 1000 //5min
                    }],
                    exporting: {
                        enabled: false
                    }
                });
            };
        }
        function query(id) {
            location.href = "SBLB.aspx?id=" + id; //location.href实现客户端页面的跳转 
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
        <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2" style="background-image: url(../img/SY_CZ_bg_1.jpg); height: 5px;">
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <div>
                        <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" class="div_title1">
                                    <img alt="" src="../img/SY_CZ_bg_2.jpg" />
                                </td>
                                <td align="left" valign="middle" class="div_title2">
                                    &nbsp;场站信息
                                </td>
                                <td align="right" class="div_title3">
                                    <img alt="" src="../img/SY_CZ_bg_4.jpg" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center" valign="middle">
                                     <div id="div_quyu">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center" valign="middle" height="290px">
                                    <img src="../img/SY_CZ_img_1.jpg" id="dianzhan_img" alt="" />
                                    <%--<input id="dianzhan_img" name="" type="image" alt="" src="../img/SY_CZ_img_1.jpg" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <div>
                                        <table height="100%" width="80%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="div_table_1" align="right">
                                                    电站容量：
                                                </td>
                                                <td class="div_table_1">
                                                    <span id="rl"></span>&nbsp;&nbsp;&nbsp;MW</td>
                                            </tr>
                                            <tr>
                                                <td class="div_table_2" align="right">
                                                    逆变器数量：
                                                </td>
                                                <td class="div_table_2">
                                                    <span id="nbq"></span>&nbsp;个
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="div_table_1" align="right">
                                                    辐照度：
                                                </td>
                                                <td class="div_table_1" align="right">
                                                    <span id="fshe"></span>&nbsp;&nbsp;&nbsp;W/M2
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="div_table_2" align="right">
                                                    环境温度：
                                                </td>
                                                <td class="div_table_2">
                                                    <span id="wd"></span>&nbsp;&nbsp;&nbsp;℃
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="div_table_1" align="right">
                                                    环境湿度：
                                                </td>
                                                <td class="div_table_1">
                                                    <span id="sd"></span>&nbsp;&nbsp;&nbsp;%
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="div_table_2" align="right">
                                                    风速：
                                                </td>
                                                <td class="div_table_2">
                                                    <span id="fs"></span>&nbsp;&nbsp;&nbsp;m/s
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td align="center" valign="top">
                    <div>
                        <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" class="div_title1">
                                    <img alt="" src="../img/SY_CZ_bg_2.jpg" />
                                </td>
                                <td align="left" valign="middle" class="div_title2">
                                    &nbsp;实时数据
                                </td>
                                <td align="right" class="div_title3">
                                    <img alt="" src="../img/SY_CZ_bg_4.jpg" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div>
                                        <table border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    当前发电功率:&nbsp;<span id="gl"></span>&nbsp;kW
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFFF;">
                                                    <div id="div4" style="height: 50px; width: 520px;">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    今日发电量:&nbsp;<span id="dlD"></span> &nbsp;kWh</td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFFF;">
                                                    <div id="div5" style="height: 50px; width: 520px;">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    累计发电量:&nbsp;<span id="dlM"></span>&nbsp;kWh</td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFFF;">
                                                    <div id="div6" style="height: 50px; width: 520px;">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #FFFFFF;">
                                                    <div id="div7" style="height: 50px; width: 520px;">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="div_title1">
                                    <img alt="" src="../img/SY_CZ_bg_2.jpg" />
                                </td>
                                <td align="left" valign="middle" class="div_title2">
                                    &nbsp;发电功率曲线
                                </td>
                                <td align="right" class="div_title3">
                                    <img alt="" src="../img/SY_CZ_bg_4.jpg" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <div id="div8" style="height: 210px; width: 520px;">
                                    </div>
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
