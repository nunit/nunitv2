using System;

namespace NUnit.Core
{
	/// <summary>
	/// Interface to be implemented by filters applied to tests.
	/// The filter applies when running the test, after it has been
	/// loaded, since this is the only time an ITest exists.
	/// </summary>
	public interface ITestFilter
	{
		/// <summary>
		/// Indicates whether this is the EmptyFilter
		/// </summary>
		bool IsEmpty { get; }

		/// <summary>
		/// Determine if a particular test passes the filter criteria. The default 
		/// implementation simply checks the test itself using Match.
		/// </summary>
		/// <param name="test">The test to which the filter is applied</param>
		/// <returns>True if the test passes the filter, otherwise false</returns>
		bool Pass( ITest test );

		/// <summary>
		/// Determine whether the test itself matches the filter criteria.
		/// </summary>
		/// <param name="test">The test to which the filter is applied</param>
		/// <returns>True if the filter matches the any parent of the test</returns>
		bool Match( ITest test );
	}
}
