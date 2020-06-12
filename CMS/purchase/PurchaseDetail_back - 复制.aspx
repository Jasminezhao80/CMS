<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDetail_back - 复制.aspx.cs" Inherits="purchase_PurchaseDetail_back" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap3.min.css"/>
    <link rel="stylesheet" href="../css/usercontrol.css" />
    <link rel="stylesheet" href="../css/common.css" />
    <script src ="../js/jquery-3.1.1.min.js"></script>
    <script src ="../js/bootstrap.min.js"></script>
    <style>

    </style>
    <script type="text/javascript">
        //function SearchClick() {
            
        //    var key = $.trim($("#txt_searchKey").val());
        //    alert(key);
        //    var gridView = document.getElementById("grid_product");
        //    $.ajax({
        //        //async: false,
        //        url: "../purchase/PurchaseDetail.aspx/BindProductGrid",
        //        type: "post",
        //        data: {searchKey:key,grid:gridView},
        //        dataType: "json",
        //        contentType:"application/json; charset=utf-8",
        //        success: function (result) {
        //            alert("成功");
        //            //alert(result);
        //            alert(result.d);
        //            $("#txt_searchKey").val(result.d);
        //        },
        //        error: function (e) {
        //            alert("失败");
        //            alert(e);
        //        },
        //        complete: function () {
        //            alert("完成");
        //        }
        //    });
        //}
        function SaveClick() {
            if ($("#txt_orderNum").val() == "") {
                alert("请输入采购单号！");
                return false;
            }
            //var select = document.getElementById('ddl_project')
            if ($("#ddl_project").val() == "0") {
                alert("请选择所属项目！");
                return false;
            }
            if ($("#txt_applyDate").val() == "") {
                alert("请输入申请日期！");
                return false;
            }
            var grid = document.getElementById("<%=grid_productList.ClientID%>");
            var rowCount = 0;
            if (grid.rows.length < 2) {
                alert("请选择商品！");
                return false;
            }
            //grid 表头行的index=0
            for (var index = 1; index < grid.rows.length; index++) {
                var cells = grid.rows[index].cells;
                //alert("innerText7:" + (row.cells[7].getElementsByTagName("input")[0]).value);
                //交货日期
                if ((cells[7].getElementsByTagName("input")[0]).value == "") {
                    alert("请输入每种商品的交货日期！");
                    return false;
                }
                var quantity = (cells[8].getElementsByTagName("input")[0]).value;
                if (quantity == "" || quantity == "0") {
                    alert("请输入每种商品的采购数量！");
                    return false;
                }
                var price = (cells[9].getElementsByTagName("input")[0]).value;
                if ( price == "" || price == "0.00") {
                    alert("请输入每种商品的单价！");
                    return false;
                }
            }
        };
        function delProduct(k) {
            $(k).parent().parent().parent().remove();
            var gridview1 = document.getElementById("<%=grid_productList.ClientID%>");
            var index = 1;
            for (var i = 1; i < gridview1.rows.length; ++i) {
                var cells = gridview1.rows[i].cells;
                   cells[0].innerText = index++;
             }
        };
        function delProduct1(k) {
            var row = $(k).parent().parent().parent();
            //alert($(row).find("input[type=hidden]").val());
            //$(row).remove();
            //$(row).style.display = "none";
            //alert($(row).find("input[type=hidden]").val());
            //$(k).parent().parent().parent().remove();
            //var gridview = document.getElementById("<%=grid_productList.ClientID%>");
            //var cell = $(gridview.rows[2]).find("input[type=hidden]");
            //var cell = gridview.rows[2].getElementsByTagName("input")[3];
            //var cell=($(k).parent().parent().parent())[12]; 
            //cell.innerText = "del";
            var gridview1 = document.getElementById("<%=grid_productList.ClientID%>");
                if(gridview1==null) return; 
            var rowIndex = k.parentElement.parentElement.parentElement.rowIndex;
            //(k.parentElement.parentElement.parentElement)[5] = "del";
            gridview1.deleteRow(rowIndex);
            var index = 1;
            for (var i = 1; i < gridview1.rows.length; ++i) {
                var cells = gridview1.rows[i].cells;
                var row = gridview1.rows[i];
                if (row.style.display != "none") {
                    //cells[0].firstChild.nodeValue = index++;
                    cells[0].innerText = index++;
                }
             }
        };
        function ClearSearch() {
            $("#txt_searchKey").val("");
            $("#btnSearch").click();
        };
        $(document).ready(function () {
            $("#div_product").on('show.bs.modal', function (event) {
                //alert("弹窗初始化");
            $("#txt_searchKey").val("");
            $("#btnSearch").click();
            })
        });
        function OKClick() {
                    //    $.ajax({
        //        //async: false,
        //        url: "../purchase/PurchaseDetail.aspx/BindProductGrid",
        //        type: "post",
        //        data: {searchKey:key,grid:gridView},
        //        dataType: "json",
        //        contentType:"application/json; charset=utf-8",
            $.ajax({
                async: false,
                url: "../purchase/PurchaseDetail.aspx/Test",
                type: "post",
                data: {},
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    alert(result.d);
                    $("#grid_productList tbody").append("result.d");
                },
                error: function (e) {
                    alert("失败");
                    alert(e);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
            <UC:Top runat="server"/>
    <UC:Left runat ="server" />
    <div class="rightPanel">
        <div class="container" style="margin-top:5px">
            <div class="row form-group">
                <div class="col-md-3 text-right">
                    <span style ="color:red">*</span>
                    <span class="text-info">采购单号:</span>
                </div>
                <div class="col-md-2">
                    <input id="txt_orderNum" runat ="server" class="form-control" type="text" />
                </div>
                <div class="col-md-2 text-right">
                    <span class="text-info">采购内容:</span>
                </div>
                <div class="col-md-2">
                <input id="txt_name" runat ="server" class="form-control " type="text" />
                </div>

            </div>
            <div class="row form-group">
                <div class="col-md-3 text-right">
                    <span class="text-info ">合同编号:</span>
                </div>
                <div class="col-md-2">
                <input id="txt_contractNum" runat ="server" class="form-control" type="text" />
                </div>
                <div class="col-md-2 text-right">
                        <span style ="color:red">*</span>
                    <span class="text-info">所属项目:</span>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList id="ddl_project" runat="server" class="form-control" ></asp:DropDownList> 
                </div>
           </div>
            <div class="row form-group">
                <div class="col-md-3 text-right">
                    <span class="text-info ">负责人:</span>
                </div>
                <div class="col-md-2">
                   <input id="txt_leader" class="form-control " runat ="server" type="text"/>
                </div>
                <div class="col-md-2 text-right">
                        <span style ="color:red">*</span>
                    <span class="text-info">申请日期:</span>
                </div>
                <div class="col-md-2">
                    <input id="txt_applyDate" runat ="server" class="form-control"  type="date" />
                </div>
            </div>
            <div class="row form-group">
                <div class="col-md-3 text-right">
                    <span class="text-info ">总金额:</span>
                </div>
                <div class="col-md-2">
                   <input class="form-control" runat ="server" type="text"/>
                </div>
                <div class="col-md-2 text-right">
                     <span class="text-info ">备注:</span>
                </div>
                <div class="col-md-2">
                   <input id="txt_memo" class="form-control" runat ="server" type="text" />
                </div>
            </div>
            </div>
        <div class="container">
            <div class="row">
                <asp:GridView ID="grid_productList" runat="server" class="table table-list table-hover" AutoGenerateColumns="false"
             ShowHeaderWhenEmpty="true" OnRowDeleting="grid_productList_RowDeleting" OnRowDataBound="grid_productList_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="序号">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商品编号">
                    <ItemTemplate>
                        <%#Eval("product_num") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商品名称">
                    <ItemTemplate>
                        <%#Eval("product_name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText ="分类">
                    <ItemTemplate>
                        <%#Eval("category") %>
                    </ItemTemplate>
                </asp:TemplateField>
                    
                <asp:TemplateField HeaderText ="规格/型号">
                    <ItemTemplate>
                        <%#Eval("product_size") %>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText ="单位">
                    <ItemTemplate>
                        <%#Eval("unit") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="供应商">
                    <ItemTemplate>
                        <asp:HiddenField id="hidden_supplierId" runat="server" Value ='<%#Eval("supplier_id") %>'/>
                        <asp:DropDownList runat="server" ID="ddl_supplier" ></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
               <asp:TemplateField HeaderText ="要求交货日期">
                    <ItemTemplate>
                        <input id="txt_deliveryDate" runat="server" type="date" value ='<%#Eval("delivery_date").ToString() == "" || Eval("delivery_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("delivery_date")).ToString("yyyy-MM-dd") %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText ="采购数量">
                    <ItemTemplate>
                        <asp:TextBox id="txt_quantity"  runat="server" style="width:100px" Text='<%#Eval("quantity") %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText ="单价">
                    <ItemTemplate>
                        <asp:TextBox id="txt_unitPrice" runat="server" style="width:50px" Text ='<%#Eval("unit_price") %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText ="总价">
                    <ItemTemplate>
                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText ="备注">
                     <ItemTemplate>
                        <asp:TextBox id="txt_memo" runat="server" type="text" Text='<%#Eval("memo") %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText ="操作">
                    <ItemTemplate>
                        <input type="hidden" id="txt_hidden" runat="server"/>
                        <a href="javascript:">
                            <span onclick="delProduct(this)">删除</span>
                        </a>
                        <%--< href="javascrip:" onclick="delProduct(this)">删除</>--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
            </div>
            <div class="row">
                <%--<asp:Button id ="btnAdd" CssClass="btn btn-primary" Text="添加商品" runat="server" data-toggle="modal" data-target="#div_product"/>--%>
                <input id="btnAdd" type="button"  class="btn btn-primary" value="添加商品" runat="server" data-toggle="modal" data-target="#div_product" />
            </div>
            <div class="row text-center">
               <asp:Button id="btnSave" type="button" class="btn btn-primary" Text="保存" runat="server" OnClick="btnSave_ServerClick" OnClientClick="return SaveClick()"/>
                <input id="btnCancel" type="button" class="btn btn-primary" value="取消" runat="server" onserverclick="btnCancel_ServerClick"/>

            </div>
        </div>
        <div class="modal fade" id="div_product" aria-hidden="true" data-backdrop="static"  >
            <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <%--<h5>请选择商品</h5>--%>
                     <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>                     
                </div>
                <div class="modal-body">
                    <div class="container">
                        <div class="row">
                            <div class="col-md-1 text-right">
                                <span class="text-info">关键字:</span>
                            </div>
                            <div class="col-md-3">
                            <input type="text" id="txt_searchKey" runat="server" class="form-control"/>
                            </div>
                            <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" text="查询" OnClick="btnSearch_Click" />
                        <%--<asp:Button ID="btnSearch" runat="server" class="btn btn-primary" text="查询"  OnClientClick="SearchClick()" />--%>
                        <%--<input id="btnSearch" type="button" class="btn btn-primary" value="查询" onclick="SearchClick()"/>--%>
                                    <%--<input id="btn" type="button" onclick="SearchClick()" value="点击" />--%>

                        </div>
                    </div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grid_product" runat="server" class="table table-list table-hover" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true">
                            <Columns>
                <asp:TemplateField HeaderText ="选择">
                    <ItemTemplate>
                        <asp:CheckBox ID="checkId" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商品编号">
                    <ItemTemplate>
                        <%#Eval("product_num") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商品名称">
                    <ItemTemplate>
                        <%#Eval("product_name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText ="分类">
                    <ItemTemplate>
                        <%#Eval("category") %>
                    </ItemTemplate>
                </asp:TemplateField>
                    
                <asp:TemplateField HeaderText ="规格/型号">
                    <ItemTemplate>
                        <%#Eval("product_size") %>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText ="单位">
                    <ItemTemplate>
                        <%#Eval("unit") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <%--<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick" />--%>
                    </Triggers>
 
                    </asp:UpdatePanel>
    
                </div>
                <div class="modal-footer">
                    <%--<asp:Button ID="btnOk" runat="server" class="btn btn-primary" Text="确定" OnClick="btnOk_Click" OnClientClick="OKClick()"/>--%>
                <input id="btnOk" runat="server" class="btn btn-primary" value="确定" onclick="OKClick()"/>
                </div>
            </div>

            </div>

        </div>
    </div>
    </form>

</body>
</html>
