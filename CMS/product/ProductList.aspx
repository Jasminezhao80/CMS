<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductList.aspx.cs" Inherits="product_ProductList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap3.min.css"/>
    <link rel="stylesheet" href="../css/common.css" />
    <link rel="stylesheet" href="../css/usercontrol.css" />
    <script src="../js/jquery-3.1.1.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function addClick() {
            
            $("#txt_id").val(0);
            $("#txt_num").val("");
            $("#txt_name").val("");
            $("#txt_size").val("");
            //$("#ddl_type option[index='1']").attr('selected', 'true');
            //$("#ddl_type option[index='1']").siblings().attr('selected', 'false');
            $("#ddl_type option:first").attr('selected', 'true');
            $("select#ddl_unit option:first").attr('selected', 'true');
        };
        function EditClick(rowIndex, categoryId, unitId, id) {
            $.ajax({
                async: false,
                url: "../product/ProductList.aspx/GetProduct",
                type: "post",
                data: "{ id: " + id +  "}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    var product = JSON.parse(result.d);
                    $("#txt_name").val(product.Name);
                    $("#txt_size").val(product.Size);
                    $("#txt_id").val(product.Id);
                    $("#txt_material").val(product.Material);
                    $(":radio[value=" + product.InstoreFlag + "]").attr('checked', 'checked');
                    $("select#ddl_type option[value=" + product.CategoryId + "]").attr('selected', 'true');
                    $("select#ddl_unit option[value=" + product.UnitId + "]").attr('selected', 'true');
                },
                error: function (e) {
                    alert("失败");
                    alert(e);
                }
            });
        }
        function SaveClick() {
            if ($("#txt_num").val() == "") {
                alert("请输入商品编号！");
                return false;
            }
            if ($("#txt_name").val() == "") {
                alert("请输入商品名称！");
                return false;
            }
            if ($("#txt_size").val() == "") {
                alert("请输入商品规格！");
                return false;
            }
        }
    </script>
</head>
<body>
    <UC:Top runat="server"/>
    <UC:Left runat="server"/>
    <form id="form1" runat="server">
        <div class="rightPanel">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-5">
                        <input id="txt_search" class="form-control" runat="server" placeholder="商品名称/规格/型号/类别/材质"/>
                    </div>
                    <div class="col-md-1">
                        <input type="button" class="btn btn-primary" value="查询" runat="server" onserverclick="Unnamed_ServerClick" />
                    </div>
                    <div class="col-md-5 text-right">
                    <input id="btnAdd" type="button"  class="btn btn-primary" value ="+添加新商品" data-toggle="modal" data-target="#div_detail" onclick="addClick()"/>
                    </div>
                </div>
                <div class="row">
            <asp:GridView ID="grid_product" runat="server" AutoGenerateColumns="false" class="table table-list table-hover" HeaderStyle-Wrap="false"
                 RowStyle-Wrap="false" AllowPaging="true" PageSize="12" OnPageIndexChanging="grid_product_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText ="序号">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText ="商品编号" Visible="false">
                        <ItemTemplate>
                            <%#Eval("product_num") %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="商品名称">
                        <ItemTemplate>
                            <%#Eval("product_name") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText ="类别">
                        <ItemTemplate>
                            <%#Eval("category") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="规格/型号">
                        <ItemTemplate>
                            <%#Eval("product_size") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="材质">
                        <ItemTemplate>
                            <%#Eval("product_material") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText ="单位">
                        <ItemTemplate>
                            <%#Eval("unit") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText ="是否需要入库" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <%#Common.ConvertToYesNo(Eval("instore_flag").ToString()) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText ="操作">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='<%#string.Format("EditClick({0},{1},{2},{3})",new object[] { Container.DataItemIndex + 1, Eval("product_category_id"), Eval("product_unit_id"), Eval("id") })%>' data-toggle="modal" data-target="#div_detail">修改</asp:LinkButton>
                            <%--<asp:LinkButton ID="btnEdit" runat="server" OnClientClick='<%#Eval("product_size", "EditClick(\"{0}\")")%>' data-toggle="modal" data-target="#div_detail">修改</asp:LinkButton>--%>
                            <asp:LinkButton ID="btnDel" runat="server" OnClick="btnDel_Click" CommandArgument='<%#Eval("id") %>' OnClientClick="javascript:return confirm('确定要删除么？')">删除</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:CommandField ShowDeleteButton="true" ShowEditButton="true" />--%>

                </Columns>
            </asp:GridView>
                </div>
            </div>

            <div id="div_detail" class="modal fade" aria-hidden="true" data-backdrop="static" >         
                <div class="modal-dialog">
                    <div class="modal-content">
                    <div class="modal-header">
                        <input id="txt_id" runat="server" type ="text"  hidden="hidden"/>
                        <button class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="container" >
                            <%--<div class="row form-group">
                                <div class="col-md-1 text-right">
                                    <span class="text-info">商品编号:</span>
                                </div>
                                <div class="col-md-3">
                                    <input id="txt_num" runat="server" type="text" class="form-control"/>
                                </div>
                            </div>--%>
                            <div class="row form-group">
                                <div class="col-md-2 text-right">
                                    <span class="text-info">商品名称:</span>
                                </div>
                                <div class="col-md-3">
                                    <input id="txt_name" runat="server" type="text" class="form-control"/>
                                </div>
                            </div>
                             <div class="row form-group">
                                <div class="col-md-2 text-right">
                                    <span class="text-info">商品类别:</span>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddl_type" class="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-2 text-right">
                                    <span class="text-info">规格/型号:</span>
                                </div>
                                <div class="col-md-3">
                                    <input id="txt_size" runat="server" type="text" class="form-control"/>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-2 text-right">
                                    <span class="text-info">材质:</span>
                                </div>
                                <div class="col-md-3">
                                    <input id="txt_material" runat="server" type="text" class="form-control"/>
                                </div>
                            </div> 
                            <div class="row form-group">
                                <div class="col-md-2 text-right">
                                    <span class="text-info">单位:</span>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddl_unit" runat="server" class="form-control" style="width:80px">

                                    </asp:DropDownList>
                                </div>
                            </div>             
                            <div class="row form-group">
                                <div class="col-md-2 text-right">
                                    <span class="text-info">是否需要入库:</span>
                                </div>
                                <div class="col-md-3">
                                    <input type="radio" name="radio" value="1"  checked="checked"/>是
                                    <input type="radio" name="radio" value="0" /> 否
                                </div>
                            </div>                      
                        </div>

                        
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-default">关闭</button>
                        <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="保存" OnClick="btnSave_Click" OnClientClick="return SaveClick()"></asp:Button>
                    </div>
                </div>
            
                  </div>
        </div>
        
        </div>
    </form>
</body>
</html>
