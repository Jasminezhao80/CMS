<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OutStoreManager.aspx.cs" Inherits="warehouse_OutStoreManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap3.min.css" />
    <link rel="stylesheet" href="../css/common.css" />
    <link rel="stylesheet" href="../css/usercontrol.css" />
        <script src ="../js/jquery-3.1.1.min.js"></script>
    <script src ="../js/bootstrap.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#pop_outStore').on('show.bs.modal', function () {
                Clear();
            })
        });
        function Clear() {
            $("#txt_quantity").val("");
            $("#txt_leaveStore").val("");
            $("#txt_contractId").val("");
            $("#txt_contract").val("");
            $("#txt_person").val("");
            $("#txt_part").val("");
            $("#txt_borrowProject").val("");
            document.getElementById("contractDiv").style.display = "none";
            document.getElementById("contractInfo").innerText = "";
        };

        function OutStore_Click(obj, productId) {
            var tr = $(obj).parent().parent();
            var td = $(tr).find("td");
            var store = td.eq(6).text().trim();
            $("#txt_store").val(store);
            $("#txt_productid").val(productId);

        };
        function TypeChange(){
            var ddl = $('#ddl_outStoreType').val();
            var div_sale = document.getElementById('div_Sale');
            var div_produce = document.getElementById('div_Produce');
            var div_borrow = document.getElementById('div_Borrow');
            if (ddl == '99') {
                div_borrow.style.display = 'none';
                div_produce.style.display = 'none';
                div_sale.style.display = 'block';
            }
            if (ddl == '100') {
                div_borrow.style.display = 'none';
                div_produce.style.display = 'block';
                div_sale.style.display = 'none';
            }
            if (ddl == '101') {
                div_borrow.style.display = 'block';
                div_produce.style.display = 'none';
                div_sale.style.display = 'none';
            }
            Clear();
        }
        function find_contract() {
            $("#txt_contractId").val('');
            var pname = document.getElementById("txt_contract").value;
            
            $.ajax({
                async: false,
                url: "../warehouse/OutStoreManager.aspx/GetName",
                type: "post",
                data: "{'searchKey':'" + pname + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d === "") {
                        document.getElementById("contractDiv").style.display = "none";
                        document.getElementById("contractShow").innerHTML = "";
                    }
                    else {
                        document.getElementById("contractDiv").style.display = "block";
                        var list = eval('(' + data.d + ')');
                        //var userlist = data.d;
                        document.getElementById("contractShow").innerHTML = "";
                        //alert(userlist.length);
                        for (var i = 0; i < list.length; i++) {
                            $('#contractShow').append('<option value="' + list[i].id + '">' + list[i].contract_num + ":" + list[i].contract_name + '</option>');
                        }
                    }

        },
                error: function (data, error, msg) {
                    console.log(data);
                    console.log(error);
                    console.log(msg);
        }
    });
        }
        function store_change() {
            var total = $("#txt_store").val();
            var out = $("#txt_quantity").val(); 
            var leave = $("#txt_leaveStore");
            leave.val(parseInt(total) - parseInt(out));
            if (parseInt(leave.val()) < 0)
            {
                alert("出库数量不能大于现有库存数量！");
                $("#txt_quantity").focus();
            }
        };
        function selected() {
            var selectList = $("#contractShow option:selected");
            var id = selectList.val();
            var name = selectList.text();
            $("#txt_contractId").val(id);
            $("#txt_contract").val(name);
            document.getElementById("contractDiv").style.display = "none";
            document.getElementById("contractInfo").innerText = name;
        }; 
        function submit_click() {
            var type = $("#ddl_outStoreType").val();
            var person = $("#txt_person");
            var date = $("#txt_outStoreTime");
            if (person.val() == "") {
                alert("请输入领用人！");
                person.focus();
                return false;
            }
            if (type == '99')//销售
            {
                var contract = $("#txt_contract");
                if (contract.val() == "") {
                    alert("请输入合同！");
                    contract.focus();
                    return false;
                }
            }
            if (type == '100')//领用
            {
                var project = $("#txt_part");//领用部门
                if (project.val() == "") {
                    alert("请输入领用部门！");
                    project.focus();
                    return false;
                }
            }
            if (type == '101')//借用
            {
                var depart = $("#txt_borrowProject");//借用项目
                if (depart.val() == "") {
                    alert("请输入借用项目！");
                    depart.focus();
                    return false;
                }
            }
            if (date.val() == "") {
                alert("请输入出库日期！");
                date.focus();
                return false;
            }
            var total = $("#txt_store").val();
            var out = $("#txt_quantity");
            if (out.val() == "") {
                alert("请输入出库数量！");
                out.focus();
                return false;
            }
            if ((parseInt(total) - parseInt(out)) < 0) {
                alert("出库数量不能大于现有库存数量！");
                out.focus();
                return false;
            }
            alert("检验成功！");
            return true;
        };
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <UC:Top runat="server"/>
        <UC:Left runat="server" />
        <div class="rightPanel">
            <div class="container">
                <div class="row">
                    <asp:gridview runat="server" ID="gridList" AutoGenerateColumns="false" class="table table-list table-hover" >
                        <Columns>
                            <asp:TemplateField HeaderText="序号" >
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
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
                            <asp:TemplateField HeaderText="总入库数量">
                                <ItemTemplate>
                                    <%#Eval("inTotal") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="总出库数量">
                                <ItemTemplate>
                                    <%#Eval("outTotal") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="现库存数量">
                                <ItemTemplate>
                                    <%#Convert.ToInt32(Eval("inTotal"))-Convert.ToInt32(Eval("outTotal")) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                            <asp:LinkButton ID="btnOut" runat="server" data-toggle="modal" data-target="#pop_outStore"  OnClientClick='<%#Eval("product_id","OutStore_Click(this,{0})") %>' Visible='<%#Function.CheckButtonPermission("A040301") %>'>出库</asp:LinkButton>
                                    </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:gridview>

                </div>
            </div>
            <div class="modal fade" id="pop_outStore" onload="">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <span class="text-info">商品出库</span>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>                     

                        </div>
                        <div class="modal-body">
                            <div class="container-fluid">
                                <div class="row form-group">
                                    <div class="col-md-2 text-right">
                                        <span class="text-info">出库类型:</span>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddl_outStoreType" onchange="TypeChange()">
                                            
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 text-right">
                                        <span class="text-info">领用人:</span>
                                    </div>
                                    <div class="col-md-4">
                                        <input class="form-control" type="text" id="txt_person" runat="server" />
                                    </div>
                                </div>

                                <div id="div_Sale" class="row form-group" runat="server" style="display:none" >
                                    <div class="col-md-2 text-right">
                                        <span class="text-info">合同编号:</span>
                                    </div>
                                    <div class="col-md-4">
                                        <input  class="form-control" id="txt_contract" value="" runat="server" oninput="find_contract()" />
                                        
                                        <input type="hidden" id="txt_contractId" runat="server"  value=""/>
                                        <input type="hidden" id="txt_productid" runat="server" value=""/>
                                        <div id="contractDiv" style="display: none;position: fixed;z-index:9;width:400px">
					                        <select class="form-control" multiple="multiple" id="contractShow" ondblclick="selected()" style="width: 500px; margin-left: 0px; margin-top: 0px;" >
					                        </select>
				                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <span id="contractInfo"></span>
                                    </div>
                                </div>
                                <div id="div_Produce" class="row form-group" runat="server" style="display:none">
                                     <div class="row form-group">
                                        <div class="col-md-2 text-right">
                                            <span class="text-info">领用部门:</span>
                                        </div>
                                        <div class="col-md-4">
                                            <input class="form-control" type="text" id="txt_part" runat="server" />
                                        </div>

                                    </div>

 
                                </div>
                                <div id="div_Borrow" class="row form-group" runat="server" >
                                    <div class="row form-group">
                                        <div class="col-md-2 text-right">
                                            <span class="text-info">借用项目:</span>
                                        </div>
                                        <div class="col-md-4">
                                            <input class="form-control" type="text" id="txt_borrowProject" runat="server" />
                                        </div>
 
                                </div>
                            </div>

                                <div class="row form-group">
                                    <div class="col-md-2 text-right">
                                        <span class="text-info">出库日期:</span>
                                    </div>
                                    <div class="col-md-4">
                                        <input class="form-control" type="date" id="txt_outStoreTime" runat="server" />
                                    </div>
                                    <div class="col-md-2 text-right">
                                        <span class="text-info">现有库存:</span>
                                    </div>
                                    <div class="col-md-4">
                                        <input class="form-control" type="text" id="txt_store" readonly="true" runat="server" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-2 text-right">
                                        <span class="text-info">出库数量:</span>
                                    </div>
                                    <div class="col-md-4">
                                        <input class="form-control" type="text" id="txt_quantity" onblur="store_change()" onkeyup="store_change()" runat="server" />
                                    </div>
                                    <div class="col-md-2 text-right">
                                        <span class="text-info">剩余库存:</span>
                                    </div>
                                    <div class="col-md-4">
                                        <input class="form-control" type="text" id="txt_leaveStore" readonly="true" runat="server" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-6 text-right">
                                        <input type="submit" id="btn_submit" class="btn btn-primary" value="提交" runat="server" onclick="return submit_click();" onserverclick="btn_submit_ServerClick" />
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

