<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LibraryManagement.system.Models.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Library Management System</title>
    <link rel="stylesheet" href="../App_Data/css/managepatrons.css"/>
    <link rel="preconnect" href="https://fonts.googleapis.com"/>
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin=""/>
    <link href="https://fonts.googleapis.com/css2?family=Inconsolata&family=Montserrat&family=Roboto&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="css/default.css" />
</head>
<body>
    <div class="container">
        <!--NAVBAR START-->
        <div class="navbar">
            <ul>
                <li><a href="Default.aspx"><p>Home</p></a></li>
                <li><a href="AddBook.aspx"><p>Add Book</p></a></li>
                <li><a href="BorrowBook.aspx"><p>Borrow Book</p></a></li>
                <li><a href="ReturnBook.aspx"><p>Return Book</p></a></li>
                <li><a href="ManagePatrons.aspx"><p>Manage Patrons</p></a></li>
                <li><a href="ManageTransactions.aspx"><p>Manage Transactions</p></a></li>                
            </ul>
        </div>
        <hr class="vertical-line"/>
        <!--NAVBAR END-->
        <div class="content">
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
            
            <div id="EditSection" runat="server" visible="false">
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
                    <input type="text" id="EditSection" runat="server" />
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

            <div id="DeleteSection" runat="server" visible="false">
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
        </div>
    </div>           
</body>
</html>
