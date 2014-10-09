using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

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
            if (ValidateElementsAndSave())
            {
                (this.Page.Master as GlobalMaster).PageID = nPageId;
                (this.Page.Master as GlobalMaster).ModuleType = Convert.ToInt32(ddlModule.SelectedValue);

                Response.Redirect("AddDataElements.aspx?fas=1"); // add a flag for from add screen
            }
        }

        private bool ValidateElementsAndSave()
        {
            bool isvalid = false;
            int nPageId = Convert.ToInt32(ddlPage.SelectedValue);
            if (nPageId != -1)
            {
                XDocument xdoc = new XDocument();
                XElement xRootElement = new XElement("ElementSet");
                for (int i = 0; i < gvPageElements.Rows.Count; i++)
                {
                    CheckBox chkIsToBeadded = (CheckBox)gvPageElements.Rows[i].FindControl("chkSelect");
                    int elementId;
                    int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Values["ElementID"]), out elementId);
                    string strElementName = (gvPageElements.Rows[i].FindControl("txtElementName") as TextBox).Text.Trim();
                    string strLength = (gvPageElements.Rows[i].FindControl("txtLength") as TextBox).Text.Trim();
                    int intControlType = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlControlType") as DropDownList).SelectedValue);
                    int bIsRequired = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddllsRequired") as DropDownList).SelectedValue);
                    string strReferenceTable = (gvPageElements.Rows[i].FindControl("ddlReferenceTable") as DropDownList).SelectedValue.Trim();
                    string strDisplayRule = (gvPageElements.Rows[i].FindControl("txtDisplayRule") as TextBox).Text.Trim();
                    string strValidations = (gvPageElements.Rows[i].FindControl("txtValidations") as TextBox).Text.Trim();
                    string strValidationTrigger = (gvPageElements.Rows[i].FindControl("txtValidationTrigger") as TextBox).Text.Trim();
                    string strErrorCode = (gvPageElements.Rows[i].FindControl("txtErrorCode") as TextBox).Text.Trim();
                    int intStatus = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlStatus") as DropDownList).SelectedValue);

                    int bIsKTAP = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddllsRequired") as DropDownList).SelectedValue);
                    int bIsSNAP = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddllsRequired") as DropDownList).SelectedValue);
                    int bIsMedicAid = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddllsRequired") as DropDownList).SelectedValue);
                    string bIsOtherPrograms = (gvPageElements.Rows[i].FindControl("txtOtherPrograms") as TextBox).Text;
                    string strDatabaseName = (gvPageElements.Rows[i].FindControl("txtDatabaseTableName") as TextBox).Text.Trim();
                    string strDatabaseFields = (gvPageElements.Rows[i].FindControl("txtDatabaseFields") as TextBox).Text.Trim();
                    string strOpenQuestions = (gvPageElements.Rows[i].FindControl("txtOpenQuestions") as TextBox).Text.Trim();

                    string strSSPDispName = (gvPageElements.Rows[i].FindControl("txtSSPDispName") as TextBox).Text.Trim();

                    string strWPDispName = (gvPageElements.Rows[i].FindControl("txtWPDisplayName") as TextBox).Text.Trim();

                    if (chkIsToBeadded.Checked)
                    {
                        XElement elementroot = new XElement("Elements");
                        elementroot.Add(new XElement("ElementID", elementId));
                        elementroot.Add(new XElement("ElementName", strElementName));
                        elementroot.Add(new XElement("Length", strLength));
                        elementroot.Add(new XElement("ControlType", intControlType));
                        elementroot.Add(new XElement("IsRequired", bIsRequired));
                        elementroot.Add(new XElement("ReferenceTable", SBHelper.EncodeData(strReferenceTable)));
                        elementroot.Add(new XElement("DisplayRule", SBHelper.EncodeData(strDisplayRule)));
                        elementroot.Add(new XElement("Validations", SBHelper.EncodeData(strValidations)));
                        elementroot.Add(new XElement("ValidationTrigger", SBHelper.EncodeData(strValidationTrigger)));
                        elementroot.Add(new XElement("ErrorCode", SBHelper.EncodeData(strErrorCode)));
                        elementroot.Add(new XElement("Status", intStatus));
                        elementroot.Add(new XElement("KTAP", bIsKTAP));
                        elementroot.Add(new XElement("SNAP", bIsSNAP));
                        elementroot.Add(new XElement("MEDICAID", bIsMedicAid));
                        elementroot.Add(new XElement("OtherPrograms", SBHelper.EncodeData(bIsOtherPrograms)));
                        elementroot.Add(new XElement("DatabaseTableName", SBHelper.EncodeData(strDatabaseName)));
                        elementroot.Add(new XElement("DatabaseTableFields", SBHelper.EncodeData(strDatabaseFields)));
                        elementroot.Add(new XElement("OpenQuestions", SBHelper.EncodeData(strOpenQuestions)));
                        elementroot.Add(new XElement("SSPDisplayName", SBHelper.EncodeData(strSSPDispName)));
                        elementroot.Add(new XElement("WPDisplayName", SBHelper.EncodeData(strWPDispName)));
                        xRootElement.Add(elementroot);
                    }
                }
                xdoc.Add(xRootElement);
                ViewState["ElementData"] = xdoc.ToString();
                DataSet dselemvalidationresult = DataMaster.ValidateModifiedElements(xdoc.ToString());
                if (dselemvalidationresult.Tables[0].Rows.Count > 0)
                {
                    grdElemValidationResults.DataSource = dselemvalidationresult.Tables[0];
                    grdElemValidationResults.DataBind();
                    ShowConfirmationDialog();
                    isvalid= false;
                }
                else
                {
                    int count = xdoc.Element("ElementSet").Elements("Elements").Count();
                    for (int i = 0; i < count; i++)
                    {
                        int _elementid = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ElementID").Value);
                        string strElementName = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ElementName").Value;
                        string strLength = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("Length").Value;
                        int intControlType = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ControlType").Value);
                        int bIsRequired = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("IsRequired").Value);
                        string strReferenceTable = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ReferenceTable").Value;
                        string strDisplayRule = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("DisplayRule").Value;
                        string strValidations = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("Validations").Value;
                        string strValidationTrigger = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ValidationTrigger").Value;
                        string strErrorCode = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ErrorCode").Value;
                        int intStatus = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("Status").Value);
                        int bIsKTAP = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("KTAP").Value);
                        int bIsSNAP = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("SNAP").Value);
                        int bIsMedicAid = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("MEDICAID").Value);
                        string bIsOtherPrograms = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("OtherPrograms").Value;
                        string strDatabaseName = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("DatabaseTableName").Value;
                        string strDatabaseFields = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("DatabaseTableFields").Value;
                        string strOpenQuestions = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("OpenQuestions").Value;
                        string strSSPDispName = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("SSPDisplayName").Value;
                        string strWPDispName = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("WPDisplayName").Value;
                        DataMaster.UpdatePageElement(nPageId, _elementid, strElementName, strLength, intControlType, bIsRequired, strReferenceTable, strDisplayRule, strValidations, strValidationTrigger, strErrorCode, intStatus, bIsKTAP, bIsSNAP, bIsMedicAid, bIsOtherPrograms, strDatabaseName, strDatabaseFields, strOpenQuestions, strSSPDispName, strWPDispName, "-1");
                        DataMaster.AddPageElementMapping(nPageId, _elementid);
                    }
                    isvalid= true;
                }
            }
            return isvalid;

        }

        private void ShowConfirmationDialog()
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(this.GetType(), "ConfirmSave", "ConfirmElementUpdate()", true);
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

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (ViewState["ElementData"] != null)
            {
                int nPageId = Convert.ToInt32(ddlPage.SelectedValue);
                XDocument xdoc = XDocument.Parse(Convert.ToString(ViewState["ElementData"]));
                int count = xdoc.Element("ElementSet").Elements("Elements").Count();
                for (int i = 0; i < count; i++)
                {
                    int _elementid = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ElementID").Value);
                    string strElementName = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ElementName").Value;
                    string strLength = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("Length").Value;
                    int intControlType = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ControlType").Value);
                    int bIsRequired = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("IsRequired").Value);
                    string strReferenceTable = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ReferenceTable").Value;
                    string strDisplayRule = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("DisplayRule").Value;
                    string strValidations = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("Validations").Value;
                    string strValidationTrigger = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ValidationTrigger").Value;
                    string strErrorCode = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("ErrorCode").Value;
                    int intStatus = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("Status").Value);
                    int bIsKTAP = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("KTAP").Value);
                    int bIsSNAP = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("SNAP").Value);
                    int bIsMedicAid = Convert.ToInt32(xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("MEDICAID").Value);
                    string bIsOtherPrograms = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("OtherPrograms").Value;
                    string strDatabaseName = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("DatabaseTableName").Value;
                    string strDatabaseFields = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("DatabaseTableFields").Value;
                    string strOpenQuestions = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("OpenQuestions").Value;
                    string strSSPDispName = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("SSPDisplayName").Value;
                    string strWPDispName = xdoc.Element("ElementSet").Elements("Elements").ElementAt(i).Element("WPDisplayName").Value;
                    DataMaster.UpdatePageElement(nPageId, _elementid, strElementName, strLength, intControlType, bIsRequired, strReferenceTable, strDisplayRule, strValidations, strValidationTrigger, strErrorCode, intStatus, bIsKTAP, bIsSNAP, bIsMedicAid, bIsOtherPrograms, strDatabaseName, strDatabaseFields, strOpenQuestions, strSSPDispName, strWPDispName, "-1");
                    DataMaster.AddPageElementMapping(nPageId, _elementid);
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            int roleid = (Session["User"] as User).RoleId;
            if (roleid == 2)
            {
                btnAddExisting.Visible = false;
            }
        }
    }
}