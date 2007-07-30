using System;
using System.Threading;
using System.Globalization;
using NUnit.Framework;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Summary description for TestContextTests.
	/// </summary>
	[TestFixture]
	public class TestContextTests
	{
		string currentDirectory;
		CultureInfo currentCulture;

		/// <summary>
		/// Since we are testing the mechanism that saves and
		/// restores contexts, we save manually here
		/// </summary>
		[SetUp]
		public void SaveContext()
		{
			currentDirectory = Environment.CurrentDirectory;
			currentCulture = CultureInfo.CurrentCulture;
		}

		[TearDown]
		public void RestoreContext()
		{
			Environment.CurrentDirectory = currentDirectory;
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}

		[Test]
		public void SetAndRestoreCurrentDirectory()
		{
			Assert.AreEqual( currentDirectory, TestContext.CurrentDirectory, "Directory not in initial context" );
			
			using ( new TestContext() )
			{
				string otherDirectory = System.IO.Path.GetTempPath();
				if( otherDirectory[otherDirectory.Length-1] == System.IO.Path.DirectorySeparatorChar )
					otherDirectory = otherDirectory.Substring(0, otherDirectory.Length-1);
				TestContext.CurrentDirectory = otherDirectory;
				Assert.AreEqual( otherDirectory, Environment.CurrentDirectory, "Directory was not set" );
				Assert.AreEqual( otherDirectory, TestContext.CurrentDirectory, "Directory not in new context" );
			}

			Assert.AreEqual( currentDirectory, Environment.CurrentDirectory, "Directory was not restored" );
			Assert.AreEqual( currentDirectory, TestContext.CurrentDirectory, "Directory not in final context" );
		}

		[Test]
		public void SetAndRestoreCurrentCulture()
		{
			Assert.AreEqual( currentCulture, TestContext.CurrentCulture, "Culture not in initial context" );
			
			using ( new TestContext() )
			{
				CultureInfo otherCulture =
					new CultureInfo( currentCulture.Name == "fr-FR" ? "en-GB" : "fr-FR" );
				TestContext.CurrentCulture = otherCulture;
				Assert.AreEqual( otherCulture, CultureInfo.CurrentCulture, "Culture was not set" );
				Assert.AreEqual( otherCulture, TestContext.CurrentCulture, "Culture not in new context" );
			}

			Assert.AreEqual( currentCulture, CultureInfo.CurrentCulture, "Culture was not restored" );
			Assert.AreEqual( currentCulture, TestContext.CurrentCulture, "Culture not in final context" );
		}
	}
}
