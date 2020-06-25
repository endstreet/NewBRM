<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DestructionExclusion.aspx.cs" Inherits="SASSADirectCapture.Views.DestructionExclusion" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function ValidateID() {
        if (!(/^[0-9]{12,13}$/.test(<%=txtSearchID.ClientID%>.value))) {
            alert('A valid ID Number is 12 to 13 characters long and only contain numbers.');
        return false;
        }
                else {
                    return true;
        }
    };
    </script>
            <asp:UpdatePanel ID="updPanelDestruction" runat="server" EnableViewState="true">
            <ContentTemplate>

    <div>
    <div class="row" id="divError" style="padding:20px;" runat="server" >
        <div class="form-group col-xs-12 alert alert-danger">
            <asp:Label ID="lblError" runat="server" EnableViewState="true" CssClass="error"></asp:Label>
        </div>
    </div>
        <h1>Add Exclusion(s)</h1> <%--ID|Date|ExclusionType|Region|Person--%>
            <table style="border-collapse: collapse;">
            <tr>
                <td>
                    <b>Exclusion Type:</b>
                </td>
                <td style="padding-top: 20px;">
                    <asp:DropDownList ID="ddExclusionType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddExclusionType_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>&nbsp;
                </td>
            </tr>
            <tr>
                <td >
                    <asp:TextBox ID="txtSearchID" runat="server"  CssClass="form-control" Width="200px" placeholder="Pension no .." onfocus="this.select();"></asp:TextBox>
                </td>

                <td >
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="btn btn-primary active" Width="105px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload id=ExclusionFile runat="server" />

                </td>
                <td>
                    <asp:Button id="UploadBtn" Text="Upload File" OnClick="UploadBtn_Click" runat="server" class="btn btn-primary active" Width="105px" />
                </td>
            </tr>
                <tr>
                    <td>
               <asp:RegularExpressionValidator   id="RegularExpressionValidator1" runat="server" ErrorMessage="Upload .csv only."   
                ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.csv|.CSV)$"   
                ControlToValidate="ExclusionFile" CssClass="error">  
                </asp:RegularExpressionValidator> 
                    </td>
                </tr>
        </table>
    </div>
    <br />
    <div>
        <h2>Exclusions</h2>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="False" style="height: 100%; width: 100%;">
    <ContentTemplate>
        <section class="contact">
            <asp:GridView ID="grdExclusions" CssClass="gridView" runat="server"
                ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True"
                PageSize="40" CellPadding="4" ForeColor="#333333" GridLines="None"
                OnPageIndexChanging="grdExclusions_PageIndexChanging" AllowPaging="True">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="Exclusion_Type" HeaderText="Type"></asp:BoundField>
                    <asp:BoundField DataField="Excl_Date" HeaderText="Date"></asp:BoundField>
                    <asp:BoundField DataField="ID_NO" HeaderText="Pension no"></asp:BoundField>
                    <asp:BoundField DataField="UserName" HeaderText="Captured by"></asp:BoundField>
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </section>
    </ContentTemplate>
        
    </asp:UpdatePanel>
        <table>
                        <tr>
                <td>
                    <b>Destruction Year:</b>
                </td>
                <td style="padding-top: 20px;">
                    <asp:DropDownList ID="ddDestructionYears" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddDestructionYear_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>&nbsp;
                </td>
                <td>
                    <asp:Button ID="SubmitBatch" runat="server" Text="Submit Batch" OnClick="btnSubmit_Batch_Click" CssClass="btn btn-primary active" Width="120px"/>
                 </td>
                 <td>
                    <p>Please note: Only one batch of exclusions are allowed per region per Destruction year.<br /> Only once a batch is processed can exclusions again be added for that region/year.</p>
                </td>
            </tr>
        </table>
        
    </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="UploadBtn" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>
