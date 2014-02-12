<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageMenu.aspx.cs" Inherits="SACSIS.Admin.ManageMenu" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单管理</title>
    <link href="../Js/jQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jQueryEasyUI/css/djxt.css" rel="stylesheet" type="text/css" />
    <link href="../css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jQueryEasyUI/jquery-1.6.2.js" type="text/javascript"></script>
    <script src="../Js/jQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Js/jQueryZtree/jquery.ztree.core-3.5.js" type="text/javascript"></script>
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
            //UDate(treeNode.id);
            //var oid = treeNode.pId;
            //alert(treeNode.owner);
            $('#nPid').val(treeNode.pId);
            //alert($('#nPid').val());
            $('#nId').val(treeNode.id);
            $('#nName').val(treeNode.name);
            $('#nOrder').val(treeNode.order);
            $('#nUrl').val(treeNode.FileName);
            if (treeNode.visible == "1") {
                $('#nVis').attr("checked", true);
            }
            else {
                $('#nVis').attr("checked", false);
            }
        };
        var ids;
        $(function () {
            $('#dv_add').hide();
            $('#dv_edit').hide();
            $('#nPid').hide();
            load();
        });
        function UDate(id) {
            var url = "<%=blackUrl %>";
            if (url.indexOf('?') >= 0) {
                $(window.parent.document).find("#txt_orgId").val(id);
                $("#iframe_con", parent.document.body).attr("src", url + "&orgID=" + id);
            } else {
                $(window.parent.document).find("#txt_orgId").val(id);
                $("#iframe_con", parent.document.body).attr("src", url + "?orgID=" + id);
            }
        }

        function load() {
            Tree("");
        }

        function Tree(id) {
            $.post("ManageMenu.aspx", { param: 'showTree', id: id }, function (data) {
                var nodeEval = eval(data.menu);
                $.fn.zTree.init($("#tree"), setting, nodeEval);
            }, 'json');
        };

        function AddShow() {
            if ($('#nId').val() == '') {
                $.messager.alert('添加菜单信息', '请为将要添加的菜单选择一个父菜单', 'info');
            }
            else {
                $('#dv_add').show();
                $("#dv_add").attr('title', '添加菜单');
                var dlg = $('#dv_add').dialog({
            });
            dlg.parent().appendTo(jQuery("form:first"));
        }
    }

    function Add() {
        //alert($('#txtVis').attr("checked"));
        var pId = $('#nId').val();
        var name = $('#txtName').val();
        var order = $('#txtOrder').val();
        var url = $('#txtUrl').val();
        var vis = '0';
        if ($('#txtVis').attr("checked") == "checked") {
            vis = '1';
        }
        //alert(vis);
        if (name == '') {
            $.messager.alert('添加菜单信息', '请至少填写菜单名称', 'info');
        }
        else {
            $('#dv_add').dialog('close');
            $.post("ManageMenu.aspx", { param: 'Add', pId: pId, name: name, order: order, url: url, vis: vis }, function (data) {
                $.messager.alert('添加菜单信息', data.message, 'info');
                $('#txtName').val('');
                $('#txtOrder').val('');
                $('#txtUrl').val('');
                $('#txtVis').attr("checked", true);
                //主页面的文本框清空
                $('#nPid').val('');
                $('#nId').val('');
                $('#nName').val('');
                $('#nOrder').val('');
                $('#nUrl').val('');
                $('#nVis').attr("checked", false);
                Tree(pId);
            }, 'json');
        }
    }

    function EditShow() {
        if ($('#nId').val() == '') {
            $.messager.alert('编辑菜单信息', '请选择需要编辑的菜单', 'info');
        }
        else {
            $('#dv_edit').show();
            $("#dv_edit").attr('title', '编辑菜单');
            $('#eName').val($('#nName').val());
            $('#eOrder').val($('#nOrder').val());
            $('#eUrl').val($('#nUrl').val());
            if ($('#nVis').attr("checked") == "checked") {
                $('#eVis').attr("checked", true);
            }
            else {
                $('#eVis').attr("checked", false);
            }
            var dlg = $('#dv_edit').dialog({
        });
        dlg.parent().appendTo(jQuery("form:first"));
    }
}

