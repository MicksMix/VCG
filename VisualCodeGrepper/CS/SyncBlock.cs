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
	
	public class SyncBlock
	{
		
		public System.Int32 BlockIndex = 0;
		public string OuterObject = "";
		public ArrayList InnerObjects = new ArrayList();
		
		public bool IsLockedBy(string InnerObject)
		{
			// Checks if an object name is in the list of locked objects
			//==========================================================
			bool blnRetVal = false;
			
			if (InnerObjects.Contains(InnerObject))
			{
				blnRetVal = true;
			}
			
			return blnRetVal;
			
		}
		
	}
	
}
