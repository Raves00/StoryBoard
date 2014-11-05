<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPageDetails.aspx.cs" Inherits="StoryBoard.AddPageDetails" MasterPageFile="~/GlobalMaster.Master" %>

<%@ Register Src="SearchPage.ascx" TagName="SearchPage" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h3>Add Page Details </h3>
 
        <uc1:SearchPage ID="ucSearchPage" runat="server" />
        <br />
         <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>

        <table class="auto-style1" runat="server" id="tblPageDetails" visible="false" >
            <tr>
                <td style="background-color: #507CD1; color: #FFFFFF;">Designation:</td>
                <td>
                    <asp:TextBox ID="txtPageDesignation" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="background-color: #507CD1; color: #FFFFFF;">Name:</td>
                <td>
                    <asp:TextBox ID="txtPageName" runat="server" Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPageName" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ValidationGroup="vgadd"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="background-color: #507CD1; color: #FFFFFF;">Description:</td>
                <td>
                    <asp:TextBox ID="txtPageDescription" runat="server" Height="144px" TextMode="MultiLine" Width="475px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="background-color: #507CD1; color: #FFFFFF;">Business Process:</td>
                <td>
                    <asp:TextBox ID="txtBusinessProcess" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="background-color: #507CD1; color: #FFFFFF;">Activity:</td>
                <td>
                    <asp:TextBox ID="txtActivity" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="background-color: #507CD1; color: #FFFFFF;">Programs:</td>
                <td>
                    <asp:TextBox ID="txtPrograms" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td >&nbsp;</td>
                <td>
                    <asp:Button ID="btnAddPageDetails" runat="server" Text="Add Page" OnClick="btnAddPageDetails_Click" ValidationGroup="vgadd" />
                </td>
            </tr>
        </table>

    </div>
</asp:Content>
