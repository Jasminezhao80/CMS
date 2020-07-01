<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContractList.aspx.cs" Inherits="PurchaseList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/bootstrap3.min.css"/>
    <link rel="stylesheet" href="css/usercontrol.css" />
    <link rel="stylesheet" href="css/common.css" />
    <script src="js/jquery-3.1.1.min.js"></script>
    <style type="text/css">

    </style>
    
</head>
<body>
    <form id="form1" runat="server">
        <UC:Top runat="server" />
        <UC:Left runat="server"/>
        <div class="rightPanel">
            <div class="container-fluid">
                <div class="row form-inline form-group">
                    <div class="col-md-1 text-right">
                        <span class="text-info">合同类型:</span>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddl_category" runat="server" class="form-control" style="width:70%"  OnSelectedIndexChanged="ddl_category_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList> 
                    </div>
                    <div class="col-md-1 text-right">
                    <span class="text-info">合同性质:</span>
                    </div>
                    <div class="col-md-2">
                    <asp:DropDownList ID="ddl_type" runat="server" class="form-control" style="width:70%" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged" AutoPostBack="true" >
                    </asp:DropDownList>                    

                    </div>
                    <div class="col-md-1 text-right">
                        <span class="text-info">所属项目:</span>
                    </div>
                    <div class="col-md-2">
                    <asp:DropDownList ID="ddl_project" runat="server" class="form-control" style="width:100%" OnSelectedIndexChanged="ddl_project_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList> 
                    </div>
      
                </div>
                <div class="row form-inline form-group">
                   <div class="col-md-1 text-right">
                        <span class="text-info">是否履约:</span>
                    </div>
                    <div class="col-md-2">
                    <asp:DropDownList ID="ddl_IsAppointment" runat="server" class="form-control" style="width:70%" OnSelectedIndexChanged="ddl_IsAppointment_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    </div>
                    <div class="col-md-1 text-right">
                        <span class="text-info">是否完成:</span>
                    </div>
                    <div class="col-md-2">
                    <asp:DropDownList ID="ddl_IsComplete" runat="server" class="form-control" style="width:70%" OnSelectedIndexChanged="ddl_IsComplete_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    </div>
                    <div class="col-md-1 text-right">
                        <span class="text-info">关键字:</span>
                    </div>
                    <div class="col-md-2">
                    <input id="txt_contractNum" type ="text" runat="server" class="form-control" style="width:100%" placeholder="编号/内容/项目/对象"/>
                    </div>
                    <div class="col-md-1">
                        <input type="button" name="btSearch" value="查询" id="btSearch" runat="server" class="btn btn-primary" onserverclick="btSearch_ServerClick" />   
                    </div>
                    <div class="col-md-2">
                        <input type="button" name="btNew" value="+添加新合同" id="btNew" runat="server" class="btn btn-primary" onserverclick="btnAdd_ServerClick" />   
                    </div>
                </div>
                <div class="row" style="overflow-x:scroll;overflow-y:scroll">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="true"
                AutoGenerateColumns="False" class="table table-list table-hover" PageSize="10" 
                OnPageIndexChanging="GridView1_PageIndexChanging" OnRowEditing="GridView1_RowEditing" OnRowDataBound="GridView1_RowDataBound"
                  >
                <Columns>
                    <asp:TemplateField HeaderText="序号">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="合同类型">
                        <ItemTemplate>
                            <%#Eval("contractCategory")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="合同性质">
                        <ItemTemplate>
                            <%#Eval("contractType")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="所属项目">
                        <ItemTemplate>
                            <%#Eval("projectName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="合同编号">
                        <ItemTemplate>
                        <asp:HyperLink NavigateUrl='<%#Eval("id","~/ContractDetail.aspx?contractId={0}&search=") + GetSearchQueryStr() %>' runat="server" Text='<%#Eval("contract_num") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="合同内容摘要">
<%--                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_ContractName" Text='<%#Eval("contract_name") %>'></asp:TextBox>
                        </EditItemTemplate>--%>
                        <ItemTemplate>
                            <%#Eval("contract_name") %>
                        </ItemTemplate>                    
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="合同对象">
                        <ItemTemplate>
                            <%#Eval("contract_client") %>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
<%--                    <asp:TemplateField HeaderText="合同金额">
                        <ItemTemplate>
                            <%#Eval("contract_amount") %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="合同签订日期">
                        <ItemTemplate>
                            <%# Eval("signature_date") is null || Eval("signature_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("signature_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="履约" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%--<%# Convert.ToString(Eval("is_appointment"))=="0" ? "Y" : "N" %>--%>
                        <%#Eval("is_appointment") %>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="完成" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate >
                            <%--<%# Convert.ToString(Eval("is_complete"))=="0" ? "Y" : "N" %>--%>
                            <%#Eval("is_complete") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="交货日期">
                        <ItemTemplate>
                            <%# Eval("delivery_date") is null || Eval("delivery_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("delivery_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
<%--                    <asp:TemplateField HeaderText="第一约定日期">
                        <ItemTemplate>
                            <%# Eval("first_date") is null || Eval("first_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("first_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="约定金额">
                        <ItemTemplate>
                            <%#Eval("first_amount") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="付款日期">
                        <ItemTemplate>
                            <%# Eval("first_pay_date") is null || Eval("first_pay_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("first_pay_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="第二约定日期">
                        <ItemTemplate>
                            <%# Eval("second_date") is null || Eval("second_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("second_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="付款日期">
                        <ItemTemplate>
                            <%# Eval("second_pay_date") is null || Eval("second_pay_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("second_pay_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="第三约定日期">
                        <ItemTemplate>
                            <%# Eval("third_date") is null || Eval("third_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("third_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="付款日期">
                        <ItemTemplate>
                            <%# Eval("third_pay_date") is null || Eval("third_pay_date") is DBNull ? string.Empty : Convert.ToDateTime(Eval("third_pay_date")).ToString("yyyy-MM-dd") %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="操作">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
<%--                            <a href="Default.aspx">详细</a>--%>
                            <asp:LinkButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" CommandArgument='<%#Eval("id") %>'>修改</asp:LinkButton>
                            <%--<asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="编辑"></asp:LinkButton>--%>
                            <asp:LinkButton ID="btnDel" runat="server" OnClick="btnDel_Click" CommandArgument='<%#Eval("id") %>' OnClientClick="javascript:return confirm('确定要删除么？')">删除</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle Wrap="False" />
                <RowStyle Wrap="False" />
            </asp:GridView>
                </div>
            </div>
     
        </div>
    </form>
</body>
</html>
