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
    public partial class SearchElements : System.Web.UI.Page
    {

        #region properties
        DataTable PageElementsTable
        {
            get
            {
                if (ViewState["dtPageElements"] != null)
                {
                    return ViewState["dtPageElements"] as DataTable;
                }
                else
                    return null;
            }

            set { ViewState["dtPageElements"] = value; }
        }

        DataTable dtControlTypes
        {
            get
            {
                if (ViewState["dtControlTypes"] != null)
                {
                    return ViewState["dtControlTypes"] as DataTable;
                }
                else
                    return null;
            }

            set { ViewState["dtControlTypes"] = value; }
        }


        DataTable dtStatuses
        {
            get
            {
                if (ViewState["dtStatuses"] != null)
                {
                    return ViewState["dtStatuses"] as DataTable;
                }
                else
                    return null;
            }

            set { ViewState["dtStatuses"] = value; }
        }

        DataTable dtReferences
        {
            get
            {
                if (ViewState["dtReferences"] != null)
                {
                    return ViewState["dtReferences"] as DataTable;
                }
                else
                    return null;
            }

            set { ViewState["dtReferences"] = value; }
        }

        DataTable dtYesNo
        {
            get
            {
                if (ViewState["dtYesNo"] != null)
                {
                    return ViewState["dtYesNo"] as DataTable;
                }
                else
                    return null;
            }

            set { ViewState["dtYesNo"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPages();
                PopulateMasters();
                SetSelectionFromSession();
                BindStoryBoardPages();
                SetPageSelection();
                BindSearchPages();
            }
            lblErrorMessage.Visible = false;
        }

        private void SetPageSelection()
        {
            if (HttpContext.Current.Session["PageId"] != null)
            {
                int pageid = Convert.ToInt32(HttpContext.Current.Session["PageId"]);
                if (pageid != -1)
                {
                    ddlPage.SelectedIndex = -1;
                    var item = ddlPage.Items.FindByValue(pageid.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        private void SetSelectionFromSession()
        {
            if (HttpContext.Current.Session["ModuleType"] != null)
            {
                int moduleid = Convert.ToInt32(HttpContext.Current.Session["ModuleType"]);
                if (moduleid != -1)
                {
                    ddlModule.SelectedIndex = -1;
                    var item = ddlModule.Items.FindByValue(moduleid.ToString());
                    if (item != null)
                        item.Selected = true;
                }
            }


        }

        private void PopulateMasters()
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetControlTypes", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        dtControlTypes = ds.Tables[0];
                    }

                    using (SqlCommand cmd = new SqlCommand("stp_GetStatusMaster", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        dtStatuses = ds.Tables[0];
                    }

                    using (SqlCommand cmd = new SqlCommand("stp_GetReferenceTables", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        dtReferences = ds.Tables[0];
                    }

                    using (SqlCommand cmd = new SqlCommand("stp_GetYesNoMaster", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        dtYesNo = ds.Tables[0];
                    }
                }
            }
        }


        private void BindPages()
        {
            ddlSearchPage.DataSource = DataMaster.GetPageList();
            ddlSearchPage.DataTextField = "PageName";
            ddlSearchPage.DataValueField = "PageID";
            ddlSearchPage.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string strPageId = ddlSearchPage.SelectedValue;
            string strElementName = txtElementName.Text;
            DataTable dtSearchResults = DataMaster.SearchElements(strElementName, strPageId);
            gvPageElements.DataSource = dtSearchResults;
            gvPageElements.DataBind();
            bool isdataavailable = dtSearchResults.Rows.Count > 0;
            btnAddExisting.Visible = isdataavailable;
            tr_Module.Visible = isdataavailable;
            tr_Page.Visible = isdataavailable;
            // tablecontainer.Visible = isdataavailable;

        }

        protected void gvPageElements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                DropDownList ddlControlType = e.Row.FindControl("ddlControlType") as DropDownList;
                ddlControlType.DataSource = dtControlTypes;
                ddlControlType.DataBind();
                var selectedcontroltype = ddlControlType.Items.FindByValue(Convert.ToString(drv["ControlType"]));
                if (selectedcontroltype != null)
                    selectedcontroltype.Selected = true;

                DropDownList ddlStatuses = e.Row.FindControl("ddlStatus") as DropDownList;
                ddlStatuses.DataSource = dtStatuses;
                ddlStatuses.DataBind();
                var selectedstatus = ddlStatuses.Items.FindByValue(Convert.ToString(drv["Status"]));
                if (selectedstatus != null)
                    selectedstatus.Selected = true;

                DropDownList ddlReferences = e.Row.FindControl("ddlReferenceTable") as DropDownList;
                ddlReferences.DataSource = dtReferences;
                ddlReferences.DataBind();
                var selectedreference = ddlReferences.Items.FindByValue(Convert.ToString(drv["ReferenceTable"]));
                if (selectedreference != null)
                    selectedreference.Selected = true;


                DropDownList ddllsRequired = e.Row.FindControl("ddllsRequired") as DropDownList;
                ddllsRequired.DataSource = dtYesNo;
                ddllsRequired.DataBind();
                var selectedisrequired = ddllsRequired.Items.FindByValue(Convert.ToString(drv["IsRequired"]));
                if (selectedisrequired != null)
                    selectedisrequired.Selected = true;


                DropDownList ddllsKTAP = e.Row.FindControl("ddlIsKTAP") as DropDownList;
                ddllsKTAP.DataSource = dtYesNo;
                ddllsKTAP.DataBind();
                var selectedisKTAP = ddllsKTAP.Items.FindByValue(Convert.ToString(drv["KTAP"]));
                if (selectedisKTAP != null)
                    selectedisKTAP.Selected = true;

                DropDownList ddllsSNAP = e.Row.FindControl("ddlIsSNAP") as DropDownList;
                ddllsSNAP.DataSource = dtYesNo;
                ddllsSNAP.DataBind();
                var selectedisSNAP = ddllsSNAP.Items.FindByValue(Convert.ToString(drv["SNAP"]));
                if (selectedisSNAP != null)
                    selectedisSNAP.Selected = true;

                DropDownList ddllsMedicaid = e.Row.FindControl("ddlIsMedicaid") as DropDownList;
                ddllsMedicaid.DataSource = dtYesNo;
                ddllsMedicaid.DataBind();
                var selectedismedicaid = ddllsMedicaid.Items.FindByValue(Convert.ToString(drv["MEDICAID"]));
                if (selectedismedicaid != null)
                    selectedismedicaid.Selected = true;


                //DropDownList ddllsOtherProg = e.Row.FindControl("ddlIsOtherProgram") as DropDownList;
                //ddllsOtherProg.DataSource = dtYesNo;
                //ddllsOtherProg.DataBind();
                //var selectedisotherprog = ddllsOtherProg.Items.FindByValue(Convert.ToString(drv["OtherPrograms"]));
                //if (selectedisotherprog != null)
                //    selectedisotherprog.Selected = true;
            }
        }

        protected void gvPageElements_PreRender(object sender, EventArgs e)
        {
            if (gvPageElements.Rows.Count > 0)
            {
                //This replaces <td> with <th> and adds the scope attribute
                gvPageElements.UseAccessibleHeader = true;

                //This will add the <thead> and <tbody> elements
                gvPageElements.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gvPageElements_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //no action

        }

        protected void btnAddExisting_Click(object sender, EventArgs e)
        {
            int nPageId = Convert.ToInt32(ddlPage.SelectedValue);
            if (nPageId != -1)
            {
                for (int i = 0; i < gvPageElements.Rows.Count; i++)
                {
                    CheckBox chkIsToBeadded = (CheckBox)gvPageElements.Rows[i].FindControl("chkSelect");
                    int elementId;
                    int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Values["ElementID"]), out elementId);
                    if (chkIsToBeadded.Checked)
                    {
                        DataMaster.AddPageElementMapping(nPageId, elementId);
                        lblErrorMessage.Text = "Mapping added successfully";
                        lblErrorMessage.Visible = true;
                    }
                }
                (this.Page.Master as GlobalMaster).PageID = nPageId;
                (this.Page.Master as GlobalMaster).ModuleType = Convert.ToInt32(ddlModule.SelectedValue);
            }

            Response.Redirect("AddDataElements.aspx?fas=1"); // add a flag for from add screen
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindStoryBoardPages();
            HttpContext.Current.Session["ModuleType"] = ddlModule.SelectedValue;
        }

        private void BindStoryBoardPages()
        {
            DataTable dtPagesForModules = DataMaster.GetPagesForModule(Convert.ToInt32(ddlModule.SelectedValue));
            ddlPage.Items.Clear();
            ddlPage.DataSource = dtPagesForModules;
            ddlPage.DataBind();
            ddlPage.Items.Insert(0, (new ListItem("--Select--", "-1")));
        }

        protected void ddlSearchModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSearchPages();
        }

        private void BindSearchPages()
        {
            DataTable dtPagesForModules = DataMaster.GetPagesForModule(Convert.ToInt32(ddlSearchModule.SelectedValue));
            ddlSearchPage.Items.Clear();
            ddlSearchPage.DataSource = dtPagesForModules;
            ddlSearchPage.DataBind();
            ddlSearchPage.Items.Insert(0, (new ListItem("--Select--", "-1")));
        }
    }
}