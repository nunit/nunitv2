using System;
using System.Collections;
using System.IO;

namespace NUnit.Framework.Constraints
{
    class DirectoryConstraint
    {
    }

    public class EmptyDirectoryContraint : Constraint
    {
        private int files = 0;
        private int subdirs = 0;

        public override bool Matches(object actual)
        {
            this.actual = actual;

            DirectoryInfo dirInfo = actual as DirectoryInfo;
            if (dirInfo == null)
                throw new ArgumentException("The actual value must be a DirectoryInfo", "actual");

            files = dirInfo.GetFiles().Length;
            subdirs = dirInfo.GetDirectories().Length;

            return files == 0 && subdirs == 0;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write( "An empty directory" );
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            DirectoryInfo dir = actual as DirectoryInfo;
            if (dir == null)
                base.WriteActualValueTo(writer);
            else
            {
                writer.WriteActualValue(dir);
                writer.Write(" with {0} files and {1} directories", files, subdirs);
            }
        }
    }

    public class SubDirectoryConstraint : Constraint
    {
        private DirectoryInfo parentDir;

        public SubDirectoryConstraint( DirectoryInfo dirInfo)
        {
            parentDir = dirInfo;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;

            DirectoryInfo dirInfo = actual as DirectoryInfo;
            if (dirInfo == null)
                throw new ArgumentException("The actual value must be a DirectoryInfo", "actual");

            return IsDirectoryOnPath(parentDir, dirInfo);
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("A subdirectory of");
            writer.WriteExpectedValue(parentDir.FullName);
        }

        /// <summary>
        /// Builds a list of DirectoryInfo objects, recursing where necessary
        /// </summary>
        /// <param name="StartingDirectory">directory to recurse</param>
        /// <returns>list of DirectoryInfo objects from the top level</returns>
        private ArrayList BuildDirectoryList(DirectoryInfo StartingDirectory)
        {
            ArrayList alDirectories = new ArrayList();

            // recurse each directory
            foreach (DirectoryInfo adirectory in StartingDirectory.GetDirectories())
            {
                alDirectories.Add(adirectory);
                alDirectories.AddRange(BuildDirectoryList(adirectory));
            }

            return alDirectories;
        }

        /// <summary>
        /// private method to determine whether a directory is within the path
        /// </summary>
        /// <param name="ParentDirectory">top-level directory to search</param>
        /// <param name="SearchDirectory">directory to search for</param>
        /// <returns>true if found, false if not</returns>
        private bool IsDirectoryOnPath(DirectoryInfo ParentDirectory, DirectoryInfo SearchDirectory)
        {
            if (ParentDirectory == null)
            {
                return false;
            }

            ArrayList listDirectories = BuildDirectoryList(ParentDirectory);

            foreach (DirectoryInfo adirectory in listDirectories)
            {
                if (DirectoriesEqual(adirectory, SearchDirectory))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method to compare two DirectoryInfo objects
        /// </summary>
        /// <param name="expected">first directory to compare</param>
        /// <param name="actual">second directory to compare</param>
        /// <returns>true if equivalent, false if not</returns>
        private bool DirectoriesEqual(DirectoryInfo expected, DirectoryInfo actual)
        {
            return expected.Attributes == actual.Attributes
                && expected.CreationTime == actual.CreationTime
                && expected.FullName == actual.FullName
                && expected.LastAccessTime == actual.LastAccessTime;
        }
    }
}
