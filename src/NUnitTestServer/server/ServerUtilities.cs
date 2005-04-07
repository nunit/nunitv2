using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

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
			serverProvider.TypeFilterLevel = 
				System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
			BinaryClientFormatterSinkProvider clientProvider =
				new BinaryClientFormatterSinkProvider();

			return new TcpChannel( properties, clientProvider, serverProvider );
		}
	}
}
