using System;
using System.Collections.Generic;

namespace Xamarin.Utilities.Core.Services
{
    public delegate void AddExtraInformationDelegate(Exception exception, Dictionary<string, string> extras);
     
    public interface IErrorService
    {
        event AddExtraInformationDelegate AddExtraInformation;

        void Init(string sentryUrl, string sentryClientId, string sentrySecret);

        void ReportError(Exception e);
    }
}
