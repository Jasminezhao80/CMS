<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InStoreDetailList.aspx.cs" Inherits="warehouse_InStoreDetailList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap3.min.css" />
    <link rel="stylesheet" href="../css/common.css" />
    <link rel="stylesheet" href="../css/usercontrol.css" />
</head>
<body>
    <form id="form1" runat="server">
        <UC:Top runat="server"/>
        <UC:Left runat="server" />
        <div class="rightPanel">
            <div class="container">
                <div class="row">
                    <asp:gridview runat="server" ID="gridList" EmptyDataText="没有数据" AutoGenerateColumns="false" class="table table-list table-hover" >
                        <Columns>
                            <asp:TemplateField HeaderText="序号" >
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="采购单号">
                                <ItemTemplate>
                                    <%#Eval("order_num") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="商品名称">
                                <ItemTemplate>
                                    <%#Eval("product_name") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="规格">
                                <ItemTemplate>
                                    <%#Eval("product_size") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="材质">
                                <ItemTemplate>
                                    <%#Eval("product_material") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="入库数量">
                                <ItemTemplate>
                                    <%#Eval("quantity") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                         <asp:TemplateField HeaderText="入库日期">
                        <ItemTemplate>
                            <%# Eval("instore_date") is null || Eval("instore_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("instore_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField> 
                        </Columns>
                    </asp:gridview>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
