using System;
using System.IO;
using System.Data;
using System.Data.SqlServerCe;
using System.Deployment.Application;
using System.Linq;

namespace DocScript.Scripts
{
    class Data
    {

        /// <summary>
        /// Used for values across different classes
        /// </summary>
        public static class AppVariables
        {

            /// <summary>
            /// variable used for the table name used to populate a datagrid
            /// </summary>
            public static string TableName { get; set; }

        }

        /// <summary>
        /// Relative database connection string
        /// </summary>
        /// <returns>the data source of the database</returns>
        public static string Connection()
        {
            string dataFolder = "App_Data";
            string versionNumber = string.Empty;
            string userFilePath = string.Empty;
            string pathDeploy = Properties.Settings.Default.App_PathDeploy;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                versionNumber = string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
                versionNumber = "_" + versionNumber.Replace(".", "_");

                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                userFilePath = Path.Combine(localAppData, AssemblyInfo.Copyright.Replace(" ", "_"), AssemblyInfo.Product, dataFolder);

                if (!Directory.Exists(userFilePath)) Directory.CreateDirectory(userFilePath);

                string sourceFilePath = Path.Combine(pathDeploy, "Application Files", AssemblyInfo.Product + versionNumber, dataFolder, AssemblyInfo.Product + ".sdf.deploy");
                string destFilePath = Path.Combine(userFilePath, AssemblyInfo.Product + ".sdf");
                if (!File.Exists(destFilePath)) File.Copy(sourceFilePath, destFilePath);
            }
            else
            {
                userFilePath = System.IO.Path.Combine(DocScript.Scripts.AssemblyInfo.GetClickOnceLocation(), dataFolder);
            }
            Properties.Settings.Default.App_PathUserData = userFilePath;
            string databaseFile = "Data Source=" + Path.Combine(userFilePath, AssemblyInfo.Product + ".sdf");
            return databaseFile;
        }

        /// <summary>
        /// List of code types
        /// </summary>
        public static DataTable CodeTypeTable = new DataTable();

        /// <summary>
        /// List of docbase names
        /// </summary>
        public static DataTable DocbaseTable = new DataTable();

        /// <summary>
        /// Lines of code
        /// </summary>
        public static DataTable ScriptTable = new DataTable();

        /// <summary>
        /// Creates the datatable for the script type list from a comma delimited settings string
        /// </summary>
        public static void CreateCodeTypeTable()
        {
            try
            {
                string sql = "SELECT * FROM CodeType";
                CodeTypeTable.Rows.Clear();
                using (var da = new SqlCeDataAdapter(sql, Connection()))
                {
                    da.Fill(CodeTypeTable);
                }
                CodeTypeTable.DefaultView.Sort = CodeTypeTable.Columns[0].ColumnName + " ASC";

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }

        }

        /// <summary>
        /// Creates the datatable for the docbase list from a comma delimited settings string
        /// </summary>
        /// <param name="where">the where clause to load the datatable</param>
        public static void CreateDocbaseTable(string where = "")
        {
            try
            {
                string sql = "SELECT * FROM Docbase" + where;
                DocbaseTable.Rows.Clear();
                using (var da = new SqlCeDataAdapter(sql, Connection()))
                {
                    da.Fill(DocbaseTable);
                }
                DocbaseTable.DefaultView.Sort = DocbaseTable.Columns[0].ColumnName + " ASC";

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }

        }

        /// <summary>
        /// Creates the datagrid structure for the script file
        /// </summary>
        public static void CreateScriptTable()
        {
            try
            {
                dynamic dcStatus = new DataColumn("Status", typeof(string));
                dynamic dcCodeLine = new DataColumn("CodeLine", typeof(string));
                ScriptTable.Columns.Add(dcStatus);
                ScriptTable.Columns.Add(dcCodeLine);

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }

        }

    }
}
