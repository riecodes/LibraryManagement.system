using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace LibraryManagement.system.Models
{
    public partial class ManagePatrons : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPatronData();
            }
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;

        protected void AddPatronButton_Click(object sender, EventArgs e)
        {
            string patronName = AddPatronName.Text;
            string patronCourse = AddPatronCourse.Text;
            string patronSection = AddPatronSection.Text;

            // Check if any of the required fields is empty
            if (string.IsNullOrEmpty(patronName) || string.IsNullOrEmpty(patronCourse) || string.IsNullOrEmpty(patronSection))
            {
                AddPatronConfirmation.Text = "Please enter all required fields.";
                AddPatronConfirmation.CssClass = "error-message";
                return;
            }

            // Generate a unique borrowerid value
            string borrowerId = GenerateBorrowerId(patronName, patronCourse, patronSection);

            // Add the patron to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO borrowerinfo (borrowerid, borrowerName, course, section) VALUES (@BorrowerId, @Name, @Course, @Section)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    command.Parameters.AddWithValue("@Name", patronName);
                    command.Parameters.AddWithValue("@Course", patronCourse);
                    command.Parameters.AddWithValue("@Section", patronSection);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Reset the input fields
                        AddPatronName.Text = string.Empty;
                        AddPatronCourse.Text = string.Empty;
                        AddPatronSection.Text = string.Empty;

                        // Display a success message
                        AddPatronConfirmation.Text = "Patron added successfully. Your Borrower ID: <strong>" + borrowerId + "</strong>";
                        AddPatronConfirmation.CssClass = "success-message";
                        LoadPatronData();
                    }
                    else
                    {
                        // Display an error message
                        AddPatronConfirmation.Text = "Failed to add patron";
                        AddPatronConfirmation.CssClass = "error-message";
                    }
                }
            }
        }

        // Helper method to generate a unique borrowerid value
        private string GenerateBorrowerId(string name, string course, string section)
        {
            string initials = GetInitials(name);
            string date = GetDate();
            string courseCode = GetCourseCode(course);
            string sectionCode = GetSectionCode(section);

            string borrowerId = initials + date + courseCode + sectionCode;

            return borrowerId;
        }


        private string GetInitials(string name)
        {
            // Split the name into individual words
            string[] words = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Get the first character of each word
            string initials = "";
            foreach (string word in words)
            {
                initials += char.ToUpper(word[0]);
            }

            return initials;
        }

        private string GetCourseCode(string course)
        {
            // Generate a course code based on the course name or abbreviation
            // Implement your own logic here to generate the code
            // For example, you can use a dictionary to map course names to codes

            // Assuming the course name is abbreviated, you can simply return a shortened version
            return course.Substring(0, Math.Min(course.Length, 3));
        }

        private string GetSectionCode(string section)
        {
            // Generate a section code based on the section name or abbreviation
            // Implement your own logic here to generate the code
            // For example, you can use a dictionary to map section names to codes

            // Assuming the section name is abbreviated, you can simply return it
            return section;
        }

        private string GetDate()
        {
            // Get the current date in the format: YYYYMMDD
            return DateTime.Now.ToString("yyyyMMdd");
        }

        protected void SearchPatronButton_Click(object sender, EventArgs e)
        {
            string patronName = SearchPatronName.Text.Trim();

            // Add validation and error handling if needed

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM borrowerinfo WHERE borrowerName LIKE @Name";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", "%" + patronName + "%");

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                // Patrons found, display the search results
                                SearchPatronGridView.DataSource = reader;
                                SearchPatronGridView.DataBind();

                                // Hide the "No patrons found" message, if previously shown
                                SearchPatronResults.Visible = false;
                            }
                            else
                            {
                                // No results found, display the message
                                SearchPatronResults.Text = "No patrons found";
                                SearchPatronResults.Visible = true;

                                // Clear the GridView
                                SearchPatronGridView.DataSource = null;
                                SearchPatronGridView.DataBind();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Display an error message
                        SearchPatronResults.Text = "Failed to search patron: " + ex.Message;
                        SearchPatronResults.Visible = true;
                    }
                }
            }
        }

        protected void EditPatronGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            EditPatronGridView.EditIndex = e.NewEditIndex;
            LoadPatronData();
        }

        protected void EditPatronGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            EditPatronGridView.EditIndex = -1;
            LoadPatronData();
        }

        protected void EditPatronGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            string borrowerId = EditPatronGridView.DataKeys[rowIndex].Values["borrowerid"].ToString();
            TextBox txtPatronName = EditPatronGridView.Rows[rowIndex].FindControl("txtPatronName") as TextBox;
            TextBox txtPatronCourse = EditPatronGridView.Rows[rowIndex].FindControl("txtPatronCourse") as TextBox;
            TextBox txtPatronSection = EditPatronGridView.Rows[rowIndex].FindControl("txtPatronSection") as TextBox;

            string connectionString = "LibraryManagementSystemConnectionString";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE borrowerinfo SET borrowerName = @Name, course = @Course, section = @Section WHERE borrowerid = @BorrowerID";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", txtPatronName.Text);
                    command.Parameters.AddWithValue("@Course", txtPatronCourse.Text);
                    command.Parameters.AddWithValue("@Section", txtPatronSection.Text);
                    command.Parameters.AddWithValue("@BorrowerID", borrowerId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Update successful
                        lblEditBookError.Text = "Patron information updated successfully.";
                    }
                    else
                    {
                        // Update failed
                        lblEditBookError.Text = "Failed to update patron information.";
                    }
                }
            }

            EditPatronGridView.EditIndex = -1;
            LoadPatronData();
        }

        protected void LoadPatronData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM borrowerinfo";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        EditPatronGridView.DataSource = dt;
                        EditPatronGridView.DataBind();
                    }
                }
            }
        }

        protected void DeletePatronButton_Click(object sender, EventArgs e)
        {
            string patronId = DeletePatronId.Text;

            // Add validation and error handling if needed

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM borrowerinfo WHERE borrowerid = @PatronId";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PatronId", patronId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Display a success message
                            DeletePatronConfirmation.Text = "Patron deleted successfully";
                        }
                        else
                        {
                            // Display an error message
                            DeletePatronConfirmation.Text = "Failed to delete patron";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Display an error message
                        DeletePatronConfirmation.Text = "Failed to delete patron: " + ex.Message;
                    }
                }
            }
        }
    }
}
