<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LocalOffice.aspx.cs" Inherits="SASSADirectCapture.Views.LocalOffice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
</head>
<body>
    <div class="container">
        <form id="form1" runat="server">
            <div class="container col-md-12">
                <div class="form-group">
                    <h2 class="text-center form-group">My Office</h2>
                    <%-- Local--%>
                    Select your region and office
                </div>
                <%-- local--%>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                <ContentTemplate>
                    <div class="container col-md-12">
                        <div id="divError" runat="server" visible="false" class="alert alert-danger">
                            <asp:Label runat="server" ID="lblError" />
                        </div>
                        <div class="row">
                            <asp:Label runat="server" CssClass="control-label vertical-align">Region</asp:Label>
                            <asp:DropDownList ID="ddlRegion" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" DataValueField="key" DataTextField="value" AutoPostBack="true" CssClass="form-control" runat="server" />
                        </div>
                        <br />
                        <div class="row">
                            <asp:Label runat="server" CssClass="control-label vertical-align">Office</asp:Label><%--Local --%>
                            <asp:DropDownList ID="ddlLocalOffice" DataValueField="key" DataTextField="value" AutoPostBack="true" CssClass="form-control" runat="server" />
                        </div>
                        <div id="bottomRow" class="row">
                            <div>
                                <asp:Button ID="btnSave" runat="server" class="btn btn-primary active pull-right" Text="Save" type="button" OnClick="btnSave_Click" CausesValidation="true" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>