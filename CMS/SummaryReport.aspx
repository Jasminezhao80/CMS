<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SummaryReport.aspx.cs" Inherits="SummaryReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/bootstrap3.min.css" />

    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/usercontrol.css" />
    <script src="js/jquery-3.1.1.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <style>
        .search {
        width:100px;
        height:28px;
        }
        #tab_content div {
        
        }
        #searchTb th {
        width:auto;
        color:#2679B5;
        font-weight:normal;
        }
        #searchTb td {
        width:120px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#tab_title li").on("click", function () {
                //alert($(this).attr("id"));
                document.getElementById("txt_hidden").value =$(this).attr("id");
                //$("#txt_hidden").value = $(this).attr("id");
            });
            var screenHeight = document.documentElement.clientHeight;
            var top = document.getElementById("tab_content").offsetTop;
            var hight = screenHeight - top -10;
            document.getElementById("report1").style.height = hight + "px";
            document.getElementById("report2").style.height = hight + "px";
            document.getElementById("report3").style.height = hight + "px";
            document.getElementById("report4").style.height = hight + "px";
            document.getElementById("report5").style.height = hight + "px";
            document.getElementById("report6").style.height = hight + "px";
            //document.getElementById("grid_detail").offsetTop = 50;

        });
        //function TabClick() {
        //    Session["Active"] = 
        //    alert("test");
        //};
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <UC:Top runat="server" />
        <UC:Left runat="server"/>
        <%--<div style="float:right;width:88%">--%>
            <div class="rightPanel">
                <div class="container">
                    <div class="row form-inline form-group">
                        <div class="col-md-2 text-right">
                            <span class="text-info">本周时间段:</span>
                        </div>
                        <div class="col-md-4">
                            <input type="date" id="weekFrom" runat="server" class="form-control"/>
                            <span class="text-info">～</span>
                            <input type="date" id="weekTo" runat="server" class="form-control" />
                        </div>
                    </div>
                    <div class="row form-inline ">
                        <div class="col-md-2 text-right">
                            <span class="text-info">下周时间段:</span>
                        </div>
                        <div class="col-md-4">
                            <input type="date" id="nextWeekFrom" runat="server" class="form-control"/>
                            <span class="text-info">～</span>
                            <input type="date" id="nextWeekTo" runat="server" class="form-control" />
                        </div>                        
                        <div class="col-md-5 text-right">
     
                            <input id="btnSearch" type="button" class="btn btn-primary" runat="server" value="查询数据" onserverclick="btnSearch_ServerClick" />

                            <input id="btnReport" type="button" class="btn btn-primary" runat="server" value="下载报表" onserverclick="btnReport_ServerClick"/>

                        </div>
                    </div>
                </div>
            <hr />
            <ul id="tab_title" runat="server" class="nav nav-tabs" style="margin-top:15px;list-style:none">
                <li id="li_report1" runat="server" class="active">
                    <a href="#report1" data-toggle="tab" >汇总统计</a>
                </li>
                <li id="li_report2" runat="server">
                    <a href="#report2" data-toggle="tab">延期收款合同</a>
                </li>
                <li id="li_report3" runat="server">
                    <a href="#report3" data-toggle="tab">延期付款合同</a>
                </li>
                <li id="li_report4" runat="server">
                    <a href="#report4" data-toggle="tab">下周预警收款合同</a>
                </li>
                <li id="li_report5" runat="server">
                    <a href="#report5" data-toggle="tab">下周预警付款合同</a>
                </li>
                <li id="li_report6" runat="server">
                    <a href="#report6" data-toggle="tab">本周新订立合同</a>
                </li>
            </ul>

            <div id="tab_content" runat="server" class="tab-content">

                <div id="report1" runat="server" class="tab-pane fade in active" style="overflow-x:scroll;overflow-y:scroll">
                    <asp:GridView ID="GridView1" runat="server" class="table-hover table-list"
                         AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" OnRowDataBound="GridView1_RowDataBound" ShowFooter="True" EnableModelValidation="False">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="类别">
                                <ItemTemplate>
                                    <%#Eval("type") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同总数">
                                <ItemTemplate>
                                    <%#Eval("contractCount") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="本周新增数">
                                <ItemTemplate>
                                    <%#Eval("weekAdded") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="本周完成数">
                                <ItemTemplate>
                                    <%#Eval("weekComplete") %>
                                </ItemTemplate>
                            </asp:TemplateField>  
                    
                            <asp:TemplateField HeaderText="总完成数">
                                <ItemTemplate>
                                    <%#Eval("completeCount") %>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="完成率">
                                <ItemTemplate>
                                    <%# Convert.ToInt32((Convert.ToDouble(Eval("completeCount"))/ Convert.ToDouble(Eval("contractCount")))*100) + "%"%> 
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="延期合同数">
                                <ItemTemplate>
                                    <%#Eval("delayCount") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <%#Eval("memo") %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                
                <div id="report2" runat="server" class="tab-pane fade" style="overflow-x:scroll;overflow-y:scroll">
                    <asp:GridView ID="GridView2" runat="server" class="table-hover table-list"
                         AutoGenerateColumns="false" HeaderStyle-Wrap="false">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同类别">
                                <ItemTemplate>
                                    <%#Eval("projectName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="合同编号" Visible="false" >
                            <ItemTemplate>
                                    <%#Eval("contract_num") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同编号" >
                                <ItemTemplate>
                                    <asp:HyperLink NavigateUrl='<%#Eval("id","~/ContractDetail.aspx?contractId={0}&type=1&tabIndex=2&reportStatus=") + ViewState["reportStatus"]%>' runat="server" Text='<%#Eval("contract_num") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同内容摘要">
                                <ItemTemplate>
                                    <%#Eval("contract_name") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同双方">
                                <ItemTemplate>
                                    <%#Eval("contract_client") %>
                                </ItemTemplate>
                            </asp:TemplateField>  

                            <asp:TemplateField HeaderText="延期内容">
                                <ItemTemplate>
                                    <%#Eval("delayName")%> 
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="应付款日期">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("delayDate")).ToString("yyyy-MM-dd") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="应付金额">
                                <ItemTemplate>
                                    <%#Eval("delayAmount") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="备注" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <%#Eval("memo") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div id="report3" runat="server" class="tab-pane fade" style="overflow-x:scroll;overflow-y:scroll">
                    <asp:GridView ID="GridView3" runat="server" class="table-hover table-list"
                         AutoGenerateColumns="false" HeaderStyle-Wrap="false" RowStyle-Wrap="false">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同类别">
                                <ItemTemplate>
                                    <%#Eval("purchaseType") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="合同编号" Visible="false" >
                            <ItemTemplate>
                                    <%#Eval("contract_num") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同编号">
                                <ItemTemplate>
                                    <asp:HyperLink NavigateUrl='<%#Eval("id","~/ContractDetail.aspx?contractId={0}&type=1&tabIndex=3&reportStatus=") + ViewState["reportStatus"]%>' runat="server" Text='<%#Eval("contract_num") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同内容摘要">
                                <ItemTemplate>
                                    <%#Eval("contract_name") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同双方">
                                <ItemTemplate>
                                    <%#Eval("contract_client") %>
                                </ItemTemplate>
                            </asp:TemplateField>  

                            <asp:TemplateField HeaderText="延期内容">
                                <ItemTemplate>
                                    <%#Eval("delayName")%> 
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="应付款日期">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("delayDate")).ToString("yyyy-MM-dd") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="应付金额">
                                <ItemTemplate>
                                    <%#Eval("delayAmount") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <%#Eval("memo") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
 
               <div id="report4" runat="server" class="tab-pane fade" style="overflow-x:scroll;overflow-y:scroll">
                    <asp:GridView ID="GridView4" runat="server" class="table-hover table-list"
                         AutoGenerateColumns="false" HeaderStyle-Wrap="false" >
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同类别">
                                <ItemTemplate>
                                    <%#Eval("purchaseType") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="所属项目">
                                <ItemTemplate>
                                    <%#Eval("projectName") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同编号" Visible="false">
                                <ItemTemplate>
                                    <%#Eval("contract_num") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同编号">
                                <ItemTemplate>
                                    <asp:HyperLink NavigateUrl='<%#Eval("id","~/ContractDetail.aspx?contractId={0}&type=1&tabIndex=4&reportStatus=") + ViewState["reportStatus"]%>' runat="server" Text='<%#Eval("contract_num") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同内容摘要">
                                <ItemTemplate>
                                    <%#Eval("contract_name") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同双方">
                                <ItemTemplate>
                                    <%#Eval("contract_client") %>
                                </ItemTemplate>
                            </asp:TemplateField>  

                            <asp:TemplateField HeaderText="待收内容">
                                <ItemTemplate>
                                    <%#Eval("delayName")%> 
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="应收款日期">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("delayDate")).ToString("yyyy-MM-dd") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="应收金额">
                                <ItemTemplate>
                                    <%#Eval("delayAmount") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                                <asp:TemplateField HeaderText="备注" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <%#Eval("memo") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                         </Columns>
                    </asp:GridView>
                </div>
               
                <div id="report5" runat="server" class="tab-pane fade" style="overflow-x:scroll;overflow-y:scroll">
                    <asp:GridView ID="GridView5" runat="server" class="table-hover table-list"
                         AutoGenerateColumns="false" HeaderStyle-Wrap="false" RowStyle-Wrap="false">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同类别">
                                <ItemTemplate>
                                    <%#Eval("purchaseType") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="所属项目">
                                <ItemTemplate>
                                    <%#Eval("projectName") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同编号" Visible="false">
                                <ItemTemplate>
                                    <%#Eval("contract_num") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同编号">
                                <ItemTemplate>
                                    <asp:HyperLink NavigateUrl='<%#Eval("id","~/ContractDetail.aspx?contractId={0}&type=1&tabIndex=5&reportStatus=") + ViewState["reportStatus"]%>' runat="server" Text='<%#Eval("contract_num") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同内容摘要">
                                <ItemTemplate>
                                    <%#Eval("contract_name") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同双方">
                                <ItemTemplate>
                                    <%#Eval("contract_client") %>
                                </ItemTemplate>
                            </asp:TemplateField>  

                            <asp:TemplateField HeaderText="待付款内容">
                                <ItemTemplate>
                                    <%#Eval("delayName")%> 
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="应付款日期">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("delayDate")).ToString("yyyy-MM-dd") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="应收金额">
                                <ItemTemplate>
                                    <%#Eval("delayAmount") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <%#Eval("memo") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                         </Columns>
                    </asp:GridView>
                </div>

                <div id="report6" runat="server" class="tab-pane fade" style="overflow-x:scroll;overflow-y:scroll">
                    <asp:GridView ID="GridView6" runat="server" class="table-hover table-list"
                         AutoGenerateColumns="false" HeaderStyle-Wrap="false" RowStyle-Wrap="false">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同类别">
                                <ItemTemplate>
                                    <%#Eval("purchaseType") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="所属项目">
                                <ItemTemplate>
                                    <%#Eval("projectName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                     <asp:TemplateField HeaderText="合同编号" Visible="false">
                                <ItemTemplate>
                                    <%#Eval("contract_num") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="合同编号">
                                <ItemTemplate>
                                    <asp:HyperLink NavigateUrl='<%#Eval("id","~/ContractDetail.aspx?contractId={0}&type=1&tabIndex=6&reportStatus=") + ViewState["reportStatus"]%>' runat="server" Text='<%#Eval("contract_num") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同内容摘要">
                                <ItemTemplate>
                                    <%#Eval("contract_name") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同双方">
                                <ItemTemplate>
                                    <%#Eval("contract_client") %>
                                </ItemTemplate>
                            </asp:TemplateField>  

                            <asp:TemplateField HeaderText="签订日期">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("signature_date")).ToString("yyyy-MM-dd") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="合同金额">
                                <ItemTemplate>
                                    <%#Eval("contract_amount") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="备注">
                                <ItemTemplate>
                                    <%#Eval("memo") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </div>

<%--            <div>
                 <input id="btnReport" type="button" class="btn btn-primary" runat="server" value="导出报表" onserverclick="btnReport_ServerClick"/>
            </div>--%>
        </div>
        <asp:TextBox ID="txt_hidden" runat ="server" OnTextChanged="txt_hidden_TextChanged" AutoPostBack="true" OnUnload="txt_hidden_Unload" Visible="false"></asp:TextBox>
    </form>
</body>
</html>
