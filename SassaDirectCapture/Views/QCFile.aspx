<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="QCFile.aspx.cs" Inherits="SASSADirectCapture.Views.QCFile" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function OpenFileCover(pensionNo, grantname, granttype, appdate, SRDNo, brmno, CLMNo, childID) {
            var myURL = 'FileCover.aspx?PensionNo=' + pensionNo + '&boxaudit=&boxNo=&batching=n&trans=&BRM=' + brmno + '&gn=' + grantname + '&gt=' + granttype + '&appdate=' + appdate + '&SRDNo=' + SRDNo + '&tempBatch=Y&CLM=' + CLMNo + '&ChildID=' + childID;
            var newWindow = window.open(myURL, '_blank');
            newWindow.focus();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--ERROR DIV -->
            <div class="row" id="divError" runat="server" visible="false">
                <br />
                <div class="form-group col-xs-12 alert alert-danger">
                    <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                </div>
            </div>
            <!--SUCCESS DIV -->
            <div class="row" id="divSuccess" runat="server" visible="false">
                <div class="form-group col-xs-12 alert alert-success">
                    <asp:Label ID="lblSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                </div>
            </div>

                <div style="display: table-row;">
                    <div style="vertical-align: middle;display: table-cell; padding:0 5px 0 5px;">
                        <asp:Label for="txtBRM" class="control-label text-left" runat="server" Text="BRM File Number :"></asp:Label>
                    </div>
                    <div style="vertical-align:bottom;display:table-cell;padding:0 5px 0 5px;">
                        <asp:TextBox ID="txtBRM" runat="server" CssClass="form-control" Width="200px" placeholder="Enter here..." OnTextChanged="txtBRM_TextChanged"></asp:TextBox>
                    </div>
                    <div style="vertical-align: middle;display: table-cell;padding:0 5px 0 5px;">
                       <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary active" />
                    </div>
                </div>
                
                <%--OnClientClick="myECM.showPleaseWait('Searching...');"--%>
            <asp:Label ID="lblMess" class="col-sm-3 control-label text-left" runat="server" Text=""></asp:Label>
            <asp:GridView ID="fileGridView" runat="server" CssClass="gridView"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="25"
                CellPadding="4" ForeColor="#333333" GridLines="None">
                <%--OnSelectedIndexChanged="fileGridView_SelectedIndexChanged"--%><%-- OnPageIndexChanging="gvResults_PageIndexChanging"--%>
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText="Compliant">
                        <ItemTemplate>
                            <asp:CheckBox fileId='<%# Eval("UNQ_FILE_NO") %>'
                                brmFileId='<%# Eval("BRM_BARCODE") %>' ID="chkSelect"
                                AutoPostBack="true" runat="server"
                                OnCheckedChanged="chkSelect_CheckedChanged"
                                Checked='<%# (Eval("NON_COMPLIANT").ToString() == "N" ? true : false) %>' />
                            <%--"Y" ? false : true--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="COMPLIANT" HeaderText="COMPLIANT"></asp:BoundField>--%>
                    <asp:TemplateField HeaderText="ID Number" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Underline="true" Visible="false">
                        <ItemTemplate>
                            <a href="FileCover.aspx?PensionNo=<%# Eval("UNQ_FILE_NO") %>&batching=n&brmBC=<%# Eval("BRM_BARCODE").ToString() %>" target="_blank">View</a>
                            <%--<a href="FileCover.aspx?PensionNo=<%# Eval("UNQ_FILE_NO") %>&batching=n&trans=<%#Eval("TRANS_TYPE")%>&brmBC=<%# Eval("BRM_BARCODE").ToString() %>" target="_blank">View</a>--%>
                        </ItemTemplate>
                        <ItemStyle Font-Underline="True" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="APPLICANT_NO " HeaderText="ID no" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="CLM_UNIQUE_CODE" HeaderText="Unique File no"></asp:BoundField>
                    <asp:BoundField DataField="REGION_NAME" HeaderText="Region"></asp:BoundField>
                    <asp:BoundField DataField="FULL_NAME" HeaderText="Applicant Name"></asp:BoundField>
                    <asp:BoundField DataField="GRANT_TYPE_NAME" HeaderText="Grant Type"></asp:BoundField>
                    <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM File no"></asp:BoundField>
                    <%--<asp:BoundField DataField="QC_USER_FN" HeaderText="Name"></asp:BoundField>
                    <asp:BoundField DataField="QC_USER_LN" HeaderText="Surname"></asp:BoundField>--%>
                    <%--<asp:BoundField DataField="NON_COMPLIANT" HeaderText="Non-Compliant?"></asp:BoundField>--%>
                    <%--<asp:BoundField DataField="TRANS_TYPE" HeaderText="Transaction Type"></asp:BoundField>--%>
                    <asp:BoundField DataField="TDW_BOXNO" HeaderText="TDW Box No"></asp:BoundField>
                    <asp:TemplateField HeaderText="File Coversheet" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Underline="true">
                        <ItemTemplate>
                            <a href="#" onclick="OpenFileCover('<%# Eval("PENSION_NO") %>','<%# Eval("GRANT_TYPE_NAME") %>','<%# Eval("GRANT_TYPE") %>','<%# Eval("AppDate") %>', '<%# Eval("SRD_NO") %>', '<%# Eval("BRM_BARCODE") %>', '<%# Eval("UNQ_FILE_NO") %>', '<%# Eval("CHILD_ID_NO") %>'); return false;" target="_blank" title="Reprint the File Cover">View</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FILE_COMMENT" HeaderText="Comment"></asp:BoundField>
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