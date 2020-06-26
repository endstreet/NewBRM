using SASSADirectCapture.BL;
using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class BoxAudit : SassaPage
    {
        #region Private Fields

        //private SASSA_Authentication authObject = new SASSA_Authentication();

        private Entities en = new Entities();


        #endregion Private Fields

        #region Protected Methods

        protected void boxGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Retrieve the underlying data item.
                MISBoxFiles rowFile = (MISBoxFiles)e.Row.DataItem;

                switch (rowFile.REGISTRY_TYPE)
                {
                    case "DESTROY":
                        e.Row.Style["background-color"] = "#FF3333";//Red
                        break;

                    case "ARCHIVE":
                        e.Row.Style["background-color"] = "#FFCC33";//Orange
                        break;

                    case "MAIN":
                        e.Row.Style["background-color"] = "#55FF55";//Green
                        break;

                    default: break;
                }

                if (rowFile.TRANSFER_REGION != null && rowFile.REGION_NAME != rowFile.TRANSFER_REGION)
                {
                    e.Row.Cells[7].Style["background-color"] = "#AAAAAA";//Grey
                    e.Row.Cells[9].Style["background-color"] = "#AAAAAA";//Grey
                }

                if (rowFile.EXCLUSIONS != "")
                {
                    e.Row.Cells[14].Style["background-color"] = "#88BBFF";//Blue
                }

                int iPrintOrder = Int32.Parse(e.Row.Cells[21].Text);
                int iLastPrintOrder = (Int32)(ViewState["iLastPrintOrder"] == null ? -1 : ViewState["iLastPrintOrder"]);

                if (iPrintOrder > iLastPrintOrder)
                {
                    iLastPrintOrder = iPrintOrder;
                }
            }
        }

        protected void btnAddFileToBox_Click(object sender, EventArgs e)
        {
            divReboxError.Visible = false;
            divReboxSuccess.Visible = false;

            try
            {
                string fileno = txtBRMFileToRebox.Text.Trim().ToUpper();
                string boxBarcode = txtBoxNo.Text.Trim().ToUpper();
                string boxType = ddlBoxType.SelectedValue == "3" ? ddlTransferTo.SelectedValue : ddlBoxType.SelectedValue;
                string boxArchiveYear = txtReboxArchYear.Text;

                if (string.IsNullOrEmpty(boxBarcode))
                {
                    lblReboxError.Text = "No Box Barcode specified.";
                    divReboxError.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(fileno))
                {
                    lblReboxError.Text = "No BRM file number specified.";
                    divReboxError.Visible = true;
                    return;
                }

                if (ddlBoxType.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ReboxInvalid", "alert('Please select a Box Type.');", true);
                }
                else
                {
                    switch (boxType)
                    {
                        case "0":
                            ScriptManager.RegisterStartupScript(this, GetType(), "ReboxInvalid", "alert('Please select a Transfer To option for this Transfer box.');", true);
                            break;

                        case "14":
                        case "15":
                        case "16":
                        case "17":
                        case "18":
                            if (boxArchiveYear == "")
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "ReboxInvalid", "alert('Please enter the Archive Year for the box.');", true);
                            }
                            else
                            {
                                int iArchiveYear = 0;
                                if (boxArchiveYear.Length != 4 || !(Int32.TryParse(boxArchiveYear, out iArchiveYear)))
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "ReboxInvalid", "alert('Archive Year for the box must be 4 numbers.');", true);
                                }
                                else
                                {
                                    updateFileReboxStatus(fileno, boxBarcode, boxType, boxArchiveYear);
                                    txtBRMFileToRebox.Text = string.Empty;
                                }
                            }
                            break;

                        default:
                            updateFileReboxStatus(fileno, boxBarcode, boxType, boxArchiveYear);
                            txtBRMFileToRebox.Text = string.Empty;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                txtBRMFileToRebox.Text = string.Empty;
                lblReboxError.Text = ex.Message;
                divReboxError.Visible = true;
            }
        }

        protected void btnBoxFull_Click(object sender, EventArgs e)
        {
            string sBoxBarcode = txtBoxNo.Text.Trim().ToUpper();
            string sAltBoxNo = "";
            using (Entities en = new Entities())
            {
                var y = en.DC_FILE
                        .Where(f => f.TDW_BOXNO == sBoxBarcode && f.ALT_BOX_NO.Trim() != null).OrderByDescending(g => g.UPDATED_DATE).FirstOrDefault();

                if (y == null)
                {
                    using (Entities en2 = new Entities())
                    {
                        var x = en2.DC_FILE
                        .Where(f => f.TDW_BOXNO == sBoxBarcode).OrderByDescending(g => g.UPDATED_DATE).FirstOrDefault();
                        switch (UserSession.Office.RegionId)
                        {
                            case "1":
                                sAltBoxNo = "WCA" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_WCA.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "2":
                                sAltBoxNo = "ECA" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_ECA.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "3":
                                sAltBoxNo = "NCA" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_NCA.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "4":
                                sAltBoxNo = "FST" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_FST.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "5":
                                sAltBoxNo = "KZN" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_KZN.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "6":
                                sAltBoxNo = "NWP" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_NWP.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "7":
                                sAltBoxNo = "GAU" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_GAU.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "8":
                                sAltBoxNo = "MPU" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_MPU.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "9":
                                sAltBoxNo = "LIM" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_LIM.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            default:
                                break;
                        }

                        x.ALT_BOX_NO = sAltBoxNo;
                        x.UPDATED_DATE = DateTime.Now;

                        if (bool.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["UseADAuth"].ToString()))
                        {
                            x.UPDATED_BY_AD = UserSession.SamName;
                            //}
                            //else
                            //{
                            //    x.UPDATED_BY = new SASSA_Authentication().getUserID();
                            //    x.UPDATED_BY_AD = util.getUserFullName(x.UPDATED_BY.ToString());
                            //}
                            en2.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Box Full Update"));
                            en2.SaveChanges();
                        }
                    }
                }
                else
                {
                    sAltBoxNo = y.ALT_BOX_NO;
                }
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "AltBoxNo", "alert('Please write down the following Alternate Box Number for the box before closing the screen: " + sAltBoxNo + "'); var printwin = window.open('BoxCover.aspx?BoxNo=" + sBoxBarcode + "&bxf=y', 'Get Box Barcode Number'); printwin.focus();", true);
        }

        protected void btnBoxFullError_Click(object sender, EventArgs e)
        {
            lblReboxError.Text = "Please enter TDW Box Number and try again";
            divReboxError.Visible = true;
        }

        protected void btnCheckFile_Click(object sender, EventArgs e)
        {
            bool bFound = false;
            if (UserSession.Office.RegionId == "2")
            {
                foreach (GridViewRow row in boxGridView.Rows)
                {
                    if (!bFound && row.Cells[0].Text == hfCheckFile.Value && !(((CheckBox)row.FindControl("cbxFound")).Checked))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "ClickCheckbox", "document.getElementById('" + row.FindControl("cbxFound").ClientID + "').click();", true);
                        bFound = true;
                    }
                }
            }
            else
            {
                foreach (GridViewRow row in boxGridView.Rows)
                {
                    if (!bFound && row.Cells[4].Text == hfCheckFile.Value && !(((CheckBox)row.FindControl("cbxFound")).Checked))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "ClickCheckbox", "document.getElementById('" + row.FindControl("cbxFound").ClientID + "').click();", true);
                        bFound = true;
                    }
                }
            }
        }

        protected void btnRevertBRM_Click(object sender, EventArgs e)
        {
            boxGridView.Rows[Int32.Parse(hfRowIndex.Value)].Cells[2].Text = boxGridView.Rows[Int32.Parse(hfRowIndex.Value)].Cells[3].Text;
            ((CheckBox)boxGridView.Rows[Int32.Parse(hfRowIndex.Value)].Cells[19].FindControl("cbxFound")).Checked = false;
        }

        protected void btnSetBRM_Click(object sender, EventArgs e)
        {
            boxGridView.Rows[Int32.Parse(hfRowIndex.Value)].Cells[2].Text = Session["BRM"].ToString();
        }

        protected void btnSetReboxFields_Click(object sender, EventArgs e)
        {
            try
            {
                using (Entities context = new Entities())
                {
                    DC_FILE file = context.DC_FILE.Where(f => f.TDW_BOXNO == txtBoxNo.Text).OrderByDescending(g => g.UPDATED_DATE).FirstOrDefault();

                    if (file != null)
                    {
                        string sBoxTypeID = file.TDW_BOX_TYPE_ID.ToString();
                        switch (sBoxTypeID)
                        {
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                            case "9":
                            case "10":
                            case "11":
                            case "12":
                                ddlBoxType.SelectedValue = "3";
                                ddlTransferTo.SelectedValue = sBoxTypeID;
                                tdlblTransferTo.Attributes["Style"] = "";
                                tdddlTransferTo.Attributes["Style"] = "padding-bottom: 10px;";
                                tdlblReboxArchYear.Attributes["Style"] = "display: none";
                                tdtxtReboxArchYear.Attributes["Style"] = "display: none";
                                break;

                            case "14":
                            case "15":
                            case "16":
                            case "17":
                            case "18":
                                ddlBoxType.SelectedValue = sBoxTypeID;
                                tdlblTransferTo.Attributes["Style"] = "display: none";
                                tdddlTransferTo.Attributes["Style"] = "display: none";
                                tdlblReboxArchYear.Attributes["Style"] = "";
                                tdtxtReboxArchYear.Attributes["Style"] = "padding-bottom: 10px;";
                                break;

                            default:
                                ddlBoxType.SelectedValue = sBoxTypeID == "" ? "0" : sBoxTypeID;
                                tdlblTransferTo.Attributes["Style"] = "display: none";
                                tdddlTransferTo.Attributes["Style"] = "display: none";
                                tdlblReboxArchYear.Attributes["Style"] = "display: none";
                                tdtxtReboxArchYear.Attributes["Style"] = "display: none";
                                break;
                        }

                        txtReboxArchYear.Text = file.TDW_BOX_ARCHIVE_YEAR;
                    }
                }
            }
            catch (Exception ex)
            {
                lblReboxError.Text = ex.Message;
                divReboxError.Visible = true;
            }
        }

        protected void cbxFound_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbxFound = ((CheckBox)sender);
            hfRowIndex.Value = (((GridViewRow)cbxFound.Parent.Parent).RowIndex).ToString();
            if (cbxFound.Checked)
            {
                int iLastPrintOrder = (Int32)(ViewState["iLastPrintOrder"] == null ? -1 : ViewState["iLastPrintOrder"]);
                iLastPrintOrder++;
                ViewState.Add("iLastPrintOrder", iLastPrintOrder);
                ((GridViewRow)cbxFound.Parent.Parent).Cells[21].Text = iLastPrintOrder.ToString();

                ScriptManager.RegisterStartupScript(this, GetType(), "ClickCheckbox", "openBRMForm();", true);
            }
            else
            {
                btnRevertBRM_Click(sender, e);
            }
        }

        protected void ddlBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlBoxType.SelectedValue)
            {
                case "3":
                    tdlblTransferTo.Attributes["Style"] = "";
                    tdddlTransferTo.Attributes["Style"] = "padding-bottom: 10px;";
                    tdlblReboxArchYear.Attributes["Style"] = "display: none";
                    tdtxtReboxArchYear.Attributes["Style"] = "display: none";
                    break;

                case "14":
                case "15":
                case "16":
                case "17":
                case "18":
                    tdlblTransferTo.Attributes["Style"] = "display: none";
                    tdddlTransferTo.Attributes["Style"] = "display: none";
                    tdlblReboxArchYear.Attributes["Style"] = "";
                    tdtxtReboxArchYear.Attributes["Style"] = "padding-bottom: 10px;";
                    break;

                default:
                    tdlblTransferTo.Attributes["Style"] = "display: none";
                    tdddlTransferTo.Attributes["Style"] = "display: none";
                    tdlblReboxArchYear.Attributes["Style"] = "display: none";
                    tdtxtReboxArchYear.Attributes["Style"] = "display: none";
                    break;
            }
        }

        protected void ddlRegistryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opt = ddlRegistryType.SelectedIndex.ToString();
            switch (opt)
            {
                case "0":
                    ScriptManager.RegisterStartupScript(this, GetType(), "enterRTwrong", "alert('Please select a Registry Type');", true);
                    break;

                case "1":
                    divArchYear.Visible = false;
                    divAY1.Visible = false;
                    txtArchYear.Enabled = false;
                    txtArchYear.Text = String.Empty;
                    ScriptManager.RegisterStartupScript(this, GetType(), "sendbtnPickBox", "sendbtn('btnPickBox');", true);
                    txtBoxPickedNo.Focus();
                    break;

                case "2":
                    divArchYear.Visible = true;
                    divAY1.Visible = true;
                    txtArchYear.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "sendbtnPickBox", "sendbtn('btnPickBox');", true);
                    txtBoxPickedNo.Focus();
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string authenticatedUsername = UserSession.SamName;

                //If no session values are found , redirect to the login screen
                //if (authenticatedUsername == string.Empty)
                //{
                //    this.authObject.RedirectToLoginPage();
                //}

                showlastPicklistValues();

                if (UserSession.Office.RegionId == "2")
                {
                    ddlDistrict.DataSource = util.getECDistricts();
                    ddlDistrict.DataBind();
                    TDlblSearchDistrict.Attributes["Style"] = "";
                    TDddlDistrict.Attributes["Style"] = "";
                    TDlblSearchBin.Attributes["Style"] = "display: none";
                    TDtxtSearchBin.Attributes["Style"] = "display: none";
                }
                else
                {
                    ddlDistrict.DataSource = null;
                    ddlDistrict.DataBind();
                    TDlblSearchDistrict.Attributes["Style"] = "display: none";
                    TDddlDistrict.Attributes["Style"] = "display: none";
                    TDlblSearchBin.Attributes["Style"] = "";
                    TDtxtSearchBin.Attributes["Style"] = "";
                }

                ddlBoxType.DataSource = util.getBoxTypes();
                ddlBoxType.DataBind();

                ddlTransferTo.DataSource = util.getBoxTypesTransfer();
                ddlTransferTo.DataBind();
            }

            if (txtBoxNo.Text == "")
            {
                tdBRMFileToRebox.Attributes["Style"] = "display: none";
            }
            else
            {
                tdBRMFileToRebox.Attributes["Style"] = "";
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "HideShowArchYear", "HideShowArchYear(" + ddlRegistry_Type.ClientID + ");", true);
        }

        #endregion Protected Methods

        #region MIS BOX TAB METHODS

        protected static void Close_Bulk_Batch_For_Office(decimal batchNo)
        {
            using (Entities context = new Entities())
            {
                try
                {
                    var query = from bn in context.DC_BATCH
                                .Where(bn => bn.BATCH_NO == batchNo)
                                .Where(bn => bn.BATCH_STATUS == "BULK")
                                .Where(bc => bc.BATCH_CURRENT == "Y")
                                select bn;
                    if (query.Any())
                    {
                        foreach (DC_BATCH s in query)
                        {
                            s.BATCH_CURRENT = "N";
                        }
                        context.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Close Bulk Batch For Office"));
                        context.SaveChanges();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        protected static decimal Create_Bulk_Batch_For_Office()
        {
            string localOffice = UserSession.Office.OfficeId;

            decimal batchNo = 0;
            batchNo = Get_Bulk_Batch_For_Office(localOffice);

            using (Entities context = new Entities())
            {
                try
                {
                    if (batchNo == 0)
                    {
                        DC_BATCH b = new DC_BATCH();
                        b.BATCH_STATUS = "BULK";
                        b.BATCH_CURRENT = "Y";
                        b.OFFICE_ID = UserSession.Office.OfficeId;
                        b.UPDATED_DATE = DateTime.Now;

                        b.UPDATED_BY_AD = UserSession.SamName;
                        context.DC_BATCH.Add(b);
                        context.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Create Bulk Batch For Office"));
                        context.SaveChanges();
                        batchNo = Get_Bulk_Batch_For_Office(localOffice);
                    }
                }
                catch { }
            }
            return batchNo;
        }

        protected static string Get_Account_Status(
            string ps1, string ps2, string ps3, string ps4,
            string ss1, string ss2, string ss3, string ss4)
        {
            //  IF ANY OF THE GRANTS FOR THIS PERSON IS STILL ACTIVE ALL ARE REGISTRY TYPE MAIN
            string status_1 = (ss1 + ps1).ToString();
            string status_2 = (ss2 + ps2).ToString();
            string status_3 = (ss3 + ps3).ToString();
            string status_4 = (ss4 + ps4).ToString();

            string reg_type = "ARCHIVE";

            //-----------------------  MAIN  -------------------------
            if ((status_1 == "2A") || (status_1 == "2B") || (status_1 == "29"))
            {
                reg_type = "MAIN";
            }
            else if ((status_2 == "2A") || (status_2 == "2B") || (status_2 == "29"))
            {
                reg_type = "MAIN";
            }
            else if ((status_3 == "2A") || (status_3 == "2B") || (status_3 == "29"))
            {
                reg_type = "MAIN";
            }
            else if ((status_4 == "2A") || (status_4 == "2B") || (status_4 == "29"))
            {
                reg_type = "MAIN";
            }

            return reg_type;
        }

        protected static string Get_ArchiveYear(string sd1, string sd2, string sd3, string sd4)
        {
            int tryDate1 = 0;
            int tryDate2 = 0;
            int tryDate3 = 0;
            int tryDate4 = 0;

            if (!string.IsNullOrEmpty(sd1)) { int.TryParse(sd1, out tryDate1); }
            if (!string.IsNullOrEmpty(sd2)) { int.TryParse(sd2, out tryDate2); }
            if (!string.IsNullOrEmpty(sd3)) { int.TryParse(sd3, out tryDate3); }
            if (!string.IsNullOrEmpty(sd4)) { int.TryParse(sd4, out tryDate4); }

            int lastdate;
            if (tryDate1 > tryDate2) { lastdate = tryDate1; } else { lastdate = tryDate2; }
            if (tryDate3 > lastdate) { lastdate = tryDate3; }
            if (tryDate4 > lastdate) { lastdate = tryDate4; }

            return lastdate == 0 ? null : lastdate.ToString().Substring(0, 4);
        }

        protected static decimal Get_Bulk_Batch_For_Office(string office)
        {
            decimal batchno = 0;

            using (Entities context = new Entities())
            {
                try
                {
                    IEnumerable<decimal> query = context.Database.SqlQuery<decimal>
                        (@"select b.BATCH_NO
                            from CONTENTSERVER.DC_BATCH b
                                    WHERE b.OFFICE_ID = '" + office + @"'
                                    AND b.BATCH_STATUS = 'BULK'
                                    AND b.BATCH_CURRENT = 'Y'");
                    foreach (decimal s in query)
                    {
                        batchno = s;
                    }
                }
                catch (Exception)
                {
                }
                return batchno;
            }
        }

        protected static int Get_FiveYears_Later(string fromString)
        {
            string fiveYearsLater = String.Empty;
            string nowCCYY = fromString.Substring(0, 4);
            int nowNumber = int.Parse(nowCCYY);
            int thenNumber = nowNumber + 6;//Changed to 6 years, since we're only comparing years, not months or days.
            return thenNumber;
        }

        protected static string Get_Grant_Date(
            string sd1, string sd2, string sd3, string sd4,
            string gType1, string gType2, string gType3, string gType4, string GRANT_TYPE)
        {
            int tryDate1 = 0;
            int tryDate2 = 0;
            int tryDate3 = 0;
            int tryDate4 = 0;

            if (!string.IsNullOrEmpty(sd1)) { int.TryParse(sd1, out tryDate1); }
            if (!string.IsNullOrEmpty(sd2)) { int.TryParse(sd2, out tryDate2); }
            if (!string.IsNullOrEmpty(sd3)) { int.TryParse(sd3, out tryDate3); }
            if (!string.IsNullOrEmpty(sd4)) { int.TryParse(sd4, out tryDate4); }

            int lastdate;
            if (tryDate1 > tryDate2) { lastdate = tryDate1; } else { lastdate = tryDate2; }
            if (tryDate3 > lastdate) { lastdate = tryDate3; }
            if (tryDate4 > lastdate) { lastdate = tryDate4; }

            return lastdate.ToString();
        }

        protected void btnPrintBulk_Hidden_Click(object sender, EventArgs e)//btnPrintBulk_Click
        {
            try
            {
                if (hfTDWBoxNo.Value != "")
                {
                    string sTDWBoxNo = hfTDWBoxNo.Value;

                    //  Check if you have a bulk batch for this RMC
                    //  if not Create BULK batch for RMC

                    decimal thisBatch = 0;
                    string box_selected = txtSearchBox.Text;
                    string sAltBoxNo = "";

                    using (Entities en = new Entities())
                    {
                        DC_FILE file = en.DC_FILE.Where(f => f.TDW_BOXNO == hfTDWBoxNo.Value).OrderByDescending(g => g.UPDATED_DATE).FirstOrDefault();
                        if (file != null)
                        {
                            sAltBoxNo = file.ALT_BOX_NO;
                            thisBatch = file.BATCH_NO == null ? Create_Bulk_Batch_For_Office() : (decimal)file.BATCH_NO;
                        }
                        else
                        {
                            thisBatch = Create_Bulk_Batch_For_Office();
                        }
                    }

                    //  Get all MIS files for that box
                    //  For each

                    DateTime nowDate = DateTime.Now;

                    string localRegion = UserSession.Office.RegionCode;

                    var brmNumberDictionary = new Dictionary<string, bool>();
                    var brmNumberList = new List<string>();
                    foreach (GridViewRow row in boxGridView.Rows)
                    {
                        // BRM no
                        string sBRMNumber = row.Cells[2].Text.Trim(); //FILE_NUMBER
                        if (sBRMNumber != "&nbsp;")
                        {
                            brmNumberDictionary.Add(sBRMNumber, false);
                            brmNumberList.Add($"'{sBRMNumber}'");
                        }
                    }

                    var commandText = $@"SELECT BRM_BARCODE FROM DC_FILE WHERE BRM_BARCODE IN ({string.Join(",", brmNumberList)})";
                    using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(en.Database.Connection.ConnectionString))
                    {
                        connection.Open();

                        var command = new Oracle.ManagedDataAccess.Client.OracleCommand
                        {
                            CommandText = commandText,
                            Connection = connection,
                            CommandTimeout = 45,
                            CommandType = CommandType.Text
                        };

                        var oracleDataReader = command.ExecuteReader();
                        if (oracleDataReader.HasRows)
                        {
                            while (oracleDataReader.Read())
                            {
                                var brmNumber = oracleDataReader.IsDBNull(0) ? string.Empty : oracleDataReader.GetString(0);
                                if (brmNumberDictionary.ContainsKey(brmNumber))
                                {
                                    brmNumberDictionary[brmNumber] = true;
                                }
                            }
                        }
                    }

                    foreach (GridViewRow row in boxGridView.Rows)
                    {
                        // set variables to create dc_file record
                        string UpdateorCreate = "C";

                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            //UNQ_FILE_NO...
                            string unqfileno = row.Cells[1].Text;
                            if (unqfileno == "&nbsp;") { unqfileno = ""; }

                            // BRM no
                            string sBRMNumber = row.Cells[2].Text.Trim(); //FILE_NUMBER
                            if (sBRMNumber == "&nbsp;") { sBRMNumber = ""; }

                            // check if the file already exists or not
                            if (brmNumberDictionary.ContainsKey(sBRMNumber))
                            {
                                if (brmNumberDictionary[sBRMNumber])
                                {
                                    UpdateorCreate = "U";
                                }
                            }

                            // mis file no
                            string filenumber = row.Cells[4].Text.Trim(); //FILE_NUMBER
                            if (filenumber == "&nbsp;") { filenumber = ""; }

                            // name and surname
                            string name = row.Cells[5].Text.Trim();       //USER_FIRSTNAME    //NAME
                            if (name == "&nbsp;") { name = ""; }

                            string surname = row.Cells[6].Text.Trim();    //USER_LASTNAME     //SURNAME
                            if (surname == "&nbsp;") { surname = ""; }

                            // region name and id 5 & 6
                            string regid = row.Cells[8].Text.Trim();      //REGION_ID       xxxxx hidn
                            if (regid == "&nbsp;") { regid = ""; }

                            if (localRegion == "&nbsp;") { localRegion = ""; }

                            string txfer = String.Empty;                  //TRANSFERRED
                            if (regid != localRegion) { txfer = "Y"; }

                            // grant name and type 7 & 8
                            string grant = row.Cells[11].Text.Trim();      //GRANT_TYPE xxxxx
                            if (grant == "&nbsp;") { grant = ""; }

                            // Registry Type
                            string regtype = row.Cells[12].Text.Trim();    //APPLICATION_STATUS and REGISTRY_TYPE
                            if (regtype == "&nbsp;")
                            {
                                regtype = "";
                            }
                            else if (regtype == "DESTROY")
                            {
                                if (sAltBoxNo == "" || sAltBoxNo == null)
                                {
                                    using (Entities en2 = new Entities())
                                    {
                                        switch (localRegion)
                                        {
                                            case "1":
                                                sAltBoxNo = "WCA" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_WCA.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                                break;

                                            case "2":
                                                sAltBoxNo = "ECA" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_ECA.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                                break;

                                            case "3":
                                                sAltBoxNo = "NCA" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_NCA.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                                break;

                                            case "4":
                                                sAltBoxNo = "FST" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_FST.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                                break;

                                            case "5":
                                                sAltBoxNo = "KZN" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_KZN.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                                break;

                                            case "6":
                                                sAltBoxNo = "NWP" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_NWP.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                                break;

                                            case "7":
                                                sAltBoxNo = "GAU" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_GAU.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                                break;

                                            case "8":
                                                sAltBoxNo = "MPU" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_MPU.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                                break;

                                            case "9":
                                                sAltBoxNo = "LIM" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_LIM.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                                break;

                                            default:
                                                break;
                                        }
                                    }
                                }
                            }

                            // Archive Year
                            string ayear = row.Cells[13].Text.Trim();     //ARCHIVE_YEAR
                            if (ayear == "&nbsp;") { ayear = ""; } else { ayear = ayear.Substring(0, 4); }

                            // Exclusions
                            string excl = row.Cells[14].Text.Trim();      //EXCLUSIONS
                            if (excl == "&nbsp;") { excl = ""; }

                            // id number
                            string idno = row.Cells[0].Text.Trim();
                            if (idno == "&nbsp;") { idno = ""; }

                            // work it out...
                            string transdate = row.Cells[16].Text.Trim(); //APP_DATE
                            DateTime? transDatetime = null;
                            if (transdate != "&nbsp;" && transdate != "0")
                            {
                                transDatetime = DateTime.ParseExact(transdate.Trim(), "yyyyMMdd", null);
                            }

                            // mis box number
                            string boxno = String.Empty;                  //MIS_BOXNO xxxxx
                            boxno = row.Cells[17].Text.Trim();
                            if (boxno == "&nbsp;") { boxno = ""; }
                            if (string.IsNullOrEmpty(boxno)) { boxno = txtSearchBox.Text; }

                            // child id number
                            string sChildID = row.Cells[18].Text.Trim();
                            if (sChildID == "&nbsp;") { sChildID = ""; }

                            // Missing?
                            string isMissing = "N";                       //MISSING //checkBoxMissing
                            string statusAudit = "Completed";
                            //CheckBox chkBoxMissing = (row.FindControl("checkBoxMissing") as CheckBox);
                            //if (chkBoxMissing.Checked) { isMissing = "Y"; statusAudit = "Missing"; }
                            CheckBox cbxFound = (row.FindControl("cbxFound") as CheckBox);
                            if (!cbxFound.Checked) { isMissing = "Y"; statusAudit = "Missing"; }

                            // Non-compliant
                            string isNonComp = "N";                       //NON_COMPLIANT //checkBoxNonCompliant
                            CheckBox chkBoxNonComp = (row.FindControl("checkBoxNonCompliant") as CheckBox);
                            if (chkBoxNonComp.Checked) { isNonComp = "Y"; }

                            // office id
                            string offid = UserSession.Office.OfficeId; //OFFICE_ID

                            // USER
                            string myUserID = "0";
                            // batch no - thisBatch

                            // Temporary Box Number
                            int? iTempBoxNo = null;
                            switch (row.Cells[12].Text.Trim())//Registry Type
                            {
                                case "MAIN":
                                    if (isNonComp == "Y")
                                    {
                                        iTempBoxNo = 3;//Main - Non Compliant
                                    }
                                    else
                                    {
                                        iTempBoxNo = 1;//Main
                                    }
                                    break;

                                case "ARCHIVE":
                                    iTempBoxNo = 2;//Archive
                                    break;

                                default:
                                    break;
                            }

                            switch (excl)
                            {
                                case "Legal":
                                    iTempBoxNo = 4;//Exclusions - Legal
                                    break;

                                case "Debtors":
                                    iTempBoxNo = 5;//Exclusions - Debtors
                                    break;

                                case "Fraud":
                                    iTempBoxNo = 6;//Exclusions - Fraud
                                    break;

                                default:
                                    break;
                            }

                            string sTransferRegion = row.Cells[9].Text.Trim();
                            if (row.Cells[7].Text.Trim() != sTransferRegion)//Region Name VS Transfer Region Name
                            {
                                switch (sTransferRegion)
                                {
                                    case "Western Cape":
                                        iTempBoxNo = 7;//Transfer - WCA
                                        break;

                                    case "Eastern Cape":
                                        iTempBoxNo = 8;//Transfer - ECA
                                        break;

                                    case "Gauteng":
                                        iTempBoxNo = 9;//Transfer - GAU
                                        break;

                                    case "Kwazulu Natal":
                                        iTempBoxNo = 10;//Transfer - KZN
                                        break;

                                    case "Limpopo":
                                        iTempBoxNo = 11;//Transfer - LIM
                                        break;

                                    case "North West":
                                        iTempBoxNo = 12;//Transfer - NWP
                                        break;

                                    case "Free State":
                                        iTempBoxNo = 13;//Transfer - FST
                                        break;

                                    case "Northern Cape":
                                        iTempBoxNo = 14;//Transfer - NCA
                                        break;

                                    case "Mpumalanga":
                                        iTempBoxNo = 15;//Transfer - MPU
                                        break;

                                    default:
                                        break;
                                }
                            }
                            //Not currently catering for Temp Box No 16 (Misplaced)

                            // Print order
                            string sPrintOrder = row.Cells[21].Text.Trim();
                            if (sPrintOrder == "&nbsp;") { sPrintOrder = "-1"; }

                            try
                            {
                                if (UpdateorCreate == "C")
                                {
                                    //now STORE THE DC_FILE RECORD
                                    using (Entities context = new Entities())
                                    {
                                        DC_FILE file = new DC_FILE();
                                        file.APPLICANT_NO = idno;
                                        file.BATCH_NO = thisBatch;
                                        file.OFFICE_ID = offid;
                                        file.REGION_ID = regid;
                                        file.GRANT_TYPE = grant;
                                        file.UPDATED_BY = int.Parse(myUserID);
                                        file.UPDATED_DATE = nowDate;
                                        file.USER_FIRSTNAME = name;
                                        file.USER_LASTNAME = surname;
                                        file.BATCH_ADD_DATE = nowDate;
                                        file.APPLICATION_STATUS = regtype;
                                        file.NON_COMPLIANT = isNonComp;
                                        file.MIS_BOXNO = boxno;
                                        file.FILE_NUMBER = filenumber;
                                        file.ARCHIVE_YEAR = ayear;
                                        file.EXCLUSIONS = excl;
                                        file.MISSING = isMissing;
                                        file.TRANSFERRED = txfer;
                                        file.TRANS_TYPE = 0;
                                        file.TRANS_DATE = transDatetime;
                                        file.MIS_BOX_STATUS = statusAudit;
                                        file.BRM_BARCODE = sBRMNumber;
                                        file.TDW_BOXNO = (regtype == "DESTROY" ? sTDWBoxNo : "");
                                        file.ALT_BOX_NO = (regtype == "DESTROY" ? sAltBoxNo : "");
                                        file.TEMP_BOX_NO = iTempBoxNo;
                                        file.CHILD_ID_NO = sChildID;
                                        file.PRINT_ORDER = Int32.Parse(sPrintOrder);

                                        context.DC_FILE.Add(file);
                                        context.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Add File"));
                                        context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    using (Entities context = new Entities())
                                    {
                                        DC_FILE file = context.DC_FILE.Where(f => f.UNQ_FILE_NO == unqfileno).FirstOrDefault();

                                        if (file != null)
                                        {
                                            file.APPLICANT_NO = idno;
                                            file.BATCH_NO = thisBatch;
                                            file.OFFICE_ID = offid;
                                            file.REGION_ID = regid;
                                            file.GRANT_TYPE = grant;
                                            file.UPDATED_BY = int.Parse(myUserID);
                                            file.UPDATED_DATE = nowDate;
                                            file.USER_FIRSTNAME = name;
                                            file.USER_LASTNAME = surname;
                                            file.BATCH_ADD_DATE = nowDate;
                                            file.APPLICATION_STATUS = regtype;
                                            file.NON_COMPLIANT = isNonComp;
                                            file.MIS_BOXNO = boxno;
                                            file.FILE_NUMBER = filenumber;
                                            file.ARCHIVE_YEAR = ayear;
                                            file.EXCLUSIONS = excl;
                                            file.MISSING = isMissing;
                                            file.TRANSFERRED = txfer;
                                            file.TRANS_TYPE = 0;
                                            file.TRANS_DATE = transDatetime;
                                            file.MIS_BOX_STATUS = statusAudit;
                                            file.BRM_BARCODE = sBRMNumber;
                                            file.TDW_BOXNO = (regtype == "DESTROY" ? sTDWBoxNo : "");
                                            file.ALT_BOX_NO = (regtype == "DESTROY" ? sAltBoxNo : "");
                                            file.TEMP_BOX_NO = iTempBoxNo;
                                            file.CHILD_ID_NO = sChildID;
                                            file.PRINT_ORDER = Int32.Parse(sPrintOrder);
                                            context.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Update File"));
                                            context.SaveChanges();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = ex.Message;
                                lblError.Text += " InnerException:" + ex.InnerException;
                                lblError.Text += " StackTrace:" + ex.StackTrace;
                                divError.Visible = true;
                            }
                        }
                    }

                    Close_Bulk_Batch_For_Office(thisBatch);

                    if (sAltBoxNo != "" && sAltBoxNo != null)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "AltBoxNo", "alert('Please write down the following Alternate Box Number for the box before closing the screen: " + sAltBoxNo + "');", true);
                    }

                    //  call bulk print
                    string myJavaString = "bulkCoversForBatch('" + thisBatch + "','" + box_selected + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "CoversForBatch", myJavaString, true);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void btnPrintDestroy_Click(object sender, EventArgs e)
        {
            string Status = "DESTROY";
            string BoxNo = txtSearchBox.Text;
            string RegID = UserSession.Office.RegionId;
            string RegName = UserSession.Office.RegionName; //util.getRegion("name", RegID);

            if (!CheckIfBoxPrinted())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "printMissing", "alert('Please print the batch cover sheets before printing the destroy list.');", true);
            }
            else
            {
                using (Entities context = new Entities())
                {
                    IEnumerable<FileEntity> query = context.Database.SqlQuery<FileEntity>
                          (@"select
                            UNQ_FILE_NO
                        from CONTENTSERVER.DC_FILE
                        where MIS_BOXNO = '" + BoxNo + "' and REGION_ID = '" + RegID + "' and APPLICATION_STATUS = 'DESTROY'");

                    if (!query.Any())
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "printMissing", "alert('There are currently no destroy files in the list.');", true);
                    }
                    else
                    {
                        string myScriptStr = "printBoxFiles('" + Status + "','" + BoxNo + "','" + RegID + "','" + RegName + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "printDestroy", myScriptStr, true);
                    }
                }
            }
        }

        protected void btnPrintMissing_Click(object sender, EventArgs e)
        {
            string Status = "MISSING";
            string BoxNo = txtSearchBox.Text;
            string RegID = UserSession.Office.RegionId;
            string RegName = UserSession.Office.RegionName; //util.getRegion("name", RegID);

            if (!CheckIfBoxPrinted())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "printMissing", "alert('Please print the batch cover sheets before printing the missing list.');", true);
            }
            else
            {
                using (Entities context = new Entities())
                {
                    IEnumerable<FileEntity> query = context.Database.SqlQuery<FileEntity>
                          (@"select
                            UNQ_FILE_NO
                        from CONTENTSERVER.DC_FILE
                        where MIS_BOXNO = '" + BoxNo + "' and REGION_ID = '" + RegID + "' and MISSING = 'Y'");

                    if (!query.Any())
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "NoMissingFiles", "alert('There are currently no missing files in the list.');", true);
                    }
                    else
                    {
                        string myScriptStr = "printBoxFiles('" + Status + "','" + BoxNo + "','" + RegID + "','" + RegName + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "printMissing", myScriptStr, true);
                    }
                }
            }
        }

        protected void btnSearchBox_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingMsg", "myECM.hidePleaseWait();", true);

            if (UserSession.Office.RegionId == "2")
            {
                int iBoxNumber = 0;
                if (ddlDistrict.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertnoboxno", "alert('Please enter Box no, Registry Type and District, then try again.');", true);
                }
                else if (!int.TryParse(txtSearchBox.Text, out iBoxNumber))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertnoboxno", "alert('Please enter a valid Box no. Box no consists only of numbers.');", true);
                }
                else
                {
                    btnPrintBulk.Attributes["onclick"] = "return true;";
                    btnPrintBulk.Enabled = true;
                    Load_MISBoxData();
                }
            }
            else if (string.IsNullOrEmpty(txtSearchBox.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertnoboxno", "alert('Please enter Box no and Registry Type, then try again.');", true);
            }
            else if (ddlRegistry_Type.SelectedValue == "A" && txtSearchArchYear.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertnoboxno", "alert('Please enter the Archive Year, then try again.');", true);
            }
            else
            {
                btnPrintBulk.Attributes["onclick"] = "return true;";
                btnPrintBulk.Enabled = true;
                Load_MISBoxData();
            }
        }

        protected void btnUpdateBoxComplete_Click(object sender, EventArgs e)
        {
            divError.Visible = false;
            divSuccess.Visible = false;

            string boxno = lblBoxNo.Text;

            try
            {
                if (string.IsNullOrEmpty(boxno))
                {
                    lblError.Text = "No MIS Box entered.";
                    divError.Visible = true;
                    return;
                }

                foreach (GridViewRow row in boxGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList list = (DropDownList)row.FindControl("ddlFileAction");
                        CheckBox chkRowAudited = (row.FindControl("checkBoxAudited") as CheckBox);

                        if (list.SelectedValue == SocpenFileActions.NONE && !chkRowAudited.Checked)
                        {
                            lblError.Text = "Files in this box must be marked as Audited or a file action must be specified before completing the box.";
                            divError.Visible = true;
                            divSuccess.Visible = false;
                            return;
                        }
                    }
                }

                var x = en.DC_FILE
                        .Where(f => f.MIS_BOXNO == boxno);

                foreach (DC_FILE file in x)
                {
                    file.MIS_BOX_STATUS = "Completed";
                }

                en.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Box Complete Update File"));
                en.SaveChanges();

                lblSuccess.Text = "MIS Box No " + boxno + " successfully completed.";
                lblError.Text = "";
                divSuccess.Visible = true;
                divError.Visible = false;
            }
            catch
            {
                lblError.Text = boxno + " could not be updated.";
                lblSuccess.Text = "";
                divSuccess.Visible = false;
                divError.Visible = true;
            }
        }

        protected void ddlFileAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList cb = (DropDownList)sender;

            string idNo = cb.Attributes["data-idNo"];
            string grantType = cb.Attributes["data-grant"];
            string misBox = cb.Attributes["data-misbox"];

            if (!string.IsNullOrEmpty(idNo) && !string.IsNullOrEmpty(grantType) && !string.IsNullOrEmpty(misBox))
            {
                updateAuditFileAction(idNo, grantType, misBox, cb.SelectedValue);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "updategrid", "UpdateGrid();", true);
        }

        protected void Load_MISBoxData()
        {
            divError.Visible = false;
            divSuccess.Visible = false;
            lblBoxNo.Text = string.Empty;
            lblError.Text = string.Empty;

            string myRegionID = UserSession.Office.RegionId;

            if (string.IsNullOrEmpty(myRegionID))
            {
                lblError.Text = "No region data found for logged in user.";
                divError.Visible = true;
                return;
            }

            //Get the box number from the text field
            string BoxNo = txtSearchBox.Text.ToUpper();
            if (string.IsNullOrEmpty(BoxNo))
            {
                lblError.Text = "Please enter/scan a Box Number into the field.";
                divError.Visible = true;
                return;
            }

            //Get the Registry type from the dropdown list
            string RegType = ddlRegistry_Type.SelectedValue;
            if (string.IsNullOrEmpty(RegType))
            {
                lblError.Text = "Please select the registry where this box is located.(BOX TYPE)";
                divError.Visible = true;
                return;
            }

            Dictionary<string, string> grantTypes = util.getGrantTypes();

            List<MISBoxFiles> boxFiles = new List<MISBoxFiles>();

            //If the box number is available, search MISNATIONAL + SOCPENID for the files

            using (Entities context = new Entities())
            {
                try
                {
                    try
                    {
                        var commandText = "";
                        if (myRegionID == "2")//Eastern Cape
                        {
                            commandText = $@"select DISTINCT
                                        mis.ID_NUMBER,
                                        (mis.GRANT_TYPE) AS GRANT_TYPE,
                                        (mis.FORM_TYPE)||(mis.FORM_NUMBER) AS FILE_NUMBER,
                                        DECODE(UPPER(mis.BOX_TYPE),
                                            'M', 'M',
                                            'A', 'A',
                                            'S', 'S',
                                            'NULL', null,
                                            null, null ) AS BOX_TYPE,
                                        mis.APPLICATION_DATE AS APP_DATE,
                                        mis.POSITN AS POSITION,
                                        rg.REGION_ID,
                                        rg.REGION_NAME as REGION_NAME,
                                        rg2.REGION_NAME as TRANSFER_REGION,
                                        f.UNQ_FILE_NO,
                                        f.BRM_BARCODE,
                                        f.MIS_BOX_DATE,
                                        f.BRM_BARCODE AS BRM_NO,
                                        f.APPLICATION_STATUS AS REGISTRY_TYPE,
                                        f.FILE_STATUS AS FILE_STATUS,
                                        f.MIS_BOX_STATUS,
                                        f.TDW_BOXNO AS TDW_BOXNO,
                                        f.MISSING AS MISSING,
                                        f.NON_COMPLIANT AS NON_COMPLIANT,
                                        spn.NAME,
                                        spn.SURNAME,
                                        spn.PROVINCE AS SOCPEN_PROVINCE,
                                        spn.GRANT_TYPE1,
                                        spn.GRANT_TYPE2,
                                        spn.GRANT_TYPE3,
                                        spn.GRANT_TYPE4,
                                        spn.APP_DATE1,
                                        spn.APP_DATE2,
                                        spn.APP_DATE3,
                                        spn.APP_DATE4,
                                        spn.PRIM_STATUS1,
                                        spn.PRIM_STATUS2,
                                        spn.PRIM_STATUS3,
                                        spn.PRIM_STATUS4,
                                        spn.SEC_STATUS1,
                                        spn.SEC_STATUS2,
                                        spn.SEC_STATUS3,
                                        spn.SEC_STATUS4,
                                        spn.STATUS_DATE1,
                                        spn.STATUS_DATE2,
                                        spn.STATUS_DATE3,
                                        spn.STATUS_DATE4,
                                        (f.CHILD_ID_NO) AS CHILD_ID_NO,
                                        NVL(f.PRINT_ORDER, -1) AS PRINT_ORDER
                                from CONTENTSERVER.SS_APPLICATION mis
                                inner join CONTENTSERVER.DC_REGION rg on '2' = rg.REGION_ID
                                left outer join CONTENTSERVER.DC_FILE f on f.FILE_NUMBER = (mis.FORM_TYPE || mis.FORM_NUMBER)
                                left outer join CONTENTSERVER.SOCPENBRM spn on TRIM(mis.ID_NUMBER) = spn.PENSION_NO
                                left outer join CONTENTSERVER.DC_REGION rg2 on rg2.REGION_ID = spn.PROVINCE
                                where mis.BOX = '{ BoxNo }'
                                and DECODE(UPPER(mis.BOX_TYPE),
                                            'M', 'M',
                                            'A', 'A',
                                            'S', 'S',
                                            'NULL', null,
                                            null, null ) = '{ RegType }'
                                { (ddlDistrict.SelectedValue == "" ? "" : $" and upper(mis.DISTRICT_OFFICE) = '{ ddlDistrict.SelectedValue }'") }
                                { (txtSearchArchYear.Text == "" ? "" : $" and mis.A_YEAR LIKE '%{ txtSearchArchYear.Text }%'") }
                                order by mis.POSITN, mis.ID_NUMBER";
                        }
                        else
                        {
                            commandText = $@"select DISTINCT
                                        mis.ID_NUMBER,
                                        mis.GRANT_TYPE AS GRANT_TYPE,
                                        mis.FILE_NUMBER,
                                        DECODE(mis.REGISTRY_TYPE,
                                            'Main File', 'M',
                                            'Main LC', 'M',
                                            'Archive File', 'A',
                                            'Archive LC', 'A',
                                            'Special', 'S',
                                            'Special LC', 'S',
                                            'NULL', null,
                                            null, null ) AS BOX_TYPE,
                                        mis.APP_DATE,
                                        mis.POSITION,
                                        rg.REGION_ID,
                                        rg.REGION_NAME as REGION_NAME,
                                        rg2.REGION_NAME as TRANSFER_REGION,
                                        f.UNQ_FILE_NO,
                                        f.BRM_BARCODE,
                                        f.MIS_BOX_DATE,
                                        f.BRM_BARCODE AS BRM_NO,
                                        f.APPLICATION_STATUS AS REGISTRY_TYPE,
                                        f.FILE_STATUS AS FILE_STATUS,
                                        f.MIS_BOX_STATUS,
                                        f.TDW_BOXNO AS TDW_BOXNO,
                                        f.MISSING AS MISSING,
                                        f.NON_COMPLIANT AS NON_COMPLIANT,
                                        spn.NAME,
                                        spn.SURNAME,
                                        spn.PROVINCE AS SOCPEN_PROVINCE,
                                        spn.GRANT_TYPE1,
                                        spn.GRANT_TYPE2,
                                        spn.GRANT_TYPE3,
                                        spn.GRANT_TYPE4,
                                        spn.APP_DATE1,
                                        spn.APP_DATE2,
                                        spn.APP_DATE3,
                                        spn.APP_DATE4,
                                        spn.PRIM_STATUS1,
                                        spn.PRIM_STATUS2,
                                        spn.PRIM_STATUS3,
                                        spn.PRIM_STATUS4,
                                        spn.SEC_STATUS1,
                                        spn.SEC_STATUS2,
                                        spn.SEC_STATUS3,
                                        spn.SEC_STATUS4,
                                        spn.STATUS_DATE1,
                                        spn.STATUS_DATE2,
                                        spn.STATUS_DATE3,
                                        spn.STATUS_DATE4,
                                        f.CHILD_ID_NO AS CHILD_ID_NO,
                                        NVL(f.PRINT_ORDER, -1) AS PRINT_ORDER
                                from CONTENTSERVER.MIS_LIVELINK_TBL mis
                                inner join CONTENTSERVER.DC_REGION rg on mis.REGION_ID = rg.REGION_ID
                                left outer join CONTENTSERVER.DC_FILE f on f.FILE_NUMBER = mis.FILE_NUMBER
                                left outer join CONTENTSERVER.SOCPENBRM spn on mis.ID_NUMBER = spn.PENSION_NO
                                left outer join CONTENTSERVER.DC_REGION rg2 on rg2.REGION_ID = spn.PROVINCE
                                where mis.BOX_NUMBER = '{ BoxNo }'
                                and DECODE(mis.REGISTRY_TYPE,
                                            'Main File', 'M',
                                            'Main LC', 'M',
                                            'Archive File', 'A',
                                            'Archive LC', 'A',
                                            'Special', 'S',
                                            'Special LC', 'S',
                                            'NULL', null,
                                            null, null ) = '{ RegType }'
                                and mis.REGION_ID = '{ myRegionID }'
                                { (txtBinNo.Text == "" ? "" : $" and mis.BIN_ID = '{ txtBinNo.Text }'") }
                                { (txtSearchArchYear.Text == "" ? "" : $" and mis.SUB_REGISTRY_TYPE LIKE '%{ txtSearchArchYear.Text }%'") }
                                order by mis.POSITION, mis.ID_NUMBER";
                        }

                        using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(context.Database.Connection.ConnectionString))
                        {
                            connection.Open();

                            var command = new Oracle.ManagedDataAccess.Client.OracleCommand
                            {
                                CommandText = commandText,
                                Connection = connection,
                                CommandTimeout = 45,
                                CommandType = CommandType.Text
                            };

                            var oracleDataReader = command.ExecuteReader();
                            if (oracleDataReader.HasRows)
                            {
                                while (oracleDataReader.Read())
                                {
                                    //0   ID_NUMBER,
                                    //1   GRANT_TYPE,
                                    //2   FILE_NUMBER,
                                    //3   BOX_TYPE,
                                    //4   APP_DATE,
                                    //5   POSITION,
                                    //6   REGION_ID,
                                    //7   REGION_NAME,
                                    //8   TRANSFER_REGION,
                                    //9   UNQ_FILE_NO,
                                    //10  BRM_BARCODE,
                                    //11  MIS_BOX_DATE,
                                    //12  BRM_NO,
                                    //13  REGISTRY_TYPE,
                                    //14  FILE_STATUS,
                                    //15  MIS_BOX_STATUS,
                                    //16  TDW_BOXNO,
                                    //17  MISSING,
                                    //18  NON_COMPLIANT,
                                    //19  NAME,
                                    //20  SURNAME,
                                    //21  SOCPEN_PROVINCE,
                                    //22  GRANT_TYPE1,
                                    //23  GRANT_TYPE2,
                                    //24  GRANT_TYPE3,
                                    //25  GRANT_TYPE4,
                                    //26  APP_DATE1,
                                    //27  APP_DATE2,
                                    //28  APP_DATE3,
                                    //29  APP_DATE4,
                                    //30  PRIM_STATUS1,
                                    //31  PRIM_STATUS2,
                                    //32  PRIM_STATUS3,
                                    //33  PRIM_STATUS4,
                                    //34  SEC_STATUS1,
                                    //35  SEC_STATUS2,
                                    //36  SEC_STATUS3,
                                    //37  SEC_STATUS4,
                                    //38  STATUS_DATE1,
                                    //39  STATUS_DATE2,
                                    //40  STATUS_DATE3,
                                    //41  STATUS_DATE4,
                                    //42  CHILD_ID_NO,
                                    //43  PRINT_ORDER
                                    var misBoxFile = new MISBoxFiles
                                    {
                                        ID_NUMBER = oracleDataReader.IsDBNull(0) ? string.Empty : oracleDataReader.GetString(0),
                                        GRANT_TYPE = oracleDataReader.IsDBNull(1) ? string.Empty : BoxFileActions.convertMISGrantTypeToSocpen(oracleDataReader.GetString(1)),
                                        FILE_NUMBER = oracleDataReader.IsDBNull(2) ? string.Empty : oracleDataReader.GetString(2),
                                        APP_DATE = oracleDataReader.IsDBNull(4) ? string.Empty : oracleDataReader.GetString(4),
                                        REGION_ID = oracleDataReader.IsDBNull(6) ? string.Empty : oracleDataReader.GetString(6),
                                        REGION_NAME = oracleDataReader.IsDBNull(7) ? string.Empty : oracleDataReader.GetString(7),
                                        TRANSFER_REGION = oracleDataReader.IsDBNull(8) ? string.Empty : oracleDataReader.GetString(8),
                                        UNQ_FILE_NO = oracleDataReader.IsDBNull(9) ? string.Empty : oracleDataReader.GetString(9),
                                        MIS_BOX_DATE = oracleDataReader.IsDBNull(11) ? DateTime.MinValue : oracleDataReader.GetDateTime(11),
                                        BRM_NO = oracleDataReader.IsDBNull(12) ? string.Empty : oracleDataReader.GetString(12),
                                        REGISTRY_TYPE = oracleDataReader.IsDBNull(13) ? string.Empty : oracleDataReader.GetString(13),
                                        FILE_STATUS = oracleDataReader.IsDBNull(14) ? string.Empty : oracleDataReader.GetString(14),
                                        MIS_BOX_STATUS = oracleDataReader.IsDBNull(15) ? string.Empty : oracleDataReader.GetString(15),
                                        TDW_BOXNO = oracleDataReader.IsDBNull(16) ? string.Empty : oracleDataReader.GetString(16),
                                        MISSING = oracleDataReader.IsDBNull(17) ? string.Empty : oracleDataReader.GetString(17),
                                        NON_COMPLIANT = oracleDataReader.IsDBNull(18) ? string.Empty : oracleDataReader.GetString(18),
                                        NAME = oracleDataReader.IsDBNull(19) ? string.Empty : oracleDataReader.GetString(19),
                                        SURNAME = oracleDataReader.IsDBNull(20) ? string.Empty : oracleDataReader.GetString(20),
                                        SOCPEN_PROVINCE = oracleDataReader.IsDBNull(21) ? string.Empty : oracleDataReader.GetString(21),
                                        GRANT_TYPE1 = oracleDataReader.IsDBNull(22) ? string.Empty : oracleDataReader.GetString(22),
                                        GRANT_TYPE2 = oracleDataReader.IsDBNull(23) ? string.Empty : oracleDataReader.GetString(23),
                                        GRANT_TYPE3 = oracleDataReader.IsDBNull(24) ? string.Empty : oracleDataReader.GetString(24),
                                        GRANT_TYPE4 = oracleDataReader.IsDBNull(25) ? string.Empty : oracleDataReader.GetString(25),
                                        APP_DATE1 = oracleDataReader.IsDBNull(26) ? string.Empty : oracleDataReader.GetString(26),
                                        APP_DATE2 = oracleDataReader.IsDBNull(27) ? string.Empty : oracleDataReader.GetString(27),
                                        APP_DATE3 = oracleDataReader.IsDBNull(28) ? string.Empty : oracleDataReader.GetString(28),
                                        APP_DATE4 = oracleDataReader.IsDBNull(29) ? string.Empty : oracleDataReader.GetString(29),
                                        PRIM_STATUS1 = oracleDataReader.IsDBNull(30) ? string.Empty : oracleDataReader.GetString(30),
                                        PRIM_STATUS2 = oracleDataReader.IsDBNull(31) ? string.Empty : oracleDataReader.GetString(31),
                                        PRIM_STATUS3 = oracleDataReader.IsDBNull(32) ? string.Empty : oracleDataReader.GetString(32),
                                        PRIM_STATUS4 = oracleDataReader.IsDBNull(33) ? string.Empty : oracleDataReader.GetString(33),
                                        SEC_STATUS1 = oracleDataReader.IsDBNull(34) ? string.Empty : oracleDataReader.GetString(34),
                                        SEC_STATUS2 = oracleDataReader.IsDBNull(35) ? string.Empty : oracleDataReader.GetString(35),
                                        SEC_STATUS3 = oracleDataReader.IsDBNull(36) ? string.Empty : oracleDataReader.GetString(36),
                                        SEC_STATUS4 = oracleDataReader.IsDBNull(37) ? string.Empty : oracleDataReader.GetString(37),
                                        STATUS_DATE1 = oracleDataReader.IsDBNull(38) ? string.Empty : oracleDataReader.GetString(38),
                                        STATUS_DATE2 = oracleDataReader.IsDBNull(39) ? string.Empty : oracleDataReader.GetString(39),
                                        STATUS_DATE3 = oracleDataReader.IsDBNull(40) ? string.Empty : oracleDataReader.GetString(40),
                                        STATUS_DATE4 = oracleDataReader.IsDBNull(41) ? string.Empty : oracleDataReader.GetString(41),
                                        CHILD_ID_NO = oracleDataReader.IsDBNull(42) ? string.Empty : oracleDataReader.GetString(42),
                                        PRINT_ORDER = oracleDataReader.IsDBNull(43) ? -1 : oracleDataReader.GetInt32(43)
                                    };

                                    boxFiles.Add(misBoxFile);
                                }
                            }
                        }

                        Parallel.ForEach(boxFiles, misBoxFile =>
                        {
                            misBoxFile.ARCHIVE_YEAR = string.Empty;
                            misBoxFile.REGISTRY_TYPE = string.Empty;
                            misBoxFile.GRANT_NAME = string.Empty;
                            misBoxFile.BOX_NUMBER = BoxNo;

                            misBoxFile.EXCLUSIONS = util.CheckExclusions(misBoxFile.ID_NUMBER);

                            misBoxFile.IsFound = !(misBoxFile.UNQ_FILE_NO == null || string.IsNullOrEmpty(misBoxFile.UNQ_FILE_NO) || misBoxFile.MISSING == "Y");
                            misBoxFile.MISSING = string.Empty;

                            misBoxFile.IsNonCompliant = misBoxFile.NON_COMPLIANT == "Y";
                            misBoxFile.NON_COMPLIANT = string.Empty;

                            misBoxFile.APP_DATE = Get_Grant_Date(misBoxFile.STATUS_DATE1, misBoxFile.STATUS_DATE2, misBoxFile.STATUS_DATE3, misBoxFile.STATUS_DATE4,
                                misBoxFile.GRANT_TYPE1, misBoxFile.GRANT_TYPE2, misBoxFile.GRANT_TYPE3, misBoxFile.GRANT_TYPE4,
                                misBoxFile.GRANT_TYPE);

                            //Only change value if not null or empty string
                            if (!string.IsNullOrEmpty(misBoxFile.TDW_BOXNO))
                            {
                                hfTDWBoxNo.Value = misBoxFile.TDW_BOXNO;
                            }

                            string registryType = Get_Account_Status(misBoxFile.PRIM_STATUS1, misBoxFile.PRIM_STATUS2,
                                misBoxFile.PRIM_STATUS3, misBoxFile.PRIM_STATUS4,
                                misBoxFile.SEC_STATUS1, misBoxFile.SEC_STATUS2,
                                misBoxFile.SEC_STATUS3, misBoxFile.SEC_STATUS4);

                            switch (registryType)
                            {
                                case "MAIN":
                                    {
                                        misBoxFile.REGISTRY_TYPE = "MAIN";
                                    }
                                    break;

                                case "ARCHIVE":
                                    {
                                        misBoxFile.REGISTRY_TYPE = "ARCHIVE";
                                        misBoxFile.ARCHIVE_YEAR = Get_ArchiveYear(misBoxFile.STATUS_DATE1, misBoxFile.STATUS_DATE2, misBoxFile.STATUS_DATE3, misBoxFile.STATUS_DATE4);
                                        if (misBoxFile.ARCHIVE_YEAR != null)
                                        {
                                            int destYear = Get_FiveYears_Later(misBoxFile.ARCHIVE_YEAR);
                                            int thisYear = DateTime.Now.Year; //CCYY
                                            if (destYear <= thisYear && String.IsNullOrEmpty(misBoxFile.EXCLUSIONS))
                                            {
                                                misBoxFile.REGISTRY_TYPE = "DESTROY";
                                            }
                                        }
                                        else
                                        {
                                            misBoxFile.ARCHIVE_YEAR = "0000";
                                        }
                                    }
                                    break;

                                default:
                                    {
                                        misBoxFile.REGISTRY_TYPE = string.Empty;
                                    }
                                    break;
                            }

                            string grantName = string.Empty;
                            if (grantTypes.TryGetValue(misBoxFile.GRANT_TYPE, out grantName))
                            {
                                misBoxFile.GRANT_NAME = grantName;
                            }
                        });

                        var resultSocpenChildStatus = new List<SOCPEN_CHILD_STATUS>();
                        var pensionNumbers = String.Join(",", boxFiles.Where(w => w.GRANT_TYPE == "C" && (w.ARCHIVE_YEAR == null || w.ARCHIVE_YEAR == "0000")).Select(s => s.ID_NUMBER));
                        using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(context.Database.Connection.ConnectionString))
                        {
                            connection.Open();

                            var command = new Oracle.ManagedDataAccess.Client.OracleCommand
                            {
                                CommandText = $"SELECT CAST(PENSION_NO AS VARCHAR2(30)) AS PENSION_NO, STATUS_DATE FROM SASSA.SOCPEN_P12_CHILDREN WHERE PENSION_NO IN ({pensionNumbers}) AND STATUS_CODE = '2'",
                                Connection = connection,
                                CommandTimeout = 15,
                                CommandType = CommandType.Text
                            };

                            var oracleDataReader = command.ExecuteReader();
                            if (oracleDataReader.HasRows)
                            {
                                while (oracleDataReader.Read())
                                {
                                    resultSocpenChildStatus.Add(new SOCPEN_CHILD_STATUS
                                    {
                                        PENSION_NO = oracleDataReader.GetString(0),
                                        STATUS_DATE = oracleDataReader.GetDateTime(1)
                                    });
                                }
                            }
                        }

                        Parallel.ForEach(boxFiles.Where(w => w.GRANT_TYPE == "C" && (w.ARCHIVE_YEAR == null || w.ARCHIVE_YEAR == "0000")), file =>
                        {
                            if (resultSocpenChildStatus.Select(s => s.PENSION_NO).Contains(file.ID_NUMBER.Trim()))
                            {
                                file.ARCHIVE_YEAR = resultSocpenChildStatus.First(f => f.PENSION_NO == file.ID_NUMBER.Trim()).STATUS_DATE.Year.ToString();
                            }
                        });
                    }
                    catch (Exception)
                    {
                        //lblError.Text = ex.Message;
                        //divError.Visible = true;
                    }

                    if (boxFiles.Count == 0)
                    {
                        lblError.Text = "No files were found for the box number";
                        divError.Visible = true;
                        boxGridView.DataSource = boxFiles;
                        boxGridView.DataBind();

                        lblResultCount.Text = "No of Files: 0";
                        return;
                    }

                    boxGridView.DataSource = boxFiles;
                    boxGridView.DataBind();

                    lblResultCount.Text = "No of Files: " + boxFiles.Count.ToString();
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                    divError.Visible = true;
                }
            }
        }

        protected void updateAuditFileAction(string idNo, string grantType, string misBox, string fileAction)
        {
            try
            {
                bool addNew = false;

                var x = en.DC_FILE
                        .Where(f => f.APPLICANT_NO == idNo && f.MIS_BOXNO == misBox && f.GRANT_TYPE == grantType).FirstOrDefault();

                //Add record to DC_FILE first if it doesn't exist.
                if (x == null && Session["MISBoxFiles"] != null)
                {
                    var boxFile = ((List<MISBoxFiles>)Session["MISBoxFiles"]).First(rec => rec.BOX_NUMBER == misBox && rec.ID_NUMBER == idNo && rec.GRANT_TYPE == grantType);

                    if (boxFile != null)
                    {
                        string localOffice = UserSession.Office.OfficeId;

                        x = new DC_FILE()
                        {
                            MIS_BOXNO = misBox,
                            APPLICANT_NO = boxFile.ID_NUMBER,
                            GRANT_TYPE = boxFile.GRANT_TYPE,
                            REGION_ID = boxFile.REGION_ID,
                            OFFICE_ID = localOffice, // Get office id from logged-in user.
                            USER_FIRSTNAME = boxFile.NAME,
                            USER_LASTNAME = boxFile.SURNAME
                        };

                        addNew = true;
                    }
                    else
                    {
                        return;
                    }
                }

                x.FILE_STATUS = fileAction != SocpenFileActions.NONE ? fileAction : string.Empty; //Don't add 'please select' option into table
                x.UPDATED_DATE = DateTime.Now;

                x.UPDATED_BY_AD = UserSession.SamName;


                if (addNew)
                {
                    en.DC_FILE.Add(x);
                }
                en.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Update Audit File"));
                en.SaveChanges();

                lblSuccess.Text = idNo + " successfully updated.";
                lblError.Text = "";
                divSuccess.Visible = true;
                divError.Visible = false;
            }
            catch (Exception ex)
            {
                lblError.Text = idNo + " could not be updated. " + ex.Message;
                lblSuccess.Text = "";
                divSuccess.Visible = false;
                divError.Visible = true;
            }
        }

        private bool CheckIfBoxPrinted()
        {
            foreach (GridViewRow r in boxGridView.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    if (r.Cells[1].Text == "&nbsp;" || r.Cells[1].Text == "")
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private class SOCPEN_CHILD_STATUS
        {
            #region Public Properties

            public string PENSION_NO { get; set; }

            public DateTime STATUS_DATE { get; set; }

            #endregion Public Properties
        }

        #endregion MIS BOX TAB METHODS

        #region REBOX TAB METHODS

        protected void updateFileReboxStatus(string BRMNo, string barcode, string boxType, string boxArchiveYear)//string MisBoxNo, , bool check, string CLMno
        {
            divReboxError.Visible = false;
            divReboxSuccess.Visible = false;

            try
            {
                var x = en.DC_FILE
                        .Where(f => f.BRM_BARCODE == BRMNo).FirstOrDefault();

                if (x == null)
                {
                    lblReboxError.Text = "BRM File Number " + BRMNo + " not recognised.";
                    divReboxError.Visible = true;
                    return;
                }

                //:Rolled back due to issues.
                //if (!string.IsNullOrEmpty(x.APPLICATION_STATUS))
                //{
                //    if (x.APPLICATION_STATUS.StartsWith("ARCHIVE") && ((int.Parse(boxType) > 18) || (int.Parse(boxType) < 14)))
                //    {
                //        lblReboxError.Text = "BRM File Status (" + x.APPLICATION_STATUS + ") does not match boxtype.";
                //        divReboxError.Visible = true;
                //        return;
                //    }
                //    if (!x.APPLICATION_STATUS.StartsWith("ARCHIVE") && ((int.Parse(boxType) < 19) || (int.Parse(boxType) > 13)))
                //    {
                //        lblReboxError.Text = "BRM File Status (" + x.APPLICATION_STATUS + ") does not match boxtype.";
                //        divReboxError.Visible = true;
                //        return;
                //    }
                //}

                x.MIS_REBOX_DATE = DateTime.Now;
                x.MIS_REBOX_STATUS = "Completed";
                x.TDW_BOXNO = barcode;
                x.TDW_BOX_TYPE_ID = decimal.Parse(boxType);
                x.TDW_BOX_ARCHIVE_YEAR = boxArchiveYear;

                //Get next Alternate number if necessary
                var y = en.DC_FILE
                    .Where(f => f.TDW_BOXNO == barcode && f.ALT_BOX_NO.Trim() != null).OrderByDescending(g => g.UPDATED_DATE).FirstOrDefault();

                if (y == null)
                {
                    using (Entities en2 = new Entities())
                    {
                        switch (UserSession.Office.RegionId)
                        {
                            case "1":
                                x.ALT_BOX_NO = "WCA" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_WCA.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "2":
                                x.ALT_BOX_NO = "ECA" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_ECA.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "3":
                                x.ALT_BOX_NO = "NCA" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_NCA.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "4":
                                x.ALT_BOX_NO = "FST" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_FST.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "5":
                                x.ALT_BOX_NO = "KZN" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_KZN.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "6":
                                x.ALT_BOX_NO = "NWP" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_NWP.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "7":
                                x.ALT_BOX_NO = "GAU" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_GAU.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "8":
                                x.ALT_BOX_NO = "MPU" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_MPU.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            case "9":
                                x.ALT_BOX_NO = "LIM" + en2.Database.SqlQuery<decimal>("select SEQ_DC_ALT_BOX_NO_LIM.NEXTVAL from DUAL ").FirstOrDefault().ToString();
                                break;

                            default:
                                break;
                        }
                    }
                }
                else
                {
                    x.ALT_BOX_NO = y.ALT_BOX_NO;
                }

                x.UPDATED_DATE = DateTime.Now;

                x.UPDATED_BY_AD = UserSession.SamName;

                en.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Box Rebox Update File"));
                en.SaveChanges();

                lblReboxSuccess.Text = "BRM File Number " + BRMNo + " successfully added to TDW box " + barcode + ".";
                divReboxSuccess.Visible = true;
            }
            catch
            {
                lblReboxError.Text = "BRM File Number " + BRMNo + " could not be added to TDW box " + barcode + ".";
                divReboxError.Visible = true;
            }
        }

        #endregion REBOX TAB METHODS

        #region PICKEDBOX METHODS

        // GRIDVIEW METHODS

        public IQueryable<MISBoxesPicked> FindPicklistBoxes(string x)
        {
            // TEMP
            string myUserID = "0"; //Session["CSUserID"].ToString();
            var thisuser = int.Parse(myUserID);

            string picklistno = util.getPickListForUser(thisuser);
            string regtype = util.getPickListRegistryType(picklistno);
            string archyear = util.getPickListAY(picklistno);

            txtPickListNo.Text = picklistno;
            txtArchYear.Text = archyear;

            switch (regtype)
            {
                case "M":
                    ddlRegistryType.SelectedIndex = 1;
                    divArchYear.Visible = false;
                    divAY1.Visible = false;
                    break;

                case "A":
                    ddlRegistryType.SelectedIndex = 2;
                    divArchYear.Visible = true;
                    divAY1.Visible = true;
                    break;

                default:
                    ddlRegistryType.SelectedIndex = 0;
                    divArchYear.Visible = false;
                    divAY1.Visible = false;
                    break;
            }

            if (picklistno != String.Empty)
            {
                IQueryable<MISBoxesPicked> query;
                try
                {
                    query =
                        from p in en.DC_BOXPICKED
                        where p.UNQ_PICKLIST == picklistno
                        orderby p.BIN_NUMBER, p.BOX_NUMBER
                        select new MISBoxesPicked
                        {
                            UNQ_PICKLIST = p.UNQ_PICKLIST,
                            UNQ_NO = p.UNQ_NO,
                            BIN_NUMBER = p.BIN_NUMBER,
                            BOX_NUMBER = p.BOX_NUMBER,
                            BOX_RECEIVED = p.BOX_RECEIVED,
                            BOX_COMPLETED = p.BOX_COMPLETED,
                            ARCHIVE_YEAR = p.ARCHIVE_YEAR
                        };

                    if (query.Any())
                    {
                        List<MISBoxesPicked> plb = new List<MISBoxesPicked>();
                        foreach (var item in query)
                        {
                            MISBoxesPicked p = item;
                            plb.Add(p);
                        }
                        btnPicklistCompleted.Visible = true;
                        return plb.AsQueryable();
                    }
                    else
                    {
                        btnPicklistCompleted.Visible = false;//change later
                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                btnPicklistCompleted.Visible = false;
                return null;
            }
        }

        protected void btnAddtoPicklist_Click(object sender, EventArgs e)
        {
            string picklistno = txtPickListNo.Text;
            string bin = txtBinNo.Text;
            string box = txtBoxPickedNo.Text;
            string archiveyear = txtArchYear.Text;
            string registry_type = ddlRegistryType.SelectedValue;

            if (box == "" || box == String.Empty)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "enterboxno", "alert('Please enter the Box No and try again.');", true);
            }
            else
            {
                if (registry_type == "M" || registry_type == "A")
                {
                    if ((registry_type == "A") && (archiveyear == String.Empty || archiveyear == null))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "enterArchYear", "alert('Please enter the Archive Year and try again.');", true);
                    }
                    else
                    {
                        string usedpicklistno = addBoxToPicklist(picklistno, bin, box, archiveyear, registry_type);
                        txtPickListNo.Text = usedpicklistno;
                        txtArchYear.Enabled = false;
                        ddlRegistryType.Enabled = false;
                        btnPicklistCompleted.Visible = true;

                        Label2.Text = "A box is added on the Picklist.";
                        div2.Visible = true;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "enterRegType", "alert('Please enter registry type and try again.');", true);
                }
            }
            this.PickedBoxGridView.DataBind();
            txtBinNo.Text = "";
            txtBoxPickedNo.Text = "";
            txtBoxPickedNo.Focus();
        }

        protected void btnSearchPickbox_Click(object sender, EventArgs e)
        {
            LoadPicklistBoxes();
        }

        protected void btnSearchPickedBoxes_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in PickedBoxGridView.Rows)
                {
                    LoadPicklistBoxes();
                    return;
                }
            }
            catch (Exception ex)
            {
                lblReboxError.Text = ex.Message;
            }
        }

        protected void LoadCachedPicklistBoxes()
        {
            if (Session["PickedBoxes"] != null)
            {
                PickedBoxGridView.DataSource = (List<MISBoxesPicked>)Session["PickedBoxes"];
                PickedBoxGridView.DataBind();
            }
            else
            {
                LoadPicklistBoxes();
            }
        }

        protected void LoadPicklistBoxes()
        {
            Session["PickedBoxes"] = null;
            showlastPicklistValues();

            List<MISBoxesPicked> boxes = new List<MISBoxesPicked>();

            using (Entities context = new Entities())
            {
                try
                {
                    IEnumerable<MISBoxesPicked> query = context.Database.SqlQuery<MISBoxesPicked>
                        (@"select b.UNQ_PICKLIST,
                                        b.UNQ_NO AS UNQ_NO,
                                        b.BIN_NUMBER AS BIN_NUMBER,
                                        b.BOX_NUMBER AS BOX_NUMBER,
                                        b.BOX_RECEIVED AS BOX_RECEIVED,
                                        b.BOX_COMPLETED AS BOX_COMPLETED,
                                        b.ARCHIVE_YEAR AS ARCHIVE_YEAR
                                from CONTENTSERVER.DC_BOXPICKED b
                                where b.UNQ_PICKLIST = '" + txtPickListNo.Text + "')");

                    if (query.Any())
                    {
                        //instead of just returning the IEnumerable as a list, go through each record and 'clean' it before displaying/using it.
                        foreach (MISBoxesPicked value in query.OrderBy(b => b.BOX_NUMBER))
                        {
                            MISBoxesPicked newFile = new MISBoxesPicked()
                            {
                                UNQ_PICKLIST = value.UNQ_PICKLIST,
                                UNQ_NO = value.UNQ_NO,
                                BOX_NUMBER = value.BOX_NUMBER,
                                BIN_NUMBER = value.BIN_NUMBER,
                                BOX_RECEIVED = value.BOX_RECEIVED,
                                BOX_COMPLETED = value.BOX_COMPLETED,
                                ARCHIVE_YEAR = value.ARCHIVE_YEAR
                            };
                            boxes.Add(newFile);
                        }
                        btnPicklistCompleted.Visible = true;
                    }
                    else
                    {
                        btnPicklistCompleted.Visible = false;
                        PickedBoxGridView.DataSource = boxes;
                        PickedBoxGridView.DataBind();
                        return;
                    }

                    PickedBoxGridView.DataSource = boxes;
                    PickedBoxGridView.DataBind();

                    Session["PickedBoxes"] = boxes;
                }
                catch (Exception)
                {
                }
            }
        }

        protected void PickedBoxGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PickedBoxGridView.PageIndex = e.NewPageIndex;
            LoadCachedPicklistBoxes();
        }

        protected void showlastPicklistValues()
        {
            string myUserID = "0";// Session["CSUserID"].ToString();
            var uid = int.Parse(myUserID);
            string picklistno = util.getPickListForUser(uid);
            string regtype = util.getPickListRegistryType(picklistno);
            string archyear = util.getPickListAY(picklistno);

            txtPickListNo.Text = picklistno;
            txtArchYear.Text = archyear;

            switch (regtype)
            {
                case "M":
                    ddlRegistryType.SelectedIndex = 1;
                    divArchYear.Visible = false;
                    divAY1.Visible = false;
                    break;

                case "A":
                    ddlRegistryType.SelectedIndex = 2;
                    divArchYear.Visible = true;
                    divAY1.Visible = true;
                    break;

                default:
                    ddlRegistryType.SelectedIndex = 0;
                    divArchYear.Visible = false;
                    divAY1.Visible = false;
                    break;
            }
        }

        private string addBoxToPicklist(string picklistno, string bin, string box, string archiveyear, string registry_type)
        {
            string usedpicklistno = string.Empty;
            string regionid = UserSession.Office.RegionId;

            if ((picklistno == null) || (picklistno == ""))
            {
                usedpicklistno = CreatePicklist(regionid, registry_type);
            }
            else
            {
                usedpicklistno = picklistno;
            }

            DC_BOXPICKED b = new DC_BOXPICKED();
            b.UNQ_NO = string.Empty;
            b.UNQ_PICKLIST = usedpicklistno;
            b.BIN_NUMBER = bin;
            b.BOX_NUMBER = box;
            b.BOX_RECEIVED = string.Empty;
            b.BOX_COMPLETED = string.Empty;
            b.ARCHIVE_YEAR = archiveyear;
            en.DC_BOXPICKED.Add(b);
            en.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Box Add To Picklist"));
            en.SaveChanges();
            return usedpicklistno;
        }

        #endregion PICKEDBOX METHODS

        #region PICKLIST METHODS

        public IQueryable<PickLists> FindPicklists()
        {
            try
            {
                string myUserID = "-1";//
                var x = int.Parse(myUserID);
                var query = en.DC_PICKLIST.Where(oid => oid.USERID == x)
                .OrderBy(pl => pl.UNQ_PICKLIST)
                .Select(
                    pl => new PickLists
                    {
                        UNQ_PICKLIST = pl.UNQ_PICKLIST,
                        REGION_ID = pl.REGION_ID,
                        REGISTRY_TYPE = pl.REGISTRY_TYPE,
                        PICKLIST_DATE = pl.PICKLIST_DATE,
                        PICKLIST_STATUS = pl.PICKLIST_STATUS,
                        NO_BOXES = "0"
                    });
                if (query.Any())
                {
                    List<PickLists> pls = new List<PickLists>();
                    foreach (var item in query)
                    {
                        if (item.REGISTRY_TYPE == "M")
                        {
                            item.REGISTRY_TYPE = "MAIN";
                        }
                        else
                        {
                            item.REGISTRY_TYPE = "ARCHIVE";
                        }
                        PickLists p = item;
                        p.NO_BOXES = util.getNoBoxesInPicklist(p.UNQ_PICKLIST);
                        pls.Add(p);
                    }
                    return pls.AsQueryable();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // button Picklist Complete
        protected void btnPicklistCompleted_Click(object sender, EventArgs e)
        {
            var picklistno = txtPickListNo.Text;
            DC_PICKLIST p = (from x in en.DC_PICKLIST
                             where x.UNQ_PICKLIST == picklistno
                             select x).First();
            p.PICKLIST_STATUS = "Y";
            en.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Picklist Completed"));
            en.SaveChanges();
            txtArchYear.Enabled = true;
            ddlRegistryType.Enabled = true;
            btnPicklistCompleted.Visible = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowBoxPickList", "ShowBoxPickList('" + picklistno + "');", true);
        }

        // link button on Picklist to View and print picklist
        protected void btnSearchPickLists_Click(object sender, EventArgs e)
        {
            PickedListGridView.SelectMethod = "FindPicklists";
        }

        protected void PickedListGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PickedListGridView.PageIndex = e.NewPageIndex;
        }

        private string CreatePicklist(string regionid, string registry_type)
        {
            //string myUserID = Session["CSUserID"].ToString();
            var uid = 0;

            DC_PICKLIST p = new DC_PICKLIST();
            p.UNQ_PICKLIST = string.Empty;
            p.REGION_ID = regionid;                 //"1";   //regionid;
            p.REGISTRY_TYPE = registry_type;        //"M";   //registry_type;
            p.USERID = uid;                         // 1000  // new SASSA_Authentication().getUserID();
            p.PICKLIST_DATE = DateTime.Now;
            p.PICKLIST_STATUS = "N";
            en.DC_PICKLIST.Add(p);
            try
            {
                en.DC_ACTIVITY.Add(util.CreateActivity("Boxing", "Create PickList"));
                en.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            string plist = util.getPickListForUser(uid);
            showlastPicklistValues();

            return plist;
        }

        #endregion PICKLIST METHODS
    }
}