using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.HtmlControls;

namespace LibraryManagement.system
{
    public partial class ReturnBook : System.Web.UI.Page
    {
        protected void ReturnButton_Click(object sender, EventArgs e)
        {
            string borrowerId = BorrowerIdTextBox.Value;
            string bookId = BookIdTextBox.Value;

            try
            {
                // Retrieve the borrow date and return date from the database
                DateTime borrowDate = GetBorrowDate(borrowerId, bookId);
                DateTime returnDate = GetReturnDate(borrowerId, bookId);

                // Calculate the number of days late
                int daysLate = CalculateDaysLate(borrowDate, returnDate);

                SuccessMessageLabel.Text = "Borrow date: " + borrowDate.ToString();
                SuccessMessageLabel2.Text = "Return date: " + returnDate.ToString();
                SuccessMessageLabel3.Text = "Days Late: " + daysLate.ToString();
            }
            catch (Exception ex)
            {
                // Handle the exception
                ErrorMessageLabel.Text = "Error: " + ex.Message;
            }
        }

        private DateTime GetBorrowDate(string borrowerId, string bookId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
            {
                connection.Open();

                string borrowDateQuery = "SELECT transdate FROM transactioninfo WHERE borrowerid = @BorrowerId AND bookid = @BookId AND transcatdetail = 'BORROW' ORDER BY transdate DESC LIMIT 1";

                using (MySqlCommand borrowDateCommand = new MySqlCommand(borrowDateQuery, connection))
                {
                    borrowDateCommand.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    borrowDateCommand.Parameters.AddWithValue("@BookId", bookId);

                    object borrowDateResult = borrowDateCommand.ExecuteScalar();
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

                string returnDateQuery = "SELECT transdate FROM transactioninfo WHERE borrowerid = @BorrowerId AND bookid = @BookId AND transcatdetail = 'RETURN' ORDER BY transdate DESC LIMIT 1";

                using (MySqlCommand returnDateCommand = new MySqlCommand(returnDateQuery, connection))
                {
                    returnDateCommand.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    returnDateCommand.Parameters.AddWithValue("@BookId", bookId);

                    object returnDateResult = returnDateCommand.ExecuteScalar();
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
            // Calculate the difference between borrow date and return date
            TimeSpan difference = returnDate - borrowDate;
            int daysLate = difference.Days;

            return daysLate;
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

        private bool ValidateBookExistence(MySqlConnection connection, string bookId)
        {
            string query = "SELECT COUNT(*) FROM bookinfo WHERE bookid = @BookId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private bool ValidateBookStatus(MySqlConnection connection, string bookId)
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
