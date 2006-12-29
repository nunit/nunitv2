using System;

namespace NUnit.Core
{
	/// <summary>
	/// The IService interface is implemented by all Services.
	/// </summary>
	public interface IService
	{
		/// <summary>
		/// Initialize the Service
		/// </summary>
		void InitializeService();

		/// <summary>
		/// Do any cleanup needed before terminating the service
		/// </summary>
		void UnloadService();
	}
}
