<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FileEdit.aspx.cs" Inherits="SASSADirectCapture.Views.FileEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Edit</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script type="text/javascript">
        $(document).ready(function () {
            var pressed = false;
            var chars = [];
            $(window).keypress(function (e) {
                alert("key pressed");
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
                            //$("#txtBRMBarcode").val(barcode);
                            alert("btnSave click");
                            $("#btnSave").trigger("click");
                        }
                        chars = [];
                        pressed = false;
                    }, 300);
                }
                pressed = true;
            });
        });

        function ValidateBRM() {
            if (!(/^[a-zA-Z0-9]{8}$/.test(<%=txtBRM_BARCODE.ClientID%>.value))) {
                alert('A valid BRM Number is exactly 8 characters long and only contain numbers or letters.');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</head>
<body>
    <form runat="server">
        <div class="container col-md-12">
            <div class="form-group">
                <h2 class="text-center form-group">Update File Details</h2>
            </div>
        </div>
        <div class="form-horizontal col-sm-12">
            <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Unique File Number:"></asp:Label>
            <asp:TextBox ID="txtUNQ_FILE_NO" runat="server" CssClass="form-control col-sm-3" placeholder="Enter here..." Enabled="false"></asp:TextBox>
        </div>
        <div class="form-horizontal col-sm-12">
            <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="BRM File Number:"></asp:Label>
            <asp:TextBox ID="txtBRM_BARCODE" runat="server" CssClass="form-control col-sm-3" placeholder="Enter here..."></asp:TextBox><%-- OnTextChanged="btnSave_Click"--%>
            <asp:Label ID="curBRM" runat="server" class="col-sm-3 control-label text-left" Text=""></asp:Label>
        </div>
        <br />
        <p runat="server" id="pFooter" visible="false" style="padding-left: 15px; padding-right: 15px">
            Click on <b>Update</b> if you want to update the file details entered above.<br />
        </p>
        <div class="form-horizontal col-sm-12 pull-right" style="width: 100% !important">
            <asp:Button ID="btnClose" runat="server" Text="Cancel" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnClose_Click" />
            <asp:Button ID="btnSave" runat="server" Text="Update" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnSave_Click" OnClientClick="return ValidateBRM();" />
        </div>
    </form>
</body>
</html>