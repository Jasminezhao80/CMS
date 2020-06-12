<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="InfoManager.aspx.cs" Inherits="InfoManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="css/usercontrol.css" />
    <link rel ="stylesheet" href="css/bootstrap3.min.css" />
    <link rel="stylesheet" href ="css/common.css" />
        <script src="js/jquery-3.1.1.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <title></title>
    <style>
                th {
        width:15%;
        text-align:right;
        color:#2679B5;
        font-weight:normal;
        }
    </style>
    <script>
        //$(function () {
            //$(".nav-tabs li").click(function () {
                //$(this).addClass("active").siblings().removeClass("active");
                //var id = $(this).attr("data-id");
                //alert(id);
                //$(".tab-content").find("#" + id).addClass("active").siblings().removeClass("active");
                //location.hash(id);
                
            //});
        //})
        //$(document).ready(function () {
        //    $('ul.nav > li').click(function (e) {
        //    e.preventDefault();
        //    $('ul.nav > li').removeClass('active');
        //    $(this).addClass('active');

        //    });

        //    });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <UC:Top runat="server" />
        <UC:Left runat="server" />
        <div class="rightPanel"> <%--style="float:right;width:88%">--%>
            <ul class="nav nav-tabs" style="margin-top:15px;list-style:none" >
                <li id="li_project"  runat="server" class="active">
                    <a href="#div_project" data-toggle="tab">项目管理</a>
                </li>
                <li id="li_type"  runat="server">
                    <a href="#div_type" data-toggle="tab">合同性质</a>
                </li>
                <li id="li_supplier" runat="server">
                    <a href="#div_supplier" data-toggle="tab">供应商</a>
                </li>
                <li id="li_orderType" runat="server">
                    <a href="#div_orderType" data-toggle="tab">采购物资类别</a>
                </li>
                <li id="li_unit" runat="server">
                    <a href="#div_productUnit" data-toggle="tab">商品单位</a>
                </li>
            </ul>
            <div class="tab-content">
                  <div id="div_project" runat="server" class="tab-pane fade in active" >
                    <div class="container">
                      <div class="row">
                        <div class="col-md-6">
                    <asp:GridView ID ="grid_project" AutoGenerateColumns="false" runat="server" 
                        class="table-list" OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
                         OnRowCancelingEdit="grid_RowCancelingEdit" ShowHeaderWhenEmpty="True" >
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="项目名称">
                                <ItemTemplate>
                                    <%#Eval("name") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_projectName" Text='<%#Eval("name") %>' runat="server" BackColor="LightYellow"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="true" />
                        </Columns>
                    </asp:GridView>

                        </div>

                        <div class="col-md-6" style="margin-top:10px">
                            <div class="row">
                                 <div class="col-md-3">
                                    <span class="h4" style="float:right">添加新项目:</span>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txt_newProjectName" runat="server"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-9">
                                <asp:Button ID="btnAdd" class="btn btn-primary" runat ="server" Text="添加" style="float:right" OnClick="btnAdd_Click"/>
                                </div>
                            </div>

                    </div>

                    </div>
                     </div>
                </div>
                  
                <div id="div_type" runat="server" class="tab-pane fade " >
                    <div class="container">
                      <div class="row">
                        <div class="col-md-6">
                    <asp:GridView ID ="grid_type" AutoGenerateColumns="false" runat="server" 
                        class="table-list" OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
                         OnRowCancelingEdit="grid_RowCancelingEdit" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="名称">
                                <ItemTemplate>
                                    <%#Eval("name") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_projectType" Text='<%#Eval("name") %>' runat="server" BackColor="LightYellow"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="true" />
                        </Columns>
                    </asp:GridView>

                        </div>

                        <div class="col-md-6" style="margin-top:10px">
                            <div class="row">
                                 <div class="col-md-3">
                                    <span class="h4" style="float:right">添加新类型:</span>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txt_newProjectType" runat="server"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-9">
                                <asp:Button ID="btnAddType" class="btn btn-primary" runat ="server" Text="添加" style="float:right" OnClick="btnAddType_Click"/>
                                </div>
                            </div>

                    </div>

                    </div>
                     </div>
                </div>


                <div id="div_supplier" runat="server" class="tab-pane fade " >
                    <div class="container">
                      <div class="row">
                        <div class="col-md-6">
                    <asp:GridView ID ="grid_supplier" AutoGenerateColumns="false" runat="server" 
                        class="table-list" OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
                         OnRowCancelingEdit="grid_RowCancelingEdit" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="名称">
                                <ItemTemplate>
                                    <%#Eval("name") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_supplier" Text='<%#Eval("name") %>' runat="server" BackColor="LightYellow"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="true" />
                        </Columns>
                    </asp:GridView>

                        </div>

                        <div class="col-md-6" style="margin-top:10px">
                            <div class="row">
                                 <div class="col-md-3">
                                    <span class="h4" style="float:right">添加供应商:</span>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txt_newSupplier" runat="server"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-9">
                                <asp:Button ID="btnAddSupplier" class="btn btn-primary" runat ="server" Text="添加" style="float:right" OnClick="btnAddSupplier_Click"/>
                                </div>
                            </div>

                    </div>

                    </div>
                     </div>
                </div>
                
                <div id="div_orderType" runat="server" class="tab-pane fade " >
                    <div class="container">
                      <div class="row">
                        <div class="col-md-6">
                    <asp:GridView ID ="grid_orderType" AutoGenerateColumns="false" runat="server" 
                        class="table-list" OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
                         OnRowCancelingEdit="grid_RowCancelingEdit" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="名称">
                                <ItemTemplate>
                                    <%#Eval("name") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_orderType" Text='<%#Eval("name") %>' runat="server" BackColor="LightYellow"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="true" />
                        </Columns>
                    </asp:GridView>

                        </div>

                        <div class="col-md-6" style="margin-top:10px">
                            <div class="row">
                                 <div class="col-md-3">
                                    <span class="h4" style="float:right">添加新类别:</span>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txt_newOrderType" runat="server"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-9">
                                <asp:Button ID="btnAddNewOrderType" class="btn btn-primary" runat ="server" Text="添加" style="float:right" OnClick="btnAddNewOrderType_Click"/>
                                </div>
                            </div>

                    </div>

                    </div>
                     </div>
                </div>
         
                <div id="div_productUnit" runat="server" class="tab-pane fade " >
                    <div class="container">
                      <div class="row">
                        <div class="col-md-6">
                    <asp:GridView ID ="grid_productUnit" AutoGenerateColumns="false" runat="server" 
                        class="table-list" OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
                         OnRowCancelingEdit="grid_RowCancelingEdit" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="名称">
                                <ItemTemplate>
                                    <%#Eval("name") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_unit" Text='<%#Eval("name") %>' runat="server" BackColor="LightYellow"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="true" />
                        </Columns>
                    </asp:GridView>

                        </div>

                        <div class="col-md-6" style="margin-top:10px">
                            <div class="row">
                                 <div class="col-md-3">
                                    <span class="h4" style="float:right">添加单位:</span>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txt_newUnit" runat="server"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-9">
                                <asp:Button ID="btnAddUnit" class="btn btn-primary" runat ="server" Text="添加" style="float:right"  OnClick="btnAddUnit_Click"/>
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
