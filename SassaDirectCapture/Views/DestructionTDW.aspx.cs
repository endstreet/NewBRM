using SASSADirectCapture.BL;
using SASSADirectCapture.Sassa;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class DestructionTDW : SassaPage
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
                GridApproved.DataSource = dProcess.dData.GetApprovedBatches();
                GridApproved.DataBind();
                divError.Visible = false;
            }
        }

        protected void GridApproved_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void CommandBtn_Click(object sender, GridViewCommandEventArgs e)
        {
            int batchId = (int)GridApproved.DataKeys[int.Parse(e.CommandArgument.ToString())].Value;
            switch (e.CommandName)
            {

                case "Download":

                    divError.Visible = true;
                    string Filename = string.Format("{0} - Destruction.csv", batchId);
                    lblError.Text = "Downloading file :" + Filename;
                    string csv = dProcess.dData.getTDWcsv(batchId);


                    //Download the CSV file.
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + Filename);
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(csv);
                    Response.Flush();
                    Response.End();
                    divError.Visible = false;
                    break;


                default:
                    divError.Visible = false;
                    break;

            }

            GridApproved.DataSource = dProcess.dData.GetApprovedBatches();
            GridApproved.DataBind();
        }

        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            divError.Visible = false;
            if (CompletedFile.FileContent.Length > 0)
            {

                string content;

                using (var reader = new StreamReader(CompletedFile.FileContent))
                {
                    content = reader.ReadToEnd();
                }
                var rows = content.Replace("\r\n", "|").Split('|');
                string newstatus = rows.First();
                switch (newstatus.ToLower())
                {
                    case "destroyed":
                        newstatus = "Destroyed";
                        break;
                    case "tdwnotfound":
                        newstatus = "TDWNotFound";
                        break;
                }
                try
                {

                }
                catch
                {
                    lblError.Text = " An error occured processing your file/ Please verify it integrity and retry.";
                    divError.Visible = true;
                }
                foreach (string pension in rows.Skip(1))
                {
                    dProcess.dData.UpdateDestructionStatus(pension.Trim(), newstatus);
                }

            }
        }
    }
}