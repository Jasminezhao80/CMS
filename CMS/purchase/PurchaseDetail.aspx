<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDetail.aspx.cs" Inherits="purchase_PurchaseDetail" %>

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
        function SaveClick() {
            if ($("#txt_orderNum").val() == "") {
                alert("请输入采购单号！");
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
            if ($("#txt_leader").val() == "") {
                alert("请输入负责人！");
                return false;
            }
            var len = 0;
            var flag = true;
            $("#selected_products tr").each(function () {
                //len++;
                len = $(this).find("input[name='maxIndex']").val();//len:当前行的行号，因为控件名字的后缀都是行号，即使删除了某一行，其他的行的控件名字也还是原来的。为了保证查找到的是本行的控件数据
                var name = "txt_deliveryDate" + len;
                var delivery = $(this).find("input[name='" + name + "']").val();
                //if (delivery == "") {
                //    alert("请输入每种商品的交货日期！");
                //    flag = false;
                //    return false;
                //}
                name = "txt_quantity" + len;
                var qty = $(this).find("input[name='" + name + "']").val();
                //if (qty == "" || qty == "0") {
                if (parseFloat(qty).toString() == "NaN"||parseFloat(qty) == 0) {
                    alert("请输入第"+len+"行商品的采购数量！");
                    flag = false;
                    return false;
                }

                //name = "txt_unitPrice" + len;
                //var unitPrice = $(this).find("input[name='" + name + "']").val();
                //if ( unitPrice == "" || unitPrice == "0.00") {
                //    alert("请输入每种商品的单价！");
                //    flag = false;
                //    return false;
                //}
            });
            if (flag == false) {
                return false;
            }
            if (len == 0) {
                alert("请选择商品！");
                return false;
            }
            return true;
        };
        function delProduct(k) {
            if (confirm('确定将此记录删除?')) {
                //alert($(k).parent().parent().parent());
                $(k).parent().parent().parent().remove();
            }
        };
        function ClearSearch() {
            $("#txt_searchKey").val("");
            $("#btnSearch").click();
        };
        $(document).ready(function () {
            $("#btnImportProduct").click(function (e) {
                if ($("#txt_orderNum").val() == "") {
                    alert("请输入采购单号！");
                    e.preventDefault();
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
                if ($("#txt_leader").val() == "") {
                    alert("请输入负责人！");
                    return false;
                }
                return true;
            }); 
            $("#div_product").on('show.bs.modal', function (event) {
                $("#txt_searchKey").val("");
                $("#btnSearch").click();
            });
            //初始化商品明细
            var id = $("#orderId").val();
            if (id != null && id != "") {
                $.ajax({
                    async: false,
                    url: "../purchase/PurchaseDetail.aspx/GetPurchaseDetail",
                    type: "post",
                    data: "{'orderId':" + id + "}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        $("#selected_products").append(result.d);
                        RefreshCount();
                    },
                    Error: function (e) {
                        alert("error:"+e);
                    }
                });
            } else {
                $("#pcount").val("0");
            }
  
        });
        function GetFilePath() {
            $("#txt_file").val($("#fileUpload").val());
        };
        function RefreshCount() {
            $("#selected_products tr").each(function () {
                $("#pcount").val($(this).find("input[name='maxIndex']").val());
            });
            
        }
        function AddNew() {
            document.getElementById("div_addPanel").style.display="";
        }
        function SearchKeyUp() {
            $("#btnSearch").click();
        }
        function SupplierChange(index) {
            var ddlName = "ddl_supplier" + index;
            var txtName = "txt_supplier" + index;
            if ($('[name="'+ ddlName+ '"]').val() == "00") {
                $('[name="'+ ddlName+ '"]').hide();
                $('[name="'+ txtName + '"]').attr("style","width:85px;display:block");
            }
        }
        function OKClick() {
            var selectedProductIds = "";
            $("#selected_products tr").each(function () {
                var td = $(this).find("td");
                var productId = td.eq(0).find("input").val();
                if (selectedProductIds == "") {
                    selectedProductIds = productId;
                }
                else {
                    selectedProductIds += ("," + productId);
                }
            });
            //选择checked products
            var checkedIds = "";
            $("input[type=checkbox]:checked").each(function (i) {
                if (i == 0) {
                    checkedIds = $(this).val();
                }
                else {
                    checkedIds += ("," + $(this).val());
                }
            });
            var maxIndex = $("#pcount").val();
            $.ajax({
                async: false,
                url: "../purchase/PurchaseDetail.aspx/GetCheckedProducts",
                type: "post",
                data: "{ selectedProductIds: '"+selectedProductIds+"', checkedIds: '"+checkedIds+"',rowIndex:'"+ maxIndex + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //alert(result.d);
                    $("#selected_products").append(result.d);
                    RefreshCount();
                },
                error: function (e) {
                    alert("失败");
                    alert(e);
                }
            });
        }
        function AddNewProduct() {
            var num = "";//$("#txt_newNum").val().trim();
            //if (num == "") {
            //    alert("请输入商品编号！");
            //    return false;
            //}
            var name = $("#txt_newName").val();
            if (name == "") {
                alert("请输入商品名称！");
                return false;
            }
            var category = $('#ddl_newCategory option:selected').val();
            var size = $("#txt_newSize").val();
            var material = $("#txt_newMaterial").val();
            var unit = $('#ddl_newUnit option:selected').val();
            $.ajax({
                url:"../purchase/PurchaseDetail.aspx/AddNewProduct",
                type: "post",
                data: "{num:'" + num + "',name:'" + name + "',category:'" + category+"',size:'"+size+"',material:'"+material + "',unit:'"+ unit +"'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result.d == "true") {
                        alert("添加成功！");
                        document.getElementById("div_addPanel").style.display="none";
                        //$("#txt_newNum").val("");
                        $("#txt_newName").val("");
                        $("#txt_newSize").val("");
                        $("#txt_newMaterial").val("");
                        $("#btnSearch").click();
                    }
                    else {
                        alert("商品已经存在，不能重复添加！");
                    }
        
                },
            });
        }
        </script>
