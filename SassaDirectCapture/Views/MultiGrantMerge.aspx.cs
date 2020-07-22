using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class MultiGrantMerge : SassaPage
    {

        #region Protected Methods

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Close", "window.close();", true);
        }

        protected void btnMerge_Click(object sender, EventArgs e)
        {
            try
            {
                //Refresh grid, to pull in BRM numbers before attempting to merge.
                SearchApplicant();

                string sPensionNrValue = Request.QueryString["pensionNo"];
                string sGrantType = Request.QueryString["gt"];
                string sChildID = Request.QueryString["ChildID"] == null ? "" : Request.QueryString["ChildID"];
                string sParentBRMBarcode = "";

                using (Entities context = new Entities())
                {
                    DC_FILE f = context.DC_FILE.Where(k => k.APPLICANT_NO == sPensionNrValue
                                                          && k.GRANT_TYPE == sGrantType
                                                          && ((sGrantType != "C" && sGrantType != "5") || (sChildID == null ? k.CHILD_ID_NO == null : k.CHILD_ID_NO == sChildID))
                                                      )
                                                      .FirstOrDefault();

                    sParentBRMBarcode = f.BRM_BARCODE;
                }

                using (Entities context = new Entities())
                {
                    foreach (GridViewRow gvrRow in gvResults.Rows)
                    {
                        string brmBarcode = ((HiddenField)gvrRow.Cells[0].FindControl("hfBRMNo")).Value;//gvrRow.Cells[5].Text;

                        DC_MERGE merge = context.DC_MERGE.Where(k => k.BRM_BARCODE == brmBarcode).FirstOrDefault();

                        if (merge == null)
                        {
                            DC_MERGE newMerge = new DC_MERGE();
                            newMerge.BRM_BARCODE = brmBarcode;
                            newMerge.PARENT_BRM_BARCODE = sParentBRMBarcode;

                            context.DC_MERGE.Add(newMerge);
                        }
                        else
                        {
                            merge.PARENT_BRM_BARCODE = sParentBRMBarcode;
                        }
                    }
                    context.DC_ACTIVITY.Add(util.CreateActivity("Merge", "Multi grants"));
                    context.SaveChanges();
                }

                ClientScript.RegisterStartupScript(Page.GetType(), "MessageThenClose", "alert('Multi-grant files merged successfully.'); window.close();", true);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            SearchApplicant();
        }

        /// <summary>
        /// Enables the Merge button if all items in the list have BRM numbers.
        /// </summary>
        protected void EnableMergeButtonOrNot()
        {
            bool bAllPrepared = true;
            foreach (GridViewRow gvrRow in gvResults.Rows)
            {
                string brmBarcode = ((HiddenField)gvrRow.Cells[0].FindControl("hfBRMNo")).Value;

                if (brmBarcode == "")
                {
                    bAllPrepared = false;
                }
            }

            btnMerge.Enabled = bAllPrepared;
        }

        protected string formatAppDate(string appdate, string separator)
        {
            string myYYYY = appdate.Substring(0, 4);
            string myMM = appdate.Substring(4, 2);
            string myDD = appdate.Substring(6, 2);

            return myYYYY + separator + myMM + separator + myDD;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //// Get the username of the user that is logged in from session.
                //string authenticatedUsername = this.authObject.AuthenticateCSAdminUser();

                ////If no session values are found, redirect to the login screen
                //if (authenticatedUsername == string.Empty)
                //{
                //    authObject.RedirectToLoginPage();
                //}

                SearchApplicant();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void SearchApplicant()
        {
            using (Entities context = new Entities())
            {
                using (DataTable DT = new DataTable())
                {
                    DT.Columns.Add("Pension_no", typeof(string));
                    DT.Columns.Add("CHILD_ID_NO", typeof(string));
                    DT.Columns.Add("Name", typeof(string));
                    DT.Columns.Add("Surname", typeof(string));
                    DT.Columns.Add("Region", typeof(string));
                    DT.Columns.Add("GrantType", typeof(string));
                    DT.Columns.Add("AppDate", typeof(string));
                    DT.Columns.Add("GRANT_TYPE", typeof(string));
                    DT.Columns.Add("PRIM_STATUS", typeof(string));
                    DT.Columns.Add("SEC_STATUS", typeof(string));
                    DT.Columns.Add("SOC_STATUS", typeof(string));
                    DT.Columns.Add("BRM_BARCODE", typeof(string));
                    DT.Columns.Add("IsRMC", typeof(string));

                    try
                    {
                        string sPensionNrValue = Request.QueryString["pensionNo"];
                        string sAppDate = Request.QueryString["appdate"].Replace("/", "");

                        IEnumerable<ApplicantGrants> query = context.Database.SqlQuery<ApplicantGrants>
                                                    (@"select spn.PENSION_NO as APPLICANT_NO,
                                                                spn.NAME as FIRSTNAME,
                                                                spn.SURNAME as LASTNAME,
                                                                spn.GRANT_TYPE,
                                                                spn.APP_DATE,
                                                                rg.REGION_NAME as REGION_NAME,
                                                                spn.PRIM_STATUS,
                                                                spn.SEC_STATUS,
                                                                c.APPLICATION_DATE as C_APPLICATION_DATE,
                                                                c.STATUS_CODE as C_STATUS_CODE,
                                                                to_char(c.ID_NO) as CHILD_ID_NO
                                                        from SOCPENGRANTS spn
                                                        inner join DC_REGION rg on spn.PROVINCE = rg.REGION_ID
                                                        left join SASSA.SOCPEN_P12_CHILDREN c on spn.GRANT_TYPE in ('C', '5') and c.PENSION_NO = spn.PENSION_NO and c.GRANT_TYPE = spn.GRANT_TYPE
                                                        where '" + sPensionNrValue + @"' in (spn.PENSION_NO, spn.OLD_ID1, spn.OLD_ID2, spn.OLD_ID3, spn.OLD_ID4, spn.OLD_ID5, spn.OLD_ID6, spn.OLD_ID7, spn.OLD_ID8, spn.OLD_ID9, spn.OLD_ID10)
                                                        and (
                                                                (c.PENSION_NO is null AND spn.APP_DATE = '" + sAppDate + @"')
                                                                OR
                                                                (c.PENSION_NO is not null AND c.APPLICATION_DATE = to_date('" + sAppDate + @"', 'YYYYMMDD'))
                                                            )
                                                        order by NVL(c.STATUS_DATE, to_date(spn.STATUS_DATE, 'YYYYMMDD')) desc");
                        //order by spn.STATUS_DATE desc, c.STATUS_DATE desc");

                        Dictionary<string, string> dictGrantTypes = util.getGrantTypes();

                        foreach (ApplicantGrants value in query.OrderBy(x => x.APPLICANT_NO))
                        {
                            DataRow dr = DT.NewRow();
                            dr["Pension_no"] = value.APPLICANT_NO == null ? "" : value.APPLICANT_NO.Trim();
                            dr["CHILD_ID_NO"] = value.CHILD_ID_NO == null ? "" : value.CHILD_ID_NO.Trim();
                            dr["Name"] = lblName.Text = value.FIRSTNAME == null ? "" : value.FIRSTNAME.Trim();
                            dr["Surname"] = lblSurname.Text = value.LASTNAME == null ? "" : value.LASTNAME.Trim();
                            dr["Region"] = lblRegion.Text = value.REGION_NAME;
                            dr["GRANT_TYPE"] = value.GRANT_TYPE;
                            dr["PRIM_STATUS"] = value.PRIM_STATUS;
                            dr["SEC_STATUS"] = value.SEC_STATUS;

                            switch (value.C_STATUS_CODE)
                            {
                                case null:
                                    string socstatus = (value.PRIM_STATUS + value.SEC_STATUS).ToUpper();
                                    if (socstatus == "A2" || socstatus == "92" || socstatus == "B2")
                                    {
                                        dr["SOC_STATUS"] = "ACTIVE";
                                    }
                                    else
                                    {
                                        dr["SOC_STATUS"] = "INACTIVE";
                                    }
                                    break;

                                case "1":
                                    dr["SOC_STATUS"] = "ACTIVE";
                                    break;

                                default:
                                    dr["SOC_STATUS"] = "INACTIVE";
                                    break;
                            }

                            string latestGrantType = string.Empty;
                            latestGrantType = value.GRANT_TYPE.Trim();
                            dr["AppDate"] = value.C_APPLICATION_DATE == null ? (value.APP_DATE == null ? "" : formatAppDate(value.APP_DATE, "/")) : ((DateTime)value.C_APPLICATION_DATE).ToString("yyyy/MM/dd");

                            if (dictGrantTypes.ContainsKey(latestGrantType))
                            {
                                dr["GrantType"] = dictGrantTypes[latestGrantType];
                            }
                            else
                            {
                                dr["GrantType"] = value.GRANT_NAME;
                            }

                            //If BRM_BARCODE exists in DC_FILE, file is already prepped.
                            DC_FILE f = context.DC_FILE.Where(k => k.APPLICANT_NO == sPensionNrValue
                                                                && k.GRANT_TYPE == value.GRANT_TYPE
                                                                && ((value.GRANT_TYPE != "C" && value.GRANT_TYPE != "5") || (value.CHILD_ID_NO == null ? k.CHILD_ID_NO == null : k.CHILD_ID_NO == value.CHILD_ID_NO))
                                                             )
                                        .FirstOrDefault();

                            dr["BRM_BARCODE"] = (f == null ? "" : f.BRM_BARCODE);
                            dr["IsRMC"] = Usersession.Office.OfficeType == "RMC" ? "Y" : "N";

                            DT.Rows.Add(dr);
                        }
                        if (DT.Rows.Count == 0)
                        {
                            lblMsg.Text = "No results were found for the search value";
                            divError.Visible = true;
                            gvResults.DataSource = DT;
                            gvResults.DataBind();
                            return;
                        }

                        gvResults.DataSource = DT;
                        gvResults.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = ex.Message;
                        divError.Visible = true;
                    }
                }
            }

            EnableMergeButtonOrNot();
        }

        #endregion Private Methods
    }
}