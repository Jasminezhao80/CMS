<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseList.aspx.cs" Inherits="PurchaseList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap3.min.css"/>
    <link rel="stylesheet" href="../css/common.css"/>
    <link rel="stylesheet" href="../css/usercontrol.css"/>
    <script src="../js/jquery-3.1.1.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script type="text/javascript">
                $(document).ready(function () {
            var screenHeight = document.documentElement.clientHeight;
            var top =document.getElementById("div_gridPanel").offsetTop;
            var hight = screenHeight - top -10;
            document.getElementById("div_gridPanel").style.height = hight + "px";
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <UC:Top runat="server"/>
        <UC:Left runat="server" />
        <div class="rightPanel">
            <div class="container-fluid">
                <div class="row">
<%--                    <input id="btnAdd" type="button" runat="server" class="btn btn-primary" value ="+添加新订单" data-toggle="modal" data-target="#div_add" /> --%>
                        <%--<input id="btnImport" type="button" runat="server" class="btn btn-primary" value ="导入订单"  onserverclick="btnImport_ServerClick" />--%> 
                        <input id="btnAdd" type="button" runat="server" class="btn btn-primary" value ="+添加新订单" onserverclick="btnAdd_ServerClick" /> 

                </div>
                <div id="div_gridPanel" class="row" style="overflow-y:scroll;">
                    <asp:GridView ID="grid_list" runat="server" class="table table-list table-bordered table-hover"
                         AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" OnRowDataBound="grid_list_RowDataBound" HeaderStyle-Wrap="false" RowStyle-Wrap="false">
                        <Columns>
                            <asp:TemplateField HeaderText ="序号">
                                <ItemTemplate >
                                   <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText ="采购单号">
                                <ItemTemplate>
                                   <asp:HyperLink ID="linkNum" NavigateUrl='<%#Eval("id","../purchase/purchaseDetail.aspx?id={0}")%>' runat="server" Text='<%#Eval("order_num") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField HeaderText="采购内容">
                                <ItemTemplate>
                                    <%#Eval("order_name") %>
                                </ItemTemplate>                            
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="合同编号">
                                <ItemTemplate>
                                    <%#Eval("contract_id") %>
                                </ItemTemplate>                            
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="所属项目">
                                <ItemTemplate>
                                    <%#Eval("projectName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="申请日期">
                                <ItemTemplate>
                                    <%# Eval("apply_date") is null || Eval("apply_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("apply_date")).ToString("yyyy-MM-dd") %>
                                </ItemTemplate>                            
                            </asp:TemplateField>                        
                            <asp:TemplateField HeaderText="总金额">
                                <ItemTemplate>
                                    <%--<%#Eval("price") %>--%>
                                </ItemTemplate>                            
                            </asp:TemplateField>   
                            <asp:TemplateField HeaderText="负责人">
                                <ItemTemplate>
                                    <%#Eval("leader") %>
                                </ItemTemplate>                            
                            </asp:TemplateField>                               
                            <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <%#Eval("memo") %>
                                </ItemTemplate>                            
                            </asp:TemplateField>  
                            <asp:TemplateField HeaderText="有效性">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDisable" Visible='<%#Function.CheckButtonPermission("A020102") %>' runat="server" OnClientClick="javascript:return confirm('确定要废弃此采购单么？')" OnClick="btnDisable_Click" CommandArgument='<%#Eval("id") %>' Text="废弃"></asp:LinkButton>
                                    <asp:LinkButton ID="btnEnable" Visible='<%#Function.CheckButtonPermission("A020102") %>' runat="server" OnClientClick="javascript:return confirm('确定启用此采购单么？')" OnClick="btnEnable_Click" Text="启用" CommandArgument='<%#Eval("id")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                                             
                            <asp:TemplateField HeaderText="操作" >
                                <ItemTemplate>
                                    <input type="hidden" id="txt_isDisabled" runat="server" value='<%#Eval("is_disabled") %>' />
                                    <asp:LinkButton ID="btnEdit" Visible='<%#Function.CheckButtonPermission("A020103") %>'  OnClick="btnEdit_Click" runat="server" CommandArgument='<%#Eval("id") %>' Text ="修改"></asp:LinkButton>
                                    <asp:LinkButton ID="btnDel" Visible='<%#Function.CheckButtonPermission("A020104") %>' runat="server" OnClick="btnDel_Click" OnClientClick="javascript: return confirm('确定要删除么？')" CommandArgument='<%#Eval("id")%>' Text="删除"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </div>

  <%--          <div class="modal fade" id="div_add">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>                     <h4 class="modal-title">添加采购订单</h4>
                        </div>
                        <div class="modal-body">
                            <div class="container">
                                <div class="row">
                                    <div class="col-md-3">
                                        <span>采购单号:</span>
                                        <input type="text" runat="server" />
                                    </div>
                                    <div class="col-md-3">
                                        <span>所属项目:</span>
                                        <asp:DropDownList ID="ddl_project" runat ="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col-md-3">
                                        <span>合同编号:</span>
                                        <input type="text" runat="server"/>
                                    </div>
                                    <div class="col-md-3">
                                        <span>采购内容:</span>
                                        <input id="txt_purchaseName" type="text" runat="server" />
                                    </div>                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <span>规格:</span>
                                    <input type="text" runat="server" />
                                </div>
                                <div class="col-md-3">
                                    <span>单位:</span>
                                    <input type="text" runat="server"/>
                                    <span>数量:</span>
                                    <input type="text" runat="server"/>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-default" data-dismiss="modal">关闭</button>
                            <button class="btn btn-primary">保存</button>
                        </div>
                    </div>
                </div>
            </div>
 --%>       </div>
    </form>
</body>
</html>
