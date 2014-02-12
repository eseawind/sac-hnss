<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageRoleUser.aspx.cs" Inherits="SACSIS.Admin.ManageRoleUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色用户管理</title>
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
        var ihight;
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

        $(document).ready(function () {
            ihight = pageHeight();
            $('#dv_add').hide();
            $('#dv_edit').hide();
            $('#txtRoleId').hide();
            ShowRolesTree();
            Grid("");
        });

        function ShowRolesTree() {
            $.post("ManageRoleUser.aspx", { param: 'LoadRoles' }, function (data) {
                var nodeEval = eval(data.menu);
                $.fn.zTree.init($("#rolesTree"), setting, nodeEval);
            }, 'json');
        }

        function onClick(event, treeId, treeNode, clickFlag) {
            $("#txtRoleName").val(treeNode.name); //右侧表格显示组织名称
            $("#txtRoleId").val(treeNode.id);
            roleId = treeNode.id;
            Grid(treeNode.id);  
        }

        function Grid(id) {
            $('#grid').datagrid({
                title: '人员列表',
                iconCls: 'icon-search',
                nowrap: true,
                border: false,
                autoRowHeight: false,
                striped: true,
                height: ihight - 80,
                align: 'center',
                collapsible: true,
                url: 'ManageRoleUser.aspx',
                sortName: 'ID_KEY',
                sortOrder: 'asc',
                remoteSort: false,
                queryParams: { param: 'seachList', id: id, name: '' },
                idField: 'id',
                frozenColumns: [[
                { field: 'ck', checkbox: true }
			    ]],
                columns: [[
				    { field: 'id', title: '用户名', width: 120, align: 'center' },
				    { field: 'name', title: '真实姓名', width: 150, align: 'center' }
			    ]],
                pagination: true,
                rownumbers: true,
                toolbar: [{
                    id: 'btnadd',
                    text: '添加人员',
                    iconCls: 'icon-add',
                    handler: function () {
                        $('#dv_add').show();
                        AddShow(id);
                    }
                },
//                {
//                    id: 'btnedit',
//                    text: '编辑人员',
//                    iconCls: 'icon-edit',
//                    handler: function () {
//                        var rows = $('#grid').datagrid('getSelections');
//                        var id = "";
//                        $.each(rows, function (i, n) {
//                            id += "" + n.id + ",";
//                        });
//                        id = id.substring(0, id.length - 1);
//                        //var idlist = new Array();
//                        //idlist = id.split(",");
//                        //var idlength = idlist.length;
//                        //$('#txtHide').val(idlist[idlength - 1]);
//                        //EditShow(idlist[idlength - 1]);
//                        $('#txtHide').val(id);
//                        EditShow(id);
//                    }
//                },
		        {
		            id: 'btnremove',
		            text: '删除人员',
		            iconCls: 'icon-remove',
		            handler: function () {
		                var rows = $('#grid').datagrid('getSelections');
		                var id = "";
		                var name = "";
		                $.each(rows, function (i, n) {
		                    id += "'" + n.id + "',";
		                    name += n.name + ',';
		                });
		                name = name.substring(0, name.length - 1);
		                id = id.substring(0, id.length - 1);
                        //var idlist = new Array();
                        //idlist = id.split(",");
                        //var idlength = idlist.length;
                        //$.messager.confirm('删除人员信息', '你确定要删除 ' + idlist[idlength - 1] + '  吗?', function (ok) {
                        $.messager.confirm('删除人员信息', '你确定要删除 ' + id + '  吗?', function (ok) {
		                    if (ok) {
		                        Remove(id);
		                    } else {
		                        //$.messager.alert('删除人员信息', '删除已取消!', 'info');
		                    }
		                });
		            }
		        }],
                onDblClickRow: function (rowIndex, rowData) {
                    $('#grid').datagrid('clearSelections');
                    $('#txtHide').val(rowData.id);
                    EditShow(rowData.id);
                }
            });
        }

        function AddShow(id) {
            if ($('#txtRoleId').val() == "" || $('#txtRoleName').val() == "") {
                $.messager.alert('添加人员信息', '请选择一个组织机构!', 'info');
            }
            else {
                $("#dv_add").attr('title', '添加人员');
                $('#txtUserID').val('');
                $('#txtUserName').val('');
                $('#txtPwd').val('');
                $('#txtPwd2').val('');
                $('#dv_add').show();
                $('#dv_add').dialog({
                    buttons: [{
                        text: '添加',
                        iconCls: 'icon-ok',
                        handler: function () {
                            Add($('#txtUserID').val(), $('#txtUserName').val(), $('#txtPwd').val(), $('#txtPwd2').val());
                        }
                    }, {
                        text: '重置',
                        iconCls: 'icon-no',
                        handler: function () {
                            $('#dv_add').dialog('close');
                        }
                    }]
                });
            }
        }
        function Add(id, name, pwd) {
            if ($("#txtUserID").val() == "" || $("#txtUserID").val() == null || escape($("#txtUserName").val()) == null || escape($("#txtUserName").val()) == "" || escape($("#txtPwd").val()) == null || escape($("#txtPwd").val()) == "" || escape($("#txtPwd2").val()) == null || escape($("#txtPwd2").val()) == "") {
                $("#Par").hide();
                $("#slclass").hide();
                $.messager.alert('添加人员信息', '用户ID、用户姓名、密码都不能为空!', 'info');
                $("#Par").show();
                $("#slclass").show();
            } else {
            $.post("ManageRoleUser.aspx", { param: 'JudgeMember', userID: escape($("#txtUserID").val()) }, function (data) {
                if (Number(data.judge) == 1) {
                    $('#txtPwd').val('');
                    $('#txtPwd2').val('');
                    $.messager.alert('添加人员信息', '已经存在用户名为：' + $("#txtUserID").val() + '的人员,不能重复添加!', 'error');
                }
                else if ($("#txtPwd").val() != $("#txtPwd2").val()) {
                    $.messager.alert('添加人员信息', '密码输入不一致，请重新输入密码', 'error');
                }
                else {
                    $.post("ManageRoleUser.aspx", { param: 'AddMember', userID: $("#txtUserID").val(), userName: escape($("#txtUserName").val()), pwd: escape($("#txtPwd").val()),
                        img: escape($("#flImg").val()), par: $("#Par").val(), treeNodeId: $('#txtRoleId').val()
                    }, function (data) {
                        //Tree($('#txtID').val());
                        //$('#dv_add').hide();
                        $('#dv_add').dialog('close');
                        $.messager.alert('添加人员信息', data.info, 'info');
                        Grid($('#txtRoleId').val())
                    }, 'json');
                }
            }, 'json');
            }
        }
        function EditShow(id) {
            $.post("ManageRoleUser.aspx", { param: 'Edit', id: id }, function (data) {
                var list = data.list;
                if (data.img.length > 0) {
                    $("#img_").show();
                    $("#img_").attr("src", data.img);
                } else {
                    $("#img_").hide();
                }
                $("#txtUserIDEdit").val(list[0].T_USERID);
                $("#txtUserNameEdit").val(list[0].T_USERNAME);
                $("#txtPwdEdit").val(list[0].T_PASSWD);
                $("#txtPwd2Edit").val(list[0].T_PASSWD);
                var count = 0;
            }, 'json');

            $("#dv_edit").attr('title', '编辑人员信息');
            $('#dv_edit').show();
            $('#dv_edit').dialog({
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-ok',
                    handler: function () {
                        Edit();
                    }
                }, {
                    text: '取 消',
                    handler: function () {
                        $('#dv_edit').dialog('close');
                    }
                }]
            });
        }
        function Edit() {
            if ($("#txtUserIDEdit").val() == null || $("#txtUserIDEdit").val() == '' || $("#txtUserNameEdit").val() == null || $("#txtUserNameEdit").val() == '' || $("#txtPwdEdit").val() == null || $("#txtPwdEdit").val() == '' || $("#txtPwd2Edit").val() == null || $("#txtPwd2Edit").val() == '') {
                $.messager.alert('编辑人员信息', '编辑人员信息时：用户ID、用户姓名和密码都不能为空!', 'info');
            } else {
                if ($("#txtHide").val() == $("#txtUserIDEdit").val()) {
                    if ($("#txtPwd").val() != $("#txtPwd2").val()) {
                        $.messager.alert('添加人员信息', '密码输入不一致，请重新输入密码', 'error');
                    }
                    else {
                        $.post("ManageRoleUser.aspx", { param: 'EditMemberInfo', oldId: $("#txtHide").val(), id: $("#txtUserIDEdit").val(), name: escape($("#txtUserNameEdit").val()),
                            pwd: escape($("#txtPwdEdit").val()), img: escape($("#flImgEdit").val()), treeNodeId: $('#txtRoleId').val()
                        }, function (data) {
                            Grid($('#txtRoleId').val())
                            $('#dv_edit').dialog('close');
                            $.messager.alert('编辑人员信息', data.info, 'info');
                        }, 'json');
                    }
                } else {
                    $.post("ManageRoleUser.aspx", { param: 'JudgeMember', userID: $("#txtUserIDEdit").val() }, function (data) {
                        if (Number(data.judge) == 1) {
                            $.messager.alert('编辑人员信息', '已经存在用户ID为：' + $("#txtUserIDEdit").val() + '  的人员,用户名不能重复!', 'error');
                        } else {
                            $.post("ManageRoleUser.aspx", { param: 'EditMemberInfo', oldId: $("#txtHide").val(), id: $("#txtUserIDEdit").val(), pwd: escape($("#txtPwdEdit").val()),
                                name: escape($("#txtUserNameEdit").val()), img: escape($("#flImgEdit").val()), treeNodeId: $('#txtRoleId').val()
                            }, function (data) {
                                $('#dv_edit').dialog('close');
                                $.messager.alert('编辑人员信息', data.info, 'info');
                                Grid($('#txtRoleId').val())
                            }, 'json');
                        }
                    }, 'json');
                }
            }
        }
        function Remove(id) {
            $.post("ManageRoleUser.aspx", { param: 'Remove', id: id }, function (data) {
                Grid($('#txtRoleId').val())
                $.messager.alert('删除人员信息', data.info, 'info');
            }, 'json');
        }
        </script>
