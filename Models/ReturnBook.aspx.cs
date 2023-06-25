using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

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
                ErrorMessageLabel.Text = "Please enter the Borrower ID and the Book ID";
                SuccessMessageLabel.Text = "";
                return;
            }

            if (!ValidateBorrower(borrowerId))
            {
                ErrorMessageLabel.Text = "Invalid Borrower ID";
                SuccessMessageLabel.Text = "";
                return;
            }

            if (!ValidateBookExistence(bookId))
            {
                ErrorMessageLabel.Text = "Invalid Book ID";
                SuccessMessageLabel.Text = "";
                return;
            }

            // Validate book status
            if (!ValidateBookAvailability(bookId))
            {
                ErrorMessageLabel.Text = "Book is already returned.";
                SuccessMessageLabel.Text = "";
                return;
            }

            // Update book status to "IN" in the database
            UpdateBookStatus(bookId, "IN");

            IncrementNumberOfBooksAllowed(borrowerId);

            string transactionId = GenerateTransactionId("R-");
            string transactionCatId = "RCAT002";
            string transactionCatDetail = "RETURN";
            DateTime transactionDate = DateTime.Now;

            // Insert the transaction record into the database
            InsertTransactionRecord(transactionId, transactionCatId, transactionCatDetail, borrowerId, bookId, transactionDate);

            // Retrieve the borrow date and return date from the database
            DateTime borrowDate;
            DateTime returnDate;

            try
            {
                borrowDate = GetBorrowDate(borrowerId, bookId);
                returnDate = GetReturnDate(borrowerId, bookId);
            }
            catch (Exception ex)
            {
                ErrorMessageLabel.Text = ex.Message;
                return;
            }

            // Calculate days late
            int daysLate = CalculateDaysLate(borrowDate, returnDate);

            // Handle penalty for returning after 3 days
            if (daysLate > 3)
            {
                ErrorMessageLabel.Text = "Penalty incurred for returning after 3 days. Days Late: " + daysLate;
            }

            // Display success message
            SuccessMessageLabel.Text = "Book returned successfully.";
        }


        private bool ValidateBorrower(string borrowerId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM borrowerinfo WHERE borrowerid = @BorrowerId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private bool ValidateBookExistence(string bookId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM bookinfo WHERE bookid = @BookId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);

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
                    return status == "OUT";
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


        private void IncrementNumberOfBooksAllowed(string borrowerId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "UPDATE borrowerinfo SET numberofbooksallowed = numberofbooksallowed + 1 WHERE borrowerid = @BorrowerId";

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


        private DateTime GetBorrowDate(string borrowerId, string bookId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
            {
                connection.Open();

                string query = "SELECT transdate FROM transactioninfo WHERE borrowerid = @BorrowerId AND bookid = @BookId AND transcatdetail = 'BORROW' ORDER BY transdate DESC LIMIT 1";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    command.Parameters.AddWithValue("@BookId", bookId);

                    object borrowDateResult = command.ExecuteScalar();
                    if (borrowDateResult != null && borrowDateResult != DBNull.Value)
                    {
                        return (DateTime)borrowDateResult;
                    }
                    else
                    {
                        throw new Exception("Borrow date not found for the specified borrower and book.");
                    }
                }
            }
        }

        private DateTime GetReturnDate(string borrowerId, string bookId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
            {
                connection.Open();

                string query = "SELECT transdate FROM transactioninfo WHERE borrowerid = @BorrowerId AND bookid = @BookId AND transcatdetail = 'RETURN' ORDER BY transdate DESC LIMIT 1";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    command.Parameters.AddWithValue("@BookId", bookId);

                    object returnDateResult = command.ExecuteScalar();
                    if (returnDateResult != null && returnDateResult != DBNull.Value)
                    {
                        return (DateTime)returnDateResult;
                    }
                    else
                    {
                        throw new Exception("Return date not found for the specified borrower and book.");
                    }
                }
            }
        }

        private int CalculateDaysLate(DateTime borrowDate, DateTime returnDate)
        {
            return (int)(returnDate - borrowDate).TotalDays;
        }

        private void InsertTransactionRecord(string transactionId, string transactionCatId, string transactionCatDetail, string borrowerId, string bookId, DateTime transactionDate)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
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
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private string GenerateTransactionId(string prefix)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM transactioninfo WHERE transid LIKE @Prefix AND transdate = CURDATE()";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Prefix", prefix + "%");

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    string transactionId = prefix + DateTime.Now.ToString("yyyyMMdd") + "-" + (count + 1).ToString("D3");
                    return transactionId;
                }
            }
        }

    }
}
