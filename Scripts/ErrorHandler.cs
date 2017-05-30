using System;
using System.Windows.Forms;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

// <summary> 
// This namespaces if for generic application classes
// </summary>
namespace DocScript.Scripts
{
    /// <summary> 
    /// Used to handle exceptions
    /// </summary>
    public class ErrorHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ErrorHandler));

        /// <summary>
        /// Create a log record to track which methods are being used.
        /// </summary>
        public static void CreateLogRecord()
        {
            try
            {
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
                System.Reflection.MethodBase caller = sf.GetMethod();
                string currentProcedure = (caller.Name).Trim();
                log.Info("[PROCEDURE]=|" + currentProcedure + "|[USER NAME]=|" + Environment.UserName + "|[MACHINE NAME]=|" + Environment.MachineName + "|[FILE NAME]=|" + Properties.Settings.Default.Script_PathCodeFile);
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }
        }

        /// <summary> 
        /// Used to produce an error message and create a log record
        /// <example>
        /// <code lang="C#">
        /// ErrorHandler.DisplayMessage(ex, True);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="ex">Represents errors that occur during application execution.</param>
        public static void DisplayMessage(Exception ex)
        {
            System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
            System.Reflection.MethodBase caller = sf.GetMethod();
            string currentProcedure = (caller.Name).Trim();
            string currentFileName = Properties.Settings.Default.Script_PathCodeFile;
            string errorMessageDescription = ex.ToString();
            errorMessageDescription = System.Text.RegularExpressions.Regex.Replace(errorMessageDescription, @"\r\n+", " "); //the carriage returns were messing up my log file
            string msg = "Contact your system administrator. A record has been created in the log file." + Environment.NewLine;
            msg += "Procedure: " + currentProcedure + Environment.NewLine;
            msg += "Description: " + ex.ToString() + Environment.NewLine;
            log.Error("[PROCEDURE]=|" + currentProcedure + "|[USER NAME]=|" + Environment.UserName + "|[MACHINE NAME]=|" + Environment.MachineName + "|[FILE NAME]=|" + currentFileName + "|[DESCRIPTION]=|" + errorMessageDescription);
            MessageBox.Show(msg, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary> 
        /// Check if all the values have been filled out
        /// </summary>
        /// <returns>Has all the values been filled out to run the script?</returns>
        public static Boolean IsSelectionFilledIn()
        {
            try
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.Script_CodeType) || string.IsNullOrEmpty(Properties.Settings.Default.Script_Docbase) || string.IsNullOrEmpty(Properties.Settings.Default.Script_Functional_UserName) || string.IsNullOrEmpty(Properties.Settings.Default.Script_Functional_Password) || string.IsNullOrEmpty(Properties.Settings.Default.Script_PathCodeFile) || string.IsNullOrEmpty(Properties.Settings.Default.Script_PathResultsFile))
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);
                return false;
            }
        }

    }
}