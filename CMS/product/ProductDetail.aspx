<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductDetail.aspx.cs" Inherits="product_ProductDetail" %>

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
</head>
<body>
    <UC:Top runat="server" />
    <UC:Left runat="server" />
    <form id="form1" runat="server">
        <div class="rightPanel">
            <div class="container" >
                <div class="row">
                    <div class="col-md-2 text-right">
                        <span class="text-info">商品编码:</span>
                    </div>
                    <div class="col-md-4">
                        <input type="text" class="form-control"/>
                    </div>
                    <div class="col-md-2 text-right">
                        <span class="text-info">商品名称:</span>
                    </div>
                    <div class="col-md-4">
                        <input type="text" class="form-control"/>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
