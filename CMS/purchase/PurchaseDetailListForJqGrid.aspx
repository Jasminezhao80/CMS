<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDetailListForJqGrid.aspx.cs" Inherits="purchase_PurchaseDetailListForJqGrid" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../css/bootstrap3.min.css"/>
    <link rel="stylesheet" href="../css/usercontrol.css" />
    <link rel="stylesheet" href="../css/common.css" />
    <link rel="stylesheet" href="../css/ui.jqgrid.css" />
    <script src="../js/jquery-3.1.1.min.js"></script>
    <link rel="stylesheet" href="../css/jquery-ui.min.css"/>
    <script src="../js/jquery-ui.min.js"></script>
    <script src="../js/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="../js/jqGrid/grid.locale-cn.js"></script>
    <script type="text/javascript">
        function SearchClick() {
            var ddl_project = $("#ddl_project").val();
            var ddl_supplier = $("#ddl_supplier").val();
            var ddl_isInWarehouse = $("#ddl_isInWarehouse").val();
            var txt_searchKey = $("#txt_searchKey").val();
            //清空表格中数据
            $("#tableList").jqGrid("clearGridData");
            //alert($("#ddl_project").var());
            $("#tableList").jqGrid("setGridParam",
                {
                    url: "../purchase/PurchaseHandler.ashx",
                    postData: { pId: ddl_project, pSupplier: ddl_supplier, pWarehouse: ddl_isInWarehouse, pSearch: txt_searchKey },
                    mtype: "post",
                    datatype: "json"
                }).trigger("reloadGrid");
        }
        function getSupplier() {
            var data;
            $.ajax({
                async: false,
                url: "../purchase/PurchaseDetailListForJqGrid.aspx/GetSupplier",
                type: "post",
                //data: "{}",
                //dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //alert("getSupplier:"+result.d);
                    data = result.d;
                }
            })
            return data;
        }
        function editLink(cellValue, options, rowdata, action) {
            // alert(rowdata.order_id);
            <%--var searchString = <%GetSearchKeys();%>;--%>
            var searchString = $("#ddl_project").val() + "_" +$("#ddl_supplier").val() + "_" + $("#ddl_isInWarehouse").val() + "_" + $("#txt_searchKey").val();
            return "<a href='../purchase/PurchaseDetail.aspx?backType=jqGridDetailList&&id=" + rowdata.order_id + "&&searchKey=" +searchString  +"' style='color:Highlight'>" + rowdata.order_num +" </a > ";
        }
        function load() {
             var screenHeight = document.documentElement.clientHeight;
            var top =document.getElementById("div_rightPanel").offsetTop;
            var hight = screenHeight - top -110;
            var ddl_project = $("#ddl_project").val();
            var ddl_supplier = $("#ddl_supplier").val();
            var ddl_isInWarehouse = $("#ddl_isInWarehouse").val();
            var txt_searchKey = $("#txt_searchKey").val();
            //alert($("#ddl_project").var());
            $("#tableList").jqGrid(
                {
                    //url: "../purchase/PurchaseDetailListForJqGrid.aspx/GetJsonString",
                    url: "../purchase/PurchaseHandler.ashx",
                    postData: {pId:ddl_project,pSupplier:ddl_supplier,pWarehouse:ddl_isInWarehouse,pSearch:txt_searchKey},
                    mtype: "post",
                    datatype: "json",
                     colNames: ['所属项目', '类别', '申购单号','合同编号','申请日期','采购内容','规格','单位','单价','数量','总价','入库日期','供应商','负责人','备注','商品ID','order_id'],
                    colModel: [{ name: 'projectName', index: 'projectName' },
                    { name: 'category', index: 'category', classes: 'GridCell' },
                    { name: 'order_num', index: 'order_num', width:350,formatter: editLink, formatoptions: { baseLinkUrl: "../purchase/PurchaseDetail.aspx", addParam: '&&orderId=10&&backType=jqGridDetailList' } },
                    //{ name: 'order_num', index: 'order_num', formatter: 'showlink', formatoptions: {baseLinkUrl:"../purchase/PurchaseDetail.aspx",addParam:'&&orderId=10&&backType=jqGridDetailList'} },
                    { name: 'contract_id', index: 'contract_id' },
                    { name: 'apply_date', index: 'apply_date', formatter: 'date', formatoptions: { newformat: 'Y-m-d' } },
                    { name: 'product_name', index: 'product_name', editable: false, edittype: 'text', editrules: { required: true } },
                    { name: 'product_size', index: 'product_size', editable: false, edittype: 'text', editrules: { required: true } },
                    { name: 'unit', index: 'unit' },
                    { name: 'unit_price', index: 'unit_price', editable: false, edittype: 'text', editrules: { required: false, number: true } },
                    { name: 'quantity', index: 'quantity', editable: false, edittype: 'text', editrules: { required: true, integer: true } },
                    { name: 'price', index: 'price' },
                    {
                        name: 'in_warehouse_date', index: 'date', formatter: 'date', formatoptions: { newformat: 'Y-m-d' }, editable: false, edittype: 'text',
                        editoptions: {
                            size: 10,
                            dataInit: function (element) {
                                $(element).datepicker({
                                    //changeMonth: true,
                                    //changeYear: true,
                                    dateFormat: 'yy-mm-dd',
                                    showButtonPanel: true,
                                    currentText: '今天',
                                    closeText: '关闭',
                                    monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                                        '七月', '八月', '九月', '十月', '十一月', '十二月'],
                                    dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
                                    weekHeader: '周',
                                    prevText: '上月',
                                    nextText: '下月',
                                    monthNamesShort: ['一月', '二月', '三月', '四月', '五月', '六月',
                                        '七月', '八月', '九月', '十月', '十一月', '十二月']
                                });

                            }
                        }
                    },
                    //{ name: 'supplier', index: 'supplier', editable: true, edittype: 'select', editoptions: { dataUrl:"../purchase/PurchaseHandler.ashx?Function=Supplier"} },
                    { name: 'supplier', index: 'supplier', editable: false, edittype: 'select', editoptions: { value:getSupplier()} },//getSupplier不起作用，没有找到原因
                        //{ name: 'delivery_date', index: 'delivery_date', formatter: 'date', formatoptions: { newformat: 'Y-m-d' } },
                        { name: 'leader', index: 'leader', editable: false, edittype: 'text' },
                        { name: 'memo', index: 'memo',editable: false,edittype:'text' },
                        { name: 'product_id', index: 'product_id', hidedlg: true, hidden: true },
                        { name: 'order_id', index: 'order_id', hidden: true }

                    ],
                    cellurl:"../purchase/PurchaseHandler.ashx",
                    rowNum: 1000,
                    rowList: [100, 200, 300,500,1000],
                    pager: "#pager1",
                    sortname: "order_num",
                    sortorder: 'desc',
                    loadonce: true,
                    //caption: "Json Example",
                    autowidth: true,
                    rownumbers: true,
                    height: hight,
                    rownumWidth:40,
                    cellEdit: true,
                    shrinkToFit: true,
                    ondblClickRow: function (rowid, iRow, iCol, e) {
                        var flag = "<%=editFlag%>";
                        if (flag == "False") {
                            return;
                        }
                        if (iCol == 6) {
                            $("#tableList").setColProp("product_name",{editable:true});
                        }
                        if (iCol == 7) {
                            $("#tableList").setColProp("product_size",{editable:true});
                        }
                        if (iCol == 9) {
                            $("#tableList").setColProp("unit_price",{editable:true});
                        }
                        if (iCol == 10) {
                            $("#tableList").setColProp("quantity",{editable:true});
                        }
                        if (iCol == 12) {
                            $("#tableList").setColProp("in_warehouse_date",{editable:true});
                        }
                        if (iCol == 13) {
                            $("#tableList").setColProp("supplier",{editable:true});
                        }
                        if (iCol == 14) {
                            $("#tableList").setColProp("leader",{editable:true});
                        }
                        if (iCol == 15) {
                            $("#tableList").setColProp("memo",{editable:true});
                        }
                    },
                    afterSaveCell: function (rowid, name, val, iRow, iCol) {
                        var flag = "<%=editFlag%>";
                        //alert(flag);
                        if (flag == "False") {
                            alert("此账号没有修改权限！");
                            return;
                        }
                        switch (name) {
                            case "quantity":
                                    $.ajax({
                                    async: false,
                                    url: "../purchase/PurchaseDetailList.aspx/SaveQuantity",
                                    type: "post",
                                    data: "{'id':"+rowid +",'quantity':" + val+"}",
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                        success: function (result) {
                                            var unitPrice = $("#tableList").getCell(rowid, 'unit_price');
                                            $("#tableList").jqGrid('setCell', rowid, "price", (unitPrice*val).toFixed(2));
                                        $("#tableList").setColProp(name, { editable: { value: "True:False" } });
                                        },
                                    error: function (err) {
                                        alert("修改失败！");
                                    }

                                });
                                break;
                            case "unit_price":
                                $.ajax({
                                    async: false,
                                    url: "../purchase/PurchaseDetailList.aspx/SavePrice",
                                    type: "post",
                                    data: "{'id':"+rowid +",'price':" + val+"}",
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    success: function (result) {
                                        var quantity = $("#tableList").getCell(rowid, 'quantity');
                                        $("#tableList").jqGrid('setCell', rowid, "price", (quantity * val).toFixed(2));
                                        $("#tableList").setColProp(name, { editable: { value: "True:False" } });
                                    },
                                    error: function (err) {
                                        alert("修改失败！");
                                    }
                                });
                                break;
                            case "product_name":
                                var productId = $("#tableList").getCell(rowid, 'product_id');
                                //alert("productId:" + product_id);
                                $.ajax({
                                    async: false,
                                    url: "../purchase/PurchaseDetailList.aspx/UpdateProduct",
                                    type: "post",
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    data: "{'id':" + rowid + ",'productId':" + productId + ",'value':'" + val + "','item':'name'}",
                                    success: function (res) {
                                        $("#tableList").setColProp(name, { editable: {value:"True:False"} });
                                    },
                                    error: function (err) {
                                        alert("修改失败！");
                                    }
                                });
                                break;
                            case "product_size":
                                var productId = $("#tableList").getCell(rowid, 'product_id');
                                $.ajax({
                                    async: false,
                                    url: "../purchase/PurchaseDetailList.aspx/UpdateProduct",
                                    type: "post",
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    data: "{'id':" + rowid + ",'productId':" + productId + ",'value':'" + val + "','item':'size'}",
                                    success: function (res) {
                                        $("#tableList").setColProp(name, { editable: {value:"True:False"} });
                                    },
                                    error: function (err) {
                                        alert("修改失败！");
                                    }
                                });
                                break;
                            case "in_warehouse_date":
                                $.ajax({
                                    async: false,
                                    url: "../purchase/PurchaseDetailList.aspx/SaveWareHouseDate",
                                    type: "post",
                                    data: "{'id':" + rowid + ",'date':'" + val + "'}",
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    success: function (res) {
                                        $("#tableList").setColProp(name, { editable: {value:"True:False"} });
                                    },
                                    error: function (err) {
                                        alert("修改失败！");
                                    }
                                });
                                break;
                            case "supplier":
                                $.ajax({
                                    async: false,
                                    url: "../purchase/PurchaseDetailList.aspx/ChangeValue",
                                    type: "post",
                                    data: "{'id':"+rowid +",'value':'" + val+"','item':'supplier_id'}",
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    success: function (res) {
                                        $("#tableList").setColProp(name, { editable: {value:"True:False"} });
                                    },
                                    error: function (err) {
                                        alert("修改失败！");
                                    }

                                });
                                break;
                            case "memo":
                            case "leader":
                                $.ajax({
                                    async: false,
                                    url: "../purchase/PurchaseDetailList.aspx/ChangeValue",
                                    type: "post",
                                    data: "{'id':"+rowid +",'value':'" + val+"','item':'"+name + "'}",
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8",
                                    success: function (res) {
                                        $("#tableList").setColProp(name, { editable: {value:"True:False"} });
                                    },
                                    error: function (err) {
                                        alert("修改失败！");
                                    }

                                });
                                break;
                        }
                    }
                });
            $("#tableList").jqGrid('setLabel', 0, '序号', 'labelstyle');
        };
        $(document).ready(function () {
            load();
        });
    </script>
     <style type="text/css">
        thead tr {
        height:36px;
        }
         thead tr td {
         font-weight:100;
        font-size:small;
         }
        .ui-jqgrid .ui-jqgrid-htable .ui-th-div {
            height:18px;
            font-weight:bolder;
            font-size:small;
            font-style:normal;
           
        }
        .ui-jqgrid tr.jqgrow {
        height: 36px;
        border: 1px solid #d7d7d7;
        }
        .ui-jqgrid tr.jqgrow td { 
        text-overflow : ellipsis;
        padding-left: 6px;
        padding-right: 2px;
        border-right: dotted 1px #c7c7c7;
        font-weight:100;
        font-size:small;
            }


    </style>
