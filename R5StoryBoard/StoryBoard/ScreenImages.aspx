<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScreenImages.aspx.cs" MasterPageFile="~/GlobalMaster.Master" Inherits="StoryBoard.ScreenImages" %>

<%@ Register Src="SearchPage.ascx" TagName="SearchPage" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <h3>Page Screenshots </h3>
        <uc1:SearchPage ID="ucSearchPage" runat="server" ShowAddButton="false" />
    </div>
    <div>
        <table class="auto-style1" runat="server" id="tblFileUpload" visible="false">
            <tr>
                <td colspan="3">Select Files :</td>
               
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>1.</td>
                <td>
                    <asp:FileUpload ID="fu_1" runat="server" AllowMultiple="true" />
                </td>
                <td>2.</td>
                <td>
                    <asp:FileUpload ID="fu_2" runat="server" AllowMultiple="true" />
                </td>
                <td>3.</td>
                <td>
                    <asp:FileUpload ID="fu_3" runat="server" AllowMultiple="true" />
                </td>
                <td>4.</td>
                <td>
                    <asp:FileUpload ID="fu_4" runat="server" AllowMultiple="true" />
                </td>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btnUpload" Width="150px" runat="server" Text="Upload Files" OnClick="btnUpload_Click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblMsg" runat="server" ForeColor="Green" Text=""></asp:Label>
        <h2 style="font-weight: bold; color: #0066FF;"></h2>
    </div>
    <asp:Label ID="lblNoImages" runat="server" Text="No Screen found for this page" Visible="false"></asp:Label>
    <asp:Repeater ID="rptImageDetails" runat="server" OnItemDataBound="rptImageDetails_ItemDataBound" OnItemCommand="rptImageDetails_ItemCommand">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:ImageButton ID="btnDelete" ImageUrl="~/Images/DeleteIcon.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this file?');" CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ImageID") %>' runat="server" />
                </td>
                <td>

                   <b style="color: #0066FF;">
                        <%# DataBinder.Eval(Container.DataItem,"ImageName") %></b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Image ID="imgImage" runat="server" />
                </td>
            </tr>
          
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>

    </asp:Repeater>
</asp:Content>
