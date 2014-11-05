<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportToExcel.aspx.cs" Inherits="StoryBoard.ExportToExcel" MasterPageFile="~/GlobalMaster.Master" %>

<%@ Register Src="SearchPage.ascx" TagName="SearchPage" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h3>Export to Excel</h3>
        <uc1:SearchPage ID="ucSearch" runat="server" />
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>
    </div>
</asp:Content>
