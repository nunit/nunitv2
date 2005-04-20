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
		public static TcpChannel GetTcpChannel(IDictionary properties)
		{
			BinaryServerFormatterSinkProvider serverProvider =
				new BinaryServerFormatterSinkProvider();

			// NOTE: Use reflection in order to stay compatabe with .NET 1.0.
			// serverProvider.TypeFilterLevel =
			//    System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
			Type typeFilterLevelType = Type.GetType("System.Runtime.Serialization.Formatters.TypeFilterLevel");
			if (typeFilterLevelType != null)
			{
				object typeFilterLevel = Enum.Parse(typeFilterLevelType, "Full");
				PropertyInfo typeFilterLevelProperty = serverProvider.GetType().GetProperty("TypeFilterLevel");
				typeFilterLevelProperty.SetValue(serverProvider, typeFilterLevel, null);
			}

			BinaryClientFormatterSinkProvider clientProvider =
				new BinaryClientFormatterSinkProvider();

			return new TcpChannel(properties, clientProvider, serverProvider);
		}

	}
}
