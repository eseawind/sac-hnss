<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PowerTrends.aspx.cs" Inherits="SACSIS.Tend.PowerTrends" %>

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
    <script language="javascript" type="text/javascript">
        var vars = new Array(), hash, Groupzl_id;

        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }

        var rating_time = "";

        $(document).ready(function () {
            $.post("PowerTrends.aspx", { id: vars["top_id"], Range: vars["id"], rating_time: rating_time }, function (data) {
                $("#dv_show").show();
                getLine(data);
                $("#dv_show").hide();
            }, 'json');
            Hc();

            $("#seach").click(function () {
                var rating_time = "";
                if (($("#stime").val() != "") && ($("#etime").val() != "")) {
                    rating_time = $("#stime").val() + "," + $("#etime").val();
                    $.post("PowerTrends.aspx", { id: vars["top_id"], Range: vars["id"], rating_time: rating_time }, function (data) {

                        
                        getLine(data);

                    }, 'json');
                }
                else {
                    alert("时间不能为空！");
                }
            });
        });

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
                text: '总功率趋势对比'
            },
            exporting: {
                enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
            },

            xAxis: {
                type: 'datetime',
                labels: { formatter: function () {
                    return Highcharts.dateFormat('%H:%M:%S', this.value);
                }
                }
            },
            colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
             yAxis: {
            title: {
                text: '负荷（MW）'
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
                crosshairs: {

                    width: 2,
                    color: 'red'
                },
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
                text: '总功率趋势对比'
            },
            xAxis: {
                type: 'datetime'
            },
            exporting: {
                enabled: false //用来设置是否显示‘打印’,'导出'等功能按钮，不设置时默认为显示 
            },
            yAxis: {
                title: {
                    text: '负荷（MW）'
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

        function search() {
            var rating_time = "";
            if (($("#stime").val() != "") && ($("#etime").val() != "")) {
                rating_time=$("#stime").val()+","+$("#etime").val();
                $.post("PowerTrends.aspx", { id: vars["top_id"], Range: vars["id"], rating_time: rating_time }, function (data) {

                    GetLine(data);
                }, 'json');
            }
            else
            {
            alert("时间不能为空！");
            }
        }
//        $(function () {
//            $('#container').highcharts({
//                chart: {
//                    type: 'spline'
//                    //column
//                },
//                title: {
//                    text: '总功率趋势'
//                },
//                subtitle: {
//                    text: ''
//                },
//                xAxis: {
//                    categories: [
//                    '1:00',
//                    '2:00',
//                    '3:00',
//                    '4:00',
//                    '5:00',
//                    '6:00',
//                    '7:00',
//                    '8:00',
//                    '9:00',
//                    '10:00',
//                    '11:00',
//                    '12:00'
//                ]
//                },
//                yAxis: {
//                    min: 0,
//                    title: {
//                        text: '功率(KW)'
//                    }
//                },
//                tooltip: {
//                   
//                    pointFormat: '{series.name}: <b>{point.y:.1f} KW</b>'
//                },
//                plotOptions: {
//                    column: {
//                        pointPadding: 0.2,
//                        borderWidth: 0
//                    }
//                },
//                exporting:{enabled:false},
//                series: [{
//                    name: '地面电站',
//                    data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]

//                }, {
//                    name: 'BAPV',
//                    data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3]

//                }, {
//                    name: 'BIPV',
//                    data: [48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3, 51.2]
//                }]
//            });
//        });
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table style="font-size: 12px;">
            <tr>
                <td>
                    起始时间：
                </td>
                <td>
                    <input id="stime" class="Wdate" style="text-align: center;" runat="server" readonly="readonly"
                        type="text" onclick="WdatePicker({maxDate:'#F{$dp.$D(\'etime\')||\'2020-10-01\'}',skin:'whyGreen',dateFmt:'yyyy-MM-dd HH:mm:ss'})" />&nbsp;
                </td>
                <td>
                    结束时间：
                </td>
                <td>
                    <input id="etime" class="Wdate" style="text-align: center;" runat="server" readonly="readonly"
                        type="text"  onclick="WdatePicker({maxDate:'#F{$dp.$D(\'etime\')||\'2020-10-01\'}',skin:'whyGreen',dateFmt:'yyyy-MM-dd HH:mm:ss'})" />&nbsp;
                </td>
                
                <td  align="left">
                <a id="seach" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'">查&nbsp;&nbsp;询</a>&nbsp;&nbsp;
                    
                </td>
            </tr>
        </table>
    </div>
    <div id="dv_show" style="text-align: center; margin-top: 200px;">
                <img src="../img/loading.gif" />
            </div>
    <div id="container" style="min-width: 400px; height: 400px; margin: 400px 0px 0px 0px auto">
    </div>
    </form>
</body>
</html>
