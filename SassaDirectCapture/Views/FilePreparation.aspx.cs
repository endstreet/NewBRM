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
    public partial class FilePreparation : SassaPage
    {

        private string _schema = "";

        private string schema
        {
            get
            {
                if (_schema == string.Empty)
                    _schema = System.Configuration.ConfigurationManager.AppSettings["schema"].ToString();
                return _schema;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Dont need to handle authentication as this is done on the master page load.
            if (!Page.IsPostBack)
            {
                //ddlTransactionType.DataSource = util.getTransactionTypes();
                //ddlTransactionType.DataBind();

                //if (ddlTransactionType.Items.Count > 0)
                //{
                //    ddlTransactionType.SelectedValue = "1";
                //}
            }
        }

        private void SearchApplicant()
        {
            //This is to close the "Loading" popup on the screen when results are returned
            ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingMsg", "myECM.hidePleaseWait();", true);

            using (Entities context = new Entities())
            {
                using (DataTable DT = new DataTable())
                {
                    DT.Columns.Add("Pension_no", typeof(string));
                    DT.Columns.Add("Name", typeof(string));
                    DT.Columns.Add("Surname", typeof(string));
                    DT.Columns.Add("Region", typeof(string));
                    DT.Columns.Add("GrantType", typeof(string));
                    DT.Columns.Add("AppDate", typeof(string));
                    DT.Columns.Add("GRANT_TYPE", typeof(string));
                    // wishlist:
                    //DT.Columns.Add("Status", typeof(string));

                    string pensionNrValue = txtSearch.Text;
                    if (string.IsNullOrEmpty(pensionNrValue))
                    {
                        lblMsg.Text = "Please enter a search value";
                        divError.Visible = true;
                        return;
                    }

                    try
                    {
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
                                                               spn.APP_DATE as TRANS_DATE
                                                        from SOCPENGRANTS spn
                                                        inner join DC_REGION rg on spn.PROVINCE = rg.REGION_ID
                                                        where '" + pensionNrValue + @"' in (spn.PENSION_NO, spn.OLD_ID1, spn.OLD_ID2, spn.OLD_ID3, spn.OLD_ID4, spn.OLD_ID5, spn.OLD_ID6, spn.OLD_ID7, spn.OLD_ID8, spn.OLD_ID9, spn.OLD_ID10)
                                                        order by spn.APP_DATE desc");
                        //where spn.PENSION_NO = '" + pensionNrValue + "' order by spn.APP_DATE desc");

                        if (query.Any())
                        {
                            if (query.Count() >= 100)
                            {
                                lblMsg.Text = "More than 100 possible matches were found.  Please change the search value to limit the number of results.";
                                divError.Visible = true;
                            }

                            Dictionary<string, string> dictGrantTypes = util.getGrantTypes();

                            foreach (ApplicantGrants value in query.OrderBy(x => x.APPLICANT_NO))
                            {
                                DataRow dr = DT.NewRow();
                                dr["Pension_no"] = value.APPLICANT_NO.Trim();
                                dr["Name"] = value.FIRSTNAME.Trim();
                                dr["Surname"] = value.LASTNAME.Trim();
                                dr["Region"] = value.REGION_NAME;
                                dr["GRANT_TYPE"] = value.GRANT_TYPE;
                                //wishlist:
                                //dr["STATUS"] = value.STATUS;

                                //WORK OUT THE CORRECT GRANT TYPE TO USE BASED ON THE RUBBISH DATA IN SOCPENID.
                                int latestAppDate = 0;
                                string latestGrantType = string.Empty;
                                latestGrantType = value.GRANT_TYPE.Trim();
                                latestAppDate = int.Parse(value.APP_DATE);

                                dr["AppDate"] = latestAppDate;

                                if (dictGrantTypes.ContainsKey(latestGrantType))
                                {
                                    dr["GrantType"] = dictGrantTypes[latestGrantType];
                                }
                                else
                                {
                                    dr["GrantType"] = value.GRANT_NAME;
                                }

                                DT.Rows.Add(dr);
                            }
                        }
                        else
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
        }

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    if ((ddlServiceType.SelectedIndex > 0)&&(ddlTransactionType.SelectedIndex > 0))
        //    {
        //        divError.Visible = false;
        //        SearchApplicant();
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingMsg", "myECM.hidePleaseWait();", true);
        //        lblMsg.Text = "Please select a transaction type";
        //        divError.Visible = true;
        //    }
        //}

        protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResults.PageIndex = e.NewPageIndex;
            SearchApplicant();
        }

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