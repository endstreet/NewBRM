<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileCoverBulk.aspx.cs" Inherits="SASSADirectCapture.Views.FileCoverBulk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../Content/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <script src="../Scripts/JsBarcode.all.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/my_ecm.js"></script>
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js"></script>--%>
    <script type="text/javascript">

        $(document).ready(function () {
            // for all ID barcodes on the page

            var CollectionIDS = $(".barcodeID");
            CollectionIDS.each(function () {
                if ($(this).attr("alt") != "") {
                    $(this).JsBarcode($(this).attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
                }
            });

            // for all CLM barcodes on the page

            var CollectionCLM = $(".barcodeCLM");
            CollectionCLM.each(function () {
                if ($(this).attr("alt") != "") {
                    $(this).JsBarcode($(this).attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
                }
            });

            // for all BRM barcodes on the page

            var CollectionBRM = $(".barcodeBRM");
            CollectionBRM.each(function () {
                if ($(this).attr("alt") != "") {
                    $(this).JsBarcode($(this).attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
                }
            });

           <%-- if ($("#barcodeID").attr("alt") != "") {
                $("#divIDNoBarcode").JsBarcode($("#divIDNoBarcode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
                // get_object("divIDNoBarcode").innerHTML = DrawHTMLBarcode_Code39(get_object("divIDNoBarcode").innerHTML, 0, "yes", "in", 0, 2, 0.5, 2, "bottom", "center", "", "black", "white");
            }

            if ($("#divFileUnqBarcode").attr("alt") != "") {
                $("#divFileUnqBarcode").JsBarcode($("#divFileUnqBarcode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
                //get_object("divFileUnqBarcode").innerHTML = DrawHTMLBarcode_Code39(get_object("divFileUnqBarcode").innerHTML, 0, "yes", "in", 0, 2, 0.5, 2, "bottom", "center", "", "black", "white");
            }

            var mybrmBC = '<%= Session["BRM"] %>';
            document.getElementById('txtBRM').innerText = mybrmBC.toUpperCase();
            document.getElementById('divBRMBarCode').alt = mybrmBC.toUpperCase();

            if ($("#divBRMBarCode").attr("alt") != "")
            {
                $("#divBRMBarCode").JsBarcode($("#divBRMBarCode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
            }--%>
            // document.getElementById('divDateTime').innerHTML = printDate() + "<br />" + document.getElementById('divDateTime').innerHTML;
        });

        function printpage(isnew) {
            var printButton = document.getElementById('<%= btnPrint.ClientID %>');

            if (printButton != null && isnew == 'Y') {
                printButton.style.display = "none";
            }
            window.print();
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

    <style>
        @page {
            size: A4; /* auto is the initial value */
            margin: 10mm 10mm 10mm 10mm; /* this affects the margin in the printer settings */
        }

        @media print {
            .btn, #divError, #divSuccess {
                display: none;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="background-color: #eeeeef">
        <asp:ListView ID="ListView1" runat="server" OnItemDataBound="ListView1_ItemDataBound">
            <ItemTemplate>
                <div id="whiteblock" style="max-width: 760px; width: 760px; display: block; margin: auto; background-color: #ffffff">
                    <div class="container row" style="height: 100%; display: block;">
                        <table width="100%" style="margin-top: 0px;">
                            <tr>
                                <td style="vertical-align: top" class="auto-style7">
                                    <img alt="" src="../Content/images/sassa_logoSmall.jpg" width="200" />
                                </td>
                                <td>
                                    <label class="h2">
                                        <div class="text-center">File Coversheet</div>
                                    </label>
                                    <br />
                                    <div class="container row" style="text-align: center">
                                        <table>
                                            <tr>
                                                <td style="width: 150px;">
                                                    <div runat="server" id="div2" class="text-center">CLM Number:</div>
                                                    <%-- style="padding-left: 20px"--%>
                                                    <asp:Label ID="Label3" runat="server" Text='<%#Eval("UNQ_FILE_NO") %>' Style="font-weight: bold" />
                                                    <%--<div runat="server" style="padding-left: 20px" id="div5" class="text-center">MIS FIle Number:</div>
                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("FILE_NUMBER") %>' style="font-weight: bold" />
                                                <br />--%>
                                                    <div runat="server" id="div5" class="text-center">BRM File Number:</div>
                                                    <%-- style="padding-left: 20px"--%>
                                                    <asp:Label ID="Label4" runat="server" Text='<%#Eval("BRM_NO") %>' Style="font-weight: bold" />
                                                    <br />
                                                    <asp:PlaceHolder ID="phQR" runat="server" />
                                                </td>
                                                <td style="width: 100px; border-style: solid; border-width: 2px; margin: 0px; padding: 0px;">
                                                    <div runat="server" id="div1" style="font-weight: bold; font-size: x-small;">Temp Box Number:</div>
                                                    <asp:Label ID="lblTempBoxNo" runat="server" Text='<%#Eval("TEMP_BOX_NO") %>' Style="font-weight: bold; font-size: xx-large;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td rowspan="7">
                                    <div style="margin-top: 0px; text-align: right;">
                                        <div>Date: <%# Eval("COVER_DATE") %></div>
                                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 14px">Approved - Main</label>
                                        <asp:CheckBox
                                            ID="chkREGTYPE_MAIN"
                                            runat="server"
                                            Enabled="false"
                                            Checked='<%#Eval("CHECK_REGTYPE_MAIN") %>' /><br />
                                        <%# Eval("ARCHIVE_YEAR") %>
                                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 14px">Rejected - Archive</label>
                                        <asp:CheckBox
                                            ID="chkREGTYPE_ARCHIVE"
                                            runat="server"
                                            Enabled="false"
                                            Checked='<%#(Eval("CHECK_REGTYPE_ARCHIVE")) %>' /><br />
                                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 14px">Loose Correspondence</label>
                                        <asp:CheckBox
                                            ID="chkREGTYPE_LC"
                                            runat="server"
                                            Enabled="true" /><br />
                                        <label style="padding-right: 10px; font-size: 14px; font-weight: bold;">STATUS:</label>
                                        <div id="divStatus1" runat="server" style="padding-left: 20px">
                                            <label class="chkboxLabel" style="padding-right: 10px; font-size: 14px">Non-Compliant</label>
                                            <asp:CheckBox
                                                ID="chkSTATUS_NON_COMPLIANT"
                                                runat="server"
                                                Enabled="false"
                                                Checked='<%#(Eval("CHECK_STATUS_NON_COMPLIANT")) %>' /><br />
                                            <label class="chkboxLabel" style="padding-right: 10px; font-size: 14px">Transfer</label>
                                            <asp:CheckBox
                                                ID="chkSTATUS_TRANSFERRED"
                                                runat="server"
                                                Enabled="false"
                                                Checked='<%#(Eval("CHECK_STATUS_TRANSFERRED"))%>' />
                                        </div>
                                        <div id="divStatus2" runat="server" style="padding-left: 20px">
                                            <label class="chkboxLabel" style="padding-right: 10px; font-size: 14px">Legal</label>
                                            <asp:CheckBox
                                                ID="chkSTATUS_LEGAL"
                                                runat="server"
                                                Enabled="false"
                                                Checked='<%#(Eval("CHECK_STATUS_LEGAL"))%>' /><br />
                                            <label class="chkboxLabel" style="padding-right: 10px; font-size: 14px">Fraud</label>
                                            <asp:CheckBox
                                                ID="chkSTATUS_FRAUD"
                                                runat="server"
                                                Enabled="false"
                                                Checked='<%#(Eval("CHECK_STATUS_FRAUD"))%>' /><br />
                                            <label class="chkboxLabel" style="padding-right: 10px; font-size: 14px">Debtors</label>
                                            <asp:CheckBox
                                                ID="chkSTATUS_DEBTORS"
                                                runat="server"
                                                Enabled="false"
                                                Checked='<%#(Eval("CHECK_STATUS_DEBTORS"))%>' />
                                        </div>
                                    </div>
                                </td>
                                <td rowspan="7">&nbsp;&nbsp;&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style7" style="padding-left: 20px;">
                                    <div class="container row">
                                        <%--APPLICANT:--%>
                                        <asp:Label ID="Label6" runat="server" Text='<%#Eval("NAME") %>' Style="padding-right: 10px; font-size: 14px" />&nbsp;<asp:Label ID="Label12" runat="server" Text='<%#Eval("SURNAME") %>' Style="padding-right: 10px; font-size: 14px" />
                                    </div>
                                </td>
                                <td style="text-align: center" rowspan="6" class="auto-style12">
                                    <asp:PlaceHolder ID="plBarCode" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style7" style="padding-left: 20px;">
                                    <div class="container row">
                                        <%--ID NUMBER:--%>
                                        <asp:Label ID="Label8" runat="server" Text='<%#Eval("ID_NUMBER") %>' Style="padding-right: 10px; font-size: 14px" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style7" style="padding-left: 20px;">
                                    <div class="container row">
                                        <%--GRANT:--%>
                                        <asp:Label ID="Label9" runat="server" Text='<%#Eval("GRANT_NAME") %>' Style="padding-right: 10px; font-size: 14px" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style7" style="padding-left: 20px;">
                                    <div class="container row">
                                        <%--REGION:--%>
                                        <asp:Label ID="Label10" runat="server" Text='<%#Eval("REGION_NAME") %>' Style="padding-right: 10px; font-size: 14px" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style7" style="padding-left: 20px;">
                                    <div class="container row">
                                        <%--ARCHIVE YEAR:--%>Archive Year:
                                        <asp:Label ID="Label5" runat="server" Text='<%#Eval("ARCHIVE_YEAR") %>' Style="padding-right: 10px; font-size: 14px" />
                                    </div>
                                </td>
                            </tr>
                            <%--<tr>
                            <td class="auto-style7" style="padding-left:20px;">
                                <div class="container row">APPLICATION DATE:
                                </div>
                            </td>
                        </tr>--%>
                        </table>
                        <table style="max-width: 750px;">
                            <%-- margin-top: 0px;--%>
                            <tr style="height: 620px;">
                                <td style="vertical-align: top; width: 750px;">
                                    <img alt="" src="../Content/images/GRANTS/<%#Eval("GRANT_TYPE") %>.png" style="max-height: 620px;" />
                                </td>
                            </tr>
                        </table>
                        <div style="height: 1px; page-break-after: always; background-color: red;">
                            <img runat="server" style="left: 670px; top: -670px; position: relative" class="barcodeID some_block" id="divIDNoBarcode" alt='<%#Eval("ID_NUMBER") %>' />
                            </br>
                        <img runat="server" style="left: 670px; top: -530px; position: relative" class="barcodeCLM some_block" alt='<%#Eval("UNQ_FILE_NO") %>' id="divFileUnqBarcode" />
                            </br>
                        <img runat="server" style="left: 670px; top: -400px; position: relative;" class="barcodeBRM some_block" alt='<%#Eval("BRM_NO") %>' id="divBRMBarCode" />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
        <br />
        <asp:Button ID="btnClose" CssClass="btn btn-primary active pull-right" runat="server" Text="Close" OnClientClick="window.close();" />
        <asp:Button ID="btnPrint" CssClass="btn btn-primary active pull-right" runat="server" Text="Print all Covers" OnClientClick="window.print();window.close();" />
        <%--OnClientClick="myECM.showPleaseWait('Just a moment...');"--%>

        <!--ERROR DIV -->
        <div class="row" id="div3" runat="server" visible="false">
            <div class="form-group col-xs-12 alert alert-danger">
                <asp:Label ID="Label1" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
            </div>
        </div>
        <!--SUCCESS DIV -->
        <div class="row" id="div4" runat="server" visible="false">
            <div class="form-group col-xs-12 alert alert-success">
                <asp:Label ID="Label2" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
