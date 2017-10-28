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
	
	public class ResultsTracker
	{
		
		//==============================================================================
		// This collection holds the results of the scan to allow them to be sorted etc.
		//------------------------------------------------------------------------------
		public ArrayList ScanResults = new ArrayList();
		public Dictionary<string, IssueGroup> IssueGroups = new Dictionary<string, IssueGroup>(); // Individual issue title with collection of associated filenames/linenumbers
		public Dictionary<string, FileGroup> FileGroups = new Dictionary<string, FileGroup>(); // Individual filename with collection of associated issues
		//==============================================================================
		
		//============================================================
		//== These variables hold data about the code being scanned ==
		//------------------------------------------------------------
		public int FileCount;
		public string TargetDirectory;
		
		// This acts as the key for each issue being added
		// It allows us to link the issue data to the listview item
		public int CurrentIndex = 0;
		
		// Total counts for the directory
		public long OverallCommentCount;
		public long OverallCodeCount;
		public long OverallWhitespaceCount;
		public long OverallLineCount;
		public long OverallFixMeCount;
		public long OverallBadFuncCount;
		
		// Individual counts for each file
		public long CommentCount;
		public long CodeCount;
		public long WhitespaceCount;
		public long LineCount;
		public long FixMeCount;
		public long BadFuncCount;
		//============================================================
		
		//=====================================================================
		//== Arrays to hold the list of files in the directory being scanned ==
		//== and any fixme/todo comments found in the files                  ==
		//---------------------------------------------------------------------
		public ArrayList FileList = new ArrayList();
		public ArrayList FileDataList = new ArrayList();
		public ArrayList FixMeList = new ArrayList();
		//=====================================================================
		
		
		public void Reset()
		{
			//Set all variables to zero ready for a new scan
			//==============================================
			
			OverallCommentCount = 0;
			OverallCodeCount = 0;
			OverallWhitespaceCount = 0;
			OverallLineCount = 0;
			OverallFixMeCount = 0;
			OverallBadFuncCount = 0;
			
			CommentCount = 0;
			CodeCount = 0;
			WhitespaceCount = 0;
			LineCount = 0;
			FixMeCount = 0;
			BadFuncCount = 0;
			
			CurrentIndex = 0;
			
			FixMeList.Clear();
			ScanResults.Clear();
			IssueGroups.Clear();
			FileGroups.Clear();
			
		}
		
		public void ResetFileCountVars()
		{
			//Set all variables from local file count to zero ready for to scan a new file
			//============================================================================
			
			CommentCount = 0;
			CodeCount = 0;
			WhitespaceCount = 0;
			LineCount = 0;
			FixMeCount = 0;
			BadFuncCount = 0;
			
		}
		
		public void ResetFileListVars()
		{
			//Set all variables from local file count to zero ready for to scan a new file
			//============================================================================
			
			FileCount = 0;
			FileList.Clear();
			FileDataList.Clear();
			
		}
		
	}
	
}
