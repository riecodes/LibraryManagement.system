using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryManagement.system
{
    public partial class AddPatron : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            // Validate user input
            if (string.IsNullOrEmpty(BorrowerNameTextBox.Text))
            {
                // Display error message if borrower name is empty
                ErrorMessageLabel.Text = "Please enter a borrower name.";
                return;
            }

            // Validate course
            if (string.IsNullOrEmpty(CourseTextBox.Text))
            {
                ErrorMessageLabel.Text = "Please enter a course.";
                return;
            }

            // Validate section
            if (string.IsNullOrEmpty(SectionTextBox.Text))
            {
                ErrorMessageLabel.Text = "Please enter a section.";
                return;
            }

            // Additional validation checks
            if (!IsValidCourse(CourseTextBox.Text))
            {
                ErrorMessageLabel.Text = "Invalid course format. Please enter a valid course.";
                return;
            }

            if (!IsValidSection(SectionTextBox.Text))
            {
                ErrorMessageLabel.Text = "Invalid section format. Please enter a valid section.";
                return;
            }

            // Create new row in borrowerinfo table
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Generate a new borrowerid value
                    string borrowerId = GenerateBorrowerId(connection);

                    string query = "INSERT INTO borrowerinfo (borrowerid, borrowerName, course, section) VALUES (@borrowerid, @borrowerName, @course, @section)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@borrowerid", borrowerId);
                        command.Parameters.AddWithValue("@borrowerName", BorrowerNameTextBox.Text);
                        command.Parameters.AddWithValue("@course", CourseTextBox.Text);
                        command.Parameters.AddWithValue("@section", SectionTextBox.Text);

                        command.ExecuteNonQuery();
                    }

                    // Display confirmation message
                    SuccessMessageLabel.Text = "New borrower added successfully!";
                }
                catch (MySqlException ex)
                {
                    // Handle database-related exceptions
                    ErrorMessageLabel.Text = "An error occurred while adding the borrower: " + ex.Message;
                }
            }
        }

        private string GenerateBorrowerId(MySqlConnection connection)
        {
            string borrowerId = string.Empty;

            // Generate a new borrowerid value based on the existing borrowers in the database
            string query = "SELECT MAX(CAST(SUBSTRING(borrowerid, 6) AS UNSIGNED)) FROM borrowerinfo";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int lastBorrowerId = Convert.ToInt32(result);
                    int newBorrowerId = lastBorrowerId + 1;
                    borrowerId = "2023-" + newBorrowerId.ToString("D3");
                }
                else
                {
                    borrowerId = "2023-001";
                }
            }

            return borrowerId;
        }

        private bool IsValidCourse(string course)
        {
            // Perform custom validation for the course format
            // For example, check if it consists of two letters followed by three digits
            return Regex.IsMatch(course, "^[A-Z]{2}\\d{3}$");
        }

        private bool IsValidSection(string section)
        {
            // Perform custom validation for the section format
            // For example, check if it consists of three alphanumeric characters
            return Regex.IsMatch(section, "^[A-Za-z0-9]{3}$");
        }

    }
}