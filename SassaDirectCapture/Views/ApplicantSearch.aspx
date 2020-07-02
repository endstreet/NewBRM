<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicantSearch.aspx.cs" Inherits="SASSADirectCapture.Views.ApplicantSearch" EnableSessionState="True" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function pageLoad(sender, args) {
            WebForm_AutoFocus('<%= txtSearch.ClientID %>');
        }

        function getBRM(pensionNumber, grantName, grantType, applicationDate, srd, brmno, CLM, IsRMC, childID) {

            //set the applicant globals for applicantSearch
            var applicant = setApplicantValues(pensionNumber, grantName, grantType, applicationDate, srd, brmno, CLM, IsRMC, childID);
            var enterBRM = setEnterBRM(applicant);
            var fileCoversheet = setFileCoversheet(applicant);
            var multiGrantMerge = setMultiGrantMerge(applicant);

            if ((applicant.srdNumber === '' || applicant.srdNumber != null) && CheckIsMultiGrant(applicant.applicationDate)) {
                BRM_Business_Logic.newMultiGrantMergeWindow(multiGrantMerge);
            }
            else {
                //Check if brmno has a value
                if (applicant.brmBarcode) {
                    BRM_Business_Logic.BRMActionModal(applicant, fileCoversheet, enterBRM);
                }
                else {
                    BRM_Business_Logic.newEnterBRMWindow(enterBRM);
                }
            }
        }

        function setApplicantValues(pensionNo, grantName, grantType, applicationDate, srd, brmBarcode, CLM, IsRMC, childID) {
            var selectedApplicant = new Applicant();
            selectedApplicant.pensionNumber = pensionNo;
            selectedApplicant.grantName = grantName;
            selectedApplicant.grantType = grantType;
            selectedApplicant.applicationDate = applicationDate;
            selectedApplicant.srdNumber = srd;
            selectedApplicant.brmBarcode = brmBarcode;
            selectedApplicant.clm = CLM;
            selectedApplicant.isRMC = IsRMC;
            selectedApplicant.childID = childID;

            return selectedApplicant;
        }

        function setMultiGrantMerge(applicant) {
            var multiGrantMerge = new MultiGrantMerge();
            multiGrantMerge.applicant = applicant;

            return multiGrantMerge;
        };

        function setFileCoversheet(applicant) {
            var fileCoversheet = new FileCoversheet();
            fileCoversheet.applicant = applicant;
            fileCoversheet.batching = true;
            fileCoversheet.boxNumber = '';
            fileCoversheet.boxAudit = '';
            fileCoversheet.transaction = '';

            return fileCoversheet;
        };

        function setEnterBRM(applicant) {
            var enterBRM = new EnterBRM();
            enterBRM.applicant = applicant;
            enterBRM.batching = true;

            return enterBRM;
        };

        function CheckIsMultiGrant(appdate)
        {
            var table, tbody, i, rowLen, row, j, colLen, cell, count;

            if (document.getElementById('MainContent_rbID').checked === true) {
                table = document.getElementById("MainContent_gvResults");
            }
            else if (document.getElementById('MainContent_rbSRD').checked === true) {
                table = document.getElementById("MainContent_gvResultsSRD");
            }
            else {
                table = null;
            }
            
            tbody = table.tBodies[0];
            count = 0;

            for (i = 0, rowLen = tbody.rows.length; i < rowLen; i++)
            {
                row = tbody.rows[i];
                for (j = 0, colLen = row.cells.length; j < colLen; j++)
                {
                    cell = row.cells[j];
                    if (j == 7 && appdate == cell.innerHTML)//AppDate
                    {
                        count++;
                        if (count > 1)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        function ValidateID() {
            if (<%=rbID.ClientID%>.checked) {
                if (!(/^[0-9]{13}$/.test(<%=txtSearch.ClientID%>.value))) {
                    alert('A valid ID is exactly 13 characters long and only contain numbers.');
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                if (!(/^[0-9]{12,13}$/.test(<%=txtSearchSRD.ClientID%>.value))) {
                    alert('A valid SRD Reference Number is 12 to 13 characters long and only contain numbers.');
                    return false;
                }
                else {
                    return true;
                }
            }
        }
    </script>
    <script>
        $(function () {
            $("#dialog").dialog();
        });
    </script>
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row" id="divError" runat="server" visible="false">
                <div class="form-group col-xs-12 alert alert-danger">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                </div>
            </div>
            <div class="form-horizontal col-sm-6" style="width: 100%">
                <table>
                    <tr style="display: none">
                        <td>
                            <asp:RadioButton ID="rbIDHidden" runat="server" Text="ID Number" AutoPostBack="true" GroupName="Search" Checked="true" OnCheckedChanged="rbID_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbSRDHidden" runat="server" Text="SRD Reference Number" AutoPostBack="true" GroupName="Search" OnCheckedChanged="rbSRD_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="rbID" type="radio" runat="server" value="rbID" style="width: 30px" checked onclick="document.getElementById('MainContent_rbIDHidden').click();" />
                            <label for="rbID">ID Number</label>
                        </td>
                        <td>
                            <input id="rbSRD" type="radio" runat="server" value="rbSRD" style="width: 30px" onclick="document.getElementById('MainContent_rbSRDHidden').click();" />
                            <label for="rbSRD">SRD Reference Number</label>
                        </td>
                    </tr>
                </table>
                <div style="display: table-row;">
                    <div id="divSearchID" runat="server" style="vertical-align: middle;display: table-cell; padding:0 5px 0 5px;">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Width="200px" placeholder="Enter here..." onfocus="this.select();"></asp:TextBox>
                    </div>
                    <div id="divSearchSRD" runat="server" style="vertical-align:bottom;display:table-cell;padding:0 5px 0 5px; display: none;">
                        <asp:TextBox ID="txtSearchSRD" runat="server" CssClass="form-control" Width="200px" placeholder="Enter here..." onfocus="this.select();"></asp:TextBox>
                    </div>
                    <div  style="vertical-align: middle;display: table-cell;padding:0 5px 0 5px;">
                     <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="if (ValidateID()) {myECM.showPleaseWait('Searching...')} else {return false;};" CssClass="btn btn-primary active" />
                    </div>
                </div>
            </div>
            <br />
            <br />
            <asp:GridView ID="gvResults" runat="server" CssClass="gridView" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gvResults_PageIndexChanging" PageSize="25" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText="ID No" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Underline="true">
                        <ItemTemplate>
                            <a href="#" onclick="getBRM('<%# Eval("PENSION_NO") %>','<%# Eval("GrantType") %>','<%# Eval("GRANT_TYPE") %>','<%# Eval("AppDate") %>', '<%# Eval("SRD_NO") %>', '<%# Eval("BRM_BARCODE") %>', '<%# Eval("CLM") %>', '<%# Eval("IsRMC") %>', '<%# Eval("CHILD_ID_NO") %>'); return false;" target="_blank" title="Create or Reprint the File Cover"><%# Eval("PENSION_NO") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CHILD_ID_NO" HeaderText="Child ID No" />
                    <asp:BoundField DataField="GrantType" HeaderText="Grant Type" />
                    <asp:BoundField DataField="SOC_STATUS" HeaderText="Status" />
                    <asp:BoundField DataField="NAME" HeaderText="Name" />
                    <asp:BoundField DataField="SURNAME" HeaderText="Surname" />
                    <asp:BoundField DataField="Region" HeaderText="Region" />
                    <asp:BoundField DataField="AppDate" HeaderText="Application Date" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                    <%-- Visible="false"--%>
                    <asp:BoundField DataField="STATUS_DATE" HeaderText="Transaction Date" Visible="false" />
                    <asp:BoundField DataField="GRANT_TYPE" HeaderText="GRANT_TYPE" Visible="false" />
                    <asp:BoundField DataField="PRIM_STATUS" HeaderText="PRIM_STATUS" Visible="false" />
                    <asp:BoundField DataField="SEC_STATUS" HeaderText="SEC_STATUS" Visible="false" />
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
            <asp:GridView ID="gvResultsSRD" runat="server" CssClass="gridView" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gvResultsSRD_PageIndexChanging" PageSize="25" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="PENSION_NO" HeaderText="ID No" />
                    <asp:TemplateField HeaderText="SRD Reference Number" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Underline="true">
                        <ItemTemplate>
                            <a href="#" onclick="getBRM('<%# Eval("PENSION_NO") %>','<%# Eval("GrantType") %>','<%# Eval("GRANT_TYPE") %>','<%# Eval("AppDate") %>', '<%# Eval("SRD_NO") %>', '<%# Eval("BRM_BARCODE") %>', '<%# Eval("CLM") %>', '<%# Eval("IsRMC") %>', ''); return false;" target="_blank" title="Create or Reprint the File Cover"><%# Eval("SRD_NO") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="GrantType" HeaderText="Grant Type" />
                    <asp:BoundField DataField="SOC_STATUS" HeaderText="Status" />
                    <asp:BoundField DataField="NAME" HeaderText="Name" />
                    <asp:BoundField DataField="SURNAME" HeaderText="Surname" />
                    <asp:BoundField DataField="Region" HeaderText="Region" />
                    <asp:BoundField DataField="AppDate" HeaderText="Application Date" Visible="false" />
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>