using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoryBoard
{
    public partial class ScreenImages : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strerrormsg = string.Format("Only {0} file types allowed", ConfigurationManager.AppSettings["AllowedImageFiles"]);
                lblfp1error.Text = strerrormsg;
                lblfp2error.Text = strerrormsg;
                lblfp3error.Text = strerrormsg;
                lblfp4error.Text = strerrormsg;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ucSearchPage.PageSelectionChanged += ucSearchPage_PageSelectionChanged;
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            int roleid = (Session["User"] as User).RoleId;
            if (roleid == 2)
            {
                btnUpload.Visible = false;
            }
        }

        private bool UploadFile(HttpPostedFile PostedImage)
        {
            int nPageId = ucSearchPage.SelectedPageId;
            string strImageName = Path.GetFileName(PostedImage.FileName);
            string strExtn = Path.GetExtension(PostedImage.FileName);
            string[] _allowedExtns = Convert.ToString(ConfigurationManager.AppSettings["AllowedImageFiles"]).Split(", ".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            if (_allowedExtns.Contains(strExtn))
            {
                byte[] ImageData = new byte[PostedImage.ContentLength];
                PostedImage.InputStream.Read(ImageData, 0, (int)PostedImage.ContentLength);
                DataMaster.UploadImageForPage(nPageId, strImageName, ImageData);
                return true;
            }
            else
                return false;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fu_1.HasFile)
            {
                lblfp1error.Visible = !UploadFile(fu_1.PostedFile);
            }
            if (fu_2.HasFile)
            {
                lblfp2error.Visible = !UploadFile(fu_2.PostedFile);
            }
            if (fu_3.HasFile)
            {
                lblfp3error.Visible = !UploadFile(fu_3.PostedFile);
            }
            if (fu_4.HasFile)
            {
                lblfp4error.Visible = !UploadFile(fu_4.PostedFile);
            }

            RefreshImageDetails(ucSearchPage.SelectedPageId);
        }

        protected void ucSearchPage_PageSelectionChanged(int PageID)
        {
            if (ucSearchPage.SelectedPageId != -1)
            {
                rptImageDetails.Visible = true;
                tblFileUpload.Visible = true;
                RefreshImageDetails(ucSearchPage.SelectedPageId);
            }
            else
            {
                rptImageDetails.Visible = false;
                tblFileUpload.Visible = false;
            }
        }

        void RefreshImageDetails(int PageId)
        {
            DataTable dtImageData = DataMaster.GetImageListForPage(PageId);
            lblNoImages.Visible = dtImageData.Rows.Count == 0;
            rptImageDetails.DataSource = dtImageData;
            rptImageDetails.DataBind();
        }

        protected void rptImageDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Image img = e.Item.FindControl("imgImage") as Image;
                string ImageId = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImageID"));
                img.ImageUrl = string.Format("GetScreenImage.ashx?Id={0}", ImageId);

                ImageButton deletebtn = e.Item.FindControl("btnDelete") as ImageButton;
                int roleid = (Session["User"] as User).RoleId;
                if (roleid == 2)
                {
                    deletebtn.Visible = false;
                }
            }
        }

        protected void rptImageDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int imagetobedeleted = Convert.ToInt32(e.CommandArgument);
                DataMaster.DeleteScreenImage(imagetobedeleted);
                RefreshImageDetails(ucSearchPage.SelectedPageId);
            }
        }
    }
}