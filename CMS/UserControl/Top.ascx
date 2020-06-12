<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Top.ascx.cs" Inherits="UserControl_Top" %>
        <div class="header">
            <a href="#">
        <span id="icon_display" class="glyphicon glyphicon-chevron-right" style="margin-top:50px; visibility:hidden" onclick="LeftDisplay()"></span>
            </a>
    </div>
<script type="text/javascript">
    function LeftDisplay() {
         $(".leftPanel").css("display", "block")
        //$(".leftPanel").show("slow");
        $(".rightPanel").css("width", "91%");
        $("#icon_display").css("visibility","hidden");
    }
</script>