using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;


namespace ExamTable.Controllers.Utils
{
    public class ExportExcelUtil
    {
        public void exportExcel(string path, DataTable table)
        {
            //Creae an Excel application instance
            Excel.Application excel = new Excel.Application();

            //Create an Excel workbook instance and open it from the predefined location
            Excel.Workbook excelWorkBook = excel.Workbooks.Add(System.Reflection.Missing.Value);

            //Add a new worksheet to workbook with the Datatable name
            Excel.Worksheet excelWorkSheet = excelWorkBook.ActiveSheet;
            excelWorkSheet.Name = table.TableName;

            for (int i = 1; i < table.Columns.Count + 1; i++)
            {
                excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
            }

            for (int j = 0; j < table.Rows.Count; j++)
            {
                for (int k = 0; k < table.Columns.Count; k++)
                {
                    excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                }
            }
            
            excelWorkBook.SaveAs(path);
            excelWorkBook.Close();
            Marshal.ReleaseComObject(excelWorkBook);

            excel.Quit();
            Marshal.FinalReleaseComObject(excel);
        }
        
    }
}