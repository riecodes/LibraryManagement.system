<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagePatrons.aspx.cs" Inherits="ManagePatrons" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Patrons</title>
</head>
<body>
    <form id="form3" runat="server">
        <div>
            <h1>Manage Patrons</h1>
            <div>
                <label for="AddName">Name:</label>
                <input type="text" id="AddName" runat="server" />
            </div>
            <div>
                <label for="AddCourse">Course:</label>
                <input type="text" id="AddCourse" runat="server" />
            </div>
            <div>
                <label for="AddSection">Section:</label>
                <input type="text" id="AddSection" runat="server" />
            </div>
            <div>
                <asp:Button ID="AddButton" runat="server" Text="Add Patron" OnClick="AddButton_Click" />
            </div>
            
            <div id="editSection" runat="server" visible="false">
                <hr />
                <h3>Edit Patron</h3>
                <div>
                    <label for="EditBorrowerId">Borrower ID:</label>
                    <input type="text" id="EditBorrowerId" runat="server" />
                </div>
                <div>
                    <label for="EditName">Name:</label>
                    <input type="text" id="EditName" runat="server" />
                </div>
                <div>
                    <label for="EditCourse">Course:</label>
                    <input type="text" id="EditCourse" runat="server" />
                </div>
                <div>
                    <label for="EditSection">Section:</label>
                    <input type="text" id="Text1" runat="server" />
                </div>
                <div>
                    <label for="EditNumberOfBooksAllowed">Number of Books Allowed:</label>
                    <input type="text" id="EditNumberOfBooksAllowed" runat="server" />
                </div>
                <div>
                    <asp:Button ID="EditButton" runat="server" Text="Edit Patron" OnClick="EditButton_Click" />
                </div>
            </div>

            <hr />

            <div>
                <label for="DeleteBorrowerId">Borrower ID:</label>
                <input type="text" id="DeleteBorrowerId" runat="server" />
                <asp:Button ID="DeleteButton" runat="server" Text="Delete Patron" OnClick="DeleteButton_Click" />
            </div>

            <div id="deleteSection" runat="server" visible="false">
                <hr />
                <h3>Delete Patron</h3>
                <div>
                    <label for="DeleteName">Name:</label>
                    <asp:Label ID="DeleteName" runat="server" />
                </div>
                <div>
                    <label for="DeleteCourse">Course:</label>
                    <asp:Label ID="DeleteCourse" runat="server" />
                </div>
                <div>
                    <label for="DeleteSection">Section:</label>
                    <asp:Label ID="Label1" runat="server" />
                </div>
                <div>
                    <label for="DeleteNumberOfBooksAllowed">Number of Books Allowed:</label>
                    <asp:Label ID="DeleteNumberOfBooksAllowed" runat="server" />
                </div>
                <div>
                    <asp:Button ID="ConfirmDeleteButton" runat="server" Text="Confirm Delete" OnClick="ConfirmDeleteButton_Click" />
                </div>
            </div>

            <hr />

            <div>
                <label for="SearchBorrowerId">Borrower ID:</label>
                <input type="text" id="SearchBorrowerId" runat="server" />
                <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" />
            </div>

            <div id="searchResults" runat="server" visible="false">
                <hr />
                <h3>Search Results</h3>
                <div>
                    <label for="SearchName">Name:</label>
                    <asp:Label ID="SearchName" runat="server" />
                </div>
                <div>
                    <label for="SearchCourse">Course:</label>
                    <asp:Label ID="SearchCourse" runat="server" />
                </div>
                <div>
                    <label for="SearchSection">Section:</label>
                    <asp:Label ID="SearchSection" runat="server" />
                </div>
                <div>
                    <label for="SearchNumberOfBooksAllowed">Number of Books Allowed:</label>
                    <asp:Label ID="SearchNumberOfBooksAllowed" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
