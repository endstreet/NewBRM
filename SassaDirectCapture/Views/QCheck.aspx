<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QCheck.aspx.cs" Inherits="SASSADirectCapture.Views.QCheck" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        $(document).ready(function () {
            var pressed = false;
            var chars = [];
            $(window).keypress(function (e) {
                if (e.which === 13) {
                    console.log("Prevent form submit.");
                    e.preventDefault();
                }
                if (e.which >= 48 && e.which <= 57) {
                    chars.push(String.fromCharCode(e.which));
                }
                console.log(e.which + ":" + chars.join("|"));
                if (pressed == false) {
                    setTimeout(function () {
                        if (chars.length >= 3) {
                            var barcode = chars.join("");
                            console.log("Barcode Scanned: " + barcode);
                            // assign value to some input (or do whatever you want)
                            $("#MainContent_txtSearch").val(barcode);
                            UpdateGrid();
                        }
                        chars = [];
                        pressed = false;
                    }, 300);
                }
                pressed = true;
            });
        });

        $("#MainContent_txtSearch").keypress(function (e) {
            if (e.which === 13) {
                console.log("Prevent form submit.");
                e.preventDefault();
                UpdateGrid();
            }
        });

        function UpdateGrid() {
            $("#MainContent_btnSearch").trigger("click");
        }

        function View(batchNo) {
            //window.open('ReceivingFile.aspx?batchNo=' + batchNo, 'View Receiving Batch', 'width=400, height=300');
            PopupCenter('ReceivingFile.aspx?batchNo=' + batchNo, 'View Received Batch', '900', '600', '');
        }

        function PopupCenter(url, title, w, h, tab) {
            // Fixes dual-screen position                         Most browsers      Firefox
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;

            width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            //var newWindow = window.open(url, title, 'scrollbars=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left + ',' + tab);
            var newWindow = window.open(url, title);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }

        //Hides the relevant div depending on the button 'tab' selected
        function hideShowDiv(elem) {

            if (elem == "btnTransport") {
                document.getElementById("tab-transport").style.display = "";
                document.getElementById("tab-received").style.display = "none";
                $("#btnReceived").removeClass("btn-primary");
                $("#btnTransport").addClass("btn-primary");
                loadReceiveGrid();
            }
            else if (elem == "btnReceived") {
                document.getElementById("tab-transport").style.display = "none";
                document.getElementById("tab-received").style.display = "";
                $("#btnTransport").removeClass("btn-primary");
                $("#btnReceived").addClass("btn-primary");
                UpdateReceiveGrid();
            }
        }

        function loadReceiveGrid() {
            $("#MainContent_txtSearch").val('');
            $("#MainContent_btnDeliverShow").trigger("click");
        }

        function UpdateReceiveGrid() {
            $("#MainContent_btnHiddenReceiveSearch").trigger("click");
        }

        function openCourierSignPage() {

            var newWindow = window.open('TransportCover.aspx', '_blank');
            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }

        function UpdateDeliveryGridReset() {
            $("#MainContent_txtSearch").val('');
            $("#MainContent_btnDeliverReset").trigger("click");
        }
    </script>
    <div class="btn-group" role="group" aria-label="...">
        <button id="btnTransport" type="button" class="btn btn-default btn-primary" onclick="hideShowDiv(this.id);">Transport Delivery</button>
        <button id="btnReceived" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">Received</button>
    </div>
    <div id="tab-transport">
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="true">
            <ContentTemplate>
                <div class="row" id="divError" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="true" CssClass="error"></asp:Label>
                    </div>
                </div>
                <div class="row" id="divSuccess" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="lblSuccess" runat="server" EnableViewState="true" CssClass="success"></asp:Label>
                    </div>
                </div>
                <div class="form-horizontal col-sm-8">
                    <asp:Label for="txtSearch" class="col-sm-3 control-label text-left" Style="padding-left: 5px; padding-right: 0px; width: 17%; text-align: left" runat="server" Text="Batch Number:"></asp:Label>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control col-sm-3" Width="200px" placeholder="Enter here..."></asp:TextBox>&nbsp;
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary active" />
                    The batch is compliant.<asp:CheckBox ID="CheckBox1" runat="server" />
                    &nbsp;<div style="display: block; text-align: left; width: 517px;"></div>
                </div>
                <br />
                <br />
                <section class="contact">
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:GridView ID="batchGridView" runat="server" CssClass="gridView" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" AllowPaging="True" PageSize="25" ForeColor="#333333" GridLines="None" OnPageIndexChanging="batchGridView_PageIndexChanging" OnRowCommand="batchGridView_RowCommand">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_DATE" HeaderText="Date Sent"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_NO" HeaderText="TDW Batch Order No"></asp:BoundField>
                            <asp:BoundField DataField="COURIER_NAME" HeaderText="Courier Name"></asp:BoundField>
                            <asp:BoundField DataField="NO_OF_FILES" HeaderText="No of Files"></asp:BoundField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="120px">
                                <ItemTemplate>
                                    <%--<a style="text-decoration: underline" href="javascript:View('<%# Eval("BATCH_NO") %>');">View</a>--%>
                                    <asp:LinkButton ID="lnkRemoveBatch" runat="server" Style="text-decoration: underline" CommandName="lnkRemove" CommandArgument='<%#Eval("BATCH_NO")%>'>Remove</asp:LinkButton>
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
                </section>
                <br />
                <div style="text-align: right">
                    <asp:Label ID="lblDeliverSelected" runat="server" EnableViewState="false" Style="line-height: 35px; padding-right: 20px"></asp:Label>
                    <asp:Button ID="btnTransportDeliver" runat="server" Text="Take Delivery" Visible="false" OnClick="btnTransportDeliver_Click" CssClass="btn btn-primary active pull-right" Style="margin-right: 0px !important" />
                </div>
                <div style="display: none">
                    <asp:Button ID="btnDeliverReset" runat="server" Text="Take Delivery" OnClick="btnDeliverReset_Click" />
                    <asp:Button ID="btnDeliverShow" runat="server" Text="Take Delivery" OnClick="btnDeliverShow_Click" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div id="tab-received" style="display: none">
        <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="False">
            <ContentTemplate>
                <div style="display: none">
                    <asp:Button ID="btnHiddenReceiveSearch" runat="server" CssClass="btn btn-primary active" Text="Search" OnClick="btnHiddenReceiveSearch_Click" />
                </div>
                <div class="row" id="divReceiveError" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="lblReceiveError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                    </div>
                </div>
                <div class="row" id="divReceiveSuccess" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="lblReceiveSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                    </div>
                </div>
                <section class="contact">
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:GridView ID="receiveGridView" runat="server" CssClass="gridView" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" AllowPaging="True" PageSize="25" ForeColor="#333333" GridLines="None" OnPageIndexChanging="receiveGridView_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_DATE" HeaderText="Date Updated"></asp:BoundField>
                            <asp:BoundField DataField="BATCH_STATUS" HeaderText="Batch Status"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_NO" HeaderText="TDW Batch Order No"></asp:BoundField>
                            <asp:BoundField DataField="COURIER_NAME" HeaderText="Courier Name"></asp:BoundField>
                            <asp:BoundField DataField="NO_OF_FILES" HeaderText="No of Files"></asp:BoundField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <a style="text-decoration: underline" href="javascript:View('<%# Eval("BATCH_NO") %>');">View</a>
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
                </section>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnHiddenReceiveSearch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 571px;
        }
    </style>
</asp:Content>