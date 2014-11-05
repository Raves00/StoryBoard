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
    public partial class SearchPage : System.Web.UI.UserControl
    {
        public delegate void OnPageSelectionChanged(int PageID);
        public event OnPageSelectionChanged PageSelectionChanged;
        public event EventHandler AddButtonClicked;

        public bool ShowPgList
        {
            get { return ddlPageList.Visible; }
            set
            {
                ddlPageList.Visible = value;
                pg.Visible = false;
            }
        }

        public bool ShowAddButton
        {
            get { return btnAdd.Visible; }
            set { btnAdd.Visible = value; }
        }

        public string ButtonText
        {
            get { return btnAdd.Text; }
            set { btnAdd.Text = value; }
        }

        public string SelectedPageName
        {
            get
            {
                return ddlPageList.SelectedItem.Text;
            }
        }

        public string PageControlClientID { get { return ddlPageList.ClientID; } }
        bool _validatepageselectiononadd;
        public bool ValidatePageSelectionOnAdd
        {
            get { return _validatepageselectiononadd; }
            set
            {
                _validatepageselectiononadd = value;

            }
        }

        private void RenderValidationScript()
        {
            btnAdd.Attributes.Add("onclick", string.Format("return ValidateSelectionPage('{0}')", ddlPageList.ClientID));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ValidatePageSelectionOnAdd)
                    RenderValidationScript();
                SetModuleSelectionFromSession();
                BindStoryBoardPages();
                SetPageSelectionFromSession();
                if (Request.QueryString["fas"] != null)
                {
                    if (PageSelectionChanged != null)
                    {
                        PageSelectionChanged(Convert.ToInt32(ddlPageList.SelectedValue));
                    }
                }
            }
            //RenderAlignment();
        }



        private void SetPageSelectionFromSession()
        {
            int _pageid = (this.Page.Master as GlobalMaster).PageID;
            if (_pageid != -1)
            {
                ddlPageList.SelectedIndex = -1;
                var item = ddlPageList.Items.FindByValue(Convert.ToString(_pageid));
                if (item != null)
                    item.Selected = true;
                if (PageSelectionChanged != null)
                    PageSelectionChanged(Convert.ToInt32(ddlPageList.SelectedValue));
            }
        }


        private void SetModuleSelectionFromSession()
        {
            int _moduleid = (this.Page.Master as GlobalMaster).ModuleType;
            if (_moduleid != -1)
            {
                ddlModule.SelectedIndex = -1;
                var item = ddlModule.Items.FindByValue(Convert.ToString(_moduleid));
                if (item != null)
                    item.Selected = true;
            }
        }

        private void BindStoryBoardPages()
        {
            DataTable dtPagesForModules = DataMaster.GetPagesForModule(Convert.ToInt32(ddlModule.SelectedValue));
            ddlPageList.Items.Clear();
            ddlPageList.DataSource = dtPagesForModules;
            ddlPageList.DataBind();
            ddlPageList.Items.Insert(0, (new ListItem("--Select--", "-1")));
        }

        //private void BindStoryBoardPages()
        //{
        //    using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
        //    {
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            using (SqlCommand cmd = new SqlCommand("stpGetStoryBoardPages", sqlconn))
        //            {
        //                sda.SelectCommand = cmd;
        //                DataSet ds = new DataSet();
        //                sda.Fill(ds);
        //                ddlPageList.DataSource = ds;
        //                ddlPageList.DataTextField = "PageName";
        //                ddlPageList.DataValueField = "PageID";
        //                ddlPageList.DataBind();
        //                ddlPageList.Items.Insert(0,(new ListItem("--Select--", "-1")));
        //            }
        //        }
        //    }
        //}

        protected void ddlPageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PageSelectionChanged != null)
            {
                (this.Page.Master as GlobalMaster).PageID = Convert.ToInt32(ddlPageList.SelectedValue);
                PageSelectionChanged(Convert.ToInt32(ddlPageList.SelectedValue));
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (AddButtonClicked != null)
            {
                AddButtonClicked(null, null);
            }
        }

        internal void UpdatePageDetails()
        {
            BindStoryBoardPages();
        }



        public int SelectedPageId
        {
            get
            {
                return Convert.ToInt32(ddlPageList.SelectedValue);
            }
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            (this.Page.Master as GlobalMaster).ModuleType = Convert.ToInt32(ddlModule.SelectedValue);
            BindStoryBoardPages();
        }

        public int ModuleId
        {
            get
            {
                return Convert.ToInt32(ddlModule.SelectedValue);
            }
        }


    }
}