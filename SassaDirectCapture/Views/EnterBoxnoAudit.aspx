<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="EnterBoxnoAudit.aspx.cs" Inherits="SASSADirectCapture.Views.EnterBoxnoAudit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Box Barcode</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/my_ecm.js"></script>
</head>
<body>
    <form runat="server">
        <div class="container col-md-12">
            <div class="form-group">
                <h2 class="text-center form-group">Enter Box Barcode</h2>
                <p runat="server" id="pHeading">Enter or Scan the Box Barcode that will be used next.</p>
                <p>NOTE: FILES DUE FOR DESTRUCTION ARE NOT BOXED AGAIN.</p>
            </div>
        </div>
        <div class="col-xs-12">
            <%--<asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Registry Type:"></asp:Label>--%>
            <asp:Label ID="lblPickBoxType" CssClass="alert-danger" runat="server" class="col-sm-3 control-label text-left" Text="You have to select a valid Registry Type first." Visible="false"></asp:Label>
        </div>
        <br />
        <div class="form-horizontal col-sm-12">
            <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Box Barcode:"></asp:Label>
            <asp:TextBox ID="txtBoxBarcode" runat="server" CssClass="form-control col-sm-3" placeholder="Enter here..." OnTextChanged="txtBoxBarcode_TextChanged"></asp:TextBox>
            <asp:Label ID="lblTooShort" CssClass="alert-danger" runat="server" class="col-sm-3 control-label text-left" Text="Box Barcode did not scan correctly or is too short." Visible="false"></asp:Label>
        </div>
        <asp:TextBox ID="txtBoxType" runat="server" CssClass="form-control col-sm-3" placeholder="Enter here..." OnTextChanged="txtBoxBarcode_TextChanged" Visible="false"></asp:TextBox>
        <div class="form-horizontal col-sm-12 pull-right" style="width: 100% !important">
            <asp:Button ID="btnCancel" Visible="true" runat="server" Text="Cancel" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnCancel_Click" />
            <asp:Button ID="btnUpdateBox" Visible="true" runat="server" Text="Update" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnUpdateBox_Click" />
        </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            var pressed = false;
            var chars = [];
            $(window).keypress(function (e) {
                if (e.which === 13) {
                    console.log("Prevent form submit.");
                    e.preventDefault();
                }
                else {
                    chars.push(String.fromCharCode(e.which));
                }
                console.log(e.which + ":" + chars.join("|"));
                if (pressed == false) {
                    setTimeout(function () {
                        if (chars.length >= 3) {
                            var barcode = chars.join("");
                            $("#txtBoxBarcode").val(barcode);
                            $("#btnUpdateBox").trigger("click");
                        }
                        chars = [];
                        pressed = false;
                    }, 300);
                }
                pressed = true;
            });
        });
    </script>
</body>
</html>