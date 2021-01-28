<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OutStoreDetailList.aspx.cs" Inherits="warehouse_OutStoreDetailList" %>

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
            //$('#pop_outStore').on('show.bs.modal', function () {
                //var grid = $("#gridList");
                //$("#txt_store").val(30);
               // $("#txt_leaveStore").val(20);
           // })
        });

        function OutStore_Click(obj) {
            var tr = $(obj).parent().parent();
            var td = $(tr).find("td");
            var store = td.eq(6).text().trim();
            $("#txt_store").val(store);

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
        }
        function find_user(name) {
            $("#" + name + "id").val('');
            var userName = document.getElementById(name).value;
            $.ajax({
                async: false,
                url: "../warehouse/OutStoreDetailList.aspx/GetName",
                type: "post",
                data: "{'username':'" + userName + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d === "") {
                        document.getElementById(name + "Div").style.display = "none";
                        document.getElementById(name + "Show").innerHTML = "";
                    }
                    else {
                        document.getElementById(name + "Div").style.display = "block";
                        var userlist = eval('(' + data.d + ')');
                        //var userlist = data.d;
                        console.log(userlist);

                        document.getElementById(name + "Show").innerHTML = "";
                        //alert(userlist.length);
                        for (var i = 0; i < userlist.length; i++) {
                            $('#' + name + 'Show').append('<option value="' + userlist[i].id + '">' + userlist[i].contract_num + ":" + userlist[i].contract_name + '</option>');
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
            alert("onchange");
            var total = $("#txt_store").val();
            var out = $("#txt_quantity").val(); 
            var leave = $("#txt_leaveStore");
            leave.val(parseInt(total) - parseInt(out));
        };
        function selected(name) {
            var id = $("#" + name + "Show option:selected").val();
            var username = $("#" + name + "Show option:selected").text();
            $("#" + name + "id").val(id);
            $("#" + name).val(username);
            document.getElementById(name + "Div").style.display = "none";
            document.getElementById("contractInfo").innerText = username;        }
    </script>
    
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
                            <asp:TemplateField HeaderText="出库类型">
                                <ItemTemplate>
                                    <%#Eval("typeName") %>
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
                            <asp:TemplateField HeaderText="出库数量">
                                <ItemTemplate>
                                    <%#Eval("quantity") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="借用项目/领用部门">
                                <ItemTemplate>
                                    <%#Eval("contract_id") is null || Eval("contract_id").ToString() == "" ?  Eval("outstore_project") : Eval("contractName")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="领用人">
                                <ItemTemplate>
                                    <%#Eval("outstore_person") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="领用日期" >
                                <ItemTemplate>
                                    <%#Convert.ToDateTime(Eval("outstore_date")).ToString("yyyy-MM-dd") %>
                                    <%-- <input type="date" runat="server" style="width:100%;border:none"  value='<%# Eval("in_warehouse_date") is null || Eval("in_warehouse_date") is DBNull || Eval("in_warehouse_date").ToString() ==""? string.Empty : Convert.ToDateTime(Eval("in_warehouse_date")).ToString("yyyy-MM-dd") %>' onchange='<%#Eval("id","SaveWareHouseDate({0},this)") %>' />--%>
                                </ItemTemplate>
<%--                                <EditItemTemplate>
                                    <input type="text" runat="server" style="width:80px" value='<%#Eval("unit_price") %>' />
                                </EditItemTemplate>--%>
                            </asp:TemplateField>                            
                            </Columns>
                    </asp:gridview>

                </div>
            </div>
 
        </div>
    </form>
</body>
</html>
