using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using DocScript.Scripts;

// <summary> 
// This namespaces if for forms relating to this project
// </summary>
namespace DocScript.Forms
{
    /// <summary> 
    /// Class for the settings form
    /// </summary>
    public partial class Settings : Form
    {
        /// <summary> 
        /// The settings form
        /// </summary>
        public Settings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form closing event method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Save();

            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }

        }

        /// <summary>
        /// Form load event method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSettings_Load(object sender, EventArgs e)
        {
            try
            {
                LoadSettings();
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }
        }

        /// <summary> 
        /// Load settings to the property grid
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                this.pgdSettings.SelectedObject = Properties.Settings.Default;
                SetFormIcon();
                this.Text = "Settings for " + DocScript.Scripts.AssemblyInfo.Title.Replace("&", "&&") + " " + DocScript.Scripts.AssemblyInfo.AssemblyVersion;
                SetLabelColumnWidth(this.pgdSettings, 200);
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }

        }

        /// <summary> 
        /// Set form icon based resources
        /// </summary>
        public void SetFormIcon()
        {
            try
            {
                Bitmap b = Properties.Resources.Settings;
                IntPtr pIcon = b.GetHicon();
                Icon i = Icon.FromHandle(pIcon);
                this.Icon = i;
            }
            catch (Exception ex)
            {
                ErrorHandler.DisplayMessage(ex);

            }

        }

        /// <summary> 
        /// Sets the column width of a property grid 
        /// </summary>
        /// <param name="grid">Represents the property grid object. </param>
        /// <param name="width">Represents the width of the column. </param>
        public static void SetLabelColumnWidth(PropertyGrid grid, int width)
        {
            if (grid == null)
                return;

            FieldInfo fi = grid.GetType().GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                return;

            Control view = fi.GetValue(grid) as Control;
            if (view == null)
                return;

            MethodInfo mi = view.GetType().GetMethod("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic);
            if (mi == null)
                return;
            mi.Invoke(view, new object[] { width });
        }

    }
}
