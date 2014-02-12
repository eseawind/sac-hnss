<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePower.aspx.cs" Inherits="SACSIS.Admin.ManagePower" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>权限管理</title>
    <link href="../Js/jQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/css/djxt.css" rel="stylesheet" type="text/css" />
    <link href="../css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jQueryEasyUI/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Js/jQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Js/jQueryZtree/jquery.ztree.core-3.5.js" type="text/javascript"></script>
    <script src="../Js/jQueryZtree/jquery.ztree.excheck-3.5.js" type="text/javascript"></script>
    <style type="text/css">
     body
     {
     	height:100%; 
     	overflow:auto; 
     	margin:0px; 
     	padding:0px;
     }
     .style1
        {
            background-image:url(../img/table-head-2.jpg);
            height:25px;
            font-size:13px;     
            color:Black;
            width:300px;
        }
        .style2
        {
            background-image:url(../img/table-head-3.jpg);
            height:25px;
            width:1px;
        }
        .style3
        {
            background-image:url(../img/table-head-2.jpg);
            height:25px;
            font-size:13px;     
            color:Black;
        }
        .style5
        {
            background-image:url(../img/table-head-3.jpg);
            width:1px;
        }
        .style6
        {
            width: 50px;
            background-color:#f2f5f7;
        }
        .style7
        {
            font-size: 13px;
            height:30px;
            background-color:#f2f5f7;
        }
        .button
        {
        width:76px;  /*图片宽带*/
        background:url(../img/button.jpg) no-repeat left top;  /*图片路径*/
        border:none;  /*去掉边框*/
        height:24px; /*图片高度*/
        color:Black;
        vertical-align: middle;
        text-align:center
        }
       .title
       {
       	background-image:url(../img/table-head.jpg);
       	height:30px;
       	text-align:30px;
        	font-size:13px;
        	color:#0a4869;
        	font-weight:bold;
       }
    </style>
    <script type="text/javascript">
    
		<!--
		var btn_num_add=1;
		var btn_num_remove=1;
		var btn_num_save=1;
		var setting = {
			data: {
				key: {
					title:"t"
				},
				simpleData: {
					enable: true
				}
			},
			callback: {
				onClick: onClick
			}
        };
        var menusetting = {
            check: {  
                enable: true  //是否启用 复选框  
            },
            data: {
                key: {
                    title: "t"
                },
                simpleData: {
                    enable: true
                }
            }          
        };
		function onClick(event, treeId, treeNode, clickFlag) {
		     $("#txtRoleName").val(treeNode.name);//右侧表格显示组织名称
		     $("#txtRoleId").val(treeNode.id);
		     if(treeNode.t =="存在人员")
		     {
		         ShowMenuTree(treeNode.name, treeNode.id);
		     }
		     else {
		         $.messager.alert('添加菜单信息', '此角色下不存在用户，无法分配权限，请为该角色添加用户！', 'info');
		         $("#txtRoleName").val('尚未选择任何角色');
		     }
		}
		$(document).ready(function () {
		    $('#txtRoleId').hide();
		    ShowRolesTree();
		});

		function ShowRolesTree() {
		    $.post("ManagePower.aspx", { param: 'LoadRoles' }, function (data) {
		        var nodeEval = eval(data.menu);
		        $.fn.zTree.init($("#rolesTree"), setting, nodeEval);
		    }, 'json');
		}
		function ShowMenuTree(rName, rId) {
		    $.post("ManagePower.aspx", { param: 'LoadMenu', rName:rName, rId:rId}, function (data) {
		        var nodeEval = eval(data.menu);
		        $.fn.zTree.init($("#MenuTree"), menusetting, nodeEval);
		    }, 'json');
		}

		function getChecked() {
		    var treeObj = $.fn.zTree.getZTreeObj("MenuTree");
		    var nodes = treeObj.getCheckedNodes(true);
		    //var nodes = $('#MenuTree').tree('getChecked');
		    var s = '';
		    for (var i = 0; i < nodes.length; i++) {
		        if (s != '') s += ',';
		        s += nodes[i].id;
		        //s += nodes[i].name;
		    }
		    //alert(s);
		    var rName = $("#txtRoleName").val();
		    var rId = $("#txtRoleId").val();
		    $.post("ManagePower.aspx", { param: 'SaveMenu', rId: rId, mId: s }, function (data) {
		        $.messager.alert('保存权限信息', data.message, 'info');
		        ShowMenuTree(rName, rId);
		    }, 'json');
		}

		function cancel() {
		    ShowRolesTree();
		    var nodeEval = "";
		    $.fn.zTree.init($("#MenuTree"), menusetting, nodeEval);
		    $("#txtRoleName").val('尚未选择任何角色');
		}
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr><td colspan="3" class="title">&nbsp;权限管理</td></tr>
    <tr>
		<td valign="middle" class="style1">&nbsp;角色列表</td>
		<td class="style2"><img alt="" src="../img/table-head-3.jpg" /></td>
		<td valign="middle" class="style3">&nbsp;角色名称</td>
	</tr>
    <tr>
        <td align="left" valign="top" width="300px">
            <ul id="rolesTree" class="ztree"></ul>           
        </td>
        <td class="style5"></td>
        <td>
           <div>
              <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
                 <tr style=" height:40px">
                    <td style="background-color:#f2f5f7" align="center">
                        <input id="txtRoleName" type="text" runat="server" size="35" value="尚未选择任何角色" disabled="disabled" style="border: solid 0px #E0ECF9; background-color:#f2f5f7; text-align:center; color:#000000;"/>
                        <input id="txtRoleId" type="text" runat="server" size="5" />
                    </td>
                 </tr>
                 <tr>
                    <td valign="middle" class="style3">&nbsp;操作选项</td>
                 </tr>
                 <tr style=" height:40px">
		            <td style="background-color:#f2f5f7" align="center">
		                <input type="button" id="BtnSave" value="确定分配" onclick="getChecked()" class="button" />&nbsp;&nbsp;
                        <input type="button" id="Button1" value="取消" onclick="cancel()" class="button" />
		            </td>
	             </tr>
                 <tr>
                    <td valign="middle" class="style3">&nbsp;菜单列表</td>
                 </tr>
                 <tr>
                    <td align="left" valign="top">
                        <div id="dv_tree" class="zTreeDemoBackground left" style="overflow:auto;height:100%; ">
                            <ul id="MenuTree" class="ztree">
                            </ul>
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
