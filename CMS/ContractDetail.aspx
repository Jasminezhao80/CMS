<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContractDetail.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/bootstrap3.min.css"/>
    <link rel="stylesheet" href="css/usercontrol.css" />
    <link rel="stylesheet" href="css/common.css"/>
    <script src="js/jquery-3.1.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function SaveClick() {
            alert("保存成功！");
            window.location.href = 'ContractList.aspx?contractNum=' + $("#contractNum").val();
        };
        function selectChange(ddl,txtAmount) {
            var totalSum = $("#totalSum").val();
            var selectedValue = $("#" + ddl).val();
            if (totalSum != "" && selectedValue != "0") {
                $("#" + txtAmount).val((totalSum * selectedValue).toFixed(2));
            }
        }
    </script>
    <style type="text/css">
 
 
    </style>
    <script>
        function SetDefaultAmount(amount, payAmount)
        {
            var payValue = $("#" + payAmount).val();
            if (payValue == "" || parseInt(payValue) == 0 )
            {
                //alert($("#" + amount).val());
                //$("#" + payAmount).value = $("#" + amount).val();
                document.getElementById(payAmount).value= $("#" + amount).val();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <UC:Top runat="server"/>
        <UC:Left runat="server"></UC:Left>
        <div class="rightPanel">    
           <div class="container">
               <div class="row">
                   <span id="status" runat="server" class="text-info"></span>
               </div>
                <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">合同类型:</span>
                    </div>
                   <div class="col-md-3">
                        <asp:DropDownList ID="ddl_category" runat="server" class="form-control" OnSelectedIndexChanged="ddl_category_SelectedIndexChanged" AutoPostBack="true" >
                        </asp:DropDownList>                   
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">合同性质:</span>
                    </div>              
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddl_type" runat="server" class="form-control" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>

                    </div>
               </div>
               <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">所属项目:</span>
                    </div>
                   <div class="col-md-3">
                        <asp:DropDownList ID="ddl_project" runat="server" class="form-control">
                        </asp:DropDownList>                   
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">合同编号:</span>
                    </div>              
                    <div class="col-md-3">
                        <input type="text" id="contractNum" class="form-control" runat="server"/>
                    </div>               

               </div>
               <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">合同内容摘要:</span>
                    </div>
                   <div class="col-md-3">
                        <input id="contractName" runat="server" type="text" class="form-control"/>                  
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">合同双方:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="contractClient" type="text" class="form-control" runat="server"/>
                    </div>               

               </div>
              <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">是否需要交货:</span>
                    </div>
                   <div class="col-md-3">
                        <asp:DropDownList ID="ddl_isDelivery" runat="server" class="form-control" style="width:47%">
                        </asp:DropDownList>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">交货完成时间:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="deliveryDate" type="date" runat="server" class="form-control"/>
                    </div>               

               </div>    
               <div class="row form-group">
 
                    <div class="col-md-2 text-right">
                        <span class="text-info">合同签订时间:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="signatureDate" type="date" runat="server" class="form-control"/>
                    </div>               
                    <div class="col-md-2 text-right">
                        <span class="text-info">负责人:</span>
                    </div>
                   <div class="col-md-3 ">
                        <input id="txt_leader" runat="server" type="text" class="form-control"/>
                   </div>               

               </div>

               <div class="row form-group">
                   <div class="col-md-2 text-right">
                        <span class="text-info">合同总金额:</span>
                    </div>
                   <div class="col-md-3 form-inline">
                       <asp:DropDownList ID="ddl_moneyTypy" runat="server" class="form-control" style="width:35%">
                        </asp:DropDownList>
                       <input id="totalSum" type="text" class="form-control" style="width:63%" runat="server"/>
                   </div>
                  <div class="col-md-2 text-right">
                        <span class="text-info">备注:</span>
                    </div>
                   <div class="col-md-3">
                        <textarea id="txt_memo" runat="server" type="text" class="form-control"/>

                   </div>                  
               </div>
               <hr style="border:1px solid lightgray" />
  
               <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">第一阶段约定付款日期:</span>
                    </div>
                   <div class="col-md-3 ">
                        <input id="firstDate" runat="server" type="date" class="form-control"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">第一阶段实际付款日期:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="firstPayDate" runat="server" type="date" onblur="SetDefaultAmount('firstAmount','txt_firstPay')" class="form-control"/>
                    </div>               

               </div>
               <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">约定付款金额:</span>
                    </div>
                   <div class="col-md-3 form-inline ">
                        <asp:DropDownList ID="ddl_firstAmount" runat="server" CssClass="form-control" style="width:35%;text-align:left" onchange="selectChange('ddl_firstAmount','firstAmount')"></asp:DropDownList>
                        <input id="firstAmount" runat="server" type="text" class="form-control" style="width:63%"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">实际付款金额:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="txt_firstPay" runat="server" type="text" class="form-control"/>
                    </div>               

               </div>
               <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">第二阶段约定付款日期:</span>
                    </div>
                   <div class="col-md-3 ">
                        <input id="secondDate" runat="server" type="date" class="form-control"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">第二阶段实际付款日期:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="secondPayDate" runat="server" type="date" onblur="SetDefaultAmount('secondAmount','txt_secondPay')" class="form-control"/>
                    </div>               

               </div>
               <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">约定付款金额:</span>
                    </div>
                   <div class="col-md-3 form-inline">
                        <asp:DropDownList ID="ddl_secondAmount" runat="server" CssClass="form-control" style="width:35%;" onchange="selectChange('ddl_secondAmount','secondAmount')"></asp:DropDownList>
                        <input id="secondAmount" runat="server" type="text" class="form-control" style="width:63%"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">实际付款金额:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="txt_secondPay" runat="server" type="text" class="form-control"/>
                    </div>               

               </div>
           
              <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">第三阶段约定付款日期:</span>
                    </div>
                   <div class="col-md-3 ">
                        <input id="thirdDate" runat="server" type="date" class="form-control"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">第三阶段实际付款日期:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="thirdPayDate" runat="server" type="date" onblur="SetDefaultAmount('thirdAmount','txt_thirdPay')" class="form-control"/>
                    </div>               

               </div>
               <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">约定付款金额:</span>
                    </div>
                   <div class="col-md-3 form-inline">
                        <asp:DropDownList ID="ddl_thirdAmount" runat="server" CssClass="form-control" style="width:35%;" onchange="selectChange('ddl_thirdAmount','thirdAmount')"></asp:DropDownList>

                        <input id="thirdAmount" runat="server" type="text" class="form-control" style="width:63%"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">实际付款金额:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="txt_thirdPay" runat="server" type="text" class="form-control"/>
                    </div>               

               </div>           

              <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">第四阶段约定付款日期:</span>
                    </div>
                   <div class="col-md-3 ">
                        <input id="fourthDate" runat="server" type="date" class="form-control"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">第四阶段实际付款日期:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="fourthPayDate" runat="server" type="date" onblur="SetDefaultAmount('fourthAmount','txt_fourthPay')" class="form-control"/>
                    </div>               

               </div>
               <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">约定付款金额:</span>
                    </div>
                   <div class="col-md-3 form-inline ">
                        <asp:DropDownList ID="ddl_fourthAmount" runat="server" CssClass="form-control" style="width:35%;" onchange="selectChange('ddl_fourthAmount','fourthAmount')"></asp:DropDownList>
                        <input id="fourthAmount" runat="server" type="text" class="form-control" style="width:63%"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">实际付款金额:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="txt_fourthPay" runat="server" type="text" class="form-control"/>
                    </div>               

               </div>    

                             <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">质保金约定支付日期:</span>
                    </div>
                   <div class="col-md-3 ">
                        <input id="lastDate" runat="server" type="date" class="form-control"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">质保金实际支付日期:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="lastPayDate" runat="server" type="date" onblur="SetDefaultAmount('lastAmount','txt_lastPay')" class="form-control"/>
                    </div>               

               </div>
               <div class="row form-group">
                    <div class="col-md-2 text-right">
                        <span class="text-info">约定付款金额:</span>
                    </div>
                   <div class="col-md-3 form-inline">
                        <asp:DropDownList ID="ddl_lastAmount" runat="server" CssClass="form-control" style="width:35%;" onchange="selectChange('ddl_lastAmount','lastAmount')"></asp:DropDownList>
                        <input id="lastAmount" runat="server" type="text" class="form-control" style="width:63%"/>
                   </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">实际付款金额:</span>
                    </div>              
                    <div class="col-md-3">
                        <input id="txt_lastPay" runat="server" type="text" class="form-control"/>
                    </div>               

               </div>
              <div class="row text-center">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn btn-primary" OnClick="btnSave_Click"  />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="btn btn-primary" OnClick ="btnCancel_Click" />

              </div>     
           </div>
        </div>
    </form>

</body>
</html>
