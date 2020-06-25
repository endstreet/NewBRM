<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiGrantMerge.aspx.cs" Inherits="SASSADirectCapture.Views.MultiGrantMerge" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Multi-grant File Merge</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/my_ecm.js"></script>
    <%: Scripts.Render("~/bundles/BRMBundle") %>

    <script type="text/javascript">
        function getBRM(pensionNumber, grantName, grantType, applicationDate, srd, brmno, IsRMC, childID) {
            //set the applicant globals for applicantSearch
            var applicant = setApplicantValues(pensionNumber, grantName, grantType, applicationDate, srd, brmno, IsRMC, childID);
            var enterBRM = setEnterBRM(applicant);
            var fileCoversheet = setFileCoversheet(applicant);
            var multiGrantMerge = setMultiGrantMerge(applicant);

            //Check if brmno has a value
            if (applicant.brmBarcode) {
                //todo: check for duplicate BRMNo.
                BRM_Business_Logic.BRMActionModal(applicant, fileCoversheet, enterBRM);
            }
            else {
                BRM_Business_Logic.newEnterBRMWindow(enterBRM);
            }
        }

        function setApplicantValues(pensionNo, grantName, grantType, applicationDate, srd, brmBarcode, IsRMC, childID) {
            var selectedApplicant = new Applicant();
            selectedApplicant.pensionNumber = pensionNo;
            selectedApplicant.grantName = grantName;
            selectedApplicant.grantType = grantType;
            selectedApplicant.applicationDate = applicationDate;
            selectedApplicant.srd = srd;
            selectedApplicant.brmBarcode = brmBarcode;
            selectedApplicant.isRMC = IsRMC;
            selectedApplicant.childID = childID;

            return selectedApplicant;
        }

        function setMultiGrantMerge(applicant) {
            var multiGrantMerge = new MultiGrantMerge();
            multiGrantMerge.applicant = applicant;

            return multiGrantMerge;
        }

        function setFileCoversheet(applicant) {
            var fileCoversheet = new FileCoversheet();
            fileCoversheet.applicant = applicant;
            fileCoversheet.batching = true;
            fileCoversheet.boxNumber = '';
            fileCoversheet.boxAudit = '';
            fileCoversheet.transaction = '';

            return fileCoversheet;
        }

        function setEnterBRM(applicant) {
            var enterBRM = new EnterBRM();
            enterBRM.applicant = applicant;
            enterBRM.batching = true;

            return enterBRM;
        }
    </script>
    <script type="text/javascript">
        window.myApp =
            {
                // Function to use for the "focus" event.
            onFocus: function () {
                    var setOpener = false;
                    if (window.name === null || window.name === '') {
                        setOpener = true;
                    }

                    document.getElementById("btnRefreshGrid").click();

                    if (setOpener) {
                        window.name = 'RefocusWindow';
                    }
                }
            };

        /* Detect if the browser supports "addEventListener" Complies with DOM Event specification. */
        if (window.addEventListener) {
            // Handle window's "load" event.
            window.addEventListener('load', function () {
                // Wire up the "focus" and "blur" event handlers.
                window.addEventListener('focus', window.myApp.onFocus);
            }
            );
        }
        /* Detect if the browser supports "attachEvent" Only Internet Explorer browsers support that. */
        else if (window.attachEvent) {
            // Handle window's "load" event.
            window.attachEvent('onload', function () {
                // Wire up the "focus" and "blur" event handlers.
                window.attachEvent('onfocus', window.myApp.onFocus);
            }
            );
        }
        /* If neither event handler function exists, then overwrite the built-in event handers. With this technique any previous event handlers are lost. */
        else {
            // Handle window's "load" event.
            window.onload = function () {
                // Wire up the "focus" and "blur" event handlers.
                window.onfocus = window.myApp.onFocus;
            };
        }
    </script>
</head>
<body>
    <form runat="server">
        <div class="container col-md-12">
            <div class="form-group">
                <h2 class="text-center form-group">The File selected is part of a Multi-grant.</h2>
                <p runat="server" id="pHeading">Please select each and every file below to capture the BRM File Number in order to generate their <b>File CoverSheets</b> before merging the files.</p>
            </div>
        </div>
        <div class="row" id="divError" runat="server" visible="false">
            <div class="form-group col-xs-12 alert alert-danger">
                <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
            </div>
        </div>
        <div class="form-horizontal col-sm-12">
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Applicant Name:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" class="col-sm-3 control-label text-left" ID="lblName"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Applicant Surname:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" class="col-sm-3 control-label text-left" ID="lblSurname"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Region:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" class="col-sm-3 control-label text-left" ID="lblRegion"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div class="form-horizontal col-sm-12">
            <asp:GridView ID="gvResults" runat="server" CssClass="gridView" AutoGenerateColumns="False" AllowPaging="True" PageSize="25" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText="Beneficiary ID No" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Underline="true">
                        <ItemTemplate>
                            <a href="#" onclick="getBRM('<%# Eval("PENSION_NO") %>','<%# Eval("GrantType") %>','<%# Eval("GRANT_TYPE") %>','<%# Eval("AppDate") %>', '', '<%# Eval("BRM_BARCODE") %>', '<%# Eval("IsRMC") %>', '<%# Eval("CHILD_ID_NO") %>'); return false;" target="_blank" title="Create or Reprint the File Cover"><%# Eval("PENSION_NO") %></a>
                            <asp:HiddenField ID="hfBRMNo" runat="server" Value='<%# Eval("BRM_BARCODE") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CHILD_ID_NO" HeaderText="Child ID No" />
                    <asp:BoundField DataField="GrantType" HeaderText="Grant Type" />
                    <asp:BoundField DataField="SOC_STATUS" HeaderText="Status" />
                    <asp:BoundField DataField="AppDate" HeaderText="Application Date" />
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
        </div>
        <br />
        <div class="form-horizontal col-sm-12 pull-right">
            <%-- style="width: 100% !important"--%>
            <div style="display: none">
                <asp:Button ID="btnRefreshGrid" runat="server" Text="Refresh Grid" Height="28px" CssClass="btn btn-primary active" OnClick="btnRefreshGrid_Click" /><%-- pull-right--%>
            </div>
            <asp:Button ID="btnMerge" Visible="true" runat="server" Text="Merge" Height="28px" CssClass="btn btn-primary active" OnClick="btnMerge_Click" /><%-- pull-right--%>
            <asp:Button ID="btnCancel" Visible="true" runat="server" Text="Cancel" Height="28px" CssClass="btn btn-primary active" OnClick="btnCancel_Click" /><%-- pull-right--%>
        </div>
    </form>
</body>
</html>