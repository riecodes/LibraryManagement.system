<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx" Inherits="LibraryManagement.system.Models.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Library Management System</title>
    <link rel="stylesheet" href="borrowbook.css" />
</head>
<body>
    <form id="form" runat="server">
        <div class="navbar">
            <ul>
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="AddBook.aspx">Add Book</a></li>
                <li><a href="BorrowBook.aspx">Borrow Book</a></li>
                <li><a href="ReturnBook.aspx">Return Book</a></li>
                <li><a href="ManagePatrons.aspx">Manage Patrons</a></li>
                <li><a href="ManageTransactions.aspx">Manage Transactions</a></li>
                <li><a href="ViewTransactions.aspx">View Transactions</a></li>
            </ul>
        </div>
        <div class="container">
            <h2>Welcome to Library Management System!</h2>
            <h4>Kindly select the necessary tabs that suit your query :)</h4>
        </div>
    </form>
</body>
</html>
