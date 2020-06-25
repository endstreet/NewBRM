<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="TransportCover.aspx.cs" Inherits="SASSADirectCapture.Views.TransportCover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transport Cover</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <%-- <script src="../Scripts/code39.js"></script>--%>
    <script src="../Scripts/JsBarcode.all.min.js"></script>

    <style>
        @media print {
            .btn, #divError, #divSuccess {
                display: none;
            }
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {

            $('.barcode').each(function () {

                if ($(this).attr("alt") != '') {
                    $(this).JsBarcode($(this).attr("alt"), { format: "CODE128", displayValue: false, fontSize: 14, height: 50, width: 2 });
                }

                //if ($(this).html() != '') {
                //    $(this).html(DrawHTMLBarcode_Code39($(this).html(), 0, "no", "in", 0, 2, 0.4, 2, "bottom", "right", "", "black", "white"));
                //}
            });

            document.getElementById('divDateTime').innerHTML = printDate();
        });

        function printpage(isnew) {
            var printButton = document.getElementById('<%= btnPrintClose.ClientID %>');
            var reprintButton = document.getElementById('btnReprint');

            if (printButton != null) {
                printButton.style.display = "none";
            }
            if (reprintButton != null && isnew == 'Y') {
                reprintButton.style.display = "";
            }

            window.print();
            window.opener.location.reload();
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

        function padStr(i) {
            return (i < 10) ? "0" + i : "" + i;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="background-color: #eeeeef; min-height: 650px;">
        <div style="max-width: 1024px; width: 1024px; min-height: 600px; display: block; margin: auto; background-color: #ffffff">
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <div class="container row" style="height: 100%; display: block;">
                <div style="width: 30%; float: left; display: inline-block">
                    <img alt="" src="../Content/images/sassa_logoSmall.jpg" width="200" />
                </div>
                <div style="width: 40%; display: inline-block; height: 100px; text-align: center; padding-top: 10px;">
                    <label runat="server" id="lblHeading" class="h2">Transport Receipt</label>
                </div>
                <div id="divDateTime" style="margin-right: 15px; width: 25%">
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
            <div class="container">
                <br />
                <asp:GridView ID="batchGridView" runat="server" CssClass="gridView" DataKeyNames="BATCH_NO" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No"></asp:BoundField>
                        <asp:BoundField DataField="NO_OF_FILES" HeaderText="No of Files"></asp:BoundField>
                        <asp:BoundField DataField="WAYBILL_NO" HeaderText="TDW Batch Order No"></asp:BoundField>
                        <asp:BoundField DataField="COURIER_NAME" HeaderText="Courier Name"></asp:BoundField>
                        <asp:TemplateField HeaderText="Batch No - Barcode" HeaderStyle-CssClass="barcodeColumn-h">
                            <ItemTemplate>
                                <%--<div class="divBarcode" runat="server"><%# Eval("BATCH_NO") %></div>--%>
                                <img runat="server" class="barcode" alt='<%# Eval("BATCH_NO") %>' />
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
            </div>
            <br />
            <br />
            <div style="display: inline-block; position: relative; width: 50%;">
                <asp:Label Style="padding-right: 10px; padding-left: 15px;" runat="server" Text="Total number of batches:"></asp:Label>
                <asp:Label runat="server" ID="lblTotalBatches" Text=""></asp:Label>
            </div>
            <div style="display: inline-block; position: relative; width: 50%;">
                <asp:Label Style="padding-right: 33px; padding-left: 15px;" runat="server" Text="Total number of files:"></asp:Label>
                <asp:Label runat="server" ID="lblTotalFiles" Text=""></asp:Label>
            </div>
            <div style="display: inline-block; position: relative; width: 50%; text-align: left">
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
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hiddenReceiptType" runat="server" />
            <input type="button" id="btnCancel" class="btn btn-primary active pull-right" value="Cancel" onclick="window.close();" />
            <input id="btnReprint" type="button" class="btn btn-primary active pull-right" style="display: none" value="Reprint" onclick="printpage('N')" />
            <asp:Button ID="btnPrintClose" runat="server" OnClick="btnPrintClose_Click" CssClass="btn btn-primary active pull-right" Style="margin-right: 15px" Text="Print and Close" />
        </div>
    </form>
</body>
</html>