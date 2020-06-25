<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FilePreparation.aspx.cs" Inherits="SASSADirectCapture.Views.FilePreparation" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function PopupCenter(url, title, w, h, tab) {
            // Fixes dual-screen position                         Most browsers      Firefox
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;

            width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var newWindow = window.open(url, title, 'scrollbars=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left + ',' + tab);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }

        function getBRM(pensionNo, grantname, granttype, appdate) {
            //alert("pensionNo" + pensionNo);
            //alert("grantname" + grantname);
            //alert("appdate" + appdate);
            //alert("granttype" + granttype);

            //if ((transtype == "") || (transtype == null)) {
            //    alert("Transaction type was not filled in.\n");
            //    return false;
            // }
            //else {
            //PopupCenter('EnterBRM.aspx?PensionNo=' + pensionNo + '&batching=y&trans=' + transtype, 'Enter BRM barcode number', '400', '300', '');
            //PopupCenter('EnterBRM.aspx?PensionNo=' + pensionNo + '&batching=y&trans=' + transtype + '&gt=' + granttype + '&gn=' + grantname + '&appdate=' + appdate, 'Enter BRM barcode number', '400', '300', '');

            PopupCenter('EnterBRM.aspx?PensionNo=' + pensionNo + '&batching=y&gt=' + granttype + '&gn=' + grantname + '&appdate=' + appdate, 'Enter BRM barcode number', '650', '350', '');
            // }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row" id="divError" runat="server" visible="false">
                <div class="form-group col-xs-12 alert alert-danger">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                </div>
            </div>
            <div class="form-horizontal col-sm-6">
                <asp:Label for="txtSearch" class="col-sm-3 control-label text-left" runat="server" Text="ID Number :"></asp:Label>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control col-sm-3" Width="200px" placeholder="Enter here..."></asp:TextBox>&nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="myECM.showPleaseWait('Searching...');" CssClass="btn btn-primary active" />
            </div>
            <div class="form-horizontal col-sm-6">
                <asp:Label runat="server" class="col-sm-4 control-label text-left" Text="Service Category :"></asp:Label>
                <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control" Style="width: 50%" OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="---Select Category---" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Applications" Value="Applications"></asp:ListItem>
                    <asp:ListItem Text="Customer Care" Value="Customer Care"></asp:ListItem>
                    <asp:ListItem Text="Disability Management" Value="Disability Management"></asp:ListItem>
                    <asp:ListItem Text="Payments" Value="Payments"></asp:ListItem>
                </asp:DropDownList>
                <%-- <asp:Label for="ddlTransactionType" class="col-sm-4 control-label text-left" runat="server" Text="Transaction Type :"></asp:Label>
                <asp:DropDownList ID="ddlTransactionType" DataValueField="key" DataTextField="value" CssClass="form-control" Style="width: 50%" runat="server" Enabled="false" />--%>
            </div>
            <br />
            <br />
            <br />
            <asp:GridView ID="gvResults" runat="server" CssClass="gridView" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gvResults_PageIndexChanging" PageSize="25" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText="ID Number" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Underline="true">
                        <ItemTemplate>
                            <a href="#" onclick="getBRM('<%# Eval("PENSION_NO") %>','<%# Eval("GrantType") %>','<%# Eval("GRANT_TYPE") %>','<%# Eval("AppDate") %>'); return false;" target="_blank" title="Create or Reprint the File Cover"><%# Eval("PENSION_NO") %></a>
                            <%--<a href="#" onclick="openFileCover('<%# Eval("PENSION_NO") %>'); return false;" target="_blank" title="Create or Reprint the File Cover"><%# Eval("PENSION_NO") %></a>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NAME" HeaderText="Name" />
                    <asp:BoundField DataField="SURNAME" HeaderText="Surname" />
                    <asp:BoundField DataField="Region" HeaderText="Region" />
                    <asp:BoundField DataField="GrantType" HeaderText="Grant Type" />
                    <asp:BoundField DataField="AppDate" HeaderText="Application Date" />
                    <asp:BoundField DataField="GRANT_TYPE" HeaderText="GRANT_TYPE" Visible="false" />
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
</asp:Content>