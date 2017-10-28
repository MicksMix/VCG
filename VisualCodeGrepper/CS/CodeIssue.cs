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
	
	public class CodeIssue
	{
		// Used to store the name of a dangerous function along with any associated
		// description and rationale that will be provided to the user
		//
		// N.B. - a dictionary was not feasible as the description will not always be present
		//       and this more flexible approach allows for severity levels, etc.
		//===================================================================================
		
		//=================================================
		//== Constants to mark the severity of the issue ==
		//-------------------------------------------------
		public const int CRITICAL = 1;
		public const int HIGH = 2;
		public const int MEDIUM = 3;
		public const int STANDARD = 4;
		public const int LOW = 5;
		public const int INFO = 6;
		public const int POSSIBLY_SAFE = 7;
		//=================================================
		
		public string FunctionName;
		public string Description;
		
		public int Severity = STANDARD;
		
	}
	
}
