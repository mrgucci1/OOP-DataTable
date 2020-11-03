using System;
using System.Data;
using System.Data.SqlClient;

namespace OOP_DataTable
{
    class Program
    {
        public static void Main(string[] args)
        {
            dataTableParent table1 = new dataTableParent();
            sqlConnection sql = new sqlConnection();
            //Configuration
            //Place the SQL Information Here 
            string serverName = "Initial Server Name";
            string dataBaseName = "Inital DataBase Name";
            string tableName = "Initial Table Name";
            string username = "Initial Username";
            string password = "Initial Password";
            //Put the column numbers of any column with a date in the array below
            int[] dateColumns = { 1, 2, 3 };
            //Insert your SQL DataBase Column Headers
            string[] dataHeaders = { "ID", "Name", "Type", "Price", "Date" };
            //Create data table
            //Add your ColumnHeaders and Data types!
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("ID", typeof(string)));
            tbl.Columns.Add(new DataColumn("Vendor", typeof(string)));
            tbl.Columns.Add(new DataColumn("Ordered_On", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Ship_to_location", typeof(string)));
            tbl.Columns.Add(new DataColumn("Window_Type", typeof(string)));
            tbl.Columns.Add(new DataColumn("Window_Start", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Window_End", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Total_Cases", typeof(int)));
            tbl.Columns.Add(new DataColumn("Total_Cost", typeof(decimal)));
            tbl.Columns.Add(new DataColumn("Currency", typeof(string)));
            //Populate Table
            tbl = table1.tablePopulate(tbl, dateColumns, dataHeaders);
            //Get SQL Connection
            SqlConnection cnn = sql.connect(dataBaseName, username, password, serverName);
            //Insert Table
            table1.tableInsert(tbl, cnn, dataBaseName, tableName, dataHeaders);
        }
    }
}
