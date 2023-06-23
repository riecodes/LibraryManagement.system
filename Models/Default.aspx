<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LibraryManagement.system.Models.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Library Management System</title>
    <link rel="stylesheet" href="css/default.css" />
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
            <div class="welcome">
                <h2>Welcome to Library Management System!</h2>
                <h4>Kindly select the necessary tabs that suit your query :)</h4>
            </div>
                <form id="form" runat="server">
                    <div class="box">                    
                        <h2>Add Book</h2>
                        <div class="row">
                            <div class="form-group">
                                <label for="txtBookCategory">Book Category:</label>
                                <input type="text" id="txtBookCategory" name="txtBookCategory" runat="server" class="asptextbox" />
                                <asp:RequiredFieldValidator ID="rfvBookCategory" runat="server" ControlToValidate="txtBookCategory" Text="* Please enter the book category."></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label for="txtBookCategoryDetail">Book Category Detail:</label>
                                <input type="text" id="txtBookCategoryDetail" name="txtBookCategoryDetail" runat="server" class="asptextbox" />
                                <asp:RequiredFieldValidator ID="rfvBookCategoryDetail" runat="server" ControlToValidate="txtBookCategoryDetail" Text="* Please enter the book category detail."></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label for="txtBookTitle">Book Title:</label>
                                <input type="text" id="txtBookTitle" name="txtBookTitle" runat="server" class="asptextbox" />
                                <asp:RequiredFieldValidator ID="rfvBookTitle" runat="server" ControlToValidate="txtBookTitle" Text="* Please enter the book title."></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label for="txtCopyNum">Copy Number:</label>
                                <input type="number" id="txtCopyNum" name="txtCopyNum" runat="server" class="asptextbox" min="1" />
                                <asp:RequiredFieldValidator ID="rfvCopyNum" runat="server" ControlToValidate="txtCopyNum" Text="* Please enter the copy number."></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label for="txtNumberOfDaysAllowed">Number of Days Allowed:</label>
                                <input type="number" id="txtNumberOfDaysAllowed" name="txtNumberOfDaysAllowed" runat="server" class="asptextbox" min="1" />
                                <asp:RequiredFieldValidator ID="rfvNumberOfDaysAllowed" runat="server" ControlToValidate="txtNumberOfDaysAllowed" Text="* Please enter the number of days allowed."></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <asp:Button ID="btnAddBook" runat="server" Text="Add Book" OnClick="btnAddBook_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="error-message"></asp:Label>
                        </div>
                    </div>

                    <div class="box">
                        <h3>Search Book</h3>
                        <div>
                            <label for="SearchBook">Name:</label>
                            <asp:TextBox CssClass="asptextbox" ID="SearchBook" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Button CssClass="aspbutton" ID="SearchBookButton" runat="server" Text="Search" OnClick="SearchBookButton_Click" />
                        </div>                    
                        <div>
                            <asp:GridView CssClass="aspgridview" ID="SearchBookGridView" runat="server"></asp:GridView>
                        </div>                        
                        <div>
                            <asp:Label CssClass="asplabel" ID="SearchBookResults" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="box">
                        <h3>Edit Book</h3>
                        <asp:GridView ID="BookGridView" runat="server" AutoGenerateColumns="False" OnRowEditing="BookGridView_RowEditing" OnRowCancelingEdit="BookGridView_RowCancelingEdit" OnRowUpdating="BookGridView_RowUpdating" OnRowDeleting="BookGridView_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="bookcategory" HeaderText="Book Category" SortExpression="bookcategory" />
                                <asp:BoundField DataField="bookcatdetail" HeaderText="Book Category Detail" SortExpression="bookcatdetail" />
                                <asp:BoundField DataField="bookid" HeaderText="Book ID" SortExpression="bookid" />
                                <asp:BoundField DataField="booktitle" HeaderText="Book Title" SortExpression="booktitle" />
                                <asp:BoundField DataField="copynum" HeaderText="Copy Number" SortExpression="copynum" />
                                <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                                <asp:BoundField DataField="numberofdaysallowed" HeaderText="Number of Days Allowed" SortExpression="numberofdaysallowed" />
                                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                            </Columns>
                        </asp:GridView>
                    </div>

                    <div class="box">
                        <h3>Delete Book</h3>
                        <div>
                            <label for="DeleteBookId">Book ID:</label>
                            <asp:TextBox CssClass="asptextbox" ID="DeleteBookId" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Button CssClass="aspbutton" ID="DeleteBookButton" runat="server" Text="Delete Book" OnClick="DeleteBookButton_Click" />
                        </div>
                        <div>
                            <asp:Label CssClass="asplabel" ID="DeleteBookConfirmation" runat="server"></asp:Label>
                        </div>
                    </div>
                </form>
            </div>            
    </div>           
</body>
</html>