using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web.UI.WebControls;

namespace LibraryManagement.system
{
    public partial class ReturnBook : System.Web.UI.Page
    {
        protected void ReturnButton_Click(object sender, EventArgs e)
        {
            string borrowerId = BorrowerIdTextBox.Value;
            string bookId = BookIdTextBox.Value;

            if (string.IsNullOrEmpty(borrowerId) || string.IsNullOrEmpty(bookId))
            {
                ErrorMessageLabel.Text = "Please enter the borrower ID and book ID.";
                SuccessMessageLabel.Text = "";
            }
            else
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
                    {
                        connection.Open();

                        // Check if borrower exists
                        if (ValidateBorrower(connection, borrowerId))
                        {
                            // Check if book return is valid
                            if (ValidateBookReturn(connection, bookId))
                            {
                                // Update book status to "IN" in the database
                                UpdateBookStatus(connection, bookId, "IN");

                                // Increment the borrower's numberofbooksallowed by 1
                                IncrementNumberOfBooksAllowed(connection, borrowerId);

                                // Generate transaction details
                                string transactionId = GenerateTransactionId("R-DATE-");
                                string transactionCatId = GetBookCategory(connection, bookId);
                                string transactionCatDetail = "RETURN";
                                DateTime transactionDate = DateTime.Now;

                                // Insert the transaction record into the database
                                InsertTransactionRecord(connection, transactionId, transactionCatId, transactionCatDetail, borrowerId, bookId, transactionDate);

                                // Display success message
                                SuccessMessageLabel.Text = "Book returned successfully.";
                                ErrorMessageLabel.Text = "";
                            }
                            else
                            {
                                ErrorMessageLabel.Text = "Invalid book ID or the book is already returned.";
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
                catch (Exception)
                {
                    ErrorMessageLabel.Text = "An error occurred while processing the request. Please try again later.";
                    SuccessMessageLabel.Text = "";
                    // Log the exception or perform additional error handling if needed
                }
            }
        }

        private bool ValidateBorrower(MySqlConnection connection, string borrowerId)
        {
            string query = "SELECT COUNT(*) FROM borrowerinfo WHERE borrowerid = @BorrowerId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private bool ValidateBookReturn(MySqlConnection connection, string bookId)
        {
            string query = "SELECT status FROM bookinfo WHERE bookid = @BookId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);
                string status = command.ExecuteScalar()?.ToString();
                return status == "OUT";
            }
        }

        private void UpdateBookStatus(MySqlConnection connection, string bookId, string status)
        {
            string query = "UPDATE bookinfo SET status = @Status WHERE bookid = @BookId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@BookId", bookId);
                command.ExecuteNonQuery();
            }
        }

        private void IncrementNumberOfBooksAllowed(MySqlConnection connection, string borrowerId)
        {
            string query = "UPDATE borrowerinfo SET numberofbooksallowed = numberofbooksallowed + 1 WHERE borrowerid = @BorrowerId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                command.ExecuteNonQuery();
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

        private string GetBookCategory(MySqlConnection connection, string bookId)
        {
            string query = "SELECT category FROM bookinfo WHERE bookid = @BookId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);
                return command.ExecuteScalar()?.ToString();
            }
        }

        private void InsertTransactionRecord(MySqlConnection connection, string transactionId, string transactionCatId, string transactionCatDetail, string borrowerId, string bookId, DateTime transactionDate)
        {
            string query = "INSERT INTO transactioninfo (transactionid, transactioncatid, transactioncatdetail, borrowerid, bookid, transactiondate) " +
                           "VALUES (@TransactionId, @TransactionCatId, @TransactionCatDetail, @BorrowerId, @BookId, @TransactionDate)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TransactionId", transactionId);
                command.Parameters.AddWithValue("@TransactionCatId", transactionCatId);
                command.Parameters.AddWithValue("@TransactionCatDetail", transactionCatDetail);
                command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@TransactionDate", transactionDate);
                command.ExecuteNonQuery();
            }
        }
    }
}
