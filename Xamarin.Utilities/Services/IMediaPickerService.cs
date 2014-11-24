using Splat;
using System.Threading.Tasks;

namespace Xamarin.Utilities.Services
{
    public interface IMediaPickerService
    {
        Task<IBitmap> PickPhoto();
    }
}

