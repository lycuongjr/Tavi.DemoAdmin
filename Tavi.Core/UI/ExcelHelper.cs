using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Collections;
namespace Tavi.Core.UI
{
    public class ExcelHelper
    {
        #region Private Members

        /// <summary>
        /// Connection string of excel sheet 2003
        /// </summary>
        string _xlsConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0";
        /// <summary>
        /// Connection string of excel sheet 2007 or Above
        /// </summary>
        string _xlsxConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0";
        /// <summary>
        /// Current Connection String
        /// </summary>
        string _connString = string.Empty;
        #endregion


        #region Constructors

        /// <summary>
        /// Default Constructor.s
        /// ducla
        /// </summary>
        public ExcelHelper()
        {
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Nghinv
        /// </summary>
        /// <param name="ExcelSheetPath"></param>
        private void SetConnectionString(string ExcelSheetPath)
        {
            int count = ExcelSheetPath.Split('.').Count();
            string ext = ExcelSheetPath.Split('.')[count - 1];
            _connString = string.Format(_xlsConString, ExcelSheetPath);
            if (ext.ToLower().Equals("xlsx"))
                _connString = string.Format(_xlsxConString, ExcelSheetPath);
        }

        /// <summary>
        /// Get Names of all Sheets in Excel 
        /// </summary>
        /// <param name="ExcelSheetPath">Excel Sheet file Path ex.C:\Excel.xlsx</param>
        /// <returns>Data Table contain Name of All sheets</returns>
        public DataTable GetAllSheetsName(string ExcelSheetPath)
        {
            // Create the connection object
            SetConnectionString(ExcelSheetPath);
            OleDbConnection oledbConn = new OleDbConnection(_connString);
            try
            {

                // Open connection
                oledbConn.Open();

                // Create OleDbCommand object and select data from worksheet Sheet1

                DataTable dt = oledbConn.GetSchema("Tables");
                DataTable bindDT = new DataTable();
                DataColumn dc = new DataColumn("Sheet Name");
                dc.ColumnName = "SheetName";
                bindDT.Columns.Add(dc);
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drow = bindDT.NewRow();
                    drow[dc] = dr["TABLE_NAME"].ToString();
                    bindDT.Rows.Add(drow);
                }

                // return the data to the data table
                return bindDT;


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                // Close connection
                oledbConn.Close();
            }

        }

        /// <summary>
        /// Fill All Sheets into hash table key of it is sheet name and value is sheet data represented in data table 
        /// </summary>
        /// <param name="ExcelSheetPath">Excel Sheet file Path ex.C:\Excel.xlsx</param>
        /// <returns>hash table contain sheet name and data of it</returns>
        public Hashtable FillAllDataFromExcelSheet(string ExcelSheetPath)
        {
            Hashtable toReturn = new Hashtable();
            string tableNametoFill = string.Empty;
            string selectCommand = string.Empty;
            // Create the connection object
            SetConnectionString(ExcelSheetPath);
            OleDbConnection connection = new OleDbConnection(_connString);
            try
            {

                DataTable dt = connection.GetSchema("Tables");
                foreach (DataRow row in dt.Rows)
                {
                    tableNametoFill = row["TABLE_NAME"].ToString();
                    selectCommand = string.Format("SELECT * FROM [{0}]", tableNametoFill.Trim());
                    OleDbCommand cmd = new OleDbCommand(selectCommand, connection);

                    // Create new OleDbDataAdapter
                    OleDbDataAdapter oleda = new OleDbDataAdapter();

                    oleda.SelectCommand = cmd;

                    // Create a DataSet which will hold the data extracted from the worksheet.
                    DataTable dtData = new DataTable();
                    oleda.Fill(dtData);
                    toReturn.Add(tableNametoFill, dtData);
                }
                return toReturn;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                connection.Close();

            }
            return toReturn;
        }

        /// <summary>
        /// Fill Sheet into DataTable 
        /// </summary>
        /// <param name="ExcelSheetPath">Excel Sheet file Path ex.C:\Excel.xlsx<</param>
        /// <param name="SheetName">Name of Sheet in Excel</param>
        /// <returns></returns>
        public DataTable FillDataFromExcelSheet(string ExcelSheetPath, string SheetName)
        {
            // Create the connection object
            SetConnectionString(ExcelSheetPath);
            OleDbConnection connection = new OleDbConnection(_connString);
            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                string select = string.Format("SELECT * FROM [{0}]", SheetName.Trim());

                OleDbCommand command = new OleDbCommand(select, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                dt.Clear();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                connection.Close();

            }
            return new DataTable();
        }

        /// <summary>
        /// Fill Sheet into DataTable 
        /// </summary>
        /// <param name="ExcelSheetPath">Excel Sheet file Path ex.C:\Excel.xlsx<</param>
        /// <param name="SheetName"> Sheet Index in Excel</param>
        /// <returns></returns>
        public DataTable FillDataFromExcelSheet(string ExcelSheetPath, int SheetIndex)
        {
            // Create the connection object
            SetConnectionString(ExcelSheetPath);
            OleDbConnection connection = new OleDbConnection(_connString);
            try
            {
                connection.Open();
                DataTable dtSchema = connection.GetSchema("Tables");
                if (SheetIndex > dtSchema.Rows.Count - 1)
                    throw new Exception("Index out of Range");

                string tableNamex = dtSchema.Rows[SheetIndex]["TABLE_NAME"].ToString();

                DataTable dt = new DataTable();
                string select = string.Format("SELECT * FROM [{0}]", tableNamex.Trim());

                OleDbCommand command = new OleDbCommand(select, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                dt.Clear();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                connection.Close();

            }
            return new DataTable();
        }


        /// <summary>
        /// Validate Excel Sheet to match your structure
        /// </summary>
        /// <param name="ExcelSheetPath">Excel Sheet file Path ex.C:\Excel.xlsx</param>
        /// <param name="SheetIndex">Sheet Index in Excel</param>
        /// <param name="Columns">Array of column name to validate excel. must be ordered as appeared in Excel Column </param>
        /// <returns>is valid</returns>
        public Boolean ValidateExcelSchema(string ExcelSheetPath, int SheetIndex, params string[] Columns)
        {

            DataTable dtToValidate = null;
            dtToValidate = FillDataFromExcelSheet(ExcelSheetPath, SheetIndex);
            return ValidateDataTableSchema(dtToValidate, Columns);
        }

        /// <summary>
        /// Validate Excel Sheet to match your structure
        /// </summary>
        /// <param name="ExcelSheetPath">Excel Sheet file Path ex.C:\Excel.xlsx</param>
        /// <param name="SheetName">Sheet Index in Excel</param>
        /// <param name="Columns">Array of column name to validate excel. must be ordered as appeared in Excel Column  </param>
        /// <returns></returns>
        public Boolean ValidateExcelSchema(string ExcelSheetPath, string SheetName, params string[] Columns)
        {

            DataTable dtToValidate = null;
            dtToValidate = FillDataFromExcelSheet(ExcelSheetPath, SheetName);
            return ValidateDataTableSchema(dtToValidate, Columns);
        }

        private bool ValidateDataTableSchema(DataTable dtToValidate, params string[] Columns)
        {
            bool toReturn = false;
            if (dtToValidate != null)
            {
                int index = 0;
                foreach (string column in Columns)
                {
                    if (dtToValidate.Columns.Contains(column) && dtToValidate.Columns[column].Ordinal == index)
                    {
                        toReturn = true;
                        index++;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            return toReturn;
        }

        #endregion
    }
}
