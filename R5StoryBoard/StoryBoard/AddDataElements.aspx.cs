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
    public partial class AddDataElements : System.Web.UI.Page
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

        private DataTable dtPageElements;

        protected void Page_Init(object sender, EventArgs e)
        {
            ucSearch.AddButtonClicked += ucSearch_AddButtonClicked;
            ucSearch.PageSelectionChanged += ucSearch_PageSelectionChanged;
        }

        void ucSearch_PageSelectionChanged(int PageID)
        {
            if (PageID != -1)
            {
                ViewState["PageId"] = PageID;
                PopulateExistingDataElements(PageID);
                gvPageElements.Visible = true;
                btnSubmit.Visible = true;

                btnDelete.Visible = gvPageElements.Rows.Count > 0;
                //tablecontainer.Visible = gvPageElements.Rows.Count > 0;
                lblErrorMessage.Visible = false;
            }
            else
            {
                gvPageElements.Visible = false;
                btnSubmit.Visible = false;

                btnDelete.Visible = false;
            }
        }

        private void PopulateExistingDataElements(int PageID)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetPageElementsByPageId", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageId", PageID);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet("ElementSet");
                        sda.Fill(ds);
                        PageElementsTable = ds.Tables[0];
                        PageElementsTable.TableName = "Elements";
                        gvPageElements.DataSource = PageElementsTable;
                        gvPageElements.DataBind();
                    }
                }
            }
        }

        void ucSearch_AddButtonClicked(object sender, EventArgs e)
        {
            if (ucSearch.SelectedPageId != -1)
            {
                lblErrorMessage.Visible = false;
                CopyExistingData();
                AddNewRow();
                btnSubmit.Visible = true;
                btnDelete.Visible = true;
                //tablecontainer.Visible = true;
                lblErrorMessage.Visible = false;
            }
            else
            {
                lblErrorMessage.Text = "Please select a page.";
                lblErrorMessage.Visible = true;
                btnSubmit.Visible = false;
                btnDelete.Visible = false;
            }
        }

        private void CopyExistingData(List<int> ItemsToBeDeleted = null)
        {
            if (PageElementsTable != null)
            {
                PageElementsTable.Rows.Clear();

                for (int i = 0; i < gvPageElements.Rows.Count; i++)
                {
                    int elementId;
                    int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Values["ElementID"]), out elementId);
                    int associd;
                    int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Values["PageElementMappingId"]), out associd);
                    string strElementName = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtElementName") as TextBox).Text.Trim());
                    string strLength = (gvPageElements.Rows[i].FindControl("txtLength") as TextBox).Text.Trim();
                    int intControlType = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlControlType") as DropDownList).SelectedValue);
                    int bIsRequired = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddllsRequired") as DropDownList).SelectedValue);
                    string strReferenceTable = (gvPageElements.Rows[i].FindControl("ddlReferenceTable") as DropDownList).SelectedValue.Trim();
                    string strDisplayRule = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtDisplayRule") as TextBox).Text.Trim());
                    string strValidations = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtValidations") as TextBox).Text.Trim());
                    string strValidationTrigger = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtValidationTrigger") as TextBox).Text.Trim());
                    string strErrorCode = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtErrorCode") as TextBox).Text.Trim());
                    int intStatus = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlStatus") as DropDownList).SelectedValue);

                    int bIsKTAP = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlIsKTAP") as DropDownList).SelectedValue);
                    int bIsSNAP = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlIsSNAP") as DropDownList).SelectedValue);
                    int bIsMedicAid = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlIsMedicaid") as DropDownList).SelectedValue);
                    string bIsOtherPrograms = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtOtherPrograms") as TextBox).Text);
                    string strDatabaseName = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtDatabaseTableName") as TextBox).Text.Trim());
                    string strDatabaseFields = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtDatabaseFields") as TextBox).Text.Trim());
                    string strOpenQuestions = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtOpenQuestions") as TextBox).Text.Trim());

                    string strSSPDispname = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtSSPDispName") as TextBox).Text.Trim());
                    string strWPDispname = SBHelper.EncodeData((gvPageElements.Rows[i].FindControl("txtWPDisplayName") as TextBox).Text.Trim());

                    string IAElemID = (gvPageElements.Rows[i].FindControl("IAElemID") as TextBox).Text.Trim();

                    DataRow dr = PageElementsTable.NewRow();
                    dr["ElementID"] = elementId;
                    dr["PageElementMappingId"] = associd;
                    dr["ElementName"] = strElementName;
                    dr["Length"] = strLength;
                    dr["ControlType"] = intControlType;
                    dr["IsRequired"] = bIsRequired;
                    dr["KTAP"] = bIsKTAP;
                    dr["SNAP"] = bIsSNAP;
                    dr["MEDICAID"] = bIsMedicAid;
                    dr["OtherPrograms"] = bIsOtherPrograms;
                    dr["ReferenceTable"] = strReferenceTable;
                    dr["DisplayRule"] = strDisplayRule;
                    dr["Validations"] = strValidations;
                    dr["ValidationTrigger"] = strValidationTrigger;
                    dr["ErrorCode"] = strErrorCode;
                    dr["Status"] = intStatus;
                    dr["DatabaseTableName"] = strDatabaseName;
                    dr["DatabaseTableFields"] = strDatabaseFields;
                    dr["OpenQuestions"] = strOpenQuestions;
                    dr["SSPDisplayName"] = strSSPDispname;
                    dr["WPDisplayName"] = strWPDispname;
                    dr["IAElemID"] = IAElemID;

                    if (ItemsToBeDeleted == null || (ItemsToBeDeleted != null && !ItemsToBeDeleted.Contains(i)))
                        PageElementsTable.Rows.Add(dr);
                }
            }
        }

        private void AddNewRow()
        {
            EnsureSchema();
            //add the rew row
            DataRow drElementRow = PageElementsTable.NewRow();
            PageElementsTable.Rows.Add(drElementRow);
            gvPageElements.DataSource = PageElementsTable;
            gvPageElements.DataBind();
        }

        private void EnsureSchema()
        {
            if (PageElementsTable == null)
            {
                dtPageElements = new DataTable();
                DataColumn dc = new DataColumn("ElementID", typeof(int));
                dc.DefaultValue = -1;

                DataColumn dcmapp = new DataColumn("PageElementMappingId", typeof(int));
                dcmapp.DefaultValue = -1;
                dtPageElements.Columns.Add(dc);
                dtPageElements.Columns.Add(dcmapp);
                dtPageElements.Columns.Add("ElementName", typeof(string));
                dtPageElements.Columns.Add("Length", typeof(string));
                dtPageElements.Columns.Add("ControlType", typeof(int));
                DataColumn dcIsReq = new DataColumn("IsRequired", typeof(int));
                dcIsReq.DefaultValue = false;

                DataColumn KTAP = new DataColumn("KTAP", typeof(int));
                KTAP.DefaultValue = 1;

                DataColumn SNAP = new DataColumn("SNAP", typeof(int));
                SNAP.DefaultValue = 1;

                DataColumn MEDICAID = new DataColumn("MEDICAID", typeof(int));
                MEDICAID.DefaultValue = 1;

                DataColumn OtherPrograms = new DataColumn("OtherPrograms", typeof(int));
                OtherPrograms.DefaultValue = 1;

                dtPageElements.Columns.Add(dcIsReq);
                dtPageElements.Columns.Add("ReferenceTable", typeof(string));
                dtPageElements.Columns.Add("DisplayRule", typeof(string));
                dtPageElements.Columns.Add("Validations", typeof(string));
                dtPageElements.Columns.Add("ValidationTrigger", typeof(string));
                dtPageElements.Columns.Add("ErrorCode", typeof(string));
                dtPageElements.Columns.Add("Status", typeof(int));
                dtPageElements.Columns.Add(KTAP);
                dtPageElements.Columns.Add(SNAP);
                dtPageElements.Columns.Add(MEDICAID);
                dtPageElements.Columns.Add(OtherPrograms);
                dtPageElements.Columns.Add("DatabaseTableName", typeof(string));
                dtPageElements.Columns.Add("DatabaseTableFields", typeof(string));
                dtPageElements.Columns.Add("OpenQuestions", typeof(string));
                dtPageElements.Columns.Add("SSPDisplayName", typeof(string));
                dtPageElements.Columns.Add("WPDisplayName", typeof(string));
                dtPageElements.Columns.Add("IAElemID", typeof(string));
                PageElementsTable = dtPageElements;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateMasters();
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

                DropDownList ddlSSPControlType = e.Row.FindControl("ddlSSPControlType") as DropDownList;
                ddlSSPControlType.DataSource = dtControlTypes;
                ddlSSPControlType.DataBind();
                var selectedsspcontroltype = ddlSSPControlType.Items.FindByValue(Convert.ToString(drv["SSP_ControlType"]));
                if (selectedsspcontroltype != null)
                    selectedsspcontroltype.Selected = true;


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

                DropDownList ddlSSPlsRequired = e.Row.FindControl("ddlSSPlsRequired") as DropDownList;
                ddlSSPlsRequired.DataSource = dtYesNo;
                ddlSSPlsRequired.DataBind();
                var selectedsspisrequired = ddlSSPlsRequired.Items.FindByValue(Convert.ToString(drv["SSP_IsRequired"]));
                if (selectedsspisrequired != null)
                    selectedsspisrequired.Selected = true;


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

        bool ParseBoolean(object data)
        {
            if (data == DBNull.Value || data == null || Convert.ToBoolean(data) == false)
            {
                return false;
            }
            return true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateElements())
            {

                if (ViewState["PageId"] != null)
                {
                    try
                    {
                        CommitDataToDB();
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.LogError("AddDataElements", ex);
                        lblErrorMessage.Text = "Error occurred while saving data.";
                        lblErrorMessage.Visible = true;
                    }
                    finally
                    {

                    }
                }
                else
                {
                    //Page not selected
                    lblErrorMessage.Text = "Please select a page.";
                    lblErrorMessage.Visible = true;
                    btnSubmit.Visible = false;

                }
            }

        }

        private void CommitDataToDB()
        {
            int nPageId = Convert.ToInt32(ViewState["PageId"]);
            for (int i = 0; i < gvPageElements.Rows.Count; i++)
            {
                int elementId;
                int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Values["ElementID"]), out elementId);
                int associd;
                int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Values["PageElementMappingId"]), out associd);
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

                int bIsKTAP = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlIsKTAP") as DropDownList).SelectedValue);
                int bIsSNAP = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlIsSNAP") as DropDownList).SelectedValue);
                int bIsMedicAid = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlIsMedicaid") as DropDownList).SelectedValue);
                string bIsOtherPrograms = (gvPageElements.Rows[i].FindControl("txtOtherPrograms") as TextBox).Text;
                string strDatabaseName = (gvPageElements.Rows[i].FindControl("txtDatabaseTableName") as TextBox).Text.Trim();
                string strDatabaseFields = (gvPageElements.Rows[i].FindControl("txtDatabaseFields") as TextBox).Text.Trim();
                string strOpenQuestions = (gvPageElements.Rows[i].FindControl("txtOpenQuestions") as TextBox).Text.Trim();

                string strSSPDispName = (gvPageElements.Rows[i].FindControl("txtSSPDispName") as TextBox).Text.Trim();

                string strWPDispName = (gvPageElements.Rows[i].FindControl("txtWPDisplayName") as TextBox).Text.Trim();
                string IAElemID = (gvPageElements.Rows[i].FindControl("IAElemID") as TextBox).Text.Trim();

                int intSSPControlType = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlSSPControlType") as DropDownList).SelectedValue);
                int bSSPIsRequired = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddlSSPlsRequired") as DropDownList).SelectedValue);
                string strSSPDisplayRule = (gvPageElements.Rows[i].FindControl("txtSSPDisplayRule") as TextBox).Text.Trim();
                string strSSPValidations = (gvPageElements.Rows[i].FindControl("txtSSPValidations") as TextBox).Text.Trim();
                string strSSPErrorCode = (gvPageElements.Rows[i].FindControl("txtSSPErrorCode") as TextBox).Text.Trim();

                if (strElementName.Trim().Length > 0)
                {
                    if (elementId == 0)
                    {
                        //insert

                        DataMaster.InsertPageElement(nPageId, strElementName, strLength, intControlType, bIsRequired, strReferenceTable,
                            strDisplayRule, strValidations, strValidationTrigger, strErrorCode, intStatus, bIsKTAP,
                            bIsSNAP, bIsMedicAid, bIsOtherPrograms, strDatabaseName, strDatabaseFields, strOpenQuestions, strSSPDispName,
                            strWPDispName, IAElemID, intSSPControlType, bSSPIsRequired, strSSPDisplayRule, strSSPValidations, strSSPErrorCode, associd);
                    }
                    else
                    {
                        //update
                        DataMaster.UpdatePageElement(nPageId, elementId, strElementName, strLength, intControlType, bIsRequired, strReferenceTable,
                            strDisplayRule, strValidations, strValidationTrigger, strErrorCode, intStatus, bIsKTAP, bIsSNAP, bIsMedicAid, bIsOtherPrograms,
                            strDatabaseName, strDatabaseFields, strOpenQuestions, strSSPDispName, strWPDispName, IAElemID, intSSPControlType, bSSPIsRequired, strSSPDisplayRule, strSSPValidations, strSSPErrorCode, associd);
                    }
                }
            }

            lblErrorMessage.Text = "Data saved successfully.";
            lblErrorMessage.Visible = true;
            //if the user clicks submit again, it has to be update and not insert and hence we rebind it from database
            PopulateExistingDataElements(nPageId);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            int roleid = (Session["User"] as User).RoleId;
            if (roleid == 2)
            {
                btnDelete.Visible = false;
                ucSearch.ShowAddButton = false;
                btnAddExisting.Visible = false;
                btnSubmit.Visible = false;
            }
        }

        private bool ValidateElements()
        {
            CopyExistingData(); //update the data set first
            DataSet ds;
            if (PageElementsTable.DataSet == null)
            {
                ds = new DataSet();
                ds.Tables.Add(PageElementsTable);
            }
            else
                ds = PageElementsTable.DataSet;

            ds.DataSetName = "ElementSet";
            ds.Tables[0].TableName = "Elements";
            string xmlData = ds.GetXml();
            DataSet validationresults = DataMaster.ValidateModifiedElements(xmlData);
            if (validationresults.Tables[0].Rows.Count > 0)
            {
                grdElemValidationResults.DataSource = validationresults.Tables[0];
                grdElemValidationResults.DataBind();
                ShowConfirmationDialog();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ShowConfirmationDialog()
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(this.GetType(), "ConfirmSave", "ConfirmElementUpdate()", true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> lstItemsToBeDeleted = new List<int>();
            for (int i = 0; i < gvPageElements.Rows.Count; i++)
            {
                CheckBox chkIsToBeDeleted = (CheckBox)gvPageElements.Rows[i].FindControl("chkSelect");
                int elementId;
                int elementMappingId;
                int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Values["ElementID"]), out elementId);
                int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Values["PageElementMappingId"]), out elementMappingId);
                if (chkIsToBeDeleted.Checked)
                {
                    //delete from database
                    string strUserId = Session["User"] != null ? ((User)Session["User"]).UserName : "";
                    if (elementId > 0)
                        DataMaster.DeleteElement(elementMappingId, strUserId);

                    //add element to delete list
                    lstItemsToBeDeleted.Add(i);
                }

            }
            //refresh the data to reflect updated data
            CopyExistingData(lstItemsToBeDeleted);
            gvPageElements.DataSource = PageElementsTable;
            gvPageElements.DataBind();
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

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            CommitDataToDB();
        }
    }
}