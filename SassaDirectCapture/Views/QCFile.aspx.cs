using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class QCFile : SassaPage
    {

        #region Private Fields

        private string _schema = "";


        private Entities en = new Entities();

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
            lblError.Text = "";
            divError.Visible = false;

            if (txtBRM.Text != null)
            {
                SearchFiles();
            }
            else
            {
                lblError.Text = "BRM File number not entered or scanned.";
                divError.Visible = true;
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            string fileId = cb.Attributes["fileId"];
            string brmFileId = cb.Attributes["brmFileId"];

            if (string.IsNullOrEmpty(brmFileId))
            {
                return;
            }

            updateFileQC(brmFileId, fileId, cb.Checked);
        }


        protected void txtBRM_TextChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;

            if (txtBRM.Text != null)
            {
                SearchFiles();
            }
            else
            {
                lblError.Text = "BRM File number not entered or scanned.";
                divError.Visible = true;
            }
        }

        protected void updateFileQC(string brmFileId, string fileId, bool check)
        {
            try
            {
                var x = en.DC_FILE.Where(f => f.BRM_BARCODE == brmFileId && f.UNQ_FILE_NO == fileId).FirstOrDefault();

                if (x == null)
                {
                    return;
                }

                if (check)
                {
                    x.NON_COMPLIANT = "N";
                    x.QC_USER_FN = "0";
                    x.QC_USER_LN = Usersession.Name;
                    x.QC_DATE = DateTime.Now;
                }
                else
                {
                    x.NON_COMPLIANT = "Y";
                    x.QC_USER_FN = "0";
                    x.QC_USER_LN = Usersession.Name;
                    x.QC_DATE = DateTime.Now;
                }

                x.UPDATED_DATE = DateTime.Now;

                x.UPDATED_BY_AD = Usersession.SamName;

                en.DC_ACTIVITY.Add(util.CreateActivity("QCFile", "Update File QC"));
                en.SaveChanges();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateCheckBox(true);", true);
                lblError.Text = "";
                divError.Visible = false;
                lblSuccess.Text = brmFileId + " successfully updated.";
                divSuccess.Visible = true;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "reportBack", "UpdateCheckBox(false);", true);
                lblError.Text = brmFileId + " could not be updated.";
                divError.Visible = true;
                lblSuccess.Text = "";
                divSuccess.Visible = false;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private IQueryable<FileEntity> GetAllFilesByBRM()
        {
            if (txtBRM.Text != null)
            {
                var x = en.DC_FILE
                    .Where(bn => bn.BRM_BARCODE == txtBRM.Text)
                    .OrderBy(f => f.UNQ_FILE_NO)
                    .Select(f => new FileEntity
                    {
                        APPLICANT_NO = f.APPLICANT_NO,
                        CLM_UNIQUE_CODE = f.UNQ_FILE_NO,
                        REGION_NAME = f.DC_LOCAL_OFFICE.DC_REGION.REGION_NAME,
                        FIRST_NAME = f.USER_FIRSTNAME,
                        LAST_NAME = f.USER_LASTNAME,
                        GRANT_TYPE_NAME = f.DC_GRANT_TYPE.TYPE_NAME,
                        BRM_BARCODE = f.BRM_BARCODE,
                        FILE_COMMENT = f.FILE_COMMENT,
                        NON_COMPLIANT = f.NON_COMPLIANT,
                        QC_USER_FN = f.QC_USER_FN,
                        QC_USER_LN = f.QC_USER_LN,
                        UNQ_FILE_NO = f.UNQ_FILE_NO,
                        QC_DATE = f.QC_DATE,
                        TDW_BOXNO = f.TDW_BOXNO,
                        GRANT_TYPE = f.GRANT_TYPE,
                        APP_DATE_DT = f.TRANS_DATE,
                        SRD_NO = f.SRD_NO
                    });
                ;

                List<FileEntity> fe = new List<FileEntity>();
                foreach (var item in x)
                {
                    FileEntity f = item;
                    fe.Add(f);
                }

                return fe.AsQueryable();
            }
            else
            {
                return null;
            }
        }

        private void SearchFiles()
        {
            lblError.Text = "";
            divError.Visible = false;
            lblSuccess.Text = "";
            divSuccess.Visible = false;

            using (Entities context = new Entities())
            {
                using (DataTable DT = new DataTable())
                {
                    DT.Columns.Add("UNQ_FILE_NO", typeof(string));
                    DT.Columns.Add("CLM_UNIQUE_CODE", typeof(string));
                    DT.Columns.Add("REGION_NAME", typeof(string));
                    DT.Columns.Add("FULL_NAME", typeof(string));
                    DT.Columns.Add("GRANT_TYPE_NAME", typeof(string));
                    DT.Columns.Add("FILE_COMMENT", typeof(string));
                    //DT.Columns.Add("TRANS_TYPE", typeof(string));
                    DT.Columns.Add("FILE_STATUS_COMPLETED", typeof(bool));
                    DT.Columns.Add("BRM_BARCODE", typeof(string));
                    DT.Columns.Add("NON_COMPLIANT", typeof(string));
                    DT.Columns.Add("QC_USER_FN", typeof(string));
                    DT.Columns.Add("QC_USER_LN", typeof(string));
                    DT.Columns.Add("QC_DATE", typeof(string));
                    DT.Columns.Add("TDW_BOXNO", typeof(string));
                    DT.Columns.Add("PENSION_NO", typeof(string));
                    DT.Columns.Add("GRANT_TYPE", typeof(string));
                    DT.Columns.Add("AppDate", typeof(string));
                    DT.Columns.Add("SRD_NO", typeof(string));
                    DT.Columns.Add("CHILD_ID_NO", typeof(string));

                    try
                    {
                        if (txtBRM.Text != null)
                        {
                            var query = GetAllFilesByBRM();

                            //if (query.Any())
                            //{
                            foreach (FileEntity value in query.OrderBy(x => x.UNQ_FILE_NO))
                            {
                                DataRow dr = DT.NewRow();
                                dr["UNQ_FILE_NO"] = value.UNQ_FILE_NO;
                                dr["CLM_UNIQUE_CODE"] = value.CLM_UNIQUE_CODE;
                                dr["REGION_NAME"] = value.REGION_NAME;
                                dr["FULL_NAME"] = value.FULL_NAME;
                                dr["GRANT_TYPE_NAME"] = value.GRANT_TYPE_NAME;
                                //dr["TRANS_TYPE"] = value.TRANS_TYPE.ToString();
                                dr["FILE_COMMENT"] = value.FILE_COMMENT;
                                dr["FILE_STATUS_COMPLETED"] = value.FILE_STATUS_COMPLETED;
                                dr["BRM_BARCODE"] = value.BRM_BARCODE;
                                dr["NON_COMPLIANT"] = value.NON_COMPLIANT;
                                dr["QC_USER_FN"] = value.QC_USER_FN;
                                dr["QC_USER_LN"] = value.QC_USER_LN;
                                dr["QC_DATE"] = value.QC_DATE.ToString();
                                dr["TDW_BOXNO"] = value.TDW_BOXNO.ToString();
                                dr["PENSION_NO"] = value.APPLICANT_NO;
                                dr["GRANT_TYPE"] = value.GRANT_TYPE;
                                dr["AppDate"] = value.APP_DATE_DT.HasValue ? value.APP_DATE_DT.Value.ToString("yyyy/MM/dd") : "";
                                dr["SRD_NO"] = value.SRD_NO;
                                dr["CHILD_ID_NO"] = value.CHILD_ID_NO;

                                DT.Rows.Add(dr);
                            }
                            //}
                        }

                        if (DT.Rows.Count == 0)
                        {
                            lblError.Text = "No results were found for the search values";
                            divError.Visible = true;
                            fileGridView.DataSource = DT;
                            fileGridView.DataBind();
                            return;
                        }

                        fileGridView.DataSource = DT;
                        fileGridView.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        divError.Visible = true;
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
