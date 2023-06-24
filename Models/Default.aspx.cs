using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace LibraryManagement.system.Models
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearInputFields();
                BindBookGrid();
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
                            lblAddBookError.Text = "Book added successfully.";                             
                            BindBookGrid();
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
            txtBookCategory.Value = string.Empty;
            txtBookCategoryDetail.Value = string.Empty;
            txtBookTitle.Value = string.Empty;
            txtCopyNumber.Value = string.Empty;
            txtNumberOfDaysAllowed.Value = string.Empty;
            lblDeleteBookError.Text = string.Empty;
            lblAddBookError.Text = string.Empty;
            DeleteBookId.Text = string.Empty;

            // Set the default value for Number of Days Allowed
            txtNumberOfDaysAllowed.Value = "3";

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

        private void BindBookGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM bookinfo", con))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        BookGridView.DataSource = dt;
                        BookGridView.DataBind();
                    }
                }
            }
        }

        protected void BookGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            BookGridView.EditIndex = e.NewEditIndex;
            BindBookGrid();
        }

        protected void BookGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            BookGridView.EditIndex = -1;
            BindBookGrid();
        }

        protected void BookGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = BookGridView.Rows[e.RowIndex];
            string bookId = BookGridView.DataKeys[e.RowIndex].Value.ToString();
            string bookCategory = (row.FindControl("TextBoxBookCategory") as TextBox).Text.Trim();
            string bookCategoryDetail = (row.FindControl("TextBoxBookCategoryDetail") as TextBox).Text.Trim();
            string updatedBookId = (row.FindControl("TextBoxBookId") as Label).Text.Trim();
            string bookTitle = (row.FindControl("TextBoxBookTitle") as TextBox).Text.Trim();
            int copyNumber = Convert.ToInt32((row.FindControl("TextBoxCopyNumber") as TextBox).Text.Trim());
            string status = (row.FindControl("TextBoxStatus") as TextBox).Text.Trim();
            int numberOfDaysAllowed = Convert.ToInt32((row.FindControl("TextBoxNumberOfDaysAllowed") as TextBox).Text.Trim());

            string constr = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("UPDATE bookinfo SET bookcategory = @BookCategory, bookcatdetail = @BookCategoryDetail, bookid = @UpdatedBookId, booktitle = @BookTitle, copynum = @CopyNumber, status = @Status, numberofdaysallowed = @NumberOfDaysAllowed WHERE bookid = @BookId", con))
                {
                    cmd.Parameters.AddWithValue("@BookCategory", bookCategory);
                    cmd.Parameters.AddWithValue("@BookCategoryDetail", bookCategoryDetail);
                    cmd.Parameters.AddWithValue("@UpdatedBookId", updatedBookId);
                    cmd.Parameters.AddWithValue("@BookTitle", bookTitle);
                    cmd.Parameters.AddWithValue("@CopyNumber", copyNumber);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@NumberOfDaysAllowed", numberOfDaysAllowed);
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            BookGridView.EditIndex = -1;
            BindBookGrid(); // Rebind the GridView with updated data
        }


        protected void BookGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string bookId = BookGridView.DataKeys[e.RowIndex].Value.ToString();

            string constr = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM bookinfo WHERE bookid = @BookId", con))
                {
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            BindBookGrid();
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

            // Validate input
            if (string.IsNullOrWhiteSpace(bookID))
            {
                lblDeleteBookError.Text = "Please enter a valid Book ID.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Check if the book exists
                    string bookQuery = "SELECT * FROM bookinfo WHERE bookid = @BookID";
                    MySqlCommand bookCmd = new MySqlCommand(bookQuery, con);
                    bookCmd.Parameters.AddWithValue("@BookID", bookID);
                    MySqlDataReader bookReader = bookCmd.ExecuteReader();

                    if (!bookReader.HasRows)
                    {
                        lblDeleteBookError.Text = "Book not found.";
                        bookReader.Close();
                        return;
                    }

                    bookReader.Close();

                    // Check if the book has any associated transactioninfo
                    string transactionQuery = "SELECT transid, transcatdetail FROM transactioninfo WHERE bookid = @BookID";
                    MySqlCommand transactionCmd = new MySqlCommand(transactionQuery, con);
                    transactionCmd.Parameters.AddWithValue("@BookID", bookID);
                    MySqlDataReader transactionReader = transactionCmd.ExecuteReader();

                    if (transactionReader.HasRows)
                    {
                        List<string> borrowTransactionIDs = new List<string>();
                        List<string> returnTransactionIDs = new List<string>();

                        while (transactionReader.Read())
                        {
                            string transactionID = transactionReader["transid"].ToString();
                            string transactionCategory = transactionReader["transcatdetail"].ToString();

                            if (transactionCategory == "BORROW")
                            {
                                borrowTransactionIDs.Add(transactionID);
                            }
                            else if (transactionCategory == "RETURN")
                            {
                                returnTransactionIDs.Add(transactionID);
                            }
                        }

                        // Book has associated transactions, display confirmation message
                        string confirmationMessage = "This book has the following transactions:\n";

                        if (borrowTransactionIDs.Count > 0)
                        {
                            confirmationMessage += "- Borrow transaction IDs:\n";
                            foreach (string transactionID in borrowTransactionIDs)
                            {
                                confirmationMessage += $"  - {transactionID}\n";
                            }
                        }

                        if (returnTransactionIDs.Count > 0)
                        {
                            confirmationMessage += "- Return transaction IDs:\n";
                            foreach (string transactionID in returnTransactionIDs)
                            {
                                confirmationMessage += $"  - {transactionID}\n";
                            }
                        }

                        confirmationMessage += "Are you sure you want to delete it?";

                        // Hide the delete button and display the confirmation message
                        DeleteBookButton.Visible = false;
                        ConfirmDeleteBookButton.Visible = true;
                        ConfirmDeleteBookButton.Attributes["onclick"] = $"DeleteBook('{bookID}');";

                        DeleteBookConfirmation.Text = confirmationMessage;
                        BindBookGrid();
                    }


                    transactionReader.Close();
                }
                catch (Exception ex)
                {
                    lblDeleteBookError.Text = "An error occurred while deleting the book: " + ex.Message;
                }
            }

            BindBookGrid();
        }

        protected void ConfirmDeleteBookButton_Click(object sender, EventArgs e)
        {
            string bookID = DeleteBookId.Text;

            // Validate input again (optional)
            if (string.IsNullOrWhiteSpace(bookID))
            {
                lblDeleteBookError.Text = "Please enter a valid Book ID.";
                return;
            }

            DeleteBook(bookID);
        }

        private void DeleteBook(string bookID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // Delete the associated transactioninfo first
                string deleteTransactionQuery = "DELETE FROM transactioninfo WHERE bookid = @BookID";
                MySqlCommand deleteTransactionCmd = new MySqlCommand(deleteTransactionQuery, con);
                deleteTransactionCmd.Parameters.AddWithValue("@BookID", bookID);
                deleteTransactionCmd.ExecuteNonQuery();

                // Delete the book itself
                string deleteBookQuery = "DELETE FROM bookinfo WHERE bookid = @BookID";
                MySqlCommand deleteBookCmd = new MySqlCommand(deleteBookQuery, con);
                deleteBookCmd.Parameters.AddWithValue("@BookID", bookID);
                deleteBookCmd.ExecuteNonQuery();

                con.Close();
            }

            DeleteBookConfirmation.Text = "Book deleted successfully.";
            ClearInputFields();
        }



    }
}
