<%@ Page Title="" Language="C#" MasterPageFile="~/GlobalMaster.Master" AutoEventWireup="true" CodeBehind="ImportExport.aspx.cs" Inherits="StoryBoard.ImportExport" %>

<%@ Register Src="SearchPage.ascx" TagName="SearchPage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h3>Import Data</h3>
        <uc1:SearchPage ID="ucSearch" runat="server" ShowAddButton ="false" ShowPgList="False"/>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID ="btImport" runat ="server" Text="Import" OnClick="btImport_Click"/>        <br />
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" />
    </div>
</asp:Content>
