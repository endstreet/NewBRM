<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileRequestIn.aspx.cs" Inherits="SASSADirectCapture.Views.FileRequestIn" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        @media print {
            /*, #divError2, #divSuccess2, #divError3, #divSuccess3*/
            .btn, #divError, #divSuccess {
                display: none;
            }
        }
    </style>
    <script type="text/javascript">
        function printPicklist(picklist) {
            var myPrintURL = "FileRequestPickList.aspx?picklist=" + picklist;
            var printwin = window.open(myPrintURL, "Print File Picklist");
            printwin.focus();
        }
    </script>
    <script type="text/javascript">
        function hideShowDiv(s) {
            switch (s) {
                case 1:
                    document.getElementById("tab-Historic").style.display = "";
                    document.getElementById("tab-RMCPick").style.display = "none";     //  2
                    document.getElementById("tab-TDWPick").style.display = "none";     //  3
                    $("#btnHistoric").addClass("btn-primary");
                    $("#btnRMCPick").removeClass("btn-primary");                   //  2
                    $("#btnTDWPick").removeClass("btn-primary");                   //  3

                    //alert("getHistoricData");
                    break;

                case 2:
                    document.getElementById("tab-RMCPick").style.display = "";
                    document.getElementById("tab-Historic").style.display = "none";    //  1
                    document.getElementById("tab-TDWPick").style.display = "none";     //  3
                    //alert("TDWPick ok");
                    $("#btnRMCPick").addClass("btn-primary");
                    $("#btnHistoric").removeClass("btn-primary");                  //  1
                    $("#btnTDWPick").removeClass("btn-primary");                   //  3

                    //alert("getHistoricData");
                    getRMCData();
                    break;

                case 3:
                    document.getElementById("tab-TDWPick").style.display = "";
                    document.getElementById("tab-Historic").style.display = "none";    //  1
                    document.getElementById("tab-RMCPick").style.display = "none";     //  2
                    $("#btnTDWPick").addClass("btn-primary");
                    $("#btnHistoric").removeClass("btn-primary");                  //  1
                    $("#btnRMCPick").removeClass("btn-primary");                   //  2

                    //alert("getHistoricData");
                    getTDWData();
                    break;
            }

            document.getElementById('<%= divSuccess.ClientID %>').style.display = 'none';
            document.getElementById('<%= divError.ClientID %>').style.display = 'none';

            document.getElementById('<%= lblError.ClientID %>').innerHTML = "";
            document.getElementById('<%= lblSuccess.ClientID %>').innerHTML = "";
        }
    </script>
    <script type="text/javascript">
        function updateRequest(fileNo) {
            PopupCenter('FileRequestDetail.aspx?fileNo=' + fileNo, 'File Request Detail', '600', '650', '');
        }
    </script>
    <script type="text/javascript">
        function PopupCenter(url, title, w, h, tab) {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            //var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;

            width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            //var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=no, width=' + w + ', height=' + h + ', top=10, left=' + left + ',' + tab);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }
    </script>
    <script type="text/javascript">
        function UpdateMessage(success, message, idNo, fileNo) {

            document.getElementById('<%= divSuccess.ClientID %>').style.display = 'none';
            document.getElementById('<%= divError.ClientID %>').style.display = 'none';

            document.getElementById('<%= lblError.ClientID %>').innerHTML = "";
            document.getElementById('<%= lblSuccess.ClientID %>').innerHTML = "";

            if (success == "true") {
                document.getElementById('<%= divSuccess.ClientID %>').style.display = '';
                document.getElementById('<%= lblSuccess.ClientID %>').innerHTML = message;

                getHistoricData();
            }
            else if (success == "false") {
                document.getElementById('<%= divError.ClientID %>').style.display = '';
                document.getElementById('<%= lblError.ClientID %>').innerHTML = message;
            }
            else {
                //Do nothing - clear messages - (use with close window function)
            }
        }
    </script>
    <script type="text/javascript">
        function getHistoricData() {
            //alert("getHistoricData");
            $("#<%= hiddenBtnData.ClientID %>").click();
        }
    </script>
    <script type="text/javascript">
        function getRMCData() {
            //$("#<%= hiddenBtnRMC.ClientID %>").click();
            //alert("getRMCData");
            $("#MainContent_hiddenBtnRMC").trigger("click");
        }
    </script>
    <script type="text/javascript">
        function getTDWData() {
            //$("#<%= hiddenBtnTDW.ClientID %>").click();
            //alert("getTDWData");
            $("#MainContent_hiddenBtnTDW").trigger("click");
        }
    </script>
    <div class="btn-group" role="group" aria-label="...">
        <button id="btnHistoric" type="button" class="btn btn-default btn-primary" onclick="hideShowDiv(1);">Incoming</button>
        <button id="btnRMCPick" type="button" class="btn btn-default" style="display: none" onclick="hideShowDiv(2);">RMC Picklist</button>
        <button id="btnTDWPick" type="button" class="btn btn-default" onclick="hideShowDiv(3);">TDW Picklist</button>
    </div>
    <div id="myPrint">
        <div id="tab-Historic">
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
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="false">
                <ContentTemplate>
                    <div style="display: none">
                        <asp:Button ID="hiddenBtnData" runat="server" OnClick="hiddenBtnData_Click"></asp:Button>
                    </div>
                    <asp:GridView ID="fileGridView" runat="server" CssClass="gridView" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" AllowPaging="True" PageSize="25" ForeColor="#333333" GridLines="None" OnPageIndexChanging="fileGridView_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="REQUESTED_DATE" HeaderText="Date Requested"></asp:BoundField>
                            <asp:BoundField DataField="REQUESTED_OFFICE_NAME" HeaderText="Office Requested"></asp:BoundField>
                            <asp:BoundField DataField="ID_NO" HeaderText="ID Number"></asp:BoundField>
                            <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM File Number"></asp:BoundField>
                            <asp:BoundField DataField="NAME" HeaderText="Name"></asp:BoundField>
                            <asp:BoundField DataField="SURNAME" HeaderText="Surname"></asp:BoundField>
                            <asp:BoundField DataField="MIS_FILE_NO" HeaderText="MIS File Number"></asp:BoundField>
                            <asp:BoundField DataField="BIN_ID" HeaderText="Bin No"></asp:BoundField>
                            <asp:BoundField DataField="BOX_NUMBER" HeaderText="Box No"></asp:BoundField>
                            <asp:BoundField DataField="POSITION" HeaderText="Position"></asp:BoundField>
                            <asp:BoundField DataField="TDW_BOXNO" HeaderText="TDW Box No"></asp:BoundField>
                            <asp:BoundField DataField="PICKLIST_STATUS" HeaderText="Pick List Status"></asp:BoundField>
                            <%-- <asp:BoundField DataField="GRANT_NAME" HeaderText="Grant Type"></asp:BoundField>--%>
                            <asp:TemplateField HeaderText="Action" ItemStyle-Font-Underline="true">
                                <ItemTemplate>
                                    <a href="javascript:updateRequest('<%# Eval("UNQ_FILE_NO") %>');">Detail</a>
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
        <div id="tab-RMCPick" style="display: none;">
            <!--ERROR DIV -->
            <div class="row" id="divError2" runat="server" style="display: none">
                <div class="form-group col-xs-12 alert alert-danger">
                    <asp:Label ID="lblError2" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                </div>
            </div>
            <!--SUCCESS DIV -->
            <div class="row" id="divSuccess2" runat="server" style="display: none">
                <div class="form-group col-xs-12 alert alert-success">
                    <asp:Label ID="lblSuccess2" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="false">
                <ContentTemplate>
                    <div style="display: none">
                        <asp:Button ID="hiddenBtnRMC" runat="server" OnClick="hiddenBtnRMC_Click"></asp:Button>
                    </div>
                    <asp:GridView ID="fileGridView2" runat="server" CssClass="gridView" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" AllowPaging="True" PageSize="25" ForeColor="#333333" GridLines="None" OnPageIndexChanging="fileGridView2_PageIndexChanging" OnDataBound="fileGridView2_DataBound">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
            <div class="row" id="divPrintRMC" runat="server">
                <asp:Button ID="btnPrintRMC" CssClass="btn btn-primary active pull-right" runat="server" Text="View RMC Picklist" OnClick="btnPrintRMC_Click" />
            </div>
        </div>
        <div id="tab-TDWPick" style="display: none;">
            <!--ERROR DIV -->
            <div class="row" id="divError3" runat="server" style="display: none">
                <div class="form-group col-xs-12 alert alert-danger">
                    <asp:Label ID="lblError3" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                </div>
            </div>
            <!--SUCCESS DIV -->
            <div class="row" id="divSuccess3" runat="server" style="display: none">
                <div class="form-group col-xs-12 alert alert-success">
                    <asp:Label ID="lblSuccess3" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" EnableViewState="false">
                <ContentTemplate>
                    <div style="display: none">
                        <asp:Button ID="hiddenBtnTDW" runat="server" OnClick="hiddenBtnTDW_Click"></asp:Button>
                    </div>
                    <asp:GridView ID="fileGridView3" runat="server" CssClass="gridView" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" AllowPaging="True" PageSize="25" ForeColor="#333333" GridLines="None" OnPageIndexChanging="fileGridView3_PageIndexChanging" OnDataBound="fileGridView3_DataBound">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="REQUESTED_DATE" HeaderText="Date Requested"></asp:BoundField>
                            <asp:BoundField DataField="REQUESTED_OFFICE_NAME" HeaderText="Office Requested"></asp:BoundField>
                            <asp:BoundField DataField="ID_NO" HeaderText="ID No"></asp:BoundField>
                            <asp:BoundField DataField="APPLICATION_STATUS" HeaderText="Registry Type"></asp:BoundField>
                            <asp:BoundField DataField="ARCHIVE_YEAR" HeaderText="Archive Year"></asp:BoundField>
                            <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM No"></asp:BoundField>
                            <asp:BoundField DataField="MIS_FILE_NO" HeaderText="File No"></asp:BoundField>
                            <asp:BoundField DataField="BIN_ID" HeaderText="Bin No"></asp:BoundField>
                            <asp:BoundField DataField="BOX_NUMBER" HeaderText="Box No"></asp:BoundField>
                            <asp:BoundField DataField="POSITION" HeaderText="Position"></asp:BoundField>
                            <asp:BoundField DataField="TDW_BOXNO" HeaderText="TDW Box"></asp:BoundField>
                            <asp:BoundField DataField="NAME" HeaderText="Name"></asp:BoundField>
                            <asp:BoundField DataField="SURNAME" HeaderText="Surname"></asp:BoundField>
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
            <br />
            <div class="row" id="divPrintTDW" runat="server">
                <asp:Button ID="btnPrintTDW" CssClass="btn btn-primary active pull-right" runat="server" Text="View TDW Picklist" OnClick="btnPrintTDW_Click" Style="display: none" />
                <asp:Button ID="btnExcelExport" runat="server" Text="Excel Export" CssClass="btn btn-primary active pull-right" OnClick="btnExcelExport_Click" />
            </div>
        </div>
    </div>
</asp:Content>