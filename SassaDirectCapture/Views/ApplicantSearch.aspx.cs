using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

//---------------

namespace SASSADirectCapture.Views
{
    public partial class ApplicantSearch : SassaPage
    {
        #region Public Fields

        #endregion Public Fields

        #region Private Fields

        private string _schema = "";

        #endregion Private Fields

        #region Private Properties

        private string schema
        {
            get
            {
                if (_schema == string.Empty)
                    _schema = System.Configuration.ConfigurationManager.AppSettings["schema"].ToString();
                return _schema;
            }
        }

        #endregion Private Properties

        #region Protected Methods

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if ((ddlServiceType.SelectedIndex > 0)&&(ddlTransactionType.SelectedIndex > 0))
            //{
            //    divError.Visible = false;
            SearchApplicant();
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingMsg", "myECM.hidePleaseWait();", true);
            //    lblMsg.Text = "Please select a transaction type";
            //    divError.Visible = true;
            //}
        }

        protected string formatAppDate(string appdate, string separator)
        {
            string myYYYY = appdate.Substring(0, 4);
            string myMM = appdate.Substring(4, 2);
            string myDD = appdate.Substring(6, 2);

            return myYYYY + separator + myMM + separator + myDD;
        }

        protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResults.PageIndex = e.NewPageIndex;
            SearchApplicant();
        }

        protected void gvResultsSRD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResultsSRD.PageIndex = e.NewPageIndex;
            SearchApplicant();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void rbID_CheckedChanged(object sender, EventArgs e)
        {
            divSearchID.Style.Add("display", "");
            divSearchSRD.Style.Add("display", "none");

            gvResults.DataSource = null;
            gvResults.DataBind();
            gvResultsSRD.DataSource = null;
            gvResultsSRD.DataBind();
        }

        protected void rbSRD_CheckedChanged(object sender, EventArgs e)
        {
            divSearchID.Style.Add("display", "none");
            divSearchSRD.Style.Add("display", "");

            gvResults.DataSource = null;
            gvResults.DataBind();
            gvResultsSRD.DataSource = null;
            gvResultsSRD.DataBind();
        }

        #endregion Protected Methods

        #region Private Methods

        private void SearchApplicant()
        {
            //This is to close the "Loading" popup on the screen when results are returned
            ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingMsg", "myECM.hidePleaseWait();", true);

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
                    DT.Columns.Add("STATUS_DATE", typeof(string));
                    DT.Columns.Add("SRD_NO", typeof(string));
                    DT.Columns.Add("BRM_BARCODE", typeof(string));
                    DT.Columns.Add("CLM", typeof(string));
                    DT.Columns.Add("IsRMC", typeof(string));

                    // wishlist:
                    //DT.Columns.Add("Status", typeof(string));

                    try
                    {
                        //Searching on ID
                        if (rbID.Checked)
                        {
                            string pensionNrValue = txtSearch.Text;
                            if (string.IsNullOrEmpty(pensionNrValue))
                            {
                                lblMsg.Text = "Please enter a search value";
                                divError.Visible = true;
                                return;
                            }

                            //object descending = null;
                            IEnumerable<ApplicantGrants> query = context.Database.SqlQuery<ApplicantGrants>
                                                        (@"select spn.PENSION_NO as APPLICANT_NO,
                                                                   spn.NAME as FIRSTNAME,
                                                                   spn.SURNAME as LASTNAME,
                                                                   spn.GRANT_TYPE,
                                                                   spn.APP_DATE,
                                                                   rg.REGION_ID as REGION_ID,
                                                                   rg.REGION_CODE as REGION_CODE,
                                                                   rg.REGION_NAME as REGION_NAME,
                                                                   spn.APP_DATE as TRANS_DATE,
                                                                   spn.PRIM_STATUS,
                                                                   spn.SEC_STATUS,
                                                                   spn.STATUS_DATE,
                                                                   c.APPLICATION_DATE as C_APPLICATION_DATE,
                                                                   c.STATUS_CODE as C_STATUS_CODE,
                                                                   c.STATUS_DATE as C_STATUS_DATE,
                                                                   to_char(c.ID_NO) as CHILD_ID_NO
                                                            from SOCPENGRANTS spn
                                                            inner join DC_REGION rg on spn.PROVINCE = rg.REGION_ID
                                                            left join SASSA.SOCPEN_P12_CHILDREN c on spn.GRANT_TYPE in ('C', '5') and c.PENSION_NO = spn.PENSION_NO and c.GRANT_TYPE = spn.GRANT_TYPE
                                                           -- where '" + pensionNrValue + @"' in (spn.PENSION_NO, spn.OLD_ID1, spn.OLD_ID2, spn.OLD_ID3, spn.OLD_ID4, spn.OLD_ID5, spn.OLD_ID6, spn.OLD_ID7, spn.OLD_ID8, spn.OLD_ID9, spn.OLD_ID10)
                                                            where spn.PENSION_NO  = '" + pensionNrValue + @"'
                                                            /*or spn.OLD_ID1 = '" + pensionNrValue + @"' or
                                                            spn.OLD_ID2  = '" + pensionNrValue + @"' or
                                                            spn.OLD_ID3  = '" + pensionNrValue + @"' or
                                                            spn.OLD_ID4  = '" + pensionNrValue + @"' or
                                                            spn.OLD_ID5  = '" + pensionNrValue + @"' or
                                                            spn.OLD_ID6  = '" + pensionNrValue + @"' or
                                                            spn.OLD_ID7  = '" + pensionNrValue + @"' or
                                                            spn.OLD_ID8  = '" + pensionNrValue + @"' or
                                                            spn.OLD_ID9  = '" + pensionNrValue + @"' or
                                                            spn.OLD_ID10  = '" + pensionNrValue + @"'*/
                                                            order by NVL(c.STATUS_DATE, to_date(spn.STATUS_DATE, 'YYYYMMDD')) desc");
                            //order by spn.STATUS_DATE desc, c.STATUS_DATE desc");
                            //where spn.PENSION_NO = '" + pensionNrValue + "' order by spn.APP_DATE desc");
                            //spn.GRANT_TYPE = 'C

                            //if (query.Any())
                            //{
                            //    if (query.Count() >= 100)
                            //    {
                            //        lblMsg.Text = "More than 100 possible matches were found.  Please change the search value to limit the number of results.";
                            //        divError.Visible = true;
                            //    }
                            Dictionary<string, string> dictGrantTypes = util.getGrantTypes();

                            foreach (ApplicantGrants value in query.OrderBy(x => x.APPLICANT_NO))
                            {
                                DataRow dr = DT.NewRow();
                                dr["Pension_no"] = value.APPLICANT_NO == null ? "" : value.APPLICANT_NO.Trim();
                                dr["CHILD_ID_NO"] = value.CHILD_ID_NO == null ? "" : value.CHILD_ID_NO.Trim();
                                dr["Name"] = value.FIRSTNAME == null ? "" : value.FIRSTNAME.Trim();
                                dr["Surname"] = value.LASTNAME == null ? "" : value.LASTNAME.Trim();
                                dr["Region"] = value.REGION_NAME;
                                dr["GRANT_TYPE"] = value.GRANT_TYPE;
                                dr["PRIM_STATUS"] = value.PRIM_STATUS;
                                dr["SEC_STATUS"] = value.SEC_STATUS;
                                //string socstatus = (value.PRIM_STATUS + value.SEC_STATUS).ToUpper();
                                //dr["SOC_STATUS"] = "INACTIVE";

                                //if (socstatus == "A2" || socstatus == "92" || socstatus == "B2")
                                //{
                                //    dr["SOC_STATUS"] = "ACTIVE";
                                //}

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

                                ////WORK OUT THE CORRECT GRANT TYPE TO USE BASED ON THE RUBBISH DATA IN SOCPENID.
                                //int latestAppDate = 0;
                                string latestGrantType = string.Empty;
                                latestGrantType = value.GRANT_TYPE.Trim();
                                //latestAppDate = int.Parse(value.APP_DATE);

                                //dr["AppDate"] = formatAppDate(latestAppDate.ToString(),"/"); //Convert.ToDateTime(.ToString("yyyyMMdd"));
                                //dr["AppDate"] = value.APP_DATE == null ? "" : formatAppDate(value.APP_DATE, "/");
                                dr["AppDate"] = value.C_APPLICATION_DATE == null ? (value.APP_DATE == null ? "" : formatAppDate(value.APP_DATE, "/")) : ((DateTime)value.C_APPLICATION_DATE).ToString("yyyy/MM/dd");
                                //dr["STATUS_DATE"] = formatAppDate(int.Parse(value.STATUS_DATE).ToString(), "/");
                                //dr["STATUS_DATE"] = value.STATUS_DATE == null ? "" : formatAppDate(value.STATUS_DATE, "/");
                                dr["STATUS_DATE"] = value.C_STATUS_DATE == null ? (value.STATUS_DATE == null ? "" : formatAppDate(value.STATUS_DATE, "/")) : ((DateTime)value.C_STATUS_DATE).ToString("yyyy/MM/dd");

                                if (dictGrantTypes.ContainsKey(latestGrantType))
                                {
                                    dr["GrantType"] = dictGrantTypes[latestGrantType];
                                }
                                else
                                {
                                    dr["GrantType"] = value.GRANT_NAME;
                                }

                                //If BRM_BARCODE exists in DC_FILE, file is already prepped.
                                DC_FILE f = context.DC_FILE.Where(k => k.APPLICANT_NO == pensionNrValue
                                                                    && k.GRANT_TYPE == value.GRANT_TYPE
                                                                    && ((value.GRANT_TYPE != "C" && value.GRANT_TYPE != "5") || (value.CHILD_ID_NO == null ? k.CHILD_ID_NO == null : k.CHILD_ID_NO == value.CHILD_ID_NO))
                                                                 )
                                            .FirstOrDefault();

                                dr["BRM_BARCODE"] = (f == null ? "" : f.BRM_BARCODE);
                                dr["CLM"] = (f == null ? "" : f.UNQ_FILE_NO);

                                dr["IsRMC"] = Usersession.Office.OfficeType == "RMC" ? "Y" : "N";

                                DT.Rows.Add(dr);
                            }
                            //}
                            //else
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
                        else
                        {
                            //Searching on SRD
                            string SRDNo = txtSearchSRD.Text;
                            if (string.IsNullOrEmpty(SRDNo))
                            {
                                lblMsg.Text = "Please enter a search value";
                                divError.Visible = true;
                                return;
                            }

                            IEnumerable<SocpenSRD> query = context.Database.SqlQuery<SocpenSRD>
                                                        (@"select srdben.ID_NO as APPLICANT_NO,
                                                                  srdben.NAME as FIRSTNAME,
                                                                  srdben.SURNAME as LASTNAME,
                                                                  srdtype.APPLICATION_DATE as APP_DATE,
                                                                  'S' as GRANT_TYPE,
                                                                  rg.REGION_ID as REGION_ID,
                                                                  rg.REGION_CODE as REGION_CODE,
                                                                  rg.REGION_NAME as REGION_NAME,
                                                                  srdtype.APPLICATION_DATE as TRANS_DATE,
                                                                  srdtype.APPLICATION_STATUS as APPLICATION_STATUS
                                                            from SASSA.SOCPEN_SRD_BEN srdben
                                                            left join SASSA.SOCPEN_SRD_TYPE srdtype on srdtype.SOCIAL_RELIEF_NO = srdben.SRD_NO
                                                            inner join DC_REGION rg on rg.REGION_ID = cast(srdben.PROVINCE as NUMBER)
                                                            where cast(srdben.SRD_NO as NUMBER) = cast('" + SRDNo + @"' as NUMBER)
                                                            order by srdtype.APPLICATION_DATE desc");

                            foreach (SocpenSRD value in query.OrderBy(x => x.APPLICANT_NO))
                            {
                                DataRow dr = DT.NewRow();
                                dr["Pension_no"] = value.APPLICANT_NO;
                                dr["Name"] = value.FIRSTNAME == null ? "" : value.FIRSTNAME.Trim();
                                dr["Surname"] = value.LASTNAME == null ? "" : value.LASTNAME.Trim();
                                dr["Region"] = value.REGION_NAME;
                                dr["GRANT_TYPE"] = value.GRANT_TYPE;
                                dr["GrantType"] = "SRD";
                                string socstatus = value.APPLICATION_STATUS == null ? "" : (value.APPLICATION_STATUS).ToUpper();
                                dr["SOC_STATUS"] = "INACTIVE";

                                if (socstatus == "2")
                                {
                                    dr["SOC_STATUS"] = "ACTIVE";
                                }

                                dr["AppDate"] = value.APP_DATE == null ? "" : ((DateTime)value.APP_DATE).ToString("yyyy/MM/dd");

                                string sID = value.APPLICANT_NO.ToString();

                                //If BRM_BARCODE exists in DC_FILE, file is already prepped.
                                DC_FILE f = context.DC_FILE.Where(k => k.SRD_NO == SRDNo).FirstOrDefault();

                                dr["SRD_NO"] = SRDNo;

                                dr["BRM_BARCODE"] = (f == null ? "" : f.BRM_BARCODE);
                                dr["CLM"] = (f == null ? "" : f.UNQ_FILE_NO);

                                dr["IsRMC"] = Usersession.Office.OfficeType == "RMC" ? "Y" : "N";

                                DT.Rows.Add(dr);
                            }

                            if (DT.Rows.Count == 0)
                            {
                                lblMsg.Text = "No results were found for the search value";
                                divError.Visible = true;
                                gvResultsSRD.DataSource = DT;
                                gvResultsSRD.DataBind();
                                return;
                            }

                            gvResultsSRD.DataSource = DT;
                            gvResultsSRD.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = ex.Message;
                        divError.Visible = true;
                    }
                }
            }
        }

        #endregion Private Methods

        //protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(ddlServiceType.SelectedValue))
        //    {
        //        ddlTransactionType.Enabled = true;
        //        ddlTransactionType.DataSource = util.getTransactionTypesByService(ddlServiceType.SelectedValue);
        //        ddlTransactionType.DataBind();
        //    }
        //    else
        //    {
        //        ddlTransactionType.Enabled = false;
        //    }
        //    ddlTransactionType.SelectedIndex = 0;
        //}
    }
}
