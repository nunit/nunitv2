using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using NUnit.Framework;

namespace NUnit.Tests.Core
{
	/// <summary>
	/// Summary description for CallContextTests.
	/// </summary>
	[TestFixture]
	public class CallContextTests
	{
		const string CONTEXT_DATA = "MyContextData";
		IPrincipal savedPrincipal;

		[SetUp]
		public void SetUp()
		{
			savedPrincipal = System.Threading.Thread.CurrentPrincipal;
		}

		[TearDown]
		public void FreeCallContextDataSlot()
		{
			// These are just workarounds. Currently, tests must free 
			// any context data slots that have been allocated to avoid 
			// serialization problems. If a custom principal is set,
			// the original value must be restored.
			CallContext.FreeNamedDataSlot(CONTEXT_DATA);
			System.Threading.Thread.CurrentPrincipal = savedPrincipal;
		}

		[Test]
		public void ILogicalThreadAffinativeTest()
		{	
			CallContext.SetData( CONTEXT_DATA, new EmptyCallContextData() );
		}

		[Test]
		public void GenericPrincipalTest()
		{
			GenericIdentity ident = new GenericIdentity("Bob");
			GenericPrincipal prpal = new GenericPrincipal(ident, 
				new string[] {"Level1"});

			CallContext.SetData( CONTEXT_DATA, new PrincipalCallContextData( prpal ) );
		}

		[Test]
		public void SetGenericPrincipalOnThread()
		{
			GenericIdentity ident = new GenericIdentity("Bob");
			GenericPrincipal prpal = new GenericPrincipal(ident, 
				new string[] {"Level1"});

			System.Threading.Thread.CurrentPrincipal = prpal;
		}

		[Test]
		public void SetCustomPrincipalOnThread()
		{
			MyPrincipal prpal = new MyPrincipal();

			System.Threading.Thread.CurrentPrincipal = prpal;
		}
	}

	/// <summary>
	/// Helper class that implements ILogicalThreadAffinative
	/// but holds no data at all
	/// </summary>
	[Serializable]
	public class EmptyCallContextData : ILogicalThreadAffinative
	{
	}

	[Serializable]
	public class PrincipalCallContextData : ILogicalThreadAffinative
	{
		IPrincipal principal;

		public PrincipalCallContextData( IPrincipal principal )
		{
			this.principal = principal;
		}

		IPrincipal Principal
		{
			get { return principal; }
		}
	}

	[Serializable]
	public class MyPrincipal : IPrincipal
	{
		public IIdentity Identity
		{
			get
			{
				// TODO:  Add MyPrincipal.Identity getter implementation
				return null;
			}
		}

		public bool IsInRole(string role)
		{
			// TODO:  Add MyPrincipal.IsInRole implementation
			return false;
		}
	}

}
