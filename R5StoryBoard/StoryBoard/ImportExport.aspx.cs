using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoryBoard
{
    public partial class ImportExport : System.Web.UI.Page
    {
        string Sheet1 = ConfigurationManager.AppSettings["Sheet1Const"];
        string Sheet2 = ConfigurationManager.AppSettings["Sheet2Const"];
        string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
        
        protected void Page_Init(object sender, EventArgs e)
        {
            ucSearch.PageSelectionChanged += ucSearch_PageSelectionChanged;
        }

        void ucSearch_PageSelectionChanged(int PageID)
        {
            if (PageID != -1)
            {
                ViewState["PageId"] = PageID;
                lblErrorMessage.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                //string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                //string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);

                //string FilePath = Server.MapPath(FolderPath + FileName);
                //FileUpload1.SaveAs(FilePath);
            }
            string Extension = ".xlsx";
            string FilePath = "C:\\Users\\rmaurya\\Documents\\GitHub\\StoryBoard\\R5StoryBoard\\StoryBoard\\Files\\16_DC_Individual Education.xlsx";
            Import_To_Grid(FilePath, Extension);

        }

        private void Import_To_Grid(string FilePath, string Extension)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"]
                             .ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"]
                              .ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt;
            cmdExcel.Connection = connExcel;

            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //Populate Page Details
            dt = new DataTable();
            cmdExcel.CommandText = "SELECT * From [" + Sheet1 + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            PopulatePage(dt);

            dt = new DataTable();
            cmdExcel.CommandText = "SELECT * From [" + Sheet2 + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);

            PopulateElements(dt);

            //foreach (DataRow rw in dtExcelSchema.Rows)
            //{
            //    string SheetName = rw["TABLE_NAME"].ToString();

            //    //Page Sheet
            //    if (SheetName.Equals(Sheet1))
            //    {
            //        dt = new DataTable();
            //        cmdExcel.CommandText = "SELECT * From [" + Sheet1 + "]";
            //        oda.SelectCommand = cmdExcel;
            //        oda.Fill(dt);

            //        PopulatePage(dt);
            //    }

            //    //Elements Sheet
            //    if (SheetName.Equals(Sheet2))
            //    {
            //        dt = new DataTable();
            //        cmdExcel.CommandText = "SELECT * From [" + Sheet2 + "]";
            //        oda.SelectCommand = cmdExcel;
            //        oda.Fill(dt);

            //        PopulateElements(dt);

            //    }
            //}

            connExcel.Close();
        }

        private void PopulatePage(DataTable dt)
        {
            int nStatus = 0;
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stp_InsertPageDetails", sqlconn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageDesignation", "");
                    cmd.Parameters.AddWithValue("@PageName", (dt.Rows[0].ItemArray[1]).ToString());
                    cmd.Parameters.AddWithValue("@PageDescription", (dt.Rows[1].ItemArray[1]).ToString());
                    cmd.Parameters.AddWithValue("@BusinessProcess", (dt.Rows[2].ItemArray[1]).ToString());
                    cmd.Parameters.AddWithValue("@Activity", (dt.Rows[3].ItemArray[1]).ToString());
                    cmd.Parameters.AddWithValue("@Programs", (dt.Rows[4].ItemArray[1]).ToString());
                    cmd.Parameters.AddWithValue("@Module", ucSearch.ModuleId);
                    cmd.Connection.Open();
                    nStatus = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Connection.Close();
                    if (nStatus == 0)
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = string.Format(" Page with name '{0}' already exists", (dt.Rows[0].ItemArray[1]).ToString());
                    }
                    else
                    {
                        ViewState["PageId"] = nStatus;
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.ForeColor = System.Drawing.Color.Green;
                        lblErrorMessage.Text = string.Format(" Page with name '{0}' added", (dt.Rows[0].ItemArray[1]).ToString());
                    }
                }
            }
        }

        private void PopulateElements(DataTable dt)
        {
            //Validations - control type,Status,ref table
            DataTable dtCtrl = DataMaster.GetControlList();
            DataTable dtStatus = DataMaster.GetStatusList();
            DataTable dtRef = DataMaster.GetRefTableList();
            StringBuilder err = new StringBuilder();
            bool invalid = false;

            #region Validations
            foreach (DataRow rw in dt.Rows)
            {
                string ctrl = rw.ItemArray[3].ToString().Replace(" ", ""); // Remove White spaces for control names
                if (dtCtrl.Select("ControlTypeDesc = '" + ctrl + "'").Length == 0 & !string.IsNullOrEmpty(ctrl))
                {
                    invalid = true;

                    if (!err.ToString().Contains(ctrl))
                    {
                        err.Append(ctrl).Append(" is not a valid control type").Append("<br/>");
                    }
                }

                string st = rw.ItemArray[10].ToString().Replace(" ", ""); // Remove White spaces 
                if (dtStatus.Select("StatusName = '" + st + "'").Length == 0 & !string.IsNullOrEmpty(st))
                {
                    invalid = true;
                    if (!err.ToString().Contains(st))
                    {
                        err.Append(st).Append(" is not a valid status").Append("<br/>");
                    }
                }

                string reft = rw.ItemArray[5].ToString().Replace(" ", ""); // Remove White spaces 
                if (dtRef.Select("ReferenceTableName = '" + reft + "'").Length == 0 & !string.IsNullOrEmpty(reft))
                {
                    invalid = true;
                    if (!err.ToString().Contains(reft))
                    {
                        err.Append(reft).Append(" is not a reference table").Append("<br/>");
                    }
                }


                string temp = rw.ItemArray[11].ToString();
                if (DataMaster.YesNoMap(temp) == 0) 
                {
                    invalid = true;
                    err.Append(temp).Append(" is not a valid value for KTAP").Append("<br/>");
                }

                temp = rw.ItemArray[12].ToString();
                if (DataMaster.YesNoMap(temp) == 0)
                {
                    invalid = true;
                    err.Append(temp).Append(" is not a valid value for SNAP").Append("<br/>");
                }

                temp = rw.ItemArray[13].ToString();
                if (DataMaster.YesNoMap(temp) == 0)
                {
                    invalid = true;
                    err.Append(temp).Append(" is not a valid value for MedicAid").Append("<br/>");
                }
            }
            #endregion

            if (invalid)
            {
                err.Append("Please make corrections and try again.");
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = err.ToString();
                return;
            }

            #region insert elements
            else //Insert Elements
            {
                DataTable dtSearchResults = DataMaster.SearchElements("", "-1");
                foreach (DataRow dr in dt.Rows)
                {
                    string IAElemID = "0";
                    string elName = (!string.IsNullOrEmpty(dr.ItemArray[1].ToString())) ? dr.ItemArray[1].ToString().Trim() : string.Empty;

                    if (!string.IsNullOrEmpty(elName))
                    {
                        DataRow[] rows = dtSearchResults.Select("ElementName = '" + elName + "'");
                        if (rows.Length == 0) //Insert element
                        {
                            if (ViewState["PageId"] != null)
                            {
                                string temp;
                                int nPageId = Convert.ToInt32(ViewState["PageId"]);
                                string strElementName = elName;

                                temp = dr.ItemArray[2].ToString();
                                string strLength = (!string.IsNullOrEmpty(temp)) ? temp.Trim() : string.Empty;

                                temp = dr.ItemArray[3].ToString().Replace(" ", "");
                                int intControlType = (!string.IsNullOrEmpty(temp)) ?
                                    Convert.ToInt32(dtCtrl.Select("ControlTypeDesc = '" + temp + "'")[0].ItemArray[1].ToString().Trim()) : 0;

                                temp = dr.ItemArray[4].ToString();
                                int bIsRequired = DataMaster.YesNoMap(temp);

                                temp = dr.ItemArray[5].ToString();
                                string strReferenceTable = (!string.IsNullOrEmpty(temp)) ?
                                    (dtRef.Select("ReferenceTableName = '" + temp + "'")[0].ItemArray[1].ToString().Trim()) : string.Empty;

                                temp = dr.ItemArray[6].ToString();
                                string strDisplayRule = (!string.IsNullOrEmpty(temp)) ? temp.Trim() : string.Empty;

                                temp = dr.ItemArray[7].ToString();
                                string strValidations = (!string.IsNullOrEmpty(temp)) ? temp.Trim() : string.Empty;

                                temp = dr.ItemArray[8].ToString();
                                string strValidationTrigger = (!string.IsNullOrEmpty(temp)) ? temp.Trim() : string.Empty;

                                temp = dr.ItemArray[9].ToString();
                                string strErrorCode = (!string.IsNullOrEmpty(temp)) ? temp.Trim() : string.Empty;

                                temp = dr.ItemArray[10].ToString();
                                int intStatus = (!string.IsNullOrEmpty(temp)) ?
                                    Convert.ToInt32(dtStatus.Select("StatusName = '" + temp + "'")[0].ItemArray[1].ToString().Trim()) : 0;

                                temp = dr.ItemArray[11].ToString();
                                int bIsKTAP = DataMaster.YesNoMap(temp);

                                temp = dr.ItemArray[12].ToString();
                                int bIsSNAP = DataMaster.YesNoMap(temp);

                                temp = dr.ItemArray[13].ToString();
                                int bIsMedicAid = DataMaster.YesNoMap(temp);

                                temp = dr.ItemArray[14].ToString();
                                string bIsOtherPrograms = (!string.IsNullOrEmpty(temp)) ? temp.Trim() : string.Empty;

                                temp = dr.ItemArray[15].ToString();
                                string strDatabaseName = (!string.IsNullOrEmpty(temp)) ? temp.Trim() : string.Empty;

                                temp = dr.ItemArray[16].ToString();
                                string strDatabaseFields = (!string.IsNullOrEmpty(temp)) ? temp.Trim() : string.Empty;

                                temp = dr.ItemArray[17].ToString();
                                string strOpenQuestions = (!string.IsNullOrEmpty(temp)) ? temp.Trim() : string.Empty;

                                string strSSPDispName = elName;
                                string strWPDispName = elName;

                                IAElemID = "0";
                                DataMaster.InsertPageElement(nPageId, strElementName, strLength, intControlType, bIsRequired, strReferenceTable, strDisplayRule, strValidations, strValidationTrigger, strErrorCode, intStatus, bIsKTAP, bIsSNAP, bIsMedicAid, bIsOtherPrograms, strDatabaseName, strDatabaseFields, strOpenQuestions, strSSPDispName, strWPDispName, IAElemID);
                            }
                            else
                            {
                                lblErrorMessage.Visible = true;
                                lblErrorMessage.Text = "Please select a page and module";
                            }
                        }
                        else //pick first element ID
                        {
                            string ElemID = rows[0].ItemArray[0].ToString();

                            if (ViewState["PageId"] != null)
                            {
                                int nPgId = Convert.ToInt32(ViewState["PageId"]);
                                DataMaster.InsertPageElement(nPageId: nPgId, IAElemID: ElemID);
                            }
                            else
                            {
                                lblErrorMessage.Visible = true;
                                lblErrorMessage.Text = "Please select a page and module";
                            }
                        }
                    }

                }

            }
            #endregion
        }
    }
}