<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddComponent.aspx.cs" Inherits="StoryBoard.AddComponent" MasterPageFile="~/GlobalMaster.Master" %>
<%@ Register Src="SearchPage.ascx" TagName="SearchPage" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ValidatePage() {
            var pageid = '<%= ucModulePage.PageControlClientID %>'
            var pagectrl = document.getElementById(pageid);
            if (parseInt(pagectrl.options[pagectrl.selectedIndex].value) == -1) {
                alert("Please select a Page");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h3>Add Component </h3> 
        <uc1:SearchPage ID="ucModulePage" runat="server" ValidatePageSelectionOnAdd="true" ButtonText="Add Component" />
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="False" ></asp:Label>
        <br />

    <asp:GridView ID="grdComponents" AutoGenerateColumns="False" runat="server" CellPadding="4" ForeColor="#333333" DataKeyNames="ComponentId" GridLines="None" OnRowDataBound="grdComponents_RowDataBound" OnRowCommand="grdComponents_RowCommand">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="Type">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlComponentType" DataValueField="ComponentTypeId" DataTextField="ComponentTypeText" runat="server">
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:TextBox ID="txtComponentName" runat="server" MaxLength="100" Text='<%# DataBinder.Eval(Container.DataItem,"ComponentName") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <asp:TextBox ID="txtComponentDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ComponentDescription") %>' Height="60px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:ImageButton ID="imgbtn_Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this component ?')"  CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ComponentId") %>' CommandName="Remove" ImageUrl="~/Images/DeleteIcon.png" ToolTip="Delete" />
                </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <EmptyDataTemplate>
            No Components Found.
        </EmptyDataTemplate>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>

     <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return ValidatePage();" OnClick="btnSubmit_Click" ValidationGroup="submit" Visible="False" />
</asp:Content>
