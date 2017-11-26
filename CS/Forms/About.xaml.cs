using System;
using System.Linq;
using System.Windows;

namespace DocScript.Forms
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        /// <summary>
        /// About for startup
        /// </summary>
        public About()
        {
            InitializeComponent();
            this.Title = "About the " + DocScript.Scripts.AssemblyInfo.Title + " " + DocScript.Scripts.AssemblyInfo.AssemblyVersion;
            this.txtTitle.Text = DocScript.Scripts.AssemblyInfo.Title;
            this.txtDescription.Text = DocScript.Scripts.AssemblyInfo.Description;
            this.txtVersion.Text = DocScript.Scripts.AssemblyInfo.AssemblyVersion;
            this.txtAppPath.Text = DocScript.Scripts.AssemblyInfo.FilePath;
            this.txtEmail.Inlines.Add(Properties.Settings.Default.App_HelpEmail);
            this.txtAuthor.Text = Properties.Settings.Default.App_Author;
            this.txtCopyright.Text = DocScript.Scripts.AssemblyInfo.Copyright;
            this.txtWarning.Text = "This computer program is protected by copyright law and international treaties.  Unauthorized reproduction or distribution of this program, or any portion of it, may result in severe civil and criminal penalties, and will be prosecuted to the maximum extent possible under law.";
            this.StatusBarTextCompanyName.Text = DocScript.Scripts.AssemblyInfo.Copyright;
        }

        /// <summary> 
        /// Creates an email to the help desk
        /// </summary>
        private void TxtEmailClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = "mailto:" + Properties.Settings.Default.App_HelpEmail;
                string product = DocScript.Scripts.AssemblyInfo.Title.Replace("&", "&&");
                msg += "?subject=" + product + " " + DocScript.Scripts.AssemblyInfo.AssemblyVersion;
                msg += "&body=Please create a ticket for user " + Environment.UserName + " ";
                msg += "on machine " + Environment.MachineName + " ";
                System.Diagnostics.Process.Start(msg);

            }
            catch (Exception ex)
            {
                Scripts.ErrorHandler.DisplayMessage(ex);

            }
        }

    }
}
