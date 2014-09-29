<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDataElements.aspx.cs" Inherits="StoryBoard.AddDataElements" MasterPageFile="~/GlobalMaster.Master" %>

<%@ Register Src="SearchPage.ascx" TagName="SearchPage" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var _VMContext;
        var CurrentEditedRow;
        function CheckUncheckAll(headercheckbox) {
            var ischecked = headercheckbox.checked;
            $('.SelectBox input').each(function () {
                this.checked = ischecked;
            });
        };

        function OpenExElemWin() {
            window.location.href = "SearchElements.aspx";
        }

        function OpenSuggest(Elementobj) {
            var strElementName = Elementobj.value;
            CurrentEditedRow = $(Elementobj).closest('tr').parent().closest('tr');
            if (strElementName.length > 0) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "AjaxDataProcessor.aspx/SearchDataElements",
                    data: "{'ElementName':'" + strElementName + "'}",
                    dataType: "json",
                    success: function (data) {
                        var elemjson = $.parseJSON(data.d);
                        var observableData = ko.mapping.fromJS(elemjson);
                        var array = observableData();
                        _VMContext.ElementList(array);
                        OpenSearchDialog();

                    },
                    error: function (result) {
                        alert(result.message);
                    }
                });
            }
        }

        function OpenSearchDialog() {
            var wWidth = $(window).width();
            var dWidth = wWidth * 0.95;
            var wHeight = $(window).height();
            $('#elemsearchresults').dialog({
                title: "Search Results",
                modal: true,
                width: dWidth,
                maxHeight: wHeight
            });
        }

        function ElementSearchVM() {
            var self = this;
            self.ElementList = ko.observableArray();
            self.selectedElement = ko.observable();
            self.selectElement = function (data) {
                self.selectedElement(data);
                $('#elemsearchresults').dialog('close');
                $.each(data, function (classname, value) {
                    CurrentEditedRow.find('.' + classname).val(value());
                });
            };
        }

        $(document).ready(function () {
            _VMContext = new ElementSearchVM();
            ko.applyBindings(_VMContext);
        });
    </script>

    <script type='text/javascript'>
        xAddEventListener(window, 'load',
            function () { new xTableHeaderFixed('gvPageElements', 'table-container', 0); }, false);
    </script>

