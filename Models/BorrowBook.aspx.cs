﻿using System;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace LibraryManagement.system
{
    public partial class BorrowBook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate the Book ID dropdown list
                PopulateBookIdDropDownList();
            }
        }

        protected void BorrowButton_Click(object sender, EventArgs e)
        {
            string borrowerId = BorrowerIdTextBox.Text;
            string bookId = BookIdDropDownList.SelectedValue;

            if (string.IsNullOrEmpty(borrowerId) && string.IsNullOrEmpty(bookId))
            {
                ErrorMessageLabel.Text = "Please enter the borrower ID and select a book.";
                SuccessMessageLabel.Text = "";
            }
            else if (string.IsNullOrEmpty(borrowerId))
            {
                ErrorMessageLabel.Text = "Please enter the borrower ID.";
                SuccessMessageLabel.Text = "";
            }
            else if (string.IsNullOrEmpty(bookId))
            {
                ErrorMessageLabel.Text = "Please select a book.";
                SuccessMessageLabel.Text = "";
            }
            else
            {
                if (ValidateBorrower(borrowerId))
                {
                    if (ValidateBookAvailability(bookId))
                    {
                        // Update book status to "OUT" in the database
                        UpdateBookStatus(bookId, "OUT");

                        // Decrement the borrower's numberofbooksallowed by 1
                        DecrementNumberOfBooksAllowed(borrowerId);

                        // Generate transaction details
                        string transactionId = GenerateTransactionId("B-DATE-");
                        string transactionCatId = "CAT001";
                        string transactionCatDetail = "BORROW";
                        DateTime transactionDate = DateTime.Now;

                        // Insert the transaction record into the database
                        InsertTransactionRecord(transactionId, transactionCatId, transactionCatDetail, borrowerId, bookId, transactionDate);

                        // Display success message
                        SuccessMessageLabel.Text = "Book borrowed successfully.";
                        ErrorMessageLabel.Text = "";
                    }
                    else
                    {
                        ErrorMessageLabel.Text = "Book is not available for borrowing.";
                        SuccessMessageLabel.Text = "";
                    }
                }
                else
                {
                    ErrorMessageLabel.Text = "Borrower ID not found.";
                    SuccessMessageLabel.Text = "";
                }
            }
        }

        private void PopulateBookIdDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "SELECT bookid FROM bookinfo";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string bookId = reader["bookid"].ToString();
                            BookIdDropDownList.Items.Add(bookId);
                        }
                    }
                }
            }
        }

        private bool ValidateBorrower(string borrowerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "SELECT COUNT(*) FROM borrowerinfo WHERE borrowerid = @BorrowerId";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private bool ValidateBookAvailability(string bookId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "SELECT status FROM bookinfo WHERE bookid = @BookId";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);
                    connection.Open();
                    string status = Convert.ToString(command.ExecuteScalar());
                    return status == "IN";
                }
            }
        }

        private void UpdateBookStatus(string bookId, string status)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "UPDATE bookinfo SET status = @Status WHERE bookid = @BookId";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DecrementNumberOfBooksAllowed(string borrowerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "UPDATE borrowerinfo SET numberofbooksallowed = numberofbooksallowed - 1 WHERE borrowerid = @BorrowerId";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private string GenerateTransactionId(string prefix)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "SELECT COUNT(*) FROM transactioninfo WHERE transid LIKE @Prefix + '%' AND transdate = @TransactionDate";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Prefix", prefix);
                    command.Parameters.AddWithValue("@TransactionDate", DateTime.Today);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    string transactionId = prefix + DateTime.Now.ToString("yyyyMMdd") + "-" + (count + 1).ToString("D3");
                    return transactionId;
                }
            }
        }

        private void InsertTransactionRecord(string transactionId, string transactionCatId, string transactionCatDetail, string borrowerId, string bookId, DateTime transactionDate)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "INSERT INTO transactioninfo (transid, transcatid, transcatdetail, borrowerid, bookid, transdate) " +
                           "VALUES (@TransId, @TransCatId, @TransCatDetail, @BorrowerId, @BookId, @TransDate)";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TransId", transactionId);
                    command.Parameters.AddWithValue("@TransCatId", transactionCatId);
                    command.Parameters.AddWithValue("@TransCatDetail", transactionCatDetail);
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    command.Parameters.AddWithValue("@TransDate", transactionDate);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
