using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class BoxContents : SassaPage
    {
        private Entities en = new Entities();
        private static List<FileEntity> boxFiles = new List<FileEntity>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Dont need to handle authentication as this is done on the master page load.
            if (!IsPostBack)
            {
                string BoxNo = Request.QueryString["BoxNo"].ToString();
                string RegID = Request.QueryString["RegID"].ToString();
                string RegName = Request.QueryString["RegName"].ToString();
                string Status = Request.QueryString["Status"].ToString();
                lblHead.Text = Status + " files of box " + BoxNo + " for " + RegName + " - Date:" + DateTime.Now.ToString();
                string missingStr = "GetBoxFilesMissing('" + BoxNo + "', '" + RegID + "', '" + Status + "')";
                string destroyStr = "GetBoxFilesDestroy('" + BoxNo + "', '" + RegID + "', '" + Status + "')";

                boxFiles = new List<FileEntity>();

                if (Status == "DESTROY")
                {
                    GetBoxFilesDestroy(BoxNo, RegID, Status);
                }
                else
                {
                    GetBoxFilesMissing(BoxNo, RegID, Status);
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the run time error "
            //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."
        }

        protected void GetBoxFilesDestroy(string BoxNo, string RegID, string Status)
        {
            //List<FileEntity> boxFiles = new List<FileEntity>();
            using (Entities context = new Entities())
            {
                try
                {
                    //IEnumerable<FileEntity> query = context.Database.SqlQuery<FileEntity>
                    IEnumerable<FileEntity> query;
                    if (RegID == "2")//Eastern Cape
                    {
                        query = context.Database.SqlQuery<FileEntity>
                        (@"select
                            mis.POSITN,
                            f.APPLICANT_NO,
                            f.UNQ_FILE_NO,
                            f.FILE_NUMBER,
                            f.BRM_BARCODE,
                            f.USER_FIRSTNAME AS FIRST_NAME,
                            f.USER_LASTNAME AS LAST_NAME,
                            f.GRANT_TYPE,
                            f.APPLICATION_STATUS,
                            f.ARCHIVE_YEAR,
                            f.EXCLUSIONS,
                            f.MIS_BOXNO,
                            f.MIS_BOX_STATUS,
                            f.MIS_REBOX_STATUS,
                            f.FILE_STATUS
                        from CONTENTSERVER.SS_APPLICATION mis
                        left outer join CONTENTSERVER.DC_FILE f on f.FILE_NUMBER = TRIM(mis.FORM_TYPE)||TRIM(mis.FORM_NUMBER)
                        where f.MIS_BOXNO = '" + BoxNo + "' and f.REGION_ID = '" + RegID + "' and f.APPLICATION_STATUS = 'DESTROY'"
                        + " order by cast(NVL(REPLACE(mis.POSITN, 'NULL', '0'), '0') as number) ");
                        //from DC_FILE
                        //    where MIS_BOXNO = '" + BoxNo + "' and REGION_ID = '" + RegID + "' and APPLICATION_STATUS = 'DESTROY'");
                    }
                    else
                    {
                        query = context.Database.SqlQuery<FileEntity>
                        (@"select
                            mis.POSITION,
                            f.APPLICANT_NO,
                            f.UNQ_FILE_NO,
                            f.FILE_NUMBER,
                            f.BRM_BARCODE,
                            f.USER_FIRSTNAME AS FIRST_NAME,
                            f.USER_LASTNAME AS LAST_NAME,
                            f.GRANT_TYPE,
                            f.APPLICATION_STATUS,
                            f.ARCHIVE_YEAR,
                            f.EXCLUSIONS,
                            f.MIS_BOXNO,
                            f.MIS_BOX_STATUS,
                            f.MIS_REBOX_STATUS,
                            f.FILE_STATUS
                        from CONTENTSERVER.MIS_LIVELINK_TBL mis
                        left outer join CONTENTSERVER.DC_FILE f on f.FILE_NUMBER = mis.FILE_NUMBER
                        where f.MIS_BOXNO = '" + BoxNo + "' and f.REGION_ID = '" + RegID + "' and f.APPLICATION_STATUS = 'DESTROY'"
                        + " order by cast(NVL(REPLACE(mis.POSITION, 'NULL', '0'), '0') as number) ");
                    }

                    if (query.Any())
                    {
                        foreach (FileEntity value in query)
                        {
                            FileEntity newFile = new FileEntity()
                            {
                                APPLICANT_NO = value.APPLICANT_NO,
                                UNQ_FILE_NO = value.UNQ_FILE_NO,
                                FILE_NUMBER = value.FILE_NUMBER,
                                BRM_BARCODE = value.BRM_BARCODE,
                                FIRST_NAME = value.FIRST_NAME,
                                LAST_NAME = value.LAST_NAME,
                                GRANT_TYPE = value.GRANT_TYPE,
                                GRANT_TYPE_NAME = util.getGrantName(value.GRANT_TYPE),
                                APPLICATION_STATUS = value.APPLICATION_STATUS,
                                ARCHIVE_YEAR = value.ARCHIVE_YEAR,
                                EXCLUSIONS = value.EXCLUSIONS,
                                MIS_BOXNO = value.MIS_BOXNO,
                                MIS_BOX_STATUS = value.MIS_BOX_STATUS,
                                MIS_REBOX_STATUS = value.MIS_REBOX_STATUS,
                                FILE_STATUS = value.FILE_STATUS
                            };
                            boxFiles.Add(newFile);
                        }
                        BoxlistGridView.DataSource = boxFiles;
                        BoxlistGridView.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "GetBoxFilesDestroy -" + ex.Message;
                }
            }
        }

        protected void GetBoxFilesMissing(string BoxNo, string RegID, string Status)
        {
            //List<FileEntity> boxFiles = new List<FileEntity>();

            using (Entities context = new Entities())
            {
                try
                {
                    //IEnumerable<FileEntity> query = context.Database.SqlQuery<FileEntity>
                    IEnumerable<FileEntity> query;
                    if (RegID == "2")//Eastern Cape
                    {
                        query = context.Database.SqlQuery<FileEntity>
                        (@"select
                            mis.POSITN as POSITION,
                            f.APPLICANT_NO,
                            f.UNQ_FILE_NO,
                            f.FILE_NUMBER,
                            f.BRM_BARCODE,
                            f.USER_FIRSTNAME AS FIRST_NAME,
                            f.USER_LASTNAME AS LAST_NAME,
                            f.GRANT_TYPE,
                            f.APPLICATION_STATUS,
                            f.ARCHIVE_YEAR,
                            f.EXCLUSIONS,
                            f.MIS_BOXNO,
                            f.MIS_BOX_STATUS,
                            f.MIS_REBOX_STATUS,
                            f.FILE_STATUS
                        from CONTENTSERVER.SS_APPLICATION mis
                        left outer join CONTENTSERVER.DC_FILE f on f.FILE_NUMBER = TRIM(mis.FORM_TYPE)||TRIM(mis.FORM_NUMBER)
                        where f.MIS_BOXNO = '" + BoxNo + "' and f.REGION_ID = '" + RegID + "' and f.MISSING = 'Y'"
                        + " order by cast(NVL(REPLACE(mis.POSITN, 'NULL', '0'), '0') as number) ");
                        //from DC_FILE
                        //where MIS_BOXNO = '" + BoxNo + "' and REGION_ID = '" + RegID + "' and MISSING = 'Y'");
                    }
                    else
                    {
                        query = context.Database.SqlQuery<FileEntity>
                        (@"select
                            mis.POSITION,
                            f.APPLICANT_NO,
                            f.UNQ_FILE_NO,
                            f.FILE_NUMBER,
                            f.BRM_BARCODE,
                            f.USER_FIRSTNAME AS FIRST_NAME,
                            f.USER_LASTNAME AS LAST_NAME,
                            f.GRANT_TYPE,
                            f.APPLICATION_STATUS,
                            f.ARCHIVE_YEAR,
                            f.EXCLUSIONS,
                            f.MIS_BOXNO,
                            f.MIS_BOX_STATUS,
                            f.MIS_REBOX_STATUS,
                            f.FILE_STATUS
                        from CONTENTSERVER.MIS_LIVELINK_TBL mis
                        left outer join CONTENTSERVER.DC_FILE f on f.FILE_NUMBER = mis.FILE_NUMBER
                        where f.MIS_BOXNO = '" + BoxNo + "' and f.REGION_ID = '" + RegID + "' and f.MISSING = 'Y'"
                        + " order by cast(NVL(REPLACE(mis.POSITION, 'NULL', '0'), '0') as number) ");
                    }

                    //if (query.Any())
                    //{
                    foreach (FileEntity value in query)
                    {
                        FileEntity newFile = new FileEntity()
                        {
                            APPLICANT_NO = value.APPLICANT_NO,
                            UNQ_FILE_NO = value.UNQ_FILE_NO,
                            FILE_NUMBER = value.FILE_NUMBER,
                            BRM_BARCODE = value.BRM_BARCODE,
                            FIRST_NAME = value.FIRST_NAME,
                            LAST_NAME = value.LAST_NAME,
                            GRANT_TYPE = value.GRANT_TYPE,
                            GRANT_TYPE_NAME = util.getGrantName(value.GRANT_TYPE),
                            APPLICATION_STATUS = value.APPLICATION_STATUS,
                            ARCHIVE_YEAR = value.ARCHIVE_YEAR,
                            EXCLUSIONS = value.EXCLUSIONS,
                            MIS_BOXNO = value.MIS_BOXNO,
                            MIS_BOX_STATUS = value.MIS_BOX_STATUS,
                            MIS_REBOX_STATUS = value.MIS_REBOX_STATUS,
                            FILE_STATUS = value.FILE_STATUS
                        };
                        boxFiles.Add(newFile);
                    }
                    //}
                    //else
                    if (boxFiles.Count == 0)
                    {
                        lblError.Text = "No missing files were found";
                    }
                    BoxlistGridView.DataSource = boxFiles;
                    BoxlistGridView.DataBind();
                }
                catch (Exception ex)
                {
                    lblError.Text = "GetBoxFilesMissing -" + ex.Message;
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "print", "window.print();window.close();", true);
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            string BoxNo = Request.QueryString["BoxNo"].ToString();
            string Status = Request.QueryString["Status"].ToString();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Box_" + BoxNo + "_" + Status + "_Files.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                BoxlistGridView.AllowPaging = false;
                BoxlistGridView.DataSource = boxFiles;
                BoxlistGridView.DataBind();

                BoxlistGridView.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in BoxlistGridView.HeaderRow.Cells)
                {
                    cell.BackColor = BoxlistGridView.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in BoxlistGridView.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = BoxlistGridView.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = BoxlistGridView.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                BoxlistGridView.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        protected void BoxlistGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BoxlistGridView.PageIndex = e.NewPageIndex;
            BoxlistGridView.DataSource = boxFiles;
            BoxlistGridView.DataBind();
        }
    }
}