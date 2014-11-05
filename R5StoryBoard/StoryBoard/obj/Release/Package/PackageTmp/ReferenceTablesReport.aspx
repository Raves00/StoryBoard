<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReferenceTablesReport.aspx.cs" MasterPageFile="~/GlobalMaster.Master" Inherits="StoryBoard.ReferenceTablesReport" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ToggleChild(obj) {
            var nexttr = $(obj).closest('tr').next();
            if (nexttr[0].style.display == '') {
                obj.src = 'Images/toggle-expand-alt_blue.png';
                nexttr[0].style.display = 'none';

            }
            else {
                obj.src = 'Images/toggle_collapse_alt.png';
                nexttr[0].style.display = '';
            }
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="margin-left: 40px">
        <h3>Reference Tables </h3>
        <table class="auto-style1">
            <tr>
                <td>Reference Table:</td>
                <td>
                    <asp:DropDownList ID="ddlReferenceTable" AutoPostBack="true" AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlReferenceTable_SelectedIndexChanged">
                        <asp:ListItem Text="-- Select --" Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <br />

        <table>
            <tr>
                <td style="vertical-align: top;" width="350px">
                    <strong>Reference Table Values</strong><asp:GridView ID="grdReferenceTableValues" runat="server" Width="100%" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="ReferenceCode" HeaderText="ReferenceCode" ReadOnly="True" SortExpression="ReferenceCode" />
                            <asp:BoundField DataField="ReferenceCodeDescription" HeaderText="ReferenceCodeDescription" ReadOnly="True" SortExpression="ReferenceCodeDescription" />
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <EmptyDataTemplate>
                            No Values found for the selected Reference Table
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
                </td>
                <td style="vertical-align: top; text-align: left"><strong>Reference Table Additional Attributes</strong>
                    <br />
                    <asp:Repeater ID="lstReferenceTableAddAttributes" runat="server" OnItemDataBound="lstReferenceTableAddAttributes_ItemDataBound">
                        <HeaderTemplate>
                            <table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="left">
                                    <table style="width: 100%">
                                        <tr>
                                            <td width="10px" valign="top">
                                                <img src="Images/toggle-expand-alt_blue.png" onclick="ToggleChild(this);" style="height: 25px; width: 25px" /></td>
                                            <td colspan="5" valign="top"><%# DataBinder.Eval(Container.DataItem,"AdditionalAttributeName") %></td>

                                        </tr>
                                        <tr style="display: none">
                                            <td colspan="6">
                                                <asp:GridView ID="lstRefTableValues" AutoGenerateColumns="true" runat="server">
                                                     <AlternatingRowStyle BackColor="White" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#EFF3FB" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>



    </div>
</asp:Content>
