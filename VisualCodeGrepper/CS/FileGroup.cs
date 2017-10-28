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
	
	public class FileGroup
	{
		// Hold details from code scan to facilitate ordered lists etc.
		// Unlike ScanResult this will group filenames and line numbers
		// together under a single issue with the same title.
		//=============================================================
		
		
		//==================================================
		//== Constants to array position of details issue ==
		//--------------------------------------------------
		public const int SEVERITY = 0;
		public const int ISSUE = 1;
		public const int DESC = 2;
		public const int LINE = 3;
		public const int CODE = 4;
		//==================================================
		
		public string FileName = ""; // This will be unique for each issue and will serve as an identifier
		private string strDescription = "";
		
		private Dictionary<int, string[]> arrDetails = new Dictionary<int, string[]>();
		private Dictionary<int, int> arrSeverities = new Dictionary<int, int>();
		
		
		private string SetSeverity(int Level)
		{
			// Set severity level and its associated description
			//==================================================
			string strSeverityDescription = "";
			
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
					break;
			}
			
			return strSeverityDescription;
			
		}
		
		public void AddDetail(int Key, int Severity, string IssueTitle, string Description, int LineNumber = 0, string CodeLine = "")
		{
			// Add the issue, severity and line number for each individual occurence of an issue.
			// This will be held in a dictionary of all issue details, where the key is the
			// the same as the key of the individual ScanResult to assist when deleting items.
			//================================================================================
			string strSeverityDescription = "";
			
			
			if (!arrDetails.ContainsKey(Key))
			{
				strSeverityDescription = SetSeverity(Severity);
				arrSeverities.Add(Key, Severity);
				arrDetails.Add(Key, new[] {strSeverityDescription, IssueTitle, Description, LineNumber.ToString(), CodeLine});
			}
			
		}
		
		public void DeleteDetail(int Key)
		{
			// Remove the individual issue from the collection
			//================================================
			
			if (arrDetails.ContainsKey(Key))
			{
				arrDetails.Remove(Key);
			}
			
		}
		
		public string[] GetDetail(int Key)
		{
			//Return the individual details for a given issue number
			//======================================================
			string[] strRetVal = null;
			
			
			if (arrDetails.ContainsKey(Key))
			{
				strRetVal = arrDetails[Key];
			}
			else
			{
				strRetVal = new[] {};
			}
			
			return strRetVal;
			
		}
		
		public Dictionary<int, string[]> GetDetails()
		{
			//Return the the complete set of details for each occurence in this issue
			//=======================================================================
			
			return arrDetails;
			
		}
		
		public int GetItemCount()
		{
			return arrDetails.Count;
		}
		
		public int GetSeverity(int Key)
		{
			return arrSeverities[Key];
		}
		
	}
	
}
