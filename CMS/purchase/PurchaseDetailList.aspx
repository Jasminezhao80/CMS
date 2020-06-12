<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDetailList.aspx.cs" Inherits="purchase_PurchaseDetailList" %>

<!DOCTYPE html>
<script runat="server">
    protected void grid_detail_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {

    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="../css/bootstrap3.min.css" />
    <link rel ="stylesheet" href="../css/common.css" />
    <link rel="stylesheet" href="../css/usercontrol.css" />
    <script src="../js/jquery-3.1.1.min.js"></script>
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            var screenHeight = document.documentElement.clientHeight;
            var top =document.getElementById("div_gridPanel").offsetTop;
            //document.getElementById("div_gridPanel").style.height = screenHeight - 100 + "px";
            var hight = screenHeight - top -10;
            document.getElementById("div_gridPanel").style.height = hight + "px";
            //document.getElementById("grid_detail").offsetTop = 50;

            //设置固定表头
<%--            var t = document.getElementById("<%=grid_detail.ClientID%>");
            var temp = t.rows[0].cloneNode(true);--%>
            //for (i = temp.rows.length - 1; i > 1; i--) {
            //    temp.deleteRow(i);
            //}
            //t.deleteRow(0);
            //document.getElementById("div_tempTable").appendChild(temp);
        });
        function UpdateProduct(productId, obj)
        {
            var item = "size";
            if (obj.id.indexOf("name") != -1) {
                item = "name";
            }
            var value = obj.value;
            var tr = $(obj).parent().parent();
            var td = $(tr).find("td");
            var id = td.eq(0).find("input").val();
            $.ajax({
                async: false,
                url: "../purchase/PurchaseDetailList.aspx/UpdateProduct",
                type: "post",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{'id':"+id +",'productId':"+ productId +",'value':'"+ value +"','item':'"+item+"'}",
                success: function (res) {
                    alert("修改成功！");
                }
            });
        };
        function SaveWareHouseDate(key,obj) {
            var date = obj.value;
            //var date1 = event.srcElement.value;
            //alert(date + "ceshi :" + date1);
            var id = key;
            $.ajax({
                async: false,
                url: "../purchase/PurchaseDetailList.aspx/SaveWareHouseDate",
                type: "post",
                data: "{'id':"+id +",'date':'" + date+"'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    alert("修改成功！");
                }

            });
        }

        function SavePrice(key, obj) {
            var price = obj.value;
            var row = $(obj).parent().parent();
            var td = $(row).find("td");
            var quantity = td.eq(10).find("input").val();
            td.eq(11).text(quantity * price);
            var id = key;
            $.ajax({
                async: false,
                url: "../purchase/PurchaseDetailList.aspx/SavePrice",
                type: "post",
                data: "{'id':"+id +",'price':" + price+"}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    alert("修改成功！");
                }

            });
        }

        function SaveQuantity(key, obj) {
            var quantity = obj.value;
            if (quantity == "") {
                quantity = 0;
            }
            var row = $(obj).parent().parent();
            var td = $(row).find("td");
            var unitPrice = td.eq(9).find("input").val();
            td.eq(11).text(quantity * unitPrice);
            var id = key;
            $.ajax({
                async: false,
                url: "../purchase/PurchaseDetailList.aspx/SaveQuantity",
                type: "post",
                data: "{'id':"+id +",'quantity':" + quantity+"}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    alert("修改成功！");
                }

            });
        }

        function ChangeValue(key, obj) {
            var id = key;
            var item = "";
            if (obj.id.indexOf("txt_leader") != -1) {
                item = "leader";
            }
            if (obj.id.indexOf("memo") != -1) {
                item = "memo";
            }
            if (obj.id.indexOf("supplier") != -1) {
                item = "supplier_id";
                var tr = $(obj).parent().parent();
                var td = $(tr).find("td");
                id = td.eq(0).find("input").val();
            }
            var value = obj.value;
            $.ajax({
                async: false,
                url: "../purchase/PurchaseDetailList.aspx/ChangeValue",
                type: "post",
                data: "{'id':"+id +",'value':'" + value+"','item':'"+item + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    alert("修改成功！");
                }

            });
        }
        function ZoomBig() {
            var grid = document.getElementById('<%= grid_detail.ClientID %>');
            var fontSize = getComputedStyle(document.getElementById("grid_detail"), undefined).fontSize;
            fontSize = parseInt(fontSize.replace("px", "")) + 1;
            document.getElementById("grid_detail").style.fontSize = fontSize + "px";
        }
        function ZoomSmall() {
            var fontSize = getComputedStyle(document.getElementById("grid_detail"), undefined).fontSize;
            fontSize = parseInt(fontSize.replace("px", "")) - 1;
            document.getElementById("grid_detail").style.fontSize = fontSize + "px";
        }

        </script>
