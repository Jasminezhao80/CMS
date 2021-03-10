<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InStoreDetailList.aspx.cs" Inherits="warehouse_InStoreDetailList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap3.min.css" />
    <link rel="stylesheet" href="../css/common.css" />
    <link rel="stylesheet" href="../css/usercontrol.css" />
    <script src ="../js/jquery-3.1.1.min.js"></script>
    <style type="text/css">
                .table-show {
            margin-bottom:0px;
            margin-top:0px;
            table-layout:fixed;
        }
    </style>
    <script type="text/javascript">
        function loadTable() {
            var t = document.getElementById("<%=gridList.ClientID%>");
                    var t2 = t.cloneNode(true);

                    for (i = t2.rows.length - 1; i > 0; i--) {
                        t2.deleteRow(i)
                    }
                    t.deleteRow(0)
                    var divGrid = document.getElementById("gridHeader");
                    divGrid.appendChild(t2);
        }
        $(document).ready(function () {
            loadTable();
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
                  <div id="gridHeader" style="overflow-y:scroll"></div>
                   <div id="gridBody" style="height:600px;overflow-y:scroll">

                    <asp:gridview runat="server" ID="gridList" EmptyDataText="没有数据" AllowPaging="true" PageSize="100" AutoGenerateColumns="false" class="table table-list table-hover table-show" 
                        OnPageIndexChanging="gridList_PageIndexChanging" >
                        <Columns>
                            <asp:TemplateField HeaderText="序号" HeaderStyle-Width="5%" ItemStyle-Width="5%" >
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="采购单号" HeaderStyle-Width="17%" ItemStyle-Width="17%">
                                <ItemTemplate>
                                    <%#Eval("order_num") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="商品名称" HeaderStyle-Width="25%" ItemStyle-Width="25%">
                                <ItemTemplate>
                                    <%#Eval("product_name") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="规格" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <%#Eval("product_size") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="材质" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <%#Eval("product_material") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="入库数量" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <%#Eval("quantity") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                         <asp:TemplateField HeaderText="入库日期" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <%# Eval("instore_date") is null || Eval("instore_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("instore_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField> 
                            <asp:TemplateField HeaderText="操作" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDel" runat="server" OnClick="btnDel_Click" CommandArgument='<%#Eval("id")+";" +Eval("quantity")+";"+Eval("product_id") %>' OnClientClick="javascript:return confirm('确定要删除么?')" Text="删除"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:gridview>
                       </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
