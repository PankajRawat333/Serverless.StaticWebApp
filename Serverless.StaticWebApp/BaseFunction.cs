using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Serverless.StaticWebApp
{
    public class BaseFunction
    {
        private static string root = "www";

        internal static string GetFilePath(HttpRequestMessage request)
        {
            // parse query parameter
            string queryString = request.GetQueryNameValuePairs().FirstOrDefault(q => string.Compare(q.Key, "file", true) == 0).Value;

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string directory = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));

            //local
            directory = directory.Replace("\\bin\\Debug\\net461\\bin", "");
            //On azure
            //directory = directory.Replace("\\bin", "");
            string file = directory + "\\" + root + "\\Index.html";
            if (request.RequestUri.AbsoluteUri.EndsWith(".html"))
            {
                file = directory + "\\" + root + "\\" + Path.GetFileName(request.RequestUri.AbsoluteUri);
            }
            else if (request.RequestUri.AbsoluteUri.EndsWith(".css") || request.RequestUri.AbsoluteUri.EndsWith(".css"))
            {
                var jsOrCssFile = request.RequestUri.AbsolutePath.Replace("api/WebApp","");
                file = directory + "\\" + jsOrCssFile;
            }

            return file;
        }

        internal static string GetMediaType(string file)
        {
            string fileType = Path.GetExtension(file);
            switch (fileType)
            {
                case ".css":
                    return "text/css";

                case ".js":
                    return "text/javascript";

                default:
                    return "text/HTML";
            }
        }
    }
}