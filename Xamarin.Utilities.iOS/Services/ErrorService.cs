using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Xamarin.Utilities.Core.Services
{
    public class ErrorService : IErrorService
    {
        private readonly static string CrashReportFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "crash_report.json");
        private readonly JsonSerializerSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly IEnvironmentalService _environmentService;
        private string _sentryUrl;
        private string _sentryClientId;
        private string _sentrySecret;

        private static bool CrashReportExists
        {
            get { return System.IO.File.Exists(CrashReportFile); }
        }

        public ErrorService(IHttpClientService httpClient, IEnvironmentalService environmentService)
        {
            _environmentService = environmentService;
            _httpClient = httpClient.Create();
            _httpClient.Timeout = new TimeSpan(0, 0, 10);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _settings = new JsonSerializerSettings();
            _settings.ContractResolver = new SnakeCaseContractResolver();
        }

        public event AddExtraInformationDelegate AddExtraInformation;

        public void Init(string sentryUrl, string sentryClientId, string sentrySecret)
        {
            _sentryUrl = sentryUrl;
            _sentryClientId = sentryClientId;
            _sentrySecret = sentrySecret;

            if (CrashReportExists)
                SendPersistedRequest();

            AppDomain.CurrentDomain.UnhandledException += (sender, e) => LogException(e.ExceptionObject as Exception, true);
        }

        public void ReportError(Exception e)
        {
            LogException(e, false);
        }

        private void LogException(Exception exception, bool fatal)
        {
            Debug.WriteLine(exception.Message + " - " + exception.StackTrace);

            if (Debugger.IsAttached)
                Debugger.Break();
            else
            {
                try
                {
                    var request = new SentryRequest(exception);

                    // Add tags for easier sorting
                    request.Tags.Add("version", _environmentService.OSVersion);
                    request.Tags.Add("bundle_version", _environmentService.ApplicationVersion);
                    request.Tags.Add("fatal", fatal.ToString());

                    // Add some extras
                    request.Extra.Add("device_name", _environmentService.DeviceName);

                    var handle = AddExtraInformation;
                    if (handle != null)
                        handle(exception, request.Extra);

                    if (fatal)
                    {
                        PersistRequest(request);
                    }
                    else
                    {
                        // Send it out the door
                        SendRequest(request);
                    }
                }
                catch
                {
                    Debug.WriteLine("Unable to report exception: " + exception.Message);
                }
            }
        }

        private void PersistRequest(object request)
        {
            var crash = JsonConvert.SerializeObject(request, _settings);
            System.IO.File.WriteAllText(CrashReportFile, crash, Encoding.UTF8);
        }

        private void SendPersistedRequest()
        {
            try
            {
                var fileData = System.IO.File.ReadAllText(CrashReportFile, Encoding.UTF8);
                System.IO.File.Delete(CrashReportFile);
                SendRequest(JsonConvert.DeserializeObject<SentryRequest>(fileData));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to deserialize sentry request after crash: " + e.Message);
            }
        }

#if DEBUG
        private void SendRequest(SentryRequest request)
        {
            Console.WriteLine("Would have sent Sentry Dump to {0}:{1}:{2}", _sentryUrl, _sentryClientId, _sentrySecret);
            Console.WriteLine(JsonConvert.SerializeObject(request));
        }
#else
        private void SendRequest(object request)
        {
            var header = String.Format("Sentry sentry_version={0}"
                + ", sentry_client={1}"
                + ", sentry_timestamp={2}"
                + ", sentry_key={3}"
                + ", sentry_secret={4}",
                5,
                "CodeHub/1.0",
                (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
                _sentryClientId,
                _sentrySecret);

            var req = new HttpRequestMessage(HttpMethod.Post, new Uri(_sentryUrl));
            req.Headers.Add("X-Sentry-Auth", header);
            var requestData = JsonConvert.SerializeObject(request, _settings);
            req.Content = new StringContent(requestData, Encoding.UTF8, "application/json");
            _httpClient.SendAsync(req).ContinueWith(t =>
            {
                if (t.Status != System.Threading.Tasks.TaskStatus.RanToCompletion)
                    Console.WriteLine("Unable to send sentry analytic");
            });
        }
#endif

        private class SentryRequest
        {
            public SentryRequest()
            {
            }

            public SentryRequest(Exception exception)
            {
                Platform = "csharp";
                EventId = Guid.NewGuid().ToString("N");
                Timestamp = DateTimeOffset.UtcNow.ToString("o");
                Tags = new Dictionary<string, string>();
                Extra = new Dictionary<string, string>();

                Message = exception.Message;

                if (exception.TargetSite != null)
                {
                    Culprit = String.Format("{0} in {1}", ((exception.TargetSite.ReflectedType == null)
                        ? "<dynamic type>" : exception.TargetSite.ReflectedType.FullName), exception.TargetSite.Name);
                }

                Exception = new List<SentryException>();
                for (Exception currentException = exception;
                    currentException != null;
                    currentException = currentException.InnerException)
                {
                    SentryException sentryException = new SentryException(currentException)
                    {
                        Module = currentException.Source,
                        Type = currentException.GetType().Name,
                        Value = currentException.Message
                    };

                    Exception.Add(sentryException);
                }
            }

            public string EventId { get; set; }
            public string Culprit { get; set; }
            public string Timestamp { get; set; }
            public string Message { get; set; }
            public Dictionary<string, string> Tags { get; set; }
            public List<SentryException> Exception { get; set; }
            public Dictionary<string, string> Extra { get; set; }
            public string Platform { get; set; }

            public class SentryException
            {
                public SentryException()
                {
                }

                public SentryException(Exception exception)
                {
                    if (exception == null)
                        return;

                    Module = exception.Source;
                    Type = exception.GetType().FullName;
                    Value = exception.Message;

                    Stacktrace = new SentryStacktrace(exception);
                    if (Stacktrace.Frames == null || Stacktrace.Frames.Length == 0)
                        Stacktrace = null;
                }

                public string Type { get; set; }
                public string Value { get; set; }
                public string Module { get; set; }
                public SentryStacktrace Stacktrace { get; set; }

                public class SentryStacktrace
                {
                    public SentryStacktrace()
                    {
                    }

                    public SentryStacktrace(Exception exception)
                    {
                        if (exception == null)
                            return;

                        var trace = new StackTrace(exception, true);
                        var frames = trace.GetFrames();

                        if (frames == null)
                            return;

                        int length = frames.Length;
                        Frames = new SentryStackFrames[length];

                        for (int i = 0; i < length; i++)
                        {
                            StackFrame frame = trace.GetFrame(i);
                            Frames[i] = new SentryStackFrames(frame);
                        }
                    }

                    public SentryStackFrames[] Frames { get; set; }

                    public class SentryStackFrames
                    {
                        public SentryStackFrames()
                        {
                        }

                        public SentryStackFrames(StackFrame frame)
                        {
                            if (frame == null)
                                return;

                            int lineNo = frame.GetFileLineNumber();

                            if (lineNo == 0)
                            {
                                //The pdb files aren't currently available
                                lineNo = frame.GetILOffset();
                            }

                            var method = frame.GetMethod();
                            Filename = frame.GetFileName();
                            Module = (method.DeclaringType != null) ? method.DeclaringType.FullName : null;
                            Function = method.Name;
                            ContextLine = method.ToString();
                            Lineno = lineNo;
                            Colno = frame.GetFileColumnNumber();
                        }


                        public string Filename { get; set; }
                        public string Function { get; set; }
                        public int Lineno { get; set; }
                        public string ContextLine { get; set; }
                        public int Colno { get; set; }
                        public string Module { get; set; }
                    }
                }
            }
        }

        private class SnakeCaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return GetSnakeCase(propertyName);
            }

            private string GetSnakeCase(string input)
            {
                if (string.IsNullOrEmpty(input))
                    return input;

                var buffer = "";

                for (var i = 0; i < input.Length; i++)
                {
                    var isLast = (i == input.Length - 1);
                    var isSecondFromLast = (i == input.Length - 2);

                    var curr = input[i];
                    var next = !isLast ? input[i + 1] : '\0';
                    var afterNext = !isSecondFromLast && !isLast ? input[i + 2] : '\0';

                    buffer += char.ToLower(curr);

                    if (!char.IsDigit(curr) && char.IsUpper(next))
                    {
                        if (char.IsUpper(curr))
                        {
                            if (!isLast && !isSecondFromLast && !char.IsUpper(afterNext))
                                buffer += "_";
                        }
                        else
                            buffer += "_";
                    }

                    if (!char.IsDigit(curr) && char.IsDigit(next))
                        buffer += "_";
                    if (char.IsDigit(curr) && !char.IsDigit(next) && !isLast)
                        buffer += "_";
                }

                return buffer;
            }
        }
    }
}

