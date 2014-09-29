<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddComponent.aspx.cs" Inherits="StoryBoard.AddComponent" MasterPageFile="~/GlobalMaster.Master" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3>Add Component </h3> 
        <br />
    <table class="auto-style1">
        <tr>
            <td>Page</td>
            <td>
                <asp:DropDownList ID="ddlPage" runat="server" DataValueField="PageID" DataTextField="PageName">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Type</td>
            <td>
                <asp:DropDownList ID="ddlComponentType" runat="server" DataTextField="ComponentTypeText" DataValueField="ComponentTypeId">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Name</td>
            <td>
                <asp:DropDownList ID="ddlComponentName" runat="server" DataTextField="ComponentNameText" DataValueField="ComponentNameID">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Description</td>
            <td>
                <asp:TextBox ID="txtComponentDesc" runat="server" Height="91px" TextMode="MultiLine" Width="593px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
