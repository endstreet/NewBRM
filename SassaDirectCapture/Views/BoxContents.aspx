<%@ Page Title=""
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="BoxContents.aspx.cs"
    Inherits="SASSADirectCapture.Views.BoxContents" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title><%: Page.Title %> - SASSA Beneficiary Records Management - Box Contents</title>
    <link href="~/Content/bootstrap.css" rel='stylesheet' type='text/css' />
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <center>
                <h3 id="head1">
                    <asp:Label ID="lblHead" runat="server"></asp:Label>
                </h3>
                <input  id="btnPrint" type="button" style="width:200px;" class="btn-danger" value="Print" onclick="printList();" />
                <asp:Button ID="btnExcelExport" runat="server" Text="Excel Export" Width="200px" CssClass="btn-danger" OnClick="btnExcelExport_Click" />
            </center>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server"
            EnableViewState="false">
            <ContentTemplate>
                <asp:GridView ID="BoxlistGridView" runat="server"
                    CssClass="gridView"
                    ShowHeaderWhenEmpty="True"
                    AutoGenerateColumns="False"
                    CellPadding="2"
                    AllowPaging="True"
                    PageSize="25"
                    ForeColor="#333333"
                    GridLines="Both"
                    OnPageIndexChanging="BoxlistGridView_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="APPLICANT_NO" HeaderText="ID_NUMBER" />
                        <%-- 0 --%>
                        <asp:BoundField DataField="UNQ_FILE_NO" HeaderText="CLM No" />
                        <asp:BoundField DataField="FILE_NUMBER" HeaderText="MIS No" />
                        <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM No" />
                        <asp:BoundField DataField="FIRST_NAME" HeaderText="Name" />
                        <asp:BoundField DataField="LAST_NAME" HeaderText="Surname" />
                        <asp:BoundField DataField="GRANT_TYPE_NAME" HeaderText="Grant Type" />
                        <asp:BoundField DataField="ARCHIVE_YEAR" HeaderText="Archive Year" />
                        <asp:BoundField DataField="EXCLUSIONS" HeaderText="Exclusions" />
                        <asp:BoundField DataField="MIS_BOXNO" HeaderText="Box No" Visible="false" />
                        <asp:BoundField DataField="APPLICATION_STATUS" HeaderText="Application Status" />
                        <asp:BoundField DataField="MIS_BOX_STATUS" HeaderText="Audit Status" />
                        <asp:BoundField DataField="MIS_REBOX_STATUS" HeaderText="Rebox Status" />
                        <asp:BoundField DataField="FILE_STATUS" HeaderText="File Status" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
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
        </asp:UpdatePanel>
        <asp:Label class="col-sm-12 control-label text-left" runat="server" ID="lblError" Text=""></asp:Label>
    </form>
    <%--<table>
                <tr>
                    <td>
                        <br />
                        <br />
                        <br />
                        <asp:Label class="col-sm-12 control-label text-left" runat="server" ID="Label2" Text="____________________________________"></asp:Label><br />
                        <asp:Label class="col-sm-12 control-label text-left" runat="server" Text="Name and Surname"></asp:Label>
                    </td>
                     <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                     <td>
                        <br />
                        <br />
                        <br />
                        <asp:Label class="col-sm-12 control-label text-left" runat="server" ID="Label1" Text="____________________________________"></asp:Label><br />
                        <asp:Label class="col-sm-12 control-label text-left" runat="server" Text="Signature"></asp:Label>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <br />
                        <br />
                        <br />
                         <div id="divDateTime" style="margin-right: 15px; width: 25%"><br />
                         <asp:Label class="col-sm-12 control-label text-left" runat="server" Text="Date"></asp:Label>
                </div>
                    </td>
                </tr>
            </table>--%>

    <script type="text/javascript">
        function printList() {
            document.getElementById("btnPrint").style.display = "none";
            window.print();
            window.close();
        }
        $(document).ready(function () {
            window.focus();
        })
    </script>
</body>
</html>