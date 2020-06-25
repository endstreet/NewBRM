<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="PrintBoxPickList.aspx.cs" Inherits="SASSADirectCapture.Views.PrintBoxPickList" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title><%: Page.Title %> - SASSA Beneficiary Records Management BOX PICKLIST</title>
    <link href="~/Content/bootstrap.css" rel='stylesheet' type='text/css' />
</head>
<body>           
    <form runat="server"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <h3 id="head1" style="text-align:center"><asp:Label ID="lblHead" runat="server"></asp:Label></h3>
            <input  id="btnPrint" type="button" style="width:200px;" class="btn-danger" value="Print" onclick="printList();" />
        </div>
<%--        <div id="tab-picklist">
        <asp:UpdatePanel ID="updPanelPicklist" runat="server" EnableViewState="false">
            <ContentTemplate>
                <asp:GridView ID="PicklistGridView" runat="server" CssClass="gridView" 
					ShowHeaderWhenEmpty="True" 
					AutoGenerateColumns="False" 
					CellPadding="2" 
					AllowPaging="True" 
					PageSize="25" 
					ForeColor="#333333" 
					GridLines="Both">
                   <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Found">
                            <ItemTemplate>
                                <img src="../Content/images/tickboxbig.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UNQ_PICKLIST" HeaderText="Picklist"></asp:BoundField>                                
                        <asp:BoundField DataField="REGION_ID" HeaderText="Region ID"></asp:BoundField>
                        <asp:BoundField DataField="REGISTRY_TYPE" HeaderText="Date_Requested"></asp:BoundField>
                        <asp:BoundField DataField="PICKLIST_DATE" HeaderText="Office_Name"></asp:BoundField>
                        <asp:BoundField DataField="PICKLIST_STATUS" HeaderText="ID_No"></asp:BoundField>
                        <asp:TemplateField HeaderText="Received?">
                                <ItemTemplate>                                   
                                 <asp:CheckBox 
                                    ID="CheckBox1" 
                                    AutoPostBack="true" 
                                    runat="server" 
                                    Enabled="false" />                            
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Completed?">
                                <ItemTemplate>                                   
                                 <asp:CheckBox 
                                    ID="CheckBox2" 
                                    AutoPostBack="true" 
                                    runat="server" 
                                    Enabled="false" />                            
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Notes" ItemStyle-Font-Underline="true">
                                <ItemTemplate>
                                    <u>&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u>
                                </ItemTemplate>
                            </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
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
        </div>--%>
        <div id="tab-boxlist">
        <asp:UpdatePanel ID="updPanelBoxlist" runat="server" EnableViewState="true">
            <ContentTemplate>
               <asp:GridView ID="BoxlistGridView" 
                    runat="server" CssClass="gridView"  
                    SelectMethod="PrintPicklistBoxes" 
                    ShowHeaderWhenEmpty="True" 
                    AutoGenerateColumns="False" 
                    AllowSorting="True" 
                    CellPadding="4" 
                    AllowPaging="True" 
                    PageSize="20" 
                    ForeColor="#333333" 
                    GridLines="None" 
                   ><%-- OnPageIndexChanging="PickedListGridView_PageIndexChanging"--%>
                    <%--
                    ////ARCHIVE_YEAR VARCHAR2(4)
                    ////BOX_COMPLETED   CHAR(1)
                    ////BOX_RECEIVED    CHAR(1)
                    ////BOX_NUMBER  VARCHAR2(10)
                    ////BIN_NUMBER  VARCHAR2(10)
                    ////UNQ_NO  VARCHAR2(20)
                    ////PICKLIST_STATUS CHAR(1)
                    ////PICKLIST_DATE   DATE
                    ////USERID  NUMBER
                    ////REGISTRY_TYPE   CHAR(1)
                    ////REGION_ID   VARCHAR2(10)
                    ////UNQ_PICKLIST    VARCHAR2(20)
                    --%>
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Found">
                            <ItemTemplate>
                                <img src="../Content/images/tickboxbig.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UNQ_PICKLIST" HeaderText="Picklist Number"></asp:BoundField>                                
                        <asp:BoundField DataField="REGION_ID" HeaderText="Region ID"></asp:BoundField>
                        <asp:BoundField DataField="REGISTRY_TYPE" HeaderText="Registry Type"></asp:BoundField>
                        <asp:BoundField DataField="PICKLIST_DATE" HeaderText="Picklist Date"></asp:BoundField>
                        <asp:BoundField DataField="PICKLIST_STATUS" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="ARCHIVE_YEAR" HeaderText="Archive Year"></asp:BoundField>
                        <asp:BoundField DataField="BIN_NUMBER" HeaderText="BIN Number"></asp:BoundField>
                        <asp:BoundField DataField="BOX_NUMBER" HeaderText="BOX Number"></asp:BoundField>
                        <asp:TemplateField HeaderText="Received?">
                                <ItemTemplate>                                   
                                 <asp:CheckBox 
                                    ID="CheckBox1" 
                                    AutoPostBack="true" 
                                    runat="server" 
                                    Checked='<%# (Eval("BOX_RECEIVED").ToString() == "Y" ? true : false) %>'
                                    Enabled="false" />                            
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Completed?">
                                <ItemTemplate>                                   
                                 <asp:CheckBox 
                                    ID="CheckBox2" 
                                    AutoPostBack="true" 
                                    runat="server" 
                                    Checked='<%# (Eval("BOX_COMPLETED").ToString() == "Y" ? true : false) %>'
                                    Enabled="false" />                            
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Notes" ItemStyle-Font-Underline="true">
                                <ItemTemplate>
                                    <u>&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u>
                                </ItemTemplate>
                            </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
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
        </form>
        <table>
                <tr>
                    <td> 
                        <br />
                        <br />
                        <br />
                        <asp:Label class="col-sm-12 control-label text-left" runat="server" ID="Label1" Text="____________________________________"></asp:Label><br />
                        <asp:Label class="col-sm-12 control-label text-left" runat="server" Text="Courier Signature"></asp:Label>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td> 
                        <br />
                        <br />
                        <br />
                        <asp:Label class="col-sm-12 control-label text-left" runat="server" ID="Label2" Text="____________________________________"></asp:Label><br />
                        <asp:Label class="col-sm-12 control-label text-left" runat="server" Text="Name and Surname"></asp:Label>
                    </td>
                     <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <br />
                        <br />
                        <br />
                         <div id="divDateTime" style="margin-right: 15px; width: 25%"><br />
                         <asp:Label class="col-sm-12 control-label text-left" runat="server" Text="Date"></asp:Label>
                         
                </div>
                    </td>
                </tr>
            </table>
    <script type="text/javascript">
        function printList() {
            document.getElementById("btnPrint").style.display = "none";
            window.print();
            window.close();
        }
   
        function hideShowDiv(elem) {

        //if (elem == "BOX") {
        //    document.getElementById("tab-boxlist").style.display = "";
        //    document.getElementById("tab-picklist").style.display = "none";
        //}
        //else {
        //    document.getElementById("tab-boxlist").style.display = "none";
        //    document.getElementById("tab-picklist").style.display = "";
        //
        }

        function printDate() {
            var temp = new Date();
            var dateStr = padStr(temp.getFullYear()) + "/" +
                          padStr(1 + temp.getMonth()) + "/" +
                          padStr(temp.getDate()) + " " +
                          padStr(temp.getHours()) + ":" +
                          padStr(temp.getMinutes()) + ":" +
                          padStr(temp.getSeconds());

            return dateStr;
        }
       </script>   
        <script type="text/javascript">
            $(document).ready(function () {
               document.getElementById('divDateTime').innerHTML = printDate();
            }); 
        </script>   
    </body>
</html>
