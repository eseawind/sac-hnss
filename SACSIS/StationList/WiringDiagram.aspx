<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WiringDiagram.aspx.cs"
    Inherits="SACSIS.StationList.WiringDiagram" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Js/jQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/demo/demo.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../Js/jQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            Init();
            $("#NBQ1").click(function () {
                location.href = "GZJSQNBQ.aspx?id=KL-A2(3)_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ2").click(function () {
                location.href = "GZJSQNBQ.aspx?id=KL-A2(4)_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ3").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-A2_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ4").click(function () {
                location.href = "GZJSQNBQ.aspx?id=KL-A3(5)_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ5").click(function () {
                location.href = "GZJSQNBQ.aspx?id=KL-A3(6)_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ6").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-A3_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ7").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-A2_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ8").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-A4_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ9").click(function () {
                location.href = "GZJSQNBQ.aspx?id=KL-A6_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ10").click(function () {
                location.href = "GZJSQNBQ.aspx?id=KL-A6(2)_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ11").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-A6_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ12").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-A7_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ13").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-A8_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ14").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-C1_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ15").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-C2_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ16").click(function () {
                location.href = "GZJSQNBQ.aspx?id=KL-C3(7)_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ17").click(function () {
                location.href = "GZJSQNBQ.aspx?id=KL-C3(8)_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ18").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-C4_"; //location.href实现客户端页面的跳转 
            });
            $("#NBQ19").click(function () {
                location.href = "GZJSQNBQ.aspx?id=YG-C5_"; //location.href实现客户端页面的跳转 
            });
        });
        

        function Init() {
            $.post("WiringDiagram.aspx", { plant_id: "para" }, function (data) {
                var array = new Array();
                array = data.split(',');
                $("#lbl_a2_zyg").html(array[0]);
                $("#lbl_a3_zyg").html(array[1]);
                $("#lbl_a4_zyg").html(array[2]);
                $("#lbl_a2_fz").html(array[3]);
                $("#lbl_a3_fz").html(array[4]);
                $("#lbl_a4_fz").html(array[5]);
                $("#lbl_a2_pz").html(array[6]);
                $("#lbl_a3_pz").html(array[7]);
                $("#lbl_a4_pz").html(array[8]);
                $("#lbl_a2_gz").html(array[9]);
                $("#lbl_a3_gz").html(array[10]);
                $("#lbl_a4_gz").html(array[11]);
                $("#nbq_a2_1rfdl").html(array[12]);
                $("#nbq_a2_1nfdl").html(array[13]);
                $("#nbq_a2_2rfdl").html(array[14]);
                $("#nbq_a2_2nfdl").html(array[15]);
                $("#nbq_a2_3rfdl").html(array[16]);
                $("#nbq_a2_3nfdl").html(array[17]);
                $("#nbq_a3_1rfdl").html(array[18]);
                $("#nbq_a3_1nfdl").html(array[19]);
                $("#nbq_a3_2rfdl").html(array[20]);
                $("#nbq_a3_2nfdl").html(array[21]);
                $("#nbq_a3_3rfdl").html(array[22]);
                $("#nbq_a3_3nfdl").html(array[23]);
                $("#Label1").html(array[24]);
                $("#Label2").html(array[25]);
                $("#Label3").html(array[26]);
                $("#Label7").html(array[27]);
                $("#Label11").html(array[28]);
                $("#Label4").html(array[29]);
                $("#Label8").html(array[30]);
                $("#Label12").html(array[31]);
                $("#Label5").html(array[32]);
                $("#Label9").html(array[33]);
                $("#Label13").html(array[34]);
                $("#Label6").html(array[35]);
                $("#Label10").html(array[36]);
                $("#Label14").html(array[37]);
                $("#Label15").html(array[38]);
                $("#Label16").html(array[39]);
                $("#Label17").html(array[40]);
                $("#Label18").html(array[41]);
                $("#Label19").html(array[42]);
                $("#Label20").html(array[43]);
                $("#Label21").html(array[44]);
                $("#Label22").html(array[45]);
                $("#Label23").html(array[46]);
                $("#Label24").html(array[47]);

                $("#Label25").html(array[48]);
                $("#Label29").html(array[49]);
                $("#Label26").html(array[50]);
                $("#Label30").html(array[51]);
                $("#Label27").html(array[52]);
                $("#Label31").html(array[53]);
                $("#Label28").html(array[54]);
                $("#Label32").html(array[55]);
                $("#Label33").html(array[56]);
                $("#Label34").html(array[57]);
                $("#Label35").html(array[58]);
                $("#Label36").html(array[59]);

                $("#Label37").html(array[60]);
                $("#Label49").html(array[61]);
                $("#Label41").html(array[62]);
                $("#Label38").html(array[63]);
                $("#Label50").html(array[64]);
                $("#Label42").html(array[65]);

                $("#Label39").html(array[66]);
                $("#Label51").html(array[67]);
                $("#Label43").html(array[68]);
                $("#Label40").html(array[69]);
                $("#Label52").html(array[70]);
                $("#Label44").html(array[71]);
                $("#Label45").html(array[72]);
                $("#Label46").html(array[73]);
                $("#Label53").html(array[74]);
                $("#Label54").html(array[75]);
                $("#Label55").html(array[76]);
                $("#Label56").html(array[77]);

                $("#Label57").html(array[78]);
                $("#Label58").html(array[79]);
                $("#Label47").html(array[80]);
                $("#Label48").html(array[81]);
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="easyui-tabs" style="width: 1100px; height: auto">
        <div title="A2-A4" style="padding: 10px">
            <div style="padding: 0px; margin: -10px 0px 0px 0px; width: auto; height: 530px;background:url(../img/GZJSQ_A1.jpg)">
                
                <div id="divbg" style="left: 0px; top:0px; position: absolute;">
                    <div id="PDG_A2_1" style="left: 380px; top: 130px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a2_zyg" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A2_2" style="left: 380px; top: 160px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a2_fz" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A2_3" style="left: 380px; top: 180px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a2_pz" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A2_4" style="left: 380px; top: 210px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a2_gz" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A3_1" style="left: 700px; top: 130px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a3_zyg" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A3_2" style="left: 700px; top: 160px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a3_fz" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A3_3" style="left: 700px; top: 180px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a3_pz" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A3_4" style="left: 700px; top: 210px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a3_gz" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A4_1" style="left: 1020px; top: 130px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a4_zyg" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A4_2" style="left: 1020px; top: 160px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a4_fz" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A4_3" style="left: 1020px; top: 180px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a4_pz" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="PDG_A4_4" style="left: 1020px; top: 210px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="lbl_a4_gz" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="NBQ1" style="left: 80px; top: 360px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ2" style="left: 190px; top: 360px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ3" style="left: 340px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ4" style="left: 450px; top: 360px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ5" style="left: 570px; top: 360px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ6" style="left: 720px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ7" style="left: 900px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="Div1" style="left: 120px; top: 440px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="nbq_a2_1rfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div2" style="left: 120px; top: 480px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="nbq_a2_1nfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div3" style="left: 230px; top: 440px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="nbq_a2_2rfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div4" style="left: 240px; top: 480px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="nbq_a2_2nfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div5" style="left: 380px; top: 370px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="nbq_a2_3rfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div6" style="left: 380px; top: 410px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="nbq_a2_3nfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div7" style="left: 490px; top: 440px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="nbq_a3_1rfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div8" style="left: 490px; top: 480px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="nbq_a3_1nfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div9" style="left: 600px; top: 440px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="nbq_a3_2rfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div10" style="left: 600px; top: 480px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="nbq_a3_2nfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div11" style="left: 760px; top: 360px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="nbq_a3_3rfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div12" style="left: 760px; top: 400px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="nbq_a3_3nfdl" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div13" style="left: 960px; top: 360px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label1" runat="server" Text="" Height="18px" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div14" style="left: 960px; top: 400px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label2" runat="server" Text="" Height="18px" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div title="A5-A8" style="padding: 10px">
            <div style="padding: 0px; margin: -8px 0px 0px 0px; width: auto; height: 520px;background:url(../img/GZJSQ2.jpg)">
               
                <div id="div15" style="left: 0px; top: 0px; position: absolute;">
                    <div id="Div16" style="left: 340px; top: 130px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label3" runat="server" Text="" Height="18px" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div17" style="left: 340px; top: 160px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label4" runat="server" Text="" Height="18px" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div18" style="left: 340px; top: 180px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label5" runat="server" Text="" Height="18px" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div19" style="left: 340px; top: 210px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label6" runat="server" Text="" Height="18px" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div20" style="left: 700px; top: 130px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label7" runat="server" Text="" Height="18px" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div21" style="left: 700px; top: 160px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label8" runat="server" Text="" Height="18px" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div22" style="left: 700px; top: 180px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label9" runat="server" Text="" Height="18px" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div23" style="left: 700px; top: 210px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label10" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div24" style="left: 990px; top: 130px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label11" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div25" style="left: 990px; top: 160px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label12" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div26" style="left: 990px; top: 180px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label13" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div27" style="left: 990px; top: 210px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label14" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="NBQ8" style="left: 80px; top: 360px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ9" style="left: 180px; top: 360px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ10" style="left: 310px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ11" style="left: 580px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ12" style="left: 870px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="Div35" style="left: 130px; top: 450px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label15" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div36" style="left: 130px; top: 490px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label16" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div37" style="left: 220px; top: 450px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label17" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div38" style="left: 220px; top: 490px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label18" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div39" style="left: 380px; top: 370px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label19" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div40" style="left: 380px; top: 410px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label20" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div41" style="left: 640px; top: 360px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label21" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div42" style="left: 640px; top: 400px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label22" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div43" style="left: 930px; top: 370px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label23" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div44" style="left: 930px; top: 420px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label24" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div title="C1-C2" style="padding: 10px">
            <div style="padding: 0px; margin: -8px; width: auto; height: 520px;background:url(../img/GZJSQ3.jpg)">
                <div id="div45" style="left: 0px; top: 0px; position: absolute;">
                    <div id="Div48" style="left: 330px; top: 140px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label25" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div49" style="left: 330px; top: 170px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label26" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div50" style="left: 330px; top: 190px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label27" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div51" style="left: 330px; top: 220px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label28" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div52" style="left: 800px; top: 140px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label29" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div53" style="left: 800px; top: 170px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label30" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div54" style="left: 800px; top: 190px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label31" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div55" style="left: 800px; top: 220px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label32" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="NBQ13" style="left: 210px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ14" style="left: 680px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="Div58" style="left: 280px; top: 360px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label33" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div59" style="left: 280px; top: 410px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label34" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div60" style="left: 740px; top: 370px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label35" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div61" style="left: 740px; top: 410px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label36" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div title="C3-C5" style="padding: 10px">
            <div style="padding: 0px; margin: -8px; width: auto; height: 520px;background:url(../img/GZJSQ4.jpg)"">
                <div id="div62" style="left: 00px; top: 00px; position: absolute;">
                    <div id="Div63" style="left: 250px; top: 140px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label37" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div64" style="left: 250px; top: 170px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label38" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div65" style="left: 250px; top: 190px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label39" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div66" style="left: 250px; top: 220px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label40" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div67" style="left: 860px; top: 130px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label41" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div68" style="left: 860px; top: 160px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label42" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div69" style="left: 860px; top: 180px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label43" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div70" style="left: 860px; top: 210px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label44" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div77" style="left: 550px; top: 130px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label49" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div78" style="left: 550px; top: 160px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label50" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div79" style="left: 550px; top: 180px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label51" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div80" style="left: 550px; top: 210px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label52" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="NBQ16" style="left: 140px; top: 370px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ19" style="left: 750px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ17" style="left: 250px; top: 370px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="NBQ18" style="left: 430px; top: 350px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="Div73" style="left: 90px; top: 460px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="Label45" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div74" style="left: 90px; top: 500px; position: absolute; width: 87px; height: 18px;">
                        <asp:Label ID="Label46" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div82" style="left: 190px; top: 460px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label53" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div83" style="left: 190px; top: 500px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label54" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div84" style="left: 290px; top: 460px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label55" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div85" style="left: 290px; top: 500px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label56" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="NBQ15" style="left: 50px; top: 370px; position: absolute; cursor: pointer;
                        width: 60px; height: 60px;">
                    </div>
                    <div id="Div75" style="left: 810px; top: 370px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label47" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div76" style="left: 810px; top: 410px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label48" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div86" style="left: 480px; top: 370px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label57" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                    <div id="Div87" style="left: 480px; top: 410px; position: absolute; width: 87px;
                        height: 18px;">
                        <asp:Label ID="Label58" runat="server" Text="" Height="18px" BorderStyle="Solid"
                            BorderWidth="1"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
