using System;
using System.IO;
using System.Reflection;

namespace NUnit.Framework.Tests
{
	public class TestFile : IDisposable
	{
		private bool _disposedValue = false;
		private string _resourceName;
		private string _fileName;

		public TestFile(string fileName, string resourceName)
		{
			_resourceName = resourceName;
			_fileName = fileName;

			Assembly a = Assembly.GetExecutingAssembly();
			using (Stream s = a.GetManifestResourceStream(_resourceName))
			{
				if (s == null) throw new Exception("Manifest Resource Stream " + _resourceName + " was not found.");

				using (StreamReader sr = new StreamReader(s))
				{
					using (StreamWriter sw = File.CreateText(_fileName))
					{
						sw.Write(sr.ReadToEnd());
						sw.Flush();
					}
				}
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposedValue)
			{
				if (disposing)
				{
					if(File.Exists(_fileName))
					{
						File.Delete(_fileName);
					}
				}
			}
			this._disposedValue = true;
		}

		#region IDisposable Members

		public void Dispose()
		{
			// Do not change this code.  Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}


	/// <summary>
	/// Summary description for FileAssertTests.
	/// </summary>
	[TestFixture]
	public class FileAssertTests
	{

		#region AreEqual
		[Test]
		public void AreEqualPassesWhenBothAreNull()
		{
			FileStream expected = null;
			FileStream actual = null;
			FileAssert.AreEqual( expected, actual );
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualThrowsAssertionExceptionWhenBinaryFilesDiffer()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					using(FileStream expected = File.OpenRead("Test1.jpg"))
					{
						using(FileStream actual = File.OpenRead("Test2.jpg"))
						{
							FileAssert.AreEqual( expected, actual);
						}
					}
				}
			}
		}

