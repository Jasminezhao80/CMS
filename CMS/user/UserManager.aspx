<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManager.aspx.cs" Inherits="user_UserManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../css/usercontrol.css" />
    <link rel ="stylesheet" href="../css/bootstrap3.min.css" />
    <link rel="stylesheet" href ="../css/common.css" />
    <script src="../js/jquery-3.1.1.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>

    <link href="../easyui/jquery-easyui-1.7.0/themes/default/easyui.css" rel="stylesheet" />
    <link href="../easyui/jquery-easyui-1.7.0/themes/icon.css" rel="stylesheet" />
    <%--<script src="../easyui/jquery-easyui-1.7.0/jquery.min.js"></script>--%>
    <script src="../easyui/jquery-easyui-1.7.0/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        function getChecked() {
            var userId = $("#txtUserId").val();

            var nodes = $('#treeList').tree('getChecked');
			var s = '';
			for(var i=0; i<nodes.length; i++){
				if (s != '') s += ',';
				s += nodes[i].id;
            }
            $.ajax({
                url: "../user/UserManager.aspx/SavePermission",
                type: "post",
                async: false,
                dateType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{'codeList':'" + s + "','userId':"+userId+"}",
                success: function (res) {
                    alert("保存成功!");
                }
            })
			
        };
        function LoadPermission(userId,rowIndex) {
            $("#txtUserId").val(userId);
            var name = document.getElementById("userGrid").rows[rowIndex + 1].cells[1].innerText;
            $("#userName").val(name);
            $.ajax({
                async: false,
                url: "../user/UserManager.aspx/GetPermission",
                type: "post",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{'userId':" + userId + "}",
                success: function (res) {
                    loadTree();
                    var list = res.d;
                    if (list.length > 0) {
                        for (var i = 0; i < list.length; i++) {
                            var code = list[i];
                            var node = $("#treeList").tree('find', code);
                            $("#treeList").tree('check', node.target);
                        }
                    }
                }
            });
            
        }

        function loadTree() {
                       $("#treeList").tree({
                data: [{
                    "id": "A01",
                    "text": "合同管理",
                    
                    "children": [{
                        "id": "A0101",
                        "text": "合同管理明细",
                        "state":"closed",
                        "children": [{
                            "id": "A010101",
                            "text": "添加新合同",
                        }, {
                            "id": "A010102",
                            "text": "修改"
                        }, {
                                "id": "A010103",
                            "text": "删除"
                            }, {
                                "id": "A010104",
                                "text":"查询"
                            }]
                    },{
                        "id": "A0102",
                        "text": "合同汇总统计",
                        "children": [{
                            "id": "A010201",
                            "text": "下载报表"
                        }, {
                                "id": "A010202",
                                "text":"查询"
                            }]
                    }]
                },
                    {
                    "id": "A02",
                    "text": "采购管理",
                    "children": [{
                        "id": "A0201",
                        "text": "采购订单管理",
                        "children": [{
                            "id": "A020101",
                            "text":"添加新订单"
                        }, {
                                "id": "A020102",
                                "text":"启用/废弃"
                            }, {
                                "id": "A020103",
                                "text":"修改"
                            }, {
                                "id": "A020104",
                                "text":"删除"
                            }, {
                                "id": "A020105",
                                "text":"查询"
                            }]
                    }, {
                        "id": "A0202",
                            "text": "采购订单明细",
                            "children": [{
                                "id": "A020201",
                                "text":"查询"
                            }, {
                                    "id": "A020202",
                                    "text":"修改"
                                }]
                    }, {
                        "id": "A0203",
                            "text": "采购订单汇总",
                            "children": [{
                                "id": "A020301",
                                "text":"下载报表"
                            }]
                        }, {
                            "id": "A0204",
                        "text": "商品管理"
                        }]
                    }, {
                        "id": "A03",
                        "text": "基础信息管理",
                        "children": [{
                            "id": "A0301",
                            "text": "项目管理"

                        },
                            {
                                "id": "A0302",
                                "text":"用户管理"
                            }]
                    }]
            });
        };
        $(document).ready(function () {
        });
        function SaveClick() {
            var id = $("#userId").val();
            if (id == "") {
                id = 0;
            }
            if ($("#txtUser").val() == "") {
                alert("请输入用户名！");
                return false;
            };
            if ($("#txtPass").val() == "") {
                alert("请输入密码！");
                return false;
            };
            $.ajax({
                async: false,
                url: "../user/UserManager.aspx/SaveUser",
                data: "{'id':" + id+",'userName':'"+ $("#txtUser").val() + "','password':'"+$("#txtPass").val()+"','memo':'"+ $("#txtMemo").val()+"'}",
                dataType: "json",
                type: "post",
                contentType: "application/json; charset=utf-8",
                success: function (res) {
                    var data = res.d;
                    if (data == "true") {
                        alert("保存成功！");
                        window.location.reload();
                    }
                    else {
                        alert("用户名已经存在！");
                    }
                }

            });
        };
        function AddUser() {
                    $("#userId").val(0);
                    $("#txtUser").val("");
                    $("#txtPass").val("");
                    $("#txtMemo").val("");
        }
        function EditUser(userId) {
            $("#userId").val(userId);
            $.ajax({
                async: false,
                url: "../user/UserManager.aspx/GetUser",
                data: "{'id':" + userId+"}",
                dataType: "json",
                type: "post",
                contentType: "application/json; charset=utf-8",
                success: function (res) {
                    var data = res.d;
                    $("#txtUser").val(data[0]);
                    $("#txtPass").val(data[1]);
                    $("#txtMemo").val(data[2]);
                }

            });
        }
    </script>
