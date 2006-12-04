using System;
using System.IO;

namespace NUnit.Framework
{
	#region StreamAsserter
	/// <summary>
	/// Abstract asserter for handling comparison of Streams from which
	/// all other asserters will inherit.
	/// </summary>
	public abstract class StreamAsserter : AbstractAsserter
	{
		/// <summary>
		/// Used to determine the size of buffer which should be used when
		/// reading from streams.
		/// </summary>
		protected static int bufferSize = 4096;

		/// <summary>
		/// The expected Stream
		/// </summary>
		protected Stream expected;
		
		/// <summary>
		/// The actual Stream
		/// </summary>
		protected Stream actual;

		/// <summary>
		/// Construct a StreamAsserter
		/// </summary>
        /// <param name="expected">The expected Stream</param>
        /// <param name="actual">The actual Stream</param>
        /// <param name="message">A message to display on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public StreamAsserter(Stream expected, Stream actual, string message, params object[] args)
			: base( message, args ) 
		{
			this.expected = expected;
			this.actual = actual;
		}

		/// <summary>
		/// Test to confirm that two Stream objects are in fact equal.
		/// </summary>
		/// <returns>Returns true if streams are byte for byte equal.</returns>
		protected bool AreStreamsEqual()
		{
			// If both are null, then they are equal.
			if (expected == null && actual == null) return true;

			// If one is null and both are not (because of the 
			// previous if statement) then they cannot be equal.
			if (expected == null || actual == null) return false;

			if (expected.Length != actual.Length) return false;

			byte[] bufferExpected = new byte[bufferSize];
			byte[] bufferActual = new byte[bufferSize];

			BinaryReader binaryReaderExpected = new BinaryReader(expected);
			BinaryReader binaryReaderActual = new BinaryReader(actual);

			binaryReaderExpected.BaseStream.Seek(0, SeekOrigin.Begin);
			binaryReaderActual.BaseStream.Seek(0, SeekOrigin.Begin);

			for(long readByte = 0; readByte < expected.Length; readByte += bufferSize )
			{
				binaryReaderExpected.Read(bufferExpected, 0, bufferSize);
				binaryReaderActual.Read(bufferActual, 0, bufferSize);

				for (int count=0; count < bufferSize; ++count) 
				{
					if (bufferExpected[count] != bufferActual[count]) 
					{
						FailureMessage.WriteLine("\tIndex : {0}", readByte + count);
						return false;
					}
				}
			}
			return true;
		}
	}
	#endregion

    /// <summary>
    /// Asserter used to verify that two streams are not equal
    /// </summary>
	public class StreamNotEqualAsserter : StreamAsserter
	{
        /// <summary>
        /// Construct a StreamNotEqualAsserter
        /// </summary>
        /// <param name="expected">The expected Stream</param>
        /// <param name="actual">The actual Stream</param>
        /// <param name="message">A message to display on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public StreamNotEqualAsserter(Stream expected, Stream actual, string message, params object[] args)
			: base( expected, actual, message, args ) 
		{
			//FailureMessage.GetStringBuilder().Length = 0;
			FailureMessage.WriteLine("expected and actual are equal.");
			if (expected != null) FailureMessage.WriteExpectedLine("Length : " + expected.Length.ToString());
			if (actual != null) FailureMessage.WriteActualLine("Length : " + actual.Length.ToString());
		}
	
        /// <summary>
        /// Determine whether the assertion is satisfied
        /// </summary>
        /// <returns>True if the streams are unequal, false otherwise</returns>
	    public override bool Test()
		{
			return !AreStreamsEqual();
		}
	}

	/// <summary>
	/// Asserts that two streams are equal at a byte for byte level.
	/// </summary>
	public class StreamEqualAsserter : StreamAsserter
	{
        /// <summary>
        /// Construct a StreamEqualAsserter
        /// </summary>
        /// <param name="expected">The expected Stream</param>
        /// <param name="actual">The actual Stream</param>
        /// <param name="message">A message to display on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public StreamEqualAsserter(Stream expected, Stream actual, string message, params object[] args)
			: base( expected, actual, message, args ) 
		{
			FailureMessage.WriteLine("expected and actual are not equal.");
			if (expected != null) FailureMessage.WriteExpectedLine("Length : " + expected.Length.ToString());
			if (actual != null) FailureMessage.WriteActualLine("Length : " + actual.Length.ToString());
		}

		/// <summary>
		/// Test to confirm that two Stream objects are in fact equal.
		/// </summary>
		/// <returns>Returns true if streams are byte for byte equal.</returns>
		public override bool Test()
		{
			return AreStreamsEqual();
		}

	}
}
