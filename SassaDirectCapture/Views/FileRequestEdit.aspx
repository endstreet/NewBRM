<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FileRequestEdit.aspx.cs" Inherits="SASSADirectCapture.Views.FileRequestEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Request</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
</head>
<body>
    <form runat="server">
        <div class="container col-xs-12">
            <div class="form-group">
                <h2 id="headerText" runat="server" class="text-center form-group">Log File Request</h2>
                <p id="headerInfo" style="padding-left: 15px" runat="server">The following file will be requested from the RMC</p>
            </div>
        </div>
        <div style="display: none">
            <asp:TextBox ID="txtHiddenUnqFileNo" runat="server" Enabled="false"></asp:TextBox>
            <asp:HiddenField ID="hfAppStatus" runat="server" />
        </div>
        <!--ERROR DIV -->
        <div class="row" id="divError" runat="server" style="margin-left: 10px; margin-right: 10px; display: none">
            <div class="form-group alert alert-danger" style="margin-left: 15px; margin-right: 15px">
                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
            </div>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" for="txtServBy" class="col-xs-4 control-label text-left vertical-align" Text="Serviced By:"></asp:Label>
            <asp:TextBox ID="txtServBy" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" for="txtIDNo" class="col-xs-4 control-label text-left vertical-align" Text="ID No:"></asp:Label>
            <asp:TextBox ID="txtIDNo" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="MIS File No:"></asp:Label>
            <asp:TextBox ID="txtFileNo" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="BRM No:"></asp:Label>
            <asp:TextBox ID="txtBRM" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Name:"></asp:Label>
            <asp:TextBox ID="txtName" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Surname:"></asp:Label>
            <asp:TextBox ID="txtSurname" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Region:"></asp:Label>
            <asp:TextBox ID="txtRegion" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <%--        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Grant Type:"></asp:Label>
            <asp:TextBox ID="txtGrant" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
            <asp:TextBox ID="txtGrantID" runat="server" Enabled="false" style="display:none"></asp:TextBox>
        </div>--%>
        <%--        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Application Date:"></asp:Label>
            <asp:TextBox ID="txtAppDate" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>--%>
        <div class="col-xs-12" style="margin-bottom: 5px;">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Department:  <b style='color:#ff0000;'>*</b>"></asp:Label><%--Category--%>
            <asp:DropDownList ID="ddlReqCategory" DataValueField="key" DataTextField="value" AutoPostBack="true" CssClass="form-control maxWidth" runat="server" OnSelectedIndexChanged="ddlReqCategory_SelectedIndexChanged" />
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Stakeholder:  <b style='color:#ff0000;'>*</b>"></asp:Label>
            <asp:DropDownList ID="ddlStakeholder" DataValueField="key" DataTextField="value" AutoPostBack="true" CssClass="form-control maxWidth" runat="server" OnSelectedIndexChanged="ddlStakeholder_SelectedIndexChanged" />
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="File Request Reason:  <b style='color:#ff0000;'>*</b>"></asp:Label><%--Category Type--%>
            <asp:DropDownList ID="ddlReqCategoryType" DataValueField="key" DataTextField="value" AutoPostBack="true" CssClass="form-control maxWidth" runat="server" OnSelectedIndexChanged="ddlReqCategoryType_SelectedIndexChanged" />
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Detail:"></asp:Label>
            <asp:TextBox ID="txtDetail" runat="server" CssClass="form-control maxWidth" TextMode="MultiLine" Rows="3"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="MIS Bin:"></asp:Label>
            <asp:TextBox ID="txtBin" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Box:"></asp:Label>
            <asp:TextBox ID="txtBox" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Position :"></asp:Label>
            <asp:TextBox ID="txtPosition" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="TDW Box:"></asp:Label>
            <asp:TextBox ID="txtTDWBox" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="File Type:"></asp:Label>
            <label class="radio-inline" style="font-size: 14px">
                <input type="radio" name="radioFile" runat="server" id="optRadioScanned" style="width: 30px" checked="true" />Scanned</label>
            <label class="radio-inline" style="font-size: 14px">
                <input type="radio" name="radioFile" runat="server" id="optRadioPhysical" style="width: 30px" />Physical</label>
        </div>
        <div style="display: none">
            <asp:TextBox ID="txtRegionID" runat="server" Enabled="false"></asp:TextBox>
        </div>
        <br />
        <div class="form-horizontal col-sm-12 pull-right" style="width: 300px !important">
            <asp:Button ID="btnClose" runat="server" Text="Close" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnClose_Click" />
            <asp:Button ID="btnSave" runat="server" Visible="false" Text="Request File" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Visible="false" Text="Cancel Request" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnCancel_Click" />
            <asp:Button ID="btnComplete" runat="server" Visible="false" Text="Complete Request" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnComplete_Click" />
        </div>
    </form>
</body>
</html>