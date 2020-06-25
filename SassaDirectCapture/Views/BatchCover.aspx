<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="BatchCover.aspx.cs" Inherits="SASSADirectCapture.Views.BatchCover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Cover</title>
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <link href="../Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <script src="../Scripts/JsBarcode.all.min.js"></script>
    <%--BEGIN   FOR 2D BARCODE--%>
    <%--<script src="../Scripts/js/jquery.min.js"></script>--%>
    <script src="../Scripts/js/jquery.classyqr.min.js"></script>
    <%--END   FOR 2D BARCODE--%>
    <%--kobus--%>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/png" href="dummy.png" />
    <link rel="apple-touch-icon-precomposed" type="image/png" href="dummy.png" />
    <link href="//fonts.googleapis.com/css?family=Ubuntu:300,400,700" rel="stylesheet" />
    <link href="styles.css" rel="stylesheet" />
    <script src="jquery.min.js"></script>
    <script src="../jquery-qrcode-0.14.0.js"></script>
    <script src="../Scripts/jquery-code/scripts.js"></script>
    <%--end kobus--%>
    <style>
        @media print {
            .btn, #divError, #divSuccess, .no-print, .no-print * {
                display: none !important;
            }
        }
    </style>
</head>
<body>
    <script type="text/javascript">

        $(document).ready(function () {
            $('.divDateTime').html(printDate());
            Do2DJobs();
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

        function padStr(i) {
            return (i < 10) ? "0" + i : "" + i;
        }

        /* <![CDATA[ */
        function get_object(id) {
            var object = null;
            if (document.layers) {
                object = document.layers[id];
            } else if (document.all) {
                object = document.all[id];
            } else if (document.getElementById) {
                object = document.getElementById(id);
            }
            return object;
        }
        /* ]]> */

        // for 2D barcodes - START
        function Do2DJobs() {
            $('.2Dbar').each(function () {
                //alttext = $(this).attr('alt');
                //$(this).ClassyQR({ type: 'text', text: alttext });

                alttext = $(this).attr('alt');
                var patt = /[|]/g;
                //alert( patt );
                alttexttab = alttext.replace(patt, "\t");
                $(this).ClassyQR({ type: 'text', text: alttexttab });
            }
            )
            if ($("#barcode").attr("alt") != "") {
                $("#barcode").JsBarcode($("#barcode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
            }
            //alert("Do2DJobs 3");

        }
        // for 2D barcodes - END

        function DoJobs() {
            try {
                var cnt = <%=cBarCodeCount%>;
                if (cnt == 0) {
                    $("#barcode0").JsBarcode($("#barcode0").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
                }
                else {
                    for (var i = 1; i < <%=cBarCodeCount%>; i++) {
                        $("#barcode" + i.toString()).JsBarcode($("#barcode" + i.toString()).attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
                    }
                }

                var printedAlready = <%=cBatchPrintedAlready%>;
                if (printedAlready) {
                    //document.getElementById('PandC').style.display="";
                    document.getElementById('btnPrint').style.display = "";
                    document.getElementById('btnReprint').style.display = "none";

                }
                else {
                    //document.getElementById('PandC').style.display="none";
                    document.getElementById('btnPrint').style.display = "none";
                    document.getElementById('btnReprint').style.display = "";
                }
            }
            catch (err) {
                //document.getElementById('PandC').style.display="none";
                document.getElementById('btnPrint').style.display = "none";
                document.getElementById('btnReprint').style.display = "";
            }
        }

        function Edit(FileNo, brmBC, courierName, gridToUpdate) {
            PopupCenter('FileEdit.aspx?FileNo=' + FileNo + '&brmBC=' + brmBC, 'Edit File', '400', '300', '');
            //UpdateCourierGrid();
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

            // Puts focus on the newWindow
            if (window.focus) {
                newWindow.focus();
            }
        }
    </script>
    <div class="test" style="width: 200px"></div>
    <form id="form1" runat="server" style="background-color: #eeeeef; min-height: 650px;">
        <div style="max-width: 1024px; width: 1024px; min-height: 600px; display: block; margin: auto; background-color: #ffffff">
            <asp:ScriptManager runat="server"></asp:ScriptManager>

            <div class="container row" style="height: 100%; display: block;">
                <div style="width: 30%; float: left; display: inline-block">
                    <img alt="" src="../Content/images/sassa_logoSmall.jpg" width="200" />
                </div>
                <div style="width: 40%; display: inline-block; height: 100px; text-align: center; padding-top: 10px;">
                    <label class="h2">Batch Coversheet</label>
                </div>
                <div id="barcodecontainer" style="width: 25%; float: right; margin-right: 15px">
                    <div style="float: right">
                        <div class="divDateTime">
                        </div>
                        <img runat="server"
                            style="top: 50px; position: relative;"
                            class="barcode"
                            id="barcode" />
                    </div>
                </div>
            </div>

            <!--ERROR DIV -->
            <div class="row" id="divError" runat="server" visible="false">
                <div class="form-group col-xs-12 alert alert-danger" style="margin-bottom: 0px !important; margin-top: 15px !important">
                    <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                </div>
            </div>
            <!--SUCCESS DIV -->
            <div class="row" id="divSuccess" runat="server" visible="false">
                <div class="form-group col-xs-12 alert alert-success" style="margin-bottom: 0px !important; margin-top: 15px !important">
                    <asp:Label ID="lblSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                </div>
            </div>
            <table style="width: 100%">
                <tr>
                    <td style="text-align: right">Batch No:</td>
                    <td>
                        <asp:Literal runat="server" ID="txtBatchNo" EnableViewState="false" /></td>
                    <td style="text-align: right">TDW Batch Order No:</td>
                    <td>
                        <asp:Literal runat="server" ID="txtWaybillNo" EnableViewState="false" /></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: right">Local Office:</td>
                    <td>
                        <asp:Literal runat="server" ID="txtLocalOffice" EnableViewState="false" /></td>
                    <td style="text-align: right">Courier Name:</td>
                    <td>
                        <asp:Literal runat="server" ID="txtCourierName" EnableViewState="false" /></td>
                    <td style="text-align: right">No of Files:</td>
                    <td>
                        <asp:Literal runat="server" ID="txtNrOfApplicants" EnableViewState="false" /></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="False" width="99%" style="height: 100%; width: 100%;">
                            <ContentTemplate>
                                <section class="contact">
                                    <asp:GridView ID="batchGridView" CssClass="gridView" runat="server" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" PageSize="25" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="batchGridView_PageIndexChanging" AllowPaging="True" EnableSortingAndPagingCallbacks="True" OnRowDataBound="batchGridView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="CLM_UNIQUE_CODE" HeaderText="CLM Unique Code"></asp:BoundField>
                                            <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM File No"></asp:BoundField>
                                            <asp:BoundField DataField="APPLICANT_NO" HeaderText="ID No"></asp:BoundField>
                                            <asp:BoundField DataField="FULL_NAME" HeaderText="Name and Surname"></asp:BoundField>
                                            <asp:BoundField DataField="GRANT_TYPE_NAME" HeaderText="Grant Type"></asp:BoundField>
                                            <asp:BoundField DataField="APP_DATE" HeaderText="Application Date" Visible="false"></asp:BoundField>
                                            <asp:BoundField DataField="TRANS_DATE" HeaderText="Application Date" Visible="false"></asp:BoundField>
                                            <asp:BoundField DataField="APPLICATION_STATUS" HeaderText="Registry Type" Visible="true"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Application Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransactionDate" runat="server"
                                                        Text='<%#Eval("TRANS_DATE", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action" ItemStyle-Font-Underline="true">
                                                <ItemTemplate>
                                                    <a style="visibility: visible"
                                                        class='no-print'
                                                        id="linkView"
                                                        href='FileCover.aspx?PensionNo=<%# Eval("APPLICANT_NO") %>&CLM=<%# Eval("UNQ_FILE_NO") %>&batching=n&BRM=<%# Eval("BRM_BARCODE") %>&gn=<%# Eval("GRANT_TYPE_NAME") %>&gt=<%# Eval("GRANT_TYPE") %>&appdate=<%# Eval("APP_DATE") %>&ChildID=<%# Eval("CHILD_ID_NO") %>' target="_blank">View</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" ItemStyle-Font-Underline="true">
                                                <ItemTemplate>
                                                    <a style="visibility: visible" class='no-print' href="javascript:Edit('<%# Eval("UNQ_FILE_NO") %>','<%# Eval("BRM_BARCODE") %>');">Edit</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remove" ItemStyle-Font-Underline="true">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnRemove" runat="server" class="no-print" Text="Remove" OnClick="lbtnRemove_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Barcode" ItemStyle-Font-Underline="true">
                                                <ItemTemplate>
                                                    <asp:PlaceHolder ID="plBarCode" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="display: none"></td>
                    <td colspan="5">
                        <input type="button" id="btnCancel" class="btn btn-primary active pull-right" value="Cancel" onclick="window.close();" />
                        <asp:Button ID="btnPrint" CssClass="btn btn-primary active pull-right" runat="server" Text="Print to add to batch" OnClick="btnPrint_Click" OnClientClick="window.print(); myECM.showPleaseWait('Just a moment...');" />
                        <asp:Button ID="btnReprint" CssClass="btn btn-primary active pull-right" runat="server" Text="Reprint" OnClientClick="window.print();" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%; visibility: hidden;">
                <tr style="visibility: hidden;">
                    <td>
                        <div id="PrintDiv">
                            <%=cPrintDiv%>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>