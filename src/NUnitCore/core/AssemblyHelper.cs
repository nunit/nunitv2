// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.Reflection;

namespace NUnit.Core
{
    public class AssemblyHelper
    {
        #region GetAssemblyPath

        public static string GetAssemblyPath(Type type)
        {
            return GetAssemblyPath(type.Assembly);
        }

        public static string GetAssemblyPath(Assembly assembly)
        {
            string codeBase = assembly.EscapedCodeBase;
            
            if (IsFileUri(codeBase))
                return GetAssemblyPathFromEscapedCodeBase(codeBase);
            
            return assembly.Location;
        }

        #endregion

        #region

        // Public for testing purposes
        public static string GetAssemblyPathFromEscapedCodeBase(string escapedCodeBase)
        {
            Uri uri = new Uri(escapedCodeBase);

            if (uri.IsUnc)
                return escapedCodeBase.Substring(Uri.UriSchemeFile.Length + 1);

            return uri.LocalPath;
        }

        #endregion

        #region GetDirectoryName
        public static string GetDirectoryName( Assembly assembly )
        {
            return System.IO.Path.GetDirectoryName(GetAssemblyPath(assembly));
        }
        #endregion

        #region Helper Methods

        private static bool IsFileUri(string uri)
        {
            return uri.ToLower().StartsWith(Uri.UriSchemeFile);
        }

        #endregion
    }
}
