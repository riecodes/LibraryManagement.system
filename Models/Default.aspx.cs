using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace LibraryManagement.system.Models
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearInputFields();
                BindGrid();
            }
            else
            {                
                BindGrid();
            }
             
        }

        protected void BtnAddBook_Click(object sender, EventArgs e)
        {
            string bookCategory = txtBookCategory.Value;
            string bookTitle = txtBookTitle.Value;
            string bookCatDetail = txtBookCategoryDetail.Value;

            if (string.IsNullOrWhiteSpace(bookCategory))
            {
                lblAddBookError.Text = "Book Category is required.";
            }
            else if (string.IsNullOrWhiteSpace(bookCatDetail))
            {
                lblAddBookError.Text = "Book Category Detail is required.";
            }
            else if (string.IsNullOrWhiteSpace(bookTitle))
            {
                lblAddBookError.Text = "Book Title is required.";
            }
            else if (!int.TryParse(txtCopyNumber.Value, out int copyNumber))
            {
                lblAddBookError.Text = "Invalid input for Copy Number.";
            }
            else if (!int.TryParse(txtNumberOfDaysAllowed.Value, out int daysAllowed))
            {
                lblAddBookError.Text = "Invalid input for Number of Days Allowed.";
            }
            else
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();

                        // Check if the book category exists
                        string categoryExistsQuery = "SELECT COUNT(*) FROM bookinfo WHERE bookcategory = @BookCategory";
                        MySqlCommand categoryExistsCmd = new MySqlCommand(categoryExistsQuery, con);
                        categoryExistsCmd.Parameters.AddWithValue("@BookCategory", bookCategory);
                        int categoryExistsCount = Convert.ToInt32(categoryExistsCmd.ExecuteScalar());

                        // Check if the book category already has books and the copy number is sequential
                        if (categoryExistsCount > 0)
                        {
                            string existingCopyNumQuery = "SELECT COUNT(*) FROM bookinfo WHERE bookcategory = @BookCategory AND copynum = @CopyNumber";
                            MySqlCommand existingCopyNumCmd = new MySqlCommand(existingCopyNumQuery, con);
                            existingCopyNumCmd.Parameters.AddWithValue("@BookCategory", bookCategory);
                            existingCopyNumCmd.Parameters.AddWithValue("@CopyNumber", copyNumber);
                            int existingCopyNumCount = Convert.ToInt32(existingCopyNumCmd.ExecuteScalar());

                            if (existingCopyNumCount > 0)
                            {
                                lblAddBookError.Text = $"A book with Copy Number {copyNumber} already exists.";
                                return;
                            }

                            string sequentialCopyNumQuery = "SELECT MAX(copynum) FROM bookinfo WHERE bookcategory = @BookCategory";
                            MySqlCommand sequentialCopyNumCmd = new MySqlCommand(sequentialCopyNumQuery, con);
                            sequentialCopyNumCmd.Parameters.AddWithValue("@BookCategory", bookCategory);
                            int maxCopyNum = Convert.ToInt32(sequentialCopyNumCmd.ExecuteScalar());

                            if (copyNumber != maxCopyNum + 1)
                            {
                                lblAddBookError.Text = $"A book with Copy Number {copyNumber - 1} is missing. Please ensure the copy numbers are sequential.";
                                return;
                            }
                        }
                        else if (copyNumber != 1)
                        {
                            lblAddBookError.Text = "Invalid copy number. Please ensure that a book category without a copy number 1 is not added with a higher copy number.";
                            return;
                        }

                        // Generate the book ID
                        string bookId = GenerateBookId(bookCategory, copyNumber, con);

                        // Check if the generated book ID is null or invalid
                        if (string.IsNullOrEmpty(bookId))
                        {
                            lblAddBookError.Text = "Invalid copy number.";
                            return;
                        }

                        // Insert the book information into the database
                        string insertQuery = "INSERT INTO bookinfo (bookid, bookcategory, bookcatdetail, booktitle, copynum, status, numberofdaysallowed) " +
                                             "VALUES (@BookId, @BookCategory, @BookCategoryDetail, @BookTitle, @CopyNumber, 'IN', @NumberOfDaysAllowed)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, con);
                        insertCmd.Parameters.AddWithValue("@BookId", bookId);
                        insertCmd.Parameters.AddWithValue("@BookCategory", bookCategory);
                        insertCmd.Parameters.AddWithValue("@BookCategoryDetail", bookCatDetail);
                        insertCmd.Parameters.AddWithValue("@BookTitle", bookTitle);
                        insertCmd.Parameters.AddWithValue("@CopyNumber", copyNumber);
                        insertCmd.Parameters.AddWithValue("@NumberOfDaysAllowed", daysAllowed);

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ClearInputFields();

                            lblAddBookError.Text = "Book added successfully."; 
                            
                            BindGrid();
                        }
                        else
                        {

                            lblAddBookError.Text = "An error occurred while adding the book.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblAddBookError.Text = "An error occurred: " + ex.Message;
                }
            }
        }

        public void ClearInputFields()
        {
            // Clear the input fields
            txtBookCategory.Value = "";
            txtBookCategoryDetail.Value = "";
            txtBookTitle.Value = "";
            txtCopyNumber.Value = "";
            txtNumberOfDaysAllowed.Value = "";

            // Set the default value for Number of Days Allowed
            txtNumberOfDaysAllowed.Value = "3";

            // Clear the error label
            lblAddBookError.Text = string.Empty;
        }

        private string GenerateBookId(string bookCategory, int copyNumber, MySqlConnection con)
        {
            string query = "SELECT MAX(copynum) FROM bookinfo WHERE bookcategory = @BookCategory";
            MySqlCommand maxCopyNumCmd = new MySqlCommand(query, con);
            maxCopyNumCmd.Parameters.AddWithValue("@BookCategory", bookCategory);

            object result = maxCopyNumCmd.ExecuteScalar();
            if (result == DBNull.Value)
            {
                // No previous copy numbers found, generate book ID as bookCategory-001
                string formattedCopyNumber = copyNumber.ToString("D3");
                return $"{bookCategory}-{formattedCopyNumber}";
            }
            else if (int.TryParse(result.ToString(), out int maxCopyNumber))
            {
                if (copyNumber > maxCopyNumber + 1)
                {
                    return null; // Invalid copy number
                }

                string formattedCopyNumber = copyNumber.ToString("D3");
                return $"{bookCategory}-{formattedCopyNumber}";
            }
            else
            {
                return null; // Unable to retrieve maximum copy number
            }
        }

        protected void BindGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM bookinfo";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                BookGridView.DataSource = dt;
                BookGridView.DataBind();

                // Reset the EditIndex after data binding
                BookGridView.EditIndex = -1;
            }
        }

        protected void BookGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            BookGridView.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void BookGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            BookGridView.EditIndex = -1;
            BindGrid();
        }

        protected void BookGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = BookGridView.Rows[e.RowIndex];

            // Retrieve the updated values from the TextBox controls in the GridView row
            string updatedBookCategory = GetUpdatedValue(row.FindControl("TextBoxBookCategory") as TextBox);
            string updatedBookCategoryDetail = GetUpdatedValue(row.FindControl("TextBoxBookCategoryDetail") as TextBox);
            string updatedBookId = GetUpdatedValue(row.FindControl("TextBoxBookBookId") as TextBox);
            string updatedBookTitle = GetUpdatedValue(row.FindControl("TextBoxBookTitle") as TextBox);

            
            int.TryParse(GetUpdatedValue(row.FindControl("TextBoxCopyNumber") as TextBox), out int updatedCopyNumber);

            string updatedStatus = GetUpdatedValue(row.FindControl("TextBoxStatus") as TextBox);
        
            int.TryParse(GetUpdatedValue(row.FindControl("TextBoxNumberOfDaysAllowed") as TextBox), out int updatedNumberOfDaysAllowed);

            // Update the data in the database using the retrieved values
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE bookinfo SET bookcategory = @UpdatedBookCategory, bookcatdetail = @UpdatedBookCategoryDetail, " +
                                   "bookid = @UpdatedBookId, booktitle = @UpdatedBookTitle, copynum = @UpdatedCopyNumber, " +
                                   "status = @UpdatedStatus, numberofdaysallowed = @UpdatedNumberOfDaysAllowed WHERE bookid = @BookId";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UpdatedBookCategory", updatedBookCategory);
                    cmd.Parameters.AddWithValue("@UpdatedBookCategoryDetail", updatedBookCategoryDetail);
                    cmd.Parameters.AddWithValue("@UpdatedBookId", updatedBookId);
                    cmd.Parameters.AddWithValue("@UpdatedBookTitle", updatedBookTitle);
                    cmd.Parameters.AddWithValue("@UpdatedCopyNumber", updatedCopyNumber);
                    cmd.Parameters.AddWithValue("@UpdatedStatus", updatedStatus);
                    cmd.Parameters.AddWithValue("@UpdatedNumberOfDaysAllowed", updatedNumberOfDaysAllowed);
                    cmd.Parameters.AddWithValue("@BookId", BookGridView.DataKeys[e.RowIndex].Value.ToString());

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    BookGridView.EditIndex = -1;
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                // Handle any error that occurred during the database update
                // You can display an error message or perform other actions as needed
                lblEditBookError.Text = "An error occurred while updating the record: " + ex.Message;
            }
        }

        private string GetUpdatedValue(TextBox textBox)
        {
            if (textBox != null)
            {
                return textBox.Text;
            }
            return string.Empty;
        }

        protected void SearchBookButton_Click(object sender, EventArgs e)
        {
            string searchQuery = SearchBook.Text;

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                SearchBookResults.Text = "Please enter a search query.";
                SearchBookGridView.DataSource = null;
                SearchBookGridView.DataBind();
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM bookinfo WHERE booktitle LIKE @SearchQuery";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                SearchBookGridView.DataSource = dt;
                SearchBookGridView.DataBind();

                SearchBookResults.Text = "Search Results: " + dt.Rows.Count + " books found.";
            }
        }


        protected void DeleteBookButton_Click(object sender, EventArgs e)
        {
            string bookID = DeleteBookId.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM bookinfo WHERE bookid = @BookID";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BookID", bookID);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rowsAffected > 0)
                    {
                        DeleteBookConfirmation.Text = "Book deleted successfully.";
                    }
                    else
                    {
                        DeleteBookConfirmation.Text = "Book not found.";
                    }
                }
                catch (Exception ex)
                {
                    lblDeleteBookError.Text = "An error occurred while deleting the book: " + ex.Message;
                }
            }

            BindGrid();
        }
    }
}
