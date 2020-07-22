using SASSADirectCapture.BL;
using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class DestructionExclusion : SassaPage
    {
        protected DestructionProcess dProcess;
        protected int RegionId;
        protected string UserName;

        protected void Page_Load(object sender, EventArgs e)
        {

            RegionId = int.Parse(Usersession.Office.RegionId);
            UserName = Usersession.Name;


            dProcess = new DestructionProcess(RegionId, UserName);

            if (!IsPostBack)
            {
                divError.Visible = false;
                ddExclusionType.DataSource = dProcess.dData.ExclusionTypes;
                ddExclusionType.DataBind();

                ddDestructionYears.DataSource = dProcess.dData.UndestroyedYears();
                ddDestructionYears.DataBind();
            }
            grdExclusions.DataSource = dProcess.dData.getExclusions(RegionId);
            grdExclusions.DataBind();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            string PensionNo = txtSearchID.Text;

            try
            {
                dProcess.AddNewExclusion(PensionNo, ddExclusionType.SelectedValue);
                divError.Visible = false;
                grdExclusions.DataSource = dProcess.dData.getExclusions(RegionId);
                grdExclusions.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
            grdExclusions.DataSource = dProcess.dData.getExclusions(RegionId);
            grdExclusions.DataBind();
        }
        protected void ddExclusionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //populate for missing files and Activity report
            if (ddExclusionType.SelectedValue == "") return;

        }

        protected void ddDestructionYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //populate for missing files and Activity report
            if (ddExclusionType.SelectedValue == "") return;

        }
        protected void grdExclusions_PageIndexChanging(object sender, EventArgs e)
        {

        }
        public IQueryable<DC_EXCLUSIONS> GetExclusions()
        {
            Entities en = new Entities();
            int regionId = int.Parse(Usersession.Office.RegionId);
            var x = en.DC_EXCLUSIONS.Where(e => e.REGION_ID == regionId && e.EXCLUSION_BATCH_ID == 0).OrderBy(f => f.EXCL_DATE);

            int recordcount = x.Count();

            if (recordcount > 0) { grdExclusions.PageSize = recordcount; }
            return x.AsQueryable();
        }



        protected void grdExclusions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdExclusions.PageIndex = e.NewPageIndex;
            grdExclusions.SelectMethod = "GetExclusions";
            //DoMethod();
        }

        //todo: sync file format
        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            if (ExclusionFile.FileContent.Length > 0)
            {
                string content;

                using (var reader = new StreamReader(ExclusionFile.FileContent))
                {
                    content = reader.ReadToEnd();
                }
                var rows = content.Replace("\r\n", "|").Split('|');
                List<DC_EXCLUSIONS> values = rows
                            .Skip(1)
                            .Select(v => DC_EXCLUSIONS.FromCsv(v, ddExclusionType.SelectedValue, RegionId, UserName))
                            .ToList();

                foreach (DC_EXCLUSIONS exclusion in values)
                {
                    dProcess.AddNewExclusion(exclusion.ID_NO, ddExclusionType.SelectedValue);
                }
                grdExclusions.DataSource = dProcess.dData.getExclusions(RegionId);
                grdExclusions.DataBind();

            }
        }
        protected void btnSubmit_Batch_Click(object sender, EventArgs e)
        {
            divError.Visible = false;
            try
            {
                int batchId = dProcess.dData.AddExclusionBatch(RegionId, UserName, ddDestructionYears.SelectedValue);
                dProcess.dData.UpdateExclusionBatch(batchId, RegionId);
                grdExclusions.DataSource = dProcess.dData.getExclusions(RegionId);
                grdExclusions.DataBind();
                ddDestructionYears.DataSource = dProcess.dData.UndestroyedYears();
                ddDestructionYears.DataBind();

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }


    }


}

