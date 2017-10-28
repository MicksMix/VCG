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
	
	public class AppSettings
	{
		public AppSettings()
		{
			// VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
			BadFuncFile = "cppfunctions.conf";
			ListItemColour = Color.LawnGreen;
			
		}
		
		//=============================================
		//== Constants to identify the language type ==
		//---------------------------------------------
		public const int C = 0;
		public const int JAVA = 1;
		public const int SQL = 2;
		public const int CSHARP = 3;
		public const int VB = 4;
		public const int PHP = 5;
		public const int COBOL = 6;
		//=============================================
		
		
		//==========================================
		//== Identifiers for RTB results grouping ==
		//------------------------------------------
		public const int INDIVIDUAL = 0;
		public const int ISSUEGROUP = 1;
		public const int FILEGROUP = 2;
		//==========================================
		
		
		//====================================================================
		//== Config files to hold dangerous function names and descriptions ==
		//--------------------------------------------------------------------
		public string BadCommentFile = "badcomments.conf";
		public string CConfFile = "cppfunctions.conf";
		public string JavaConfFile = "javafunctions.conf";
		public string PLSQLConfFile = "plsqlfunctions.conf";
		public string CSharpConfFile = "csfunctions.conf";
		public string VBConfFile = "vbfunctions.conf";
		public string PHPConfFile = "phpfunctions.conf";
		public string COBOLConfFile = "cobolfunctions.conf";
		//====================================================================
		
		
		//==============================================
		//== Standard file suffixes for each language ==
		//----------------------------------------------
		public bool IsAllFileTypes = false;
		
		public string CSuffixes = ".cpp|.hpp|.c|.h";
		public string JavaSuffixes = ".java|.jsp|.jspf|web.xml|config.xml";
		public string PLSQLSuffixes = ".pls|.pkb|.pks";
		public string CSharpSuffixes = ".cs|.asp|.aspx|web.config";
		public string VBSuffixes = ".vb|.asp|.aspx|web.config";
		public string PHPSuffixes = ".php|.php3|php.ini";
		public string COBOLSuffixes = ".cob|.cbl|.clt|.cl2|.cics";
		
		public Array FileSuffixes;
		public int NumSuffixes = 0;
		//==============================================
		
		
		//===================================================================
		//== Initialise arrays at start - these hold bad stuff to look for ==
		//-------------------------------------------------------------------
		// Comments indicating untrusted/unfinished code
		//Public BadComments As Array = {"fixme", "fix me", "todo" & vbNewLine, "to do" & vbNewLine, "todo ", "todo:", "to do ", "wtf", "???", "hardcoded", "hard coded", "removeme", "dangerous method", "fixthis", "fix this", "crap", "shit", "bodge", "kludge", "kluge", "dunno", "assume"}
		public ArrayList BadComments = new ArrayList();
		//Public CommentArraySize As Integer = 21
		
		// File and array of bad functions
		public ArrayList BadFunctions = new ArrayList();
		public string BadFuncFile; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
		//===================================================================
		
		
		//===================================================
		//== Start with C/C++ as the default language type ==
		//---------------------------------------------------
		public int TestType = C;
		public int StartType = C;
		public string SingleLineComment = "//";
		public string AltSingleLineComment = ""; // Some languages (VB, PHP...) have more than one single line comment indicator, e.g. VB allows REM or '
		public string BlockStartComment = "/*";
		public string BlockEndComment = "*/";
		//===================================================
		
		
		//=========================
		//== Output file details ==
		//-------------------------
		public bool IsOutputFile = false;
		public string OutputFile = "";
		public bool IsXmlOutputFile = false;
		public string XmlOutputFile = "";
		public bool IsCsvOutputFile = false;
		public string CsvOutputFile = "";
		//=========================
		
		//=========================
		//== Input file details ==
		//-------------------------
		public bool IsXmlInputFile = false;
		public string XmlInputFile = "";
		public bool IsCsvInputFile = false;
		public string CsvInputFile = "";
		//=========================
		
		//==================================
		//== settings for optional checks ==
		//----------------------------------
		public bool IsFinalizeCheck = true; // Check for finalization of Java classes as per OWASP recommendations
		public bool IsInnerClassCheck = true; // Check for nesting of Java classes as per OWASP recommendations
		public bool IsConfigOnly = false; // Do we want to limit code checks to file content only, or do a full test
		public bool IsAndroid = false; // Include any Java checks for Android issues
		//==================================
		
		//=========================================================
		//== Include beta functionality if requested by the user ==
		//---------------------------------------------------------
		public bool IncludeBeta = false;
		public bool IncludeSigned = false;
		public bool IncludeCobol = false;
		//=========================================================
		
		
		//== The current colour for items which have been checked in the results list ==
		public Color ListItemColour; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
		
		//== Show visual breakdown when finished? (set by user options) ==
		public bool VisualBreakdownEnabled = false;
		public bool DisplayBreakdownOption = true;
		
		//== Report output level - show this level and above ==
		public int OutputLevel = CodeIssue.POSSIBLY_SAFE;
		
		//== Settings for Rich Text results  display ==
		public int RTBGrouping = INDIVIDUAL;
		
		//== Settings for console ouput ==
		public bool IsConsole = false;
		public bool IsVerbose = false;
		
		//== Settings for Temporary Grep
		public string TempGrepText = "";
		
	}
	
}
