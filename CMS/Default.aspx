<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="js/jquery-3.1.1.min.js"></script>
<script type="text/javascript">
    function btnClick() {
        alert("点击事件");
        $.ajax({
            type:"post",
            url:"Default.aspx/Test",
            data:{},
            dataType:"json",
            contentType:"application/json; charset=utf-8",
            beforeSend: function () {
                console.log("执行前");
                alert("执行前1");
            },
            success: function (result) {
                alert("测试");
                console.log("执行后");
                alert(result.d);
            },
            error: function (err,status) {
                alert("失败");
                console.log(err);
                alert(err);
                alert("状态："+status);
            },
            complete: function () {
                alert("完成");
            }
        });
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--<asp:Button type="button" ID="btnText" runat="server" OnClientClick ="btnClick()" Text="测试" />--%>
            <input id="btn" type="button" onclick="btnClick()" value="点击" />
        </div>
    </form>
</body>
</html>
