﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BorrowBook.aspx.cs" Inherits="LibraryManagement.system.BorrowBook" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Borrow Book</title>
    <link rel="stylesheet" href="css/borrowbook.css" />
    <link rel="preconnect" href="https://fonts.googleapis.com"/>
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin-=""/>
    <link href="https://fonts.googleapis.com/css2?family=Inconsolata&family=Montserrat&family=Roboto&display=swap" rel="stylesheet"/>
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
            <form id="form2" runat="server">
                <div class="content">
                    <h2>Borrow Book</h2>
                    <hr />
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="BookIdTextBox">Book ID:</label>
                                <asp:TextBox ID="BookIdTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="BookIdDropDownList">Book ID:</label>
                                <asp:DropDownList ID="BookIdDropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="BorrowerIdTextBox">Borrower ID:</label>
                                <asp:TextBox ID="BorrowerIdTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:Button ID="BorrowButton" runat="server" Text="Borrow" CssClass="btn btn-primary" OnClick="BorrowButton_Click" />
                            <asp:Label ID="ErrorMessageLabel" runat="server" Text="" CssClass="text-danger mt-3"></asp:Label>
                            <asp:Label ID="SuccessMessageLabel" runat="server" Text="" CssClass="text-success mt-3"></asp:Label>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>   
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js"></script>
</body>
</html>
