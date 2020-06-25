<%@ Page Title="" 
 Language="C#" 
 AutoEventWireup="true" 
 CodeBehind="PickList.aspx.cs" 
 Inherits="SASSADirectCapture.Views.PickList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pick List</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script src="../Scripts/jquery-2.1.4.min.js"></script>

    <style>
        .noteArea {
            background-color: #E5E5E5;
            padding-top: 10px;
            padding-bottom: 10px;
            padding-left: 10px;
            padding-right: 10px;
            border-radius: 10px;
        }

        .subjectArea {
            padding-top: 10px;
            padding-bottom: 10px;
            padding-left: 10px;
            padding-right: 10px;
            border-radius: 20px;
            border-color: #E5E5E5;
            border-style: solid;
            border-width: thin;
        }
    </style>
    <script type="text/javascript">

        function PrintPage()
        {
            document.getElementById("Buttonz").style.display = "none";
            window.print();
            document.getElementById("P").value= "Reprint";
            document.getElementById("Buttonz").style.display = "";

           // window.close();
        }
    </script>
    
</head>
<body id="body" style="min-height: 650px">
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $('.divDateTime').html(printDate());

                DoJobs();
            });

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
    <form id="form1" runat="server" style="background-color: #eeeeef; min-height: 650px;">
        <asp:ScriptManager runat="server" EnablePageMethods="True">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=272931&clcid=0x409 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="jquery.ui.combined" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="False" width="99%" style="height: 100%; width: 100%;">
                <ContentTemplate>
                    <section class="contact">
                        <table style="width:100%">
                        <tr style="vertical-align:bottom;">
                            <td style="width:50px;">Date:</td>
                            <td style="width:200px;"><div class="divDateTime"></div></td>
                            <td style="width:50px;">Name:</td>
                            <td style="border-bottom:solid; border-bottom-color:black; border-bottom-width:1px; width:300px;"></td>                        
                            <td>
                                <div id="Buttonz">
                                    <input type="button" id="P" class="btn btn-primary active pull-right" value="Print"onclick="PrintPage();" />
                                    <input type="button" id="C" class="btn btn-primary active pull-right" value="Close" onclick="window.close();" />
                                </div>
                            </td>
                        </tr>
                        </table>
                   
                        <asp:GridView ID="fileGridView" CssClass="gridView" runat="server" ShowHeaderWhenEmpty="True" 
                            AutoGenerateColumns="False" 
                            AllowSorting="True" 
                            PageSize="55" 
                            CellPadding="4" 
                            ForeColor="#333333" 
                            GridLines="Both"
                            AllowPaging="True" 
                            BorderColor="#666666" 
                            BorderStyle="Solid" 
                            BorderWidth="1px" 
                            EditRowStyle-BorderStyle="Dashed" 
                            AlternatingRowStyle-BorderStyle="Solid" 
                            AlternatingRowStyle-BorderColor="Gray" 
                            AlternatingRowStyle-BorderWidth="1" 
                            HeaderStyle-BackColor="Silver" 
                            HeaderStyle-BorderStyle="Solid" >
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Found">
                                    <ItemTemplate>
                                        <img src="../Content/images/tickboxbig.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PICKLIST_NO" HeaderText="Picklist"></asp:BoundField>                                
                                <asp:BoundField DataField="PICKLIST_TYPE" HeaderText="Urgency"></asp:BoundField>
                                <asp:BoundField DataField="REQUESTED_DATE" HeaderText="Date_Requested"></asp:BoundField>
                                <asp:BoundField DataField="REQUESTED_OFFICE_NAME" HeaderText="Office_Name"></asp:BoundField>
                                <asp:BoundField DataField="ID_NO" HeaderText="ID_No"></asp:BoundField>
                                <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM_No"></asp:BoundField>
                                <asp:BoundField DataField="MIS_FILE_NO" HeaderText="MIS_No"></asp:BoundField>
                                <asp:BoundField DataField="BIN_ID" HeaderText="Bin"></asp:BoundField>
                                <asp:BoundField DataField="BOX_NUMBER" HeaderText="Box" HeaderStyle-BorderStyle="Solid"></asp:BoundField>
                                <asp:BoundField DataField="POSITION" HeaderText="Position"></asp:BoundField>
                                <asp:BoundField DataField="NAME" HeaderText="Name"></asp:BoundField>
                                <asp:BoundField DataField="SURNAME" HeaderText="Surname"></asp:BoundField>
                                <asp:BoundField DataField="CATEGORY_DESCR" HeaderText="Category"></asp:BoundField>
                                <asp:BoundField DataField="TYPE_DESCR" HeaderText="Type"></asp:BoundField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
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
        </form>
    </body>
</html>