</head>
<body>
    <UC:Top runat="server"/>
    <UC:Left runat="server"/>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <input type="hidden"/>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="rightPanel">
            <div class="container-fluid">
                <div class="row form-inline form-group">
                    <div class="col-md-3 ">
                        <span class="text-info">所属项目:</span>
                        <asp:DropDownList ID="ddl_project" runat="server" CssClass="form-control" style="width:60%" OnSelectedIndexChanged="ddl_project_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <span class="text-info">供应商:</span>
                        <asp:DropDownList ID="ddl_supplier" runat="server" CssClass="form-control" style="width:65%" OnSelectedIndexChanged="ddl_supplier_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">是否入库:</span>
                        <asp:DropDownList ID="ddl_isInWarehouse" runat="server" OnSelectedIndexChanged="ddl_isInWarehouse_SelectedIndexChanged"
                            CssClass="form-control" style="width:50%" AutoPostBack="true">
                           <asp:ListItem Value="0">全部</asp:ListItem>
                           <asp:ListItem Value="1">未入库</asp:ListItem>
                           <asp:ListItem Value="2">已入库</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <span class="text-info">关键字:</span>
                        <input type="text" placeholder="采购单号/合同编号/采购内容/规格/类别" id="txt_searchKey" runat="server" class="form-control" style="width:78%" />
                    </div>
                    <div class="col-md-1">
                        <input type="button" id="btn_Search" runat="server" class="btn btn-primary" value="查询" onserverclick="btn_Search_ServerClick"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-10"></div>
                    <div class="col-md-2 " >
                    <span class="text-info">点击缩放表格:</span>
                    <a href="#">
                    <span class="glyphicon glyphicon-zoom-in" onclick="ZoomBig()" title="放大"></span>
                    </a>                    
                    <a href="#">
                    <span class="glyphicon glyphicon-zoom-out" onclick="ZoomSmall()" title="缩小"></span>
                    </a>
                    </div>
      

                </div>
                <div id="div_tempTable" class="row">

                </div>
                <div id="div_gridPanel" class="row" style="overflow-y:scroll;" >
                    <asp:GridView ID="grid_detail" runat="server" AutoGenerateColumns="false" class="table table-list table-hover" HeaderStyle-Wrap="false" RowStyle-Wrap="false"
                          OnRowDataBound="grid_detail_RowDataBound" PagerSettings-Position="TopAndBottom" AllowPaging="false" PageSize="100" OnPageIndexChanging="grid_detail_PageIndexChanging" >
                        <Columns>
                            <asp:TemplateField  HeaderText="序号" >
                                <ItemTemplate>
                                   <input type="hidden" id="txt_id" runat="server" value='<%#Eval("id") %>' />
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="所属项目">
                                <ItemTemplate>
                                    <%#Eval("projectName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="类别" >
                                <ItemTemplate>
                                    <%#Eval("category").ToString().Length > 4?Eval("category").ToString().Substring(0,3): Eval("category")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="申购单号" >
                                <ItemTemplate>
                                    <asp:HyperLink NavigateUrl='<%#Eval("order_id","~/purchase/PurchaseDetail.aspx?id={0}&&backType=") + "detailList&&searchKey=" + GetSearchKeys()%>' runat="server" Text='<%#Eval("order_num") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="合同编号" >
                                <ItemTemplate>
                                    <%#Eval("contract_id") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="申请时间" >
                                <ItemTemplate>
                                    <%# Eval("apply_date") is null || Eval("apply_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("apply_date")).ToString("yyyy-MM-dd") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="采购内容" >
                                <ItemTemplate>
                                    <input type="text" id="txt_name" runat="server" value='<%#Eval("product_name")%>' onchange='<%#Eval("product_id","UpdateProduct({0},this)") %>' style="width:100px;border:none"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="规格" >
                                <ItemTemplate>
                                    <%--<input type="text" runat="server" value='<%#Eval("product_size").ToString().Length >8?Eval("product_size").ToString().Substring(0,7): Eval("product_size")%>' onchange='<%#Eval("product_id","UpdateProduct({0},this)") %>' style="width:80px;border:none"/>--%>
                                    <input type="text" id="txt_size" runat="server" value='<%#Eval("product_size")%>' onchange='<%#Eval("product_id","UpdateProduct({0},this)") %>' style="width:80px;border:none"/>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="单位" >
                                <ItemTemplate>
                                    <%#Eval("unit") %>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="单价" >
                                <ItemTemplate>
                                    <%--<%#Eval("unit_price") %>--%>
                                    <input type="text" runat="server" style="width:60px;margin:0 auto; border:none;text-align:right" value='<%#Eval("unit_price") %>'  onchange='<%#Eval("id","SavePrice({0},this)") %>' />
                                </ItemTemplate>
