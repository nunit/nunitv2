using System;
using NUnit.Util;

namespace NUnit.UiKit
{
	/// <summary>
	/// Summary description for GuiTestEventDispatcher.
	/// </summary>
	public class GuiTestEventDispatcher : TestEventDispatcher
	{
		protected override void Fire(TestEventHandler handler, TestEventArgs e)
		{
			if ( handler != null )
				InvokeHandler( handler, e );
		}

		private void InvokeHandler( MulticastDelegate handlerList, EventArgs e )
		{
			object[] args = new object[] { this, e };
			foreach( Delegate handler in handlerList.GetInvocationList() )
			{
				object target = handler.Target;
				System.Windows.Forms.Control control 
					= target as System.Windows.Forms.Control;
				if ( control != null && control.InvokeRequired )
					control.Invoke( handler, args );
				else
					handler.Method.Invoke( target, args );
			}
		}

	}
}

