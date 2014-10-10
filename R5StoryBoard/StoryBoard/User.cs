using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoryBoard
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public byte RoleId { get; set; }
        public User()
        {

        }
        public User(int userid,string username,string fullname,byte role)
        {
            this.UserID = userid;
            this.UserName = username;
            this.FullName = fullname;
            this.RoleId = role;
        }
    }
}