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
                return;
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    if (!ValidateBorrower(connection, borrowerId))
                    {
                        ErrorMessageLabel.Text = "Borrower ID not found or invalid.";
                        SuccessMessageLabel.Text = "";
                        return;
                    }

                    if (!ValidateBookReturn(connection, bookId))
                    {
                        ErrorMessageLabel.Text = "Invalid book ID or the book is already returned.";
                        SuccessMessageLabel.Text = "";
                        return;
                    }

                    // Check for late returns
                    int numberOfDaysAllowed = GetNumberOfDaysAllowed(connection, bookId);
                    int daysLate = CalculateDaysLate(connection, borrowerId, numberOfDaysAllowed);

                    if (daysLate > 3)
                    {
                        // Display penalty message
                        ErrorMessageLabel.Text = "Late return! A penalty will be applied.";
                    }
                    else if (daysLate > 0)
                    {
                        // Display overdue message
                        ErrorMessageLabel.Text = "Overdue return! The book is " + daysLate + " day(s) late.";
                    }
                    else
                    {
                        // No penalty or overdue
                        ErrorMessageLabel.Text = "";
                    }

                    // Update book status to "IN" in the database
                    UpdateBookStatus(connection, bookId, "IN");

                    // Increment the borrower's numberofbooksallowed by 1
                    IncrementNumberOfBooksAllowed(connection, borrowerId);

                    // Generate transaction details
                    string transactionId = GenerateTransactionId("R-DATE-");
                    string transactionCatId = "RCAT002";
                    string transactionCatDetail = "RETURN";
                    DateTime transactionDate = DateTime.Now;

                    // Insert the transaction record into the database
                    InsertTransactionRecord(connection, transactionId, transactionCatId, transactionCatDetail, borrowerId, bookId, transactionDate);

                    // Clear the input fields
                    BorrowerIdTextBox.Value = "";
                    BookIdTextBox.Value = "";

                    // Display success message
                    SuccessMessageLabel.Text = "Book returned successfully.";
                    ErrorMessageLabel.Text = "";
                }
            }
            catch (Exception ex)
            {
                ErrorMessageLabel.Text = "An error occurred while processing the request. Error: " + ex.Message;
                SuccessMessageLabel.Text = "";
                // Log the exception or perform additional error handling if needed
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

        private int GetNumberOfDaysAllowed(MySqlConnection connection, string bookId)
        {
            string query = "SELECT numberofdaysallowed FROM bookinfo WHERE bookid = @BookId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);
                int numberOfDaysAllowed = Convert.ToInt32(command.ExecuteScalar());
                return numberOfDaysAllowed;
            }
        }

        private int CalculateDaysLate(MySqlConnection connection, string borrowerId, int numberOfDaysAllowed)
        {
            string query = "SELECT DATEDIFF(CURDATE(), transdate) FROM transactioninfo WHERE borrowerid = @BorrowerId AND transcatdetail = 'BORROW' AND bookid IN (SELECT bookid FROM bookinfo WHERE status = 'OUT') ORDER BY transdate DESC LIMIT 1";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int daysLate = Convert.ToInt32(result) - numberOfDaysAllowed;
                    return Math.Max(daysLate, 0);
                }
                return 0;
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
            string query = "SELECT COUNT(*) FROM transactioninfo WHERE transid LIKE @Prefix AND transdate = CURDATE()";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Prefix", prefix + "%");
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    string transactionId = prefix + DateTime.Now.ToString("yyyyMMdd") + "-" + (count + 1).ToString("D3");
                    return transactionId;
                }
            }
        }

        private void InsertTransactionRecord(MySqlConnection connection, string transactionId, string transactionCatId, string transactionCatDetail, string borrowerId, string bookId, DateTime transactionDate)
        {
            string query = "INSERT INTO transactioninfo (transid, transcatid, transcatdetail, borrowerid, bookid, transdate) " +
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
