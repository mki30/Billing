<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DBUpdate.aspx.cs" Inherits="Admin_DBUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnReadCsv" runat="server" Text="Import Items" OnClick="btnReadCsv_Click" />
        <br/>
        <br />
        <asp:Button ID="btnResetGlobal" runat="server" OnClick="btnResetGlobal_Click" Text="Reset Global" />
        <br />
        <br />
        <br />
        <asp:Button ID="btnUpdateTaxRates" runat="server" OnClick="btnUpdateTaxRates_Click" Text="Update TaxRate field from Tax in Item" BackColor="#00CC99" />
        <br />
        <br />
        <%--<asp:Button ID="btnUpdateBrandCompany" runat="server" OnClick="btnUpdateBrandCompany_Click" Text="Update Company IDs" />--%>
        <asp:Button ID="btnUpdateBillingItem" runat="server" OnClick="btnUpdateBillingItem_Click1" Text="Update TaxPer field in billingItem" BackColor="#00CC99" />
        <br />
        <br />
        <%--<asp:Button ID="btnUpdateInventoryStores" runat="server" OnClick="btnUpdateInventoryStores_Click" Text="Update Inventory Stores" />--%><%--<br />
        <br />--%>
        <asp:Button ID="btnCombineCustomers" runat="server" OnClick="btnCombineCustomers_Click" Text="Comnbine Duplicate Customers" />
        <br />
        <br />
        <asp:Button ID="btnUpdateItemStock" runat="server" OnClick="btnUpdateItemStock_Click" Text="Update Item Stock" />
        <%--<br />
        <br />
        <br />
        <br />--%><%--<asp:Button ID="btnImportCategoryCSV" runat="server" OnClick="btnImportCategoryCSV_Click" Text="Update Category from CSV" />--%>
        <br />
        <br />

        <asp:Button ID="btmUpdateUrlName" runat="server" OnClick="btmUpdateUrlName_Click" Text="Update urlname in Menu " BackColor="#00CC99" />
        <br />
        <br />

        <%--<asp:Button ID="WriteCSVFile" runat="server" OnClick="WriteCSVFile_Click" Text="WriteCSV" />--%><%--<br />
        <br />--%>
        <asp:Button ID="btnUpdatePaymentAndDue" runat="server" OnClick="btnUpdatePaymentAndDue_Click" Text="Update Payment And Due in Vendor" BackColor="#00CC99" />
        <br />
        <br />

        <%--<asp:Button ID="btnDeleteSalaryTable" runat="server" OnClick="btnDeleteSalaryTable_Click" Text="Delete Salary Table Data" />--%><%--<br />
        <br />
        <br />--%>        <%--<asp:Button ID="btnDeletemonthlyTotal" runat="server" OnClick="btnDeletemonthlyTotal_Click" Text="Delete Monthly Total Data" />--%><%--<br />
        <br />
        <br />--%>

        <asp:Button ID="btnUpdaeBillingItemDate" runat="server" OnClick="btnUpdateBillingItemDate" Text="Update BillDate  and BillNo and StoreID in BillingItem " BackColor="#00CC99" />
        <br />
        <br />

        <asp:Button ID="btnUpdaeBillingItemCost" runat="server" Text="Update Billng Item Cost" BackColor="#00CC99" OnClick="btnUpdaeBillingItemCost_Click" />
        <br />
        <br />

        <asp:Button ID="btnBillAmountDiffer" runat="server" Text="Show Bill Amount Differ Report" BackColor="#00CC99" OnClick="btnBillAmountDiffer_Click" />
        <br />
        <br />

        <asp:Button ID="btnDeleteRawBillingItems" runat="server" Text="Delete raw billing items" BackColor="#00CC99" OnClick="btnDeleteRawBillingItems_Click" />
        <br />
        <br />
        <asp:Button ID="btnDeleteSalaryRecords" runat="server" Text="Delete Salary Records" BackColor="#00CC99" OnClick="btnDeleteSalaryRecords_Click" />
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="btnUpdateLastsaleDate" runat="server" Text="Update Last sale Date in Customer" BackColor="#00CC99" OnClick="btnUpdateLastsaleDate_Click" />
        <br />
        <br />
        <br />
        <asp:Button ID="btnUpateProjectsFromTextFile" runat="server" Text="Upate Projects From Text File"  OnClick="btnUpateProjectsFromTextFile_Click"/>
        <br />
        <br />
        <asp:Button ID="btnFillDummyData" runat="server" Text="Fill Dummy Apartment Data"  OnClick="btnFillDummyData_Click" />
        <br />
        <br />
        <asp:Button ID="btnLinkCustomerAndApartment" runat="server" Text="Link Customer And Apartment"  OnClick="btnLinkCustomerAndApartment_Click" />
        <br />
        <br />
        <asp:Button ID="btnUpdateEmployeeColumn" runat="server" Text="Update Column in employee"  OnClick="btnUpdateEmployeeColumn_Click"/>
        <br />
        <br />
        <asp:Button ID="btnUpdateStoreInCustomer" runat="server" Text="Update Store In Customer"  OnClick="btnUpdateStoreInCustomer_Click"/>
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="btnDeleteRawItems" runat="server" Text="Delete raw items"  OnClick="btnDeleteRawItems_Click"/>
        <br />
        <asp:Button ID="btnShowDuplicate" runat="server"  Text="Show Duplicate Bills" OnClick="btnShowDuplicate_Click" />
        <br />
        <asp:Button ID="btnRemoveDuplicate" runat="server" OnClick="btnRemoveDuplicate_Click" Text="Remove Duplicate Bills" />
        <br />
        <br />
        <asp:Button ID="UpdateRemainingVendorIDsInentory" runat="server" OnClick="UpdateRemainingVendorIDsInentory_Click" Text="Update Remaining VendorIds In Inventory" />
        <br />
        <br />
        <br />
        <br />
        <asp:Literal ID="ltData" runat="server"></asp:Literal>


        <br />
        <br />
        <br />
        <asp:Button ID="btnUpdateAllVendorDue" runat="server" Text="Update All Vendor Due" OnClick="btnUpdateAllVendorDue_Click" />

    </form>
</body>
</html>
