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
	// VisualCodeGrepper - Code security scanner
	// Copyright (C) 2012-2014 Nick Dunn and John Murray
	//
	// This program is free software: you can redistribute it and/or modify
	// it under the terms of the GNU General Public License as published by
	// the Free Software Foundation, either version 3 of the License, or
	// (at your option) any later version.
	//
	// This program is distributed in the hope that it will be useful,
	// but WITHOUT ANY WARRANTY; without even the implied warranty of
	// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	// GNU General Public License for more details.
	//
	// You should have received a copy of the GNU General Public License
	// along with this program.  If not, see <http://www.gnu.org/licenses/>.
	
	public class ScanResult
	{
		public ScanResult()
		{
			// VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
			CheckColour = Color.LawnGreen;
			
		}
		// Hold details from code scan to facilitate ordered lists etc.
		//=============================================================
		
		
		public string Title = "";
		public string Description = "";
		public string FileName = "";
		public int LineNumber = 0;
		public string CodeLine = "";
		public int ItemKey = 1;
		
		private string strSeverityDescription = "";
		private int intSeverity = CodeIssue.STANDARD;
		
		//== Has the item been marked by the user in the ListView? ==
		public bool IsChecked = false;
		public Color CheckColour; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
		
		
		public void SetSeverity(int Level)
		{
			// Set severity level and its associated description
			//==================================================
			
			intSeverity = Level;
			
			switch (Level)
			{
				case CodeIssue.CRITICAL:
					strSeverityDescription = "Critical";
					break;
				case CodeIssue.HIGH:
					strSeverityDescription = "High";
					break;
				case CodeIssue.MEDIUM:
					strSeverityDescription = "Medium";
					break;
				case CodeIssue.LOW:
					strSeverityDescription = "Low";
					break;
				case CodeIssue.INFO:
					strSeverityDescription = "Suspicious Comment";
					break;
				case CodeIssue.POSSIBLY_SAFE:
					strSeverityDescription = "Potential Issue";
					break;
				default:
					strSeverityDescription = "Standard";
					intSeverity = CodeIssue.STANDARD;
					break;
			}
			
		}
		
		public int Severity()
		{
			return intSeverity;
		}
		
		public string SeverityDesc()
		{
			return strSeverityDescription;
		}
		
	}
	
}
