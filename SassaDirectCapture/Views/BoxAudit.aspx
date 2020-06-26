<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BoxAudit.aspx.cs" Inherits="SASSADirectCapture.Views.BoxAudit" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <style>
        .checkit {
            font-size: smaller;
            font-weight: lighter;
            font-family: Arial;
            padding-right: 20px;
        }

        .noteArea {
            background-color: #E5E5E5;
            padding-top: 10px;
            padding-bottom: 10px;
            padding-left: 10px;
            padding-right: 10px;
            border-radius: 10px;
            display: inline-block;
            /*height: 80px;*/
            position: relative;
        }

        .noteArea2 {
            background-color: #E5E5E5;
            padding-top: 10px;
            padding-bottom: 10px;
            padding-left: 10px;
            padding-right: 10px;
            border-radius: 10px;
            display: contents;
            text-align: center;
            display: inline-block;
            width: 100%;
        }

        .subjectArea {
            padding-bottom: 10px;
            padding-left: 10px;
            padding-right: 10px;
            border-radius: 20px;
            border-color: #E5E5E5;
            border-style: solid;
            border-width: thin;
            background-color: #fff;
            display: inline-block;
        }

        .subjectArea2 {
            padding-bottom: 10px;
            padding-left: 10px;
            padding-right: 10px;
            border-radius: 20px;
            border-color: #E5E5E5;
            border-style: solid;
            border-width: thin;
            background-color: #fff;
            display: inline-block;
            width: 100%;
        }
    </style>
    <div class="btn-group" role="group" aria-label="...">
        <button id="btnPickBox" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">Box Inventory</button>
        <button id="btnPickList" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">Pick Lists</button>
        <button id="btnRSWEB" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">RSWeb</button>
        <button id="btnMISBox" type="button" class="btn btn-default btn-primary" onclick="hideShowDiv(this.id);">MIS Box</button>
        <button id="btnReboxing" type="button" class="btn btn-default" onclick="hideShowDiv(this.id);">Reboxing</button>
    </div>
    <br />
    <%--====================================================================================--%>
    <div id="tab-misBox">
        <%-- style="display: none"--%>
        <asp:UpdatePanel ID="updPanelMisBox" runat="server" EnableViewState="true">
            <ContentTemplate>
                <div class="row" id="divError" runat="server" visible="false" style="width: 60%; float: right;">
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="lblError" runat="server" EnableViewState="true" CssClass="error"></asp:Label>
                    </div>
                </div>
                <div class="row" id="divSuccess" runat="server" visible="false">
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="lblSuccess" runat="server" EnableViewState="true" CssClass="success"></asp:Label>
                    </div>
                </div>
                <div id="divMISBoxUpdate" runat="server" visible="true" class="subjectArea" aria-multiselectable="False" style="width:100%">

                    <asp:Label runat="server" CssClass="control-label text-left" ID="lblBoxNo"></asp:Label>
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="1">
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Content/images/crate.jpg" Style="margin: 10px; float: left" /></td>
                            <td colspan="2">
                                <h2>MIS Box Details&nbsp;</h2>
                            </td>
                            <td colspan="6">
                                <div class="noteArea">When this box is validated, click <strong>&quot;Print Bulk&quot;</strong> to produce coversheets for all files that are present.</div>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%;">
                        <tr style="vertical-align: bottom;">
                            <td style="width: 100%;" colspan="2">
                                <table>
                                    <tr>
                                        <td style="white-space: nowrap">&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" class="control-label" Text="Registry Type: "></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="ddlRegistry_Type" runat="server" CssClass="form-control" Style="width: 100px; height: 32px !important" onchange="HideShowArchYear(this);">
                                                <asp:ListItem Text="Archive" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Main" Value="M" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Special" Value="S"></asp:ListItem>
                                                <asp:ListItem Text="Destroy" Value="D"></asp:ListItem>
                                                <asp:ListItem Text="Destroyed" Value="D2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="white-space: nowrap">&nbsp;&nbsp;<asp:Label class="control-label" runat="server" Text="MIS Box No: "></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtSearchBox" CssClass="form-control" runat="server" Style="width: 100px;" Text=""></asp:TextBox>
                                        </td>
                                        <td id="TDlblSearchDistrict" runat="server" style="white-space: nowrap">&nbsp;&nbsp;<asp:Label ID="lblSearchDistrict" runat="server" class="control-label" Text="District: "></asp:Label>
                                        </td>
                                        <td id="TDddlDistrict" runat="server">
                                            <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control" Style="width: 100px; height: 32px !important" DataTextField="Key" DataValueField="Value">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="TDlblSearchArchYear" style="display: none; white-space: nowrap;">&nbsp;&nbsp;<asp:Label ID="lblSearchArchYear" class="control-label" runat="server" Text="Archive Year: "></asp:Label>
                                        </td>
                                        <td id="TDtxtSearchArchYear" style="display: none">
                                            <asp:TextBox ID="txtSearchArchYear" CssClass="form-control" runat="server" Style="width: 100px;" Text=""></asp:TextBox>
                                        </td>
                                        <td id="TDlblSearchBin" runat="server" style="white-space: nowrap">&nbsp;&nbsp;<asp:Label ID="lblSearchBin" class="control-label" runat="server" style="white-space: nowrap" Text="Bin No: "></asp:Label>
                                        </td>
                                         <td id="TDtxtSearchBin" runat="server">
                                            <asp:TextBox ID="txtSearchBin" CssClass="form-control" runat="server" Style="width: 100px;" Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 100%;">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--</table>
                    <table style="width: 100%;">--%>
                        <tr style="vertical-align: bottom;">
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSearchBox" runat="server" Text="Display" CssClass="btn btn-primary active" Visible="true" OnClick="btnSearchBox_Click" OnClientClick="myECM.showPleaseWait('Loading...');" />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Button ID="btnPrintBulk" runat="server" CssClass="btn btn-primary active" Text="Print Bulk" OnClientClick="getBox('N'); return false;" /><%-- OnClick="btnPrintBulk_Click"--%><%--this.onclick = function() { return false; };--%>
                                            <div style="display: none">
                                                <asp:Button ID="btnPrintBulk_Hidden" runat="server" OnClick="btnPrintBulk_Hidden_Click" />
                                                <asp:HiddenField ID="hfTDWBoxNo" runat="server" />
                                            </div>
                                        </td>
                                        <%-- OnClientClick="myECM.showPleaseWait('Loading...');"--%>
                                        <td style="text-align: right">
                                            <asp:Button ID="btnPrintMissing" runat="server" CssClass="btn btn-primary active" Text="Print Missing List" OnClick="btnPrintMissing_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnPrintDestroy" runat="server" CssClass="btn btn-primary active" Text="Print Destroy List" OnClick="btnPrintDestroy_Click" />
                                        </td>
                                        <td style="width: 100%;">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="float: right">
                                <table style="border-style: solid; border-color: #000000; border-width: 1px;">
                                    <%--<tr>
                                        <td colspan="2">
                                            Legend:
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td style="background-color: #55FF55">MAIN
                                        </td>
                                        <td style="background-color: #FFCC33">ARCHIVE
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #FF3333">DESTROY
                                        </td>
                                        <td style="background-color: #AAAAAA">TRANSFER
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center; background-color: #88BBFF">EXCLUSION
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td><%-- colspan="5"--%>
                                <asp:Label ID="lblResultCount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="display: none">
                    <asp:Button ID="btnCheckFile" runat="server" Text="Check File" OnClick="btnCheckFile_Click" />
                    <asp:HiddenField ID="hfCheckFile" runat="server" />
                    <asp:HiddenField ID="hfRowIndex" runat="server" />
                    <asp:Button ID="btnSetBRM" runat="server" Text="Set BRM" OnClick="btnSetBRM_Click" />
                    <asp:Button ID="btnRevertBRM" runat="server" Text="Revert BRM" OnClick="btnRevertBRM_Click" />
                    <%--<asp:Button ID="btnUncheck" runat="server" Text="Uncheck" OnClick="btnUncheck_Click" />--%>
                </div>
                <section class="contact">
                    <asp:GridView ID="boxGridView"
                        runat="server"
                        CssClass="gridView"
                        ShowHeaderWhenEmpty="True"
                        AutoGenerateColumns="False"
                        AllowSorting="True"
                        CellPadding="4"
                        ForeColor="#333333"
                        GridLines="None"
                        OnRowDataBound="boxGridView_RowDataBound">
                        <%-- PageSize="20" OnPageIndexChanging="boxGridView_PageIndexChanging" AllowPaging="True" --%>
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <%-- 0 --%>
                            <asp:BoundField DataField="ID_NUMBER" HeaderText="ID_NUMBER" />
                            <%-- 1 --%>
                            <asp:BoundField DataField="UNQ_FILE_NO" HeaderText="CLM No" />
                            <%-- 2 --%>
                            <asp:BoundField DataField="BRM_NO" HeaderText="BRM No" />
                            <%-- 3 --%>
                            <asp:BoundField DataField="BRM_NO" HeaderText="Original BRM No" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <%-- 4 --%>
                            <asp:BoundField DataField="FILE_NUMBER" HeaderText="MIS No" />
                            <%-- 5 --%>
                            <asp:BoundField DataField="NAME" HeaderText="Name" />
                            <%-- 6 --%>
                            <asp:BoundField DataField="SURNAME" HeaderText="Surname" />
                            <%-- 7 --%>
                            <asp:BoundField DataField="REGION_NAME" HeaderText="Region" />
                            <%-- 8 --%>
                            <asp:BoundField DataField="REGION_ID" HeaderText="REGION_ID" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <%-- 9 --%>
                            <asp:BoundField DataField="TRANSFER_REGION" HeaderText="Transfer Region" />
                            <%-- 10 --%>
                            <asp:BoundField DataField="GRANT_NAME" HeaderText="Grant Type" />
                            <%-- 11 --%>
                            <asp:BoundField DataField="GRANT_TYPE" HeaderText="GRANT_TYPE" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <%-- 12 --%>
                            <asp:BoundField DataField="REGISTRY_TYPE" HeaderText="Registry Type" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <%-- 13 --%>
                            <asp:BoundField DataField="ARCHIVE_YEAR" HeaderText="Archive Year" />
                            <%-- 14 --%>
                            <asp:BoundField DataField="EXCLUSIONS" HeaderText="Exclusions" />
                            <%-- 15 --%>
                            <asp:BoundField DataField="REGION_ID_FROM" HeaderText="Previous Region" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <%-- 16 --%>
                            <asp:BoundField DataField="APP_DATE" HeaderText="Application Date" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <%-- 17 --%>
                            <asp:BoundField DataField="BOX_NUMBER" HeaderText="BOX_NUMBER" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <%-- 18 --%>
                            <asp:BoundField DataField="CHILD_ID_NO" HeaderText="CHILD_ID_NO" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <%-- 19 --%>
                            <asp:TemplateField HeaderText="Checked?" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="left-padding-h">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbxFound" AutoPostBack="true" runat="server" OnCheckedChanged="cbxFound_CheckedChanged" Checked='<%# Eval("IsFound") %>' /><%-- OnClick="cbxFoundClick(this);"--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- 20 --%>
                            <asp:TemplateField HeaderText="Non-Compliant" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="left-padding-h">
                                <ItemTemplate>
                                    <asp:CheckBox
                                        ID="checkBoxNonCompliant" AutoPostBack="true" runat="server" Enabled="true" Checked='<%# Eval("IsNonCompliant") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- 21 --%>
                            <asp:BoundField DataField="PRINT_ORDER" HeaderText="PRINT_ORDER" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
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
                <asp:AsyncPostBackTrigger ControlID="btnSearchBox" EventName="Click" />
                <%-- <asp:AsyncPostBackTrigger ControlID="btnUpdateBoxComplete" EventName="Click" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%--===========================BOX Inventory=========================================================--%>
    <div id="tab-pickbox" style="display: none">
        <div style="width:1200px;min-width: 800px;" class="subjectArea" >
        <asp:UpdatePanel ID="updPnlPickBox" runat="server" EnableViewState="true" ChildrenAsTriggers="True" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="width: 100%">
                    <tr>
                        <td style="white-space: nowrap; text-align: right;">Picklist Number:</td>
                        <td>
                            <asp:TextBox ID="txtPickListNo" runat="server" Enabled="false" CssClass="form-control col-xs-2" Text="" placeholder=""></asp:TextBox>
                        </td>
                        <td style="text-align: right;">Registry Type:</td>
                        <td style="text-align: right;">
                            <asp:DropDownList
                                ID="ddlRegistryType"
                                runat="server"
                                CssClass="form-control  col-xs-2"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlRegistryType_SelectedIndexChanged">
                                <asp:ListItem Text="" Value="" Selected="True">--Select--</asp:ListItem>
                                <asp:ListItem Text="Main" Value="M"></asp:ListItem>
                                <asp:ListItem Text="Archive" Value="A"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <div id="divAY1" runat="server" visible="false" style ="text-align: right; white-space: nowrap;">&nbsp;&nbsp;Archive Year:</div>
                        </td>
                        <td>
                            <div id="divArchYear" runat="server" visible="false">
                               <asp:TextBox ID="txtArchYear" MaxLength="4" runat="server" Enabled="true"
                                    CssClass="form-control  col-xs-2" placeholder="Eg:2017..." onkeypress="return onlyNumbers();"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <asp:Button ID="btnPicklistCompleted" runat="server"
                                CssClass="btn btn-primary active" Text="Picklist Complete"
                                OnClick="btnPicklistCompleted_Click" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td style="text-align: right;white-space: nowrap;">Enter MIS Box Number:</td>
                        <td style="text-align: right;">
                            <asp:TextBox ID="txtBoxPickedNo" runat="server" CssClass="form-control  col-xs-2" placeholder="Scan Box No here..."></asp:TextBox></td>
                        <td style="text-align: right;">Bin Number:</td>
                        <td>
                            <asp:TextBox ID="txtBinNo" runat="server" CssClass="form-control  col-xs-2"  placeholder="Scan Bin No or Type here..."></asp:TextBox></td>
                        <td>
                            <asp:Button ID="btnAddtoPicklist" runat="server" CssClass="btn btn-primary active" Text="Add to Picklist" OnClick="btnAddtoPicklist_Click" Visible="true" /></td>
                    </tr>
                </table>
                <section class="contact">
                    <asp:Button ID="btnSearchPickbox" runat="server" Text="Search" OnClick="btnSearchPickbox_Click" Style="display: none" />
                    <asp:GridView ID="PickedBoxGridView"
                        runat="server"
                        CssClass="gridView"
                        SelectMethod="FindPicklistBoxes"
                        ShowHeaderWhenEmpty="True"
                        AutoGenerateColumns="False"
                        AllowSorting="True"
                        CellPadding="4"
                        AllowPaging="True"
                        PageSize="20"
                        ForeColor="#333333"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="UNQ_PICKLIST" HeaderText="Picklist No" />
                            <asp:BoundField DataField="BOX_NUMBER" HeaderText="Box no" />
                            <asp:BoundField DataField="ARCHIVE_YEAR" HeaderText="Archive Year" />
                            <asp:BoundField DataField="BIN_NUMBER" HeaderText="MIS Bin No" />
                            <asp:TemplateField HeaderText="Received" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="left-padding-h">
                                <ItemTemplate>
                                    <asp:CheckBox
                                        ID="checkBoxReceived"
                                        AutoPostBack="true"
                                        runat="server"
                                        Enabled="false"
                                        Checked='<%# Convert.ToBoolean (Eval("BOX_RECEIVED")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Completed" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="left-padding-h">
                                <ItemTemplate>
                                    <asp:CheckBox
                                        ID="checkBoxCompleted"
                                        AutoPostBack="true"
                                        runat="server"
                                        Enabled="false"
                                        Checked='<%# Convert.ToBoolean (Eval("BOX_COMPLETED")) %>' />
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
                <asp:AsyncPostBackTrigger ControlID="btnAddtoPicklist" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnPicklistCompleted" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
            </div>
    </div>
    <%--====================================================================================--%>
    <div id="tab-picklists" style="display: none">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="true" ChildrenAsTriggers="True" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row" id="div1" runat="server" visible="false">
                    <br />
                    <div class="form-group col-xs-12 alert alert-danger">
                        <asp:Label ID="Label1" runat="server" EnableViewState="true" CssClass="error"></asp:Label>
                    </div>
                </div>
                <div class="row" id="div2" runat="server" visible="false">
                    <br />
                    <div class="form-group col-xs-12 alert alert-success">
                        <asp:Label ID="Label2" runat="server" EnableViewState="true" CssClass="success"></asp:Label>
                    </div>
                </div>
                <section class="contact">
                    <asp:GridView ID="PickedListGridView"
                        runat="server" CssClass="gridView"
                        SelectMethod="FindPicklists"
                        ShowHeaderWhenEmpty="True"
                        AutoGenerateColumns="False"
                        AllowSorting="True"
                        CellPadding="4"
                        AllowPaging="True"
                        PageSize="20"
                        ForeColor="#333333"
                        GridLines="None"
                        OnPageIndexChanging="PickedListGridView_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField HeaderText="Picklist Number" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Underline="true">
                                <ItemTemplate>
                                    <a href="#" onclick="showPickList('<%# Eval("UNQ_PICKLIST") %>');" title="See which boxes were on the picklist"><%# Eval("UNQ_PICKLIST") %></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="REGION_ID" HeaderText="REGION_ID" Visible="false" />
                            <asp:BoundField DataField="REGISTRY_TYPE" HeaderText="Registry Type" />
                            <asp:BoundField DataField="PICKLIST_DATE" HeaderText="Picklist Date" Visible="true" />
                            <asp:BoundField DataField="PICKLIST_STATUS" HeaderText="Completed?" />
                            <asp:BoundField DataField="NO_BOXES" HeaderText="Number of Boxes" />
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
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%--====================================================================================--%>
    <div id="tab-rebox" style="display: none">
        <%----%>
        <asp:UpdatePanel ID="updPnlRebox" runat="server" EnableViewState="true" ChildrenAsTriggers="True" UpdateMode="Conditional">
            <ContentTemplate>
                <section class="contact">
                    <div style="width: 100%;">
                        <%-- class="container"--%>
                        <table style="width: 100%;">
                            <tr>
                                <%--<td style="vertical-align:top; width:45%; display: none;">
                                <div class="subjectArea2">
                                    <div class="pull-left" style="display: inline-block; padding: 10px; height: 110px">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/images/filescan.jpg" />
                                        <asp:Image ID="Image6" runat="server" ImageUrl="~/Content/images/arrowright.png" />
                                    </div>
                                    <h2>For each File</h2>
                                    <div class="noteArea2">
                                        <strong>Scan the Barcode</strong> of each file being reboxed.<br />
                                        Use<strong> [Unbox] </strong>buttons to remove files from incorrect boxes.
                                    </div>
                                </div>
                            </td>
                            <td style="display: none;">
                                &nbsp;
                            </td>--%>
                                <td style="vertical-align: top;"><%-- width:55%;--%>
                                    <div class="subjectArea2" style="white-space: nowrap;">
                                        <table style="width: 100%; padding: 0px; margin: 0px;">
                                            <tr>
                                                <td style="vertical-align: middle;"><%-- colspan="3"--%>
                                                    <div>
                                                        <%-- class="row"--%>
                                                        <div class="pull-left" style="display: inline-block; padding: 10px;">
                                                            <asp:Image ImageUrl="~/Content/images/scanbox.jpg" ID="Image2" runat="server" />
                                                        </div>
                                                        <h2>Boxing Details</h2>
                                                        <div>
                                                            <%--<p>--%>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label CssClass="control-label pull-left" Style="margin-top: 5px" runat="server" Text="TDW Box Number: "></asp:Label>
                                                                    </td>
                                                                    <td style="padding-bottom: 10px;">
                                                                        <asp:TextBox ID="txtBoxNo" Text="" runat="server" CssClass="form-control col-sm-4"
                                                                            Style="width: 300px; left: 0px;" Enabled="false"></asp:TextBox><%--visibility: visible; --%><%-- OnClientClick="myECM.showPleaseWait('Loading...');"--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label CssClass="control-label pull-left" Style="margin-top: 5px" runat="server" Text="Box Type: "></asp:Label>
                                                                    </td>
                                                                    <td style="padding-bottom: 10px;">
                                                                        <%--<asp:TextBox ID="txtBoxType" runat="server" CssClass="form-control col-sm-4"
                                                                        Style="width: 150px; margin-left: 35px" Enabled="false"></asp:TextBox>--%>
                                                                        <asp:DropDownList ID="ddlBoxType" runat="server" DataTextField="Key" DataValueField="Value" CssClass="form-control" Style="width: 150px; height: 32px !important" OnSelectedIndexChanged="ddlBoxType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                    </td>
                                                                    <td id="tdlblTransferTo" runat="server" style="display: none;">&nbsp;
                                                                    <asp:Label ID="lblTransferTo" runat="server" Text="Transfer To: "></asp:Label>
                                                                    </td>
                                                                    <td id="tdddlTransferTo" runat="server" style="display: none;">
                                                                        <asp:DropDownList ID="ddlTransferTo" runat="server" DataTextField="Key" DataValueField="Value" CssClass="form-control" Style="width: 150px; height: 32px !important"></asp:DropDownList>
                                                                    </td>
                                                                    <td id="tdlblReboxArchYear" runat="server" style="display: none;">&nbsp;
                                                                    <asp:Label ID="lblReboxArchYear" runat="server" Text="Archive Year: "></asp:Label>
                                                                    </td>
                                                                    <td id="tdtxtReboxArchYear" runat="server" style="display: none;">
                                                                        <asp:TextBox ID="txtReboxArchYear" Text="" runat="server" CssClass="form-control col-sm-4"
                                                                            Style="width: 150px; left: 0px;"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="width: 100%;"></td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%; padding: 0px; margin: 0px;">
                                            <tr>
                                                <td style="padding-bottom: 10px;">
                                                    <div class="noteArea2" style="width: 250px">
                                                        Put files in a different box.
                                                    <br />
                                                        <input id="newboxbutton" type="button" value="Change Box"
                                                            onclick="javascript: getBox('Y');" class="btn btn-primary active" />

                                                        <div style="display: none">
                                                            <asp:Button ID="btnSetReboxFields" runat="server" Text="Set Rebox Fields" OnClick="btnSetReboxFields_Click" />
                                                        </div>
                                                    </div>
                                                </td>
                                                <td></td>
                                                <td style="padding-bottom: 10px;">
                                                    <div class="noteArea2" style="width: 250px">
                                                        Produce Box Coversheet.
                                                    <br />
                                                        <input id="boxfullbutton" type="button" value="Box Full"
                                                            onclick="javascript: boxFull();" class="btn btn-primary active" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnBoxFullError" runat="server" Text="Box Full Error" OnClick="btnBoxFullError_Click" />
                                                            <asp:Button ID="btnBoxFull" runat="server" Text="Box Full" OnClick="btnBoxFull_Click" />
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="width: 100%;"></td>
                                            </tr>
                                            <tr>
                                                <td id="tdBRMFileToRebox" runat="server" colspan="4">
                                                    <asp:Label CssClass="control-label pull-left" Style="margin-top: 5px" runat="server" Text="BRM File Number: "></asp:Label>
                                                    <asp:TextBox ID="txtBRMFileToRebox" runat="server" CssClass="form-control col-sm-4"
                                                        Style="width: 150px; margin-left: 35px"></asp:TextBox>
                                                    <asp:Button ID="btnAddFileToBox" runat="server" Text="Add" CssClass="btn btn-primary active" OnClick="btnAddFileToBox_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    </div>
                    <br />
                    <!--ERROR DIV -->
                    <div class="row" id="divReboxError" runat="server" visible="false">
                        <div class="form-group col-xs-12 alert alert-danger">
                            <asp:Label ID="lblReboxError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                        </div>
                    </div>
                    <!--SUCCESS DIV -->
                    <div class="row" id="divReboxSuccess" runat="server" visible="false">
                        <div class="form-group col-xs-12 alert alert-success">
                            <asp:Label ID="lblReboxSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                        </div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--====================================================================================--%>

    <script type="text/javascript">
        function hideShowDiv(elem) {
            if (elem == "btnMISBox") {
                document.getElementById("tab-misBox").style.display = "";
                document.getElementById("tab-rebox").style.display = "none";
                document.getElementById("tab-pickbox").style.display = "none";
                document.getElementById("tab-picklists").style.display = "none";

                $("#btnMISBox").addClass("btn-primary");
                $("#btnReboxing").removeClass("btn-primary");
                $("#btnPickBox").removeClass("btn-primary");
                $("#btnPickList").removeClass("btn-primary");

                //GetMISboxData();
            }
            else if (elem == "btnReboxing") {
                document.getElementById("tab-misBox").style.display = "none";
                document.getElementById("tab-rebox").style.display = "";
                document.getElementById("tab-pickbox").style.display = "none";
                document.getElementById("tab-picklists").style.display = "none";

                $("#btnMISBox").removeClass("btn-primary");
                $("#btnPickBox").removeClass("btn-primary");
                $("#btnReboxing").addClass("btn-primary");
                $("#btnPickList").removeClass("btn-primary");

                //Reload the grid with rebox data
                //GetReboxData();
            }
            else if (elem == "btnPickBox") {
                document.getElementById("tab-misBox").style.display = "none";
                document.getElementById("tab-rebox").style.display = "none";
                document.getElementById("tab-pickbox").style.display = "";
                document.getElementById("tab-picklists").style.display = "none";

                $("#btnPickBox").addClass("btn-primary");
                $("#btnPickList").removeClass("btn-primary");
                $("#btnReboxing").removeClass("btn-primary");
                $("#btnMISBox").removeClass("btn-primary");

                GetBoxesPickedData();
            }
            else if (elem == "btnPickList") {
                document.getElementById("tab-misBox").style.display = "none";
                document.getElementById("tab-rebox").style.display = "none";
                document.getElementById("tab-pickbox").style.display = "none";
                document.getElementById("tab-picklists").style.display = "";

                $("#btnPickList").addClass("btn-primary");
                $("#btnPickBox").removeClass("btn-primary");
                $("#btnReboxing").removeClass("btn-primary");
                $("#btnMISBox").removeClass("btn-primary");
            }
            else if (elem == "btnRSWEB") {
                var RSURL = "http://rsweb.tdw.co.za/rswebnet";
                //alert(RSURL);
                var RSWindow = window.open(RSURL, '_blank');
                // Puts focus on the newWindow
                if (window.focus) {
                    RSWindow.focus();
                }

            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var pressed = false;
            var chars = [];
            $(window).keypress(function (e) {
                //
                if (e.which === 13) {
                    console.log("Prevent form submit.");
                    e.preventDefault();
                }
                else {
                    chars.push(String.fromCharCode(e.which));
                }
                console.log(e.which + ":" + chars.join("|"));
                if (pressed == false) {
                    setTimeout(function () {
                        if (chars.length >= 3) {
                            var barcode = chars.join("");

                            if ($('#btnMISBox').hasClass("btn-primary")) {
                                if ($("#MainContent_txtSearchBox").is(":focus")) {
                                    $("#MainContent_txtSearchBox").val(barcode);
                                    updateGrid();
                                }
                                else if (!$("#MainContent_txtSearchArchYear").is(":focus") && !$("#MainContent_txtSearchArchYear").is(":focus")) {
                                    $("#MainContent_hfCheckFile").val(barcode);
                                    $("#MainContent_btnCheckFile").trigger("click");
                                }
                            }
                            else if ($('#btnReboxing').hasClass("btn-primary")) {
                                if ($("#MainContent_txtCLMFileToRebox").is(":focus")) {
                                    $("#MainContent_txtCLMFileToRebox").val(barcode);
                                    $("#MainContent_btnAddFileToBox").trigger("click")
                                }
                            }
                        }
                        chars = [];
                        pressed = false;
                    }, 300);
                }
                pressed = true;
                //}
            });
        });
    </script>
    <script type="text/javascript">

        function picklistInProgress(f) {
            // f is a flag with value "Y" or "N"

            var rt = document.getElementById('ddlRegistryType');
            var ay = document.getElementById('txtArchYear');

            if (f == "Y") {
                rt.Enabled = false;
                ay.Enabled = false;
            }
            else {
                rt.Enabled = true;
                ay.Enabled = true;
            }
        }
    </script>
    <script type="text/javascript">

        function showPickList(p) {
            var base_url = window.location.href.substring(0, window.location.href.toUpperCase().indexOf("/VIEWS/"));
            var myPrintURL = base_url + "/Views/BoxRequestPickList.aspx?picklist=BOX&picklistno=" + p;
            window.open(myPrintURL, "Print Box Picklist");
        }
    </script>
    <script type="text/javascript">
        function GetMISboxData() {
            $("#MainContent_btnSearchBox").trigger("click");
        }
    </script>
    <script type="text/javascript">

        function GetPickListData() {
            $("#MainContent_btnSearchPickLists").trigger("click");
        }
    </script>
    <script type="text/javascript">

        function GetBoxesPickedData() {
            $("#MainContent_btnSearchPickedBoxes").trigger("click");
        }
    </script>
    <script type="text/javascript">

        function ShowBoxPickList(p) {
            myURL = "BoxRequestPickList.aspx?picklist=BOX&picklistno=" + p;
            var newWindow = window.open(myURL, '_blank');
            window.location.reload();

        }
    </script>
    <script type="text/javascript">

        function updateGrid() {
            $("#MainContent_btnSearchBox").trigger("click");
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
            var newWindow = window.open(url, title, 'scrollbars=no, target=_blank, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left + ',' + tab);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }
    </script>
    <script type="text/javascript">

        function openBRMForm()//idNo, boxNo, transType
        {
            PopupCenter('EnterBRM.aspx?boxaudit=Y', 'Enter BRM barcode number', '650', '350', '');
        }
    </script>
    <script type="text/javascript">

        function openFileCover(p, a, box, b, t, c) {
            var myURL = 'FileCover.aspx?PensionNo=' + p + '&boxaudit=' + a + '&boxNo=' + box + '&batching=' + b + '&brmBC=' + c;
            var newWindow = window.open(myURL, '_blank');
            // Puts focus on the newWindow
            newWindow.focus();
        }
    </script>
    <script type="text/javascript">

        function sendbtn(btnid) {
            hideShowDiv(btnid);
        }
    </script>
    <script type="text/javascript">
        function getBox(isRebox) {
            var TDWBoxNo = document.getElementById('MainContent_hfTDWBoxNo').value;
            if (TDWBoxNo == '' || isRebox == 'Y') {
                PopupCenter('EnterBoxnoAudit.aspx?IsRebox=' + isRebox, 'Enter Box Barcode Number', '400', '400', '');
            }
            else {
                alert('The TDW Box Barcode for this Destroy box was captured as ' + TDWBoxNo + '.');

                PrintBulk();
            }
        }

        function PrintBulk() {
            document.getElementById("MainContent_btnPrintBulk_Hidden").click();
        }
    </script>
    <script type="text/javascript">

        function boxFull() {
            var thisBox = document.getElementById('MainContent_txtBoxNo').value;
            if (thisBox.length > 0) {
                document.getElementById('MainContent_btnBoxFull').click();
            }
            else {
                document.getElementById('MainContent_btnBoxFullError').click();
            }
        }
    </script>
    <script type="text/javascript">

        function checkRegTypeSelected() {
            var sel = document.getElementById('ddlRegistryType');
            var opt = sel.options[sel.selectedIndex];

            if (opt == 0) {
                alert('Please select a Registry Type and try again.');
                return false;
            }
        }
    </script>
    <script type="text/javascript">

        function bulkCoversForBatch(batch, box) {
            document.getElementById("MainContent_btnSearchBox").click();
            var myPrintURL = "FileCoverBulk.aspx?BatchNo=" + batch + "&BoxNo=" + box;
            var printwin = window.open(myPrintURL, "Print Bulk Cover Sheets");
            printwin.focus();
        }
    </script>
    <script type="text/javascript">

        function printBoxFiles(Status, BoxNo, RegID, RegName) {
            var myPrintURL = "BoxContents.aspx?BoxNo=" + BoxNo + "&Status=" + Status + "&RegID=" + RegID + "&RegName=" + RegName;
            var printwin = window.open(myPrintURL, "Print Box Contents - Status:" + Status);
            printwin.focus();
        }
    </script>
    <script type="text/javascript">

        function updateReboxFull() {
            $("#MainContent_txtBoxNo").val('');
            $("#MainContent_txtBoxType").val('');
            $("#MainContent_ddlBoxType").val('');
            $("#MainContent_lblTransferTo").val('');
            $("#MainContent_txtReboxArchYear").val('');
        }
    </script>
    <script type="text/javascript">

        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <script type="text/javascript">
        function HideShowArchYear(ddl) {
            if (document.getElementById(ddl.id).value == 'A') {
                document.getElementById('TDlblSearchArchYear').style.display = '';
                document.getElementById('TDtxtSearchArchYear').style.display = '';
            }
            else {
                document.getElementById('TDlblSearchArchYear').style.display = 'none';
                document.getElementById('TDtxtSearchArchYear').style.display = 'none';

            }
        }
    </script>
    <script type="text/javascript">
        function SetBRMNumber() {
            document.getElementById("MainContent_btnSetBRM").click();
        }
    </script>
    <script type="text/javascript">
        function SetReboxFields() {
            document.getElementById("MainContent_btnSetReboxFields").click();
        }
    </script>
</asp:Content>