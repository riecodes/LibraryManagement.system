using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace LibraryManagement.system.Models
{
    public partial class AddBook : System.Web.UI.Page
    {
        protected void BtnAddBook_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;

            string bookCategory = txtBookCategory.Text;
            string bookCatDetail = txtBookCatDetail.Text;
            string bookTitle = txtBookTitle.Text;
            int copyNum;
            int.TryParse(txtCopyNum.Text, out copyNum);
            string status = "IN";
            int numberOfDaysAllowed = string.IsNullOrEmpty(txtNumberOfDaysAllowed.Text) ? 3 : int.Parse(txtNumberOfDaysAllowed.Text);

            // Generate the book ID
            string bookID = GenerateBookID(bookCategory, copyNum);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the book already exists
                    string checkQuery = "SELECT COUNT(*) FROM bookinfo WHERE booktitle = @booktitle";
                    MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@booktitle", bookTitle);
                    int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        // Book already exists, show error message
                        lblMessage.Text = "The book already exists.";
                        return;
                    }

                    // Insert the book into the database
                    string insertQuery = "INSERT INTO bookinfo (bookcategory, bookcatdetail, booktitle, copynum, status, numberofdaysallowed) " +
                                         "VALUES (@bookcategory, @bookcatdetail, @booktitle, @copynum, @status, @numberofdaysallowed)";
                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@bookcategory", bookCategory);
                    insertCommand.Parameters.AddWithValue("@bookcatdetail", bookCatDetail);
                    insertCommand.Parameters.AddWithValue("@booktitle", bookTitle);
                    insertCommand.Parameters.AddWithValue("@copynum", copyNum);
                    insertCommand.Parameters.AddWithValue("@status", status);
                    insertCommand.Parameters.AddWithValue("@numberofdaysallowed", numberOfDaysAllowed);
                    insertCommand.ExecuteNonQuery();

                    // Display success message
                    lblMessage.Text = "Book added successfully.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }
        // Method to generate the book ID
        private string GenerateBookID(string bookCategory, int copyNum)
        {
            string categoryCode = bookCategory.Length >= 2 ? bookCategory.Substring(0, 2).ToUpper() : bookCategory.ToUpper();
            string copyNumFormatted = copyNum.ToString().PadLeft(3, '0');

            return $"{categoryCode}-{copyNumFormatted}";
        }

    }
}
