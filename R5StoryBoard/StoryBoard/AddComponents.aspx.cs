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
    public partial class AddComponents : System.Web.UI.Page
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
            }
            else
            {
                gvPageElements.Visible = false;
                btnSubmit.Visible = false;
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
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        gvPageElements.DataSource = ds;
                        gvPageElements.DataBind();
                        PageElementsTable = ds.Tables[0];
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
            }
            else
            {
                lblErrorMessage.Text = "Please select a page.";
                lblErrorMessage.Visible = true;
                btnSubmit.Visible = false;
            }
        }

        private void CopyExistingData()
        {
            if (PageElementsTable != null)
            {
                PageElementsTable.Rows.Clear();

                for (int i = 0; i < gvPageElements.Rows.Count; i++)
                {
                    int elementId;
                    int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Value), out elementId);
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
                    int bIsOtherPrograms = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddllsRequired") as DropDownList).SelectedValue);
                    string strDatabaseName = (gvPageElements.Rows[i].FindControl("txtDatabaseTableName") as TextBox).Text.Trim();
                    string strDatabaseFields = (gvPageElements.Rows[i].FindControl("txtDatabaseFields") as TextBox).Text.Trim();
                    string strOpenQuestions = (gvPageElements.Rows[i].FindControl("txtOpenQuestions") as TextBox).Text.Trim();


                    DataRow dr = PageElementsTable.NewRow();
                    dr["ElementID"] = elementId;
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

                dtPageElements.Columns.Add(dc);
                dtPageElements.Columns.Add("ElementName", typeof(string));
                dtPageElements.Columns.Add("Length", typeof(string));
                dtPageElements.Columns.Add("ControlType", typeof(int));
                DataColumn dcIsReq = new DataColumn("IsRequired", typeof(bool));
                dcIsReq.DefaultValue = false;

                DataColumn KTAP = new DataColumn("KTAP", typeof(bool));
                KTAP.DefaultValue = false;

                DataColumn SNAP = new DataColumn("SNAP", typeof(bool));
                SNAP.DefaultValue = false;

                DataColumn MEDICAID = new DataColumn("MEDICAID", typeof(bool));
                MEDICAID.DefaultValue = false;

                DataColumn OtherPrograms = new DataColumn("OtherPrograms", typeof(bool));
                OtherPrograms.DefaultValue = false;

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


                DropDownList ddllsOtherProg = e.Row.FindControl("ddlIsOtherProgram") as DropDownList;
                ddllsOtherProg.DataSource = dtYesNo;
                ddllsOtherProg.DataBind();
                var selectedisotherprog = ddllsOtherProg.Items.FindByValue(Convert.ToString(drv["OtherPrograms"]));
                if (selectedisotherprog != null)
                    selectedisotherprog.Selected = true;
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
            if (ViewState["PageId"] != null)
            {
                int nPageId = Convert.ToInt32(ViewState["PageId"]);
                try
                {
                    for (int i = 0; i < gvPageElements.Rows.Count; i++)
                    {
                        int elementId;
                        int.TryParse(Convert.ToString(gvPageElements.DataKeys[i].Value), out elementId);

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
                        int bIsOtherPrograms = Convert.ToInt32((gvPageElements.Rows[i].FindControl("ddllsRequired") as DropDownList).SelectedValue);
                        string strDatabaseName = (gvPageElements.Rows[i].FindControl("txtDatabaseTableName") as TextBox).Text.Trim();
                        string strDatabaseFields = (gvPageElements.Rows[i].FindControl("txtDatabaseFields") as TextBox).Text.Trim();
                        string strOpenQuestions = (gvPageElements.Rows[i].FindControl("txtOpenQuestions") as TextBox).Text.Trim();

                        if (strElementName.Trim().Length > 0)
                        {
                            if (elementId == 0)
                            {
                                //insert

                                InsertPageElement(nPageId, strElementName, strLength, intControlType, bIsRequired, strReferenceTable, strDisplayRule, strValidations, strValidationTrigger, strErrorCode, intStatus, bIsKTAP, bIsSNAP, bIsMedicAid, bIsOtherPrograms, strDatabaseName, strDatabaseFields, strOpenQuestions);
                            }
                            else
                            {
                                //update
                                UpdatePageElement(nPageId, elementId, strElementName, strLength, intControlType, bIsRequired, strReferenceTable, strDisplayRule, strValidations, strValidationTrigger, strErrorCode, intStatus, bIsKTAP, bIsSNAP, bIsMedicAid, bIsOtherPrograms, strDatabaseName, strDatabaseFields, strOpenQuestions);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Text = "Error occurred while saving data.";
                    lblErrorMessage.Visible = true;
                }
                finally
                {
                    lblErrorMessage.Text = "Data saved successfully.";
                    lblErrorMessage.Visible = true;
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

        private void InsertPageElement(int nPageId, string strElementName, string strLength, int intControlType,
            int bIsRequired, string strReferenceTable, string strDisplayRule, string strValidations, string strValidationTrigger,
            string strErrorCode, int intStatus, int bIsKTAP, int bIsSNAP, int bIsMedicAid, int bIsOtherPrograms, string strDatabaseName, string strDatabaseFields, string strOpenQuestions)
        {
            int nLength = -1;
            int.TryParse(strLength, out nLength);

            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_InsertPageElementDetails", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageID", nPageId);
                        cmd.Parameters.AddWithValue("@ElementName", strElementName);
                        cmd.Parameters.AddWithValue("@Length", strLength);
                        cmd.Parameters.AddWithValue("@ControlType", intControlType);
                        cmd.Parameters.AddWithValue("@IsRequired", bIsRequired);
                        cmd.Parameters.AddWithValue("@ReferenceTable", strReferenceTable);
                        cmd.Parameters.AddWithValue("@DisplayRule", strDisplayRule);
                        cmd.Parameters.AddWithValue("@Validations", strValidations);
                        cmd.Parameters.AddWithValue("@ValidationTrigger", strValidationTrigger);
                        cmd.Parameters.AddWithValue("@ErrorCode", strErrorCode);
                        cmd.Parameters.AddWithValue("@Status", intStatus);
                        cmd.Parameters.AddWithValue("@KTAP", bIsKTAP);
                        cmd.Parameters.AddWithValue("@SNAP", bIsSNAP);
                        cmd.Parameters.AddWithValue("@MEDICAID", bIsMedicAid);
                        cmd.Parameters.AddWithValue("@OtherPrograms", bIsOtherPrograms);
                        cmd.Parameters.AddWithValue("@DatabaseTableName", strDatabaseName);
                        cmd.Parameters.AddWithValue("@DatabaseTableFields", strDatabaseFields);
                        cmd.Parameters.AddWithValue("@OpenQuestions", strOpenQuestions);
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }
            }
        }


        private void UpdatePageElement(int nPageId, int nElementId, string strElementName, string strLength, int intControlType,
            int bIsRequired, string strReferenceTable, string strDisplayRule, string strValidations, string strValidationTrigger,
            string strErrorCode, int intStatus, int bIsKTAP, int bIsSNAP, int bIsMedicAid, int bIsOtherPrograms, string strDatabaseName, string strDatabaseFields, string strOpenQuestions)
        {
            int nLength = -1;
            int.TryParse(strLength, out nLength);

            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_UpdatePageElementDetails", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageID", nPageId);
                        cmd.Parameters.AddWithValue("@ElementID", nElementId);
                        cmd.Parameters.AddWithValue("@ElementName", strElementName);
                        cmd.Parameters.AddWithValue("@Length", strLength);
                        cmd.Parameters.AddWithValue("@ControlType", intControlType);
                        cmd.Parameters.AddWithValue("@IsRequired", bIsRequired);
                        cmd.Parameters.AddWithValue("@ReferenceTable", strReferenceTable);
                        cmd.Parameters.AddWithValue("@DisplayRule", strDisplayRule);
                        cmd.Parameters.AddWithValue("@Validations", strValidations);
                        cmd.Parameters.AddWithValue("@ValidationTrigger", strValidationTrigger);
                        cmd.Parameters.AddWithValue("@ErrorCode", strErrorCode);
                        cmd.Parameters.AddWithValue("@Status", intStatus);
                        cmd.Parameters.AddWithValue("@KTAP", bIsKTAP);
                        cmd.Parameters.AddWithValue("@SNAP", bIsSNAP);
                        cmd.Parameters.AddWithValue("@MEDICAID", bIsMedicAid);
                        cmd.Parameters.AddWithValue("@OtherPrograms", bIsOtherPrograms);
                        cmd.Parameters.AddWithValue("@DatabaseTableName", strDatabaseName);
                        cmd.Parameters.AddWithValue("@DatabaseTableFields", strDatabaseFields);
                        cmd.Parameters.AddWithValue("@OpenQuestions", strOpenQuestions);
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }
            }
        }
    }
}