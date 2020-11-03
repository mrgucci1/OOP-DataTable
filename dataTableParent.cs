using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_DataTable
{
    public class dataTableParent : excelMaster
    {
        //Max length
        const int maxLength = 200;
        //Populate Datatable with data from .csv/.xlsx file
        public DataTable tablePopulate(DataTable tbl, int[] dateColumns, string[] dataHeaders)
        {
            //Find PO Excel Doc, insert into DataTable
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            //Filter for file type .csv and .xlsx  
            files = filterNoName(files, ".csv", ".xlsx");
            if(files.Length < 1)
            {
                Console.WriteLine("No files Found, Press any key to exit...");
                Console.ReadKey();
                System.Environment.Exit(1);
            }
            int sheet = 1;
            object[,] excelValues = excelMasterStart(files[0], sheet);
            int startingNum = 2;
            int success = 0;
            excelValues = filterExcel(excelValues, startingNum);
            //Timer for Conversion Process
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Console.WriteLine($"Processing PO Data Excel file with {excelValues.GetLength(0)} rows...");
            for (int i = startingNum; i < excelValues.GetLength(0) - 1; i++)
            {
                DataRow dr = tbl.NewRow();
                //Try to convert date fields to datetime objects
                for (int index = 0; index < dateColumns.Length; index++) {
                    try { excelValues[i, dateColumns[index]] = Convert.ToDateTime(excelValues[i, dateColumns[index]].ToString()); }
                    catch { }
                }
                //Populate DataRow with excelValue objects
                for (int index = 1; index < dataHeaders.Length + 1; index++)
                {
                    try { dr[dataHeaders[index - 1]] = excelValues[i, index]; }
                    catch(Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Failed when adding data to DataTable\nFailed on row: {index}\nAttempted to add {excelValues[i, index]} to column named {dataHeaders[index-1]}");
                        Console.WriteLine($"This column is of type: {dr[dataHeaders[index - 1]].GetType().ToString()}\nError Message: \n{e.ToString().Substring(0, maxLength)}");
                        Console.ResetColor();
                        Console.WriteLine("Press any Key to exit...");
                        Console.ReadKey();
                        System.Environment.Exit(1);
                    }
                }
                tbl.Rows.Add(dr);
                success++;
                //Every 10,000 rows, report to user information about process
                if (success % 10000 == 0)
                {
                    if (success != 0)
                    {
                        decimal percent = Convert.ToDecimal(success) / Convert.ToDecimal(excelValues.GetLength(0));
                        decimal final = percent * 100;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Proccessing PO Data...\nJUST PASSED ROW: {success}\nElapsed Time: {timer.Elapsed}\nPercent Complete = {final.ToString("0.##")}%\n");
                        Console.ResetColor();
                    }
                }
            }
            timer.Stop();
            return tbl;
        }
        public void tableInsert(DataTable tbl, SqlConnection cnn, string database, string tableName, string[] dataHeaders)
        {
            SqlBulkCopy objBulk = new SqlBulkCopy(cnn);
            objBulk.DestinationTableName = tableName;
            //Map DataTable Headers to SQL Database Headers
            for (int i = 0; i < dataHeaders.Length; i++)
            {
                objBulk.ColumnMappings.Add(dataHeaders[i], dataHeaders[i]);
            }
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Console.WriteLine("Inserting....");
            objBulk.WriteToServer(tbl);
            cnn.Close();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Completed insertion into {database} PO table\nElapsed time: {timer.Elapsed}");
            Console.ResetColor();
            timer.Stop();
        }

    }
}
