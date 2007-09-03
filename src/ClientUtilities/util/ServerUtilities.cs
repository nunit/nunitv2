// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************
using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Reflection;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for RemotingUtilities.
	/// </summary>
	public class ServerUtilities
	{
		/// <summary>
		///  Create a TcpChannel with a given name, on a given port.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private static TcpChannel CreateTcpChannel( string name, int port )
		{
			ListDictionary props = new ListDictionary();
			props.Add( "port", port );
			props.Add( "name", name );
			props.Add( "bindTo", "127.0.0.1" );

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

			return new TcpChannel( props, clientProvider, serverProvider );
		}

		/// <summary>
		/// Get a channel by name, casting it to a TcpChannel.
		/// Otherwise, create, register and return a TcpChannel with
		/// that name, on the port provided as the second argument.
		/// </summary>
		/// <param name="name">The channel name</param>
		/// <param name="port">The port to use if the channel must be created</param>
		/// <returns>A TcpChannel or null</returns>
		public static TcpChannel GetTcpChannel( string name, int port )
		{
			TcpChannel channel = ChannelServices.GetChannel( name ) as TcpChannel;

			if ( channel == null )
			{
				// NOTE: Retries are normally only needed when rapidly creating
				// and destroying channels, as in running the NUnit tests.
				int retries = 10;
				while( --retries > 0 )
					try
					{
						channel = CreateTcpChannel( name, port );
						ChannelServices.RegisterChannel( channel );
						break;
					}
					catch( Exception )
					{
						System.Threading.Thread.Sleep(300);
					}
			}

			return channel;
		}

		public static void SafeReleaseChannel( IChannel channel )
		{
			if( channel != null )
				try
				{
					ChannelServices.UnregisterChannel( channel );
				}
				catch( RemotingException )
				{
					// Channel was not registered - ignore
				}
		}
	}
}
