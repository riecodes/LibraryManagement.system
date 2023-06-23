<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="LibraryManagement.system.Models.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Library Management System</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br /><br />
            <asp:MultiView ID="multiView" ActiveViewIndex="0" runat="server">
                <asp:View ID="homeView" runat="server">
                    <h2>Home</h2>
                    <p>Welcome to Library Management System!</p>
                    <p>Kindly select the necessary tabs that suit your query :)</p>
                </asp:View>
                <asp:View ID="addBookView" runat="server">
                    <h2>Add Book</h2>
                    <table>
                        <tr>
                            <td>Book Category:</td>
                            <td><asp:TextBox ID="txtBookCategory" runat="server"></asp:TextBox></td>
                            <td><asp:RequiredFieldValidator ID="rfvBookCategory" runat="server" ErrorMessage="* Please enter the book category." ControlToValidate="txtBookCategory"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>Book Category Detail:</td>
                            <td><asp:TextBox ID="txtBookCategoryDetail" runat="server"></asp:TextBox></td>
                            <td><asp:RequiredFieldValidator ID="rfvBookCategoryDetail" runat="server" ErrorMessage="* Please enter the book category detail." ControlToValidate="txtBookCategoryDetail"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>Book Title:</td>
                            <td><asp:TextBox ID="txtBookTitle" runat="server"></asp:TextBox></td>
                            <td><asp:RequiredFieldValidator ID="rfvBookTitle" runat="server" ErrorMessage="* Please enter the book title." ControlToValidate="txtBookTitle"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>Copy Number:</td>
                            <td><asp:TextBox ID="txtCopyNumber" runat="server"></asp:TextBox></td>
                            <td><asp:RequiredFieldValidator ID="rfvCopyNumber" runat="server" ErrorMessage="* Please enter the copy number." ControlToValidate="txtCopyNumber"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>Number of Days Allowed:</td>
                            <td><asp:TextBox ID="txtDaysAllowed" runat="server"></asp:TextBox></td>
                            <td><asp:RequiredFieldValidator ID="rfvDaysAllowed" runat="server" ErrorMessage="* Please enter the number of days allowed." ControlToValidate="txtDaysAllowed"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnAddBook" runat="server" Text="Add Book" OnClick="btnAddBook_Click" />
                                <asp:Button ID="btnCancelAddBook" runat="server" Text="Cancel" OnClick="btnCancelAddBook_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="searchBookView" runat="server">
                    <h2>Search Book</h2>
                    <table>
                        <tr>
                            <td>Name:</td>
                            <td><asp:TextBox ID="txtSearchBookName" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSearchBook" runat="server" Text="Search" OnClick="btnSearchBook_Click" />
                                <asp:Button ID="btnCancelSearchBook" runat="server" Text="Cancel" OnClick="btnCancelSearchBook_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="gridViewBooks" runat="server" AutoGenerateColumns="False" DataKeyNames="BookID" OnRowEditing="gridViewBooks_RowEditing" OnRowUpdating="gridViewBooks_RowUpdating" OnRowCancelingEdit="gridViewBooks_RowCancelingEdit">
                        <Columns>
                            <asp:BoundField DataField="BookCategory" HeaderText="Book Category" SortExpression="BookCategory" />
                            <asp:BoundField DataField="BookCategoryDetail" HeaderText="Book Category Detail" SortExpression="BookCategoryDetail" />
                            <asp:BoundField DataField="BookID" HeaderText="Book ID" ReadOnly="True" SortExpression="BookID" />
                            <asp:BoundField DataField="BookTitle" HeaderText="Book Title" SortExpression="BookTitle" />
                            <asp:BoundField DataField="CopyNumber" HeaderText="Copy Number" SortExpression="CopyNumber" />
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                            <asp:BoundField DataField="NumberOfDaysAllowed" HeaderText="Number of Days Allowed" SortExpression="NumberOfDaysAllowed" />
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>
                </asp:View>
                <asp:View ID="editBookView" runat="server">
                    <h2>Edit Book</h2>
                    <table>
                        <tr>
                            <td>Book Category:</td>
                            <td><asp:TextBox ID="txtEditBookCategory" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Book Category Detail:</td>
                            <td><asp:TextBox ID="txtEditBookCategoryDetail" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Book Title:</td>
                            <td><asp:TextBox ID="txtEditBookTitle" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Copy Number:</td>
                            <td><asp:TextBox ID="txtEditCopyNumber" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Number of Days Allowed:</td>
                            <td><asp:TextBox ID="txtEditDaysAllowed" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnUpdateBook" runat="server" Text="Update" OnClick="btnUpdateBook_Click" />
                                <asp:Button ID="btnCancelEditBook" runat="server" Text="Cancel" OnClick="btnCancelEditBook_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </div>
    </form>
</body>
</html>
