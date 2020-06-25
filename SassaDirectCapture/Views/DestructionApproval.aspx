<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DestructionApproval.aspx.cs" Inherits="SASSADirectCapture.Views.DestructionApproval" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <div class="row" id="divError" style="padding:20px;" runat="server" >
        <div class="form-group col-xs-12 alert alert-danger">
            <asp:Label ID="lblError" runat="server" EnableViewState="true" CssClass="error"></asp:Label>
        </div>
    </div>

        <table>
             <tr>
                <td>
                    <b>Destruction Year:</b>
                </td>
                <td style="padding-top: 20px;">
                    <asp:DropDownList ID="ddDestructionYears" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddDestructionYear_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>&nbsp;
                </td>
            </tr>
        </table>
        <h2>Batches ready for Approval</h2>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="False" style="height: 100%; width: 100%;">
        <ContentTemplate>
            <asp:GridView ID="GridApproval" CssClass="gridView" runat="server" AutoGenerateColumns="False" DataKeyNames="BATCH_ID" >
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="BATCH_ID" HeaderText="Batch Id" ReadOnly="True" SortExpression="BATCH_ID" />
                    <asp:BoundField DataField="EXCLUSION_YEAR" HeaderText="Year" SortExpression="DESTRUCTION_YEAR" />
                    <asp:BoundField DataField="REGION_NAME" HeaderText="Region" SortExpression="REGION" />
                    <asp:BoundField DataField="CREATED_BY" HeaderText="Created By" SortExpression="CREATED_BY" />
                    <asp:BoundField DataField="CREATED_DATE" HeaderText="Create Date" SortExpression="CREATED_DATE" />
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
       <table>
          <tr>

                <td>
                    <asp:Button ID="ApproveBatch" runat="server" Text="Approve" OnClick="btnApprove_Batch_Click" CssClass="btn btn-primary active" Width="120px"/>
                </td>
            </tr>
        </table>
    
    </div>
</asp:Content>
