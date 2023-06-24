<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BorrowBook.aspx.cs" Inherits="LibraryManagement.system.BorrowBook" MaintainScrollPositionOnPostBack="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Borrow Book</title>
    <link rel="stylesheet" href="css/borrowbooks.css" />
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
                <li><a href="BorrowBook.aspx"><p>Borrow Book</p></a></li>
                <li><a href="ReturnBook.aspx"><p>Return Book</p></a></li>
                <li><a href="ManageBorrowers.aspx"><p>Manage Borrowers</p></a></li>
                <li><a href="ManageTransactions.aspx"><p>Manage Transactions</p></a></li>               
            </ul>
        </div>
        <hr class="vertical-line"/>
        <!--NAVBAR END-->          
            <div class="content">
                <form id="form2" runat="server">
                    <div class="box">
                        <h2>Borrow Book</h2>
                        <div>
                            <label>Borrower ID:</label>
                            <asp:TextBox CssClass="asptextbox" ID="BorrowerIdTextBox" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <label>Book ID:</label>
                            <asp:TextBox CssClass="asptextbox" ID="BookIdTextBox" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Button CssClass="aspbutton" ID="BorrowButton" runat="server" Text="Borrow" OnClick="BorrowButton_Click" />
                        </div>
                        <div>
                            <asp:Label CssClass="asplabel" ID="ErrorMessageLabel" runat="server" ForeColor="Red"></asp:Label>
                            <asp:Label CssClass="asplabel" ID="SuccessMessageLabel" runat="server" ForeColor="Green"></asp:Label>
                        </div>
                    </div>
                </form>
            </div>            
        </div>
</body>
</html>
