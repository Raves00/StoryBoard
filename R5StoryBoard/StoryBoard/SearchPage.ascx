<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchPage.ascx.cs" Inherits="StoryBoard.SearchPage" %>
<style type="text/css">
    .auto-style1 {
        margin: 10px;
        width: 20%;
    }
</style>
<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
        <table >
            <tr>
                <td>Module:</td>
                <td>
                    <asp:DropDownList ID="ddlModule" runat="server" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Text="Self Service Portal" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Worker Portal" Value="2"></asp:ListItem>
                    </asp:DropDownList>

                </td>
                <td runat="server" id="td_top_label"></td>
                <td runat="server" id="td_top_ctrl"></td>
            </tr>
           <tr>
                <td runat="server" id="td_bottom_label">
                    <asp:label ID="pg" runat="server">Page</asp:label></td>
                <td runat="server" id="td_bottom_ctrl">
                    <asp:DropDownList ID="ddlPageList" runat="server" AutoPostBack="true" DataTextField="PageName" DataValueField="PageID" OnSelectedIndexChanged="ddlPageList_SelectedIndexChanged">
                        
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" Width="96px" />
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlModule" EventName="SelectedIndexChanged" />
        <asp:PostBackTrigger ControlID="ddlPageList" />
           <asp:PostBackTrigger ControlID="btnAdd" />
    </Triggers>
</asp:UpdatePanel>

