using System.IO;
using Xamarin.Utilities.Core.Services;
using System;

namespace Xamarin.Utilities.Services
{
    public class FilesystemService : IFilesystemService
    {
        public Stream CreateTempFile(out string path, string name = null)
        {
            path = name == null ? Path.GetTempFileName() : Path.Combine(Path.GetTempPath(), name);
            return new FileStream(path, FileMode.Create, FileAccess.Write);
        }

        public string DocumentDirectory
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }
        }
    }
}
