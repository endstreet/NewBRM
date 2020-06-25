<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FileRequestDetail.aspx.cs" Inherits="SASSADirectCapture.Views.FileRequestDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Request</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
</head>
<body>
    <script type="text/javascript">
        function enableDisableSave() {
            <%--// Changed to force these three check boxes to be entered before complete becomes visible

            var isset = "N";

            if ((document.getElementById('<%= chkSentToTDW.ClientID %>').checked)
                && (document.getElementById('<%= chkReceivedTDW.ClientID %>').checked)
                && (document.getElementById('<%= chkScanned.ClientID %>').checked)
                && (document.getElementById('<%= chkSent.ClientID %>').checked))
            {
                document.getElementById('btnSave').className = document.getElementById('btnSave').className.replace('active', '');
                document.getElementById('btnSave').disabled = true;
                isset = "Y";
                alert('complete TDW');
            }

            if (document.getElementById('<%= chkRetrieved.ClientID %>').checked)
                && (document.getElementById('<%= chkScanned.ClientID %>').checked)
                && (document.getElementById('<%= chkSent.ClientID %>').checked))
            {
                document.getElementById('btnSave').className = document.getElementById('btnSave').className.replace('active', '');
                document.getElementById('btnSave').disabled = true;
                alert('complete RMC');
                isset = "Y";
            }

            if (isset == "N")
            {
                document.getElementById('btnSave').className += document.getElementById('btnSave').className ? ' active' : 'active';
                document.getElementById('btnSave').disabled = false;
                alert('incomplete');
            }--%>

            if (document.getElementById('<%= chkReceivedTDW.ClientID %>').checked) {
                document.getElementById('<%= chkRetrieved.ClientID %>').checked = true;
            }

            if (document.getElementById('hfStatus').value == '') {
                document.getElementById('btnSave').value = document.getElementById('hfBtnVal').value = 'Accept';
            }
            else {
                //Enable Complete button if chkRetrieved, chkScanned and chkSent are checked, as well as chkSentToTDW and chkReceivedTDW both check or both unchecked.
                if (((document.getElementById('<%= chkRetrieved.ClientID %>').checked)
                    && (document.getElementById('<%= chkSent.ClientID %>').checked))
                    &&
                    (((document.getElementById('<%= chkSentToTDW.ClientID %>').checked)
                        && (document.getElementById('<%= chkReceivedTDW.ClientID %>').checked))
                    ||
                    (!(document.getElementById('<%= chkSentToTDW.ClientID %>').checked)
                        && !(document.getElementById('<%= chkReceivedTDW.ClientID %>').checked)))) {
                    document.getElementById('btnSave').value = document.getElementById('hfBtnVal').value = 'Complete';
                }
                else {
                    document.getElementById('btnSave').value = document.getElementById('hfBtnVal').value = 'Update';

                    if ((document.getElementById('<%= chkReceivedTDW.ClientID %>').checked)
                        && !(document.getElementById('<%= chkSentToTDW.ClientID %>').checked)) {
                        alert("Please indicate that the request was sent to TDW.");
                    }
                }
            }
        }
    </script>
    <form runat="server">
        <div class="container col-xs-12">
            <div class="form-group">
                <h2 id="headerText" runat="server" class="text-center form-group">Process Requested File</h2>
                <p id="headerInfo" style="padding-left: 15px" runat="server">The following file has been requested by a local office.</p>
            </div>
        </div>
        <div style="display: none">
            <asp:TextBox ID="txtHiddenUnqFileNo" runat="server" Enabled="false"></asp:TextBox>
            <asp:HiddenField ID="hfStatus" runat="server" />
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
            <asp:TextBox ID="txtBRMno" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Related File No(s):"></asp:Label>
            <asp:TextBox ID="txtRelatedMISNo" runat="server" Enabled="false" CssClass="form-control maxWidth" TextMode="MultiLine" Rows="2"></asp:TextBox>
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
        <%--<div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Grant Type:"></asp:Label>
            <asp:TextBox ID="txtGrant" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Application Date:"></asp:Label>
            <asp:TextBox ID="txtAppDate" runat="server" Enabled="false" CssClass="form-control maxWidth"></asp:TextBox>
        </div>--%>
        <div class="col-xs-12" style="margin-bottom: 5px;">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Department:"></asp:Label>
            <asp:DropDownList ID="ddlReqCategory" Enabled="false" DataValueField="key" DataTextField="value" AutoPostBack="true" CssClass="form-control maxWidth" runat="server" OnSelectedIndexChanged="ddlReqCategory_SelectedIndexChanged" />
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Stakeholder:"></asp:Label>
            <asp:DropDownList ID="ddlStakeholder" Enabled="false" DataValueField="key" DataTextField="value" AutoPostBack="true" CssClass="form-control maxWidth" runat="server" />
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="File Request Reason:"></asp:Label>
            <asp:DropDownList ID="ddlReqCategoryType" Enabled="false" DataValueField="key" DataTextField="value" AutoPostBack="true" CssClass="form-control maxWidth" runat="server" />
        </div>
        <div class="col-xs-12">
            <asp:Label runat="server" class="col-xs-4 control-label text-left vertical-align" Text="Detail:"></asp:Label>
            <asp:TextBox ID="txtDetail" runat="server" Enabled="false" CssClass="form-control maxWidth" TextMode="MultiLine" Rows="3"></asp:TextBox>
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
                <input type="radio" disabled="disabled" name="radioFile" runat="server" id="optRadioScanned" style="width: 30px" />Scanned</label>
            <label class="radio-inline" style="font-size: 14px">
                <input type="radio" disabled="disabled" name="radioFile" runat="server" id="optRadioPhysical" style="width: 30px" />Physical</label>
        </div>
        <div style="display: none">
            <asp:TextBox ID="txtRegionID" runat="server" Enabled="false"></asp:TextBox>
        </div>
        <br />
        <%--<div runat="server" id="divScanCheck" class="col-xs-12">
            <asp:Label runat="server" ID="lblComplete" class="col-xs-4 control-label text-left vertical-align" Text="Scanned to Livelink?"></asp:Label>
            <asp:CheckBox ID="chkComplete" runat="server" onclick="enableDisableSave();"></asp:CheckBox>
        </div>--%>
        <%--=====*** must always show ***===--%>
        <%--===== TDW is optional =====--%>
        <table width="100%">
            <tr>
                <td>
                    <div runat="server" id="divSentTDW" class="col-xs-12">
                        <asp:CheckBox ID="chkSentToTDW" runat="server" onclick="enableDisableSave();"></asp:CheckBox>
                        <asp:Label runat="server" ID="lblSentTDW" Text="Sent to TDW?"></asp:Label>
                    </div>
                </td>
                <td>
                    <div runat="server" id="divReceivedTDW" class="col-xs-12">

                        <asp:CheckBox ID="chkReceivedTDW" runat="server" onclick="enableDisableSave();"></asp:CheckBox>
                        <asp:Label runat="server" ID="lblReceivedTDW" Text="Received from TDW?"></asp:Label>
                    </div>
                </td>
                <td></td>
            </tr>
            <%--===== end TDW is optional =====--%>
            <%--===== complusory =====--%>
            <tr>
                <td>
                    <div runat="server" id="divRetrieved" class="col-xs-12">

                        <asp:CheckBox ID="chkRetrieved" runat="server" onclick="enableDisableSave();"></asp:CheckBox>
                        <asp:Label runat="server" ID="lblRetrieved" Text="File Retrieved?"></asp:Label>
                    </div>
                </td>
                <td>
                    <div runat="server" id="divlblScanned" class="col-xs-12">

                        <asp:CheckBox ID="chkScanned" runat="server" onclick="enableDisableSave();"></asp:CheckBox>
                        <asp:Label runat="server" ID="lblScanned" Text="Scanned to Livelink?"></asp:Label>
                    </div>
                </td>
                <td>
                    <div runat="server" id="divlblSent" class="col-xs-12">

                        <asp:CheckBox ID="chkSent" runat="server" onclick="enableDisableSave();"></asp:CheckBox>
                        <asp:Label runat="server" ID="lblSent" Text="Sent to Requestor?"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
        <%--===== end complusory =====--%>
        <%--=====*** END must always show ***===--%>

        <div class="form-horizontal col-sm-12 pull-right" style="width: 300px !important">
            <asp:Button ID="btnClose" runat="server" Text="Close" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnClose_Click" />
            <asp:Button ID="btnSave" runat="server" Text="Accept" Height="28px" CssClass="btn btn-primary active pull-right" OnClick="btnSave_Click" />
            <asp:HiddenField ID="hfBtnVal" runat="server" />
        </div>
    </form>
</body>
</html>
