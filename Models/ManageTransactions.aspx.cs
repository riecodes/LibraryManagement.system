using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

public partial class ManageTransaction : System.Web.UI.Page
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
}
