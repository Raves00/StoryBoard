using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoryBoard
{
    public partial class AddComponent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillMasters();
            }
        }

        private void FillMasters()
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetComponentTypeMaster", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        ddlComponentType.DataSource = ds.Tables[0];
                        ddlComponentType.DataBind();
                    }

                    using (SqlCommand cmd = new SqlCommand("stp_GetComponentNameMaster", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        ddlComponentName.DataSource = ds.Tables[0];
                        ddlComponentName.DataBind();
                    }

                    using (SqlCommand cmd = new SqlCommand("stpGetStoryBoardPages", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        ddlPage.DataSource = ds.Tables[0];
                        ddlPage.DataBind();
                    }
                   
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        using (SqlCommand cmd = new SqlCommand("stp_InsertComponent", sqlconn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PageId", ddlPage.SelectedValue);
                            cmd.Parameters.AddWithValue("@ComponentTypeId", ddlComponentType.SelectedValue);
                            cmd.Parameters.AddWithValue("@ComponentNameId", ddlComponentName.SelectedValue);
                            cmd.Parameters.AddWithValue("@Description", txtComponentDesc.Text);
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();
                        }
                    }
                }
                lblStatus.Text = "Data Saved Successfully";
            }
            catch (Exception)
            {
               lblStatus.Text = "Error Saving data";
            }


        }
    }
}