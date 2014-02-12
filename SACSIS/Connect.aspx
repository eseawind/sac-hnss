<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Connect.aspx.cs" Inherits="Web.Connect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>菜单</title>
    <link href="../css/update8.css" rel="stylesheet" type="text/css" />
    <link href="../css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <script src="Js/jQueryEasyUI/jquery-1.6.2.js" type="text/javascript"></script>
    <script src="Js/jQueryZtree/jquery.ztree.core-3.5.js" type="text/javascript"></script>
    <script src="Js/jQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
      <script type="text/javascript">
		<!--
          var setting = {
              data: {
                  key: {
                      title: "t"
                  },
                  simpleData: {
                      enable: true
                  }
              },
              callback: {
                  onClick: onClick
              }
          };
          function onClick(event, treeId, treeNode, clickFlag) {
              $("#fm_main").attr("src", treeNode.link);
          };
          var judge_1 = 1;
          var judge_2 = 1;
          var judge_3 = 1;
          var judge_4 = 1;
          var judge_5 = 1;
          var judge_6 = 1;
          var judge_7 = 1;
          var judge_8 = 1;
          var judge_9 = 1;
          var judge_10 = 1;
          var judge_11 = 1;
          var judge_12 = 1;
          var judge_13 = 1;
          var judge_14 = 1;
          var judge_15 = 1;
          var judge_16 = 1;
          var judge_17 = 1;
          var judge_18 = 1;
          var judge_19 = 1;
          var judge_20 = 1;
          var judge_21 = 1;

          //获取Url传递参数
          function GetRequest() {
              var url = location.search; //获取url中"?"符以及其后的字串  
              var theRequest = new Object();
              if (url.indexOf("?") != -1)//url中存在问号，也就说有参数。  
              {
                  var str = url.substr(1);
                  strs = str.split("&");
                  for (var i = 0; i < strs.length; i++) {
                      theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
                  }
              }
              return theRequest;
          }

          //获取Cookie
          function getCookie(name) {
              var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

              if (arr = document.cookie.match(reg))

                  return unescape(arr[2]);
              else
                  return null;
          }

          $(document).ready(function () {

              //初始化左侧菜单列表   
              var Request = new Object();
              Request = GetRequest();
              var id = Request['id'];

              var url = getCookie('url');

              if (url != '' && url != null && url != undefined) {
                  document.getElementById("fm_main").src = url;
              }

              leftTree(id);

              $("#dv_1").click(function () {
                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");


                  if (judge_1 == 1) {
                      $("#img_1").attr("src", "img/bg10.png");
                      $("#dv_1_1").show();
                      judge_1 = 0;
                  } else {
                      $("#img_1").attr("src", "img/bg09.png");
                      $("#dv_1_1").hide();
                      judge_1 = 1;
                  }

                  leftTreeNode($("#txt_1").val(), 'tree_1');
                  if ($("#img_1").is(":hidden") == true)
                      $("#dv_1_1").hide();

              });
              $("#dv_2").click(function () {

                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_2 == 1) {
                      $("#img_2").attr("src", "img/bg10.png");
                      $("#dv_2_1").show();
                      judge_2 = 0;
                  } else {
                      $("#img_2").attr("src", "img/bg09.png");
                      $("#dv_2_1").hide();
                      judge_2 = 1;
                  }

                  leftTreeNode($("#txt_2").val(), 'tree_2');
                  if ($("#img_2").is(":hidden") == true)
                      $("#dv_2_1").hide();
              });
              $("#dv_3").click(function () {

                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  if (judge_3 == 1) {
                      $("#img_3").attr("src", "img/bg10.png");
                      $("#dv_3_1").show();
                      judge_3 = 0;
                  } else {
                      $("#img_3").attr("src", "img/bg09.png");
                      $("#dv_3_1").hide();
                      judge_3 = 1;
                  }

                  leftTreeNode($("#txt_3").val(), 'tree_3');
                  if ($("#img_3").is(":hidden") == true)
                      $("#dv_3_1").hide();
              });

              $("#dv_4").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_4 == 1) {
                      $("#img_4").attr("src", "img/bg10.png");
                      $("#dv_4_1").show();
                      judge_4 = 0;
                  } else {
                      $("#img_4").attr("src", "img/bg09.png");
                      $("#dv_4_1").hide();
                      judge_4 = 1;
                  }

                  leftTreeNode($("#txt_4").val(), 'tree_4');
                  if ($("#img_4").is(":hidden") == true)
                      $("#dv_4_1").hide();
              });

              $("#dv_5").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_5 == 1) {
                      $("#img_5").attr("src", "img/bg10.png");
                      $("#dv_5_1").show();
                      judge_5 = 0;
                  } else {
                      $("#img_5").attr("src", "img/bg09.png");
                      $("#dv_5_1").hide();
                      judge_5 = 1;
                  }

                  leftTreeNode($("#txt_5").val(), 'tree_5');
                  if ($("#img_5").is(":hidden") == true)
                      $("#dv_5_1").hide();
              });

              $("#dv_6").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_6 == 1) {
                      $("#img_6").attr("src", "img/bg10.png");
                      $("#dv_6_1").show();
                      judge_6 = 0;
                  } else {
                      $("#img_6").attr("src", "img/bg09.png");
                      $("#dv_6_1").hide();
                      judge_6 = 1;
                  }

                  leftTreeNode($("#txt_6").val(), 'tree_6');
                  if ($("#img_6").is(":hidden") == true)
                      $("#dv_6_1").hide();
              });

              $("#dv_7").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_7 == 1) {
                      $("#img_7").attr("src", "img/bg10.png");
                      $("#dv_7_1").show();
                      judge_7 = 0;
                  } else {
                      $("#img_7").attr("src", "img/bg09.png");
                      $("#dv_7_1").hide();
                      judge_7 = 1;
                  }

                  leftTreeNode($("#txt_7").val(), 'tree_7');
                  if ($("#img_7").is(":hidden") == true)
                      $("#dv_7_1").hide();
              });


              $("#dv_8").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_8 == 1) {
                      $("#img_8").attr("src", "img/bg10.png");
                      $("#dv_8_1").show();
                      judge_8 = 0;
                  } else {
                      $("#img_8").attr("src", "img/bg09.png");
                      $("#dv_8_1").hide();
                      judge_8 = 1;
                  }

                  leftTreeNode($("#txt_8").val(), 'tree_8');
                  if ($("#img_8").is(":hidden") == true)
                      $("#dv_8_1").hide();
              });
              $("#dv_9").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_9 == 1) {
                      $("#img_9").attr("src", "img/bg10.png");
                      $("#dv_9_1").show();
                      judge_9 = 0;
                  } else {
                      $("#img_9").attr("src", "img/bg09.png");
                      $("#dv_9_1").hide();
                      judge_9 = 1;
                  }

                  leftTreeNode($("#txt_9").val(), 'tree_9');

                  if ($("#img_9").is(":hidden") == true)
                      $("#dv_9_1").hide();
              });
              $("#dv_10").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_10 == 1) {
                      $("#img_10").attr("src", "img/bg10.png");
                      $("#dv_10_1").show();
                      judge_10 = 0;
                  } else {
                      $("#img_10").attr("src", "img/bg09.png");
                      $("#dv_10_1").hide();
                      judge_10 = 1;
                  }

                  leftTreeNode($("#txt_10").val(), 'tree_10');

                  if ($("#img_10").is(":hidden") == true)
                      $("#dv_10_1").hide();
              });
              $("#dv_11").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_11 == 1) {
                      $("#img_11").attr("src", "img/bg10.png");
                      $("#dv_11_1").show();
                      judge_11 = 0;
                  } else {
                      $("#img_11").attr("src", "img/bg09.png");
                      $("#dv_11_1").hide();
                      judge_11 = 1;
                  }

                  leftTreeNode($("#txt_11").val(), 'tree_11');

                  if ($("#img_11").is(":hidden") == true)
                      $("#dv_11_1").hide();
              });
              $("#dv_12").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_12 == 1) {
                      $("#img_12").attr("src", "img/bg10.png");
                      $("#dv_12_1").show();
                      judge_12 = 0;
                  } else {
                      $("#img_12").attr("src", "img/bg09.png");
                      $("#dv_12_1").hide();
                      judge_12 = 1;
                  }

                  leftTreeNode($("#txt_12").val(), 'tree_12');

                  if ($("#img_12").is(":hidden") == true)
                      $("#dv_12_1").hide();
              });
              $("#dv_13").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_13 == 1) {
                      $("#img_13").attr("src", "img/bg10.png");
                      $("#dv_13_1").show();
                      judge_13 = 0;
                  } else {
                      $("#img_13").attr("src", "img/bg09.png");
                      $("#dv_13_1").hide();
                      judge_13 = 1;
                  }

                  leftTreeNode($("#txt_13").val(), 'tree_13');

                  if ($("#img_13").is(":hidden") == true)
                      $("#dv_13_1").hide();
              });
              $("#dv_14").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_14 == 1) {
                      $("#img_14").attr("src", "img/bg10.png");
                      $("#dv_14_1").show();
                      judge_14 = 0;
                  } else {
                      $("#img_14").attr("src", "img/bg09.png");
                      $("#dv_14_1").hide();
                      judge_14 = 1;
                  }

                  leftTreeNode($("#txt_14").val(), 'tree_14');

                  if ($("#img_14").is(":hidden") == true)
                      $("#dv_14_1").hide();
              });

              $("#dv_15").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_15 == 1) {
                      $("#img_15").attr("src", "img/bg10.png");
                      $("#dv_15_1").show();
                      judge_15 = 0;
                  } else {
                      $("#img_15").attr("src", "img/bg09.png");
                      $("#dv_15_1").hide();
                      judge_15 = 1;
                  }

                  leftTreeNode($("#txt_15").val(), 'tree_15');

                  if ($("#img_15").is(":hidden") == true)
                      $("#dv_15_1").hide();
              });

              $("#dv_16").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_16 == 1) {
                      $("#img_16").attr("src", "img/bg10.png");
                      $("#dv_16_1").show();
                      judge_16 = 0;
                  } else {
                      $("#img_16").attr("src", "img/bg09.png");
                      $("#dv_16_1").hide();
                      judge_16 = 1;
                  }

                  leftTreeNode($("#txt_16").val(), 'tree_16');

                  if ($("#img_16").is(":hidden") == true)
                      $("#dv_16_1").hide();
              });

              $("#dv_17").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_17 == 1) {
                      $("#img_17").attr("src", "img/bg10.png");
                      $("#dv_17_1").show();
                      judge_17 = 0;
                  } else {
                      $("#img_17").attr("src", "img/bg09.png");
                      $("#dv_17_1").hide();
                      judge_17 = 1;
                  }

                  leftTreeNode($("#txt_17").val(), 'tree_17');

                  if ($("#img_17").is(":hidden") == true)
                      $("#dv_17_1").hide();
              });

              $("#dv_18").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_18 == 1) {
                      $("#img_18").attr("src", "img/bg10.png");
                      $("#dv_18_1").show();
                      judge_18 = 0;
                  } else {
                      $("#img_18").attr("src", "img/bg09.png");
                      $("#dv_18_1").hide();
                      judge_18 = 1;
                  }

                  leftTreeNode($("#txt_18").val(), 'tree_18');

                  if ($("#img_18").is(":hidden") == true)
                      $("#dv_18_1").hide();
              });

              $("#dv_19").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  judge_20 = 1;
                  $("#dv_20_1").hide();
                  $("#img_20").attr("src", "img/bg09.png");

                  if (judge_19 == 1) {
                      $("#img_19").attr("src", "img/bg10.png");
                      $("#dv_19_1").show();
                      judge_19 = 0;
                  } else {
                      $("#img_19").attr("src", "img/bg09.png");
                      $("#dv_19_1").hide();
                      judge_19 = 1;
                  }

                  leftTreeNode($("#txt_19").val(), 'tree_19');

                  if ($("#img_19").is(":hidden") == true)
                      $("#dv_19_1").hide();
              });

              $("#dv_20").click(function () {
                  judge_1 = 1;
                  $("#dv_1_1").hide();
                  $("#img_1").attr("src", "img/bg09.png");

                  judge_2 = 1;
                  $("#dv_2_1").hide();
                  $("#img_2").attr("src", "img/bg09.png");

                  judge_3 = 1;
                  $("#dv_3_1").hide();
                  $("#img_3").attr("src", "img/bg09.png");

                  judge_4 = 1;
                  $("#dv_4_1").hide();
                  $("#img_4").attr("src", "img/bg09.png");

                  judge_5 = 1;
                  $("#dv_5_1").hide();
                  $("#img_5").attr("src", "img/bg09.png");

                  judge_6 = 1;
                  $("#dv_6_1").hide();
                  $("#img_6").attr("src", "img/bg09.png");

                  judge_7 = 1;
                  $("#dv_7_1").hide();
                  $("#img_7").attr("src", "img/bg09.png");

                  judge_8 = 1;
                  $("#dv_8_1").hide();
                  $("#img_8").attr("src", "img/bg09.png");

                  judge_10 = 1;
                  $("#dv_10_1").hide();
                  $("#img_10").attr("src", "img/bg09.png");

                  judge_11 = 1;
                  $("#dv_11_1").hide();
                  $("#img_11").attr("src", "img/bg09.png");

                  judge_12 = 1;
                  $("#dv_12_1").hide();
                  $("#img_12").attr("src", "img/bg09.png");

                  judge_13 = 1;
                  $("#dv_13_1").hide();
                  $("#img_13").attr("src", "img/bg09.png");

                  judge_14 = 1;
                  $("#dv_14_1").hide();
                  $("#img_14").attr("src", "img/bg09.png");

                  judge_15 = 1;
                  $("#dv_15_1").hide();
                  $("#img_15").attr("src", "img/bg09.png");

                  judge_16 = 1;
                  $("#dv_16_1").hide();
                  $("#img_16").attr("src", "img/bg09.png");

                  judge_17 = 1;
                  $("#dv_17_1").hide();
                  $("#img_17").attr("src", "img/bg09.png");

                  judge_18 = 1;
                  $("#dv_18_1").hide();
                  $("#img_18").attr("src", "img/bg09.png");

                  judge_19 = 1;
                  $("#dv_19_1").hide();
                  $("#img_19").attr("src", "img/bg09.png");

                  judge_9 = 1;
                  $("#dv_9_1").hide();
                  $("#img_9").attr("src", "img/bg09.png");

                  if (judge_20 == 1) {
                      $("#img_20").attr("src", "img/bg10.png");
                      $("#dv_20_1").show();
                      judge_20 = 0;
                  } else {
                      $("#img_20").attr("src", "img/bg09.png");
                      $("#dv_20_1").hide();
                      judge_20 = 1;
                  }

                  leftTreeNode($("#txt_20").val(), 'tree_20');

                  if ($("#img_20").is(":hidden") == true)
                      $("#dv_20_1").hide();
              });


              $("#dv_left").css("height", pageHeight() - 30);
              $("#dv_left").css("width", 182);

              $("#dv_right").css("height", pageHeight());

              $("#fm_main").css("height", pageHeight() - 2);
          });

          function leftTree(id) {
              $.post("Connect.aspx", { param: 'init', id: id }, function (data) {
                  var list = data.list;
                  for (var i = 0; i < list.length; i++) {
                      $("#sp_" + (i + 1)).html(list[i].NAME);
                      $("#txt_" + (i + 1)).val(list[i].ID);
                      $("#dv_" + (i + 1)).show();
                      if (list[i].JUDGE == '0') {
                          $("#img_" + (i + 1)).hide();
                      }
                  }
              }, 'json');
          }

          function leftTreeNode(id, TreeID) {
              $.post("Connect.aspx", { param: 'Tree', id: id }, function (data) {
                  var loadurl = data.url;
                  if (loadurl != '') document.getElementById("fm_main").src = loadurl;
                  var nodeEval = eval(data.treeNode);

                  $.fn.zTree.init($("#" + TreeID), setting, nodeEval);
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
		//-->
    </script>
</head>
<body class="easyui-layout">
    <div id="dv_left" region="west" border="true" split="true" title="功能菜单" class="cs-west"
        style="float: left; overflow: hidden; width: 182px; overflow: scroll">
        <input id="txt_1" style="display: none;" />
        <input id="txt_2" style="display: none;" />
        <input id="txt_3" style="display: none;" />
        <input id="txt_4" style="display: none;" />
        <input id="txt_5" style="display: none;" />
        <input id="txt_6" style="display: none;" />
        <input id="txt_7" style="display: none;" />
        <input id="txt_8" style="display: none;" />
        <input id="txt_9" style="display: none;" />
        <input id="txt_10" style="display: none;" />
        <input id="txt_11" style="display: none;" />
        <input id="txt_12" style="display: none;" />
        <input id="txt_13" style="display: none;" />
        <input id="txt_14" style="display: none;" />
        <input id="txt_15" style="display: none;" />
        <input id="txt_16" style="display: none;" />
        <input id="txt_17" style="display: none;" />
        <input id="txt_18" style="display: none;" />
        <input id="txt_19" style="display: none;" />
        <input id="txt_20" style="display: none;" />
        <div id="dv_1" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_1" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_1" class="box-title"></span><span
                id="sp_1_1" style="display: none;"></span>
        </div>
        <div id="dv_1_1" style="width: 182px; display: none;">
            <div id="dv_tree_1" class="zTreeDemoBackground left">
                <ul id="tree_1" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_2" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_2" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_2" class="box-title"></span><span
                id="sp_2_1" style="display: none;"></span>
        </div>
        <div id="dv_2_1" style="width: 182px; display: none;">
            <div id="dv_tree_2" class="zTreeDemoBackground left">
                <ul id="tree_2" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_3" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_3" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_3" class="box-title"></span><span
                id="sp_3_1" style="display: none;"></span>
        </div>
        <div id="dv_3_1" style="width: 182px; display: none;">
            <div id="dv_tree_3" class="zTreeDemoBackground left">
                <ul id="tree_3" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_4" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_4" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_4" class="box-title"></span><span
                id="sp_4_1" style="display: none;"></span>
        </div>
        <div id="dv_4_1" style="width: 182px; display: none;">
            <div id="dv_tree_4" class="zTreeDemoBackground left">
                <ul id="tree_4" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_5" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_5" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_5" class="box-title"></span><span
                id="sp_5_1" style="display: none;"></span>
        </div>
        <div id="dv_5_1" style="width: 182px; display: none;">
            <div id="dv_tree_5" class="zTreeDemoBackground left">
                <ul id="tree_5" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_6" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_6" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_6" class="box-title"></span><span
                id="sp_6_1" style="display: none;"></span>
        </div>
        <div id="dv_6_1" style="width: 182px; display: none;">
            <div id="dv_tree_6" class="zTreeDemoBackground left">
                <ul id="tree_6" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_7" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_7" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_7" class="box-title"></span><span
                id="sp_7_1" style="display: none;"></span>
        </div>
        <div id="dv_7_1" style="width: 182px; display: none;">
            <div id="dv_tree_7" class="zTreeDemoBackground left">
                <ul id="tree_7" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_8" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_8" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_8" class="box-title"></span><span
                id="sp_8_1" style="display: none;"></span>
        </div>
        <div id="dv_8_1" style="width: 182px; display: none;">
            <div id="dv_tree_8" class="zTreeDemoBackground left">
                <ul id="tree_8" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_9" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_9" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_9" class="box-title"></span><span
                id="sp_9_1" style="display: none;"></span>
        </div>
        <div id="dv_9_1" style="width: 182px; display: none;">
            <div id="dv_tree_9" class="zTreeDemoBackground left">
                <ul id="tree_9" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_10" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_10" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_10"
                class="box-title"></span><span id="sp_10_1" style="display: none;"></span>
        </div>
        <div id="dv_10_1" style="width: 182px; display: none;">
            <div id="dv_tree_10" class="zTreeDemoBackground left">
                <ul id="tree_10" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_11" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_11" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_11"
                class="box-title"></span><span id="sp_11_1" style="display: none;"></span>
        </div>
        <div id="dv_11_1" style="width: 182px; display: none;">
            <div id="dv_tree_11" class="zTreeDemoBackground left">
                <ul id="tree_11" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_12" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_12" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_12"
                class="box-title"></span><span id="sp_12_1" style="display: none;"></span>
        </div>
        <div id="dv_12_1" style="width: 182px; display: none;">
            <div id="dv_tree_12" class="zTreeDemoBackground left">
                <ul id="tree_12" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_13" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_13" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_13"
                class="box-title"></span><span id="sp_13_1" style="display: none;"></span>
        </div>
        <div id="dv_13_1" style="width: 182px; display: none;">
            <div id="dv_tree_13" class="zTreeDemoBackground left">
                <ul id="tree_13" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_14" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_14" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_14"
                class="box-title"></span><span id="sp_14_1" style="display: none;"></span>
        </div>
        <div id="dv_14_1" style="width: 182px; display: none;">
            <div id="dv_tree_14" class="zTreeDemoBackground left">
                <ul id="tree_14" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_15" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_15" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_15"
                class="box-title"></span><span id="sp_15_1" style="display: none;"></span>
        </div>
        <div id="dv_15_1" style="width: 182px; display: none;">
            <div id="dv_tree_15" class="zTreeDemoBackground left">
                <ul id="tree_15" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_16" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_16" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_16"
                class="box-title"></span><span id="sp_16_1" style="display: none;"></span>
        </div>
        <div id="dv_16_1" style="width: 182px; display: none;">
            <div id="dv_tree_16" class="zTreeDemoBackground left">
                <ul id="tree_16" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_17" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_17" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_17"
                class="box-title"></span><span id="sp_17_1" style="display: none;"></span>
        </div>
        <div id="dv_17_1" style="width: 182px; display: none;">
            <div id="dv_tree_17" class="zTreeDemoBackground left">
                <ul id="tree_17" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_18" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_18" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_18"
                class="box-title"></span><span id="sp_18_1" style="display: none;"></span>
        </div>
        <div id="dv_18_1" style="width: 182px; display: none;">
            <div id="dv_tree_18" class="zTreeDemoBackground left">
                <ul id="tree_18" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_19" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_19" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_19"
                class="box-title"></span><span id="sp_19_1" style="display: none;"></span>
        </div>
        <div id="dv_19_1" style="width: 182px; display: none;">
            <div id="dv_tree_19" class="zTreeDemoBackground left">
                <ul id="tree_19" class="ztree">
                </ul>
            </div>
        </div>
        <div id="dv_20" class="pnav-box" style="height: 30px; display: none;">
            <img id="img_20" style="margin-top: 10px;" src="img/bg09.png" /><span id="sp_20"
                class="box-title"></span><span id="sp_20_1" style="display: none;"></span>
        </div>
        <div id="dv_20_1" style="width: 182px; display: none;">
            <div id="dv_tree_20" class="zTreeDemoBackground left">
                <ul id="tree_20" class="ztree">
                </ul>
            </div>
        </div>
    </div>
    <div id="mainPanle" style="float: right;" region="center" border="true">
        <iframe width="100%" src="" id="fm_main" frameborder="0" name="fm_main"></iframe>
    </div>
</body>
</html>