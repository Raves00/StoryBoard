using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoryBoard
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string strUserName=login_username.Text.Trim();
                string strPassword=new EncryptDecrypt.Crypto().Encrypt( login_password.Text.Trim());
                User validatedUser = DataMaster.ValidateUser(strUserName, strPassword);
                if (validatedUser != null)
                {
                    Session["User"] = validatedUser;
                    Response.Redirect("AddPageDetails.aspx");
                }
                else
                {
                    LoginError.Visible = true;
                }
            }
        }
    }
}