using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoryBoard
{
    public partial class AddComponent : System.Web.UI.Page
    {
        DataTable dtComponentTypes
        {
            get
            {
                if (Cache["dtComponentTypes"] == null)
                    Cache["dtComponentTypes"] = DataMaster.GetComponentTypes();
                return Cache["dtComponentTypes"] as DataTable;
            }
        }

        DataTable PageControlTypes
        {
            get
            {
                if (ViewState["dtPageControlTypes"] != null)
                {
                    return ViewState["dtPageControlTypes"] as DataTable;
                }
                else
                    return null;
            }

            set { ViewState["dtPageControlTypes"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            ucModulePage.AddButtonClicked += ucModulePage_AddButtonClicked;
            ucModulePage.PageSelectionChanged += ucModulePage_PageSelectionChanged;
        }

        void ucModulePage_PageSelectionChanged(int PageID)
        {
            ShowPageComponents(PageID);
            lblErrorMessage.Visible = false;
        }

        private void ShowPageComponents(int PageID)
        {
            DataTable dtPageComponents = DataMaster.GetPageComponents(PageID);
            PageControlTypes = dtPageComponents;
            BindGrid();
        }

        private void BindGrid()
        {
            grdComponents.DataSource = PageControlTypes;
            grdComponents.DataBind();
            btnSubmit.Visible = PageControlTypes.Rows.Count > 0;
        }

        private void EnsureSchema()
        {
            if (PageControlTypes == null)
            {
                DataTable dtPageComponents = new DataTable();
                dtPageComponents.Columns.Add(new DataColumn("ComponentId", typeof(int)));
                dtPageComponents.Columns.Add(new DataColumn("ComponentType", typeof(int)));
                dtPageComponents.Columns.Add(new DataColumn("ComponentName", typeof(string)));
                dtPageComponents.Columns.Add(new DataColumn("ComponentDescription", typeof(string)));
                PageControlTypes = dtPageComponents;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            int roleid = (Session["User"] as User).RoleId;
            if (roleid == 2)
            {
                ucModulePage.ShowAddButton = false;
                btnSubmit.Visible = false;
            }
        }

        void ucModulePage_AddButtonClicked(object sender, EventArgs e)
        {
            EnsureSchema();
            SaveExistingData();
            AddEmptyRow();
            lblErrorMessage.Visible = false;
        }

        private void SaveExistingData()
        {
            PageControlTypes.Clear();
            for (int i = 0; i < grdComponents.Rows.Count; i++)
            {
                if (grdComponents.Rows[i].Visible)
                {
                    DataRow drComponentData = PageControlTypes.NewRow();
                    int _componentid = Convert.ToInt32(grdComponents.DataKeys[i].Value);
                    DropDownList ddlComponentType = (DropDownList)grdComponents.Rows[i].FindControl("ddlComponentType");
                    TextBox txtComponentName = (TextBox)grdComponents.Rows[i].FindControl("txtComponentName");
                    TextBox txtComponentDescription = (TextBox)grdComponents.Rows[i].FindControl("txtComponentDescription");
                    drComponentData["ComponentId"] = _componentid;
                    drComponentData["ComponentType"] = Convert.ToInt32(ddlComponentType.SelectedValue);
                    drComponentData["ComponentName"] = txtComponentName.Text;
                    drComponentData["ComponentDescription"] = txtComponentDescription.Text;
                    PageControlTypes.Rows.Add(drComponentData);
                }
            }
        }

        private void AddEmptyRow()
        {
            DataRow drNewRow = PageControlTypes.NewRow();
            drNewRow["ComponentId"] = -1;
            drNewRow["ComponentType"] = 1;
            drNewRow["ComponentName"] = "";
            drNewRow["ComponentDescription"] = "";
            PageControlTypes.Rows.Add(drNewRow);
            BindGrid();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void grdComponents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                DropDownList ddlComponentType = (DropDownList)e.Row.FindControl("ddlComponentType");
                ddlComponentType.DataSource = dtComponentTypes;
                ddlComponentType.DataBind();

                ImageButton deletebtn = (ImageButton)e.Row.FindControl("imgbtn_Delete");
                var selectedComponentType = Convert.ToString(drv["ComponentType"]);
                var selecteditem = ddlComponentType.Items.FindByValue(selectedComponentType);
                if (selecteditem != null)
                    selecteditem.Selected = true;

                int roleid = (Session["User"] as User).RoleId;
                if (roleid == 2)
                {
                    deletebtn.Visible = false;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string strUserId = Session["User"] != null ? ((User)Session["User"]).UserName : "";
                int _pageid = ucModulePage.SelectedPageId;
                for (int i = 0; i < grdComponents.Rows.Count; i++)
                {
                    if (grdComponents.Rows[i].Visible)
                    {
                        DataRow drComponentData = PageControlTypes.NewRow();
                        int _componentid = Convert.ToInt32(grdComponents.DataKeys[i].Value);
                        int _componenttype;
                        string _componentName;
                        string _componentDesc;
                        DropDownList ddlComponentType = (DropDownList)grdComponents.Rows[i].FindControl("ddlComponentType");
                        TextBox txtComponentName = (TextBox)grdComponents.Rows[i].FindControl("txtComponentName");
                        TextBox txtComponentDescription = (TextBox)grdComponents.Rows[i].FindControl("txtComponentDescription");
                        _componenttype = Convert.ToInt32(ddlComponentType.SelectedValue);
                        _componentName = txtComponentName.Text.Trim();
                        _componentDesc = txtComponentDescription.Text.Trim();
                        DataMaster.InsertUpdatePageComponents(_pageid, _componentid, _componenttype, _componentName, _componentDesc, strUserId);
                    }
                }
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Data Saved Successfully";
                lblErrorMessage.ForeColor = Color.Green;
                ReBindComponents(_pageid);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "Error Saving Data";
                lblErrorMessage.ForeColor = Color.Red;
                ErrorLogger.LogError("AddComponent", ex);
            }

            
        }

        private void ReBindComponents(int _pageId)
        {
            ShowPageComponents(_pageId);
        }

        protected void grdComponents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int _componentid = Convert.ToInt32(e.CommandArgument);
                if (_componentid > 0)
                    DataMaster.DeleteComponent(_componentid);

                var _rowindex = (((e.CommandSource as ImageButton).Parent  as DataControlFieldCell).Parent as GridViewRow).RowIndex;
               // PageControlTypes.Rows.RemoveAt(_rowindex);
                grdComponents.Rows[_rowindex].Visible = false;
                SaveExistingData();
                BindGrid();
            }
        }


    }
}