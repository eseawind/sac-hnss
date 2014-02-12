<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyConsumption.aspx.cs"
    Inherits="SACSIS.Tend.DailyConsumption" %>


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
    <script language="javascript" type="text/javascript">
        $(function () {
            var vars = new Array(), hash, Groupzl_id;

            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            if (vars["id"] == "1") {
                $("#tb_day").css('display', 'block'); 
                $("#tb_mon").css('display', 'none');
                $("#tb_year").css('display', 'none'); 
            }
            else if (vars["id"] == "2") {
                $("#tb_day").css('display', 'none');
                $("#tb_mon").css('display', 'block'); 
                $("#tb_year").css('display', 'none'); 
            }
            else {
                $("#tb_day").css('display', 'none');
                $("#tb_mon").css('display', 'none');
                $("#tb_year").css('display', 'block'); 
            }
        });

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
        var rating_time = "";

        $(document).ready(function () {
            var vars = new Array(), hash, Groupzl_id;

            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            $.post("DailyConsumption.aspx", { id: vars["top_id"], Range: vars["id"], rating_time: rating_time }, function (data) {
                $("#dv_show").show();
                if (vars["id"] == "1") {
                    getLine(data);
                }
                else if (vars["id"] == "2") {
                    getLineByMonth(data);
                }
                else {
                    getLineByYear(data);
                }
                $("#dv_show").hide();
            }, 'json');
            Hc();

            $("#seach").click(function () {
                var rating_time = "";

                if (($("#stime").val() != "") && ($("#etime").val() != "")) {
                    if (vars["id"] == "1") {
                        rating_time = $("#stime").val() + "," + $("#etime").val();
                    }
                    else if (vars["id"] == "2") {
                        rating_time = $("#stime1").val() + "," + $("#etime1").val();
                    }
                    else {
                        rating_time = $("#stime2").val();
                    }
                    $.post("DailyConsumption.aspx", { id: vars["top_id"], Range: vars["id"], rating_time: rating_time }, function (data) {


                        getLine(data);

                    }, 'json');
                }
                else {
                    alert("时间不能为空！");
                }
            });
        });
        function query() {
            if ($("#stime").val != "" && $("#etime").val() != "") {
                rating_time = $("#stime").val + "," + $("#etime").val();
                $.post("DailyConsumption.aspx", { id: vars["top_id"], Range: vars["id"], rating_time: rating_time }, function (data) {

                    getLine(data);
                }, 'json');
            }
            else {
                alert("请选择时间！");
            }
        }

        function getLineByMonth(list) {
            var highchartsOptions = Highcharts.setOptions(Highcharts.theme);
            if (list != null) {
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        type: 'spline',
                        zoomType: 'x'
                    },
                    title: {
                        text: '发电量趋势'
                    },
                    exporting: {
                        enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
                    },

                    xAxis: {
                        categories: list.x_data
                    },
                    colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
                    yAxis: {
                        title: {
                            text: '月发电量（kWh）'
                        },
                        labels: {
                            formatter: function () {
                                return this.value;
                            }
                        },
                        min: 0
                    },
                    tooltip: {
                        xDateFormat: '<b>' + '%Y-%m-%d %H:%M:%S' + '</b>',
                        shared: true
                        //                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        //                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        //                    '<td style="padding:0"><b>{point.y:.1f} KWH</b></td></tr>',
                        //                        footerFormat: '</table>',
                        //                        shared: true,
                        //                        useHTML: true
                    },
                    plotOptions: {
                        spline: {
                            lineWidth: 0.4,
                            states: {
                                hover: {
                                    lineWidth: 0.5
                                }
                            },
                            marker: {
                                enabled: false
                            }
                        }
                    },
                    series: list.list
                });
            }
            else {
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        type: 'spline'
                    },
                    title: {
                        text: '月发电量趋势'
                    },
                    exporting: {
                        enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
                    },
                    yAxis: {
                        title: {
                            text: '发电量（kWh）'
                        },
                        min: 0
                    }
                });
            }
        }
        function getLineByYear(list) {
            var highchartsOptions = Highcharts.setOptions(Highcharts.theme);
            if (list != null) {
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        type: 'column',
                        zoomType: 'x'
                    },
                    title: {
                        text: '发电量趋势'
                    },
                    exporting: {
                        enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
                    },

                    xAxis: {
                        categories: list.x_data
                    },
                    colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
                    yAxis: {
                        title: {
                            text: '年发电量（kWh）'
                        },
                        labels: {
                            formatter: function () {
                                return this.value;
                            }
                        },
                        min: 0
                    },
                    tooltip: {

                        xDateFormat: '<b>' + '%Y-%m-%d %H:%M:%S' + '</b>',
                        shared: true
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0
                        }
                    },
                    series: list.list
                });
            }
            else {
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        type: 'column'
                    },
                    title: {
                        text: '发电量趋势'
                    },
                    exporting: {
                        enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
                    },
                    yAxis: {
                        title: {
                            text: '年发电量（kWh）'
                        },
                        min: 0
                    }
                });
            }
        }

        function getLine(list) {
            var highchartsOptions = Highcharts.setOptions(Highcharts.theme);
            if (list != null) {
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        type: 'spline',
                        zoomType: 'x'
                    },
                    title: {
                        text: '发电量趋势'
                    },
                    exporting: {
                        enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
                    },

                    xAxis: {
                        categories: list.x_data
                    },
                    colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
                    yAxis: {
                        title: {
                            text: '发电量（kWh）'
                        },
                        labels: {
                            formatter: function () {
                                return this.value;
                            }
                        },
                        min: 0
                    },
                    tooltip: {
                        xDateFormat: '<b>' + '%Y-%m-%d %H:%M:%S' + '</b>',
                        shared: true
                    },
                    plotOptions: {
                        spline: {
                            lineWidth: 0.4,
                            states: {
                                hover: {
                                    lineWidth: 0.5
                                }
                            },
                            marker: {
                                enabled: false
                            }
                        }
                    },
                    series: list.list
                });
            }
            else {
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        type: 'spline'
                    },
                    title: {
                        text: '日发电量趋势'
                    },
                    exporting: {
                        enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
                    },
                    yAxis: {
                        title: {
                            text: '发电量（kWh）'
                        },
                        min: 0
                    }
                });
            }
        }
        function Hc() {
            var chart;
            Highcharts.theme = {
                colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
                chart: {
                    backgroundColor: {
                        linearGradient: [0, 0, 500, 500],
                        stops: [
                    [0, 'rgb(255, 255, 255)'],
                    [1, 'rgb(240, 240, 255)']
                 ]
                    },
                    borderWidth: 2,
                    plotBackgroundColor: 'rgba(255, 255, 255, .9)',
                    plotShadow: true,
                    plotBorderWidth: 1
                },
                title: {
                    style: {
                        color: '#000',
                        font: 'bold 16px "Trebuchet MS", Verdana, sans-serif'
                    }
                },
                //                global: { useUTC: false },
                //highcharts 时间问题
                //highcharts 中默认开启了UTC（世界标准时间），由于中国所在时区为+8，所以经过highcharts的处理后会减去8个小时。如果不想使用UTC，可以进行如下设置。

                subtitle: {
                    style: {
                        color: '#666666',
                        font: 'bold 12px "Trebuchet MS", Verdana, sans-serif'
                    }
                },
                xAxis: {
                    gridLineWidth: 1,
                    lineColor: '#000',
                    tickColor: '#000',
                    labels: {
                        style: {
                            color: '#000',
                            font: '11px Trebuchet MS, Verdana, sans-serif'
                        }
                    },
                    title: {
                        style: {
                            color: '#333',
                            fontWeight: 'bold',
                            fontSize: '12px',
                            fontFamily: 'Trebuchet MS, Verdana, sans-serif'

                        }
                    }
                },
                yAxis: {
                    minorTickInterval: 'auto',
                    lineColor: '#000',
                    lineWidth: 1,
                    tickWidth: 1,
                    tickColor: '#000',
                    labels: {
                        style: {
                            color: '#000',
                            font: '11px Trebuchet MS, Verdana, sans-serif'
                        }
                    },
                    title: {
                        style: {
                            color: '#333',
                            fontWeight: 'bold',
                            fontSize: '12px',
                            fontFamily: 'Trebuchet MS, Verdana, sans-serif'
                        }
                    }
                },
                legend: {
                    itemStyle: {
                        font: '9pt Trebuchet MS, Verdana, sans-serif',
                        color: 'black'

                    },
                    itemHoverStyle: {
                        color: '#039'
                    },
                    itemHiddenStyle: {
                        color: 'gray'
                    }
                },
                labels: {
                    style: {
                        color: '#99b'
                    }
                }
            };
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="font-size: 12px;">
            <tr>
                <td>
                    <table id="tb_day"">
                        <tr>
                            <td>
                                起始时间：
                            </td>
                            <td>
                                <input id="stime" class="Wdate" style="text-align: center;" runat="server" readonly="readonly"
                                    type="text" onclick="WdatePicker({maxDate:'#F{$dp.$D(\'etime\')||\'2020-10-01\'}',skin:'whyGreen',dateFmt:'yyyy-MM-dd'})" />&nbsp;
                            </td>
                            <td>
                                结束时间：
                            </td>
                            <td>
                                <input id="etime" class="Wdate" style="text-align: center;" runat="server" readonly="readonly"
                                    type="text" onclick="WdatePicker({maxDate:'#F{$dp.$D(\'etime\')||\'2020-10-01\'}',skin:'whyGreen',dateFmt:'yyyy-MM-dd'})" />&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table id="tb_mon" style="display: none">
                        <tr>
                            <td>
                                起始时间：
                            </td>
                            <td>
                                <input id="stime1" class="Wdate" style="text-align: center;" runat="server" readonly="readonly"
                                    type="text" onclick="WdatePicker({maxDate:'#F{$dp.$D(\'etime\')||\'2020-10-01\'}',skin:'whyGreen',dateFmt:'yyyy-MM'})" />&nbsp;
                            </td>
                            <td>
                                结束时间：
                            </td>
                            <td>
                                <input id="etime1" class="Wdate" style="text-align: center;" runat="server" readonly="readonly"
                                    type="text" onclick="WdatePicker({maxDate:'#F{$dp.$D(\'etime\')||\'2020-10-01\'}',skin:'whyGreen',dateFmt:'yyyy-MM'})" />&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table id="tb_year" style="display: none">
                        <tr>
                            <td>
                                日期：
                            </td>
                            <td>
                                <input id="stime2" class="Wdate" style="text-align: center;" runat="server" readonly="readonly"
                                    type="text" onclick="WdatePicker({maxDate:'#F{$dp.$D(\'etime\')||\'2020-10-01\'}',skin:'whyGreen',dateFmt:'yyyy'})" />&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="left">
                    <a id="seach" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'"
                        >查&nbsp;&nbsp;询</a>&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="dv_show" style="text-align: center; margin-top: 200px;">
                <img src="../img/loading.gif" />
            </div>
    <div id="container" style="min-width: 400px; height: 400px; margin: 0px 0px 0px 0px auto">
    </div>
    </form>
</body>
</html>
