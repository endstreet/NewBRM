<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileCover.aspx.cs" Inherits="SASSADirectCapture.Views.FileCover" %>

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
    <%: Scripts.Render("~/bundles/BRMBundle") %>

    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#divIDNoBarcode").attr("alt") != "") {
                $("#divIDNoBarcode").JsBarcode($("#divIDNoBarcode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
            }

            if ($("#divFileUnqBarcode").attr("alt") != "") {
                $("#divFileUnqBarcode").JsBarcode($("#divFileUnqBarcode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
            }

            if ($("#divBRMBarCode").attr("alt") != "") {
                $("#divBRMBarCode").JsBarcode($("#divBRMBarCode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
            }

            document.getElementById('divDateTime').innerHTML = printDate() + "<br />" + document.getElementById('divDateTime').innerHTML;

            if ($('#tblDocGeneral > tbody > tr').length == 0){
                $('#generalParticularsFieldset').css('display','none');
            }

            if ($('#tblDocIncome > tbody > tr').length == 0){
                $('#particularsOfIncomeFieldset').css('display','none');
            }

            if ($('#tblDocAssets > tbody > tr').length == 0){
                $('#particularsOfAssetsFieldset').css('display','none');
            }
        });

        function printpage(isnew) {
            var printButton = document.getElementById('<%= btnPrint.ClientID %>');
            var reprintButton = document.getElementById('<%= btnReprint.ClientID %>');

            if (printButton != null && isnew == 'Y') {
                printButton.style.display = "none";
            }
            if (reprintButton != null && isnew == 'Y') {
                reprintButton.style.display = "none";
            }
            window.print();
            reprintButton.style.display = "";
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

        function checkBoxCheck(elem) {
            switch (elem) {
                case "checkApprove":
                    if (document.getElementById(elem).checked) {
                        document.getElementById("checkReject").checked = false;
                        document.getElementById("checkLC").checked = false;
                    } break;
                case "checkReject":
                    if (document.getElementById(elem).checked) {
                        document.getElementById("checkApprove").checked = false;
                        document.getElementById("checkLC").checked = false;
                    } break
                case "checkLC":
                    if (document.getElementById(elem).checked) {
                    } break;
            }

        }

        function refocusWindowClose() {
            BRM_Utilities.refocusWindowClose();
        }
    </script>
    <script type="text/javascript">
        function popBC() {
            if ($("#divIDNoBarcode").attr("alt") != "") {
                $("#divIDNoBarcode").JsBarcode($("#divIDNoBarcode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
            }

            if ($("#divFileUnqBarcode").attr("alt") != "") {
                $("#divFileUnqBarcode").JsBarcode($("#divFileUnqBarcode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
            }

            var mybrmBC = '<%= Session["BRM"] %>';
            document.getElementById('txtBRM').innerText = mybrmBC.toUpperCase();
            document.getElementById('divBRMBarCode').alt = mybrmBC.toUpperCase();

            if ($("#divBRMBarCode").attr("alt") != "") {
                $("#divBRMBarCode").JsBarcode($("#divBRMBarCode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
            }
        }
    </script>

    <style>
        @media print {
            .btn, #divError, #divSuccess {
                display: none;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="background-color: #eeeeef">
        <div id="whiteblock" style="max-width: 760px; width: 760px; display: block; margin: auto; background-color: #ffffff">
            <!--HEADING DIV -->
            <div class="container row" style="height: 100%; display: block;">
                <div style="width: 30%; float: left; display: inline-block">
                    <img alt="" src="../Content/images/sassa_logoSmall.jpg" width="200" />
                </div>
                <div style="width: 40%; display: inline-block; height: 100px; text-align: center; padding-top: 10px;">
                    <label class="h2">File Coversheet</label>
                    <div class="container row">
                        <asp:HiddenField ID="hiddenBarcode" Value="" runat="server" />
                        <div runat="server" style="padding-left: 20px" id="div1">BRM File Number:</div>
                        <div runat="server" id="txtBRM" style="font-weight: bold"></div>
                        <div runat="server" style="padding-left: 20px" id="div2" class="text-center">CLM Number:</div>
                        <div runat="server" id="txtCLM" style="font-weight: bold"></div>
                        <asp:PlaceHolder ID="plBarCode" runat="server" />
                        <br />
                        <br />
                    </div>
                    <asp:HiddenField ID="txtProcess" runat="server" Value="" />
                </div>
                <div id="divDateTime">
                    <div>
                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 16px">Approved - Main</label><input type="checkbox" runat="server" style="margin-right: 10px" id="checkApprove" disabled="disabled" /><br />
                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 16px">Rejected - Archive</label><input type="checkbox" runat="server" style="margin-right: 10px" id="checkReject" disabled="disabled" /><br />
                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 16px">Loose Correspondence</label><input type="checkbox" runat="server" style="margin-right: 10px" id="checkLC" disabled="disabled" /><br />
                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 16px">Review</label><input type="checkbox" runat="server" style="margin-right: 10px" id="checkReview" disabled="disabled" /><br />
                    </div>
                    <br />
                </div>
            </div>
            <!--ERROR DIV -->
            <div class="row" id="divError" runat="server" visible="false">
                <div class="form-group col-xs-12 alert alert-danger">
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False" CssClass="error"></asp:Label>
                </div>
            </div>
            <!--SUCCESS DIV -->
            <div class="row" id="divSuccess" runat="server" visible="false">
                <div class="form-group col-xs-12 alert alert-success">
                    <asp:Label ID="lblSuccess" runat="server" EnableViewState="False" CssClass="success"></asp:Label>
                </div>
            </div>
            <div class="container row" style="display: block; height: 100%">
                <!--MAIN DIV FOR CONTENT-->
                <div id="divCriticalDocs" runat="server" style="display: block; width: 86%; float: left">
                    <div class="container row">
                        <div runat="server" class="divWidth-6" style="padding-left: 20px" id="divUserName">User Name</div>
                        <div runat="server" class="divWidth-6" id="divSocpenRef">SocPen REF no</div>
                        <div runat="server" id="divIDContrainer">
                            <div class="divWidth-6"></div>
                            <div runat="server" class="divWidth-6" id="divIDNo"></div>
                        </div>
                    </div>
                    <div class="container row">
                        <div runat="server" class="divWidth-6" style="padding-left: 20px" id="divRegion">Region</div>
                        <div runat="server" class="divWidth-6" id="divGrantName">Grant Name</div>
                    </div>
                    <div class="container row">
                        <div runat="server" class="divWidth-6" style="padding-left: 20px" id="divTransDate"></div>
                        <div runat="server" class="divWidth-6" id="divLCType"></div>
                        <asp:HiddenField ID="hfLCType" runat="server" />
                    </div>
                    <div class="container row">
                        <div runat="server" class="divWidth-6" style="padding-left: 20px" id="divReviewDate"></div>
                        <asp:HiddenField ID="hfReviewDate" runat="server" />
                        <div class="divWidth-6">
                            <p>Archive Year: <strong runat="server" id="divArchiveYear"></strong></p>
                        </div>
                    </div>
                    <div class="container row">
                        <fieldset id="generalParticularsFieldset">
                            <legend style="display: block">General Particulars</legend>
                            <asp:Table runat="server" ID="tblDocGeneral" Style="width: 100%;"></asp:Table>
                        </fieldset>
                    </div>
                    <br />
                    <div class="container row">
                        <fieldset id="particularsOfIncomeFieldset">
                            <legend style="display: block">Particulars of Income</legend>
                            <asp:Table runat="server" ID="tblDocIncome" Style="width: 100%;"></asp:Table>
                        </fieldset>
                    </div>
                    <br />
                    <div class="container row">
                        <fieldset id="particularsOfAssetsFieldset">
                            <legend style="display: block">Particulars of Assets</legend>
                            <asp:Table runat="server" ID="tblDocAssets" Style="width: 100%;"></asp:Table>
                        </fieldset>
                    </div>
                    <div class="container row">
                        <asp:Button ID="btnClose" CssClass="btn btn-primary active pull-right" runat="server" Text="Close" OnClientClick="" OnClick="BtnClose_Click" />
                        <asp:Button ID="btnReprint" CssClass="btn btn-primary active pull-right" runat="server" Text="Reprint" OnClientClick="printpage('N')" />
                        <asp:Button ID="btnPrint" CssClass="btn btn-primary active pull-right" runat="server" Text="Save" OnClick="btnPrint_Click" OnClientClick="myECM.showPleaseWait('Just a moment...');" />
                    </div>
                </div>
                <!--RIGHT-SIDE DIV FOR BARCODES-->
                <div style="float: right; width: 10%;">
                    <img class="some_block" runat="server" id="divIDNoBarcode" />
                    <img class="some_block" style="top: 480px" runat="server" alt="PLACEHOLDER" id="divFileUnqBarcode" />
                    <img class="some_block" style="top: 700px" runat="server" alt="PLACEHOLDER" id="divBRMBarCode" />
                </div>
            </div>
            <br />
            <asp:HiddenField ID="hfIsMGMerge" runat="server" />
        </div>
    </form>
</body>
</html>