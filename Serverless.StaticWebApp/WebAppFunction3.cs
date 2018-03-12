using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Serverless.StaticWebApp
{
    public static class WebAppFunction3
    {
        [FunctionName("WebAppFunction3")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WebApp/{folder}/{folder1}/{folder2}/{folder3}/{file}")]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                var filePath = BaseFunction.GetFilePath(req);
                var fileContent = File.ReadAllText(filePath);

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(fileContent, Encoding.UTF8, BaseFunction.GetMediaType(filePath))
                };
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //Should return error page 500.html
                return null;
            }
        }
    }
}
