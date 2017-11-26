using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Microsoft.Windows.Controls.Ribbon;
using DocScript.Scripts;

namespace DocScript
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        /// <summary>
        /// Background process to run the script file
        /// </summary>
        public BackgroundWorker ScriptWorker = new BackgroundWorker();

        #region | BackgroundWorker |

        /// <summary> 
        /// Run a Documentum script file
        ///
        /// Notes:
        /// If you need to run this for below version 6.0 Documentum you need to add idql and iapi to cboType items.
        /// Script file must be run from working directory, copy file to working directory (where iapi32.exe/idql32.exe/iapi64.exe/idql64.exe exist)
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void ScriptWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Process p = new Process();
            string workingDir = Properties.Settings.Default.Script_WorkingDirectory;
            string filePath = Properties.Settings.Default.Script_PathCodeFile;
            string script = BuildCommandLineScript();
            try
            {
                if (script == string.Empty)
                {
                    MessageBox.Show("Please check the result command line script.", "No action.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string workingFile = System.IO.Path.Combine(workingDir, System.IO.Path.GetFileName(filePath));
                    File.Copy(filePath, workingFile, true);
                    string cmdText = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\cmd.exe";
                    p.StartInfo.FileName = cmdText;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.WorkingDirectory = workingDir;
                    p.Start();
                    p.StandardInput.WriteLine(script);
                    MessageBox.Show("Script has finished." + Environment.NewLine + Environment.NewLine + "Use [F5] to refresh the result file.", "Done.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    p.Close();
                }

            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show("Script file not found." + Environment.NewLine + filePath, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
            finally
            {
                if ((p != null))
                {
                    p.Dispose();
                    p = null;
                }
            }
        }

        /// <summary>
        /// The event for when the script background process changes
        /// </summary>
        /// <example>
        /// <code lang="C#">
        /// ScriptWorker.ReportProgress(i);
        /// </code>
        /// </example>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void ScriptWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //NOTE: Can not track progress of process once it's passed to cmd
            try
            {
                //int totalRows = Data.ScriptTable.Rows.Count;
                //double currentFileNbr = e.ProgressPercentage + 1;
                //double percentChange = Math.Round((currentFileNbr / totalRows) * 100);

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }
        }

        /// <summary>
        /// The event for when the script background process finishes
        /// </summary>
        /// <example>
        /// <code lang="C#">
        ///  ScriptWorker.CancelAsync()
        /// 
        ///  if (ScriptWorker.CancellationPending) 
        ///  {
        ///    e.Cancel = true;
        ///    // exit loop here
        ///  }
        /// </code>
        /// </example>        
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void ScriptWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //NOTE: Can not track progress of process once it's passed to cmd
            try
            {
                //this.btnRunCmd.Visibility = System.Windows.Visibility.Visible;
                //this.btnCancelCmd.IsEnabled = true;
                //this.btnCancelCmd.Visibility = System.Windows.Visibility.Collapsed;
                //// <-- [update runtime here]

                //if (e.Error != null)
                //{
                //    MessageBox.Show(e.Error.Message);
                //}
                //else if (e.Cancelled)
                //{
                //    MessageBox.Show("Task cancelled!");
                //}
                //else
                //{
                //    MessageBox.Show("Task completed!");
                //}

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }
        }

        #endregion

        #region | Buttons |

        /// <summary>
        /// Copy the script to the clipboard
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnCopyCommandLine_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(BuildCommandLineScript());
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Delete the script file from the working directory
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnDeleteScriptFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string workingFile = System.IO.Path.Combine(Properties.Settings.Default.Script_WorkingDirectory, System.IO.Path.GetFileName(Properties.Settings.Default.Script_PathCodeFile));
            if (System.IO.File.Exists(workingFile))
            {
                try
                {
                    System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == System.Windows.MessageBoxResult.Yes)
                    {
                        System.IO.File.Delete(workingFile);
                        MessageBox.Show("Script has been removed from the working directory.", "Done.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (System.IO.IOException ex) // file already opened by another process.
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// Toggle full screen mode with the F11 key
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnFullScreen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (WindowState == System.Windows.WindowState.Maximized)
                {
                    WindowState = System.Windows.WindowState.Normal;
                }
                else
                {
                    WindowState = System.Windows.WindowState.Maximized;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Opens the form to edit the code type list
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnOpenFormCodeType_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Data.AppVariables.TableName = "CodeType";
                Forms.TableView formCodeType = new Forms.TableView();
                formCodeType.ShowDialog();

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }
        }
        
        /// <summary>
        /// Opens the form to edit the docbase list
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnOpenFormDocbase_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                string perserveDocbase = Properties.Settings.Default.Script_Docbase;
                Data.AppVariables.TableName = "Docbase";
                Forms.TableView formDocbase = new Forms.TableView();
                Properties.Settings.Default.Script_Docbase = perserveDocbase;
                formDocbase.ShowDialog();

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }
        }
        
        /// <summary>
        /// Open the results file
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnOpenResultsFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.Script_PathResultsFile);
            }
            catch (System.Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Open the script file
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnOpenScriptFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.Script_PathCodeFile);
            }
            catch (System.Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Open the working directory
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnOpenWorkingDirectory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (System.IO.Directory.Exists(Properties.Settings.Default.Script_WorkingDirectory))
                {
                    Process.Start(Properties.Settings.Default.Script_WorkingDirectory);
                }

            }
            catch (System.Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Refresh the results of the script in the datagrid
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnRefreshResults_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReadResultsFileToDataTable();
            UpdateStatusbarDetails();
        }

        /// <summary> 
        /// Run the script file from the working directory
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnRunCmd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Save();
                if (ErrorHandler.IsSelectionFilledIn() == true)
                {
                    if (System.IO.File.Exists(Properties.Settings.Default.Script_PathResultsFile))
                    {
                        System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("It looks like you have already ran this script.", "Run Script Again?", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question, System.Windows.MessageBoxResult.Cancel);
                        if (messageBoxResult == System.Windows.MessageBoxResult.Cancel)
                        {
                            return;
                        }
                    }
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    BuildCommandLineScript();
                    ScriptWorker.RunWorkerAsync();
                    ErrorHandler.CreateLogRecord();
                }
                else
                {
                    MessageBox.Show("Please enter all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        /// <summary>
        /// Save the results to a csv file
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnSaveResults_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                string filePath = Properties.Settings.Default.Script_PathResultsFile + ".csv";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                {
                    saveFileDialog.Filter = "CSV File(*.csv)|*.csv|All(*.*)|*";
                    saveFileDialog.FileName = filePath;
                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        filePath = saveFileDialog.FileName;
                        DataTable dtDataTable = Data.ScriptTable;
                        StreamWriter sw = new StreamWriter(filePath, false);
                        //headers  
                        for (int i = 0; i < dtDataTable.Columns.Count; i++)
                        {
                            sw.Write(dtDataTable.Columns[i]);
                            if (i < dtDataTable.Columns.Count - 1)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                        foreach (DataRow dr in dtDataTable.Rows)
                        {
                            for (int i = 0; i < dtDataTable.Columns.Count; i++)
                            {
                                if (!Convert.IsDBNull(dr[i]))
                                {
                                    string value = dr[i].ToString();
                                    if (value.Contains(','))
                                    {
                                        value = String.Format("\"{0}\"", value);
                                        sw.Write(value);
                                    }
                                    else
                                    {
                                        sw.Write(dr[i].ToString());
                                    }
                                }
                                if (i < dtDataTable.Columns.Count - 1)
                                {
                                    sw.Write(",");
                                }
                            }
                            sw.Write(sw.NewLine);
                        }
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Select a results file
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnSelectResultsFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofdScriptFile = new OpenFileDialog();
                ofdScriptFile.Filter = "Result Files|*.txt";
                ofdScriptFile.Title = "Select a results file";
                ofdScriptFile.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.Script_PathResultsFile);
                System.Windows.Forms.DialogResult result = ofdScriptFile.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Properties.Settings.Default.Script_PathResultsFile = System.IO.Path.GetFullPath(ofdScriptFile.FileName);
                }
                ReadScriptFileToDataTable();
                ReadResultsFileToDataTable();
                UpdateStatusbarDetails();

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }

        }

        /// <summary>
        /// Select a script file
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void btnSelectScriptFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofdScriptFile = new OpenFileDialog();
                ofdScriptFile.Filter = "Script Files|*.api;*.dql;*.txt";
                ofdScriptFile.Title = "Select a script file";
                ofdScriptFile.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.Script_PathCodeFile);
                System.Windows.Forms.DialogResult result = ofdScriptFile.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Properties.Settings.Default.Script_PathCodeFile = System.IO.Path.GetFullPath(ofdScriptFile.FileName);
                    UpdateResultsFileName();
                }
                ReadScriptFileToDataTable();
                ReadResultsFileToDataTable();
                UpdateStatusbarDetails();

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Select a working directory
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>>
        private void btnSelectWorkingDirectory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog fbdWorkingDirectory = new System.Windows.Forms.FolderBrowserDialog();
                fbdWorkingDirectory.Description = "Select Working Directory";
                System.Windows.Forms.DialogResult result = fbdWorkingDirectory.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Properties.Settings.Default.Script_WorkingDirectory = fbdWorkingDirectory.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        #endregion

        #region | Menu |

        /// <summary> 
        /// Opens the about form
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void mnuAbout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Forms.About f = new Forms.About();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Opens a api help file
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void mnuApiDoc_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string clickOnceLocation = AssemblyInfo.GetClickOnceLocation();
            AssemblyInfo.OpenFile(Path.Combine(clickOnceLocation, @"Documentation\\Api Help.chm"));
        }

        /// <summary>
        /// How to file
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void mnuHowTo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string clickOnceLocation = AssemblyInfo.GetClickOnceLocation();
            AssemblyInfo.OpenFile(Path.Combine(clickOnceLocation, @"Documentation\\As Built.docx"));

        }

        /// <summary>
        /// Opens the settings form
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void mnuSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Save();
                Forms.Settings formSettings = new Forms.Settings();
                formSettings.ShowDialog();

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        #endregion

        #region | Form |

        /// <summary>
        /// This is the main form initialization
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.Title = AssemblyInfo.Description;
                this.txtPassword.Password = Scripts.Security.ToInsecureString(Scripts.Security.DecryptString(Properties.Settings.Default.Script_Functional_Password));

                Data.CreateCodeTypeTable();
                string where = string.Empty;
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    string hostName = System.Net.Dns.GetHostName();
                    where = " WHERE HostName = '" + hostName + "'";
                }
                Data.CreateDocbaseTable(where);
                Data.CreateScriptTable();

                this.cboCodeTypeGalleryCategory.ItemsSource = Data.CodeTypeTable.DefaultView;
                this.cboDocbaseGalleryCategory.ItemsSource = Data.DocbaseTable.DefaultView;
                this.dgvSourceFiles.ItemsSource = Data.ScriptTable.DefaultView;

                ReadScriptFileToDataTable();
                ReadResultsFileToDataTable();
                UpdateStatusbarDetails();

                ScriptWorker.WorkerReportsProgress = true;
                ScriptWorker.WorkerSupportsCancellation = true;
                ScriptWorker.DoWork += new DoWorkEventHandler(ScriptWorker_DoWork);
                ScriptWorker.ProgressChanged += new ProgressChangedEventHandler(ScriptWorker_ProgressChanged);
                ScriptWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScriptWorker_RunWorkerCompleted);
                this.StatusBarTextCompanyName.Text = AssemblyInfo.Copyright;
                AssemblyInfo.SetAddRemoveProgramsIcon("App.ico");

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// This is the after update event for the textbox
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void BuildScriptTextbox(object sender, System.Windows.RoutedEventArgs e)
        {
            BuildCommandLineScript();
        }

        /// <summary>
        /// This is the after update event for the combobox
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void BuildScriptCombobox(object sender, EventArgs e)
        {
            UpdateResultsFileName();
            BuildCommandLineScript();
        }

        /// <summary>
        /// This sets the line numbers in the datagrid
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void dgvSourceFiles_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        /// <summary>
        /// This is run during the closing of the application
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void DocScript_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Save the password to application settings
        /// </summary>
        /// <param name="sender">Represents the control that the action is for. </param>
        /// <param name="e">Represents the data related to this event, and can be used to pass parameters. </param>
        private void txtPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Script_Functional_Password = Scripts.Security.EncryptString(Scripts.Security.ToSecureString(this.txtPassword.Password));
                Properties.Settings.Default.Save();
                BuildCommandLineScript();
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }

        }

        #endregion

        #region | Subroutines |

        /// <summary> 
        /// This builds the command line string
        /// </summary>
        /// <returns>The command line string</returns>
        public string BuildCommandLineScript()
        {
            try
            {
                string password = Scripts.Security.ToInsecureString(Scripts.Security.DecryptString(Properties.Settings.Default.Script_Functional_Password));
                switch (Properties.Settings.Default.Script_CodeType)
                {
                    case "sqlplus":
                        return Properties.Settings.Default.Script_CodeType + " " + Properties.Settings.Default.Script_Docbase + " " + Properties.Settings.Default.Script_Functional_UserName + "/" + password + "@" + Properties.Settings.Default.Script_Docbase + " @" + Properties.Settings.Default.Script_PathCodeFile;
                    default:
                        return Properties.Settings.Default.Script_CodeType + " " + Properties.Settings.Default.Script_Docbase + " -U" + Properties.Settings.Default.Script_Functional_UserName + " -P" + password + " -R" + System.IO.Path.GetFileName(Properties.Settings.Default.Script_PathCodeFile) + " >\"" + Properties.Settings.Default.Script_PathResultsFile + "\"";
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Reads the script file to the datagrid
        /// </summary>
        public void ReadScriptFileToDataTable()
        {
            try
            {
                int counter = 0;
                string line;
                Data.ScriptTable.Clear();
                if (File.Exists(Properties.Settings.Default.Script_PathCodeFile))
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(Properties.Settings.Default.Script_PathCodeFile);
                    while ((line = file.ReadLine()) != null)
                    {
                        DataRow scriptRow = Data.ScriptTable.NewRow();
                        scriptRow["CodeLine"] = line;
                        Data.ScriptTable.Rows.Add(scriptRow);
                        counter++;
                    }
                    file.Close();
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Updates the datagrid with results from the script file
        /// </summary>
        public void ReadResultsFileToDataTable()
        {
            try
            {
                int rowNum = 0;
                string line = string.Empty;

                if (File.Exists(Properties.Settings.Default.Script_PathResultsFile))
                {
                    using (FileStream fileStream = new FileStream(Properties.Settings.Default.Script_PathResultsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader file = new StreamReader(fileStream))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if (int.TryParse(line, out int rowsAffected))
                            {
                                Data.ScriptTable.Rows[rowNum]["Status"] = line;
                                rowNum += Properties.Settings.Default.Script_ResultsFileLineOffset;
                            }
                        }
                        file.Close();
                    }
                }
                else
                {
                    foreach (System.Data.DataRowView dr in dgvSourceFiles.ItemsSource)
                    {
                        dr["Status"] = null;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        /// <summary>
        /// Updates the results file name structure
        /// </summary>
        public void UpdateResultsFileName()
        {
            Properties.Settings.Default.Script_PathResultsFile = (Properties.Settings.Default.Script_PathCodeFile).Substring(0, Properties.Settings.Default.Script_PathCodeFile.Length - 4) + "_Results_" + Properties.Settings.Default.Script_Docbase + ".txt";
        }

        /// <summary>
        /// Update the details in the main form status bar
        /// </summary>
        public void UpdateStatusbarDetails()
        {
            try
            {
                DataRow[] rows;

                rows = Data.ScriptTable.Select("Status = '0'");
                double numberOfErrors = rows.Length;
                txtErrorLines.Text = numberOfErrors.ToString();

                rows = Data.ScriptTable.Select("Status IS NOT NULL");
                double numberOfResults = rows.Length;
                txtResultLines.Text = numberOfResults.ToString();

                rows = Data.ScriptTable.Select();
                double numberOfScript = rows.Length / Properties.Settings.Default.Script_ResultsFileLineOffset;
                txtScriptLines.Text = numberOfScript.ToString();

                double percentChange = 0;
                if(numberOfScript == 0)
                {
                    percentChange = 0;
                }
                else
                {
                    percentChange = Math.Round((numberOfResults / numberOfScript) * 100);
                }
                this.barBatchProgress.Value = percentChange;

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
            }
        }

        #endregion

    }
}