<script type="text/javascript">
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
    <tr><td colspan="3" class="title">&nbsp;角色用户管理</td></tr>
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
               <table height="100%" width="100%" border="0" cellpadding="0" cellspacing="0"> 
               <tr style=" height:40px"><td style="background-color:#ffffff" align="center">
                   <input id="txtRoleName" type="text" runat="server" size="35" value="尚未选择任何角色" disabled="disabled" style="border: solid 0px #E0ECF9; background-color:#ffffff; text-align:center; color:#000000;"/>
                        <input id="txtRoleId" type="text" runat="server" size="5" />
               </td></tr>
               <tr><td style=" height:1px; background-color:#99bbe8;"></td></tr>
               <tr><td><div><table id="grid" width="100%" height="100%"></table></div></td></tr>
               </table>
           </div>
        </td>
    </tr>
    </table>
    </div>
    </form>

    <div id="dv_add" data-options="iconCls:'icon-save'" style="padding: 5px; width: 700px;
        height: 280px;">
        <div id="dv_Member_info">
            <table class="admintable" width="100%">
                <tr>
                    <th class="adminth" colspan="4">
                        添加人员信息
                    </th>
                </tr>
                <tr>
                    <td class="admincls0" align="center">
                        用户ID
                    </td>
                    <td class="admincls0">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="txtUserID" type="text" />
                    </td>
                    <td class="admincls0" align="center">
                        用户姓名
                    </td>
                    <td class="admincls0">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="txtUserName" type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="admincls1" align="center">
                        用户密码
                    </td>
                    <td class="admincls1">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="txtPwd" type="password" />
                    </td>
                    <td class="admincls1" align="center">
                        确认密码
                    </td>
                    <td class="admincls1">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="txtPwd2" type="password" />
                    </td>
                </tr>
                <tr>
                    <td class="admincls0" align="center">
                        照片
                    </td>
                    <td class="admincls0" colspan="3">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="flImg" type="file" style="width: 280px;" /></td>
                </tr>
            </table>
        </div>
    </div>
    <div id="dv_edit" style="padding: 5px; width: 700px; height: 360px;">
        <div id="dv_Member_Info_Edit">
            <table class="admintable" width="100%">
                <tr>
                    <th class="adminth" colspan="4">
                        编辑人员信息
                    </th>
                </tr>
                <tr>
                    <td class="admincls0" align="center">
                        用户ID
                    </td>
                    <td class="admincls0">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="txtUserIDEdit" type="text" />
                    </td>
                    <td class="admincls0" align="center">
                        用户姓名
                    </td>
                    <td class="admincls0">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="txtUserNameEdit" type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="admincls1" align="center">
                        用户密码
                    </td>
                    <td class="admincls1">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="txtPwdEdit" type="password" />
                    </td>
                    <td class="admincls1" align="center">
                        确认密码
                    </td>
                    <td class="admincls1">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="txtPwd2Edit" type="password" />
                    </td>
                </tr>
                <tr>
                    <td class="admincls0" align="center">
                        照&nbsp;&nbsp;&nbsp;&nbsp;片
                    </td>
                    <td class="admincls0" colspan="3">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="flImgEdit" type="file" style="width: 280px;" /><br />
                    </td>
                </tr>
                <tr>
                    <td class="admincls1" align="center">
                        已传照片
                    </td>
                    <td class="admincls1" colspan="3" align="center">
                        <img id="img_" alt="" src="" style="width: 60px; height: 80px; text-align: center;" />
                        <input id="txtHide" name="name" style="display: none;" type="text" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

</body>
</html>
