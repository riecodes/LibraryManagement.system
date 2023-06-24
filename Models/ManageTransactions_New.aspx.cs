using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace LibraryManagement.system.Models
{
    public partial class ManageTransactions_New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTransactionData();
            }
        }

        protected void LoadTransactionData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM transactioninfo";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        TransactionGridView.DataSource = dt;
                        TransactionGridView.DataBind();
                    }
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            // Get the reference to the button that raised the event
            Button deleteButton = (Button)sender;

            // Find the corresponding GridView row
            GridViewRow row = (GridViewRow)deleteButton.NamingContainer;

            // Get the transaction ID from the row
            string transactionId = TransactionGridView.DataKeys[row.RowIndex].Value.ToString();

            // Perform the necessary actions to delete the transaction record using the transactionId
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystemConnectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Delete the transaction record from the transactioninfo table
                using (MySqlCommand deleteCommand = new MySqlCommand("DELETE FROM transactioninfo WHERE transid = @transactionId", connection))
                {
                    deleteCommand.Parameters.AddWithValue("@transactionId", transactionId);
                    deleteCommand.ExecuteNonQuery();
                }
            }

            // Reload the transaction data
            LoadTransactionData();
        }

    }
}
