﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePatrons.aspx.cs" Inherits="LibraryManagement.system.Models.ManagePatrons" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Library Management System - Manage Patrons</title>
    <link rel="stylesheet" href="css/managepatrons.css" />
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="" />
    <link href="https://fonts.googleapis.com/css2?family=Inconsolata&family=Montserrat&family=Roboto&display=swap" rel="stylesheet" />
</head>
<body>
    <form id="form3" runat="server">
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
                <div class="box">
                    <h3>Add Borrower</h3>
                    <div>
                        <label for="AddPatronName">Name:</label>
                        <asp:TextBox CssClass="asptextbox" ID="AddPatronName" runat="server" ></asp:TextBox>
                    </div>
                    <div>
                        <label for="AddPatronCourse">Course:</label>
                        <asp:TextBox CssClass="asptextbox" ID="AddPatronCourse" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <label for="AddPatronSection">Section:</label>
                        <asp:TextBox CssClass="asptextbox" ID="AddPatronSection" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Button CssClass="aspbutton" ID="AddPatronButton" runat="server" Text="Add Patron" OnClick="AddPatronButton_Click" />
                    </div>
                    <div>
                        <asp:Label CssClass="asplabel" ID="AddPatronConfirmation" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="box">
                    <h3>Search Borrower</h3>
                    <div>
                        <label for="SearchPatronName">Name:</label>
                        <asp:TextBox CssClass="asptextbox" ID="SearchPatronName" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Button CssClass="aspbutton" ID="SearchPatronButton" runat="server" Text="Search" OnClick="SearchPatronButton_Click" />
                    </div>                    
                    <div>
                        <asp:GridView CssClass="aspgridview" ID="SearchPatronGridView" runat="server"></asp:GridView>
                    </div>
                    <div>
                        <asp:Label CssClass="asplabel" ID="SearchPatronResults" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="box">
                    <h3>Edit Borrower</h3>
                    <div>
                        <label for="EditPatronId">Borrower ID:</label>
                        <asp:TextBox CssClass="asptextbox" ID="EditPatronId" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <label for="EditPatronName">Name:</label>
                        <asp:TextBox CssClass="asptextbox" ID="EditPatronName" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <label for="EditPatronCourse">Course:</label>
                        <asp:TextBox CssClass="asptextbox" ID="EditPatronCourse" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <label for="EditPatronSection">Section:</label>
                        <asp:TextBox CssClass="asptextbox" ID="EditPatronSection" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Button CssClass="aspbutton" ID="EditPatronButton" runat="server" Text="Edit Patron" OnClick="EditPatronButton_Click" />
                    </div>
                    <div>
                        <asp:Label CssClass="asplabel" ID="EditPatronConfirmation" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="box">
                    <h3>Delete Borrower</h3>
                    <div>
                        <label for="DeletePatronId">Borrower ID:</label>
                        <asp:TextBox CssClass="asptextbox" ID="DeletePatronId" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Button CssClass="aspbutton" ID="DeletePatronButton" runat="server" Text="Delete Patron" OnClick="DeletePatronButton_Click" />
                    </div>
                    <div>
                        <asp:Label CssClass="asplabel" ID="DeletePatronConfirmation" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
