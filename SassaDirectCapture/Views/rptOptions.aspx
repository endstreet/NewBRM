<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rptOptions.aspx.cs" Inherits="SASSADirectCapture.Reports.rptOptions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/themes/base/datepicker.css" rel='stylesheet' type='text/css' />
    <link href="../Content/themes/base/theme.css" rel='stylesheet' type='text/css' />
    <script type="text/javascript">
        $(function () {
            $("#<%=txtDateFrom.ClientID%>").datepicker({
                changeMonth: true,
                dateFormat: 'yy/mm/dd',
                changeYear: true
            });

            $("#<%=txtDateTo.ClientID%>").datepicker({
                changeMonth: true,
                dateFormat: 'yy/mm/dd',
                changeYear: true,
                maxDate: '0'
            });
        });
    </script>
    <div class="row" id="divError" runat="server" visible="false">
        <div class="form-group col-xs-12 alert alert-danger">
            <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
        </div>
    </div>
    <div >
        <div>
            <table>
                <tr>
                    <td>
                        <div>
                            <asp:DropDownList ID="drpOptions" CssClass="form-control" runat="server" OnSelectedIndexChanged="drpOptions_SelectedIndexChanged" AutoPostBack="True" Width="344px">
                                <asp:ListItem>-- Select Report --</asp:ListItem>
                                <asp:ListItem>Destruction List</asp:ListItem>
                                <asp:ListItem>Destruction Status</asp:ListItem>
                                <asp:ListItem>Missing Files Report</asp:ListItem>
                                <asp:ListItem>All Grants Report</asp:ListItem>
                                <asp:ListItem>Activity Log</asp:ListItem>
                                <asp:ListItem>Activity By Action per User</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                      </td>
                    <td>
                    <asp:Button ID="btnRun" runat="server" Text="Load" CssClass="btn btn-primary active" OnClick="btnRun_Click"></asp:Button>
                    </td><td>
                    <asp:Button ID="btnExcelExport" runat="server" Text="Excel Export" CssClass="btn btn-primary active" Visible="false" OnClick="btnExcelExport_Click" Style="float: right;" download="true"/>
                    </td>
                    </tr>
            </table>
        </div>
        <br/>
        <asp:Label ID="lblOptions" runat="server" CssClass="control-label text-left" Style="padding-left: 5px; text-align: left; max-width: 100px; width: 100px;  white-space:nowrap;" Text="Filter Options:"></asp:Label>
        <hr />
        
        <div class="col-sm-4" style="min-width: 400px; max-width: 400px; padding-left: 5px; padding-right: 5px;">

            <div id="dateRange" runat="server">
                <asp:Label runat="server" for="txtDateFrom" Text="Date From:" Style="min-width: 100px; max-width: 100px; padding-left: 5px; padding-right: 5px; width: 100px;" />
                <input name="DateFrom" id="txtDateFrom" type="text" runat="server" cssclass="form-control col-sm-6" style="width: 110px; margin: 0; height: 34px" />
                <asp:Label ID="lblDateTo" for="txtDateTo" runat="server" Text="Date To:" Style="min-width: 100px; max-width: 100px; padding-left: 5px; padding-right: 5px; width: 100px;" />
                <input name="DateTo" id="txtDateTo" type="text" runat="server" cssclass="form-control col-sm-6" style="width: 95px; margin: 0; height: 34px" />
            </div>
            <div id="yearRange" runat="server">
                <asp:DropDownList ID="ddYears" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <asp:DropDownList ID="ddRegion" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddRegion_SelectedIndexChanged" AutoPostBack="true" >
                <asp:ListItem Text="-- All Regions --" Value="" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Eastern Cape" Value="EC:2:ECA"></asp:ListItem>
                <asp:ListItem Text="Free State" Value="FS:4:FST"></asp:ListItem>
                <asp:ListItem Text="Gauteng" Value="GP:7:GAU"></asp:ListItem>
                <asp:ListItem Text="Kwazulu Natal" Value="KZN:5:KZN"></asp:ListItem>
                <asp:ListItem Text="Limpopo" Value="LP:9:LIM"></asp:ListItem>
                <asp:ListItem Text="Mpumalanga" Value="MP:8:MPU"></asp:ListItem>
                <asp:ListItem Text="Northern Cape" Value="NC:3:NCA"></asp:ListItem>
                <asp:ListItem Text="North West" Value="NW:6:NWP"></asp:ListItem>
                <asp:ListItem Text="Western Cape" Value="WC:1:WCA"></asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddOffice" runat="server" CssClass="form-control">
                <asp:ListItem Text="-- All Offices --" Value="" Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddGrantType" CssClass="form-control">
                <asp:ListItem Text="---Select Grant Type---" Value="" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Old age grant" Value="0"></asp:ListItem>
                <asp:ListItem Text="War veteran grant" Value="1"></asp:ListItem>
                <asp:ListItem Text="Disability grant" Value="3"></asp:ListItem>
                <asp:ListItem Text="State maintenance grant" Value="4"></asp:ListItem>
                <asp:ListItem Text="Foster care grant" Value="5"></asp:ListItem>
                <asp:ListItem Text="Combination grant" Value="6"></asp:ListItem>
                <asp:ListItem Text="Government Institution" Value="7"></asp:ListItem>
                <asp:ListItem Text="Grant in Aid" Value="8"></asp:ListItem>
                <asp:ListItem Text="Care dependency grant" Value="9"></asp:ListItem>
                <asp:ListItem Text="Child support grant" Value="C"></asp:ListItem>
                <asp:ListItem Text="Social relief of distress grant" Value="S"></asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList runat="server" ID="ddDestructionStatus" CssClass="form-control">
            </asp:DropDownList>
        </div>
    </div>
    <br />
    <!-- Destruction List-->
    <div class="divReportGridview" id="divResult" runat="server">
        <asp:GridView ID="grdDestruction" runat="server" CssClass="gridView" CellPadding="4"
            Font-Size="Small" Font-Bold="False" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" AllowSorting="True"
            AllowPaging="True" PageSize="50"
            OnPageIndexChanging="grdDestruction_PageIndexChanging" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <SelectedRowStyle BackColor="#E2DED6" />
            <Columns>
                <asp:BoundField DataField="REGION_NAME" HeaderText="Region" />
                <asp:BoundField DataField="PENSION_NO" HeaderText="Pension No" SortExpression="ApplicationNumber" />
                <asp:BoundField DataField="NAME" HeaderText="Name" />
                <asp:BoundField DataField="SURNAME" HeaderText="Surname" />
                <asp:BoundField DataField="GRANT_TYPE" HeaderText="Grant Type" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" ForeColor="White" />
            <PagerSettings Position="Bottom" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
    </div>
    <!-- Destruction Status-->
    <div class="divReportGridview" id="divDestructionStatus" runat="server">
        <asp:GridView ID="grdDestructionStatus" runat="server" CssClass="gridView" CellPadding="4"
            Font-Size="Small" Font-Bold="False" AutoGenerateColumns="True" ShowHeaderWhenEmpty="True" AllowSorting="True"
            AllowPaging="True" PageSize="50"
            OnPageIndexChanging="grdDestruction_PageIndexChanging" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <SelectedRowStyle BackColor="#E2DED6" />
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" ForeColor="White" />
            <PagerSettings Position="Bottom" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
    </div>
    <!-- Missing Files-->
    <div class="divReportGridview" id="divMissingFiles" runat="server">
        <asp:GridView ID="grdMissingFiles" runat="server" CssClass="gridView" CellPadding="4"
            Font-Size="Small"
            Font-Bold="false"
            CellSpacing="5" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
            AllowSorting="True" AllowPaging="True" PageSize="30"
            OnPageIndexChanging="grdMissingFiles_PageIndexChanging"
            ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <SelectedRowStyle BackColor="#E2DED6" />
            <Columns>
                <asp:BoundField DataField="REGION" HeaderText="Branch" />
                <asp:BoundField DataField="SEC_PAYPOINT" HeaderText="District" />
                <asp:BoundField DataField="PENSION_NO" HeaderText="Pension No" />
                <asp:BoundField DataField="NAME" HeaderText="Name" />
                <asp:BoundField DataField="SURNAME" HeaderText="Surname" />
                <asp:BoundField DataField="GRANT_TYPE" HeaderText="Grant Type" />
                <asp:BoundField DataField="ORIGINAL_APPLICATION_DATE" HeaderText="App Date" />
                <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
