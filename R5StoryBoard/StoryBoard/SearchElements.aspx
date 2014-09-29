<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchElements.aspx.cs" MasterPageFile="~/GlobalMaster.Master" Inherits="StoryBoard.SearchElements" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ValidatePage() {
            var pageelem = document.getElementById("ddlPage");
            if (pageelem.options[pageelem.selectedIndex].value == -1) {
                alert('Please select a Page');
                return false;
            }
            var isselected = false;
            $('.SelectBox input').each(function () {
                if (this.checked)
                    isselected = true;
            });
            if (isselected == false) {
                alert("Please select atleast one element");
                return false;
            }
            return true;
        }

        function CheckUncheckAll(headercheckbox) {
            var ischecked = headercheckbox.checked;
            $('.SelectBox input').each(function () {
                this.checked = ischecked;
            });
        };
    </script>
    <script type='text/javascript'>
        xAddEventListener(window, 'load',
            function () { new xTableHeaderFixed('gvPageElements', 'table-container', 0); }, false);
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Search Data Elements </h3>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table>

                    <tr>
                        <td>Search by Element Name</td>
                        <td>
                            <asp:TextBox ID="txtElementName" runat="server" Width="242px"></asp:TextBox>
                        </td>
                        <td>&nbsp;&nbsp; and/or&nbsp; &nbsp;</td>
                        <td>Search By Page</td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <table class="auto-style1">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlSearchModule" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearchModule_SelectedIndexChanged">
                                                    <asp:ListItem Text="Self Service Portal" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Worker Portal" Value="2"></asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td>
                                                <asp:DropDownList ID="ddlSearchPage" runat="server" AppendDataBoundItems="True">
                                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr runat="server" id="tr_Module" visible="false">
                        <td>Module</td>
                        <td>
                            <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged">
                                <asp:ListItem Text="Self Service Portal" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Worker Portal" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr runat="server" id="tr_Page" visible="false">
                        <td>Page</td>
                        <td>
                            <asp:DropDownList ID="ddlPage" ClientIDMode="Static" runat="server" AppendDataBoundItems="True" DataTextField="PageName" DataValueField="PageID">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlModule" EventName="SelectedIndexChanged" />
                <asp:PostBackTrigger ControlID="btnSearch" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    <br />
    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Green" Visible="False"></asp:Label>
    <div id='table-container'  style="height: 500px" >
        <asp:GridView ID="gvPageElements" runat="server" CssClass="gvPageElements" AutoGenerateColumns="False" DataKeyNames="ElementID" EmptyDataText="There are no data records to display." OnRowDataBound="gvPageElements_RowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None" OnPreRender="gvPageElements_PreRender" OnRowDeleting="gvPageElements_RowDeleting">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAll"  onclick="javascript:CheckUncheckAll(this)" ClientIDMode="Static" runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" CssClass="SelectBox" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="ElementName" SortExpression="ElementName">
                    <ItemTemplate>
                        <table class="auto-style2">
                            <tr>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="submit" ControlToValidate="txtElementName" Display="Dynamic" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtElementName" runat="server" Text='<%# Bind("ElementName") %>' ValidationGroup="submit" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="SSP Display Name">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSSPDispName" runat="server" Text='<%# Bind("SSPDisplayName") %>' Height="60px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="WP Display Name">
                    <ItemTemplate>
                        <asp:TextBox ID="txtWPDisplayName" runat="server" Text='<%# Bind("WPDisplayName") %>' Height="60px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Length" SortExpression="Length">
                    <ItemTemplate>
                        <asp:TextBox ID="txtLength" runat="server" Text='<%# Bind("Length") %>' Width="67px"></asp:TextBox>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="ControlType" SortExpression="ControlType">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlControlType" runat="server" DataTextField="ControlTypeDesc" DataValueField="ControlTypeID">
                        </asp:DropDownList>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Is Required" SortExpression="IsRequired">

                    <ItemTemplate>
                        <asp:DropDownList ID="ddllsRequired" runat="server" DataTextField="Text" DataValueField="ID">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reference Table" SortExpression="ReferenceTable">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlReferenceTable" DataTextField="ReferenceTableName" DataValueField="ReferenceTableCode" runat="server">
                        </asp:DropDownList>

                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Display Rule" SortExpression="DisplayRule">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDisplayRule" runat="server" Text='<%# Bind("DisplayRule") %>' Height="60px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Validations" SortExpression="Validations">
                    <ItemTemplate>
                        <asp:TextBox ID="txtValidations" runat="server" Text='<%# Bind("Validations") %>' Height="60px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Validation Trigger" SortExpression="ValidationTrigger">
                    <ItemTemplate>
                        <asp:TextBox ID="txtValidationTrigger" runat="server" Text='<%# Bind("ValidationTrigger") %>' Height="60px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Error Code" SortExpression="ErrorCode">
                    <ItemTemplate>
                        <asp:TextBox ID="txtErrorCode" runat="server" Text='<%# Bind("ErrorCode") %>' Height="60px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlStatus" DataTextField="StatusName" DataValueField="StatusID" runat="server">
                        </asp:DropDownList>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="KTAP" SortExpression="KTAP">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlIsKTAP" runat="server" DataTextField="Text" DataValueField="ID">
                        </asp:DropDownList>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="SNAP" SortExpression="SNAP">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlIsSNAP" runat="server" DataTextField="Text" DataValueField="ID">
                        </asp:DropDownList>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="MEDICAID" SortExpression="MEDICAID">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlIsMedicaid" runat="server" DataTextField="Text" DataValueField="ID">
                        </asp:DropDownList>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="OtherPrograms" SortExpression="OtherPrograms">
                    <ItemTemplate>
                        <asp:TextBox ID="txtOtherPrograms" runat="server"  Text='<%# Bind("OtherPrograms") %>' TextMode="MultiLine" Height="60px" Width="300px"></asp:TextBox>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="DatabaseTableName" SortExpression="DatabaseTableName">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDatabaseTableName" runat="server" Text='<%# Bind("DatabaseTableName") %>' TextMode="MultiLine" Height="60px" Width="300px"></asp:TextBox>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="DatabaseTableFields" SortExpression="DatabaseTableFields">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDatabaseFields" runat="server" Text='<%# Bind("DatabaseTableFields") %>' TextMode="MultiLine" Height="60px" Width="300px"></asp:TextBox>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Open Questions">
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
        <asp:Button ID="btnAddExisting" runat="server" OnClick="btnAddExisting_Click" OnClientClick="return ValidatePage();" Text="Add To page" Visible="False" />
    </p>
</asp:Content>
