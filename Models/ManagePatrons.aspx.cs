using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace LibraryManagement.system
{
    public partial class ManagePatrons : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Add necessary code if needed
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            string name = AddName.Value;
            string course = AddCourse.Value;
            string section = AddSection.Value;
            int numberOfBooksAllowed = 3; // Default value

            // Add patron to the database
            if (AddPatron(name, course, section, numberOfBooksAllowed))
            {
                // Clear input fields
                AddName.Value = string.Empty;
                AddCourse.Value = string.Empty;
                AddSection.Value = string.Empty;

                // Show success message
                ShowMessage("Patron added successfully.");
            }
            else
            {
                // Show error message
                ShowMessage("Failed to add patron. Please try again.");
            }
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            string borrowerId = EditBorrowerId.Value;
            string name = EditName.Value;
            string course = EditCourse.Value;
            string section = EditSection.Value;
            int numberOfBooksAllowed = int.Parse(EditNumberOfBooksAllowed.Value);

            // Update patron in the database
            if (UpdatePatron(borrowerId, name, course, section, numberOfBooksAllowed))
            {
                // Clear input fields
                EditBorrowerId.Value = string.Empty;
                EditName.Value = string.Empty;
                EditCourse.Value = string.Empty;
                EditSection.Value = string.Empty;
                EditNumberOfBooksAllowed.Value = string.Empty;

                // Hide edit section
                editSection.Visible = false;

                // Show success message
                ShowMessage("Patron updated successfully.");
            }
            else
            {
                // Show error message
                ShowMessage("Failed to update patron. Please try again.");
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            string borrowerId = DeleteBorrowerId.Value;

            // Retrieve patron details from the database
            DataTable patronDetails = GetPatronDetails(borrowerId);

            if (patronDetails.Rows.Count > 0)
            {
                // Display patron details in delete section
                DeleteName.Value = patronDetails.Rows[0]["borrowerName"].ToString();
                DeleteCourse.Value = patronDetails.Rows[0]["course"].ToString();
                DeleteSection.Value = patronDetails.Rows[0]["section"].ToString();
                DeleteNumberOfBooksAllowed.Value = patronDetails.Rows[0]["numberofbooksallowed"].ToString();

                // Show delete section
                deleteSection.Visible = true;
            }
            else
            {
                // Clear patron details
                DeleteName.Value = string.Empty;
                DeleteCourse.Value = string.Empty;
                DeleteSection.Value = string.Empty;
                DeleteNumberOfBooksAllowed.Value = string.Empty;

                // Hide delete section
                deleteSection.Visible = false;

                // Show error message
                ShowMessage("Patron not found.");
            }
        }

        protected void ConfirmDeleteButton_Click(object sender, EventArgs e)
        {
            string borrowerId = DeleteBorrowerId.Value;

            // Delete patron from the database
            if (DeletePatron(borrowerId))
            {
                // Clear input fields
                DeleteBorrowerId.Value = string.Empty;
                DeleteName.Value = string.Empty;
                DeleteCourse.Value = string.Empty;
                DeleteSection.Value = string.Empty;
                DeleteNumberOfBooksAllowed.Value = string.Empty;

                // Hide delete section
                deleteSection.Visible = false;
                deleteConfirmation.Visible = false;

                // Show success message
                ShowMessage("Patron deleted successfully.");
            }
            else
            {
                // Show error message
                ShowMessage("Failed to delete patron. Please try again.");
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string borrowerId = SearchBorrowerId.Value;

            // Retrieve patron details from the database
            DataTable patronDetails = GetPatronDetails(borrowerId);

            if (patronDetails.Rows.Count > 0)
            {
                // Display patron details in search results
                SearchName.Value = patronDetails.Rows[0]["borrowerName"].ToString();
                SearchCourse.Value = patronDetails.Rows[0]["course"].ToString();
                SearchSection.Value = patronDetails.Rows[0]["section"].ToString();
                SearchNumberOfBooksAllowed.Value = patronDetails.Rows[0]["numberofbooksallowed"].ToString();

                // Show search results
                searchResults.Visible = true;
            }
            else
            {
                // Clear patron details
                SearchName.Value = string.Empty;
                SearchCourse.Value = string.Empty;
                SearchSection.Value = string.Empty;
                SearchNumberOfBooksAllowed.Value = string.Empty;

                // Hide search results
                searchResults.Visible = false;

                // Show error message
                ShowMessage("Patron not found.");
            }
        }

        private bool AddPatron(string name, string course, string section, int numberOfBooksAllowed)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO borrowerinfo (borrowerName, course, section, numberofbooksallowed) VALUES (@Name, @Course, @Section, @NumberOfBooksAllowed)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Course", course);
                        cmd.Parameters.AddWithValue("@Section", section);
                        cmd.Parameters.AddWithValue("@NumberOfBooksAllowed", numberOfBooksAllowed);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return false;
            }
        }

        private bool UpdatePatron(string borrowerId, string name, string course, string section, int numberOfBooksAllowed)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE borrowerinfo SET borrowerName = @Name, course = @Course, section = @Section, numberofbooksallowed = @NumberOfBooksAllowed WHERE borrowerid = @BorrowerId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Course", course);
                        cmd.Parameters.AddWithValue("@Section", section);
                        cmd.Parameters.AddWithValue("@NumberOfBooksAllowed", numberOfBooksAllowed);
                        cmd.Parameters.AddWithValue("@BorrowerId", borrowerId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return false;
            }
        }

        private DataTable GetPatronDetails(string borrowerId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "SELECT borrowerName, course, section, numberofbooksallowed FROM borrowerinfo WHERE borrowerid = @BorrowerId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BorrowerId", borrowerId);

                        conn.Open();
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        conn.Close();

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return null;
            }
        }

        private bool DeletePatron(string borrowerId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "DELETE FROM borrowerinfo WHERE borrowerid = @BorrowerId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BorrowerId", borrowerId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return false;
            }
        }


        private void ShowMessage(string message)
        {
            ClientScript.RegisterStartupScript(GetType(), "LibraryManagementSystem", $"alert('{message}');", true);
        }
    }
}
