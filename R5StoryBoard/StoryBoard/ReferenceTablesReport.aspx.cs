using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace StoryBoard
{
    public partial class ReferenceTablesReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindReferenceTables();
            }
        }

        private void BindReferenceTables()
        {
            DataTable dtRefTables = DataMaster.GetRefTableList();
            ddlReferenceTable.DataSource = dtRefTables;
            ddlReferenceTable.DataTextField = "ReferenceTableName";
            ddlReferenceTable.DataValueField = "ReferenceTableNameId";
            ddlReferenceTable.DataBind();


        }

        protected void ddlReferenceTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRefTable = Convert.ToInt32(ddlReferenceTable.SelectedValue);
            grdReferenceTableValues.DataSource = DataMaster.GetReferenceTableValues(selectedRefTable);
            grdReferenceTableValues.DataBind();
            lstReferenceTableAddAttributes.DataSource = DataMaster.GetReferenceTableNamesWithvalues(selectedRefTable);
            lstReferenceTableAddAttributes.DataBind();
        }

        

        protected void lstReferenceTableAddAttributes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                string refvalxml = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "RefValueXml"));
                GridView grdRefValueData = e.Item.FindControl("lstRefTableValues") as GridView;
                XDocument xdoc = XDocument.Parse(refvalxml);
                var xy=xdoc.Elements("ReferenceTableAdditionalAttributes").Elements().AsEnumerable();
                grdRefValueData.DataSource = xy.Select(x => new { ReferenceCodeDescription = x.HasAttributes?x.Attribute("ReferenceCodeDescription").Value:"", ReferenceTableAdditionalAttributeValue = x.Element("ReferenceTableAdditionalAttributeValue").HasAttributes?x.Element("ReferenceTableAdditionalAttributeValue").Attribute("ReferenceDataAdditionalAttributeValue").Value:"" });
                grdRefValueData.DataBind();
            }
        }
    }
}