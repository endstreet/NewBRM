<%@ Page Title=""
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="BoxRequestPickList.aspx.cs"
    Inherits="SASSADirectCapture.Views.BoxRequestPickList" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title><%: SassaPage.Title %> - SASSA Beneficiary Records Management BOX PICKLIST</title>
    <link href="~/Content/bootstrap.css" rel='stylesheet' type='text/css' />
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <center><h3 id="head1"><asp:Label ID="lblHead" runat="server"></asp:Label></h3><input  id="btnPrint" type="button" style="width:200px;" class="btn-danger" value="Print" onclick="this.style.display = 'none'; window.print(); window.close();" /></center>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server"
            EnableViewState="false">
            <ContentTemplate>
                <asp:GridView ID="BoxlistGridView" runat="server"
                    CssClass="gridView"
                    ShowHeaderWhenEmpty="True"
                    SelectMethod="GetBoxRequestPickList"
                    AutoGenerateColumns="False"
                    CellPadding="2"
                    AllowPaging="True"
                    PageSize="25"
                    ForeColor="#333333"
                    GridLines="Both">
                    <Columns>
                        <asp:BoundField DataField="UNQ_PICKLIST" HeaderText="Picklist Number"></asp:BoundField>
                        <asp:BoundField DataField="REGION_ID" HeaderText="Region ID" Visible="false"></asp:BoundField>
                        <asp:BoundField DataField="REGISTRY_TYPE" HeaderText="Registry Type"></asp:BoundField>
                        <asp:BoundField DataField="PICKLIST_DATE" HeaderText="Picklist Date"></asp:BoundField>
                        <asp:BoundField DataField="PICKLIST_STATUS" HeaderText="Completed" Visible="false"></asp:BoundField>
                        <asp:BoundField DataField="ARCHIVE_YEAR" HeaderText="Archive Year"></asp:BoundField>
                        <asp:BoundField DataField="BIN_NUMBER" HeaderText="BIN Number"></asp:BoundField>
                        <asp:BoundField DataField="BOX_NUMBER" HeaderText="BOX Number"></asp:BoundField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Received">
                            <ItemTemplate>
                                <img src="../Content/images/tickboxbig.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Completed">
                            <ItemTemplate>
                                <img src="../Content/images/tickboxbig.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Notes" ItemStyle-Font-Underline="false">
                            <ItemTemplate>
                                &nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
    </form>
    <table>
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
                <div id="divDateTime" style="margin-right: 15px; width: 25%">
                    <br />
                    <asp:Label class="col-sm-12 control-label text-left" runat="server" Text="Date"></asp:Label>
                </div>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        function printList() {
            document.getElementById("btnPrint").style.display = "none";
            window.print();
            window.close();
        }
        $(document).ready(function () {
            window.focus();
        }
    </script>
</body>
</html>