<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseImport.aspx.cs" Inherits="purchase_PurchaseImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="../css/bootstrap3.min.css"/>
    <link rel="stylesheet" href="../css/common.css" />
    <link rel="stylesheet" href="../css/usercontrol.css" />
    <script src="../js/jquery-3.1.1.min.js"></script>
    <title></title>
    <script type="text/javascript">
        function SaveClick() {
            if ($("#txt_orderNum").val() == "") {
                alert("请输入申购单号！");
                return false;
            }
            if ($("#ddl_project").val() == "0") {
                alert("请选择所属项目！");
                return false;
            }
            if ($("#txt_applyDate").val() == "") {
                alert("请输入申请日期！");
                return false;
            }
            //alert("检验成功！");
            return true;
        };
    </script>
</head>
<body>
    <UC:Top runat="server"/>
    <UC:Left runat="server"/>
    <form id="form1" runat="server">
        <div class="rightPanel">
            <div class="container-fluid" style="margin-top:5px">
            <div class="row form-group">
                <div class="col-md-2 text-right">
                    <span style ="color:red">*</span>
                    <span class="text-info">采购单号:</span>
                </div>
                <div class="col-md-3">
                    <input id="orderId" runat ="server" type="hidden"/>
                    <input id="pcount" runat="server" type="hidden"/><%--记录明细商品的行数--%>
                    <input id="txt_orderNum" runat ="server" class="form-control" type="text" />
                </div>
                <div class="col-md-2 text-right">
                        <span style ="color:red">*</span>
                    <span class="text-info">所属项目:</span>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList id="ddl_project" runat="server" class="form-control" ></asp:DropDownList> 
                </div>

            </div>
            <div class="row form-group">
                <div class="col-md-2 text-right">
                    <span class="text-info ">合同编号:</span>
                </div>
                <div class="col-md-3">
                <input id="txt_contractNum" runat ="server" class="form-control" type="text" />
                </div>
                <div class="col-md-2 text-right">
                        <span style ="color:red">*</span>
                    <span class="text-info">申请日期:</span>
                </div>
                <div class="col-md-3">
                    <input id="txt_applyDate" runat ="server" class="form-control"  type="date" />
                </div> 
           </div>
            <div class="row form-group">
                <div class="col-md-2 text-right">
                    <span class="text-info ">负责人:</span>
                </div>
                <div class="col-md-3">
                   <input id="txt_leader" class="form-control " runat ="server" type="text"/>
                </div>
                <div class="col-md-2 text-right">
                    <span class="text-info ">总金额:</span>
                </div>
                <div class="col-md-3">
                   <input class="form-control" runat ="server" type="text"/>
                </div>
            </div>
            <div class="row form-group">

                <div class="col-md-2 text-right">
                     <span class="text-info ">备注:</span>
                </div>
                <div class="col-md-3">
                    <textarea id="txt_memo" class="form-control" runat="server" ></textarea>
                </div>
            </div>
        </div>
            <div class="container-fluid">
                <div class="row">
                    <input type="button" id="btnImport" class="btn btn-primary" value="导入" runat="server" onserverclick="btnImport_ServerClick" />
                    <asp:Button id="btnSave" class="btn btn-primary" Text="保存" runat="server" OnClientClick="return SaveClick()" OnClick="btnSave_ServerClick"/>
                    <%--<input id="fileUpload" name="fileUpload" type="file" runat="server"/>--%>
                    <asp:FileUpload id="fileUpload" runat="server"/>
                </div>
                <div class="row" >
                    <asp:GridView ID="grid" runat="server" AutoGenerateColumns="false" HeaderStyle-Wrap="false" RowStyle-Wrap="false"
                         class="table table-list table-hover" ShowHeaderWhenEmpty="true">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Eval("col1") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="商品名称">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col2" Text='<%#Eval("col2") %>' runat="server" ></asp:TextBox>
                                    <%--<input type="text" value='<%#Eval("col2") %>' style="width:80px"/>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="类别">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col3" Text='<%#Eval("col3") %>' runat="server" style="width:80px"></asp:TextBox>
                                    <%--<input type="text" value='<%#Eval("col3") %>' style="width:80px"/>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="规格/型号">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col4" Text='<%#Eval("col4") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="材质">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col5" Text='<%#Eval("col5") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="单位">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col6" Text='<%#Eval("col6") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="数量">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col7" Text='<%#Eval("col7") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="供应商">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col8" Text='<%#Eval("col8") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="要求交货日期">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col9" Text='<%# Eval("col9") is null || Eval("col9") is DBNull || Eval("col9").ToString() == "" ? string.Empty : Convert.ToDateTime(Eval("col9")).ToString("yyyy-MM-dd") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="单价">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col10" Text='<%#Eval("col10") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col11" Text='<%#Eval("col11") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
