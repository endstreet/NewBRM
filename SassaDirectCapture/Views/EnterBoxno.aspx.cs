using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Linq;
using System.Web.UI;

namespace SASSADirectCapture.Views
{
    public partial class EnterBoxno : SassaPage
    {
        private Entities en = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            //ddlBoxType.Focus();
        }

        protected void txtBoxBarcode_TextChanged(object sender, EventArgs e)
        {
            //if (txtBoxType.Text == "")
            if (ddlBoxType.SelectedValue == "")
            {
                lblPickBoxType.Visible = true;
            }
            else
            {
                lblPickBoxType.Visible = false;

                // get the last Archive year for this box no if not main.

                btnUpdateBox_Click(sender, e);
            }
        }

        protected void btnUpdateBox_Click(object sender, EventArgs e)
        {
            if (ddlBoxType.SelectedIndex != 0)
            {
                int AY;

                if (ddlBoxType.SelectedIndex != 1 && ddlBoxType.SelectedIndex != 3 && (txtArchYear.Text == "" || txtArchYear.Text.Length != 4 || !(int.TryParse(txtArchYear.Text, out AY))))
                {
                    lblAYwrong.Visible = true;
                }
                else
                {
                    lblAYwrong.Visible = false;

                    string bc = txtBoxBarcode.Text.Trim().ToUpper();
                    string bt = ddlBoxType.SelectedItem.Text;//txtBoxType.Text;
                    string btID = ddlBoxType.SelectedValue;
                    string ay = txtArchYear.Text;

                    if (bc.Length < 3)
                    {
                        lblTooShort.Visible = true;
                        ClientScript.RegisterStartupScript(Page.GetType(), "ignore", "alert('Please scan or enter the Box Barcode');", true);
                    }
                    else
                    {
                        using (Entities context = new Entities())
                        {
                            DC_FILE file = context.DC_FILE.Where(f => f.TDW_BOXNO == bc).OrderByDescending(g => g.UPDATED_DATE).FirstOrDefault();

                            if (file != null && file.TDW_BOXNO != null && (file.TDW_BOX_TYPE_ID != (btID == "" ? (Decimal?)null : Decimal.Parse(btID)) || (ddlBoxType.SelectedIndex != 1 && ddlBoxType.SelectedIndex != 3 && ay != file.TDW_BOX_ARCHIVE_YEAR)))
                            {
                                ClientScript.RegisterStartupScript(Page.GetType(), "ignore", "alert('Please note that the box is already in use as a " + util.getBoxTypes().First(x => x.Value == file.TDW_BOX_TYPE_ID).Key + (file.TDW_BOX_TYPE_ID != 1 && file.TDW_BOX_TYPE_ID != 13 ? (" box for Archive Year " + file.TDW_BOX_ARCHIVE_YEAR) : "") + ".');", true);
                            }
                            else
                            {
                                Session["BoxNo"] = bc;
                                Session["BoxType"] = bt;
                                Session["BoxTypeID"] = btID;
                                Session["ArchiveYear"] = ay;
                                //ClientScript.RegisterStartupScript(Page.GetType(), "getboxno", "window.opener.document.getElementById('txtBoxNo').value='" + bc + "';", true);
                                //ClientScript.RegisterStartupScript(Page.GetType(), "getboxtype", "window.opener.document.getElementById('txtBoxType').value='" + bt + "';", true);
                                //ClientScript.RegisterStartupScript(Page.GetType(), "getarchyear", "window.opener.document.getElementById('txtAYear').value='" + ay + "';", true);
                                ClientScript.RegisterStartupScript(Page.GetType(), "setboxno", "window.opener.location.reload();", true);
                                ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close();", true);
                            }
                        }
                    }
                }
                lblPickBoxType.Visible = false;
            }
            else
            {
                lblPickBoxType.Visible = true;
                lblAYwrong.Visible = false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "close", "window.close();", true);
        }

        protected void ddlBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBoxType.SelectedIndex == 0)
            {
                lblPickBoxType.Visible = true;
            }
            else
            {
                lblPickBoxType.Visible = false;
            }

            if (ddlBoxType.SelectedIndex == 1 || (ddlBoxType.SelectedIndex == 3))
            {
                //txtArchYear.Enabled = false;
                txtArchYear.Visible = false;
                lblAY.Visible = false;
                txtArchYear.Text = "";
            }
            else
            {
                if ((ddlBoxType.SelectedIndex == 2) || (ddlBoxType.SelectedIndex == 4))
                {
                    //txtArchYear.Enabled = true;
                    txtArchYear.Visible = true;
                    lblAY.Visible = true;
                    getBoxArchiveYear();
                }
            }
        }

        protected void getBoxArchiveYear()
        {
            string bbc = txtBoxBarcode.Text;
            string ayear = string.Empty;

            if (bbc != "")
            {
                ayear = util.getBoxArchYear(bbc);

                //if (ayear == null)
                //{
                //    //txtArchYear.Enabled = true;
                //    txtArchYear.Visible = true;
                //    lblAY.Visible = true;
                //}
                //else
                //{
                //    //txtArchYear.Enabled = false;
                //    txtArchYear.Visible = false;
                //    lblAY.Visible = false;
                //}
            }
            else
            {
                txtBoxBarcode.Focus();
            }
        }

        protected void txtArchYear_TextChanged(object sender, EventArgs e)
        {
            int AY;
            if ((txtArchYear.Text != "") && (txtArchYear.Text.Length == 4) && (int.TryParse(txtArchYear.Text, out AY)) && (ddlBoxType.SelectedIndex != 0) && (ddlBoxType.SelectedIndex != 1) && (ddlBoxType.SelectedIndex != 3))
            {
                lblAYwrong.Visible = false;
            }
            else
            {
                lblAYwrong.Visible = true;
            }
        }
    }
}