</head>
<body>
    <form id="form1" runat="server">
            <UC:Top runat="server"/>
    <UC:Left runat ="server" />
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
                    <span style="color:red">*</span>
                    <span class="text-info ">负责人:</span>
                </div>
                <div class="col-md-3">
                    <input id="txt_leader_old" runat="server" type="hidden" />
                    <input id="txt_leader" class="form-control " runat ="server" type="text"/>
                </div>
                <div class="col-md-2 text-right">
                    <span class="text-info ">总金额:</span>
                </div>
                <div class="col-md-3">
                   <input id="txt_amount" class="form-control" runat ="server" type="text"  readonly="true"/>
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
                <table class="table table-list table-hover" >
                    <thead>
                        <tr style="white-space:nowrap">
                            <th>序号</th>
                            <th>商品名称</th>
                            <th>分类</th>
                            <th>规格/型号</th>
                            <th>单位</th>
                            <th>采购数量</th>
                            <th>供应商</th>
                            <th>要求交货日期</th>
                            <th>单价</th>
                            <th>入库日期</th>
                            <th>备注</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <tbody id="selected_products" style="white-space:nowrap">
                    </tbody>
                </table>
           </div>

            <div class="row">
                <%--<asp:Button id ="btnAdd" CssClass="btn btn-primary" Text="添加商品" runat="server" data-toggle="modal" data-target="#div_product"/>--%>
                <input id="btnAdd" type="button"  class="btn btn-primary" value="添加商品" runat="server" data-toggle="modal" data-target="#div_product" />
                <input id="btnImportProduct" type="button" class="btn btn-primary" value="导入商品" runat="server" data-toggle="modal" data-target="#div_import" />
                <input id="btnDownloadTemplate" type="button" class="btn btn-primary" value="下载模板" runat="server" onserverclick="btnDownloadTemplate_ServerClick" style="float:right;margin-right:20px" />
            </div>
            <div class="row text-center">
               <asp:Button id="btnSave" Visible='<%#Function.CheckButtonPermission("A020102") %>' class="btn btn-primary" Text="保存" runat="server" OnClick="btnSave_ServerClick" OnClientClick="return SaveClick()"/>
                <asp:Button id="btnCancel" class="btn btn-primary" Text="取消" runat="server" OnClick="btnCancel_ServerClick"/>

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
                   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <div class="container-fluid">
                        <div class="row form-group">
                            <div class="col-md-2 text-right">
                                <span class="text-info">关键字:</span>
                            </div>
                            <div class="col-md-6">
                            <input type="text" id="txt_searchKey" runat="server" class="form-control" placeholder="商品编号/名称/规格/材质" onkeyup="SearchKeyUp()"/>
                            </div>
                            <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" text="查询" OnClick="btnSearch_Click" />
                        <%--<asp:Button ID="btnSearch" runat="server" class="btn btn-primary" text="查询"  OnClientClick="SearchClick()" />--%>
                        <%--<input id="btnSearch" type="button" class="btn btn-primary" value="查询" onclick="SearchClick()"/>--%>
                                    <%--<input id="btn" type="button" onclick="SearchClick()" value="点击" />--%>

                        </div>
                        <div class="row form-group" style="height:300px; overflow-y:scroll">
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grid_product" runat="server" class="table table-list table-hover" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="grid_product_Sorting" HeaderStyle-Wrap="false" RowStyle-Wrap="false">
                            <Columns>
                        <asp:TemplateField HeaderText ="选择">
                            <ItemTemplate>
                                <input type="checkbox" runat="server" name="pcheck" value='<%#Eval("id") %>' />
