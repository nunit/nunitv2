using System;
using System.Collections;
using System.Text;
using NUnit.Framework;

namespace NUnit.Util.Tests
{
    [TestFixture]
    public class RecentFileEntryTests
    {
        private RecentFileEntry entry;
        private static readonly string entryPath = "a/b/c";
		private static Version entryVersion = new Version("1.2");
		private static Version currentVersion = Environment.Version;

		[Test]
		public void CanCreateFromSimpleFileName()
		{
			RecentFileEntry entry = new RecentFileEntry( entryPath );
			Assert.AreEqual( entryPath, entry.Path );
			Assert.AreEqual( currentVersion, entry.CLRVersion );
		}

		[Test]
		public void CanCreateFromFileNameAndVersion()
		{
			RecentFileEntry entry = new RecentFileEntry( entryPath, entryVersion );
			Assert.AreEqual( entryPath, entry.Path );
			Assert.AreEqual( entryVersion, entry.CLRVersion );
		}

        [Test]
        public void EntryCanDisplayItself()
        {
			RecentFileEntry entry = new RecentFileEntry( entryPath, entryVersion );
			Assert.AreEqual(
                entryPath + RecentFileEntry.Separator + entryVersion.ToString(),
                entry.ToString());
        }

        [Test]
        public void CanParseSimpleFileName()
        {
            RecentFileEntry entry = RecentFileEntry.Parse(entryPath);
            Assert.AreEqual(entryPath, entry.Path);
            Assert.AreEqual(currentVersion, entry.CLRVersion);
        }

        [Test]
        public void CanParseFileNamePlusVersionString()
        {
            string text = entryPath + RecentFileEntry.Separator + entryVersion.ToString();
            RecentFileEntry newEntry = RecentFileEntry.Parse(text);
            Assert.AreEqual(entryPath, newEntry.Path);
            Assert.AreEqual(entryVersion, newEntry.CLRVersion);
        }
    }
}
