using System.IO;

namespace Xamarin.Utilities.Core.Services
{
    public interface IFilesystemService
    {
        string DocumentDirectory { get; }

        Stream CreateTempFile(out string path, string name = null);
    }
}
