<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="BatchEditBulk.aspx.cs" Inherits="SASSADirectCapture.Views.BatchEditBulk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Detail</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/my_ecm.js"></script>
</head>
<body>
    <form runat="server">
        <div class="container col-md-12">
            <div class="form-group">
                <h2 class="text-center form-group">Update Courier Detail</h2>
                <p runat="server" id="pHeading">Enter a TDW Batch Order No and Courier Name.</p>
            </div>
        </div>
        <div class="form-horizontal col-sm-12">
            <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Batch No:"></asp:Label>
            <asp:TextBox ID="txtBatchNo" runat="server" CssClass="form-control col-sm-3" Enabled="false"></asp:TextBox>
        </div>
        <div class="form-horizontal col-sm-12">
            <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="TDW Batch Order No:"></asp:Label>
            <asp:TextBox ID="txtWayBillNo" runat="server" CssClass="form-control col-sm-3" placeholder="Enter here..." Enabled="false"></asp:TextBox>
        </div>
        <div class="form-horizontal col-sm-12">
            <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Courier Name:"></asp:Label>
            <asp:TextBox ID="txtCourierName" runat="server" CssClass="form-control col-sm-3" placeholder="Enter here..." Enabled="false"></asp:TextBox>
        </div>

        <br />
        <p runat="server" id="pFooter" visible="false" style="padding-left: 15px; padding-right: 15px">
            Click on <b>Update All</b> if you want to update all the selected batches with the details entered above.<br />
            Click on <b>Update Missing</b> if you only want to update the missing data on the selected batch(es).
        </p>
        <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="right">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Update" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnSave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnClose" runat="server" Visible="true" Text="Close" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnClose_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>