<%@ Page Title=""
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="BoxCover.aspx.cs"
    Inherits="SASSADirectCapture.Views.BoxCover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Batch Edit</title>
    <link href="../Content/bootstrap.css" rel='stylesheet' type='text/css' />
    <link href="../Content/Site.css" rel='stylesheet' type='text/css' />
    <script src="../Scripts/jquery-3.5.1.min.js"></script>
    <%-- <script src="../Scripts/code39.js"></script>--%>
    <script src="../Scripts/JsBarcode.all.min.js"></script>

    <style type="text/css">
        .checkbox label {
            display: inline;
            margin-left: 3px;
            font-size: 1.0em;
        }
    </style>
</head>
<body>
    <script type="text/javascript">
        function printDiv(divName, doServerSide) {

            //Buttonz
            document.getElementById("Buttonz").style.display = "none";
            document.getElementById("trPrintFileCovers").style.display = "none";
            window.print();

            document.getElementById("PandC").value = "Reprint Box and Close";
            document.getElementById("Buttonz").style.display = "";
            document.getElementById("trPrintFileCovers").style.display = "";

            //print document
            //////var printContents = document.getElementById(divName).innerHTML;
            //////var originalContents = document.body.innerHTML;

            //////document.body.innerHTML = printContents;
            //////window.print();

            //////document.body.innerHTML = originalContents;

            //update server side
            //if (doServerSide)
            //{
            //    $("#<%= btnPrintClose.ClientID %>").trigger("click");
            //}

            // HIDE BUTTONS AND PRINT
            ////document.getElementById('PandC').style.display = "none";
            ////document.getElementById('Reprint').style.display = "none";
            //window.print();
            //update server side
            if (doServerSide) {
                $("#<%= btnPrintClose.ClientID %>").trigger("click");
            }
            else {
                window.close();
            }

        }

        $(document).ready(function () {
            //document.getElementById('PrintDiv').style.display="none";
            //document.getElementById("<%=btnPrintClose.ClientID%>").style.display="none";

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

        function DoJobs() {
            var myLocation = window.location.href;
            var posisie = myLocation.indexOf('BoxNo=', 0) + 6;
            var myboxBC = myLocation.substring(posisie);
            //$("#barcode").JsBarcode(myboxBC, { format: "CODE128", displayValue: true, fontSize: 14, height: 50 });
            $("#barcode").JsBarcode($("#barcode").attr("alt"), { format: "CODE128", displayValue: true, fontSize: 14, height: 50, width: 1 });
        }
    </script>
    <form id="form1" runat="server" style="background-color: #eeeeef; min-height: 650px;">
        <div style="max-width: 1024px; width: 1024px; min-height: 600px; display: block; margin: auto; background-color: #ffffff">
            <asp:ScriptManager runat="server"></asp:ScriptManager>

            <div class="container row" style="height: 100%; display: block;">
                <div style="width: 30%; float: left; display: inline-block">
                    <img alt="" src="../Content/images/sassa_logoSmall.jpg" width="200" />
                </div>
                <div style="width: 40%; display: inline-block; height: 100px; text-align: center; padding-top: 10px;">
                    <label class="h2">Box Inventory</label>
                </div>
                <div id="barcodecontainer" style="width: 25%; float: right; margin-right: 15px; margin-top: 0px; margin-bottom: 10px">
                    <div style="float: right">
                        <img runat="server"
                            style="top: 10px; position: relative;"
                            class="barcode"
                            id="barcode" />
                    </div>
                    <br />
                    <br />
                    <br />
                    <p>
                        No of Files:
                         <span runat="server" id="txtNoFiles"></span>
                        <br />
                        Box No:
                        <span runat="server" id="txtBoxNo"></span>
                        <br />
                    </p>
                    <div class="divDateTime"></div>
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
                    <td colspan="6">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="False" width="99%" style="height: 100%; width: 100%;">
                            <ContentTemplate>
                                <section class="contact">
                                    <asp:GridView ID="boxGridView" CssClass="gridView" runat="server"
                                        ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="True"
                                        PageSize="40" CellPadding="4" ForeColor="#333333" GridLines="None"
                                        OnPageIndexChanging="batchGridView_PageIndexChanging" AllowPaging="True">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="CLM_UNIQUE_CODE" HeaderText="CLM Unique Code"></asp:BoundField>
                                            <asp:BoundField DataField="BRM_BARCODE" HeaderText="BRM File no"></asp:BoundField>
                                            <asp:BoundField DataField="APPLICANT_NO" HeaderText="ID no"></asp:BoundField>
                                            <asp:BoundField DataField="FULL_NAME" HeaderText="Name and Surname"></asp:BoundField>
                                            <asp:BoundField DataField="GRANT_TYPE_NAME" HeaderText="Grant Type"></asp:BoundField>
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
            </table>

            <table align="right">
                <tr id="trPrintFileCovers">
                    <td colspan="7">
                        <asp:CheckBox ID="cbxPrintFileCovers" runat="server" Text="Also print file coversheets?" CssClass="checkbox" />
                    </td>
                </tr>
                <tr id="Buttonz">
                    <td>&nbsp;</td>
                    <td style="display: none">
                        <asp:Button ID="btnPrintClose" runat="server" OnClick="btnPrintClose_Click" CssClass="btn btn-primary active pull-right" Text="Print Box Cover and Close" /></td>
                    <td colspan="5">
                        <input type="button" id="btnCancel" class="btn btn-primary active pull-right" value="Cancel" onclick="window.close();" />
                        <input type="button" id="PandC" class="btn btn-primary active pull-right" value="Print Box and Close" onclick="printDiv('PrintDiv', true)" />
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