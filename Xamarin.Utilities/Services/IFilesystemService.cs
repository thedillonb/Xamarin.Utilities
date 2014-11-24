using System.IO;
using System.Collections.Generic;

namespace Xamarin.Utilities.Services
{
    public interface IFilesystemService
    {
        string DocumentDirectory { get; }

        Stream CreateTempFile(out string path, string name = null);

        IEnumerable<string> GetFiles(string path);
    }
}
