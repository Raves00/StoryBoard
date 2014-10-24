<%@ Page Title="" Language="C#" MasterPageFile="~/GlobalMaster.Master" AutoEventWireup="true" CodeBehind="AuditReport.aspx.cs" Inherits="StoryBoard.AuditReport" %>

<%@ Register Src="SearchPage.ascx" TagName="SearchPage" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Css/Collapsible.css" rel="stylesheet" />
    <%-- <script src="/Scripts/jquery.cookie.js"></script>--%>
    <script src="../Scripts/jquery.collapsible.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id$=txtFromDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,

                buttonImage: '/Images/Calendar.png'
            });
            $("[id$=txtToDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '/Images/Calendar.png'
            });

            $('.collapsible').collapsible({
                defaultOpen: ''

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h3>Audit Report<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        </h3>

        <table class="table" style="width: 800px;">
            <tr>
                <td>
                    <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" ReadOnly="true"></asp:TextBox>
                </td>

                <td>
                    <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Module</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlSearchModule" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearchModule_SelectedIndexChanged">
                                <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Self Service Portal" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Worker Portal" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>Page</td>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlSearchPage" runat="server" DataTextField="PageName" DataValueField="PageID" AppendDataBoundItems="True">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlSearchModule" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>

                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="search_button" />
                </td>
            </tr>
        </table>


    </div>
    <asp:Panel ID="pnlSearchResult" runat="server" Style="margin-top: 30px;" Visible="false">
        <div class="collapsible" id="nav-section1">
            &nbsp;&nbsp;&nbsp;Self Service Portal PAGE Audit History<span></span>
            <div style="float: right; padding: 0;">
                <asp:Literal ID="lblSSPCount" runat="server" Text=""></asp:Literal>
            </div>
        </div>
        <div>
            <ul style="list-style: none; margin-left: -40px;">
                <li>
                    <asp:Repeater ID="rpSSP" runat="server">
                        <ItemTemplate>
                            <asp:Literal ID="litSSP" runat="server" Text='<%# Eval("html") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:Repeater>
                </li>
            </ul>
        </div>
        <div class="collapsible" id="nav-section2">
            &nbsp;&nbsp;&nbsp;Worker Portal PAGE Audit History<span></span>
            <div style="float: right; padding: 0;">
                <asp:Literal ID="lblWPCount" runat="server" Text=""></asp:Literal>
            </div>
        </div>
        <div>
            <ul style="list-style: none; margin-left: -40px;">
                <li>
                    <asp:Repeater ID="rpWP" runat="server">
                        <ItemTemplate>
                            <asp:Literal ID="litWP" runat="server" Text='<%# Eval("html") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:Repeater>
                </li>

            </ul>
        </div>
        <div class="collapsible" id="nav-section3">
            &nbsp;&nbsp;PAGE&nbsp;Data Element Audit History<span></span>
            <div style="float: right; padding: 0;">
                <asp:Literal ID="lblElementCount" runat="server" Text=""></asp:Literal>
            </div>
        </div>
        <div>
            <ul style="list-style: none; margin-left: -40px;">
                <li>
                    <asp:Repeater ID="rpElement" runat="server">
                        <ItemTemplate>
                            <asp:Literal ID="litElement" runat="server" Text='<%# Eval("html") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:Repeater>
                </li>
            </ul>
        </div>
    </asp:Panel>
</asp:Content>
