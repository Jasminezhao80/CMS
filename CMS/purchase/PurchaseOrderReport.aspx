<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderReport.aspx.cs" Inherits="purchase_PurchaseOrderReport" %>

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
        function TabChange(index) {
            $("#txt_tabIndex").val(index);
        }
    </script>
</head>
<body>
    <UC:Top runat="server" />
    <UC:Left runat="server" />
    <form id="form1" runat="server">
    <div class="rightPanel">
        <div class="container">
            <div class="row form-inline">
                <div class="col-md-2 text-right">
                    <span class="text-info">选择时间段:</span>
                </div>
                <div class="col-md-4">
                    <input type="date" id="dateFrom" runat="server" class="form-control"/>
                    <span class="text-info">～</span>
                    <input type="date" id="dateTo" runat="server" class="form-control" />
                </div>
                <div class="col-md-4">
                    <input id="btnSearch" type="button" class="btn btn-primary" runat="server" value="查询数据"  onserverclick="btnSearch_ServerClick" />
                    <input id="btnReport" type="button" class="btn btn-primary" runat="server" value="下载报表" onserverclick="btnReport_ServerClick"/>
                </div>
            </div>
        </div>
    <hr />
        <input type="hidden" value="1" id="txt_tabIndex" runat="server" />
    <ul id="tab_title" runat="server" class="nav nav-tabs" style="margin-top:15px;list-style:none">
           <li id="li_report1" runat="server" class="active" onclick="TabChange(1)">
                <a href="#report1" data-toggle="tab" >人员汇总</a>
            </li>
            <li id="li_report2" runat="server" onclick="TabChange(2)">
                <a href="#report2" data-toggle="tab">项目汇总</a>
            </li>
            <li id="li_report3" runat="server" onclick="TabChange(3)">
                <a href="#report3" data-toggle="tab">周汇总</a>
            </li>
        </ul>
        <div id="tab_content" runat="server" class="tab-content">
            <div id="report1" runat="server" class="tab-pane fade in active">
                <asp:GridView ID="grid1" class="table table-list table-hover" runat="server" AutoGenerateColumns="false"
                     HeaderStyle-Wrap="false" RowStyle-Wrap="false" ShowFooter="true" OnRowDataBound="grid1_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="序号">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="姓名">
                            <ItemTemplate>
                                <%#Eval("leader")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="累计剩余项">
                            <ItemTemplate>
                                <%#Convert.ToInt32(Eval("oldNotFinish"))+ Convert.ToInt32(Eval("finishedOldOrder")) %>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="完成项">
                            <ItemTemplate>
                                <%#Eval("finishedOldOrder") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="完成率">
                            <ItemTemplate>
                                <%#((Convert.ToInt32(Eval("oldNotFinish"))+ Convert.ToInt32(Eval("finishedOldOrder"))) ==0?0: Convert.ToInt32((Convert.ToDouble(Eval("finishedOldOrder"))/(Convert.ToDouble(Eval("oldNotFinish"))+ Convert.ToDouble(Eval("finishedOldOrder"))))*100)) +"%"%>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="本月新增项目数">
                            <ItemTemplate>
                                <%#Eval("monthNewOrder") %>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="完成项">
                            <ItemTemplate>
                                <%#Eval("monthFinishOrder") %>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="月度完成率">
                            <ItemTemplate>
                                <%#((Convert.ToInt32(Eval("monthNewOrder")) == 0?0:Convert.ToInt32(Convert.ToDouble(Eval("monthFinishOrder"))/Convert.ToDouble(Eval("monthNewOrder"))*100)))+"%" %>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="总完成率">
                            <ItemTemplate>
                                <%#Convert.ToInt32((((Convert.ToInt32(Eval("oldNotFinish"))+ Convert.ToInt32(Eval("finishedOldOrder"))+Convert.ToInt32(Eval("monthNewOrder"))) == 0?0:(Convert.ToDouble(Eval("monthFinishOrder"))+ Convert.ToDouble(Eval("finishedOldOrder")))/(Convert.ToDouble(Eval("oldNotFinish"))+ Convert.ToDouble(Eval("finishedOldOrder"))+Convert.ToDouble(Eval("monthNewOrder"))))*100)) + "%"%>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                       
                    </Columns>
                </asp:GridView>
            </div>
            <div id="report2" runat="server" class="tab-pane fade ">
                <asp:GridView ID="grid2" runat="server" class="table table-list table-hover "
                    AutoGenerateColumns="false" HeaderStyle-Wrap="false" RowStyle-Wrap="false" ShowFooter="true" OnRowDataBound="grid2_RowDataBound" >
                    <Columns>
                        <asp:TemplateField HeaderText="序号" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="项目" >
                            <ItemTemplate>
                                <%#Eval("projectName") %>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="总采购项">
                            <ItemTemplate>
                                <%#Eval("totalCount") %>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="完成项">
                            <ItemTemplate>
                                <%#Eval("finishedCount") %>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="完成率">
                            <ItemTemplate>
                                <%#(Convert.ToInt32(Eval("totalCount"))==0?0:Convert.ToInt32(Convert.ToDouble(Eval("finishedCount"))/Convert.ToDouble(Eval("totalCount"))*100)) + "%"%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="备注">
                            <ItemTemplate>
                                
                            </ItemTemplate>
                        </asp:TemplateField>                        
                    </Columns>
                </asp:GridView>
            </div>
            <div id="report3" runat="server" class="tab-pane fade">
                <asp:GridView ID="grid3" AutoGenerateColumns="false" HeaderStyle-Wrap="false"
                     RowStyle-Wrap="false" runat="server" class="table table-list table-hover" ShowFooter="true" OnRowDataBound="grid3_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="周次">
                            <ItemTemplate>
                                <%#Eval("Week") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="采购单数">
                            <ItemTemplate>
                                <%#Eval("OrderCount") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="采购项目数">
                            <ItemTemplate>
                                <%#Eval("DetailCount") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="完成项目数">
                            <ItemTemplate>
                                <%#Eval("FinishCount") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="完成率">
                            <ItemTemplate>
                                <%#Common.PercentageFormat(Convert.ToInt32(Eval("FinishCount")),Convert.ToInt32(Eval("DetailCount"))) %>
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
