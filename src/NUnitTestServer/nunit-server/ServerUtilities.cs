// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Reflection;

namespace NUnit.TestServer
{
	/// <summary>
	/// Summary description for RemotingUtilities.
	/// </summary>
	public class ServerUtilities
	{
		public static TcpChannel GetTcpChannel( IDictionary properties )
		{
			BinaryServerFormatterSinkProvider serverProvider =
				new BinaryServerFormatterSinkProvider();

			// NOTE: The TypeFilterLevel enum/propety doesn't exist in .NET 1.0.
			Type typeFilterLevelType = typeof(object).Assembly.GetType("System.Runtime.Serialization.Formatters.TypeFilterLevel");
			if (typeFilterLevelType != null)
			{
				PropertyInfo typeFilterLevelProperty = serverProvider.GetType().GetProperty("TypeFilterLevel");
				object typeFilterLevel = Enum.Parse(typeFilterLevelType, "Full");
				typeFilterLevelProperty.SetValue(serverProvider, typeFilterLevel, null);
			}

			BinaryClientFormatterSinkProvider clientProvider =
				new BinaryClientFormatterSinkProvider();

			return new TcpChannel( properties, clientProvider, serverProvider );
		}
	}
}
