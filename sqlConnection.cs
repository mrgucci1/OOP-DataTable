
using System.Data.SqlClient;
namespace OOP_DataTable
{
    class sqlConnection
    {
        public SqlConnection connect(string databaseName, string username, string password, string serverName)
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = $@"Data Source={serverName};Initial Catalog={databaseName};User ID={username};Password={password}";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            return cnn;
        }
    }
}
