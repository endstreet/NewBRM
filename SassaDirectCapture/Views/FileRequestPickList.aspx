<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FileRequestPickList.aspx.cs" Inherits="SASSADirectCapture.Views.FileRequestPickList" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title><%: Page.Title %> - SASSA Beneficiary Records Management PICKLIST</title>
    <link href="~/Content/bootstrap.css" rel='stylesheet' type='text/css' />
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <center><h3 id="head1"><asp:Label ID="lblHead" runat="server"></asp:Label></h3><input  id="btnPrint" type="button" style="width:200;" class="btn-danger" value="Print" onclick="printList();" /></center>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server"
            EnableViewState="false">
            <ContentTemplate>
                <asp:GridView ID="fileGridView" runat="server"
                    CssClass="gridView"
                    ShowHeaderWhenEmpty="True"
                    AutoGenerateColumns="False"
                    CellPadding="2"
                    AllowPaging="True"
                    PageSize="25"
                    ForeColor="#333333"
                    GridLines="Both">
                    <Columns>
                        <asp:BoundField DataField="REQUESTED_DATE" HeaderText="Date Requested"></asp:BoundField>
                        <asp:BoundField DataField="REQUESTED_OFFICE_NAME" HeaderText="Office Requested"></asp:BoundField>
                        <asp:BoundField DataField="ID_NO" HeaderText="ID No"></asp:BoundField>
                        <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM No"></asp:BoundField>
                        <asp:BoundField DataField="MIS_FILE_NO" HeaderText="File No"></asp:BoundField>
                        <asp:BoundField DataField="BIN_ID" HeaderText="Bin No"></asp:BoundField>
                        <asp:BoundField DataField="BOX_NUMBER" HeaderText="Box No"></asp:BoundField>
                        <asp:BoundField DataField="POSITION" HeaderText="Position"></asp:BoundField>
                        <asp:BoundField DataField="TDW_BOXNO" HeaderText="TDW Box"></asp:BoundField>
                        <asp:BoundField DataField="NAME" HeaderText="Name"></asp:BoundField>
                        <asp:BoundField DataField="SURNAME" HeaderText="Surname"></asp:BoundField>
                        <asp:TemplateField HeaderText="Found" ItemStyle-Font-Underline="true">
                            <ItemTemplate>
                                <u>&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" ItemStyle-Font-Underline="true">
                            <ItemTemplate>
                                <u>&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u>
                            </ItemTemplate>
                        </asp:TemplateField>
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

        <%--        <asp:Button ID="btnPrint"runat="server" Text="Print RMC Picklist" OnClick="btnPrint_Click" />--%>
    </form>
    <script type="text/javascript">
        function printList() {
            document.getElementById("btnPrint").style.display = "none";
            window.print();
            window.close();
        }
    </script>
</body>
</html>