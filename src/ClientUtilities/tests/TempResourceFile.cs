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
			using(FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				fileStream.Write(buffer, 0, buffer.Length);
			}
		}

		public void Dispose()
		{
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
