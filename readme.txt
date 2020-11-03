A baseplate program built in OOP style to insert data from a CSV or XLSX file into a SQL database
To use, open Program.cs and follow the comments to add your specfic information. Compile, and then place your csv/xlsx file in the same directory as the .exe

Notes: 
To compile, you must have the Excel Object Libary reference (from right clicking "references" in Visual Studio and looking in the COM section).
It will read the excel doc in a 2D object array. 
It inserts data using SQL bulk copy.
Some error checking is present, will also report status of conversion if file is greater than 10,000 rows.