<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DestructionTDW.aspx.cs" Inherits="SASSADirectCapture.Views.DestructionTDW" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="False" style="height: 100%; width: 100%;">
  
        <ContentTemplate>
        <div class="row" id="divError" style="padding:20px;" runat="server" >
            <div class="form-group col-xs-12 alert alert-danger">
                <asp:Label ID="lblError" runat="server" EnableViewState="true" CssClass="error"></asp:Label>
            </div>
        </div>
        </ContentTemplate>
         </asp:UpdatePanel>
                <h2>Approved Batches</h2>
        <hr />
                <asp:GridView ID="GridApproved" runat="server" CssClass="gridView" AutoGenerateColumns="False" DataKeyNames="BATCH_ID" ShowHeaderWhenEmpty="true" OnRowCommand="CommandBtn_Click" >
                    <Columns>
                        <asp:BoundField DataField="BATCH_ID" HeaderText="Batch Id" ReadOnly="True" SortExpression="BATCH_ID" />
                        <asp:BoundField DataField="EXCLUSION_YEAR" HeaderText="Year" SortExpression="DESTRUCTION_YEAR" />
                        <asp:BoundField DataField="REGION_NAME" HeaderText="Region" SortExpression="REGION_ID" />
                        <asp:BoundField DataField="Approved_BY" HeaderText="Approved By" SortExpression="APPROVED_BY" />
                        <asp:BoundField DataField="APPROVED_DATE" HeaderText="Approved Date" SortExpression="APPROVED_DATE" />
                        <asp:ButtonField ButtonType="Button" CommandName="Download" Text="DownLoad" ControlStyle-CssClass="form-control" />
                         <asp:ButtonField ButtonType="Button" CommandName="Complete" Text="Completed" ControlStyle-CssClass="form-control" />
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

       </div>
    <div>
        <br />
        <h2>Upload results</h2>
        <hr />
        <table>
            <tr>
                <td>
                    <asp:FileUpload id=CompletedFile runat="server" />

                </td>
                <td>
                    <asp:Button id="UploadBtn" Text="Upload File" OnClick="UploadBtn_Click" runat="server" class="btn btn-primary active" Width="105px" />
                </td>
                <td>
                    <p style="margin-top:10px">Accepts single column .csv file with header DESTROYED or TDWNOTFOUND</p>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