</head>
<body>
    <UC:Top runat="server" />
    <UC:Left runat="server" />
    <form id="form1" runat="server">
        <div class="rightPanel">
            <div style="width:50%;float:left;margin-left:30px;">
                <input type="text" runat="server" id="txtUserId" hidden="hidden"/>
                <input type="button" class="btn btn-primary" value="+添加新用户" data-toggle="modal" data-target="#div_add" onclick="AddUser()"/>
                <asp:GridView ID="userGrid" runat="server" AutoGenerateColumns="false" DataKeyNames="id"
                    class="table table-list table-hover" OnRowDataBound="userGrid_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="序号">
                            <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="用户名">
                            <ItemTemplate>
                                <%#Eval("name") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="密码">
                            <ItemTemplate>
                                ***
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="备注">
                            <ItemTemplate>
                                <%#Eval("memo") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server"  OnClientClick='<%#Eval("id","EditUser({0})") %>'  data-toggle="modal" data-target="#div_add" CommandArgument='<%#Eval("id") %>'>修改</asp:LinkButton>
                            <asp:LinkButton ID="btnDel" runat="server" OnClick="btnDel_Click"  CommandArgument='<%#Eval("id") %>' OnClientClick="javascript:return confirm('确定要删除么？')">删除</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
            <div style="width:40%;float:left; margin-left:20px" class="form-inline">
                <input type="button" class="btn btn-primary" value ="保存设置" onclick="getChecked()"/>
                <input type="text" class="form-control info" id="userName" style="width:80px;border:none" />
            <ul id="treeList"  data-options="checkbox:true">
               
            </ul>            

                
            </div>

            <div id="div_add" class="modal fade"  aria-hidden="true" data-backdrop="static" >
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4>用户信息</h4>
                        </div>
                        <div class="modal-body">
                            <div class="container-fluid">
                                <input type="hidden" id="userId" runat="server" />
                                <div class="row form-group">
                                    <div class="col-md-4 text-right">
                                        <span class="text-info">用户名:</span>
                                    </div>
                                    <div class="col-md-5"> 
                                        <input id="txtUser" runat="server" type="text" class="form-control"/>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-4 text-right">
                                        <span class="text-info">密码:</span>
                                    </div>
                                   <div class="col-md-5"> 
                                        <input id="txtPass" runat="server" type="password" class="form-control"/>
                                    </div>
                                </div>
                                <div class="row form-group">
                                   <div class="col-md-4 text-right">
                                        <span class="text-info">备注:</span>
                                    </div>
                                   <div class="col-md-5"> 
                                        <input id="txtMemo" runat="server" type="text" class="form-control"/>
                                    </div>
                                </div>                            
                                <div class="row form-group">
                                   <div class="col-md-6 text-right">
                                       <input id="btnSave" class="btn btn-primary" type="button" value="保存" runat="server" onclick="SaveClick()" />
                                    </div>
                                   <div class="col-md-4 text-left"> 
                                        <input id="btnCancel" class="btn btn-primary" type="button" value="取消" data-dismiss="modal" aria-hidden="true" />
                                    </div>
                                </div>       
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
