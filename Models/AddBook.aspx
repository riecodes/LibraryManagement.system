<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBook.aspx.cs" Inherits="LibraryManagement.system.Models.AddBook" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Book</title>
    <link rel="stylesheet" href="css/addbook.css" />
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="" />
    <link href="https://fonts.googleapis.com/css2?family=Inconsolata&family=Montserrat&family=Roboto&display=swap" rel="stylesheet" />
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
        <hr class="vertical-line" />
        <!--NAVBAR END-->
        <div class="container">           
            <div class="content">
                <form id="form" runat="server">
                    <h2>Add Book</h2>
                    <div class="input-group">
                        <label for="txtBookCategory">Book Category:</label>
                        <asp:TextBox ID="txtBookCategory" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group">
                        <label for="txtBookCatDetail">Book Category Detail:</label>
                        <asp:TextBox ID="txtBookCatDetail" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group">
                        <label for="txtBookTitle">Book Title:</label>
                        <asp:TextBox ID="txtBookTitle" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group">
                        <label for="txtCopyNum">Copy Number:</label>
                        <asp:TextBox ID="txtCopyNum" runat="server" TextMode="Number"></asp:TextBox>
                    </div>
                    <div class="input-group">
                        <label for="txtNumberOfDaysAllowed">Number of Days Allowed:</label>
                        <asp:TextBox ID="txtNumberOfDaysAllowed" runat="server" TextMode="Number" value="3"></asp:TextBox>
                    </div>
                    <div class="input-group">
                        <asp:Button ID="btnAddBook" runat="server" Text="Add Book" OnClick="BtnAddBook_Click" CssClass="btn" />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="Message"></asp:Label>
                </form>
            </div>        
        </div>
    </div>       
</body>
</html>
