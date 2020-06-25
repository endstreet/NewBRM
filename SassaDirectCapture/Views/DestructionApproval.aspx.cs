using SASSADirectCapture.BL;
using SASSADirectCapture.Sassa;
using System;

namespace SASSADirectCapture.Views
{
    public partial class DestructionApproval : SassaPage
    {
        protected DestructionProcess dProcess;
        protected int RegionId;
        protected string UserName;
        protected string destructionYear;

        protected void Page_Load(object sender, EventArgs e)
        {

            RegionId = int.Parse(UserSession.Office.RegionId);
            UserName = UserSession.Name;

            dProcess = new DestructionProcess(RegionId, UserName);
            if (!IsPostBack)
            {

                ddDestructionYears.DataSource = dProcess.dData.DestructionYears;
                ddDestructionYears.DataBind();
                GridApproval.DataSource = dProcess.dData.GetExclusionBatches(ddDestructionYears.SelectedValue);
                GridApproval.DataBind();
                divError.Visible = false;
            }
        }

        protected void ddDestructionYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            destructionYear = ddDestructionYears.SelectedValue;
            GridApproval.DataSource = dProcess.dData.GetExclusionBatches(destructionYear);
            GridApproval.DataBind();
        }

        protected void btnApprove_Batch_Click(object sender, EventArgs e)
        {
            divError.Visible = false;
            try
            {
                int batchId = dProcess.dData.AddApprovalBatch(RegionId, UserName, ddDestructionYears.SelectedValue);
                if (batchId == 0) throw new System.Exception("Approval complete for this period.");
                //GridApproval.DataSource = dProcess.dData.GetExclusionBatches(destructionYear);
                //GridApproval.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
    }
}