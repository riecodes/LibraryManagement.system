<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LibraryManagement.system.Models.Default" MaintainScrollPositionOnPostBack="true"%>

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
                <li><a href="BorrowBook.aspx"><p>Borrow Book</p></a></li>
                <li><a href="ReturnBook.aspx"><p>Return Book</p></a></li>
                <li><a href="ManagePatrons.aspx"><p>Manage Borrowers</p></a></li>
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
                        <div class="form-group">
                            <label for="txtBookCategory">Book Category:</label>
                            <input type="text" id="txtBookCategory" name="txtBookCategory" runat="server" class="asptextbox" />
                        </div>
                        <div class="form-group">
                            <label for="txtBookCategoryDetail">Book Category Detail:</label>
                            <input type="text" id="txtBookCategoryDetail" name="txtBookCategoryDetail" runat="server" class="asptextbox" />
                        </div>
                        <div class="form-group">
                            <label for="txtBookTitle">Book Title:</label>
                            <input type="text" id="txtBookTitle" name="txtBookTitle" runat="server" class="asptextbox" />
                        </div>
                        <div class="form-group">
                            <label for="txtCopyNumber">Copy Number:</label>
                            <input type="number" id="txtCopyNumber" name="txtCopyNum" runat="server" class="asptextbox" min="1" />
                        </div>
                        <div class="form-group">
                            <label for="txtNumberOfDaysAllowed">Number of Days Allowed:</label>
                            <input type="number" id="txtNumberOfDaysAllowed" name="txtNumberOfDaysAllowed" runat="server" class="asptextbox" min="1" />
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnAddBook" runat="server" Text="Add Book" OnClick="BtnAddBook_Click" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="lblAddBookError" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:Label>
                        </div>
                    </div>                    

                <div class="box">
                    <h3>Search Book</h3>
                    <div>
                        <label for="SearchBook">Book Title:</label>
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
                    <asp:GridView ID="BookGridView" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="bookid"
                        OnRowEditing="BookGridView_RowEditing"
                        OnRowCancelingEdit="BookGridView_RowCancelingEdit"
                        OnRowUpdating="BookGridView_RowUpdating">    
                        <Columns>
                            <asp:TemplateField HeaderText="Book ID" ItemStyle-Width="200">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("bookid") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="TextBoxBookId" runat="server" Text='<%# Bind("bookid") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Book Category" ItemStyle-Width="130">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("bookcategory") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBoxBookCategory" runat="server" Text='<%# Bind("bookcategory") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Book Category Detail" ItemStyle-Width="300">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("bookcatdetail") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBoxBookCategoryDetail" runat="server" Text='<%# Bind("bookcatdetail") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Book Title" ItemStyle-Width="300">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("booktitle") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBoxBookTitle" runat="server" Text='<%# Bind("booktitle") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Copy Number" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("copynum") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBoxCopyNumber" runat="server" Text='<%# Bind("copynum") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBoxStatus" runat="server" Text='<%# Bind("status") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Number of Days Allowed" ItemStyle-Width="230">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("numberofdaysallowed") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBoxNumberOfDaysAllowed" runat="server" Text='<%# Bind("numberofdaysallowed") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" ItemStyle-Width="50"/>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblEditBookError" runat="server" CssClass="error-message"></asp:Label>
                </div>

                <div class="box">
                    <h3>Delete Book</h3>
                    <div>
                        <label for="DeleteBookId">Book ID:</label>
                        <asp:TextBox CssClass="asptextbox" ID="DeleteBookId" runat="server"></asp:TextBox>
                        <asp:Label ID="lblDeleteBookError" runat="server" CssClass="error-message"></asp:Label>
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