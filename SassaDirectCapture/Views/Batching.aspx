<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Batching.aspx.cs" Inherits="SASSADirectCapture.Views.Batching" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        $(document).ready(function () {
            RepopValues();

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
                            //alert("Barcode Scanned: " + barcode);
                            // assign value to some input (or do whatever you want)
                            $("#MainContent_txtSearchBarcode").val(barcode);
                            //UpdateGrid();
                            $("#MainContent_btnSearchBatchNo").trigger("click");
                        }
                        chars = [];
                        pressed = false;
                    }, 300);
                }
                pressed = true;
            });
        });

        $("#MainContent_txtSearchBarcode").keypress(function (e) {
            if (e.which === 13) {
                console.log("Prevent form submit.");
                e.preventDefault();
            }
        });

        //Hides the relevant div depending on the button 'tab' selected
        function hideShowDiv(elem) {
            if (elem == "btnCurrent") {
                document.getElementById("tab-current").style.display = "";
                document.getElementById("tab-closed").style.display = "none";
                document.getElementById("tab-courier").style.display = "none";
                //document.getElementById("sessionvars").style.display = "none";
                $("#btnClosed").removeClass("btn-primary");
                $("#btnCourier").removeClass("btn-primary");
                $("#btnCurrent").addClass("btn-primary");

                UpdateCurrentGrid();
            }
            else if (elem == "btnClosed") {
                document.getElementById("tab-current").style.display = "none";
                document.getElementById("tab-closed").style.display = "";
                document.getElementById("tab-courier").style.display = "none";
                //document.getElementById("sessionvars").style.display = "none";
                $("#btnCurrent").removeClass("btn-primary");
                $("#btnClosed").addClass("btn-primary");
                $("#btnCourier").removeClass("btn-primary");

                //Need to click the reset search button when tab is selected.
                UpdateClosedGridReset();
            }
            else if (elem == "btnCourier") {
                document.getElementById("tab-current").style.display = "none";
                document.getElementById("tab-closed").style.display = "none";
                document.getElementById("tab-courier").style.display = "";
                //document.getElementById("sessionvars").style.display = "";
                $("#btnCurrent").removeClass("btn-primary");
                $("#btnCourier").addClass("btn-primary");
                $("#btnClosed").removeClass("btn-primary");

                UpdateCourierGrid();

                WebForm_AutoFocus('<%=inputUpdateWayBillNo.ClientID%>');
            }
            else if (elem == "btnRSWEB") {
                var RSURL = "https://rsweb.tdw.co.za/rswebnet";
                //alert(RSURL);
                var RSWindow = window.open(RSURL, '_blank');
                // Puts focus on the newWindow
                if (window.focus) {
                    RSWindow.focus();
                }

            }
        } 

        function UpdateCurrentGrid() {
            $("#MainContent_btnCurrentBatch").trigger("click");
        }

        function UpdateClosedGrid() {
            $("#MainContent_btnSearchClosed").trigger("click");
        }

        function UpdateClosedGridReset() {
            $("#MainContent_btnSearchClosedReset").trigger("click");
        }

        function UpdateCourierGrid() {
            $("#MainContent_btnSearchCourier").trigger("click");
        }

        function Edit(batchNo, wayBillNo, courierName, gridToUpdate) {
            PopupCenter('BatchEdit.aspx?batchNo=' + batchNo + '&wayBillNo=' + wayBillNo + '&courierName=' + courierName + '&grd=' + gridToUpdate, 'Edit Batch', '400', '300', '');
            //UpdateCourierGrid();
        }

        function completeCourierDetails() {
            PopupCenter('BatchEdit.aspx?bc=y', 'Edit Batch', '400', '400', '');
        }

        function Cover(batchNo) {
            var newWindow = window.open('BatchCover.aspx?batchNo=' + batchNo, 'View Batch', '_blank');
            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }

        function openCourierSignPage() {
            var newWindow = window.open('TransportCover.aspx', '_blank');
            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }

        function PopupCenter(url, title, w, h, tab) {
            // Fixes dual-screen position                         Most browsers      Firefox
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;

            width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left + ',' + tab);
            //toolbar=no, menubar=no,
            // Puts focus on the newWindow location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=yes
            if (window.focus) {
                newWindow.focus();
            }
        }
    </script>

    <script type="text/javascript">
        function BulkEdit(batchNo, gridToUpdate) {
            //alert("ok:" + batchNo);
            var courierName = document.getElementById("<%=inputUpdateCourierName.ClientID%>").value;
            var wayBillNo = document.getElementById("<%=inputUpdateWayBillNo.ClientID%>").value;

            if ((courierName == null) || (courierName == "")) {
                alert("Please fill in the Batch Order number and the courier name.");
            }
            else {
                if ((wayBillNo == null) || (wayBillNo == "")) {
                    alert("Please fill in the Batch Order number and the courier name.");
                }
                else {
                    //alert('BatchEditBulk.aspx?batchNo=' + batchNo + '&wayBillNo=' + wayBillNo + '&courierName=' + courierName + '&grd=' + gridToUpdate);
                    PopupCenter('BatchEditBulk.aspx?batchNo=' + batchNo + '&wayBillNo=' + wayBillNo + '&courierName=' + courierName + '&grd=' + gridToUpdate, 'Edit Batch', '400', '400', '');
                }
            }
        }
        function RepopValues() {
<%--            document.getElementById("<%=inputUpdateCourierName.ClientID%>").value = session["courier"]
            document.getElementById("<%=inputUpdateWayBillNo.ClientID%>").value = session["workorder"];--%>
            // document.getElementById("txtUpdateCourierName").value = session["courier"];
            // document.getElementById("txtUpdateWayBillNo").value = session["workorder"];
        }

        function ListBulkChangeBatchNos() {
            document.getElementById("<%=hfBulkChangeBatchNos.ClientID%>").value = "";
            var rows = document.getElementById("<%=courierBatchGridView.ClientID%>").getElementsByTagName("tr");
            var batchNoList = "";
            for (var i = 1; i < rows.length; i++) //Exclude first row
            {
                var cbx = rows[i].getElementsByTagName("input")[0]; //First INPUT in each row is checkbox
                if (cbx != null && cbx.checked == true) {
                    batchNoList += rows[i].getElementsByTagName("td")[1].innerText + ","; //Add second column value to list
                }
            }

            if (batchNoList.length > 0) {
                batchNoList = batchNoList.substring(0, batchNoList.length - 1);
                document.getElementById("<%=hfBulkChangeBatchNos.ClientID%>").value = batchNoList;
            }
        }
    </script>

    <div class="btn-group" role="group" aria-label="...">
        <button id="btnCurrent" type="button" class="btn btn-default btn-primary" onclick="hideShowDiv(this.id);">Current</button>
        <button id="btnClosed" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">Closed</button>
        <button id="btnRSWEB" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">RSWeb</button>
        <button id="btnCourier" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">Submitted</button><%--<a style="text-decoration: underline" href="javascript:Edit('<%# Eval("BATCH_NO") %>','<%# Eval("WAYBILL_NO") %>','<%# Eval("COURIER_NAME") %>', 'CURRENT');">Edit</a>--%>
    </div>
    <br />
    <%--Current--%>
    <%--====================================================================================--%>
    <div id="tab-current" >
        <div style="display: none">
            <asp:Button ID="btnCurrentBatch" runat="server" OnClick="btnCurrentBatch_Click" Text="Current Batch" />
        </div>
        <asp:UpdatePanel ID="updPnlCurrent" runat="server" EnableViewState="False" ChildrenAsTriggers="True" UpdateMode="Conditional">
            <ContentTemplate>
                <!--ERROR DIV -->
                <div class="row" id="divCurrentError" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="lblCurrentError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                    </div>
                </div>
                <!--SUCCESS DIV -->
                <div class="row" id="divSuccess" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="lblSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                    </div>
                </div>
                <section class="contact">
                    <asp:GridView ID="currentBatchGridView" runat="server" CssClass="gridView" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" AllowPaging="True" PageSize="25" ForeColor="#333333" GridLines="None" OnPageIndexChanging="currentBatchGridView_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField HeaderText="Batch No" DataField="BATCH_NO"></asp:BoundField>
                            <asp:BoundField HeaderText="Office Id" DataField="OFFICE_NAME" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_NAME" HeaderText="Updated By"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_BY" HeaderText="Updated By id" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_DATE" HeaderText="Last Updated Date"></asp:BoundField>
                            <asp:BoundField DataField="BATCH_STATUS" HeaderText="Status" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="BATCH_COMMENT" HeaderText="Batch Comment" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_NO" HeaderText="TDW Batch Order No" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_DATE" HeaderText="Batch Order Date" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="COURIER_NAME" HeaderText="Courier Name" Visible="false"></asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="left-padding-h">
                                <ItemTemplate>
                                    <%--<a style="text-decoration: underline" href="javascript:Edit('<%# Eval("BATCH_NO") %>','<%# Eval("WAYBILL_NO") %>','<%# Eval("COURIER_NAME") %>', 'CURRENT');">Edit</a>--%>
                                    <a style="text-decoration: underline" href="BatchCover.aspx?batchNo=<%# Eval("BATCH_NO") %>" target="_blank">View</a>
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
                <asp:AsyncPostBackTrigger ControlID="btnCurrentBatch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%--cClosed--%>
    <%--====================================================================================--%>
    <div id="tab-closed" style="display: none">
        <asp:UpdatePanel ID="updPnlClosed" runat="server" EnableViewState="False" ChildrenAsTriggers="True" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: none">
                    <asp:Button ID="btnSearchClosed" runat="server" CssClass="btn btn-primary active" Text="Search" OnClick="btnSearchClosed_Click" />
                    <asp:Button ID="btnSearchClosedReset" runat="server" CssClass="btn btn-primary active" Text="Search" OnClick="btnSearchClosedReset_Click" />
                    <asp:TextBox ID="txtSearchBarcode" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearchBatchNo" runat="server" Text="Search" OnClick="btnSearchBatchNo_Click" />
                </div>
                <!--ERROR DIV -->
                <div class="row" id="divClosedError" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="lblClosedError" runat="server" EnableViewState="true" CssClass="error"></asp:Label>
                    </div>
                </div>
                <!--SUCCESS DIV -->
                <div class="row" id="divClosedSuccess" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="lblClosedSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                    </div>
                </div>
                <section class="contact">
                    <asp:GridView ID="closedBatchGridView" runat="server" CssClass="gridView" SelectMethod="FindClosedBatch" DataKeyNames="BATCH_NO,COURIER_NAME,WAYBILL_NO" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" AllowPaging="True" PageSize="25" ForeColor="#333333" GridLines="None" OnPageIndexChanging="closedBatchGridView_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Submit"><%--Transport--%>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" CssClass="left-padding-h" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No"></asp:BoundField>
                            <asp:BoundField DataField="OFFICE_NAME" HeaderText="Office Id" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_NAME" HeaderText="Updated By"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_BY" HeaderText="Updated By id" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_DATE" HeaderText="Last Updated Date"></asp:BoundField>
                            <asp:BoundField DataField="BATCH_STATUS" HeaderText="Status" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="BATCH_COMMENT" HeaderText="Batch Comment" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_NO" HeaderText="TDW Batch Order No" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_DATE" HeaderText="Batch Order Date" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="COURIER_NAME" HeaderText="Courier Name" Visible="false"></asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="left-padding-h">
                                <ItemTemplate>
                                    <%--<a style="text-decoration: underline" href="javascript:Edit('<%# Eval("BATCH_NO") %>','<%# Eval("WAYBILL_NO") %>','<%# Eval("COURIER_NAME") %>', 'CLOSED');">Edit</a>--%>
                                    <a style="text-decoration: underline" href="BatchCover.aspx?batchNo=<%# Eval("BATCH_NO") %>" target="_blank">View</a>
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
                    <asp:Label ID="lblClosedSelected" runat="server" EnableViewState="false" Style="line-height: 35px; padding-right: 20px"></asp:Label>
                    <asp:Button ID="btnDispatchCourier" runat="server" Text="Request Collection" Visible="false" OnClick="btnDispatchCourier_Click" CssClass="btn btn-primary active pull-right" Style="margin-right: 0px !important" /><%--Transport--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--Transport courier submitted--%>
    <%--====================================================================================--%>
    <div id="tab-courier" style="display: none">
        <div style="display: none">
            <asp:Button ID="btnSearchCourier" runat="server" CssClass="btn btn-primary active" Text="Search" OnClick="btnSearchCourier_Click" />
        </div>
        <table id="sessionvars" style="width: 100%;">
            <%--display: none; --%>
            <tr>
                <td colspan="5">In order to change the <strong>Batch Order</strong> and <strong>Courier Name</strong> for multiple batches, fill in these two fields and click on the <strong>Update Selected Batches</strong> button.</td>
            </tr>
            <tr>
                <td colspan="5">
                    <div style="text-align: center;"></div>
                    <!--ERROR DIV -->
                    <div class="row" id="divBulkError" runat="server" visible="false">
                        <div class="form-group col-xs-12 alert alert-danger">
                            <asp:Label ID="lblBulkError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                        </div>
                    </div>
                    <!--SUCCESS DIV -->
                    <div class="row" id="divBulkSuccess" runat="server" visible="false">
                        <div class="form-group col-xs-12 alert alert-success">
                            <asp:Label ID="lblBulkSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <%--<asp:Label runat="server" class="col-sm-3 control-label text-left" Text="TDW Batch Order No:"></asp:Label>--%>
                            TDW&nbsp;Batch&nbsp;Order&nbsp;No:&nbsp;<input runat="server" type="text" value="" id="inputUpdateWayBillNo" name="inputUpdateWayBillNo" style="width: 200px" onfocus="this.select();" />
                </td>
                <td>&nbsp;</td>
                <td>
                    <%--<asp:Label runat="server" class="col-sm-3 control-label text-left" Text="Courier Name:"></asp:Label>--%>
                            Courier&nbsp;Name:&nbsp;<input runat="server" type="text" value="TDW" id="inputUpdateCourierName" name="inputUpdateCourierName" style="width: 200px" disabled="disabled" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnBulkChange" runat="server" CssClass="btn btn-primary active" Text="Update Selected Batches" OnClick="btnBulkChange_Click" OnClientClick="ListBulkChangeBatchNos();" />
                    <asp:HiddenField ID="hfBulkChangeBatchNos" runat="server" />
                </td>
            </tr>
        </table>
        <div style="visibility: hidden">
            <asp:TextBox ID="txtUpdateWayBillNo" runat="server" CssClass="form-control col-sm-3" placeholder="" Text=""></asp:TextBox>
            <asp:TextBox ID="txtUpdateCourierName" runat="server" CssClass="form-control col-sm-3" placeholder="" Text=""></asp:TextBox>
            <asp:TextBox ID="txtThisBatch" runat="server" CssClass="form-control col-sm-3" placeholder="" Text=""></asp:TextBox>
        </div>

        <div class="row" id="divTransportError" runat="server" visible="false">
            <br />
            <div class="form-group col-xs-12 alert alert-danger">
                <asp:Label ID="lblTransportError" runat="server" EnableViewState="false" CssClass="error"></asp:Label>
            </div>
        </div>
        <asp:UpdatePanel ID="updPnlCourier" runat="server" EnableViewState="False" ChildrenAsTriggers="False" UpdateMode="Conditional">
            <ContentTemplate>
                <section class="contact">
                    <asp:GridView ID="courierBatchGridView" runat="server" 
                        EnableViewState="true" 
                        CssClass="gridView" 
                        ShowHeaderWhenEmpty="True" 
                        AutoGenerateColumns="False" 
                        AllowSorting="True" 
                        CellPadding="4" 
                        AllowPaging="True" PageSize="25" ForeColor="#333333" GridLines="Both" OnPageIndexChanging="courierBatchGridView_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <%--<asp:TemplateField HeaderText="Bulk Change" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="left-padding-h">
                                <ItemTemplate>
                                    <a style="text-decoration: underline" href="javascript:BulkEdit('<%# Eval("BATCH_NO") %>', 'SUBMITTED');">Bulk Change</a>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="left-padding-h">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="cbxBulkChange" Checked="true" />
                                    <%--<asp:HiddenField runat="server" ID="hfBatchNo" Value='<%# Eval("BATCH_NO") %>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No"></asp:BoundField>
                            <asp:BoundField DataField="OFFICE_NAME" HeaderText="Office Id" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_NAME" HeaderText="Updated By"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_BY" HeaderText="Updated By id" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="UPDATED_DATE" HeaderText="Last Updated Date"></asp:BoundField>
                            <asp:BoundField DataField="BATCH_STATUS" HeaderText="Status" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="BATCH_COMMENT" HeaderText="Batch Comment" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_NO" HeaderText="TDW Batch Order No"></asp:BoundField>
                            <asp:BoundField DataField="WAYBILL_DATE" HeaderText="Batch Order Date" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="COURIER_NAME" HeaderText="Courier Name"></asp:BoundField>
                            <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="left-padding-h">
                                <ItemTemplate>
                                    <a style="text-decoration: underline" href="javascript:Edit('<%# Eval("BATCH_NO") %>','<%# Eval("WAYBILL_NO") %>','<%# Eval("COURIER_NAME") %>', 'SUBMITTED');">Edit</a>
                                    <a style="text-decoration: underline" href="BatchCover.aspx?batchNo=<%# Eval("BATCH_NO") %>" target="_blank">View</a>
                                </ItemTemplate>
                                <HeaderStyle CssClass="left-padding-h" />
                                <ItemStyle HorizontalAlign="Center" />
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
                <asp:AsyncPostBackTrigger ControlID="btnSearchCourier" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnBulkChange" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>