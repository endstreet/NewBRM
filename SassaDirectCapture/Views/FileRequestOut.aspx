<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileRequestOut.aspx.cs" Inherits="SASSADirectCapture.Views.FileRequestOut" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function pageLoad(sender, args) {
            WebForm_AutoFocus('<%= txtSearchID.ClientID %>');
        }

        function hideShowDiv(s) {

            $("#btnNew").removeClass("btn-primary");                        //  1
            $("#btnHistoric").removeClass("btn-primary");                   //  2

            document.getElementById("tab-new").style.display = "none";          //  1
            document.getElementById("tab-historic").style.display = "none";     //  2

            switch (s) {
                case 1:
                    document.getElementById("tab-new").style.display = "";
                    $("#btnNew").addClass("btn-primary");
                    WebForm_AutoFocus('<%= txtSearchID.ClientID%>');
                    break;

                case 2:
                    document.getElementById("tab-historic").style.display = "";
                    $("#btnHistoric").addClass("btn-primary");
                    break;
            }
            document.getElementById('<%= divSuccess.ClientID %>').style.display = 'none';
            document.getElementById('<%= divError.ClientID %>').style.display = 'none';

            document.getElementById('<%= lblError.ClientID %>').innerHTML = "";
            document.getElementById('<%= lblSuccess.ClientID %>').innerHTML = "";
        }

        function Edit(idNumber, fileNumber, name, surname, regionID, region, BRM_Number, MISBin, MISBox, MISPos, TDWBoxno, AppStatus) {

            var myURL = 'FileRequestEdit.aspx?status=1&idNo=' + idNumber;
            myURL += '&fileNo=' + fileNumber;
            myURL += '&name=' + name;
            myURL += '&surname=' + surname;
            myURL += '&regionID=' + regionID;
            myURL += '&region=' + region;
            myURL += '&BRM=' + BRM_Number;
            myURL += '&Bin=' + MISBin;
            myURL += '&Box=' + MISBox;
            myURL += '&Pos=' + MISPos;
            myURL += '&TDW=' + TDWBoxno;
            myURL += '&AppStatus=' + AppStatus;

            PopupCenter(myURL, 'File Request', '600', '630', '');
        }

        function closeCancelRequest(fileNo) {
            PopupCenter('FileRequestEdit.aspx?status=2&fileNo=' + fileNo, 'File Request', '600', '630', '');
        }

        function PopupCenter(url, title, w, h, tab) {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;

            width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var newWindow = window.open(url, title, 'scrollbars=no, width=' + w + ', height=' + h + ', top=10, left=' + left + ',' + tab);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }

        function UpdateMessage(success, message, idNo, fileNo) {
            document.getElementById('<%= divSuccess.ClientID %>').style.display = 'none';
            document.getElementById('<%= divError.ClientID %>').style.display = 'none';

            document.getElementById('<%= lblError.ClientID %>').innerHTML = "";
            document.getElementById('<%= lblSuccess.ClientID %>').innerHTML = "";

            if (success == "true") {
                document.getElementById('<%= divSuccess.ClientID %>').style.display = '';
                document.getElementById('<%= lblSuccess.ClientID %>').innerHTML = message;

                if (document.getElementById("tab-historic").style.display != "none") {
                    getHistoricData();
                }
            }
            else if (success == "false") {
                document.getElementById('<%= divError.ClientID %>').style.display = '';
                document.getElementById('<%= lblError.ClientID %>').innerHTML = message;
            }
            else {
                //Do nothing - clear messages - (use with close window function)
            }
        }

        function getHistoricData() {
            $("#<%= hiddenBtnData.ClientID %>").click();
        }
    </script>
    <div class="btn-group" role="group" aria-label="...">
        <button id="btnNew" type="button" class="btn btn-default btn-primary" onclick="hideShowDiv(1);">New</button>
        <button id="btnHistoric" type="button" class="btn btn-default" onclick="hideShowDiv(2); getHistoricData();">Historic</button>
    </div>
    <br />
    <div id="tab-new">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <br />
                <!--ERROR DIV -->
                <div class="row" id="divError" runat="server" style="display: none">
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                    </div>
                </div>
                <!--SUCCESS DIV -->
                <div class="row" id="divSuccess" runat="server" style="display: none">
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="lblSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                    </div>
                </div>
                <div class="form-horizontal" >
                    <table style="width:800px">
                        <tr>
                            <td nowrap>&nbsp;&nbsp;
                                <asp:Label for="txtSearchID" class="control-label text-left" runat="server" Text="ID Number:"></asp:Label>
                            </td>
                            <td nowrap>
                                <asp:TextBox ID="txtSearchID" runat="server" CssClass="form-control" Width="200px" placeholder="Enter here..." onfocus="this.select();"></asp:TextBox>
                            </td>
                            <td nowrap>&nbsp;&nbsp;
                                <asp:Label for="txtSearchBRMNo" class="control-label text-left" runat="server" Text="BRM Number:"></asp:Label>
                            </td>
                            <td nowrap>
                                <asp:TextBox ID="txtSearchBRMNo" runat="server" CssClass="form-control" Width="200px" placeholder="Enter here..." onfocus="this.select();"></asp:TextBox>
                            </td>
                            <td nowrap>&nbsp;&nbsp;
                                <asp:Label for="txtSearchCLMNo" class="control-label text-left" runat="server" Text="CLM Number:"></asp:Label>
                            </td>
                            <td nowrap>
                                <asp:TextBox ID="txtSearchCLMNo" runat="server" CssClass="form-control" Width="200px" placeholder="Enter here..." onfocus="this.select();"></asp:TextBox>
                            </td>
                            <td nowrap>&nbsp;&nbsp;
                                <asp:Label for="txtSearchSrdNo" class="control-label text-left" runat="server" Text="SRD Number:"></asp:Label>
                            </td>
                            <td nowrap>
                                <asp:TextBox ID="txtSearchSrdNo" runat="server" CssClass="form-control" Width="200px" placeholder="Enter here..." onfocus="this.select();"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="text-align: center;">
                                <br />
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="myECM.showPleaseWait('Searching...');" CssClass="btn btn-primary active" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <asp:GridView ID="gvResults" runat="server" CssClass="gridView" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gvResults_PageIndexChanging" PageSize="25" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="ID_Number" HeaderText="ID Number" />
                        <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM File Number" />
                        <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No" />
                        <asp:BoundField DataField="Type" HeaderText="Reg Type" />
                        <asp:BoundField DataField="UNQ_FILE_NO" HeaderText="CLM File Number" />
                        <asp:BoundField DataField="File_Number" HeaderText="MIS File Number" />
                        <asp:BoundField DataField="GrantType" HeaderText="Grant Type" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Surname" HeaderText="Surname" />
                        <asp:BoundField DataField="MISBin" HeaderText="Bin" />
                        <asp:BoundField DataField="MISBox" HeaderText="Box" />
                        <asp:BoundField DataField="MISPos" HeaderText="Position" />
                        <asp:BoundField DataField="TDWBoxno" HeaderText="TDW Box No" />
                        <asp:BoundField DataField="AppDate" HeaderText="Application Date" />
                        <asp:BoundField DataField="ArchiveDate" HeaderText="Archived" />
                        <asp:BoundField DataField="PARENT_BRM_NUMBER" HeaderText="Parent BRM" />
                        <asp:BoundField DataField="UPDATED_BY" HeaderText="Updated By" />
                        <asp:TemplateField HeaderText="Action" ItemStyle-Font-Underline="true">
                            <ItemTemplate>
                                <a href="javascript:Edit('<%# Eval("ID_Number") %>','<%# Eval("File_Number") %>','<%# Eval("Name") %>','<%# Eval("Surname") %>','<%# Eval("RegionID") %>','<%# Eval("Region") %>','<%# Eval("BRM_Barcode") %>','<%# Eval("MISBin") %>','<%# Eval("MISBox") %>','<%# Eval("MISPos") %>','<%# Eval("TDWBoxno") %>', '<%# Eval("AppStatus") %>');">Request</a>
                            </ItemTemplate>
                        </asp:TemplateField>
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
    </div>
    <div id="tab-historic" style="display: none">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="false">
            <ContentTemplate>
                <div style="display: none">
                    <asp:Button ID="hiddenBtnData" runat="server" OnClick="hiddenBtnData_Click"></asp:Button>
                </div>
                <asp:GridView ID="fileGridView" runat="server" CssClass="gridView" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" AllowPaging="True" PageSize="20" ForeColor="#333333" GridLines="None" OnPageIndexChanging="fileGridView_PageIndexChanging">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="REQUESTED_DATE" HeaderText="Date Requested"></asp:BoundField>
                        <asp:BoundField DataField="ID_NO" HeaderText="ID No"></asp:BoundField>
                        <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM Number"></asp:BoundField>
                        <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No"></asp:BoundField>
                        <asp:BoundField DataField="MIS_FILE_NO" HeaderText="MIS File Number"></asp:BoundField>
                        <asp:BoundField DataField="NAME" HeaderText="Name"></asp:BoundField>
                        <asp:BoundField DataField="SURNAME" HeaderText="Surname"></asp:BoundField>
                        <asp:BoundField DataField="REQUESTER_NAME" HeaderText="Requested By"></asp:BoundField>
                        <asp:BoundField DataField="STATUS" HeaderText="Status"></asp:BoundField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-Font-Underline="true">
                            <ItemTemplate>
                                <a href="javascript:closeCancelRequest('<%# Eval("UNQ_FILE_NO") %>');"><%# Eval("SCANNED_DATE") == null ? "Cancel" : "Complete" %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
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
        </asp:UpdatePanel>
    </div>
</asp:Content>