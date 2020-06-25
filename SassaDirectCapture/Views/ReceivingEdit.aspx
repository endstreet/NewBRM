<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ReceivingEdit.aspx.cs" Inherits="SASSADirectCapture.Views.ReceivingEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Edit</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
</head>
<body>
    <form runat="server" style="width: 400px; margin: auto">
        <table style="padding: 10px; width: 100%;">
            <tr>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td>
                    <div class="container col-md-12">
                        <div class="form-group">
                            <h2 class="text-left form-group">Add File Comment</h2>
                            Enter a comment and click on Save
                        </div>
                    </div>
                    <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Comment:"></asp:Label>
                    <asp:TextBox ID="txtComment" runat="server" CssClass="form-control" Style="height: 100px; padding-left: 20px;" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary active pull-left" OnClick="btnClose_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary active pull-left" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>