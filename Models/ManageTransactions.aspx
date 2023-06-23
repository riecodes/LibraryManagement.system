<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageTransactions.aspx.cs" Inherits="LibraryManagement.system.Models.ManageTransactions" MaintainScrollPositionOnPostBack="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Transactions</title>
    <link rel="stylesheet" href="css/managetransactions.css"/>
    <link rel="preconnect" href="https://fonts.googleapis.com"/>
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin=""/>
    <link href="https://fonts.googleapis.com/css2?family=Inconsolata&family=Montserrat&family=Roboto&display=swap" rel="stylesheet"/>
</head>
<body>
    <div class="container">
        <!--NAVBAR START-->
        <div class="navbar">
            <ul>
                <li><a href="Default.aspx"><p>Home</p></a></li>
                <li><a href="BorrowBook.aspx"><p>Borrow Book</p></a></li>
                <li><a href="ReturnBook.aspx"><p>Return Book</p></a></li>
                <li><a href="ManagePatrons.aspx"><p>Manage Borrowers</p></a></li>
                <li><a href="ManageTransactions.aspx"><p>Manage Transactions</p></a></li>                
            </ul>
        </div>
        <hr class="vertical-line"/>
        <!--NAVBAR END-->
        <div class="content">
            <form id="form" runat="server">
            
            </form>
        </div>
    </div>
</body>
</html>
