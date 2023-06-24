<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageTransactions_New.aspx.cs" Inherits="LibraryManagement.system.Models.ManageTransactions_New" MaintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Transactions</title>
    <link rel="stylesheet" href="css/managetransactions_New.css"/>
    <link rel="preconnect" href="https://fonts.googleapis.com"/>
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin=""/>
    <link href="https://fonts.googleapis.com/css2?family=Inconsolata&family=Montserrat&family=Roboto&display=swap" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!--NAVBAR START-->
            <div class="navbar">
                <ul>
                    <li><a href="Default.aspx"><p>Home</p></a></li>
                    <li><a href="BorrowBook.aspx"><p>Borrow Book</p></a></li>
                    <li><a href="ReturnBook.aspx"><p>Return Book</p></a></li>
                    <li><a href="ManageBorrowers.aspx"><p>Manage Borrowers</p></a></li>
                    <li><a href="ManageTransactions_New.aspx"><p>View Transactions</p></a></li>              
                </ul>
            </div>
            <hr class="vertical-line"/>
            <!--NAVBAR END-->
            <div class="content">
                <div class="box">
                    <h1>Transaction Information</h1>
                    <asp:GridView ID="TransactionGridView" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Transaction ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("transid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transaction Category ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("transcatid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transaction Category Detail">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("transcatdetail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Borrower ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("borrowerid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Book ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("bookid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transaction Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Eval("transdate", "{0:yyyy-MM-dd}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
