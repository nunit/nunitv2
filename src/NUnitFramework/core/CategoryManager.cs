using System;
using System.Collections;
using System.Reflection;

namespace NUnit.Core
{
	public class CategoryManager
	{
		private static Hashtable categories = new Hashtable();

		public static void Add(string name) 
		{
			categories[name] = name;
		}

		public static void Add(IList list) 
		{
			foreach(string name in list) 
			{
				Add(name);
			}
		}

		public static ICollection Categories 
		{
			get { return categories.Values; }
		}

		public static void Clear() 
		{
			categories = new Hashtable();
		}

		public static IList GetCategories( MemberInfo member )
		{
			System.Attribute[] attributes = 
				Reflect.GetAttributes( member, "NUnit.Framework.CategoryAttribute", false );
			IList categories = new ArrayList();

			foreach( Attribute categoryAttribute in attributes ) 
			{
				string category = (string)Reflect.GetPropertyValue( 
					categoryAttribute, 
					"Name", 
					BindingFlags.Public | BindingFlags.Instance );
				categories.Add( category );
				Add( category );
			}

			return categories;
		}
	}
}
