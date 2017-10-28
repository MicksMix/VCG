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

using System.Text.RegularExpressions;

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



namespace VisualCodeGrepper
{
	sealed class modJavaCheck
	{
		
		// Specific checks for Java code
		//==============================
		
		public static void CheckJavaCode(string CodeLine, string FileName)
		{
			// Carry out any specific checks for the language in question
			//===========================================================
			
			// Is there a struts validator or similar framework in place
			if (modMain.ctCodeTracker.HasValidator == false && FileName.ToLower().EndsWith(".xml") && CodeLine.ToLower().Contains("<plug-in") && CodeLine.ToLower().Contains(".validator"))
			{
				modMain.ctCodeTracker.HasValidator = true;
			}
			
			CheckServlet(CodeLine, FileName); // Check for any issues specific to servlets
			CheckSQLiValidation(CodeLine, FileName); // Check for potential SQLi
			CheckXSSValidation(CodeLine, FileName); // Check for potential XSS
			CheckRunTime(CodeLine, FileName); // Check use of java.lang.Runtime.exec
			CheckIsHttps(CodeLine, FileName); // Check any URLs being used are not http
			CheckClone(CodeLine, FileName); // Check for unsafe cloning implementation
			CheckSerialize(CodeLine, FileName); // Check for unsafe serialization implementation
			IdentifyServlets(CodeLine); // List any servlet instantiations for the thread checks
			CheckModifiers(CodeLine, FileName); // Check for public variables in classes
			CheckThreadIssues(CodeLine, FileName); // Check for good/bad thread management
			CheckUnsafeTempFiles(CodeLine, FileName); // Check for temp files with obvious names
			CheckPrivileged(CodeLine, FileName); // Check for potential user access to code with system privileges
			CheckRequestDispatcher(CodeLine, FileName); // Check for user control of request dispatcher
			CheckXXEExpansion(CodeLine, FileName); // Check for safe/unsafe XML expansion
			CheckOverflow(CodeLine, FileName); // Check use of primitive types and any operations upon them
			CheckResourceRelease(CodeLine, FileName); // Are file resources safely handled in try ... catch blocks
			
			// Identify any nested classes (if required by user)
			if (modMain.asAppSettings.IsInnerClassCheck)
			{
				CheckInnerClasses(CodeLine, FileName);
			}
			
			// Carry out any Android-specific checks
			if (modMain.asAppSettings.IsAndroid == true)
			{
				CheckAndroidStaticCrypto(CodeLine, FileName);
				CheckAndroidIntent(CodeLine, FileName);
			}
			
		}
		
		private static void CheckServlet(string CodeLine, string FileName)
		{
			// Determine whether or not the module contains code for a servlet
			// Check for any problems specific to servlets if necessary
			//================================================================
			string[] arrFragments = null;
			string strServletName = "";
			
			
			//== Determine whether this is a servlet in order to check for servlet-specific problems ==
			if (modMain.ctCodeTracker.IsServlet == false && (CodeLine.Contains("extends HttpServlet") || CodeLine.Contains("implements Servlet")))
			{
				modMain.ctCodeTracker.IsServlet = true;
				
				//== Store servlet name for thread-saftey checks ==
				arrFragments = Regex.Split(CodeLine, "(extends\\s+HttpServlet|implements\\s+Servlet)");
				strServletName = modMain.GetLastItem(arrFragments.First());
				
				if (string.IsNullOrEmpty(strServletName))
				{
					return;
				}
				
				modMain.ctCodeTracker.ServletName = strServletName;
				if (!modMain.ctCodeTracker.ServletNames.Contains(strServletName))
				{
					modMain.ctCodeTracker.ServletNames.Add(strServletName);
				}
				
			}
			
			//== Check for Thread.sleep() in servlet ==
			if (modMain.ctCodeTracker.IsServlet == true && CodeLine.Contains("Thread.sleep"))
			{
				frmMain.Default.ListCodeIssue("Use of Thread.sleep() within a Java servlet", "The use of Thread.sleep() within Java servlets is discouraged", FileName);
			}
			
			//== List any getter and setter methods for the servlet's instance variables so that we can check these are handled in a thread-safe manner ==
			if (modMain.ctCodeTracker.IsServlet == true)
			{
				IdentifyGetAndSet(CodeLine);
			}
			
		}
		
		private static void CheckSQLiValidation(string CodeLine, string FileName)
		{
			// Check for any SQL injection problems
			//=====================================
			string strVarName = ""; // Holds the variable name for the dynamic SQL statement
			
			
			//== Only check unvalidated code ==
			if (modMain.ctCodeTracker.HasValidator == true)
			{
				return;
			}
			
			
			//== Is unsanitised dynamic SQL statement prepared beforehand? ==
			if (CodeLine.Contains("=") && (CodeLine.ToLower().Contains("sql") || CodeLine.ToLower().Contains("query")) && (CodeLine.Contains("\"") && CodeLine.Contains("+")))
			{
				//== Extract variable name from assignment statement ==
				strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
				modMain.ctCodeTracker.HasVulnSQLString = true;
				if (!modMain.ctCodeTracker.SQLStatements.Contains(strVarName) && (!(strVarName.Contains("(") || strVarName.Contains(")") || strVarName.Contains("[") || strVarName.Contains("]") || strVarName.Contains(" ") || strVarName.Contains("+") || strVarName.Contains("*"))))
				{
					modMain.ctCodeTracker.SQLStatements.Add(strVarName);
				}
			}
			
			
			if (Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise"))
			{
				
				//== Remove any variables which have been sanitised from the list of vulnerable variables ==
				if (modMain.ctCodeTracker.SQLStatements.Count > 0)
				{
					foreach (var strVar in modMain.ctCodeTracker.SQLStatements)
					{
						
						if (Regex.IsMatch(CodeLine, strVar + "\\s*\\=\\s*\\S*validate|encode|sanitize|sanitise\\S*\\(" + System.Convert.ToString(strVar)))
						{
							modMain.ctCodeTracker.SQLStatements.Remove(strVar);
							break;
						}
					}
				}
			}
			else if (Regex.IsMatch(CodeLine, "\\S*\\.(prepareStatement|executeQuery|query|queryForObject|queryForList|queryForInt|queryForMap|update|getQueryString|executeQuery|createNativeQuery|createQuery)\\s*\\("))
			{
				
				//== Check usage of java.sql.Statement.executeQuery, etc. ==
				if (CodeLine.Contains("\"") && CodeLine.Contains("+"))
				{
					//== Dynamic SQL built into connection/update ==
					frmMain.Default.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via dynamic SQL statements. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.CRITICAL, CodeLine);
				}
				else if (modMain.ctCodeTracker.HasVulnSQLString == true)
				{
					//== Otherwise check for use of pre-prepared statements ==
					foreach (var strVar in modMain.ctCodeTracker.SQLStatements)
					{
						if (CodeLine.Contains(System.Convert.ToString(strVar)))
						{
							frmMain.Default.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via a pre-prepared dynamic SQL statement. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.CRITICAL, CodeLine);
							break;
						}
					}
				}
			}
			
		}
		
