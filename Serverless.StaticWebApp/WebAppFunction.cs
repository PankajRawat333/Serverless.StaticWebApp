using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Serverless.StaticWebApp
{
    public static class WebAppFunction
    {
        private static string root = "www";

        [FunctionName("WebAppFunction")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WebApp")]HttpRequestMessage req, TraceWriter log)
        {
            // parse query parameter
            string file = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "file", true) == 0)
                .Value;

            var filePath = GetFilePath(file);
            var fileContent = File.ReadAllText(filePath);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(fileContent, Encoding.UTF8, GetMediaType(file))
            };
        }

        private static string GetFilePath(string requestFile)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            var directory = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));

            directory = directory.Replace("\\bin\\Debug\\net461\\bin", "");
            string file = directory + "\\" + root + "\\Index.html";

            if (!string.IsNullOrWhiteSpace(requestFile))
            {
                file = directory + "\\" + root + "\\" + requestFile;
            }
            return file;
        }

        private static string GetMediaType(string file)
        {
            string fileType = Path.GetExtension(file);
            switch (fileType)
            {
                case "css":
                    return "text/css";

                case "js":
                    return "text/javascript";

                default:
                    return "text/HTML";
            }
            return fileType;
        }
    }
}