using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

            // TODO: Add additional validation checks as needed

            // Create new row in borrowerinfo table
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString))
            {
                connection.Open();

                string query = "INSERT INTO borrowerinfo (borrowerid, borrowerName, course, section) VALUES (@borrowerid, @borrowerName, @course, @section)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // TODO: Generate a new borrowerid value
                    command.Parameters.AddWithValue("@borrowerid", "2023-001");
                    command.Parameters.AddWithValue("@borrowerName", BorrowerNameTextBox.Text);
                    command.Parameters.AddWithValue("@course", CourseTextBox.Text);
                    command.Parameters.AddWithValue("@section", SectionTextBox.Text);

                    command.ExecuteNonQuery();
                }
            }

            // Display confirmation message
            SuccessMessageLabel.Text = "New borrower added successfully!";
        }

    }
}