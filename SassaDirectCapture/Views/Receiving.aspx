<%@ Page Title="Receiving"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Receiving.aspx.cs"
    Inherits="SASSADirectCapture.Views.Receiving"
    Trace="false" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="btn-group" role="group" aria-label="...">
        <button id="btnIncomingTab" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">Incoming</button>
        <button id="btnDeliveryTab" type="button" class="btn btn-default btn-primary" onclick="hideShowDiv(this.id);">Accept Delivery</button>
        <%--<button id="btnReceivedTab" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">Received</button>--%>
    </div>
    <%--===========================================Accept Delivery=========================================--%>
    <div id="tab-delivery">
        <asp:UpdatePanel ID="DeliveryPanel" runat="server" EnableViewState="True">
            <ContentTemplate>
                <br />
                <asp:Label ID="Label1" runat="server" EnableViewState="False" Text="Delivery Accepted:" CssClass="alert-info"></asp:Label>
                <br />
                <li>
                <asp:Label ID="Label3" runat="server" EnableViewState="False" Text="Scan or type in the Batch number to take delivery of a batch, until all batches received are successfully added to receipt list." CssClass="alert-dismissable"></asp:Label>
                </li> 
                <li>
                <asp:Label ID="Label4" runat="server" EnableViewState="False" Text="Please do not forget to click the [Accept Batch] button when you are done, or you will have to do it again. Do not refresh your screen." CssClass="alert-dismissable"></asp:Label>
                </li>
                <div class="row" id="divDeliveryError" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="lblDeliveryError" runat="server" EnableViewState="true" CssClass="error"></asp:Label>
                    </div>
                </div>
                <div class="row" id="divDeliverySuccess" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="lblDeliverySuccess" runat="server" EnableViewState="true" CssClass="success"></asp:Label>
                    </div>
                </div>
                <br />
                <div style="display: table-row;">
                    <div style="vertical-align: middle;display: table-cell; padding:0 5px 0 5px;">
                        <asp:Label for="txtDeliverySearch" class="control-label text-left" runat="server" Text="Batch Number:"></asp:Label>
                    </div>
                    <div style="vertical-align:bottom;display:table-cell;padding:0 5px 0 5px;">
                        <asp:TextBox ID="txtDeliverySearch" runat="server" class="form-control"  Width="200px" placeholder="Scan Batch no or Type here..." onfocus="this.select();"></asp:TextBox>
                    </div>
                    <div style="vertical-align: middle;display: table-cell;padding:0 5px 0 5px;">
                        <asp:Button ID="btnDeliverySearch" runat="server" Text="Accept Batch" CssClass="btn btn-primary active" Enabled="true" OnClick="btnDeliverySearch_Click" />
                    </div>
                </div>
                <section class="contact">
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:GridView ID="deliveryGridView" runat="server" CssClass="gridView"
                        ShowHeaderWhenEmpty="True" AutoGenerateColumns="False"
                        AllowSorting="True" CellPadding="4"
                        AllowPaging="True" PageSize="25"
                        ForeColor="#333333" GridLines="None"
                        OnPageIndexChanging="deliveryGridView_PageIndexChanging"
                        OnRowCommand="deliveryGridView_RowCommand">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_DATE" HeaderText="Date Sent"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_NO" HeaderText="TDW Batch Order No"></asp:BoundField>
                            <asp:BoundField DataField="COURIER_NAME" HeaderText="Courier Name"></asp:BoundField>
                            <asp:BoundField DataField="NO_OF_FILES" HeaderText="No of Files"></asp:BoundField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="120px">
                                <ItemTemplate>
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
                    <hr />
                </section>
                <div style="text-align: right">
                    <asp:Label ID="lblDeliverSelected" runat="server" EnableViewState="false" Style="line-height: 35px; padding-right: 20px"></asp:Label>
                    <asp:Button ID="btnTakeDelivery" runat="server" Text="Take Delivery" Visible="false" OnClick="btnTakeDelivery_Click" CssClass="btn btn-primary active pull-right" Style="margin-right: 0px !important" ToolTip="btnTakeDelivery" />
                </div>
                <div style="display: none">
                    <asp:Button ID="btnTakeDeliveryReset" runat="server" Text="Take Delivery" OnClick="btnTakeDeliveryReset_Click" ToolTip="btnTakeDeliveryReset" />
                    <asp:Button ID="btnDeliverShow" runat="server" Text="Take Delivery" OnClick="btnDeliverShow_Click" ToolTip="btnDeliverShow" />
                </div>
                <%--<table style=" width:100%; background-color:black;"><tr style="height:1px;"><td></td></tr></table>--%>
                <%--<hr style="border-top-color: #000000;" />--%>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDeliverySearch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <%--==========================================Incoming==========================================--%>

    <div id="tab-incoming">
        <asp:UpdatePanel ID="IncomingPanel" runat="server" EnableViewState="true">
            <ContentTemplate>
                <hr />
                <asp:Label ID="lblIncomingLabel1" runat="server" EnableViewState="False" Text="Incoming:" CssClass="alert-info"></asp:Label>
                <div class="row" id="divIncomingError" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="lblIncomingError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                    </div>
                </div>
                <div style="display: none">
                    <asp:Button ID="btnHiddenIncomingSearch" runat="server" CssClass="btn btn-primary active" Text="List Incoming Batches" OnClick="btnHiddenIncomingSearch_Click" />
                </div>
                <div class="row" id="divIncomingSuccess" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="lblIncomingSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                    </div>
                </div>
                <section class="contact">
                    <asp:GridView ID="incomingGridView" runat="server" CssClass="gridView" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" AllowPaging="True" PageSize="50" ForeColor="#333333" GridLines="Both" OnPageIndexChanging="incomingGridView_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_DATE" HeaderText="Date Updated"></asp:BoundField>
                            <asp:BoundField DataField="BATCH_STATUS" HeaderText="Batch Status"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_NO" HeaderText="TDW Batch Order No"></asp:BoundField>
                            <asp:BoundField DataField="COURIER_NAME" HeaderText="Courier Name"></asp:BoundField>
                            <asp:BoundField DataField="NO_OF_FILES" HeaderText="No of Files"></asp:BoundField>
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
                <asp:AsyncPostBackTrigger ControlID="btnHiddenIncomingSearch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%--==========================================Received==========================================--%>
    <div id="tab-received">
        <%-- style="display: none"--%>
        <asp:UpdatePanel ID="ReceivedPanel" runat="server" EnableViewState="False">
            <ContentTemplate>
                <hr style="border-top-color: #000000;" />
                <div>
                    <asp:Label ID="lblReceived" runat="server" EnableViewState="False" Text="Received:" CssClass="alert-info"></asp:Label>
                </div>
                <div class="row" id="divReceiveError" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="lblReceiveError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                    </div>
                </div>
                <div style="display: none;">
                    <asp:Button ID="btnHiddenReceiveSearch" runat="server" CssClass="btn btn-primary active" Text="List Received Batches" OnClick="btnHiddenReceiveSearch_Click" />
                </div>
                <div class="row" id="divReceiveSuccess" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="lblReceiveSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                    </div>
                </div>
                <div class="form-horizontal col-sm-8">
                    Batch Number:
                    <input type="text" id="txtSearch2" onchange="View2();" style="width: 100px;" onfocus="this.select();" />&nbsp;[Quick find: Scan or Type in Batch number and press Tab]
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

    <%--==========================================scripts begin==========================================--%>
    <script type="text/javascript">
        //Hides the relevant div depending on the button 'tab' selected
        function hideShowDiv(elem) {
            if (elem == "btnIncomingTab") {
                document.getElementById("tab-incoming").style.display = "";
                document.getElementById("tab-delivery").style.display = "none";
                document.getElementById("tab-received").style.display = "none";
                $("#btnIncomingTab").addClass("btn-primary");
                if ($("#btnDeliveryTab").hasClass("btn-primary")) {
                    $("#btnDeliveryTab").removeClass("btn-primary");
                }
                //if ($("#btnReceivedTab").hasClass("btn-primary")) {
                //    $("#btnReceivedTab").removeClass("btn-primary");
                //}
                updateIncomingGrid();
                //alert("btnIncomingTab ok");
            }
            if (elem == "btnDeliveryTab") {
                document.getElementById("tab-incoming").style.display = "";
                document.getElementById("tab-delivery").style.display = "";
                document.getElementById("tab-received").style.display = "";
                $("#btnDeliveryTab").addClass("btn-primary");
                if ($("#btnIncomingTab").hasClass("btn-primary")) {
                    $("#btnIncomingTab").removeClass("btn-primary");
                }
                //if ($("#btnReceivedTab").hasClass("btn-primary")) {
                //    $("#btnReceivedTab").removeClass("btn-primary");
                //}

                updateIncomingGrid();
                updateDeliveryGrid();
                //alert("btnDeliveryTab ok");

                //var bn1 = document.getElementById("txtDeliverySearch");
                //bn1.focus();
            }
            //if (elem == "btnReceivedTab")
            //{
            //    document.getElementById("tab-incoming").style.display = "none";
            //    document.getElementById("tab-delivery").style.display = "none";
            //    document.getElementById("tab-received").style.display = "";
            //    $("#btnReceivedTab").addClass("btn-primary");
            //    if ($("#btnIncomingTab").hasClass("btn-primary")) {
            //        $("#btnIncomingTab").removeClass("btn-primary");
            //    }
            //    if ($("#btnDeliveryTab").hasClass("btn-primary")) {
            //        $("#btnDeliveryTab").removeClass("btn-primary");
            //    }
            //    //updateReceiveGrid();
            //    //alert("btnReceivedTab ok");

            //    //var bn2 = document.getElementById("txtSearch2");
            //    //bn2.focus();

            //    WebForm_AutoFocus('txtSearch2');
            //}
        }
    </script>

    <%--    scripts for incoming batches --%>

    <script type="text/javascript">
        function updateIncomingGrid() {
            $("#MainContent_btnHiddenIncomingSearch").trigger("click");
        }
    </script>

    <%--    scripts for accept delivery of batches  --%>

    <script type="text/javascript">
        function updateDeliveryGrid() {
            $("#MainContent_txtSearch").val('');
            $("#MainContent_btnDeliverShow").trigger("click");
        }
    </script>

    <script type="text/javascript">
        function openCourierSignPage() {
            //alert('Before TransportCover.aspx');
            var newWindow = window.open('TransportCover.aspx', '_blank');
            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }
    </script>

    <script type="text/javascript">
        function UpdateDeliveryGridReset() {
            $("#MainContent_txtDeliverySearch").val('');
            $("#MainContent_btnDeliverReset").trigger("click");
        }
    </script>

    <%--    scripts for received of batches --%>

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
                            //taken out//console.log("Barcode Scanned: " + barcode);
                            // assign value to some input (or do whatever you want)
                            $("#MainContent_txtSearch").val(barcode);
                            //taken out();
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
                UpdateReceivedGrid();
            }
        });
    </script>

    <script type="text/javascript">
        function UpdateReceivedGrid() {
            $("#MainContent_btnHiddenReceiveSearch").trigger("click");
        }
    </script>

    <script type="text/javascript">
        function View(batchNo) {
            document.getElementById("txtSearch2").value = batchNo;  // !!!!!!!!!!!!
            PopupCenter('ReceivingFile.aspx?batchNo=' + batchNo, 'View Received Batch', '900', '600', '');
        }
    </script>

    <script type="text/javascript">
        function View2() {
            var batchNo = document.getElementById("txtSearch2").value;  // !!!!!!!!!!!!!!
            if ((batchNo == "") || (batchNo == null)) {
                alert("Please enter or scan the batch barcode.");
                return false;
            }

            WebForm_AutoFocus('txtSearch2');

            PopupCenter('ReceivingFile.aspx?batchNo=' + batchNo, 'View Received Batch', '900', '600', '');
        }
    </script>

    <script type="text/javascript">
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
    </script>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            if (document.getElementById("tab-delivery").style.display == "") {
                WebForm_AutoFocus('<%= txtDeliverySearch.ClientID%>');
            }
            //else if (document.getElementById("tab-received").style.display == "")
            //{
            //    document.getElementById('txtSearch2').focus();
            //}
        }
    </script>
    <%--==========================================scripts end==========================================--%>
</asp:Content>