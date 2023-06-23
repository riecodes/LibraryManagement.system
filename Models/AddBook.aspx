<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBook.aspx.cs" Inherits="LibraryManagement.system.AddBook" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Add Book</title>
    <link rel="stylesheet" href="css/addbooks.css" />
    <link rel="preconnect" href="https://fonts.googleapis.com"/>
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin=""/>
    <link href="https://fonts.googleapis.com/css2?family=Inconsolata&family=Montserrat&family=Roboto&display=swap" rel="stylesheet"/>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
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
            <form id="form1" runat="server"> 
                <div class="box">
                    <h2>Add Book</h2>
                    <div class="row">
                        <div class="form-group">
                            <label for="txtBookCategory">Book Category:</label>
                            <input type="text" id="txtBookCategory" name="txtBookCategory" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator ID="rfvBookCategory" runat="server" ControlToValidate="txtBookCategory" Text="* Please enter the book category."></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="txtBookCategoryDetail">Book Category Detail:</label>
                            <input type="text" id="txtBookCategoryDetail" name="txtBookCategoryDetail" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator ID="rfvBookCategoryDetail" runat="server" ControlToValidate="txtBookCategoryDetail" Text="* Please enter the book category detail."></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="txtBookTitle">Book Title:</label>
                            <input type="text" id="txtBookTitle" name="txtBookTitle" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator ID="rfvBookTitle" runat="server" ControlToValidate="txtBookTitle" Text="* Please enter the book title."></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="txtCopyNum">Copy Number:</label>
                            <input type="number" id="txtCopyNum" name="txtCopyNum" runat="server" class="form-control" min="1" />
                            <asp:RequiredFieldValidator ID="rfvCopyNum" runat="server" ControlToValidate="txtCopyNum" Text="* Please enter the copy number."></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label for="txtNumberOfDaysAllowed">Number of Days Allowed:</label>
                            <input type="number" id="txtNumberOfDaysAllowed" name="txtNumberOfDaysAllowed" runat="server" class="form-control" min="1" />
                            <asp:RequiredFieldValidator ID="rfvNumberOfDaysAllowed" runat="server" ControlToValidate="txtNumberOfDaysAllowed" Text="* Please enter the number of days allowed."></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnAddBook" runat="server" Text="Add Book" OnClick="btnAddBook_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="error-message"></asp:Label>
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
