<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePatrons.aspx.cs" Inherits="LibraryManagement.system.AddPatron" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Patron</title>
    <link rel="stylesheet" href="css/managepatrons.css">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Inconsolata&family=Montserrat&family=Roboto&display=swap" rel="stylesheet">
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
            <form id="form" runat="server">  
                <div>
                    <h1>Add Patron</h1>
                    <asp:Label ID="ErrorMessageLabel" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <asp:Label ID="SuccessMessageLabel" runat="server" ForeColor="Green" Visible="false"></asp:Label>
                    <p>
                        <asp:Label ID="BorrowerNameLabel" runat="server" Text="Borrower Name:"></asp:Label>
                        <asp:TextBox ID="BorrowerNameTextBox" runat="server"></asp:TextBox>
                    </p> 
                    <p>
                        <asp:Label ID="CourseLabel" runat="server" Text="Course:"></asp:Label>
                        <asp:TextBox ID="CourseTextBox" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Label ID="SectionLabel" runat="server" Text="Section:"></asp:Label>
                        <asp:TextBox ID="SectionTextBox" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Button ID="SubmitButton" runat="server" Text="Submit" OnClick="SubmitButton_Click" />
                    </p>
                </div>
            </form>  
        </div>
    </div>
    
</body>
</html>
