<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDetailListForJqGrid.aspx.cs" Inherits="purchase_PurchaseDetailListForJqGrid" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link rel="stylesheet" href="../css/bootstrap3.min.css"/>
    <link rel="stylesheet" href="../css/usercontrol.css" />
    <link rel="stylesheet" href="../css/common.css" />
    <link href="../css/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" />
    <link rel="stylesheet" href="../css/jquery-ui.theme.min.css" />
    <link rel="stylesheet" href="../css/ui.jqgrid.css" />
    <link rel="stylesheet" href="../css/jquery-ui.min.css"/>
    <script src="../js/jquery-3.1.1.min.js"></script>
    <script src="../js/i18n/grid.locale-en.js"></script>
    <script src="../js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var screenHeight = document.documentElement.clientHeight;
            var top =document.getElementById("div_rightPanel").offsetTop;
            //document.getElementById("div_gridPanel").style.height = screenHeight - 100 + "px";
            var hight = screenHeight - top -110;
            //document.getElementById("tableList").style.height = hight + "px";
            $("#tableList").jqGrid(
                {
                    //url: "../purchase/PurchaseDetailListForJqGrid.aspx/GetJsonString",
                    url:"../purchase/PurchaseHandler.ashx",
                    mtype: "post",
                    datatype: "json",
                    //colNames: ['所属项目'],
                    //colModel: [{name:'product_id',index:'product_id'}],
                    colNames: ['所属项目', '类别', '申购单号','合同编号','申请日期','采购内容','规格','单位','单价','数量','总价','入库日期','供应商','要求交货日期','负责人','备注'],
                    colModel: [{ name: 'projectName', index: 'projectName' },
                        { name: 'category', index: 'category',classes:'GridCell' },
                        { name: 'order_num', index: 'order_num' },
                        { name: 'contract_id', index: 'contract_id' },
                        { name: 'apply_date', index: 'apply_date', formatter: 'date', formatoptions: {newformat:'Y-m-d'} },
                        { name: 'product_name', index: 'product_name' },
                        { name: 'product_size', index: 'product_size' },
                        { name: 'unit', index: 'unit' },
                        { name: 'unit_price', index: 'unit_price' },
                        { name: 'quantity', index: 'quantity' },
                        { name: 'price', index: 'price' },
                        { name: 'in_warehouse_date', index: 'in_warehouse_date', formatter: 'date', formatoptions: {newformat:'Y-m-d'} },
                        { name: 'supplier', index: 'supplier' },
                        { name: 'delivery_date', index: 'delivery_date' },
                        { name: 'leader', index: 'leader' },
                        { name: 'memo', index: 'memo' }
                    ],
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
                    //autoScroll: false,
                    //shrinkToFit: false,
                    rownumWidth: 40,
                    
                    loadComplete: function () { 
                        //var grid = $("#tableList");
                        //var ids = grid.getDataIDs();
                        ////alert();
                        //for (var i = 0; i < ids.length; i++) {
                        //    grid.setRowData(ids[i], false, { height:40});
                        //}
    	    	        //$('.ui-jqgrid-bdiv').scrollTop(0);
                  //      $("#tableList").setJqGridRowHeight(35);
                        //alert("loadComplete finish");
                    } 

                 });
            $("#tableList").jqGrid('setLabel', 0, '序号', 'labelstyle');
            //$("#tableList").jqGrid('setFrozenColumns');
            //$("#tableList").jqGrid("navGrid", "#pager1", { edit: false, add: true, del: true });
        });
    </script>
    <style type="text/css">
        thead tr {
        height:36px;
        }
        .ui-jqgrid .ui-jqgrid-htable .ui-th-div {
            height:18px;
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
            }


    </style>
</head>
<body>
    <form id="form1" runat="server">
    <UC:Top runat="server" ID="Top" />
    <UC:Left runat="server" ID="Left" />
        <div id="div_rightPanel" class="rightPanel">
                <table id="tableList" class="table-hover"  ></table>
                <div id="pager1"></div>
        </div>


    </form>
</body>
</html>