		private static void CheckXSSValidation(string CodeLine, string FileName)
		{
			// Check for any XSS problems
			//===========================
			
			string[] arrFragments = null; // Holds the parts of any line containing HttpRequest var assignments
			string strVarName = ""; // Holds the variable name assigned to any HttpRequest data
			
			
			//== Only check unvalidated code ==
			if (modMain.ctCodeTracker.HasValidator == true)
			{
				return;
			}
			
			
			//== Identify any HttpRequest variables in servlets ==
			if (CodeLine.Contains("HttpServletRequest ") && !CodeLine.Contains("import "))
			{
				
				arrFragments = Regex.Split(CodeLine, "HttpServletRequest ");
				strVarName = System.Convert.ToString(arrFragments.Last().Trim());
				
				if (!string.IsNullOrEmpty(strVarName))
				{
					//== Variable name should be immediately followed by either whitespace, a comma, an equals sign or a semi-colon
					arrFragments = Regex.Split(strVarName, "[,;=\\s+]");
					strVarName = System.Convert.ToString(arrFragments.First().Trim());
					modMain.ctCodeTracker.HasHttpRequestData = true;
					modMain.ctCodeTracker.HttpRequestVar = strVarName;
				}
				
			}
			else if (FileName.ToLower().EndsWith(".jsp") && Regex.IsMatch(CodeLine, "\\s*\\S*\\s*={1}?\\s*\\S*\\s*\\brequest\\b\\.\\bgetParameter\\b"))
			{
				
				//== Identify any GET parameters assigned to variables ==
				strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine, true));
				
