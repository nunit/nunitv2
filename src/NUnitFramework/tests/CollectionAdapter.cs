using System;
using System.Collections;

namespace NUnit.Framework.Tests
{
	class CollectionAdapter : ICollection
	{
		private readonly ICollection inner;
		public CollectionAdapter(ICollection inner)
		{
			this.inner = inner;
		}
		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			inner.CopyTo(array, index);
		}

		public int Count
		{
			get { return inner.Count; }
		}

		public bool IsSynchronized
		{
			get { return  inner.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return inner.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return inner.GetEnumerator();
		}

		#endregion
	}
}
