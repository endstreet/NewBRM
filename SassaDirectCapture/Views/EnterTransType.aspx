<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnterTransType.aspx.cs" Inherits="SASSADirectCapture.Views.EnterTransType" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Type</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/my_ecm.js"></script>
</head>
<body>
    <form runat="server">
        <div class="container col-md-12">
            <h2 class="text-center form-group">Select a Transaction Type</h2>
            <p runat="server" id="pHeading">Select a Service Category and the relevant Transaction Type. </p>
            <div class="form-horizontal col-sm-12">
                <div class="form-group">
                    <asp:Label runat="server" class="col-sm-10 control-label text-left" Text="Service Category"></asp:Label>
                    <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control" Style="width: 100%" OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="---Select Category---" Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Applications" Value="Applications"></asp:ListItem>
                        <asp:ListItem Text="Customer Care" Value="Customer Care"></asp:ListItem>
                        <asp:ListItem Text="Disability Management" Value="Disability Management"></asp:ListItem>
                        <asp:ListItem Text="Payments" Value="Payments"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Label for="ddlTransactionType" class="col-sm-10 control-label text-left" runat="server" Text="Transaction Type"></asp:Label>
                    <asp:DropDownList ID="ddlTransactionType" DataValueField="key" DataTextField="value" CssClass="form-control" Style="width: 100%" runat="server" Enabled="false" />
                </div>
            </div>
            <br />

            <div class="form-horizontal pull-right" style="width: 100% !important">
                <asp:Button ID="btnCancel" Visible="true" runat="server" Text="Cancel" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnCancel_Click" />
                <asp:Button ID="btnUpdateTransType" Visible="true" runat="server" Text="Update" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnUpdateTransType_Click" />
            </div>
        </div>
    </form>
</body>
</html>