namespace NUnit.Util.Tests
{
	using System;
	using System.IO;

	class TempResourceFile : IDisposable
	{
		string path;

		public TempResourceFile(Type type, string name) : this(type, name, name) {}

		public TempResourceFile(Type type, string name, string path)
		{
			this.path = path;
			Stream stream = type.Assembly.GetManifestResourceStream(type, name);
			byte[] buffer = new byte[(int)stream.Length];
			stream.Read(buffer, 0, buffer.Length);

			string dir = System.IO.Path.GetDirectoryName(path);
			if(dir != null && dir.Length != 0)
			{
				Directory.CreateDirectory(dir);
			}

			using(FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				fileStream.Write(buffer, 0, buffer.Length);
			}
		}

		public void Dispose()
		{
			string path = this.path;
			while(true)
			{
				path = System.IO.Path.GetDirectoryName(path);
				if(path == null || path.Length == 0 || Directory.GetFiles(path).Length > 0)
				{
					break;
				}

				Directory.Delete(path);
			}

			File.Delete(this.path);
		}

		public string Path
		{
			get
			{
				return this.path;
			}
		}
	}
}
