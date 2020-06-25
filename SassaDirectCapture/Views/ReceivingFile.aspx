<%@ Page Title="" Language="C#"
    AutoEventWireup="true"
    CodeBehind="ReceivingFile.aspx.cs"
    Inherits="SASSADirectCapture.Views.ReceivingFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Edit</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <style>
        .noteArea {
            background-color: #E5E5E5;
            padding-top: 2px;
            padding-bottom: 2px;
            padding-left: 2px;
            padding-right: 2px;
            border-radius: 10px;
        }

        .subjectArea {
            padding-top: 4px;
            padding-bottom: 4px;
            padding-left: 4px;
            padding-right: 4px;
            border-radius: 20px;
            border-color: #E5E5E5;
            border-style: solid;
            border-width: thin;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            var pressed = false;
            var chars = [];
            $(window).keypress(function (e) {
                if (e.which === 13) {
                    console.log("Prevent form submit.");
                    e.preventDefault();
                }
                else {
                    chars.push(String.fromCharCode(e.which));
                }
                //if (e.which >= 48 && e.which <= 57) {
                //    chars.push(String.fromCharCode(e.which));
                //}
                console.log(e.which + ":" + chars.join("|"));
                if (pressed == false) {
                    setTimeout(function () {
                        if (chars.length >= 3) {
                            var barcode = chars.join("");

                            // assign value to some input (or do whatever you want)
                            $("#txtSearchBarcode").val(barcode);
                            //UpdateGrid();
                            $("#btnSearchFileNo").trigger("click");
                            //alert("Barcode Scanned: " + barcode);

                        }
                        chars = [];
                        pressed = false;
                    }, 300);
                }
                pressed = true;
            });
        });
    </script>

    <script type="text/javascript">
        function boxFull() {
            var thisBox = document.getElementById('txtBoxNo').value;
            var myPrintURL = "BoxCover.aspx?BoxNo=" + thisBox;
            var printwin = window.open(myPrintURL, "Get Box Barcode Number");
            printwin.focus();
        }
    </script>

    <script type="text/javascript">
        function Comment(fileNo, comment) {
            //window.open('ReceivingEdit.aspx?fileNo=' + fileNo + '&comment=' + escape(comment), 'Edit Batch', 'width=400, height=300');
            PopupCenter('ReceivingEdit.aspx?fileNo=' + fileNo + '&comment=' + escape(comment), 'Edit Batch', '400', '300', '');
        }
    </script>

    <script type="text/javascript">
        function Edit(fileNo, BRMno) {
            PopupCenter('FileEdit.aspx?FileNo=' + fileNo + '&brmBC=' + escape(BRMno), 'Edit BRM file number', '400', '300', '');
        }
    </script>

    <script type="text/javascript">
        function unBox(fileNo, BRMno) {
            $("#MainContent_btnUnBox").trigger("click");
            //PopupCenter('FileEdit.aspx?FileNo=' + fileNo + '&brmBC=' + escape(BRMno), 'Edit BRM file number', '400', '300', '');
        }
    </script>

    <script type="text/javascript">
        function getBox() {
            PopupCenter('EnterBoxno.aspx', 'Enter Box Barcode Number', '500', '400', '');
        }
    </script>

    <script type="text/javascript">
        function getSessionBoxNo() {
            //alert("getSessionBoxNo");
            var s = session['BoxNo'].toString();
            //alert(s);
            document.getElementById('txtBoxNo').value = s;
            //alert(sessionValue);
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
            var newWindow = window.open(url, title, 'scrollbars=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left + ',' + tab);
            //var newWindow = window.open(url, title);

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }
    </script>

    <script type="text/javascript">
        function SuccessAll() {
            //window.opener.location.reload();
            window.opener.hideShowDiv('btnReceived');
            window.close();
        }
    </script>

    <script type="text/javascript">
        function UpdateCheckBox(success, claimFileId) {
            if (success) {
                document.getElementById('divSuccess').style.display = '';
                document.getElementById('divError').style.display = 'none';
            }
            else {
                document.getElementById('divSuccess').style.display = 'none';
                document.getElementById('divError').style.display = '';
            }
        }
    </script>

    <script type="text/javascript">
        function UpdateBatch(success) {
            if (success) {
                document.getElementById('divSuccess').style.display = '';
                document.getElementById('divError').style.display = 'none';
            }
            else {
                document.getElementById('divSuccess').style.display = 'none';
                document.getElementById('divError').style.display = '';
            }
        }
    </script>

    <script type="text/javascript">
        function UpdateGrid() {
            window.location.reload();
            // window.location.reload(true);
        }
    </script>
</head>
<body id="body" style="min-height: 650px">
    <form runat="server" class="content-wrapper" style="background-color: #ffffff">
        <asp:ScriptManager runat="server" EnablePageMethods="True"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="False" ChildrenAsTriggers="True" UpdateMode="Conditional">
            <ContentTemplate>
                <section class="contact">
                    <div class="container" style="margin: auto; width: 100%;">
                        <table style="width: 100%">
                            <tr>
                                <td style="vertical-align: top; text-align: center;">
                                    <div class="subjectArea">
                                        <table style="border-style: none; border-width: 1px; padding: 0px;">
                                            <tr>
                                                <td>
                                                    <h2>Batch Receiving Details</h2>
                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Content/images/crate.jpg" />
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="Batch No: "></asp:Label></td>
                                                            <td colspan="2" class="text-left">
                                                                <asp:Label runat="server" CssClass="control-label text-left" ID="lblBatchNo"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label class="control-label" runat="server" Text="Batch Sent Date: "></asp:Label></td>
                                                            <td colspan="2" class="text-left">
                                                                <asp:Label runat="server" CssClass="control-label" ID="lblBatchSentDate"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label class="control-label" runat="server" Text="Courier Name: "></asp:Label></td>
                                                            <td colspan="2" class="text-left">
                                                                <asp:Label runat="server" CssClass="control-label" ID="lblCourierName"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label class="control-label" runat="server" Text="TDW Batch Order No: "></asp:Label></td>
                                                            <td colspan="2" class="text-left">
                                                                <asp:Label runat="server" CssClass="control-label" ID="lblWayBillNo"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label class="control-label" runat="server" Text="Batch Status: "></asp:Label></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlBatchStatus" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Received" Value="Received"></asp:ListItem>
                                                                    <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary active" Text="Update" OnClick="btnSubmit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <div class="noteArea">When all files from the batch is reboxed, select Batch Status <strong>&quot;Completed&quot;</strong> and click <strong>[Update]</strong>.</div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td style="vertical-align: top; padding: 0; display: none;">
                                    <div class="subjectArea">
                                        <table style="border-style: none; border-width: 1px; padding: 0px;">
                                            <tr>
                                                <td style="text-align: center">
                                                    <h2>For each File
                                                    </h2>
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Content/images/filescan.jpg" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Content/images/arrowright.png" /><p>&nbsp;</p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center">
                                                    <div class="noteArea">
                                                        <strong>Scan the Barcode</strong> of each file being reboxed.
                                                        <br />
                                                        Use<strong> [Unbox] </strong>buttons to remove files from incorrect boxes.
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td style="display: none;">&nbsp;</td>
                                <td style="vertical-align: top; text-align: center;">
                                    <div class="subjectArea">
                                        <table style="border-style: none; border-width: 1px; padding: 0px;">
                                            <tr>
                                                <td>
                                                    <h2 style="text-align: center">Boxing Details</h2>
                                                    <asp:Image ImageUrl="~/Content/images/scanbox.jpg" ID="Image2" runat="server" />
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="Box Barcode: "></asp:Label><asp:Label ID="labJustSaying" CssClass="control-label" runat="server" Text="" Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox
                                                                    ID="txtBoxNo"
                                                                    Text=""
                                                                    runat="server"
                                                                    CssClass="form-control col-sm-3"
                                                                    Style="visibility: visible;"
                                                                    Width="150px"
                                                                    Enabled="false">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="Registry Type: "></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox
                                                                    ID="txtBoxType"
                                                                    Text=""
                                                                    runat="server"
                                                                    CssClass="form-control col-sm-3"
                                                                    Style="visibility: visible;"
                                                                    Width="150px"
                                                                    Enabled="false">
                                                                </asp:TextBox>
                                                                <asp:HiddenField ID="hfBoxTypeID" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblAYear" CssClass="control-label" runat="server" Text="Archive Year: "></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox
                                                                    ID="txtAYear"
                                                                    Text=""
                                                                    runat="server"
                                                                    CssClass="form-control col-sm-3"
                                                                    Style="visibility: visible;"
                                                                    Width="150px"
                                                                    Enabled="false">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <input id="boxfullbutton" type="button" value="Box Full" onclick="javascript: boxFull();" class="btn btn-primary active" style="margin-bottom: 5px;" />
                                                            </td>
                                                            <td>
                                                                <input id="newboxbutton" type="button" value="Change Box" onclick="javascript: getBox();" class="btn btn-primary active" style="margin-bottom: 5px;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 50%;">
                                                                <div class="noteArea">Produce Box Inventory.</div>
                                                            </td>

                                                            <td style="width: 50%;">
                                                                <div class="noteArea">Put files in a different box.</div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <!--ERROR DIV -->
                    <div class="row" id="divError" runat="server" visible="false">
                        <div class="form-group col-xs-12 alert alert-danger">
                            <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                        </div>
                    </div>
                    <!--SUCCESS DIV -->
                    <div class="row" id="divSuccess" runat="server" style="display: none">
                        <div class="form-group col-xs-12 alert alert-success">
                            <asp:Label ID="lblSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                        </div>
                    </div>
                    <div style="display: none">
                        <asp:TextBox ID="txtSearchBarcode" runat="server"></asp:TextBox>
                        <asp:Button ID="btnSearchFileNo" runat="server" Text="Search" OnClick="btnSearchFileNo_Click" />
                    </div>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <%--$("#MainContent_btnSearchClosed").trigger("click");--%><%--<asp:Button ID="btnUnBox" runat="server" CssClass="btn btn-primary active" Text="Unbox" OnClick="btnUnBox_Click" />--%>
                    <asp:GridView ID="fileGridView" runat="server"
                        ShowHeaderWhenEmpty="True"
                        CssClass="gridView"
                        AutoGenerateColumns="False" PageSize="55" AllowSorting="True" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="batchGridView_PageIndexChanging" AllowPaging="True">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <%--0--%><asp:TemplateField HeaderText="Boxed?">
                                <ItemTemplate>
                                    <asp:Button ID="btnUnBox" runat="server" Title="Unbox" CommandArgument='<%# Eval("UNQ_FILE_NO") %>' OnCommand="btnUnBox_Click" BackColor="#c8c891" Font-Overline="False" Font-Size="X-Small" Font-Weight="Bold" Height="28px" Text="Unbox" Width="45px" />
                                    &nbsp;<asp:CheckBox
                                        fileId='<%# Eval("UNQ_FILE_NO") %>'
                                        claimFileId='<%# Eval("CLM_UNIQUE_CODE") %>'
                                        brmno='<%# Eval("BRM_BARCODE") %>'
                                        ayear='<%# Eval("ARCHIVE_YEAR") %>'
                                        ID="CheckBox1"
                                        AutoPostBack="true"
                                        runat="server"
                                        Enabled="false"
                                        OnCheckedChanged="CheckBox1_CheckedChanged"
                                        Checked='<%# Convert.ToBoolean (Eval("FILE_STATUS_COMPLETED")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--1--%><asp:BoundField DataField="CLM_UNIQUE_CODE" HeaderText="CLM Number"></asp:BoundField>
                            <%--2--%><asp:BoundField DataField="REGION_NAME" HeaderText="Region"></asp:BoundField>
                            <%--3--%><asp:BoundField DataField="FULL_NAME" HeaderText="Applicant Name"></asp:BoundField>
                            <%--4--%><asp:BoundField DataField="GRANT_TYPE_NAME" HeaderText="Grant Type"></asp:BoundField>
                            <%--5--%><asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM File No"></asp:BoundField>
                            <%--6--%><asp:BoundField DataField="TDW_BOXNO" HeaderText="TDW Box No"></asp:BoundField>
                            <%--7--%><asp:BoundField DataField="APPLICATION_STATUS" HeaderText="Registry Type"></asp:BoundField>
                            <%--8--%><asp:BoundField DataField="ARCHIVE_YEAR" HeaderText="Archive Year"></asp:BoundField>

                            <%--8 ... 9--%>
                            <asp:TemplateField HeaderText="Note">
                                <ItemTemplate>
                                    <a style="text-decoration: underline" href="javascript:Comment('<%# Eval("UNQ_FILE_NO") %>','<%# Eval("FILE_COMMENT") %>');">
                                        <asp:Image ID="Image6" runat="server" Height="16px" ImageUrl="~/Content/images/note.png" ToolTip='<%#Eval("FILE_COMMENT")%>' Width="13px" /></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--9 ... 10--%>
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <%--<a style="text-decoration: underline" href="FileCover.aspx?PensionNo=<%# Eval("UNQ_FILE_NO") %>&batching=n&trans=<%# Eval("TRANS_TYPE").ToString() %>&brmBC=<%# DataBinder.Eval(Container.DataItem, "BRM_BARCODE") != null && !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "BRM_BARCODE").ToString()) ? DataBinder.Eval(Container.DataItem, "BRM_BARCODE").ToString() : "" %>" target="_blank" title="View the File Cover Sheet">View</a>--%>
                                    <a style="text-decoration: underline" href="FileCover.aspx?PensionNo=<%# Eval("UNQ_FILE_NO") %>&batching=n&boxaudit=N&boxing=n&BRM=<%# DataBinder.Eval(Container.DataItem, "BRM_BARCODE") != null && !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "BRM_BARCODE").ToString()) ? DataBinder.Eval(Container.DataItem, "BRM_BARCODE").ToString() : "" %>&AppDate=<%# Eval("APP_DATE") %>&CLM=<%# Eval("UNQ_FILE_NO") %>&ChildID=<%# Eval("CHILD_ID_NO") %>" target="_blank" title="View the File Cover Sheet">View</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--10 ...11--%>
                            <%--<asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                     <a style="text-decoration: underline" href="javascript:Edit('<%# Eval("UNQ_FILE_NO") %>','<%# Eval("BRM_BARCODE") %>');" title="Change BRM File# / TDW Box#">Edit</a>
                               </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="Unbox">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--11 ...12--%><asp:BoundField DataField="UNQ_FILE_NO" HeaderText="Unique File No" Visible="false"></asp:BoundField>
                            <%--12 ...13--%><asp:BoundField DataField="FILE_COMMENT" HeaderText="Comment" Visible="false"></asp:BoundField>
                            <%--13 ...14--%><%--<asp:BoundField DataField="TRANS_TYPE" HeaderText="Transaction Type" Visible="false"></asp:BoundField>  --%>
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
        </asp:UpdatePanel>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
    </form>
</body>
</html>