#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Core
{
	using System;
	using System.IO;

	/// <summary>
	///  Extends SimpleTestRunner by redirecting its console output to an 
	///  eventlistener text writer so it can be received as an event.
	/// </summary>
	public class OutputEventTestRunner : SimpleTestRunner
	{
		protected override TestResult[] doRun( EventListener listener, Test[] tests )
		{
			// Save static context so we can change and restore Console
			// Out and Error and in case the tests change anything - as
			// happens, for example, in testing NUnit itself.
			using( new TestContext() )
			{
				EventListenerTextWriter outWriter = new EventListenerTextWriter(listener, TestOutputType.Out);
				EventListenerTextWriter errorWriter = new EventListenerTextWriter(listener, TestOutputType.Error);

				TestContext.Out = outWriter;
				TestContext.Error = errorWriter;

				return base.doRun (listener, tests);
			}
		}

	}
}

