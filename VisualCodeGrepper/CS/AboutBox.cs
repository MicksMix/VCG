// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports


namespace VisualCodeGrepper
{
	public sealed partial class AboutBox
	{
		public AboutBox()
		{
			InitializeComponent();
		}
		
		public void AboutBox_Load(System.Object sender, System.EventArgs e)
		{
			// Set the title of the form.
			string ApplicationTitle = "";
			if ((new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Title != "")
			{
				ApplicationTitle = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Title;
			}
			else
			{
				ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension((new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.AssemblyName);
			}
			this.Text = string.Format("About {0}", ApplicationTitle);
			// Initialize all of the text displayed on the About Box.
			// TODO: Customize the application's assembly information in the "Application" pane of the project
			//    properties dialog (under the "Project" menu).
			this.LabelProductName.Text = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName;
			this.LabelVersion.Text = string.Format("Version {0}", (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Version.ToString());
			this.LabelCopyright.Text = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Copyright;
			this.LabelCompanyName.Text = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.CompanyName;
			this.TextBoxDescription.Text = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Description;
		}
		
		public void OKButton_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		
	}
	
}
