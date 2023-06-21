using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace LibraryManagement.system.Models
{
    public partial class AddBook : System.Web.UI.Page
    {
        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;

            string bookCategory = txtBookCategory.Text;
            string bookCatDetail = txtBookCatDetail.Text;
            string bookTitle = txtBookTitle.Text;
            int copyNum = int.Parse(txtCopyNum.Text);

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
                    string insertQuery = "INSERT INTO bookinfo (bookcategory, bookcatdetail, booktitle, copynum) " +
                                         "VALUES (@bookcategory, @bookcatdetail, @booktitle, @copynum)";
                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@bookcategory", bookCategory);
                    insertCommand.Parameters.AddWithValue("@bookcatdetail", bookCatDetail);
                    insertCommand.Parameters.AddWithValue("@booktitle", bookTitle);
                    insertCommand.Parameters.AddWithValue("@copynum", copyNum);
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
    }
}