<%--                                <EditItemTemplate>
                                    <input type="text" runat="server" style="width:80px" value='<%#Eval("unit_price") %>' />
                                </EditItemTemplate>--%>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="数量" >
                                <ItemTemplate>
                                    <input type="text" runat="server" style="width:100%;border:none;text-align:right" value='<%#Eval("quantity") %>' onchange='<%#Eval("id","SaveQuantity({0},this)") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>                            

                            <asp:TemplateField HeaderText="总价" >
                                <ItemTemplate>
                                    <%# Eval("price")%>
                                </ItemTemplate>
                            </asp:TemplateField>                            

                           
                            <asp:TemplateField HeaderText="入库日期" >
                                <ItemTemplate>
                                    <input type="date" runat="server" style="width:100%;border:none"  value='<%# Eval("in_warehouse_date") is null || Eval("in_warehouse_date") is DBNull || Eval("in_warehouse_date").ToString() ==""? string.Empty : Convert.ToDateTime(Eval("in_warehouse_date")).ToString("yyyy-MM-dd") %>' onchange='<%#Eval("id","SaveWareHouseDate({0},this)") %>' />
                                </ItemTemplate>
<%--                                <EditItemTemplate>
                                    <input type="text" runat="server" style="width:80px" value='<%#Eval("unit_price") %>' />
                                </EditItemTemplate>--%>
                            </asp:TemplateField>                            

                            <asp:TemplateField HeaderText="供应商" >
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddl_supplierDetail" style="border:none" runat="server" onchange ="ChangeValue('147',this)" AutoPostBack="false" >
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="要求交货日期" >
                                <ItemTemplate>
                                    <%# Eval("delivery_date") is null || Eval("delivery_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("delivery_date")).ToString("yyyy-MM-dd") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="负责人">
                                <ItemTemplate>
                                    <%--<asp:TextBox ID="txt_leader" runat="server"></asp:TextBox>--%>
                                    <input type ="text" id="txt_leader" style="width:80px;border:none" runat="server" onchange='<%#Eval("id","ChangeValue({0},this)") %>' value='<%#Eval("leader") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <input type ="text" id="txt_memo" style="width:100%;border:none" runat="server" onchange='<%#Eval("id","ChangeValue({0},this)") %>' value='<%#Eval("memo") %>' />
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
