<%@ Page Title="" Language="C#" MasterPageFile="~/GlobalMaster.Master" AutoEventWireup="true" CodeBehind="ImportExport.aspx.cs" Inherits="StoryBoard.ImportExport" %>

<%@ Register Src="SearchPage.ascx" TagName="SearchPage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ValidateReviewSelection() {
            var isvalid = false;
            $('.rvchkselect input').each(function () {
                if (this.checked)
                    isvalid=true;
            });

            if (!isvalid) {
                alert('Please select an element from review elements')
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h3>Import Data</h3>
        <uc1:SearchPage ID="ucSearch" runat="server" ShowAddButton="false" ShowPgList="False" />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="btImport" runat="server" Text="Import" OnClick="btImport_Click" />
        <br />
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        <div runat="server" id="divReviewItems" visible="false">
            <div>
                Elements that we found are matching
                <asp:GridView ID="grdExistingMatched" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                 <AlternatingRowStyle BackColor="White" />
                 <Columns>
                     <%--<asp:TemplateField HeaderText="Select">
                         <ItemTemplate>
                             <asp:CheckBox ID="chkSelect" runat="server" />
                         </ItemTemplate>
                     </asp:TemplateField>--%>
                     <asp:BoundField HeaderText="ElementName" DataField="ElementName" />
                    <%-- <asp:BoundField HeaderText="SSP Display Name" DataField="SSPDisplayName" />
                     <asp:BoundField HeaderText="WP Display Name" DataField="WPDisplayName" />--%>
                     <asp:BoundField HeaderText="Length" DataField="Length" />
                     <asp:BoundField HeaderText="Control Type" DataField="ControlTypeText" />
                     <asp:BoundField HeaderText="Is Required" DataField="IsRequiredText" />
                     <asp:BoundField HeaderText="Reference Table" DataField="ReferenceTableName" />
                     <asp:BoundField HeaderText="Display Rule" DataField="DisplayRule" />
                     <asp:BoundField HeaderText="Validations" DataField="Validations" />
                     <asp:BoundField HeaderText="ValidationTrigger" DataField="ValidationTrigger" />
                     <asp:BoundField HeaderText="ErrorCode" DataField="ErrorCode" />
                     <asp:BoundField HeaderText="Status" DataField="StatusText" />
                     <asp:BoundField HeaderText="KTAP" DataField="KTAPText" />
                     <asp:BoundField HeaderText="SNAP" DataField="SNAPText" />
                     <asp:BoundField HeaderText="MEDICAID" DataField="MEDICAIDText" />
                     <asp:BoundField HeaderText="OtherPrograms" DataField="OtherPrograms" />
                     <asp:BoundField HeaderText="DatabaseTableName" DataField="DatabaseTableName" />
                     <asp:BoundField HeaderText="DatabaseTableFields" DataField="DatabaseTableFields" />
                     <asp:BoundField HeaderText="OpenQuestions" DataField="OpenQuestions" />
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
            <br />
            <div>
                Elements that you have uploaded.
                <asp:GridView ID="grdExcelElements" runat="server" DataKeyNames="ElementID" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="rvchkselect" />
                        </ItemTemplate>
                    </asp:TemplateField>
                   <%-- <asp:BoundField HeaderText="SSP Display Name" DataField="SSPDisplayName" />
                    <asp:BoundField HeaderText="WP Display Name" DataField="WPDisplayName" />--%>
                    <asp:TemplateField HeaderText="ElementName">
                        <ItemTemplate>
                           <asp:TextBox ID="txtElementName" runat="server" MaxLength="100" Text='<%# Bind("ElementName") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Length" DataField="Length" />
                    <asp:BoundField HeaderText="Control Type" DataField="ControlTypeText" />
                    <asp:BoundField HeaderText="Is Required" DataField="IsRequiredText" />
                    <asp:BoundField HeaderText="Reference Table" DataField="ReferenceTableName" />
                    <asp:BoundField HeaderText="Display Rule" DataField="DisplayRule" />
                    <asp:BoundField HeaderText="Validations" DataField="Validations" />
                    <asp:BoundField HeaderText="ValidationTrigger" DataField="ValidationTrigger" />
                    <asp:BoundField HeaderText="ErrorCode" DataField="ErrorCode" />
                    <asp:BoundField HeaderText="Status" DataField="StatusText" />
                    <asp:BoundField HeaderText="KTAP" DataField="KTAPText" />
                    <asp:BoundField HeaderText="SNAP" DataField="SNAPText" />
                    <asp:BoundField HeaderText="MEDICAID" DataField="MEDICAIDText" />
                    <asp:BoundField HeaderText="OtherPrograms" DataField="OtherPrograms" />
                    <asp:BoundField HeaderText="DatabaseTableName" DataField="DatabaseTableName" />
                    <asp:BoundField HeaderText="DatabaseTableFields" DataField="DatabaseTableFields" />
                    <asp:BoundField HeaderText="OpenQuestions" DataField="OpenQuestions" />
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
            <asp:Label ID="lblReviewStatus" runat="server"></asp:Label>
            <asp:Button ID="btnChange" runat="server" Text="" Visible="false" OnClientClick="return ValidateReviewSelection();" OnClick="btnChange_Click" />
        </div>
    </div>
</asp:Content>
