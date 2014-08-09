using System;
using System.IO;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Android.Services
{
    public class FilesystemService : IFilesystemService
    {
        public string DocumentDirectory
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }
        }

        public Stream CreateTempFile(out string path, string name = null)
        {
            path = name == null ? Path.GetTempFileName() : Path.Combine(Path.GetTempPath(), name);
            return new FileStream(path, FileMode.Create, FileAccess.Write);
        }
    }
}