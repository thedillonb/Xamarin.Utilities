using System.IO;

namespace Xamarin.Utilities.Core.Services
{
    public interface IFilesystemService
    {
        Stream CreateTempFile(out string path, string name = null);
    }
}
