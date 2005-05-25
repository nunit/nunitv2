using System;

namespace NUnit.Framework
{
	/// <summary>
	/// AbstractAsserter is the base class for all asserters.
	/// Asserters encapsulate a condition test and generation 
	/// of an AssertionException with a tailored message. They
	/// are used by the Assert class as helper objects.
	/// 
	/// User-defined asserters may be passed to the 
	/// Assert.DoAssert method in order to implement 
	/// extended asserts.
	/// </summary>
	public abstract class AbstractAsserter : IAsserter
	{
		/// <summary>
		/// The user-defined message for this asserter.
		/// </summary>
		protected readonly string userMessage;
		
		/// <summary>
		/// Arguments to use in formatting the user-defined message.
		/// </summary>
		protected readonly object[] args;

		/// <summary>
		/// Our failure message object, initialized as needed
		/// </summary>
		protected AssertionFailureMessage failureMessage;

		/// <summary>
		/// Constructs an AbstractAsserter
		/// </summary>
		/// <param name="message">The message issued upon failure</param>
		/// <param name="args">Arguments to be used in formatting the message</param>
		public AbstractAsserter( string message, params object[] args )
		{
			this.userMessage = message;
			this.args = args;
		}

		/// <summary>
		/// Helper routine to initialize the failure message object.
		/// It returns the message for ease in chaining calls that
		/// add to the message.
		/// </summary>
		/// <returns>A reference to our failure message</returns>
		protected AssertionFailureMessage CreateFailureMessage()
		{
			failureMessage = new AssertionFailureMessage( userMessage, args );
			return failureMessage;
		}

		#region IAsserter Interface
		/// <summary>
		/// Test method to be implemented by derived types.
		/// Default always succeeds.
		/// </summary>
		/// <returns>True if the test succeeds</returns>
		public virtual bool Test()
		{
			return true;
		}

		/// <summary>
		/// Message related to a failure. If no failure has
		/// occured, the result is unspecified.
		/// </summary>
		public virtual string Message
		{
			get
			{
				if ( failureMessage == null )
					CreateFailureMessage();
				return failureMessage.ToString();
			}
		}
		#endregion
	}
}
