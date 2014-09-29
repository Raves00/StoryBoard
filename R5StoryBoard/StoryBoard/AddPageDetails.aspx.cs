using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace StoryBoard
{
    public partial class AddPageDetails : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            ucSearchPage.AddButtonClicked += ucSearchPage_AddButtonClicked;
            ucSearchPage.PageSelectionChanged += ucSearchPage_PageSelectionChanged;
        }

        void ucSearchPage_PageSelectionChanged(int PageID)
        {
            if (PageID != -1)
            {
                tblPageDetails.Visible = true;
                PopulatePageDetails(PageID);
                btnAddPageDetails.Text = "Update";
            }
            else
            {
                tblPageDetails.Visible = false;
            }
            ViewState["PageID"] = PageID;
        }

        private void PopulatePageDetails(int PageID)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetStoryBoardPages", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageId", PageID);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            txtPageDesignation.Text = dr.Field<string>("PageDesignation");
                            txtPageName.Text = dr.Field<string>("PageName");
                            txtPageDescription.Text = dr.Field<string>("PageDescription");
                            txtBusinessProcess.Text = dr.Field<string>("BusinessProcess");
                            txtActivity.Text = dr.Field<string>("Activity");
                            txtPrograms.Text = dr.Field<string>("Programs");
                        }
                    }
                }
            }
        }

        void ucSearchPage_AddButtonClicked(object sender, EventArgs e)
        {
            ViewState["PageID"] = -1;
            tblPageDetails.Visible = true;
            btnAddPageDetails.Text = "Add";
            ucSearchPage.ShowAddButton = false;
            txtPageDesignation.Text = "";
            txtPageName.Text = "";
            txtPageDescription.Text = "";
            txtBusinessProcess.Text = "";
            txtActivity.Text = "";
            txtPrograms.Text = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnAddPageDetails_Click(object sender, EventArgs e)
        {
            bool issuccess = false;
            lblErrorMessage.Visible = false;
            if (Convert.ToString(ViewState["PageID"]) == "-1")
            {
                issuccess = InsertPageDetails();
            }
            else
            {
                issuccess = UpdatePageDetails();
            }

            if (issuccess)
            {
                tblPageDetails.Visible = false;
                ucSearchPage.UpdatePageDetails();
                ucSearchPage.ShowAddButton = true;
            }
        }

        private bool UpdatePageDetails()
        {
            int nStatus = 0;
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stp_UpdatePageDetails", sqlconn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageId", Convert.ToInt32(ViewState["PageID"]));
                    cmd.Parameters.AddWithValue("@PageDesignation", txtPageDesignation.Text);
                    cmd.Parameters.AddWithValue("@PageName", txtPageName.Text);
                    cmd.Parameters.AddWithValue("@PageDescription", txtPageDescription.Text);
                    cmd.Parameters.AddWithValue("@BusinessProcess", txtBusinessProcess.Text);
                    cmd.Parameters.AddWithValue("@Activity", txtActivity.Text);
                    cmd.Parameters.AddWithValue("@Programs", txtPrograms.Text);
                    cmd.Parameters.AddWithValue("@Module", ucSearchPage.ModuleId);
                    cmd.Connection.Open();
                    nStatus = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Connection.Close();
                    if (nStatus == 0)
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = string.Format(" Page with name '{0}' already exists", txtPageName.Text);
                    }
                }
            }
            return nStatus != 0;
        }

        private bool InsertPageDetails()
        {
            int nStatus = 0;
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stp_InsertPageDetails", sqlconn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageDesignation", txtPageDesignation.Text);
                    cmd.Parameters.AddWithValue("@PageName", txtPageName.Text);
                    cmd.Parameters.AddWithValue("@PageDescription", txtPageDescription.Text);
                    cmd.Parameters.AddWithValue("@BusinessProcess", txtBusinessProcess.Text);
                    cmd.Parameters.AddWithValue("@Activity", txtActivity.Text);
                    cmd.Parameters.AddWithValue("@Programs", txtPrograms.Text);
                    cmd.Parameters.AddWithValue("@Module", ucSearchPage.ModuleId);
                    cmd.Connection.Open();
                    nStatus = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Connection.Close();
                    if (nStatus == 0)
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = string.Format(" Page with name '{0}' already exists", txtPageName.Text);
                    }
                }
            }
            return nStatus != 0;
        }
    }
}