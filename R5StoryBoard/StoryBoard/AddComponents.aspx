<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddComponents.aspx.cs" Inherits="StoryBoard.AddComponents" MasterPageFile="~/GlobalMaster.Master" %>

<%@ Register Src="SearchPage.ascx" TagName="SearchPage" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h3>Add Components</h3>
        <br />
        <uc1:SearchPage ID="ucSearch" runat="server" />
        <br />
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        <br />
        <br />

        <asp:GridView ID="gvPageElements" runat="server" AutoGenerateColumns="False" DataKeyNames="ElementID" EmptyDataText="There are no data records to display." OnRowDataBound="gvPageElements_RowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="Type" SortExpression="ComponentType">
                    <ItemTemplate>
                        <asp:DropDownList ID="lstComponentType" runat="server" ></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name" SortExpression="Length">
                    <ItemTemplate>
                        <asp:TextBox ID="txtComponentName" runat="server" Text='<%# Bind("ComponentName") %>' Width="150px"></asp:TextBox>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:TextBox ID="txtOpenQuestions" runat="server" Height="60px" TextMode="MultiLine" Text='<%# Bind("OpenQuestions") %>' Width="300px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
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


    </div>
    <p>
        &nbsp;
    </p>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="submit" Visible="False" />

</asp:Content>
