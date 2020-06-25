<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileCoverView.aspx.cs" Inherits="SASSADirectCapture.Views.FileCoverView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../Content/Site.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-2.1.4.min.js"></script>
    <%--<script src="../Scripts/code39.js"></script>--%>
    <script src="../Scripts/JsBarcode.all.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/my_ecm.js"></script>

    <script type="text/javascript">

        $(document).ready(function () 
        {

            if ($("#divIDNoBarcode").attr("alt") != "") {
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
            }
        //} 
        //else 
        //{
        //        alert('BRM Barcode not present.');
        //    }



            //  --- End added

            document.getElementById('divDateTime').innerHTML = printDate() + "<br />" + document.getElementById('divDateTime').innerHTML;
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
                        document.getElementById("checkReview").checked = false;
                    } break;
                case "checkReject":
                    if (document.getElementById(elem).checked) {
                        document.getElementById("checkApprove").checked = false;
                        document.getElementById("checkReview").checked = false;
                    } break
                case "checkReview":
                    if (document.getElementById(elem).checked) {
                        document.getElementById("checkApprove").checked = false;
                        document.getElementById("checkReject").checked = false;
                    } break;
            }

        }


    </script>
    <script type="text/javascript">
        function getBRMBarcodeAndReload() {
            //var brmBC = prompt("--- INSTRUCTIONS: ---\n\nThe BRM file number could not be established\nIt does not exist on the database for this file, nor was it entered on the previous screen.\nPlease do one of the following:\n \052 If you do have access to a scanner, please SCAN the BRM barcode;\n \052 Otherwise please TYPE in the BRM File number and press 'OK'.\n[Take note of the case - UPPER or lower],", "");
            //if ((brmBC == "") || (brmBC == null)) {
            //    alert("The barcode must be entered, you cannot proceed without it.\nPlease use either the scanner or type it in.");
            //    return false;
            //}
            //var newURL = window.location.href;
            //alert("current URL:" + newURL);
            //newURL += '&brmBC=' + brmBC;
            //alert("new URL:" + newURL);

            //var reloadWindow = window.open(newURL, '_self');

            //// Puts focus on the newWindow
            //if (window.focus) {
            //    reloadWindow .focus();
            //}
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
                    <%--<label class="h2">SASSA Application</label>--%>
                    <%--<br />--%>
                    <label class="h2">File Coversheet</label>
                    <div class="container row">
                        <asp:HiddenField ID="hiddenBarcode" Value="" runat="server" />

                        <br />
                        <div runat="server" style="padding-left: 20px" id="div1">BRM File Number:</div>
                        <div runat="server" id="txtBRM" style="font-weight: bold"></div>
                    </div>
                    <asp:HiddenField ID="txtProcess" runat="server" Value="" />
                </div>
                <div id="divDateTime">
                    <div>
                        <%--style="margin-top: 20px"--%>
                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 16px">Approved - Main</label><input type="checkbox" runat="server" style="margin-right: 10px" id="checkApprove" onclick="checkBoxCheck(this.id);" /><br />
                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 16px">Rejected - Archive</label><input type="checkbox" runat="server" style="margin-right: 10px" id="checkReject" onclick="checkBoxCheck(this.id);" /><br />
                        <label class="chkboxLabel" style="padding-right: 10px; font-size: 16px">Review - LC</label><input type="checkbox" runat="server" style="margin-right: 10px" id="checkReview" onclick="checkBoxCheck(this.id)" /><br />
                    </div>
                </div>
            </div>
            <br />
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
                    </div>
                    <div class="container row">
                        <div runat="server" class="divWidth-6" style="padding-left: 20px" id="divIDNo">ID No</div>
                        <div runat="server" class="divWidth-6" id="divGrantName">Grant Name</div>
                    </div>
                    <div class="container row">
                        <div runat="server" class="divWidth-6" style="padding-left: 20px" id="divTransType"></div>
                        <div runat="server" class="divWidth-6" id="divRegion">Region</div>
                    </div>
                    <div class="container row">
                        <div runat="server" class="divWidth-6" style="padding-left: 20px" id="divTransDate"></div>
                    </div>
                    <br />
                    <div class="container row">
                        <fieldset>
                            <legend style="display: block">General Particulars</legend>
                            <asp:Table runat="server" ID="tblDocGeneral" Style="width: 100%;"></asp:Table>
                        </fieldset>
                    </div>
                    <br />
                    <div class="container row">
                        <fieldset>
                            <legend style="display: block">Particulars of Income</legend>
                            <asp:Table runat="server" ID="tblDocIncome" Style="width: 100%;"></asp:Table>
                        </fieldset>
                    </div>
                    <br />
                    <div class="container row">
                        <fieldset>
                            <legend style="display: block">Particulars of Assets</legend>
                            <asp:Table runat="server" ID="tblDocAssets" Style="width: 100%;"></asp:Table>
                        </fieldset>
                    </div>
                    <div class="container row">
                        <asp:Button ID="btnClose" CssClass="btn btn-primary active pull-right" runat="server" Text="Close" OnClientClick="window.close();" />
                        <asp:Button ID="btnReprint" CssClass="btn btn-primary active pull-right" runat="server" Text="Reprint" OnClientClick="printpage('N')" />
                        <asp:Button ID="btnPrint" CssClass="btn btn-primary active pull-right" runat="server" Text="Print to add to batch" OnClick="btnPrint_Click" OnClientClick="myECM.showPleaseWait('Just a moment...');" />
                    </div>
                </div>
                <!--RIGHT-SIDE DIV FOR BARCODES-->
                <%-- <div style="display: block; float: right; width:10%; min-height:700px">
                    <img class="verticalDiv" runat="server" id="divIDNoBarcode" style="top:-50px; width:250px"/>
                    <img class="verticalDiv" runat="server" alt="PLACEHOLDER" id="divFileUnqBarcode" style="top:200px;width:250px" />
                </div>--%>
                <div style="float: right; width: 10%;">
                    <img class="some_block" runat="server" id="divIDNoBarcode" />
                    <img class="some_block" style="top: 480px" runat="server" alt="PLACEHOLDER" id="divFileUnqBarcode" />
                    <img class="some_block" style="top: 700px" runat="server" alt="PLACEHOLDER" id="divBRMBarCode" />
                </div>
            </div>
            <br />
        </div>
    </form>
</body>
</html>
