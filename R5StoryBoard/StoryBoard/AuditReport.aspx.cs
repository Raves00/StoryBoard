using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace StoryBoard
{
    public partial class AuditReport : System.Web.UI.Page
    {
        #region Global Varibales

        String startdate;
        String enddate;
        int moduleid = 0;
        int pageid = 0;
        int elementid = 0;

        #endregion

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["ActiveTab"] = "";
                BindStoryBoardPages();
            }
        }

        #endregion

        #region Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form[txtFromDate.UniqueID].Length > 0 && Request.Form[txtToDate.UniqueID].Length > 0)
                {
                    String fromdate = DateTime.Parse(Request.Form[txtFromDate.UniqueID]).ToString("yyyy-MM-dd");
                    String todate = DateTime.Parse(Request.Form[txtToDate.UniqueID]).ToString("yyyy-MM-dd");
                    PopulateDetails(fromdate, todate, Convert.ToInt32(ddlSearchModule.SelectedValue), Convert.ToInt32(ddlSearchPage.SelectedValue), elementid);
                    pnlSearchResult.Visible = true;
                    txtFromDate.Text = Request.Form[txtFromDate.UniqueID];
                    txtToDate.Text = Request.Form[txtToDate.UniqueID];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlSearchModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindStoryBoardPages();
        }

        private void BindStoryBoardPages()
        {
            DataTable dtPagesForModules = DataMaster.GetPagesForModule(Convert.ToInt32(ddlSearchModule.SelectedValue));
            ddlSearchPage.Items.Clear();
            ddlSearchPage.DataSource = dtPagesForModules;
            ddlSearchPage.DataBind();
            ddlSearchPage.Items.Insert(0, (new ListItem("--Select--", "-1")));
        }

        protected void gvDE_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = ((System.Data.DataRowView)(e.Row.DataItem));
                if (Convert.ToString(dr.Row["Action"]).Equals("Add"))
                {
                    //gvDE.Columns[3].Visible = false;
                    //gvDE.Columns[4].Visible = false;
                    //gvDE.Columns[5].Visible = true;
                    //gvDE.Columns[6].Visible = true;
                }
            }
        }

        #endregion

        #region Private Methods

        private void PopulateDetails(String startdate, String enddate, int moduleid, int pageid, int elementid)
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        using (SqlCommand cmd = new SqlCommand("GETAUDITINFO", sqlconn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@STARTDATE", startdate);
                            cmd.Parameters.AddWithValue("@ENDDATE", enddate);
                            cmd.Parameters.AddWithValue("@MODULEID", moduleid);
                            cmd.Parameters.AddWithValue("@PAGEID", pageid);
                            cmd.Parameters.AddWithValue("@ELEMENTID", elementid);
                            sda.SelectCommand = cmd;
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            PopulateSSPDetails(ds);
                            PopulateWPDetails(ds);
                            PopulateDEDetails(ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PopulateSSPDetails(DataSet ds)
        {
            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "ModuleText='Self Service Portal'";
            //gvSSP.DataSource = dv.ToTable();
            //gvSSP.DataBind();
            lblSSPCount.Text = "Count : " + Convert.ToString(dv.ToTable().Rows.Count);
            ViewState["ActiveTab"] = "SSP";

            StringBuilder strSSP = new StringBuilder();

            foreach (DataRow dr in dv.ToTable(true, "Page Name").Rows)
            {
                //int count = 0;
                //dv = null;
                //dv = ds.Tables[0].DefaultView;
                //dv.RowFilter = "[Page Name]='" + Convert.ToString(dr["Page Name"]) + "'";
                //count = dv.ToTable().Rows.Count;
                strSSP.Append("<div class='collapsible' style='margin-left:40px;width:90%'>  &nbsp;&nbsp;&nbsp;" + Convert.ToString(dr["Page Name"]) + "<span></span>");
                //strSSP.Append(" <div style='float: right; padding: 0;width:500px;'> " + "Pages :  @#$" + " </div> </div>");
                strSSP.Append(" <div style='float: right; padding: 0;width:500px;'> </div> </div>");
                strSSP.Append("  <div> <ul style='list-style: none; margin-left: -40px;'><li>");
                strSSP.Append("<table width='100%' style='width:91% !important;margin-left:44px' class='table'>");
                //strSSP.Append("<tr><th align='left' valign='top'>Page Name </th>");
                strSSP.Append("<th align='left' valign='top'>Column Name </th>");
                strSSP.Append("<th align='left' valign='top'>Old Value </th>");
                strSSP.Append("<th align='left' valign='top'>New Value </th>");
                strSSP.Append("<th align='left' valign='top'>Action </th>");
                strSSP.Append("<th align='left' valign='top'>Updated By </th>");
                strSSP.Append("<th align='left' valign='top'>Updated On </th></tr>");

                foreach (DataRow drSub in dv.ToTable().Rows)
                {
                    if (Convert.ToString(dr["Page Name"]).Equals(Convert.ToString(drSub["Page Name"]), StringComparison.OrdinalIgnoreCase))
                    {
                        strSSP.Append("<tr>");
                        //strSSP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["Page Name"]) + "</td>");
                        strSSP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["Column Name"]) + "</td>");
                        strSSP.Append("<td align='left' valign='top'>" + Convert.ToString(TruncateAtWord(drSub["Initial Value"].ToString(), 100)) + "</td>");
                        strSSP.Append("<td align='left' valign='top'>" + Convert.ToString(TruncateAtWord(drSub["Updated Value"].ToString(), 100)) + "</td>");
                        strSSP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["Action"]) + "</td>");
                        strSSP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["UserName"]) + "</td>");
                        strSSP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["UpdatedOn"]) + "</td>");
                        strSSP.Append("</tr>");
                        //strSSP.Replace("@#$", Convert.ToString(drSub["PagesName"]));
                    }
                }
                strSSP.Append("</table>");
                strSSP.Append("</li></ul></div>");

            }

            DataSet dsSSP = new DataSet();
            DataTable dtSSp = new DataTable();
            DataColumn dcSSP = new DataColumn("html");
            dtSSp.Columns.Add(dcSSP);
            DataRow drSSP = dtSSp.NewRow();
            drSSP["html"] = strSSP.ToString();
            if (string.IsNullOrEmpty(strSSP.ToString()))
            {
                drSSP["html"] = "<table style='width:100% !important' class='table'> <tr><td> No Record Found </td></tr></table>";
            }
            dtSSp.Rows.Add(drSSP);
            dsSSP.Tables.Add(dtSSp);
            dsSSP.AcceptChanges();

            rpSSP.DataSource = dsSSP;
            rpSSP.DataBind();
        }

        private void PopulateWPDetails(DataSet ds)
        {
            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "ModuleText='Worker Portal'";
            //gvWP.DataSource = dv.ToTable();
            //gvWP.DataBind();
            lblWPCount.Text = "Count : " + Convert.ToString(dv.ToTable().Rows.Count);
            ViewState["ActiveTab"] = "WP";

            StringBuilder strWP = new StringBuilder();

            foreach (DataRow dr in dv.ToTable(true, "Page Name").Rows)
            {
                //int count = 0;
                //dv = null;
                //dv = ds.Tables[0].DefaultView;
                //dv.RowFilter = "[Page Name]='" + Convert.ToString(dr["Page Name"]) + "'";
                //count = dv.ToTable().Rows.Count;
                strWP.Append("<div class='collapsible' style='margin-left:40px;width:90%'>  &nbsp;&nbsp;&nbsp;" + Convert.ToString(dr["Page Name"]) + "<span></span>");
                //strWP.Append(" <div style='float: right; padding: 0;width:500px;'> " + "Pages :  @#$" + " </div> </div>");
                strWP.Append(" <div style='float: right; padding: 0;width:500px;'>  </div> </div>");
                strWP.Append("  <div> <ul style='list-style: none; margin-left: -40px;'><li>");
                strWP.Append("<table width='100%' style='width:91% !important;margin-left:44px' class='table'>");
                //strWP.Append("<tr><th align='left' valign='top'>Page Name </th>");
                strWP.Append("<th align='left' valign='top'>Column Name </th>");
                strWP.Append("<th align='left' valign='top'>Old Value </th>");
                strWP.Append("<th align='left' valign='top'>New Value </th>");
                strWP.Append("<th align='left' valign='top'>Action </th>");
                strWP.Append("<th align='left' valign='top'>Updated By </th>");
                strWP.Append("<th align='left' valign='top'>Updated On </th></tr>");

                foreach (DataRow drSub in dv.ToTable().Rows)
                {
                    if (Convert.ToString(dr["Page Name"]).Equals(Convert.ToString(drSub["Page Name"]), StringComparison.OrdinalIgnoreCase))
                    {
                        strWP.Append("<tr>");
                        //strWP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["Page Name"]) + "</td>");
                        strWP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["Column Name"]) + "</td>");
                        strWP.Append("<td align='left' valign='top'>" + Convert.ToString(TruncateAtWord(drSub["Initial Value"].ToString(), 100)) + "</td>");
                        strWP.Append("<td align='left' valign='top'>" + Convert.ToString(TruncateAtWord(drSub["Updated Value"].ToString(), 100)) + "</td>");
                        strWP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["Action"]) + "</td>");
                        strWP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["UserName"]) + "</td>");
                        strWP.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["UpdatedOn"]) + "</td>");
                        strWP.Append("</tr>");
                        //strWP.Replace("@#$", Convert.ToString(drSub["PagesName"]));
                    }
                }
                strWP.Append("</table>");
                strWP.Append("</li></ul></div>");

            }

            DataSet dsWP = new DataSet();
            DataTable dtWP = new DataTable();
            DataColumn dcWP = new DataColumn("html");
            dtWP.Columns.Add(dcWP);
            DataRow drWP = dtWP.NewRow();
            drWP["html"] = strWP.ToString();
            if (string.IsNullOrEmpty(strWP.ToString()))
            {
                drWP["html"] = "<table style='width:100% !important' class='table'> <tr><td> No Record Found </td></tr></table>";
            }
            dtWP.Rows.Add(drWP);
            dsWP.Tables.Add(dtWP);
            dsWP.AcceptChanges();

            rpWP.DataSource = dsWP;
            rpWP.DataBind();
        }

        private void PopulateDEDetails(DataSet ds)
        {
            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "ModuleId='0'";
            //gvDE.DataSource = dv.ToTable();
            //gvDE.DataBind();
            lblElementCount.Text = "Count : " + Convert.ToString(dv.ToTable().Rows.Count);
            ViewState["ActiveTab"] = "DE";

            StringBuilder strElement = new StringBuilder();

            foreach (DataRow dr in dv.ToTable(true, "Element Name").Rows)
            {
                //int count = 0;
                //dv = null;
                //dv = ds.Tables[0].DefaultView;
                //dv.RowFilter = "[Page Name]='" + Convert.ToString(dr["Page Name"]) + "'";
                //count = dv.ToTable().Rows.Count;
                strElement.Append("<div class='collapsible' style='margin-left:40px;width:90%'>  &nbsp;&nbsp;&nbsp;" + Convert.ToString(dr["Element Name"]) + "<span></span>");
                strElement.Append(" <div style='float: right; padding: 0;width:500px;'> " + "Pages :  @#$" + " </div> </div>");
                strElement.Append("  <div> <ul style='list-style: none; margin-left: -40px;'><li>");
                strElement.Append("<table width='100%' style='width:91% !important;margin-left:44px' class='table'>");
                //strElement.Append("<tr><th align='left' valign='top'>Element Name </th>");
                strElement.Append("<th align='left' valign='top'>Column Name </th>");
                strElement.Append("<th align='left' valign='top'>Old Value </th>");
                strElement.Append("<th align='left' valign='top'>New Value </th>");
                strElement.Append("<th align='left' valign='top'>Action </th>");
                strElement.Append("<th align='left' valign='top'>Updated By </th>");
                strElement.Append("<th align='left' valign='top'>Updated On </th></tr>");

                foreach (DataRow drSub in dv.ToTable().Rows)
                {
                    if (Convert.ToString(dr["Element Name"]).Equals(Convert.ToString(drSub["Element Name"]), StringComparison.OrdinalIgnoreCase))
                    {
                        strElement.Append("<tr>");
                        //strElement.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["Element Name"]) + "</td>");
                        strElement.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["Column Name"]) + "</td>");
                        strElement.Append("<td align='left' valign='top'>" + Convert.ToString(TruncateAtWord(drSub["Initial Value"].ToString(), 100)) + "</td>");
                        strElement.Append("<td align='left' valign='top'>" + Convert.ToString(TruncateAtWord(drSub["Updated Value"].ToString(),100)) + "</td>");
                        strElement.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["Action"]) + "</td>");
                        strElement.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["UserName"]) + "</td>");
                        strElement.Append("<td align='left' valign='top'>" + Convert.ToString(drSub["UpdatedOn"]) + "</td>");
                        strElement.Append("</tr>");
                        strElement.Replace("@#$", Convert.ToString(drSub["PagesName"]));
                    }
                }
                strElement.Append("</table>");
                strElement.Append("</li></ul></div>");

            }

            DataSet dsElement = new DataSet();
            DataTable dtElement = new DataTable();
            DataColumn dcElement = new DataColumn("html");
            dtElement.Columns.Add(dcElement);
            DataRow drElement = dtElement.NewRow();
            drElement["html"] = strElement.ToString();
            if (string.IsNullOrEmpty(strElement.ToString()))
            {
                drElement["html"] = "<table style='width:100% !important' class='table'> <tr><td> No Record Found </td></tr></table>";
            }
            dtElement.Rows.Add(drElement);
            dsElement.Tables.Add(dtElement);
            dsElement.AcceptChanges();

            rpElement.DataSource = dsElement;
            rpElement.DataBind();
        }

        private string TruncateAtWord(string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length);
            return string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }

        #endregion
    }
}