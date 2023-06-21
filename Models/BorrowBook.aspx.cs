using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;

namespace LibraryManagement.system
{
    public partial class BorrowBook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBookIds();
            }
        }

        protected void LoadBookIds()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "SELECT bookid FROM bookinfo WHERE status = 'IN'";

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

        protected void BorrowButton_Click(object sender, EventArgs e)
        {
            string borrowerId = BorrowerIdTextBox.Text;
            string bookId = BookIdDropDownList.SelectedValue;
            DateTime borrowDate = DateTime.Now;
            DateTime returnDate = borrowDate.AddDays(GetNumberOfDaysAllowed(bookId));

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "INSERT INTO transactioninfo (borrowerid, bookid, borrowdate, returndate) VALUES (@borrowerid, @bookid, @borrowdate, @returndate)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@borrowerid", borrowerId);
                    command.Parameters.AddWithValue("@bookid", bookId);
                    command.Parameters.AddWithValue("@borrowdate", borrowDate);
                    command.Parameters.AddWithValue("@returndate", returnDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            // Update book status to 'OUT'
            UpdateBookStatus(bookId, "OUT");

            // Clear the form
            BorrowerIdTextBox.Text = string.Empty;
            BookIdDropDownList.SelectedIndex = 0;

            SuccessMessageLabel.Text = "Book borrowed successfully!";
        }

        protected int GetNumberOfDaysAllowed(string bookId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "SELECT numberofdaysallowed FROM bookinfo WHERE bookid = @bookid";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bookid", bookId);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }

                    return 0;
                }
            }
        }

        protected void UpdateBookStatus(string bookId, string status)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            string query = "UPDATE bookinfo SET status = @status WHERE bookid = @bookid";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@bookid", bookId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
