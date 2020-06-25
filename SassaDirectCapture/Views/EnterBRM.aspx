<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="EnterBRM.aspx.cs" Inherits="SASSADirectCapture.Views.EnterBRM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enter BRM Barcode</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/my_ecm.js"></script>
    <%: Scripts.Render("~/bundles/BRMBundle") %>
    <script type="text/javascript">
        function openFileCover() {
            var pensionNumber = document.getElementById("hf_PENSION_NUMBER").value
            var batching = document.getElementById("hf_BATCHING").value;
            var transaction = document.getElementById("hf_TRANSACTION").value;
            var boxAudit = document.getElementById("hf_BOX_AUDIT").value;
            var boxNumber = document.getElementById("hf_BOX_NUMBER").value;
            var grantName = document.getElementById("hf_GRANT_NAME").value;
            var applicationDate = document.getElementById("hf_APPLICATION_DATE").value;
            var grantType = document.getElementById("hf_GRANT_TYPE").value;
            var brmBarcode = document.getElementById("txtBRMBarcode").value;
            var SRDNo = document.getElementById("hf_SRD_NUMBER").value;
            var tempBatch = document.getElementById("hf_TEMP_BATCH").value;
            var childID = document.getElementById("hf_CHILD_ID").value;
            var isReview = document.getElementById("hf_IS_REVIEW").value;
            var LCType = document.getElementById("hf_LC_TYPE").value;

            var myURL = 'FileCover.aspx?PensionNo=' + pensionNumber + '&boxaudit=' + boxAudit + '&boxNo=' + boxNumber + '&batching=' + batching + '&trans=' + transaction + '&brmBC=' + brmBarcode + '&gn=' + grantName + '&gt=' + grantType + '&appdate=' + applicationDate + '&SRDNo=' + SRDNo + '&tempBatch=' + tempBatch + '&ChildID=' + childID + '&IsReview=' + isReview + '&LCType=' + LCType;

            var newWindow = window.open(myURL, '_blank');

            window.opener.name = 'RefocusWindow';

            // Puts focus on the newWindow
            newWindow.focus();
        }

        function hasBRMNumber() {
            var hasBRMNumber = false;
            if ($("#hf_BRM_BARCODE").val() !== '') {
                hasBRMNumber = true;
            }

            return hasBRMNumber;
        }

        $(document).ready(function () {
            if (hasBRMNumber()) {
                $("#application").hide();
                $("#applicationPlacehoder").hide();
            }

            var pressed = false;
            var chars = [];
            $(window).keypress(function (e) {
                if (e.which === 13) {
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
                            $("#txtBRMBarcode").val(barcode);
                            $("#btnUpdateBRM").trigger("click");
                        }
                        chars = [];
                        pressed = false;
                    }, 300);
                }
                pressed = true;
            });
        });

        function ValidateBRM() {
            if (!(/^[a-zA-Z0-9]{8}$/.test(<%=txtBRMBarcode.ClientID%>.value))) {
                alert('A valid BRM Number is exactly 8 characters long and only contain numbers or letters.');
                return false;
            }
            else {
                return true;
            }
        }

        function CheckBRMUsedOnParentPage(BRMNo, txtBRMBarcodeClientID, MGMerge) {
            var table, tbody, i, rowLen, row, j, colLen, cell;

            if (MGMerge == "Y") {
                table = window.opener.document.getElementById("gvResults");
            }
            else {
                table = window.opener.document.getElementById("MainContent_boxGridView");
            }
            tbody = table.tBodies[0];
            BRMNo = BRMNo.toUpperCase();

            for (i = 0, rowLen = tbody.rows.length; i < rowLen; i++) {
                row = tbody.rows[i];
                for (j = 0, colLen = row.cells.length; j < colLen; j++) {
                    cell = row.cells[j];
                    if (j == 2 && BRMNo == cell.innerHTML)//BRM No
                    {
                        alert('Please scan or enter a different BRM Barcode,\n' + BRMNo + ' is already in use.');
                        WebForm_AutoFocus(txtBRMBarcodeClientID);
                        return false;
                    }
                }
            }
        }
    </script>
