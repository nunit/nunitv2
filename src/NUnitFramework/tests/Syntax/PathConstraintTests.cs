using System;

namespace NUnit.Framework.Syntax
{
    namespace Helpers
    {
        public class PathConstraintTests
        {
            [Test]
            public void SimplePathConstraintTests()
            {
                Assert.That("/folder1/./junk/../folder2",
                    Is.SamePath("/folder1/folder2"));
                Assert.That("/folder1/./junk/../folder2/x",
                    Is.Not.SamePath("/folder1/folder2"));

                Assert.That(@"C:\folder1\folder2",
                    Is.SamePath(@"C:\Folder1\Folder2").IgnoreCase);
                Assert.That("/folder1/folder2",
                    Is.Not.SamePath("/Folder1/Folder2").RespectCase);

                Assert.That("/folder1/./junk/../folder2",
                    Is.SamePathOrUnder("/folder1/folder2"));
                Assert.That("/folder1/junk/../folder2/./folder3",
                    Is.SamePathOrUnder("/folder1/folder2"));
                Assert.That("/folder1/junk/folder2/folder3",
                    Is.Not.SamePathOrUnder("/folder1/folder2"));

                Assert.That(@"C:\folder1\folder2\folder3",
                    Is.SamePathOrUnder(@"C:\Folder1\Folder2").IgnoreCase);
                Assert.That("/folder1/folder2/folder3",
                    Is.Not.SamePathOrUnder("/Folder1/Folder2").RespectCase);
            }
        }
    }

    namespace Inherited
    {
        public class PathConstraintTests : AssertionHelper
        {
            [Test]
            public void SimplePathConstraintTests()
            {
                Expect("/folder1/./junk/../folder2",
                    SamePath("/folder1/folder2"));
                Expect("/folder1/./junk/../folder2/x",
                    Not.SamePath("/folder1/folder2"));

                Expect(@"C:\folder1\folder2",
                    SamePath(@"C:\Folder1\Folder2").IgnoreCase);
                Expect("/folder1/folder2",
                    Not.SamePath("/Folder1/Folder2").RespectCase);

                Expect("/folder1/./junk/../folder2",
                    SamePathOrUnder("/folder1/folder2"));
                Expect("/folder1/junk/../folder2/./folder3",
                    SamePathOrUnder("/folder1/folder2"));
                Expect("/folder1/junk/folder2/folder3",
                    Not.SamePathOrUnder("/folder1/folder2"));

                Expect(@"C:\folder1\folder2\folder3",
                    SamePathOrUnder(@"C:\Folder1\Folder2").IgnoreCase);
                Expect("/folder1/folder2/folder3",
                    Not.SamePathOrUnder("/Folder1/Folder2").RespectCase);
            }
        }
    }
}
