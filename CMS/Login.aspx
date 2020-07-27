<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="css/bootstrap3.min.css"/>
    <script src="js/jquery-3.1.1.min.js" type="text/javascript"></script>
    <style>
    body {
    width:100%;
    height:100%;
    background-color:Highlight;
    }
    #loginForm {
    width:400px;
    height:400px;
    background-color:darkgray;
    margin:200px auto;
    padding:20px 30px;
    }
</style>
    <script>
        function btnLogin_onClick() {
            if ($("#txtUserName").val() == "") {
                alert("请输入用户名！");
                return false;
            }

            //if ($("#txtUserName").val() != "admin") {
            //    alert("用户名不存在！");
            //    return false;
            //}

            if ($("#txtPass").val() == "") {
                alert("请输入密码！")
                return false;
            }

            //if ($("#txtPass").val() != "123") {
            //    alert("密码不正确！")
            //    return false;
            //}
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div id="loginForm" >
                <div>
                   <h2>用户登录</h2>
                </div>
                <div class="form-group has-success has-feedback">
                    <input type="text" class="form-control"  runat="server" id="txtUserName" placeholder="用户名"/>
                    <span class="glyphicon glyphicon-user form-control-feedback"></span>
                </div>
                <div class="form-group has-success has-feedback">
                    <input type="password" class="form-control" id="txtPass" placeholder="密码" runat="server" />
                    <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                </div>
                <div class="form-check">
                <label class="form-check-label">
                    <input type="checkbox" class="form-check-input" value=""/>
                    <span style="font-size:15px;">记住我</span>
                </label>
            </div>
            <div>
                <asp:button ID="btnLogin" runat="server" Text="登录"  class="btn btn-primary btn-block"  OnClick="btnLogin_Click" OnClientClick="return btnLogin_onClick()"></asp:button>
            </div>
            <div>
                <a href="#" style="display:inline;font-size:15px;">还未注册</a>
                <a href="#" style="display:inline;font-size:15px;margin-left:185px;">忘记密码？</a>
            </div>
            </div>
        </div>
    </form>
</body>
</html>