				// Add variable to dictionary if it's alphanumeric (we have not accidentally caught an expression)
				modMain.ctCodeTracker.HasGetVariables = true;
				if (Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.SQLStatements.Contains(strVarName))
				{
					modMain.ctCodeTracker.GetVariables.Add(strVarName);
				}
				
			}
			else if (FileName.ToLower().EndsWith(".jsp") && Regex.IsMatch(CodeLine, "\\<\\%\\=\\s*\\w+\\.getParameter\\s*\\("))
			{
				frmMain.Default.ListCodeIssue("Potential XSS", "The application appears to reflect a HTTP request parameter to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine);
				
			}
			else if (FileName.ToLower().EndsWith(".jsp") && modMain.ctCodeTracker.GetVariables.Count > 0 && (CodeLine.ToLower().Contains("validate") || CodeLine.ToLower().Contains("encode") || CodeLine.ToLower().Contains("sanitize") || CodeLine.ToLower().Contains("sanitise")))
			{
				
				//== Check for sanitisation of any GET/POST params and remove from dictionary ==
				if (modMain.ctCodeTracker.GetVariables.Count > 0)
				{
					foreach (var strVar in modMain.ctCodeTracker.GetVariables)
					{
						if (!(strVar.contains("(") || strVar.contains(")") || strVar.contains("[") || strVar.contains("]") || strVar.contains(" ") || strVar.contains("+") || strVar.contains("*")))
						{
							if (Regex.IsMatch(CodeLine, strVar + "\\s*\\=\\s*\\S*validate|encode|sanitize|sanitise\\S*\\(" + System.Convert.ToString(strVar)))
							{
								modMain.ctCodeTracker.GetVariables.Remove(strVar);
								break;
							}
						}
					}
				}
				
			}
			else if (modMain.ctCodeTracker.HasHttpRequestData == true && CodeLine.Contains(modMain.ctCodeTracker.HttpRequestVar))
			{
				
				//== If sanitisation takes place reset all HttpRequest variables ==
				if (CodeLine.ToLower().Contains("validate") || CodeLine.ToLower().Contains("encode") || CodeLine.ToLower().Contains("sanitize") || CodeLine.ToLower().Contains("sanitise"))
				{
					modMain.ctCodeTracker.HasHttpRequestData = false;
					modMain.ctCodeTracker.HttpRequestVar = "";
				}
				else if (CodeLine.Contains("getCookies") || CodeLine.Contains("getHeader") || CodeLine.Contains("getPart") || CodeLine.Contains("getQueryString") || CodeLine.Contains("getParameter") || CodeLine.Contains("getRequestUR"))
				{
					
					//== If this point has been reached then variables probably used without sanitisation ==
					if (FileName.ToLower().EndsWith(".jsp"))
					{
						frmMain.Default.ListCodeIssue("Potential XSS", "The application appears to use data contained in the HttpServletRequest without validation or sanitisation. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine);
					}
					else
					{
						frmMain.Default.ListCodeIssue("Poor Input Validation", "The application appears to use data contained in the HttpServletRequest without validation or sanitisation. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine);
					}
					
				}
				
			}
			else if (FileName.ToLower().EndsWith(".jsp") && Regex.IsMatch(CodeLine, "<%=\\s*\\S*\\bsession\\b\\.\\bgetAttribute\\b\\s*\\("))
			{
				//== Check JSPs for session variables being reflected back to page ==
				frmMain.Default.ListCodeIssue("Potential XSS", "The JSP displays a session variable directly to the screen. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine);
				
			}
			else if (FileName.ToLower().EndsWith(".jsp") && modMain.ctCodeTracker.GetVariables.Count > 0 && CodeLine.Contains("<%="))
			{
				
				//== Check for get params being reflected to page without sanitisation ==
				foreach (var strVar in modMain.ctCodeTracker.GetVariables)
				{
					if (!(strVar.contains("(") || strVar.contains(")") || strVar.contains("[") || strVar.contains("]") || strVar.contains(" ") || strVar.contains("+") || strVar.contains("*")))
					{
						if (Regex.IsMatch(CodeLine, "<%=\\s*\\S*\\s*\\b" + System.Convert.ToString(strVar) + "\\b"))
						{
							frmMain.Default.ListCodeIssue("Potential XSS", "The JSP displays directly a user-supplied parameter directly to the screen. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine);
						}
					}
				}
				
			}
			else if (FileName.ToLower().EndsWith(".jsp") && Regex.IsMatch(CodeLine, "<c:\\bout\\b\\s*\\S*\\s*=\\s*['\"]\\s*\\$\\{\\s*\\S*\\}\\s*['\"]\\s*\\bescapeXML\\b\\s*=\\s*['\"]\\bfalse\\b['\"]\\s*/>"))
			{
				//== Check JSPs for variables being reflected back to page without XML encoding ==
				frmMain.Default.ListCodeIssue("Potential XSS", "The JSP displays application data without applying XML encoding. No validator plug-ins were located in the application's XML files.", FileName, CodeIssue.HIGH, CodeLine);
			}
			
		}
		
		private static void CheckRunTime(string CodeLine, string FileName)
		{
			// Determine whether or not the module uses java.lang.Runtime.exec
			// Check for any any unsafe usage if necessary
			//================================================================
			
			//== Check for use of java.lang.Runtime ==
			if (CodeLine.Contains("Runtime."))
			{
				modMain.ctCodeTracker.IsRuntime = true;
			}
			
			//== Check for unsafe use of java.lang.Runtime.exec ==
			if (modMain.ctCodeTracker.IsRuntime && (CodeLine.Contains(".exec ") || CodeLine.Contains(".exec(")) && (!CodeLine.Contains("\"")))
			{
				frmMain.Default.ListCodeIssue("java.lang.Runtime.exec Gets Path from Variable", "The pathname used in the call appears to be loaded from a variable. Check the code manually to ensure that malicious filenames cannot be submitted by an attacker.", FileName, CodeIssue.HIGH, CodeLine);
			}
			
		}
		
		private static void CheckIsHttps(string CodeLine, string FileName)
		{
			// Determine whether or not the code connects to external URLs
			// Check for any any unsafe usage if necessary
			//============================================================
			
			//== Check for secure HTTP usage ==
			if (CodeLine.Contains("URLConnection") && CodeLine.Contains("HTTP:"))
			{
				frmMain.Default.ListCodeIssue("URL request sent over HTTP:", "The URL used in the HTTP request appears to be unencrypted. Check the code manually to ensure that sensitive data is not being submitted.", FileName, CodeIssue.STANDARD, CodeLine);
			}
			else if ((CodeLine.Contains("URLConnection(") || CodeLine.Contains("URLConnection (")) && (!CodeLine.Contains("\"")))
			{
				frmMain.Default.ListCodeIssue("URL Request Gets Path from Variable", "The URL used in the HTTP request appears to be loaded from a variable. Check the code manually to ensure that malicious URLs cannot be submitted by an attacker.", FileName, CodeIssue.STANDARD, CodeLine);
			}
			
		}
		
		private static void CheckClone(string CodeLine, string FileName)
		{
			// Determine whether or not the code implements cloning
			// Check for any any unsafe usage if necessary
			//=====================================================
			
			//== Check for safe or unsafe implementation of cloning ==
			if (Regex.IsMatch(CodeLine, "\\bpublic\\b\\s+\\w+\\s+\\bclone\\b\\s*\\("))
			{
				modMain.ctCodeTracker.ImplementsClone = true;
			}
			if (modMain.ctCodeTracker.ImplementsClone == true && CodeLine.Contains("throw new java.lang.CloneNotSupportedException"))
			{
				modMain.ctCodeTracker.ImplementsClone = false;
			}
			
		}
		
		private static void CheckSerialize(string CodeLine, string FileName)
		{
			// Determine whether or not the code implements serialization
			// Check for any any unsafe usage if necessary
			//===========================================================
			
			//== Check for safe or unsafe implementation of serialization/deserialization ==
			if (CodeLine.Contains(" writeObject"))
			{
				modMain.ctCodeTracker.IsSerialize = true;
			}
			if (CodeLine.Contains(" readObject"))
			{
				modMain.ctCodeTracker.IsDeserialize = true;
			}
			
			if (modMain.ctCodeTracker.IsSerialize == true && modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.SerializeBraces) && CodeLine.Contains("throw new java.io.IOException"))
			{
				modMain.ctCodeTracker.IsSerialize = false;
			}
			if (modMain.ctCodeTracker.IsDeserialize == true && modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.DeserializeBraces) && CodeLine.Contains("throw new java.io.IOException"))
			{
				modMain.ctCodeTracker.IsDeserialize = false;
			}
			
		}
		
		private static void CheckModifiers(string CodeLine, string FileName)
		{
			// Identify any public variables in classes
			//=========================================
			string strVarName = "";
			
			
			//== Check for public variables and non-final classes (finalize check is optional) ==
			if (CodeLine.Contains("public ") && CodeLine.Contains(";") && !(CodeLine.Contains("{") || CodeLine.Contains("abstract ") || CodeLine.Contains("class ") || CodeLine.Contains("static ")))
			{
				
				strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
				if (Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$"))
				{
					frmMain.Default.ListCodeIssue("Class Contains Public Variable: " + strVarName, "The class variable may be accessed and modified by other classes without the use of getter/setter methods. It is considered unsafe to have public fields or methods in a class unless required as any method, field, or class that is not private is a potential avenue of attack. It is safer to provide accessor methods to variables in order to limit their accessibility.", FileName, CodeIssue.STANDARD, CodeLine);
				}
				
				//== Store public variable name for any thread safety checks if this is a servlet ==
				if (modMain.ctCodeTracker.IsServlet)
				{
					if (!modMain.ctCodeTracker.GlobalVars.ContainsKey(strVarName))
					{
						modMain.ctCodeTracker.GlobalVars.Add(strVarName, modMain.ctCodeTracker.ServletName);
					}
				}
				
			}
			else if (modMain.asAppSettings.IsFinalizeCheck && (CodeLine.Contains("public ") && CodeLine.Contains("class ")) && !CodeLine.Contains("final "))
			{
				frmMain.Default.ListCodeIssue("Public Class Not Declared as Final", "The class is not declared as final as per OWASP recommendations. It is considered best practice to make classes final where possible and practical (i.e. It has no classes which inherit from it). Non-Final classes can allow an attacker to extend a class in a malicious manner. Manually inspect the code to determine whether or not it is practical to make this class final.", FileName, CodeIssue.POSSIBLY_SAFE, CodeLine);
			}
			
		}
		
		private static void CheckInnerClasses(string CodeLine, string FileName)
		{
			// Identify any inner classes within classes
			//==========================================
			
			//== Check for entry into class ==
			if (!modMain.ctCodeTracker.IsInsideClass && Regex.IsMatch(CodeLine, "\\bpublic\\b\\s*\\bclass\\b"))
			{
				if (CodeLine.Contains("{"))
				{
					modMain.ctCodeTracker.IsInsideClass = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.ClassBraces);
				}
				else
				{
					modMain.ctCodeTracker.IsInsideClass = true;
				}
			}
			else if (modMain.ctCodeTracker.IsInsideClass)
			{
				if (CodeLine.Contains("private ") && CodeLine.Contains("class "))
				{
					frmMain.Default.ListCodeIssue("Class Contains Inner Class", "When translated into bytecode, any inner classes are rebuilt within the JVM as external classes within the same package. As a result, any class in the package can access these inner classes. The enclosing class's private fields become protected fields, accessible by the now external 'inner class'.", FileName, CodeIssue.STANDARD, CodeLine);
				}
				modMain.ctCodeTracker.IsInsideClass = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.ClassBraces);
			}
			
		}
		
		private static void CheckThreadIssues(string CodeLine, string FileName)
		{
			// Identify potential for race conditions and deadlocking
			//=======================================================
			bool blnIsRace = false;
			string strSyncObject = "";
			
			
			
			//== Identify object locked for use in synchronized block ==
			if (modMain.ctCodeTracker.IsSynchronized == false && Regex.IsMatch(CodeLine, "\\bsynchronized\\b\\s*\\(\\s*\\w+\\s*\\)"))
			{
				strSyncObject = GetSyncObject(CodeLine);
				modMain.ctCodeTracker.LockedObject = strSyncObject;
				modMain.ctCodeTracker.SyncIndex++;
			}
			
			
			
			//== Identify entry into a synchronized block ==
			//== The synchronized may be followed by method type and name for a synchronized method, or by braces for a synchronized block ==
			if (modMain.ctCodeTracker.IsSynchronized == false && Regex.IsMatch(CodeLine, "\\bsynchronized\\b\\s*\\S*\\s*\\S*\\s*\\("))
			{
				if (CodeLine.Contains("{"))
				{
					modMain.ctCodeTracker.IsSynchronized = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.SyncBraces);
				}
				else
				{
					modMain.ctCodeTracker.IsSynchronized = true;
				}
				
			}
			else if (modMain.ctCodeTracker.IsSynchronized == false && modMain.ctCodeTracker.IsServlet == true)
			{
				
				//== Check for any unsafe modifications to instance variables ==
				if (modMain.ctCodeTracker.GlobalVars.Count > 0)
				{
					
					foreach (var itmItem in modMain.ctCodeTracker.GlobalVars)
					{
						if (CodeLine.Contains(System.Convert.ToString(itmItem.Key)))
						{
							frmMain.Default.ListCodeIssue("Possible Race Condition", "A HttpServlet instance variable is being used/modified without a synchronize block: " + itmItem.Key + Constants.vbNewLine + "Check if any code which calls this code is thread-safe.", FileName, CodeIssue.MEDIUM);
							break;
						}
					}
				}
				
			}
			else if (modMain.ctCodeTracker.IsSynchronized == false)
			{
				
				//== Check for any unsafe modifications to instance variables ==
				if (modMain.ctCodeTracker.GlobalVars.Count > 0)
				{
					foreach (var itmItem in modMain.ctCodeTracker.GlobalVars)
					{
						blnIsRace = CheckRaceCond(CodeLine, FileName, itmItem);
						if (blnIsRace)
						{
							break;
						}
					}
				}
				
				if (blnIsRace == false && modMain.ctCodeTracker.GetSetMethods.Count > 0)
				{
					foreach (var itmItem in modMain.ctCodeTracker.GetSetMethods)
					{
						blnIsRace = CheckRaceCond(CodeLine, FileName, itmItem);
						if (blnIsRace)
						{
							break;
						}
					}
				}
				
			}
			else if (modMain.ctCodeTracker.IsSynchronized)
			{
				//== Track the amount of code that is inside the lock - resources may be locked unnecessarily ==
				if (CodeLine.Trim() != "{" && CodeLine.Trim() != "}")
				{
					modMain.ctCodeTracker.SyncLineCount++;
				}
				
				//== Check whether still inside synchronized code ==
				modMain.ctCodeTracker.IsSynchronized = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.SyncBraces);
				
				//== Check for large areas of locked code and potential deadlock ==
				CheckSyncIssues(CodeLine, FileName);
			}
			
		}
		
		private static void IdentifyServlets(string CodeLine)
		{
			// Identify any instantiation of Servlet Classes and store the object names for thread safety checks
			//==================================================================================================
			string strVarName = "";
			
			
			if ((CodeLine.Contains("public ") || CodeLine.Contains("private ") || CodeLine.Contains("protected ")) && CodeLine.Contains(";") && !CodeLine.Contains("{") && !CodeLine.Contains("abstract "))
			{
				foreach (var strName in modMain.ctCodeTracker.ServletNames)
				{
					if (CodeLine.Contains(System.Convert.ToString(strName)))
					{
						strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
						if (!modMain.ctCodeTracker.ServletInstances.ContainsKey(strVarName))
						{
							modMain.ctCodeTracker.ServletInstances.Add(strVarName, strName.ToString());
						}
						break;
					}
				}
			}
			
		}
		
		private static void IdentifyGetAndSet(string CodeLine)
		{
			// Identify any getter and setter methods within Servlet Classes and store the object names for thread safety checks
			//==================================================================================================================
			string strMethodName = "";
			string[] arrFragments = null;
			
			
			//== Do we have a susceptible method? ==
			if (Regex.IsMatch(CodeLine, "\\s*\\bpublic\\b\\s+\\S*\\s+(g|s)et\\S+\\s*\\("))
			{
				
				//== Extract method name ==
				arrFragments = CodeLine.Split("(".ToCharArray());
				strMethodName = arrFragments.First();
				strMethodName = modMain.GetLastItem(strMethodName);
				
				if (!modMain.ctCodeTracker.GetSetMethods.ContainsKey(strMethodName))
				{
					modMain.ctCodeTracker.GetSetMethods.Add(strMethodName, modMain.ctCodeTracker.ServletName);
				}
				
			}
			
		}
		
		private static bool CheckRaceCond(string CodeLine, string FileName, KeyValuePair<string, string> DictionaryItem)
		{
			// Check if line contains any references to public variables of servlets or to getter/setter methods of servlets
			//==============================================================================================================
			string strServletName = "";
			string[] arrFragments = null;
			bool blnRetVal = false;
			
			
			if (CodeLine.Contains("." + System.Convert.ToString(DictionaryItem.Key)))
			{
				arrFragments = Regex.Split(CodeLine, "." + System.Convert.ToString(DictionaryItem.Key));
				strServletName = modMain.GetLastItem(arrFragments.First());
				if (modMain.ctCodeTracker.ServletInstances.Count > 0 && modMain.ctCodeTracker.ServletInstances.ContainsKey(strServletName))
				{
					if (DictionaryItem.Value == modMain.ctCodeTracker.ServletInstances[strServletName])
					{
						frmMain.Default.ListCodeIssue("Possible Race Condition", "A HttpServlet instance variable is being used/modified without a synchronize block.", FileName, CodeIssue.HIGH);
						blnRetVal = true;
					}
				}
			}
			
			return blnRetVal;
			
		}
		
		private static void CheckUnsafeTempFiles(string CodeLine, string FileName)
		{
			// Identify any creation of temp files with static names
			//======================================================
			
			if (Regex.IsMatch(CodeLine, "\\bnew\\b\\s+File\\s*\\(\\s*\\\"*\\S*(temp|tmp)\\S*\\\"\\s*\\)"))
			{
				frmMain.Default.ListCodeIssue("Unsafe Temporary File Allocation", "The application appears to create a temporary file with a static, hard-coded name. This causes security issues in the form of a classic race condition (an attacker creates a file with the same name between the application's creation and attempted usage) or a symbolic linbk attack where an attacker creates a symbolic link at the temporary file location.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		private static string GetSyncObject(string CodeLine)
		{
			// Extract the name of a synchronized object from a line of code
			//==============================================================
			string strSyncObject = "";
			string[] strFragments = null;
			
			
			strFragments = Regex.Split(CodeLine, "\\bsynchronized\\b\\s*\\(");
			strSyncObject = modMain.GetFirstItem(strFragments.Last(), ")");
			if (!string.IsNullOrEmpty(strSyncObject))
			{
				modMain.ctCodeTracker.LockedObject = strSyncObject;
			}
			
			return strSyncObject;
			
		}
		
		private static void CheckSyncIssues(string CodeLine, string FileName)
		{
			// Check for, and report on, any issues located inside the synchronized block or when leaving the block
			//=====================================================================================================
			int intSeverity = 0;
			int intIndex = 0;
			string strSyncObject = "";
			string strOuterSyncObject = "";
			
			
			//== Report potentially excessive locking when leaving the code block ==
			if (modMain.ctCodeTracker.IsSynchronized == false)
			{
				
				if (modMain.ctCodeTracker.SyncLineCount > 14)
				{
					intSeverity = CodeIssue.MEDIUM;
				}
				else if (modMain.ctCodeTracker.SyncLineCount > 10)
				{
					intSeverity = CodeIssue.STANDARD;
				}
				else if (modMain.ctCodeTracker.SyncLineCount > 6)
				{
					intSeverity = CodeIssue.LOW;
				}
				
				if (modMain.ctCodeTracker.SyncLineCount > 6)
				{
					frmMain.Default.ListCodeIssue("Synchronized Code - Possible Performance Impact", "There are " + System.Convert.ToString(modMain.ctCodeTracker.SyncLineCount) + " lines of code in the synchronized block. Manually check the code to ensure any shared resources are not being locked unnecessarily.", FileName, intSeverity);
				}
				
				modMain.ctCodeTracker.SyncLineCount = 0;
				
			}
			else if (!string.IsNullOrEmpty(modMain.ctCodeTracker.LockedObject)&& Regex.IsMatch(CodeLine, "\\bsynchronized\\b\\s*\\(\\s*\\w+\\s*\\)"))
			{
				//== Build dictionary for potential deadlocks by tracking synchronized blocks inside synchronized blocks ==
				strOuterSyncObject = modMain.ctCodeTracker.LockedObject;
				strSyncObject = GetSyncObject(CodeLine);
				
				if (!string.IsNullOrEmpty(strSyncObject))
				{
					//== Check if this sync block already exists ==
					foreach (var itmItem in modMain.ctCodeTracker.SyncBlockObjects)
					{
						if (itmItem.BlockIndex == modMain.ctCodeTracker.SyncIndex)
						{
							intIndex = System.Convert.ToInt32(itmItem.BlockIndex);
							//== Add to existing block ==
							if (!itmItem.InnerObjects.Contains(strSyncObject))
							{
								itmItem.InnerObjects.Add(strSyncObject);
							}
							break;
						}
					}
					
					//== Create new sync block an add inner object name ==
					if (intIndex == 0)
					{
						AddNewSyncBlock(strOuterSyncObject, strSyncObject);
					}
					
					CheckDeadlock(strOuterSyncObject, strSyncObject, FileName);
					
				}
			}
			
		}
		
		public static void AddNewSyncBlock(string OuterObject, string InnerObject)
		{
			// Initialise a new syncblock container to hold details of locked items
			//=====================================================================
			SyncBlock sbSyncBlock = new SyncBlock();
			
			sbSyncBlock.BlockIndex = modMain.ctCodeTracker.SyncIndex;
			sbSyncBlock.OuterObject = OuterObject;
			sbSyncBlock.InnerObjects.Add(InnerObject);
			
			modMain.ctCodeTracker.SyncBlockObjects.Add(sbSyncBlock);
			
		}
		
		public static void CheckDeadlock(string OuterObject, string InnerObject, string FileName)
		{
			// Check whether the locked object combination has a reverse block where the inner item and outer item swap places
			//================================================================================================================
			
			foreach (var itmItem in modMain.ctCodeTracker.SyncBlockObjects)
			{
				if (itmItem.OuterObject == InnerObject && itmItem.InnerObjects.Contains(OuterObject))
				{
					frmMain.Default.ListCodeIssue("Synchronized Code May Result in DeadLock", "The objects " + OuterObject + " and " + InnerObject + " lock each other in such a way that they may become deadlocked.", FileName, CodeIssue.MEDIUM);
					break;
				}
			}
			
		}
		
		private static void CheckPrivileged(string CodeLine, string FileName)
		{
			// Check for unsafe use of privileged blocks
			//==========================================
			int intSeverity = 0;
			
			
			//== The IsInsideClass variable tracks whether we are inside a public class and can be re-used here ==
			if (modMain.ctCodeTracker.IsInsideClass == true)
			{
				
				//== Check for public method ==
				if (modMain.ctCodeTracker.IsInsideMethod == false && Regex.IsMatch(CodeLine, "\\bpublic\\b\\s+\\w+\\s+\\w+\\s*\\w*\\s*\\("))
				{
					
					if (CodeLine.Contains("{"))
					{
						modMain.ctCodeTracker.IsInsideMethod = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.MethodBraces);
					}
					else
					{
						modMain.ctCodeTracker.IsInsideMethod = true;
					}
					
				}
				else if (modMain.ctCodeTracker.IsInsideMethod == true && Regex.IsMatch(CodeLine, "\\bAccessController\\b\\.\\bdoPrivileged\\b"))
				{
					
					if (CodeLine.Contains("{"))
					{
						modMain.ctCodeTracker.IsPrivileged = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.PrivBraces);
					}
					else
					{
						modMain.ctCodeTracker.IsPrivileged = true;
					}
					frmMain.Default.ListCodeIssue("Use of AccessController.doPrivileged() in Public Method of Public Class", "The code will execute with system privileges and should be manually checked with great care to ensure no vulnerabilities are present.", FileName, CodeIssue.MEDIUM, CodeLine);
					
				}
				else if (modMain.ctCodeTracker.IsPrivileged == false && Regex.IsMatch(CodeLine, "\\bAccessController\\b\\.\\bdoPrivileged\\b"))
				{
					
					if (CodeLine.Contains("{"))
					{
						modMain.ctCodeTracker.IsPrivileged = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.PrivBraces);
					}
					else
					{
						modMain.ctCodeTracker.IsPrivileged = true;
					}
					
				}
				else if (modMain.ctCodeTracker.IsPrivileged == true)
				{
					
					//== Track the amount of code that is inside the lock - resources may be locked unnecessarily ==
					if (CodeLine.Trim() != "{" && CodeLine.Trim() != "}")
					{
						modMain.ctCodeTracker.PrivLineCount++;
					}
					modMain.ctCodeTracker.IsPrivileged = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.PrivBraces);
					
					//== If we've exited then give notice of excessively large privileged blocks ==
					if (modMain.ctCodeTracker.IsInsideMethod == true && modMain.ctCodeTracker.IsPrivileged == false)
					{
						
						if (modMain.ctCodeTracker.PrivLineCount > 20)
						{
							intSeverity = CodeIssue.MEDIUM;
						}
						else if (modMain.ctCodeTracker.PrivLineCount > 15)
						{
							intSeverity = CodeIssue.STANDARD;
						}
						else if (modMain.ctCodeTracker.PrivLineCount > 10)
						{
							intSeverity = CodeIssue.LOW;
						}
						
						if (modMain.ctCodeTracker.PrivLineCount > 10)
						{
							frmMain.Default.ListCodeIssue("Privileged Code - Possible Risks", "There are " + System.Convert.ToString(modMain.ctCodeTracker.PrivLineCount) + " lines of code in the privileged block. Manually check the code to ensure no unnecessary code is included.", FileName, intSeverity);
						}
						modMain.ctCodeTracker.PrivLineCount = 0;
					}
					else if (modMain.ctCodeTracker.IsPrivileged == true)
					{
						//== Check for use of user-controlled variables within privileged code ==
						foreach (var strVar in modMain.ctCodeTracker.GetVariables)
						{
							if (!(strVar.contains("(") || strVar.contains(")") || strVar.contains("[") || strVar.contains("]") || strVar.contains(" ") || strVar.contains("+") || strVar.contains("*")))
							{
								if (Regex.IsMatch(CodeLine, "\\b" + System.Convert.ToString(strVar) + "\\b"))
								{
									frmMain.Default.ListCodeIssue("Use of User-Controlled Variable Within Privileged Code", "The code will execute with system privileges and the usage of the variable should be manually checked with great care.", FileName, CodeIssue.HIGH, CodeLine);
									break;
								}
							}
						}
					}
				}
				
			}
			
		}
		
		private static void CheckRequestDispatcher(string CodeLine, string FileName)
		{
			// Check for unsafe use of RequestDispatcher
			//==========================================
			
			if (Regex.IsMatch(CodeLine, "\\.\\bgetRequestDispatcher\\b\\s*\\("))
			{
				//== Check for use of user-controlled variable within privileged code ==
				foreach (var strVar in modMain.ctCodeTracker.GetVariables)
				{
					if (!(strVar.contains("(") || strVar.contains(")") || strVar.contains("[") || strVar.contains("]") || strVar.contains(" ") || strVar.contains("+") || strVar.contains("*")))
					{
						if (Regex.IsMatch(CodeLine, "\\bgetRequestDispatcher\\b\\s*\\(\\s*\\S*\\s*\\S*\\s*\\b" + System.Convert.ToString(strVar) + "\\b"))
						{
							frmMain.Default.ListCodeIssue("Use of RequestDispatcher in Combination with User-Controlled Variable", "The code appears to use a user-controlled variable in a RequestDispatcher method which can allow horizontal directory traversal, allowing an attacker to download system files.", FileName, CodeIssue.HIGH, CodeLine);
							break;
						}
					}
				}
			}
			
		}
		
		private static void CheckXXEExpansion(string CodeLine, string FileName)
		{
			// Determine whether XML expansion is possible and check feasibility of XML-Bomb delivery
			//=======================================================================================
			
			//== Check for use of XXE parser ==
			if (modMain.ctCodeTracker.HasXXEEnabled == false && Regex.IsMatch(CodeLine, "import\\s+javax\\.xml\\.bind\\.JAXB\\s*\\;"))
			{
				modMain.ctCodeTracker.HasXXEEnabled = true;
			}
			
			if (modMain.ctCodeTracker.HasXXEEnabled == true && Regex.IsMatch(CodeLine, "\\(\\s*(XMLConstants\\.FEATURE_SECURE_PROCESSING|XMLInputFactory.SUPPORT_DTD)\\s*\\,\\s*false\\s*\\)"))
			{
				//== Deliberate setting of entity expansion ==
				frmMain.Default.ListCodeIssue("XML Entity Expansion Enabled", "The FEATURE_SECURE_PROCESSING attribute is set to false which can render the application vulnerable to the use of XML bombs. Check the necessity of enabling this feature and check for validation of incoming data.", FileName, CodeIssue.HIGH, CodeLine);
			}
			else if (modMain.ctCodeTracker.HasXXEEnabled == true && Regex.IsMatch(CodeLine, "\\(\\s*(XMLConstants\\.FEATURE_SECURE_PROCESSING|XMLInputFactory.SUPPORT_DTD)\\s*\\,\\s*true\\s*\\)"))
			{
				//== Security settings have been applied ==
				modMain.ctCodeTracker.HasXXEEnabled = false;
			}
			else if (modMain.ctCodeTracker.HasXXEEnabled == true)
			{
				
			}
			
		}
		
		private static void CheckOverflow(string CodeLine, string FileName)
		{
			// Identify occurences of primitive types and warn for any potential overflows
			//============================================================================
			
			//== Identify any primitives and add to dictionary ==
			if (Regex.IsMatch(CodeLine, "\\b(short|int|long)\\b\\s+\\w+\\s*(\\=|\\;)"))
			{
				modMain.ctCodeTracker.HasPrimitives = true;
				modMain.ctCodeTracker.AddInteger(CodeLine);
			}
			
			//== Warn of any mathematical operations on integers and possible overflows ==
			if (modMain.ctCodeTracker.HasPrimitives == true && (CodeLine.Contains("+") || CodeLine.Contains("-") || CodeLine.Contains("*")))
			{
				foreach (var itmIntItem in modMain.ctCodeTracker.GetIntegers())
				{
					if (!(itmIntItem.Key.Contains("(") || itmIntItem.Key.Contains(")") || itmIntItem.Key.Contains("[") || itmIntItem.Key.Contains("]") || itmIntItem.Key.Contains(" ") || itmIntItem.Key.Contains("+") || itmIntItem.Key.Contains("*")))
					{
						if (Regex.IsMatch(CodeLine, "\\b" + itmIntItem.Key + "\\b"))
						{
							foreach (var itmVarItem in modMain.ctCodeTracker.GetVariables)
							{
								frmMain.Default.ListCodeIssue("Operation on Primitive Data Type", "The code appears to be carrying out a mathematical operation involving a primitive data type and a user-supplied variable. This may result in an overflow and unexpected behaviour. Check the code manually to determine the risk.", FileName, CodeIssue.HIGH, CodeLine);
								return;
							}
							frmMain.Default.ListCodeIssue("Operation on Primitive Data Type", "The code appears to be carrying out a mathematical operation on a primitive data type. In some circumstances this can result in an overflow and unexpected behaviour. Check the code manually to determine the risk.", FileName, CodeIssue.LOW, CodeLine);
							return;
						}
					}
				}
			}
			
		}
		
		private static void CheckResourceRelease(string CodeLine, string FileName)
		{
			// Check that try ... catch blocks are being used to release resources and avoid DoS
			//==================================================================================
			
			//== Record any instances of filestreams being created ==
			if (modMain.ctCodeTracker.IsFileOpen == false && Regex.IsMatch(CodeLine, "\\bnew\\b\\s+\\bFileOutputStream\\b\\s*\\("))
			{
				modMain.ctCodeTracker.IsFileOpen = true;
				modMain.ctCodeTracker.HasResourceRelease = false;
				modMain.ctCodeTracker.FileOpenLine = (int) modMain.rtResultsTracker.LineCount;
			}
			
			
			//== Check for safe release of resources in all cases ==
			if (modMain.ctCodeTracker.IsFileOpen == true && Regex.IsMatch(CodeLine, "\\btry\\b"))
			{
				modMain.ctCodeTracker.HasTry = true;
			}
			else if (modMain.ctCodeTracker.IsFileOpen == true && Regex.IsMatch(CodeLine, "\\bfinally\\b"))
			{
				modMain.ctCodeTracker.IsFileOpen = false;
				if (Regex.IsMatch(CodeLine, "\\.\\bclose\\b\\s*\\("))
				{
					modMain.ctCodeTracker.HasResourceRelease = true;
				}
			}
			
		}
		
		private static void CheckAndroidStaticCrypto(string CodeLine, string FileName)
		{
			// Determine whether static crypto is being used for Android apps
			//===============================================================
			
			//== Check for use of static string in crypto command ==
			if (Regex.IsMatch(CodeLine, "CryptoAPI\\.(encrypt|decrypt)\\s*\\(\\\"\\w+\\\"\\s*\\,"))
			{
				frmMain.Default.ListCodeIssue("Static Crypto Keys in Use", "The application appears to be using static crypto keys. The absence of secure key storage may allow unauthorised decryption of data.", FileName, CodeIssue.HIGH, CodeLine);
			}
			
		}
		
		private static void CheckAndroidIntent(string CodeLine, string FileName)
		{
			// Determine whether implicit intents are being used for Android apps
			//===================================================================
			string[] strFragments = null;
			string strIntent = "";
			
			
			//== Check for creation of blank intent ==
			if (modMain.ctCodeTracker.HasIntent == false && Regex.IsMatch(CodeLine, "\\bIntent\\b\\s+\\w+\\s*\\=\\s*new\\s+Intent\\s*\\(\\s*\\)"))
			{
				
				// Sey boolean to show that an intent exists
				modMain.ctCodeTracker.HasIntent = true;
				
				// Store the name of the intent
				strFragments = Regex.Split(CodeLine, "\\=\\s*new\\s+Intent\\s*\\(\\s*\\)");
				strIntent = modMain.GetLastItem(strFragments.First());
				if (!string.IsNullOrEmpty(strIntent)&& !modMain.ctCodeTracker.AndroidIntents.Contains(strIntent))
				{
					modMain.ctCodeTracker.AndroidIntents.Add(strIntent);
				}
				
			}
			else if (modMain.ctCodeTracker.HasIntent == true && Regex.IsMatch(CodeLine, "\\.setClass\\("))
			{
				
				// Remove any explicit intents from the dictionary
				strFragments = Regex.Split(CodeLine, "\\.setClass\\(");
				
				if (strFragments.Count() > 1)
				{
					strIntent = modMain.GetFirstItem(System.Convert.ToString(strFragments.ElementAt(1)), ")");
					
					if (!string.IsNullOrEmpty(strIntent)&& modMain.ctCodeTracker.AndroidIntents.Contains(strIntent))
					{
						modMain.ctCodeTracker.AndroidIntents.Remove(strIntent);
					}
					if (modMain.ctCodeTracker.AndroidIntents.Count == 0)
					{
						modMain.ctCodeTracker.HasIntent = false;
					}
				}
				
			}
			else if (modMain.ctCodeTracker.HasIntent == true && Regex.IsMatch(CodeLine, "\\bstartActivity\\b\\s*\\("))
			{
				
				// Remove any explicit intents from the dictionary
				strFragments = Regex.Split(CodeLine, "\\bstartActivity\\b\\s*\\(");
				
				if (strFragments.Count() > 1)
				{
					strIntent = modMain.GetFirstItem(System.Convert.ToString(strFragments.ElementAt(1)), ")");
					if (!string.IsNullOrEmpty(strIntent)&& modMain.ctCodeTracker.AndroidIntents.Contains(strIntent))
					{
						modMain.ctCodeTracker.AndroidIntents.Remove(strIntent);
						if (modMain.ctCodeTracker.AndroidIntents.Count == 0)
						{
							modMain.ctCodeTracker.HasIntent = false;
						}
						frmMain.Default.ListCodeIssue("Implicit Intents in Use", "The application appears to be using implicit intents which could be intercepted by rogue applications. Intent name: " + strIntent, FileName, CodeIssue.MEDIUM, CodeLine);
					}
				}
			}
			
		}
		
	}
	
}
