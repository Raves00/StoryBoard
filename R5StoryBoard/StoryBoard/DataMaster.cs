using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StoryBoard
{
    public class DataMaster
    {
        public static int YesNoMap(string temp)
        {
            if (!string.IsNullOrEmpty(temp))
            {
                switch (temp.Trim())
                {
                    case "Yes": return 1;
                    case "No": return 2;
                    case "N/A": return 3;
                }
            }
            return 0;
        }

        public static bool DeleteElement(int nElementMappingID)
        {
            bool issucess = false;
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        using (SqlCommand cmd = new SqlCommand("stp_DeleteElement", sqlconn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PageElementMappingID", nElementMappingID);
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();
                        }
                    }
                }
                issucess = true;
            }
            catch (Exception)
            {
                issucess = false;
            }
            return issucess;
        }

        public static DataTable GetPageList()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        using (SqlCommand cmd = new SqlCommand("stpGetStoryBoardPages", sqlconn))
                        {
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                        }
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static DataTable GetControlList()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        using (SqlCommand cmd = new SqlCommand("Select ControlTypeDesc,ControlTypeID From ControlTypeMaster", sqlconn))
                        {
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                        }
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DataTable GetStatusList()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        using (SqlCommand cmd = new SqlCommand("Select StatusName,StatusID From StatusMaster", sqlconn))
                        {
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                        }
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DataTable GetRefTableList()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        using (SqlCommand cmd = new SqlCommand("stp_GetReferenceTables", sqlconn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                        }
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static DataTable SearchElements(string strElementName, string strPage)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_SearchElementsByName", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ElementName", strElementName);
                        cmd.Parameters.AddWithValue("@PageId", strPage);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }

        }

        internal static void AddPageElementMapping(int nPageId, int elementId)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("stp_AddPageElementMapping", sqlconn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageId", nPageId);
                    cmd.Parameters.AddWithValue("@ElementId", elementId);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

            }
        }

        internal static void UploadImageForPage(int nPageId, string strImageName, byte[] ImageData)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stp_PageImageUpload", sqlconn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageId", nPageId);
                    cmd.Parameters.AddWithValue("@ImageName", strImageName);
                    cmd.Parameters.AddWithValue("@ImageContent", ImageData);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

            }
        }

        internal static KeyValuePair<string, byte[]> GetImageFromDB(int imgId)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetScreenImageById", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", imgId);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return new KeyValuePair<string, byte[]>(Convert.ToString(ds.Tables[0].Rows[0]["ImageName"]), (byte[])ds.Tables[0].Rows[0]["ImageContent"]);
                        }
                        else
                            return new KeyValuePair<string, byte[]>(null, null);
                    }
                }
            }
        }

        internal static DataTable GetImageListForPage(int pPageId)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetScreenImageByPageId", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageId", pPageId);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        internal static void DeleteScreenImage(int imagetobedeleted)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stp_DeleteScreenImage", sqlconn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ImageId", imagetobedeleted);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

            }
        }

        internal static DataTable GetPagesForModule(int pModule)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetStoryBoardPagesByModule", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ModuleId", pModule);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        internal static void UpdatePageElement(int nPageId, int nElementId, string strElementName, string strLength, int intControlType, int bIsRequired, string strReferenceTable, string strDisplayRule, string strValidations, string strValidationTrigger, string strErrorCode, int intStatus, int bIsKTAP, int bIsSNAP, int bIsMedicAid, string bIsOtherPrograms, string strDatabaseName, string strDatabaseFields, string strOpenQuestions, string strSSPDispName, string strWPDispName, string IAElemID)
        {
            int nLength = -1;
            int.TryParse(strLength, out nLength);

            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("stp_UpdatePageElementDetails", sqlconn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageID", nPageId);
                    cmd.Parameters.AddWithValue("@ElementID", nElementId);
                    cmd.Parameters.AddWithValue("@ElementName", SBHelper.EncodeData(strElementName));
                    cmd.Parameters.AddWithValue("@Length", SBHelper.EncodeData(strLength));
                    cmd.Parameters.AddWithValue("@ControlType", intControlType);
                    cmd.Parameters.AddWithValue("@IsRequired", bIsRequired);
                    cmd.Parameters.AddWithValue("@ReferenceTable", SBHelper.EncodeData(strReferenceTable));
                    cmd.Parameters.AddWithValue("@DisplayRule", SBHelper.EncodeData(strDisplayRule));
                    cmd.Parameters.AddWithValue("@Validations", SBHelper.EncodeData(strValidations));
                    cmd.Parameters.AddWithValue("@ValidationTrigger", SBHelper.EncodeData(strValidationTrigger));
                    cmd.Parameters.AddWithValue("@ErrorCode", SBHelper.EncodeData(strErrorCode));
                    cmd.Parameters.AddWithValue("@Status", intStatus);
                    cmd.Parameters.AddWithValue("@KTAP", bIsKTAP);
                    cmd.Parameters.AddWithValue("@SNAP", bIsSNAP);
                    cmd.Parameters.AddWithValue("@MEDICAID", bIsMedicAid);
                    cmd.Parameters.AddWithValue("@OtherPrograms", SBHelper.EncodeData(bIsOtherPrograms));
                    cmd.Parameters.AddWithValue("@DatabaseTableName", SBHelper.EncodeData(strDatabaseName));
                    cmd.Parameters.AddWithValue("@DatabaseTableFields", SBHelper.EncodeData(strDatabaseFields));
                    cmd.Parameters.AddWithValue("@OpenQuestions", SBHelper.EncodeData(strOpenQuestions));
                    cmd.Parameters.AddWithValue("@SSPDispName", SBHelper.EncodeData(strSSPDispName));
                    cmd.Parameters.AddWithValue("@WPDispName", SBHelper.EncodeData(strWPDispName));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

            }
        }

        internal static void InsertPageElement(int nPageId = 0, string strElementName = null, string strLength = null, int intControlType = 0, int bIsRequired = 0, string strReferenceTable = null, string strDisplayRule = null, string strValidations = null, string strValidationTrigger = null, string strErrorCode = null, int intStatus = 0, int bIsKTAP = 0, int bIsSNAP = 0, int bIsMedicAid = 0, string bIsOtherPrograms = null, string strDatabaseName = null, string strDatabaseFields = null, string strOpenQuestions = null, string strSSPDispName = null, string strWPDispName = null, string IAElemID = null)
        {
            int nLength = -1;
            int.TryParse(strLength, out nLength);

            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("stp_InsertPageElementDetails", sqlconn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageID", nPageId);
                    cmd.Parameters.AddWithValue("@ElementName", SBHelper.EncodeData(strElementName));
                    cmd.Parameters.AddWithValue("@Length", SBHelper.EncodeData(strLength));
                    cmd.Parameters.AddWithValue("@ControlType", intControlType);
                    cmd.Parameters.AddWithValue("@IsRequired", bIsRequired);
                    cmd.Parameters.AddWithValue("@ReferenceTable", SBHelper.EncodeData(strReferenceTable));
                    cmd.Parameters.AddWithValue("@DisplayRule", SBHelper.EncodeData(strDisplayRule));
                    cmd.Parameters.AddWithValue("@Validations", SBHelper.EncodeData(strValidations));
                    cmd.Parameters.AddWithValue("@ValidationTrigger", SBHelper.EncodeData(strValidationTrigger));
                    cmd.Parameters.AddWithValue("@ErrorCode", SBHelper.EncodeData(strErrorCode));
                    cmd.Parameters.AddWithValue("@Status", intStatus);
                    cmd.Parameters.AddWithValue("@KTAP", bIsKTAP);
                    cmd.Parameters.AddWithValue("@SNAP", bIsSNAP);
                    cmd.Parameters.AddWithValue("@MEDICAID", bIsMedicAid);
                    cmd.Parameters.AddWithValue("@OtherPrograms", SBHelper.EncodeData(bIsOtherPrograms));
                    cmd.Parameters.AddWithValue("@DatabaseTableName", SBHelper.EncodeData(strDatabaseName));
                    cmd.Parameters.AddWithValue("@DatabaseTableFields", SBHelper.EncodeData(strDatabaseFields));
                    cmd.Parameters.AddWithValue("@OpenQuestions", SBHelper.EncodeData(strOpenQuestions));
                    cmd.Parameters.AddWithValue("@SSPDispName", SBHelper.EncodeData(strSSPDispName));
                    cmd.Parameters.AddWithValue("@WPDispName", SBHelper.EncodeData(strWPDispName));
                    cmd.Parameters.AddWithValue("@IAElemID", SBHelper.EncodeData(IAElemID));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

            }
        }


        internal static DataSet ValidateModifiedElements(string xmlData)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_ValidateElementAssociation", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@strElementText", xmlData);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        internal static User ValidateUser(string strUserName, string strPassword)
        {
            User validatedUser = null;
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_ValidateUser", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", strUserName);
                        cmd.Parameters.AddWithValue("@Password", strPassword);
                        sqlconn.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.Read())
                        {
                            validatedUser = new User(sdr.GetFieldValue<int>(0), sdr.GetFieldValue<string>(1), sdr.GetFieldValue<string>(2), sdr.GetFieldValue<byte>(3));
                        }

                        sqlconn.Close();
                        return validatedUser;
                    }
                }
            }
        }

        internal static DataTable GetReferenceTableValues(int selectedRefTable)
        {
           using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetReferenceTableValuesForTable", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ReferenceTableNameId", selectedRefTable);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }
        
        internal static DataTable GetReferenceTableNamesWithvalues(int selectedRefTable)
        {
           using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetReferenceTableAttributes", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RefTableNameId", selectedRefTable);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }
    }
}