<%--                <asp:BoundField DataField="SourceTBL" HeaderText="Source" />--%>
                <asp:BoundField DataField="AGE" HeaderText="Age" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" ForeColor="White" />
            <PagerSettings Position="Bottom" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
    </div>
    <!-- Activity log-->
    <div class="divReportGridview" id="divActivityLog" runat="server">
        <asp:GridView ID="grdActivityLog" runat="server" CssClass="gridView" CellPadding="4" Font-Size="Small" Font-Bold="false"
            CellSpacing="5" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="True" PageSize="50"
            OnPageIndexChanging="grdActivityLog_PageIndexChanging"
            ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <SelectedRowStyle BackColor="#E2DED6" />
            <Columns>
                <asp:BoundField DataField="ACTIVITY_DATE" HeaderText="Date" />
                <asp:BoundField DataField="USERNAME" HeaderText="User" />
                <asp:BoundField DataField="AREA" HeaderText="Area" />
                <asp:BoundField DataField="ACTIVITY" HeaderText="Activity" />
                <asp:BoundField DataField="OFFICE_NAME" HeaderText="Office" />
                <%--<asp:BoundField DataField="RESULT" HeaderText="Result" />--%>
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" ForeColor="White" />
            <PagerSettings Position="Bottom" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
    </div>
    <!-- Activity pivot-->
    <div class="divReportGridview" id="divActivityPivot" runat="server">
        <asp:GridView ID="grdActivityPivot" runat="server" CssClass="gridView" CellPadding="4" Font-Size="Small" Font-Bold="false"
            CellSpacing="5" AutoGenerateColumns="True" ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="True" PageSize="50"
            OnPageIndexChanging="grdActivityPivot_PageIndexChanging"
            ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <SelectedRowStyle BackColor="#E2DED6" />
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" ForeColor="White" />
            <PagerSettings Position="Bottom" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 379px;
        }
    </style>
</asp:Content>