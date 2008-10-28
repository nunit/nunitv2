using System;

namespace NUnit.Util
{
	/// <summary>
	/// Abstract base for all types of TestAgents.
    /// A TestAgent provides services of locating,
    /// loading and running tests in a particular
    /// context such as an AppDomain or Process.
	/// </summary>
	public abstract class TestAgent : MarshalByRefObject, IDisposable
	{
		#region Fields
		/// <summary>
		/// Reference to the TestAgency that controls this agent
		/// </summary>
		private TestAgency agency;

		/// <summary>
		/// This agent's assigned id
		/// </summary>
		private Guid agentId;
		#endregion

		#region Constructors
        /// <summary>
        /// Constructs a TestAgent
        /// </summary>
        /// <param name="agentId"></param>
        public TestAgent(Guid agentId)
        {
            this.agentId = agentId;
        }

        /// <summary>
        /// Consructor used by TestAgency when creating
        /// an agent.
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="agency"></param>
		public TestAgent( Guid agentId, TestAgency agency )
		{
			this.agency = agency;
			this.agentId = agentId;
		}
		#endregion

		#region Properties
        /// <summary>
        /// The TestAgency with which this agent is asssociated,
        /// or null if the agent is not tied to an agency.
        /// </summary>
		public TestAgency Agency
		{
			get { return agency; }
		}

        /// <summary>
        /// A Guid that uniquely identifies this agent.
        /// </summary>
		public Guid Id
		{
			get { return agentId; }
		}
		#endregion

		#region Public Methods
        /// <summary>
        /// Starts the agent, performing any required initialization
        /// </summary>
        /// <returns></returns>
        public virtual bool Start()
        {
            return true;
        }

        /// <summary>
        /// Stops the agent, releasing any resources
        /// </summary>
        [System.Runtime.Remoting.Messaging.OneWay]
        public virtual void Stop()
        {
        }

		/// <summary>
		///  Creates a runner using a given runner id
		/// </summary>
        public abstract NUnit.Core.TestRunner CreateRunner(int runnerId);
		#endregion

        #region IDisposable Members
        public void Dispose()
        {
            this.Stop();
        }
        #endregion
    }
}