</head>
<body>
    <form runat="server">
        <div class="container col-md-12">
            <div class="form-group">
                <h2 class="text-center form-group">Enter BRM Barcode</h2>
                <p runat="server" id="pHeading">If the file does not already have a BRM barcode on it, please use a new TDW sticker to put onto the file, then scan it - because it will be printed on the File Coversheet. </p>
            </div>
        </div>
        <div class="form-horizontal col-sm-12">
            <table style="width: 100%">
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" class="control-label text-left" Text="Please select the Application Type:"></asp:Label>
                    </td>
                </tr>
                <tr style="display: none">
                    <td>
                        <asp:RadioButton runat="server" ID="rbApplicationHidden" Width="100%" Text="Application" Value="A" GroupName="AppTypeHidden" Checked="true" OnCheckedChanged="rbApplicationType_CheckedChanged" AutoPostBack="true" />
                    </td>
                    <td>
                        <asp:RadioButton runat="server" ID="rbLCHidden" Width="100%" Text="Loose Correspondence" Value="LC" GroupName="AppTypeHidden" OnCheckedChanged="rbApplicationType_CheckedChanged" AutoPostBack="true" />
                    </td>
                    <td>
                        <asp:RadioButton runat="server" ID="rbReviewHidden" Width="100%" Text="Review" Value="R" GroupName="AppTypeHidden" OnCheckedChanged="rbApplicationType_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td id="application">
                        <input id="rbApplication" type="radio" runat="server" value="A" style="width: 30px" name="AppType" checked onclick="document.getElementById('rbApplicationHidden').click();" />
                        <label for="rbApplication" style="display: inline-block">Application</label>
                    </td>
                    <td>
                        <input id="rbLC" type="radio" runat="server" value="LC" style="width: 30px" name="AppType" onclick="document.getElementById('rbLCHidden').click();" />
                        <label for="rbLC" style="display: inline-block">Loose Correspondence</label>
                    </td>
                    <td>
                        <input id="rbReview" type="radio" runat="server" value="R" style="width: 30px" name="AppType" onclick="document.getElementById('rbReviewHidden').click();" />
                        <label for="rbReview" style="display: inline-block">Review</label>
                    </td>
                </tr>
                <tr>
                    <td id="applicationPlacehoder"></td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblLCType" Text="Loose Correspondence Type: " Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlLCType" DataTextField="Key" DataValueField="Value" Visible="false"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="form-horizontal col-sm-12">
            <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="BRM Barcode:"></asp:Label>
            <asp:TextBox ID="txtBRMBarcode" runat="server" CssClass="form-control col-sm-3" placeholder="Enter here..." Text=""></asp:TextBox>
        </div>
        <br />

        <div class="form-horizontal col-sm-12 pull-right" style="width: 100% !important">
            <asp:Button ID="btnCancel" Visible="true" runat="server" Text="Cancel" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnCancel_Click" />
            <asp:Button ID="btnUpdateBRM" Visible="true" runat="server" Text="Update" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnUpdateBRM_Click" OnClientClick="return ValidateBRM();" />
        </div>
        <asp:HiddenField ID="hf_BRM_BARCODE" runat="server" Value="" />
        <asp:HiddenField ID="hf_PENSION_NUMBER" runat="server" Value="" />
        <asp:HiddenField ID="hf_BATCHING" runat="server" Value="" />
        <asp:HiddenField ID="hf_TRANSACTION" runat="server" Value="" />
        <asp:HiddenField ID="hf_BOX_AUDIT" runat="server" Value="" />
        <asp:HiddenField ID="hf_BOX_NUMBER" runat="server" Value="" />
        <asp:HiddenField ID="hf_GRANT_NAME" runat="server" Value="" />
        <asp:HiddenField ID="hf_APPLICATION_DATE" runat="server" Value="" />
        <asp:HiddenField ID="hf_GRANT_TYPE" runat="server" Value="" />
        <asp:HiddenField ID="hf_SRD_NUMBER" runat="server" Value="" />
        <asp:HiddenField ID="hf_TEMP_BATCH" runat="server" Value="" />
        <asp:HiddenField ID="hf_CHILD_ID" runat="server" Value="" />
        <asp:HiddenField ID="hf_IS_REVIEW" runat="server" Value="" />
        <asp:HiddenField ID="hf_LC_TYPE" runat="server" Value="" />
        <asp:TextBox ID="TextBox1" runat="server" Visible="false" Enabled="false" placeholder="Enter here..." Text="" onfocus="this.select();"></asp:TextBox>
    </form>
</body>
</html>