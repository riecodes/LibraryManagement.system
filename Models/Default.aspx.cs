using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;

namespace LibraryManagement.system.Models
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            string bookCategory = txtBookCategory.Value;
            string bookCategoryDetail = txtBookCategoryDetail.Value;
            string bookTitle = txtBookTitle.Value;
            int copyNumber = Convert.ToInt32(txtCopyNum.Value);
            int daysAllowed = Convert.ToInt32(txtNumberOfDaysAllowed.Value);

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO bookinfo (bookcategory, bookcatdetail, bookid, booktitle, copynum, numberofdaysallowed, status) " +
                               "VALUES (@BookCategory, @BookCategoryDetail, @BookID, @BookTitle, @CopyNumber, @DaysAllowed, 'IN')";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BookCategory", bookCategory);
                cmd.Parameters.AddWithValue("@BookCategoryDetail", bookCategoryDetail);
                cmd.Parameters.AddWithValue("@BookID", Guid.NewGuid().ToString("N"));
                cmd.Parameters.AddWithValue("@BookTitle", bookTitle);
                cmd.Parameters.AddWithValue("@CopyNumber", copyNumber);
                cmd.Parameters.AddWithValue("@DaysAllowed", daysAllowed);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            ClearFields();
            BindGrid();
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
            string bookID = BookGridView.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = BookGridView.Rows[e.RowIndex];
            TextBox txtBookCategory = (TextBox)row.FindControl("txtBookCategory");
            TextBox txtBookCategoryDetail = (TextBox)row.FindControl("txtBookCategoryDetail");
            TextBox txtBookTitle = (TextBox)row.FindControl("txtBookTitle");
            TextBox txtCopyNumber = (TextBox)row.FindControl("txtCopyNumber");
            TextBox txtDaysAllowed = (TextBox)row.FindControl("txtDaysAllowed");

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "UPDATE bookinfo SET bookcategory = @BookCategory, bookcatdetail = @BookCategoryDetail, " +
                               "booktitle = @BookTitle, copynum = @CopyNumber, numberofdaysallowed = @DaysAllowed " +
                               "WHERE bookid = @BookID";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BookCategory", txtBookCategory.Text);
                cmd.Parameters.AddWithValue("@BookCategoryDetail", txtBookCategoryDetail.Text);
                cmd.Parameters.AddWithValue("@BookTitle", txtBookTitle.Text);
                cmd.Parameters.AddWithValue("@CopyNumber", Convert.ToInt32(txtCopyNumber.Text));
                cmd.Parameters.AddWithValue("@DaysAllowed", Convert.ToInt32(txtDaysAllowed.Text));
                cmd.Parameters.AddWithValue("@BookID", bookID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BookGridView.EditIndex = -1;
            BindGrid();
        }

        protected void BookGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string bookID = BookGridView.DataKeys[e.RowIndex].Value.ToString();

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM bookinfo WHERE bookid = @BookID";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BookID", bookID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindGrid();
        }

        private void ClearFields()
        {
            txtBookCategory.Value = "";
            txtBookCategoryDetail.Value = "";
            txtBookTitle.Value = "";
            txtCopyNum.Value = "";
            txtNumberOfDaysAllowed.Value = "";
        }

        protected void SearchBookButton_Click(object sender, EventArgs e)
        {
            string searchQuery = SearchBook.Text;

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

            BindGrid();
        }
    }
}