		[Test]
		public void AreEqualFailureMessageFormat()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					using(FileStream expected = File.OpenRead("Test1.jpg"))
					{
						using(FileStream actual = File.OpenRead("Test2.jpg"))
						{
							StreamAsserter asserter = new StreamEqualAsserter( expected, actual, "message {0}", 42 );
							Assert.IsFalse( asserter.Test() );
							Assert.AreEqual( "message 42" + System.Environment.NewLine + "expected and actual are not equal." + System.Environment.NewLine + "\texpected: Length : 3304" + System.Environment.NewLine + "\t but was: Length : 3464" + Environment.NewLine, asserter.Message );
						}
					}
				}
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualOverloadThrowsAssertionExceptionWhenBinaryFilesDiffer()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					using(FileStream expected = File.OpenRead("Test1.jpg"))
					{
						using(FileStream actual = File.OpenRead("Test2.jpg"))
						{
							FileAssert.AreEqual( expected, actual, "What?!" );
						}
					}
				}
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualThrowsAssertionExceptionIfOneIsNull()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(FileStream expected = File.OpenRead("Test1.jpg"))
				{
						FileAssert.AreEqual( expected, null, "What?!" );
				}
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualThrowsAssertionExceptionIfFilesAreDifferent()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					FileInfo expected = new FileInfo( "Test1.jpg" );
					FileInfo actual = new FileInfo( "Test2.jpg" );
					FileAssert.AreEqual( expected, actual );
				}
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualThrowsAssertionExceptionIfFilesAreDifferentWithMsg()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					FileInfo expected = new FileInfo( "Test1.jpg" );
					FileInfo actual = new FileInfo( "Test2.jpg" );
					FileAssert.AreEqual( expected, actual, "test" );
				}
			}
		}

		[Test]
		public void AreEqualWorksHappyPath()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
				{
					using(FileStream expected = File.OpenRead("Test1.jpg"))
					{
						using(FileStream actual = File.OpenRead("Test2.jpg"))
						{
							FileAssert.AreEqual( expected, actual );
							FileAssert.AreEqual( expected, actual, "What?!" );
						}
					}
				}
			}
		}


		[Test]
		public void AreEqualWorksWithFileInfo()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
				{
					FileInfo expected = new FileInfo( "Test1.jpg" );
					FileInfo actual = new FileInfo( "Test2.jpg" );
					FileAssert.AreEqual( expected, actual );
					FileAssert.AreEqual( expected, actual, "testing" );
				}
			}
		}


		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualThrowsAssertionExceptionIfFileStringsAreDifferent()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					FileAssert.AreEqual( "Test1.jpg", "Test2.jpg" );
				}
			}
		}

		[Test]
		public void AreEqualWorksIfFileStringsAreSame()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				FileAssert.AreEqual( "Test1.jpg", "Test1.jpg" );
				FileAssert.AreEqual( "Test1.jpg", "Test1.jpg" , "testing!" );
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreEqualThrowsAssertionExceptionIfFileStringsAreDifferentWithMessage()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					FileAssert.AreEqual( "Test1.jpg","Test2.jpg", "testing" );
				}
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void IteratesOverTheEntireFileAndFails()
		{
			using(TestFile tf1 = new TestFile("Test1.txt","NUnit.Framework.Tests.TestText1.txt"))
			{
				using(TestFile tf2 = new TestFile("Test2.txt","NUnit.Framework.Tests.TestText2.txt"))
				{
					FileAssert.AreEqual( "Test1.txt", "Test2.txt", "What?!" );
				}
			}
		}

		[Test]
		public void IteratesOverTheEntireFile()
		{
			using(TestFile tf1 = new TestFile("Test1.txt","NUnit.Framework.Tests.TestText1.txt"))
			{
				using(TestFile tf2 = new TestFile("Test2.txt","NUnit.Framework.Tests.TestText1.txt"))
				{
					FileAssert.AreEqual( "Test1.txt", "Test2.txt", "What?!" );
				}
			}
		}
		#endregion


		#region AreNotEqual

		[Test, ExpectedException(typeof(AssertionException))]
		public void AreNotEqualFailsWhenBothAreNull()
		{
			FileStream expected = null;
			FileStream actual = null;
			FileAssert.AreNotEqual( expected, actual );
		}

		[Test]
		public void AreNotEqualPassesWhenBinaryFilesDiffer()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					using(FileStream expected = File.OpenRead("Test1.jpg"))
					{
						using(FileStream actual = File.OpenRead("Test2.jpg"))
						{
							FileAssert.AreNotEqual( expected, actual);
						}
					}
				}
			}
		}

		[Test]
		public void AreNotEqualOverloadSucceedsWhenBinaryFilesDiffer()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					using(FileStream expected = File.OpenRead("Test1.jpg"))
					{
						using(FileStream actual = File.OpenRead("Test2.jpg"))
						{
							FileAssert.AreNotEqual( expected, actual, "What?!" );
						}
					}
				}
			}
		}

		[Test]
		public void AreNotEqualSucceedsIfOneIsNull()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(FileStream expected = File.OpenRead("Test1.jpg"))
				{
					FileAssert.AreNotEqual( expected, null, "What?!" );
				}
			}
		}

		[Test]
		public void AreNotEqualSucceedsIfFilesAreDifferent()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					FileInfo expected = new FileInfo( "Test1.jpg" );
					FileInfo actual = new FileInfo( "Test2.jpg" );
					FileAssert.AreNotEqual( expected, actual );
				}
			}
		}

		[Test]
		public void AreNotEqualSucceedsIfFilesAreDifferentWithMsg()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					FileInfo expected = new FileInfo( "Test1.jpg" );
					FileInfo actual = new FileInfo( "Test2.jpg" );
					FileAssert.AreNotEqual( expected, actual, "test" );
				}
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreNotEqualFailsFileStream()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
				{
					using(FileStream expected = File.OpenRead("Test1.jpg"))
					{
						using(FileStream actual = File.OpenRead("Test2.jpg"))
						{
							FileAssert.AreNotEqual( expected, actual );
						}
					}
				}
			}
		}


		[Test]
		public void AreNotEqualFailureMessageFormat()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
				{
					using(FileStream expected = File.OpenRead("Test1.jpg"))
					{
						using(FileStream actual = File.OpenRead("Test2.jpg"))
						{
							StreamAsserter asserter = new StreamNotEqualAsserter( expected, actual, "message {0}", 42 );
							Assert.IsFalse( asserter.Test() );
							Assert.AreEqual( "message 42" + System.Environment.NewLine + "expected and actual are equal." + System.Environment.NewLine + "\texpected: Length : 3304" + System.Environment.NewLine + "\t but was: Length : 3304" + Environment.NewLine, asserter.Message );
						}
					}
				}
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreNotEqualFailsWithFileInfo()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
				{
					FileInfo expected = new FileInfo( "Test1.jpg" );
					FileInfo actual = new FileInfo( "Test2.jpg" );
					FileAssert.AreNotEqual( expected, actual );
				}
			}
		}


		[Test]
		public void AreNotEqualPassesIfFileStringsAreDifferent()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					FileAssert.AreNotEqual( "Test1.jpg", "Test2.jpg" );
				}
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreNotEqualFailsIfFileStringsAreSame()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				FileAssert.AreNotEqual( "Test1.jpg", "Test1.jpg" );
			}
		}

		[Test]
		public void AreNotEqualPassesIfFileStringsAreDifferentWithMessage()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","NUnit.Framework.Tests.TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","NUnit.Framework.Tests.TestImage2.jpg"))
				{
					FileAssert.AreNotEqual( "Test1.jpg","Test2.jpg", "testing" );
				}
			}
		}

		[Test]
		public void AreNotEqualIteratesOverTheEntireFile()
		{
			using(TestFile tf1 = new TestFile("Test1.txt","NUnit.Framework.Tests.TestText1.txt"))
			{
				using(TestFile tf2 = new TestFile("Test2.txt","NUnit.Framework.Tests.TestText2.txt"))
				{
					FileAssert.AreNotEqual( "Test1.txt", "Test2.txt", "What?!" );
				}
			}
		}

		[Test]
		[ExpectedException(typeof(AssertionException))]
		public void AreNotEqualIteratesOverTheEntireFileAndFails()
		{
			using(TestFile tf1 = new TestFile("Test1.txt","NUnit.Framework.Tests.TestText1.txt"))
			{
				using(TestFile tf2 = new TestFile("Test2.txt","NUnit.Framework.Tests.TestText1.txt"))
				{
					FileAssert.AreNotEqual( "Test1.txt", "Test2.txt", "What?!" );
				}
			}
		}
		#endregion

	}
}
