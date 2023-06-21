<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePatrons.aspx.cs" Inherits="LibraryManagement.system.AddPatron" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Patron</title>
</head>
<body>
    <div class="navbar">
        <div>
            <ul>
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="ManagePatron.aspx">Manage Patrons</a></li>
                <li><a href="AddBook.aspx">Add Book</a></li>
                <li><a href="BorrowBook.aspx">Borrow Book</a></li>
                <li><a href="ReturnBook.aspx">Return Book</a></li>
                <li><a href="ViewTransactions.aspx">View Transactions</a></li>
            </ul>
        </div>
    </div>
    <form id="form1" runat="server">  
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
</body>
</html>
