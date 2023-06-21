<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBook.aspx.cs" Inherits="LibraryManagement.system.Models.AddBook" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Book</title>
    <link rel="stylesheet" href="/App_Data/css/addbook.css">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Inconsolata&family=Montserrat&family=Roboto&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <div class="navbar">
            <ul>
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="AddBook.aspx">Add Book</a></li>
                <li><a href="BorrowBook.aspx">Borrow Book</a></li>
                <li><a href="ReturnBook.aspx">Return Book</a></li>
                <li><a href="ViewTransactions.aspx">View Transactions</a></li>
            </ul>
        </div>
    </form>
</body>
</html>
