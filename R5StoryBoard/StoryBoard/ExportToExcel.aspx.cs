using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;

namespace StoryBoard
{
    public partial class ExportToExcel : System.Web.UI.Page
    {
        #region Constants

        public const string author = "Story Board";
        public const string subject = "";
        public const string title = "";

        #endregion Constants

        public void ConvertDataElementsToExcel(DataSet ds, HttpResponse Response)
        {
            #region Commented

            //string sTableStart = @"<HTML><BODY><TABLE Border=1>";
            //string sTableEnd = @"</TABLE></BODY></HTML>";
            //string sTHead = "<TR>";
            //StringBuilder sTableData = new StringBuilder();
            //foreach (DataColumn col in ds.Tables[0].Columns)
            //{
            //    sTHead += @"<TH style='width:auto; color: #FFF;text-align: left;vertical-align: top;border: 0.5pt solid #000;background: none repeat scroll 0% 0% #002060;white-space: normal;border-left:none;'>" + col.ColumnName + @"</TH>";
            //}
            //sTHead += @"</TR>";
            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    sTableData.Append(@"<TR style='vertical-align: top;background: none repeat scroll 0% 0% #FFF;white-space: normal;' >");
            //    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            //    {
            //        sTableData.Append(@"<TD style='text-align: left;border: 0.5pt solid #000;white-space: normal;border-top:none;'>" + row[i].ToString().Replace("–", "&#8211;") + @"</TD>");
            //    }
            //    sTableData.Append(@"</TR>");
            //}
            //string sTable = sTableStart + sTHead + sTableData.ToString() + sTableEnd;

            ////Response.Clear();
            ////Response.Charset = "";
            ////Response.ContentType = "application/vnd.ms-excel";
            ////System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            ////stringWrite.Write(sTable);
            ////System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);

            ////Response.Write(stringWrite.ToString());
            ////Response.End();

            #endregion Commented

            string strfilename = ucSearch.SelectedPageName;
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Overview");
                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Details");

                ws.Cells["A1"].LoadFromDataTable(ds.Tables[0], true);
                ws2.Cells["A1"].LoadFromDataTable(ds.Tables[1], true); ;


                ws.DeleteRow(1, 1, true);
                int[] cols_to_modify = { 9, 10, 11, 12, 20 };
                if (ws.Dimension != null && ws2.Dimension != null)
                {
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws2.Cells[ws2.Dimension.Address].AutoFitColumns();
                    int rowcnt = ws2.Dimension.End.Row;
                    for (int i = 2; i < rowcnt; i++)
                    {
                        foreach (var col in cols_to_modify)
                        {
                            ws2.Cells[i, col].Value = SBHelper.DecodeData(ws2.Cells[i, col].GetValue<string>());
                        }

                    }
                }
                ExcelRange range = ws.Cells["A1:A6"];
                range.Merge = false;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                range.Style.Font.Color.SetColor(Color.White);

                ExcelRange range2 = ws2.Cells["A1:T1"];
                range2.Merge = false;
                range2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range2.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                range2.Style.Font.Color.SetColor(Color.White);

                //pck.Workbook.Properties.Title = title;
                //pck.Workbook.Properties.Author = author;
                //pck.Workbook.Properties.Subject = subject;

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}.xlsx", strfilename));
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.Flush();
                Response.End();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ucSearch.AddButtonClicked += ucSearch_AddButtonClicked;
            ucSearch.ShowAddButton = true;
            ucSearch.ButtonText = "Export";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ucSearch_AddButtonClicked(object sender, EventArgs e)
        {
            if (ucSearch.SelectedPageId != -1)
            {
                lblErrorMessage.Visible = false;
                PopulateExistingDataElements(ucSearch.SelectedPageId);
            }
            else
            {
                lblErrorMessage.Text = "Please select a page.";
                lblErrorMessage.Visible = true;
            }
        }

        private void PopulateExistingDataElements(int PageID)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoryBoardConnStr"].ConnectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand("stp_GetPageElementsByPageIdExcel", sqlconn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageId", PageID);
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet("ElementSet");
                        sda.Fill(ds);

                        ConvertDataElementsToExcel(ds, Response);
                    }
                }
            }
        }
    }
}