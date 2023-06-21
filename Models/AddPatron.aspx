<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPatron.aspx.cs" Inherits="LibraryManagement.system.AddPatron" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Patron</title>
</head>
<body>
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