<%--                        <asp:CheckBox ID="checkId" runat="server"/>
                        <input type="hidden" id="hidden_pid" value='<%#Eval("id") %>' runat="server" />--%>
                    </ItemTemplate>
                </asp:TemplateField>
      <%--          <asp:TemplateField HeaderText="商品编号" SortExpression="product_num">
                    <ItemTemplate>
                        <%#Eval("product_num") %>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="商品名称" SortExpression="product_name">
                    <ItemTemplate>
                        <%#Eval("product_name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText ="分类">
                    <ItemTemplate>
                        <%#Eval("category") %>
                    </ItemTemplate>
                </asp:TemplateField>
                    
                <asp:TemplateField HeaderText ="规格/型号" SortExpression="product_size">
                    <ItemTemplate>
                        <%#Eval("product_size") %>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText ="材质">
                    <ItemTemplate>
                        <%#Eval("product_material") %>
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
                        <div class="row form-group text-right">
                    <input id="btnOk" type="button" runat="server" class="btn btn-primary" value="确定" onclick="OKClick()" data-dismiss="modal" aria-hidden="true"/>
                    <input id="btnAdd1" type="button" runat="server" clsss="btn btn-primary" value="添加新商品" onclick="AddNew()" class="btn btn-primary" />

                        </div> 
                    </div>
                </div>
                <div class="modal-footer">
                        <div id="div_addPanel" class="panel panel-default" style="padding:5px 5px;display:none" >
                                    <div class="row form-group">
<%--                            <div class="col-md-2 text-right">
                                <span class="text-info">商品编号:</span>
                            </div>
                            <div class="col-md-4">
                                <input id="txt_newNum" runat="server" type="text" class="form-control"/>
                            </div>--%>
                            <div class="col-md-2">
                                <span class="text-info">商品名称:</span>
                            </div>
                            <div class="col-md-4">
                                <input id="txt_newName" runat="server" type="text" class="form-control"/>
                            </div>
                            <div class="col-md-2 text-right">
                                <span class="text-info">商品分类:</span>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddl_newCategory" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                         <div class="row  form-group">
                            <div class="col-md-2 text-right">
                                <span class="text-info">规格/型号:</span>
                            </div>
                            <div class="col-md-4">
                                <input type="text" runat="server" id="txt_newSize" class="form-control"/>
                            </div>
                            <div class="col-md-2 text-right">
                                <span class="text-info">材质:</span>
                            </div>
                            <div class="col-md-4">
                                <input type="text" runat="server" id="txt_newMaterial" class="form-control"/>
                            </div>
                        </div>
                        <div class="row  form-group">
                            <div class="col-md-2">
                                <span class="text-info">单位:</span>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddl_newUnit" CssClass="form-control" runat="server">

                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12 text-right">
                            <input type="button" value="保存" runat="server" id="btnAddNew" class="btn btn-primary" onclick="AddNewProduct()" />                        
                            </div>
                        </div>
                        </div>
 
                    <%--<asp:Button ID="btnOk" runat="server" class="btn btn-primary" Text="确定" OnClick="btnOk_Click" OnClientClick="OKClick()"/>--%>
                </div>
            </div>

            </div>

        </div>
        <div class="modal fade" id="div_import" aria-hidden="true" data-backdrop="static"  >
            <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                     <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>                     
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row form-inline">
                            <asp:TextBox ID="txt_file" runat="server" CssClass="form-control" style="width:60%"></asp:TextBox>
                            <input id="btnSelectFile" type="button" runat="server" class="btn btn-primary" value="预览" onclick="fileUpload.click()"/>
                            <asp:Button ID="btnImport" CssClass="btn btn-primary" Text="导入" runat="server" OnClick="btnImport_ServerClick" />
                            <asp:FileUpload ID="fileUpload" runat="server" onchange="GetFilePath()" style="visibility:hidden" />
                        </div>
                        <div class="row" style="overflow:auto;height:400px;" >
                        <asp:UpdatePanel ID="UpdatePane2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
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
                           <asp:TemplateField HeaderText="入库日期">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col11" Text='<%# Eval("col11") is null || Eval("col11") is DBNull || Eval("col11").ToString() == "" ? string.Empty : Convert.ToDateTime(Eval("col11")).ToString("yyyy-MM-dd") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_col12" Text='<%#Eval("col12") %>' runat="server" style="width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           </Columns>
                    </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <%--<asp:AsyncPostBackTrigger ControlID="btnImport" EventName="Click" />--%>
                            </Triggers>
                        </asp:UpdatePanel>
                          </div>
                    </div>
                </div> 
                <div class="modal-footer">
                    <asp:Button id="btnImportSave" class="btn btn-primary" Text="生成订单" runat="server"  OnClick="btnImportSave_Click"/>
                </div>
            </div>
            </div>
        </div>
        </div>
     </form>

</body>
</html>