</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h3>Add Page Elements </h3>
        <uc1:SearchPage ID="ucSearch" runat="server" />
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" Visible="False" /></td>
                <td>
                    <input type="button" value="Add Existing Element" onclick="OpenExElemWin();" /></td>
            </tr>
        </table>

        <div id='table-container' style="height: 500px">
            <asp:GridView ID="gvPageElements" runat="server" CssClass="gvPageElements" AutoGenerateColumns="False" DataKeyNames="ElementID,PageElementMappingId" EmptyDataText="There are no data records to display." OnRowDataBound="gvPageElements_RowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None" OnPreRender="gvPageElements_PreRender" OnRowDeleting="gvPageElements_RowDeleting">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                        <ItemTemplate>
                            <asp:TextBox ID="IAElemID" runat="server" CssClass="ElementID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" onclick="javascript:CheckUncheckAll(this)" ClientIDMode="Static" runat="server" />
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
                                        <asp:TextBox ID="txtElementName" onblur="OpenSuggest(this);" CssClass="ElementName" runat="server" Text='<%# Bind("ElementName") %>' ValidationGroup="submit" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SSP Display Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSSPDispName" runat="server" CssClass="SSPDisplayName" Text='<%# Bind("SSPDisplayName") %>' Height="60px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="WP Display Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtWPDisplayName" runat="server" CssClass="WPDisplayName" Text='<%# Bind("WPDisplayName") %>' Height="60px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Length" SortExpression="Length">
                        <ItemTemplate>
                            <asp:TextBox ID="txtLength" runat="server" Text='<%# Bind("Length") %>' Width="67px" CssClass="Length"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ControlType" SortExpression="ControlType">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlControlType" runat="server" DataTextField="ControlTypeDesc" DataValueField="ControlTypeID" CssClass="ControlType">
                            </asp:DropDownList>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Is Required" SortExpression="IsRequired">

                        <ItemTemplate>
                            <asp:DropDownList ID="ddllsRequired" runat="server" DataTextField="Text" DataValueField="ID" CssClass="IsRequired">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reference Table" SortExpression="ReferenceTable">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlReferenceTable" DataTextField="ReferenceTableName" DataValueField="ReferenceTableCode" runat="server" CssClass="ReferenceTable">
                            </asp:DropDownList>

                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Display Rule" SortExpression="DisplayRule">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDisplayRule" runat="server" Text='<%# Bind("DisplayRule") %>' Height="60px" TextMode="MultiLine" Width="300px" CssClass="DisplayRule"></asp:TextBox>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Validations" SortExpression="Validations">
                        <ItemTemplate>
                            <asp:TextBox ID="txtValidations" runat="server" Text='<%# Bind("Validations") %>' Height="60px" TextMode="MultiLine" Width="300px" CssClass="Validations"></asp:TextBox>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Validation Trigger" SortExpression="ValidationTrigger">
                        <ItemTemplate>
                            <asp:TextBox ID="txtValidationTrigger" runat="server" Text='<%# Bind("ValidationTrigger") %>' Height="60px" TextMode="MultiLine" Width="300px" CssClass="ValidationTrigger"></asp:TextBox>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Error Code" SortExpression="ErrorCode">
                        <ItemTemplate>
                            <asp:TextBox ID="txtErrorCode" runat="server" Text='<%# Bind("ErrorCode") %>' Height="60px" TextMode="MultiLine" Width="300px" CssClass="ErrorCode"></asp:TextBox>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlStatus" DataTextField="StatusName" DataValueField="StatusID" runat="server" CssClass="Status">
                            </asp:DropDownList>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="KTAP" SortExpression="KTAP">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlIsKTAP" runat="server" DataTextField="Text" DataValueField="ID" CssClass="KTAP">
                            </asp:DropDownList>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SNAP" SortExpression="SNAP">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlIsSNAP" runat="server" DataTextField="Text" DataValueField="ID" CssClass="SNAP">
                            </asp:DropDownList>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MEDICAID" SortExpression="MEDICAID">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlIsMedicaid" runat="server" DataTextField="Text" DataValueField="ID" CssClass="MEDICAID">
                            </asp:DropDownList>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="OtherPrograms" SortExpression="OtherPrograms">
                        <ItemTemplate>
                            <asp:TextBox ID="txtOtherPrograms" runat="server" TextMode="MultiLine" Height="60px" Width="300px" CssClass="OtherPrograms"></asp:TextBox>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DatabaseTableName" SortExpression="DatabaseTableName">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDatabaseTableName" runat="server" Text='<%# Bind("DatabaseTableName") %>' TextMode="MultiLine" Height="60px" Width="300px" CssClass="DatabaseTableName"></asp:TextBox>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DatabaseTableFields" SortExpression="DatabaseTableFields">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDatabaseFields" runat="server" Text='<%# Bind("DatabaseTableFields") %>' TextMode="MultiLine" Height="60px" Width="300px" CssClass="DatabaseTableFields"></asp:TextBox>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Open Questions">
                        <ItemTemplate>
                            <asp:TextBox ID="txtOpenQuestions" runat="server" Height="60px" TextMode="MultiLine" Text='<%# Bind("OpenQuestions") %>' Width="300px" CssClass="OpenQuestions"></asp:TextBox>
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
        <table>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>

    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="submit" Visible="False" />
    <div id="elemsearchresults" style="display: none">
        <table class="dialogstyle" border="1"  >
            <thead>
                <tr>

                    <th>Element Name</th>
                    <th>SSP Display Name</th>
                    <th>WP Display Name</th>
                    <th>Length</th>
                    <th>ControlType</th>
                    <th>IsRequired</th>
                    <th>Reference Table</th>
                    <th>Display Rule</th>
                    <th>Validations</th>
                    <th>Validation Trigger</th>
                    <th>ErrorCode</th>
                    <th>Status</th>
                    <th>KTAP</th>
                    <th>SNAP</th>
                    <th>MEDICAID</th>
                    <th>OtherPrograms</th>
                    <th>DatabaseTableName</th>
                    <th>DatabaseTableFields</th>
                    <th>OpenQuestions</th>
                </tr>
            </thead>
            <tbody data-bind="foreach: ElementList">
                <tr data-bind="click: $root.selectElement, css: {'selected':$root.selectedElement() == $data}">
                    <td data-bind="text: ElementName"></td>
                    <td data-bind="text: SSPDisplayName"></td>
                    <td data-bind="text: WPDisplayName"></td>
                    <td data-bind="text: Length"></td>
                    <td data-bind="text: ControlType"></td>
                    <td data-bind="text: IsRequired"></td>
                    <td data-bind="text: ReferenceTable"></td>
                    <td data-bind="text: DisplayRule"></td>
                    <td data-bind="text: Validations"></td>
                    <td data-bind="text: ValidationTrigger"></td>
                    <td data-bind="text: ErrorCode"></td>
                    <td data-bind="text: Status"></td>
                    <td data-bind="text: KTAP"></td>
                    <td data-bind="text: SNAP"></td>
                    <td data-bind="text: MEDICAID"></td>
                    <td data-bind="text: OtherPrograms"></td>
                    <td data-bind="text: DatabaseTableName"></td>
                    <td data-bind="text: DatabaseTableFields"></td>
                    <td data-bind="text: OpenQuestions"></td>

                </tr>
            </tbody>
        </table>
        <br />
    </div>
</asp:Content>
