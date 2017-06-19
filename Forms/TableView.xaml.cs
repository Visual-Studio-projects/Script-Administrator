using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlServerCe;
using DocScript.Scripts;

namespace DocScript.Forms
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : Window
    {

		/// <summary>
		/// 
		/// </summary>
        public TableView()
        {
            try
            {
                InitializeComponent();
                string tableName = Data.AppVariables.TableName;
                this.Title = "List of " + tableName;
                switch (tableName)
                {
                    case "CodeType":
                        dgvTableView.DataContext = Data.CodeTypeTable.DefaultView;
                        break;
                    case "Docbase":
                        Data.CreateDocbaseTable();
                        dgvTableView.DataContext = Data.DocbaseTable.DefaultView;
                        break;
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }

        }

        private void dgvTableView_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        private void dgvTableView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = dgvTableView;
                if (dataGrid == null) return;

                var firstColumn = dataGrid.Columns.FirstOrDefault();
                if (firstColumn != null)
                    firstColumn.SortDirection = System.ComponentModel.ListSortDirection.Ascending;

                var lastColumn = dataGrid.Columns.LastOrDefault();
                if (lastColumn != null)
                    lastColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

                // Autofit all other columns
                foreach (var column in dataGrid.Columns)
                {
                    if (column == lastColumn) break;

                    double beforeWidth = column.ActualWidth;
                    column.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
                    double sizeCellsWidth = column.ActualWidth;
                    column.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToHeader);
                    double sizeHeaderWidth = column.ActualWidth;
                    column.MinWidth = Math.Max(beforeWidth, Math.Max(sizeCellsWidth, sizeHeaderWidth));
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                string tableName = Data.AppVariables.TableName;
                string sql = "SELECT * FROM " + tableName;
                string where = string.Empty;
                SqlCeConnection cn = new SqlCeConnection(Data.Connection());
                SqlCeCommandBuilder scb = default(SqlCeCommandBuilder);
                SqlCeDataAdapter sda = new SqlCeDataAdapter(sql, cn);
                sda.TableMappings.Add("Table", tableName);
                scb = new SqlCeCommandBuilder(sda);
                switch (tableName)
                {
                    case "CodeType":
                        string codetype = Properties.Settings.Default.Script_CodeType;
                        sda.Update(Data.CodeTypeTable.Select(null, null, DataViewRowState.Deleted));
                        sda.Update(Data.CodeTypeTable.Select(null, null, DataViewRowState.ModifiedCurrent));
                        sda.Update(Data.CodeTypeTable.Select(null, null, DataViewRowState.Added));
                        Data.CreateCodeTypeTable();
                        Properties.Settings.Default.Script_CodeType = codetype;
                        break;
                    case "Docbase":
                        string docbase = Properties.Settings.Default.Script_Docbase;
                        sda.Update(Data.DocbaseTable.Select(null, null, DataViewRowState.Deleted));
                        sda.Update(Data.DocbaseTable.Select(null, null, DataViewRowState.ModifiedCurrent));
                        sda.Update(Data.DocbaseTable.Select(null, null, DataViewRowState.Added));
                        if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                        {
                            string hostName = System.Net.Dns.GetHostName();
                            where = " WHERE HostName = '" + hostName + "'";
                        }
                        Data.CreateDocbaseTable(where);
                        Properties.Settings.Default.Script_Docbase = docbase;
                        break;
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }
        }

    }
}
