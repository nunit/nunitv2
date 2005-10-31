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

using System;

namespace NUnit.Core
{
	/// <summary>
	/// TestFixture is a surrogate for a user test fixture class,
	/// containing one or more tests.
	/// </summary>
	public class TestFixture : TestSuite
	{
		#region Constructors
		public TestFixture( Type fixtureType, int assemblyKey )
			: base( fixtureType, assemblyKey ) { }
		#endregion

		#region Properties
		public override bool IsFixture
		{
			get { return true; }
		}
		#endregion

		#region TestSuite Overrides

		public override void DoFixtureSetUp( TestResult suiteResult )
		{
			try 
			{
				if ( Fixture == null )
					Fixture = Reflect.Construct( FixtureType );

				if (this.fixtureSetUp != null)
					Reflect.InvokeMethod(fixtureSetUp, Fixture);
				Status = SetUpState.SetUpComplete;
			} 
			catch (Exception ex) 
			{
				//NunitException nex = ex as NunitException;
				if ( ex is NunitException || ex is System.Reflection.TargetInvocationException )
					ex = ex.InnerException;

				if ( testFramework.IsIgnoreException( ex ) )
				{
					this.ShouldRun = false;
					suiteResult.NotRun(ex.Message);
					suiteResult.StackTrace = ex.StackTrace;
					this.IgnoreReason = ex.Message;
				}
				else
				{
					suiteResult.Failure( ex.Message, ex.StackTrace );
					this.Status = SetUpState.SetUpFailed;
				}
			}
			finally
			{
				if ( testFramework != null )
					suiteResult.AssertCount = testFramework.GetAssertCount();
			}
		}

		public override void DoFixtureTearDown( TestResult suiteResult )
		{
			if (this.ShouldRun) 
			{
				try 
				{
					Status = SetUpState.SetUpNeeded;
					if (this.fixtureTearDown != null)
						Reflect.InvokeMethod(fixtureTearDown, Fixture);
				} 
				catch (Exception ex) 
				{
					// Error in TestFixtureTearDown causes the
					// suite to be marked as a failure, even if
					// all the contained tests passed.
					NunitException nex = ex as NunitException;
					if (nex != null)
						ex = nex.InnerException;

				
					suiteResult.Failure( ex.Message, ex.StackTrace);
				}
				finally
				{
					if ( testFramework != null )
						suiteResult.AssertCount += testFramework.GetAssertCount();
				}
			}
		}

		#endregion
	}
}
