using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace LibraryManagement.system.Models
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBookGrid();
            }
        }

        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            string bookCategory = txtBookCategory.Value.Trim();
            string bookCategoryDetail = txtBookCategoryDetail.Value.Trim();
            string bookTitle = txtBookTitle.Value.Trim();
            int copyNum = Convert.ToInt32(txtCopyNum.Value.Trim());
            int numberOfDaysAllowed = Convert.ToInt32(txtNumberOfDaysAllowed.Value.Trim());

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO bookinfo (bookcategory, bookcatdetail, bookid, booktitle, copynum, numberofdaysallowed) " +
                               "VALUES (@BookCategory, @BookCategoryDetail, @BookID, @BookTitle, @CopyNum, @NumberOfDaysAllowed)";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BookCategory", bookCategory);
                    cmd.Parameters.AddWithValue("@BookCategoryDetail", bookCategoryDetail);
                    cmd.Parameters.AddWithValue("@BookID", GenerateBookID(bookCategory, copyNum));
                    cmd.Parameters.AddWithValue("@BookTitle", bookTitle);
                    cmd.Parameters.AddWithValue("@CopyNum", copyNum);
                    cmd.Parameters.AddWithValue("@NumberOfDaysAllowed", numberOfDaysAllowed);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            ClearAddBookForm();
            BindBookGrid();
        }

        protected void SearchBookButton_Click(object sender, EventArgs e)
        {
            string searchBookName = SearchBook.Text.Trim();
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM bookinfo WHERE booktitle LIKE @SearchBookName";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SearchBookName", "%" + searchBookName + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    SearchBookGridView.DataSource = dt;
                    SearchBookGridView.DataBind();

                    SearchBookResults.Text = "Total books found: " + dt.Rows.Count;
                }
            }
        }

        protected void DeleteBookButton_Click(object sender, EventArgs e)
        {
            string deleteBookID = DeleteBookId.Text.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM bookinfo WHERE bookid = @DeleteBookID";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DeleteBookID", deleteBookID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            ClearDeleteBookForm();
            BindBookGrid();
        }

        private void BindBookGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM bookinfo";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    BookGridView.DataSource = dt;
                    BookGridView.DataBind();
                }
            }
        }

        private string GenerateBookID(string bookCategory, int copyNum)
        {
            // Example: BC01-001
            string categoryCode = bookCategory.Substring(0, 2).ToUpper();
            string copyNumFormatted = copyNum.ToString().PadLeft(3, '0');
            return categoryCode + "-" + copyNumFormatted;
        }


        private void ClearAddBookForm()
        {
            txtBookCategory.Value = string.Empty;
            txtBookCategoryDetail.Value = string.Empty;
            txtBookTitle.Value = string.Empty;
            txtCopyNum.Value = string.Empty;
            txtNumberOfDaysAllowed.Value = string.Empty;
        }

        private void ClearDeleteBookForm()
        {
            DeleteBookId.Text = string.Empty;
        }

        protected void gridViewBooks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridViewBooks.EditIndex = e.NewEditIndex;

            // Disable validation controls
            rfvBookCategory.Enabled = false;
            rfvBookCategoryDetail.Enabled = false;
            rfvBookTitle.Enabled = false;
            rfvCopyNumber.Enabled = false;
            rfvDaysAllowed.Enabled = false;

            BindGrid();
        }

        protected void gridViewBooks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow row = gridViewBooks.Rows[rowIndex];

            string bookID = gridViewBooks.DataKeys[rowIndex].Values["BookID"].ToString();
            string bookCategory = (row.FindControl("txtEditBookCategory") as TextBox).Text;
            string bookCategoryDetail = (row.FindControl("txtEditBookCategoryDetail") as TextBox).Text;
            string bookTitle = (row.FindControl("txtEditBookTitle") as TextBox).Text;
            string copyNumber = (row.FindControl("txtEditCopyNumber") as TextBox).Text;
            string daysAllowed = (row.FindControl("txtEditDaysAllowed") as TextBox).Text;

            // Connection string for connecting to your MySQL database
            string connectionString = "server=localhost;port=3306;database=lib_management_schema;uid=username;password=password;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // SQL update statement
                string query = "UPDATE bookinfo SET bookcategory = @Category, bookcatdetail = @CategoryDetail, " +
                               "booktitle = @Title, copynum = @CopyNumber, numberofdaysallowed = @DaysAllowed " +
                               "WHERE bookid = @BookID";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters to the SQL query
                    command.Parameters.AddWithValue("@Category", bookCategory);
                    command.Parameters.AddWithValue("@CategoryDetail", bookCategoryDetail);
                    command.Parameters.AddWithValue("@Title", bookTitle);
                    command.Parameters.AddWithValue("@CopyNumber", copyNumber);
                    command.Parameters.AddWithValue("@DaysAllowed", daysAllowed);
                    command.Parameters.AddWithValue("@BookID", bookID);

                    // Execute the SQL query
                    command.ExecuteNonQuery();
                }
            }

            gridViewBooks.EditIndex = -1;
            BindGrid();
        }

        protected void gridViewBooks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridViewBooks.EditIndex = -1;

            // Enable validation controls
            rfvBookCategory.Enabled = true;
            rfvBookCategoryDetail.Enabled = true;
            rfvBookTitle.Enabled = true;
            rfvCopyNumber.Enabled = true;
            rfvDaysAllowed.Enabled = true;

            BindGrid();
        }


        protected void BookGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string bookid = BookGridView.DataKeys[e.RowIndex].Value.ToString();
            // Delete the book with the corresponding bookid

            BindBookGrid();
        }

    }
}