function Edit() {
    //alert($('#nId').val());
    var pId = $('#nPid').val();
    var id = $('#nId').val();
    var name = $('#eName').val();
    var order = $('#eOrder').val();
    var url = $('#eUrl').val();
    var vis = '0';
    if ($('#eVis').attr("checked") == "checked") {
        vis = '1';
    }
    //alert(vis);
    if (name == '') {
        $.messager.alert('编辑菜单信息', '请至少填写菜单名称', 'info');
    }
    else {
        $('#dv_edit').dialog('close');
        $.post("ManageMenu.aspx", { param: 'Edit', pId:pId, id: id, name: name, order: order, url: url, vis: vis }, function (data) {
            $.messager.alert('编辑菜单信息', data.message, 'info');
            //主页面的文本框清空
            $('#nPid').val('');
            $('#nId').val('');
            $('#nName').val('');
            $('#nOrder').val('');
            $('#nUrl').val('');
            $('#nVis').attr("checked", false);
            Tree(id);
        }, 'json');
    }
}

function Del() {
        var pId = $('#nPid').val();
        var id = $('#nId').val();
        if (id == '') {
            $.messager.alert('删除菜单信息', '请选择需要删除的菜单', 'info');
        }
        else {
            $.post("ManageMenu.aspx", { param: 'Del', id: id }, function (data) {
                $.messager.alert('删除菜单信息', data.message, 'info');
                //主页面的文本框清空
                $('#nPid').val('');
                $('#nId').val('');
                $('#nName').val('');
                $('#nOrder').val('');
                $('#nUrl').val('');
                $('#nVis').attr("checked", false);
                Tree(pId);
            }, 'json');
        }
    }

        function Cancel() {
            $('#dv_add').dialog('close');
        }

        function Cancel2() {
            $('#dv_edit').dialog('close');
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
<body>
    <form id="form1" runat="server">
    <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr><td colspan="3" class="title">&nbsp;菜单管理</td></tr>
    <tr>
		<td valign="middle" class="style1">&nbsp;菜单列表</td>
		<td class="style2"><img alt="" src="../img/table-head-3.jpg" /></td>
		<td valign="middle" class="style3">&nbsp;导入菜单</td>
	</tr>
    <tr><td align="left" valign="top" width="300px">
    <div id="dv_tree" class="zTreeDemoBackground left" style="overflow:auto;height:100%; ">
        <ul id="tree" class="ztree" style="width: 300px;">
        </ul>
    </div>
    </td>
    <td class="style5"></td>
    <td >
      <div>
          <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
             <tr><td class="style6"></td>
	         <td  align="left" valign="middle" class="style7">
		                     导入菜单文件:<asp:FileUpload ID="FileUpload1" runat="server"/>
				             <asp:Button ID="BtnPutin" runat="server" Text="上传" OnClick="BtnPutin_Click" CssClass="button"/>		    
	         </td>
             </tr>
             <tr>
	            <td colspan="2" valign="middle" class="style3">&nbsp;操作选项</td>
	        </tr>
            <tr height="40px">
                <td class="style6"></td>
		        <td colspan="2"  align="left" class="style7">&nbsp;&nbsp;
                      <input type="button" id="BtnAdd" value="添加" onclick="AddShow()" class="button" />
                      <input type="button" id="BtnEdit" value="修改" onclick="EditShow()" class="button" />
	                  <input type="button" id="ButDel" value="删除" onclick="Del()" class="button" />
                </td>
	        </tr>
            <tr height="30px">
                <td class="style6"></td>
		        <td  align="left" valign="middle" class="style7">
		              菜单编号:<input id="nId" type="text" runat="server" size="35" disabled="disabled" style="border: solid 1px #E0ECF9"/><input id="nPid" type="text" runat="server" size="5"/>
		        </td>
	         </tr>            
             <tr height="30px">
	            <td class="style6"></td>
		        <td  align="left" valign="middle"  class="style7">
		              菜单名称:<input id="nName" type="text" runat="server" size="35" disabled="disabled" style="border: solid 1px #E0ECF9"/>
		        </td>
	         </tr>
             <tr height="30px">
                <td class="style6"></td>
		        <td  align="left" valign="middle" class="style7">
		              菜单序号:<input id="nOrder" type="text" runat="server" size="35" disabled="disabled" style="border: solid 1px #E0ECF9"/>
		        </td>
	         </tr>
             <tr height="30px">
	            <td class="style6"></td>
		        <td  align="left" valign="middle"  class="style7">
		              菜单链接:<input id="nUrl" type="text" runat="server" size="35" disabled="disabled" style="border: solid 1px #E0ECF9"/>
		        </td>
	         </tr>
             <tr height="30px">
	            <td class="style6"></td>
		        <td  align="left" valign="middle"  class="style7">
		              显示菜单:<input type="checkbox" runat="server" ID="nVis" runat="server" disabled="disabled" style="border: solid 1px #E0ECF9"/>
		        </td>
	         </tr>
             <tr>
	            <td colspan="2" style="background-color:#f2f5f7" align="center" valign="middle" ></td>
	        </tr>
            
          </table>
      </div>
    </td>
    </tr>
    </table>

     <div id="dv_add" data-options="iconCls:'icon-save'" style="padding: 5px; width: 400px; background-color:#f2f5f7;
        height: 200px;">          

            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 菜单名称:&nbsp;
            <input id="txtName" class="easyui-validatebox" type="text" name="name" runat="server" style="border: solid 1px #E0ECF9; text-align: center;" size="35" /><br/>

            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 菜单序号:&nbsp;
            <input id="txtOrder" class="easyui-validatebox" type="text" name="name" runat="server" style="border: solid 1px #E0ECF9; text-align: center;" size="35" /><br/>

            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 菜单链接:&nbsp;
            <input id="txtUrl" class="easyui-validatebox" type="text" name="name" runat="server" style="border: solid 1px #E0ECF9; text-align: center;" size="35"/><br/>

            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 显示菜单:&nbsp;
            <input type="checkbox" id="txtVis" name="txtVis" checked="checked"  runat="server"/><br/><br/>
            
              &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
              <input id="ButAdd" type="button" runat="server" value="保存" onclick="Add()" class="button"/>
              <input id="ButCal" type="button" runat="server" value="取消" onclick="Cancel()" class="button"/>
              
    </div>

    <div id="dv_edit" data-options="iconCls:'icon-save'" style="padding: 5px; width: 400px; background-color:#f2f5f7;
        height: 200px;">          

            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 菜单名称:&nbsp;
            <input id="eName" class="easyui-validatebox" type="text" name="name" runat="server" style="border: solid 1px #E0ECF9; text-align: center;" size="35" /><br/>

            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 菜单序号:&nbsp;
            <input id="eOrder" class="easyui-validatebox" type="text" name="name" runat="server" style="border: solid 1px #E0ECF9; text-align: center;" size="35" /><br/>

            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 菜单链接:&nbsp;
            <input id="eUrl" class="easyui-validatebox" type="text" name="name" runat="server" style="border: solid 1px #E0ECF9; text-align: center;" size="35"/><br/>

            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; 显示菜单:&nbsp;
            <input type="checkbox" id="eVis" name="eVis" runat="server"/><br/><br/>
            
              &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
              <input id="ButEdit" type="button" runat="server" value="保存" onclick="Edit()" class="button"/>
              <input id="ButCal2" type="button" runat="server" value="取消" onclick="Cancel2()" class="button"/>
              
    </div>
    </form>
</body>
</html>
