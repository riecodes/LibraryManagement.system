using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace LibraryManagement.system
{
    public partial class AddBook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblErrorMessage.Text = "";
            }
        }

        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            string bookCategory = txtBookCategory.Value.Trim();
            string bookCategoryDetail = txtBookCategoryDetail.Value.Trim();
            string bookTitle = txtBookTitle.Value.Trim();
            string copyNumValue = txtCopyNum.Value.Trim();
            string numberOfDaysAllowedValue = txtNumberOfDaysAllowed.Value.Trim();

            // Validate input
            if (string.IsNullOrEmpty(bookCategory))
            {
                lblErrorMessage.Text = "Please enter the book category.";
                return;
            }

            if (string.IsNullOrEmpty(bookCategoryDetail))
            {
                lblErrorMessage.Text = "Please enter the book category detail.";
                return;
            }

            if (string.IsNullOrEmpty(bookTitle))
            {
                lblErrorMessage.Text = "Please enter the book title.";
                return;
            }

            if (string.IsNullOrEmpty(copyNumValue))
            {
                lblErrorMessage.Text = "Please enter the copy number.";
                return;
            }

            if (!int.TryParse(copyNumValue, out int copyNum) || copyNum <= 0)
            {
                lblErrorMessage.Text = "Invalid copy number. Please enter a valid positive integer.";
                return;
            }

            if (string.IsNullOrEmpty(numberOfDaysAllowedValue))
            {
                lblErrorMessage.Text = "Please enter the number of days allowed.";
                return;
            }

            if (!int.TryParse(numberOfDaysAllowedValue, out int numberOfDaysAllowed) || numberOfDaysAllowed <= 0)
            {
                lblErrorMessage.Text = "Invalid number of days allowed. Please enter a valid positive integer.";
                return;
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the book already exists
                    string checkQuery = "SELECT COUNT(*) FROM bookinfo WHERE bookcategory = @bookCategory AND bookcatdetail = @bookCategoryDetail AND booktitle = @bookTitle";
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@bookCategory", bookCategory);
                        checkCommand.Parameters.AddWithValue("@bookCategoryDetail", bookCategoryDetail);
                        checkCommand.Parameters.AddWithValue("@bookTitle", bookTitle);
                        int existingBooksCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (existingBooksCount > 0)
                        {
                            lblErrorMessage.Text = "The book already exists.";
                            return;
                        }
                    }

                    // Generate book ID
                    string bookID = GenerateBookID(bookCategory, copyNum);

                    // Insert the book into the database
                    string insertQuery = "INSERT INTO bookinfo (bookcategory, bookcatdetail, bookid, booktitle, copynum, status, numberofdaysallowed) VALUES (@bookCategory, @bookCategoryDetail, @bookID, @bookTitle, @copyNum, 'IN', @numberOfDaysAllowed)";
                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@bookCategory", bookCategory);
                        insertCommand.Parameters.AddWithValue("@bookCategoryDetail", bookCategoryDetail);
                        insertCommand.Parameters.AddWithValue("@bookID", bookID);
                        insertCommand.Parameters.AddWithValue("@bookTitle", bookTitle);
                        insertCommand.Parameters.AddWithValue("@copyNum", copyNum);
                        insertCommand.Parameters.AddWithValue("@numberOfDaysAllowed", numberOfDaysAllowed);
                        insertCommand.ExecuteNonQuery();
                    }

                    // Display success message
                    lblErrorMessage.Text = "Book added successfully.";

                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "An error occurred: " + ex.Message;
            }
        }



        private string GenerateBookID(string bookCategory, int copyNum)
        {
            // Example: BC01-001
            string categoryCode = bookCategory.Substring(0, 2).ToUpper();
            string copyNumFormatted = copyNum.ToString().PadLeft(3, '0');
            return categoryCode + "-" + copyNumFormatted;
        }
    }
}
