﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Left.ascx.cs" Inherits="UserControl_Left" %>
    <div class="leftPanel" id="div_Left" >
        <ul style="list-style:none;padding:0px;margin:0px; height:500px;width:100%">
            <li>
                <a href="javascript:" class="menu-item">
                    <span>合同管理</span>
                    <span class="glyphicon glyphicon-chevron-left" style="float:right;" onclick="LeftOut()"></span>
                </a>
                <ul style="list-style:none;padding-left:20px">
                    <li style="margin-left:1px">
                        <a href="../ContractList.aspx" class="menu-item-detail">
                            <span class="text">合同管理明细</span> 
                        </a>
                    </li>
                    <li>
                        <a href="../SummaryReport.aspx" class="menu-item-detail">
                            合同汇总统计</a>
                    </li>

                </ul>
            </li>
            <li>
                <a href="javascript:" class="menu-item">
                    <span>采购管理</span>
                </a>
                <ul style="list-style:none;padding-left:20px">
                    <li><a href="../purchase/PurchaseList.aspx" class="menu-item-detail">采购订单管理</a></li>
                    <li><a href="../purchase/PurchaseDetailList.aspx" class="menu-item-detail">采购订单明细</a></li>
                    <li><a href="../purchase/PurchaseDetailListForJqGrid.aspx" class="menu-item-detail">采购明细_New</a></li>
                    <li><a href="../purchase/PurchaseOrderReport.aspx" class="menu-item-detail">采购订单汇总</a></li>
                    <li><a href="../product/ProductList.aspx" class="menu-item-detail">商品管理</a></li>
                </ul>
            </li>
            <li>
                <a href="javascript:" class="menu-item">
                    <span>基础信息管理</span>
                </a>
                <ul style="list-style:none;padding-left:20px">
                    <li>
                        <a href="../InfoManager.aspx" class="menu-item-detail">项目管理</a>
                    </li>
                </ul>
            </li>
        </ul>
    </div>
<script type="text/javascript">
    $(function () {
    $(".menu-item").click(function () {
        $(this).next("ul").slideToggle();
    });
});
    function LeftOut() {
        //$("#div_Left").css("margin-left", "-80px");
        //$(".leftPanel").css("display", "none")
        $(".leftPanel").hide();
        $(".rightPanel").css("width","100%");
        $("#icon_display").css("visibility","visible");

    }
</script>