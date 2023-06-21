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
        <div class="content">
            <form id="form1" runat="server">
                <asp:TextBox ID="txtBookCategory" runat="server"></asp:TextBox>
                <asp:TextBox ID="txtBookCatDetail" runat="server"></asp:TextBox>
                <asp:TextBox ID="txtBookTitle" runat="server"></asp:TextBox>
                <asp:TextBox ID="txtCopyNum" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Add Book" OnClick="btnAddBook_Click" />
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

                <div class="input-group">
                    <label for="bookTitle">Book Title:</label>
                    <input type="text" id="bookTitle" name="bookTitle" runat="server" />
                </div>
                <div class="input-group">
                    <label for="bookCategory">Book Category:</label>
                    <input type="text" id="bookCategory" name="bookCategory" runat="server" />
                </div>
                <div class="input-group">
                    <label for="bookAuthor">Book Author:</label>
                    <input type="text" id="bookAuthor" name="bookAuthor" runat="server" />
                </div>
                <div class="input-group">
                    <label for="bookISBN">Book ISBN:</label>
                    <input type="text" id="bookISBN" name="bookISBN" runat="server" />
                </div>
                <div class="input-group">
                    <label for="copyNum">Copy Number:</label>
                    <input type="text" id="copyNum" name="copyNum" runat="server" />
                </div>
                <div class="input-group">
                    <label for="status">Status:</label>
                    <select id="status" name="status" runat="server">
                        <option value="IN">IN</option>
                        <option value="OUT">OUT</option>
                    </select>
                </div>
                <div class="input-group">
                    <label for="numberOfDaysAllowed">Number of Days Allowed:</label>
                    <input type="text" id="numberOfDaysAllowed" name="numberOfDaysAllowed" runat="server" />
                </div>
                <div class="input-group">
                    <input type="submit" id="btnAddBook" value="Add Book" runat="server" />
                </div>
            </form>
        </div>
    </div>       
</body>
</html>