</head>
<body>
    <form id="form1" runat="server">
    <UC:Top runat="server" ID="Top" />
    <UC:Left runat="server" ID="Left" />
        <div id="div_rightPanel" class="rightPanel">
            <div class="container-fluid">
                <div class="row form-inline form-group">
                    <div class="col-md-3 ">

                        <span class="text-info">所属项目:</span>
                        <asp:DropDownList ID="ddl_project" runat="server" CssClass="form-control" style="width:60%" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <span class="text-info">供应商:</span>
                        <asp:DropDownList ID="ddl_supplier" runat="server" CssClass="form-control" style="width:65%"  AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">是否入库:</span>
                        <asp:DropDownList ID="ddl_isInWarehouse" runat="server" 
                            CssClass="form-control" style="width:55%" AutoPostBack="true">
                           <asp:ListItem Value="0">全部</asp:ListItem>
                           <asp:ListItem Value="1">未入库</asp:ListItem>
                           <asp:ListItem Value="2">已入库</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <span class="text-info">关键字:</span>
                        <input type="text" placeholder="采购单号/合同编号/采购内容/规格/类别" id="txt_searchKey" runat="server" class="form-control" style="width:78%" />
                    </div>
                    <div class="col-md-1">
                        <input type="button" id="btn_Search" runat="server" class="btn btn-primary" value="查询" onclick="SearchClick()"/>
                    </div>
                </div>
                <div class="row">
                <table id="tableList" style="font-weight:normal;width:100%"  ></table>
                <div id="pager1"></div>
                </div>
            </div>
        </div>


    </form>
</body>
</html>
